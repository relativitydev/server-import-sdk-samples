// ----------------------------------------------------------------------------
// <copyright file="FunctionalTestHelper.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;

using Relativity.Testing.Framework.Configuration;

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Defines static methods to setup and teardown functional tests.
	/// </summary>
	public static class FunctionalTestHelper
	{
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
		public static FunctionalTestParameters Create(IConfigurationService configuration)
		{
			// Note: don't create the logger until all parameters have been retrieved.
			FunctionalTestParameters parameters = new FunctionalTestParameters
				                                      {
					                                      RelativityUserName =
						                                      configuration.RelativityInstance.AdminUsername,
					                                      RelativityPassword =
						                                      configuration.RelativityInstance.AdminPassword,
					                                      RelativityRestUrl =
						                                      CreateUri(
							                                      configuration,
							                                      configuration.RelativityInstance
							                                                   .RestServicesHostAddress,
							                                      "relativity.rest/api"),
					                                      RelativityWebServiceUrl =
						                                      CreateUri(
							                                      configuration,
							                                      configuration.RelativityInstance
							                                                   .RestServicesHostAddress,
							                                      "RelativityWebApi"),
					                                      RelativityUrl = CreateUri(
						                                      configuration,
						                                      configuration.RelativityInstance.RelativityHostAddress),
				                                      };
            SetupLogger(parameters);
			SetupServerCertificateValidation(parameters);
			Console.WriteLine("Creating a test workspace...");
			WorkspaceHelper.CreateTestWorkspace(parameters, Logger);
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
		}

		private static Uri CreateUri(IConfigurationService configuration, string hostAddress, string relative = null)
		{
			System.Uri hosAddressUri = new System.Uri($"{configuration.RelativityInstance.ServerBindingType}://{hostAddress}");
			return string.IsNullOrEmpty(relative) ? hosAddressUri : new System.Uri(hosAddressUri, relative);
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
					                                                 Application =
						                                                 "8A1A6418-29B3-4067-8C9E-51E296F959DE",
					                                                 ConfigurationFileLocation =
						                                                 Path.Combine(
							                                                 ResourceFileHelper.GetBasePath(),
							                                                 "LogConfig.xml"),
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