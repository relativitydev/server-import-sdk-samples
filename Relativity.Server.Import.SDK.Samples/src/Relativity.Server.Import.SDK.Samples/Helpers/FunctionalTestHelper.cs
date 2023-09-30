// ----------------------------------------------------------------------------
// <copyright file="FunctionalTestHelper.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Defines static methods to setup and teardown functional tests.
	/// </summary>
	public static class FunctionalTestHelper
	{
		/// <summary>
		/// Gets or sets a value indicating whether environment variables are enabled.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when environment variables are enabled; otherwise, <see langword="false" />.
		/// </value>
		public static bool EnvironmentVariablesEnabled { get; set; } = true;

		/// <summary>
		/// Gets the Relativity logging instance.
		/// </summary>
		/// <value>
		/// The <see cref="Relativity.Logging.ILog"/> instance.
		/// </value>
		public static Relativity.Logging.ILog Logger
		{
			get;
			private set;
		}

		/// <summary>
		/// Create the integration test environment with a new test workspace and returns the test parameters.
		/// </summary>
		/// <returns>
		/// The <see cref="FunctionalTestParameters"/> instance.
		/// </returns>
		public static FunctionalTestParameters Create()
		{
			// Note: don't create the logger until all parameters have been retrieved.
			FunctionalTestParameters parameters = ReadIntegrationTestParameters();
			SetupLogger(parameters);
			SetupServerCertificateValidation(parameters);
			Console.WriteLine("Creating a test workspace...");
			WorkspaceHelper.CreateTestWorkspace(parameters, Logger);
			kCura.Relativity.ImportAPI.ImportAPI iapi = new kCura.Relativity.ImportAPI.ImportAPI(
				parameters.RelativityUserName,
				parameters.RelativityPassword,
				parameters.RelativityWebApiUrl.ToString());
			IEnumerable<kCura.Relativity.ImportAPI.Data.Workspace> workspaces = iapi.Workspaces();
			kCura.Relativity.ImportAPI.Data.Workspace workspace =
				workspaces.FirstOrDefault(x => x.ArtifactID == parameters.WorkspaceId);
			if (workspace == null)
			{
				throw new InvalidOperationException(
					$"This operation cannot be performed because the workspace {parameters.WorkspaceId} that was just created doesn't exist.");
			}

			parameters.FileShareUncPath = workspace.DocumentPath;
			Console.WriteLine($"Created {parameters.WorkspaceId} test workspace.");
			return parameters;
		}

		/// <summary>
		/// Destroy the integration test environment that was previously created.
		/// </summary>
		/// <param name="parameters">
		/// The integration test parameters.
		/// </param>
		public static void Destroy(FunctionalTestParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			WorkspaceHelper.DeleteTestWorkspace(parameters, Logger);
			string database = $"EDDS{parameters.WorkspaceId}";
			if (parameters.SqlDropWorkspaceDatabase && parameters.WorkspaceId > 0)
			{
				try
				{
					SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
					{
						DataSource = parameters.SqlInstanceName,
						IntegratedSecurity = false,
						UserID = parameters.SqlAdminUserName,
						Password = parameters.SqlAdminPassword,
						InitialCatalog = string.Empty,
					};

					SqlConnection.ClearAllPools();
					using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
					{
						connection.Open();
						using (SqlCommand command = connection.CreateCommand())
						{
							command.CommandText = $@"
IF EXISTS(SELECT name FROM sys.databases WHERE name = '{database}')
BEGIN
	ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [{database}]
END";
							command.CommandType = CommandType.Text;
							command.ExecuteNonQuery();
							Logger.LogInformation("Successfully dropped the {DatabaseName} SQL workspace database.", database);
							Console.WriteLine($"Successfully dropped the {database} SQL workspace database.");
						}
					}
				}
				catch (Exception e)
				{
					Logger.LogError(e, "Failed to drop the {DatabaseName} SQL workspace database.", database);
					Console.WriteLine($"Failed to drop the {database} SQL workspace database. Exception: " + e);
				}
			}
			else
			{
				Logger.LogInformation("Skipped dropping the {DatabaseName} SQL workspace database.", database);
			}
		}

		private static FunctionalTestParameters ReadIntegrationTestParameters()
		{
			FunctionalTestParameters parameters = new FunctionalTestParameters();
			foreach (var prop in parameters.GetType().GetProperties())
			{
				FunctionalTestParameterAttribute attribute =
					prop.GetCustomAttribute<FunctionalTestParameterAttribute>();
				if (attribute == null || !attribute.IsMapped)
				{
					continue;
				}

				string value = GetConfigurationStringValue(prop.Name);
				if (prop.PropertyType == typeof(string))
				{
					prop.SetValue(parameters, value);
				}
				else if (prop.PropertyType == typeof(bool))
				{
					prop.SetValue(parameters, bool.Parse(value));
				}
				else if (prop.PropertyType == typeof(Uri))
				{
					prop.SetValue(parameters, new Uri(value));
				}
				else
				{
					string message =
						$"The integration test parameter '{prop.Name}' of type '{prop.PropertyType}' isn't supported by the integration test helper.";
					throw new ConfigurationErrorsException(message);
				}
			}

			return parameters;
		}

		private static string GetConfigurationStringValue(string key)
		{
			string value = System.Configuration.ConfigurationManager.AppSettings.Get(key);
			if (!string.IsNullOrEmpty(value))
			{
				return value;
			}

			throw new InvalidOperationException($"The '{key}' app.config setting value is not specified.");
		}

		private static void SetupServerCertificateValidation(FunctionalTestParameters parameters)
		{
			if (!parameters.ServerCertificateValidation)
			{
				ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;
			}
		}

		private static void SetupLogger(FunctionalTestParameters parameters)
		{
			Relativity.Logging.LoggerOptions loggerOptions = new Relativity.Logging.LoggerOptions
			{
				Application = "8A1A6418-29B3-4067-8C9E-51E296F959DE",
				ConfigurationFileLocation = Path.Combine(ResourceFileHelper.GetBasePath(), "LogConfig.xml"),
				System = "Import-API",
				SubSystem = "Samples",
			};

			// Configure the optional SEQ sink to periodically send logs to the local SEQ server for improved debugging.
			// See https://getseq.net/ for more details.
			loggerOptions.AddSinkParameter(
				Relativity.Logging.Configuration.SeqSinkConfig.ServerUrlSinkParameterKey,
				new Uri("http://localhost:5341"));

			// Configure the optional HTTP sink to periodically send logs to Relativity.
			loggerOptions.AddSinkParameter(
				Relativity.Logging.Configuration.RelativityHttpSinkConfig.CredentialSinkParameterKey,
				new NetworkCredential(parameters.RelativityUserName, parameters.RelativityPassword));
			loggerOptions.AddSinkParameter(
				Relativity.Logging.Configuration.RelativityHttpSinkConfig.InstanceUrlSinkParameterKey,
				parameters.RelativityUrl);
			Logger = Relativity.Logging.Factory.LogFactory.GetLogger(loggerOptions);

			// Until Import API supports passing a logger instance via constructor, the API
			// internally uses the Logger singleton instance if defined.
			Relativity.Logging.Log.Logger = Logger;
		}
	}
}