// ----------------------------------------------------------------------------
// <copyright file="FunctionalTestParameters.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System;

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Represents the parameters used by all functional tests.
	/// </summary>
	public class FunctionalTestParameters
	{
		/// <summary>
		/// The maximum folder length.
		/// </summary>
		public const int MaxFolderLength = 255;

		/// <summary>
		/// Initializes a new instance of the <see cref="FunctionalTestParameters"/> class.
		/// </summary>
		public FunctionalTestParameters()
		{
			this.FileShareUncPath = null;
			this.RelativityPassword = null;
			this.RelativityRestUrl = null;
			this.RelativityUrl = null;
			this.RelativityUserName = null;
			this.RelativityWebApiUrl = null;
			this.ServerCertificateValidation = false;
			this.SkipDirectModeTests = false;
			this.WorkspaceId = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FunctionalTestParameters"/> class.
		/// </summary>
		/// <param name="copy">
		/// The object to copy.
		/// </param>
		public FunctionalTestParameters(FunctionalTestParameters copy)
		{
			if (copy == null)
			{
				throw new ArgumentNullException(nameof(copy));
			}

			this.FileShareUncPath = copy.FileShareUncPath;
			this.RelativityPassword = copy.RelativityPassword;
			this.RelativityRestUrl = new Uri(copy.RelativityRestUrl.ToString());
			this.RelativityUrl = new Uri(copy.RelativityUrl.ToString());
			this.RelativityUserName = copy.RelativityUserName;
			this.RelativityWebApiUrl = new Uri(copy.RelativityWebApiUrl.ToString());
			this.ServerCertificateValidation = copy.ServerCertificateValidation;
			this.SkipDirectModeTests = copy.SkipDirectModeTests;
			this.SqlAdminPassword = copy.SqlAdminPassword;
			this.SqlAdminUserName = copy.SqlAdminUserName;
			this.SqlDropWorkspaceDatabase = copy.SqlDropWorkspaceDatabase;
			this.SqlInstanceName = copy.SqlInstanceName;
			this.WorkspaceId = copy.WorkspaceId;
		}

		/// <summary>
		/// Gets or sets the full UNC path to the file share.
		/// </summary>
		/// <value>
		/// The full path.
		/// </value>
		[FunctionalTestParameter(false)]
		public string FileShareUncPath
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Relativity password used to authenticate.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
#if DEBUG
		[FunctionalTestParameter(true, false)]
#else
		[IntegrationTestParameter(true, true)]
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		public string RelativityPassword
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Relativity REST API URL.
		/// </summary>
		/// <value>
		/// The <see cref="Uri"/> instance.
		/// </value>
		[FunctionalTestParameter(true)]
		public Uri RelativityRestUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Relativity URL.
		/// </summary>
		/// <value>
		/// The <see cref="Uri"/> instance.
		/// </value>
		[FunctionalTestParameter(true)]
		public Uri RelativityUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Relativity user name used to authenticate.
		/// </summary>
		/// <value>
		/// The user name.
		/// </value>
		[FunctionalTestParameter(true)]
		public string RelativityUserName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Relativity WebAPI URL.
		/// </summary>
		/// <value>
		/// The <see cref="Uri"/> instance.
		/// </value>
		[FunctionalTestParameter(true)]
		public Uri RelativityWebApiUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to enforce server certificate validation errors.
		/// </summary>
		/// <value>
		/// <see langword="true" /> to enforce server certificate validation errors; otherwise, <see langword="false" />.
		/// </value>
		[FunctionalTestParameter(true)]
		public bool ServerCertificateValidation
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SQL admin password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
#if DEBUG
		[FunctionalTestParameter(true, false)]
#else
		[IntegrationTestParameter(true, true)]
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		public string SqlAdminPassword
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SQL admin user name.
		/// </summary>
		/// <value>
		/// The user name.
		/// </value>
		[FunctionalTestParameter(true)]
		public string SqlAdminUserName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to drop the test workspace SQL database.
		/// </summary>
		/// <value>
		/// <see langword="true" /> to drop the workspace SQL database; otherwise, <see langword="false" />.
		/// </value>
		[FunctionalTestParameter(true)]
		public bool SqlDropWorkspaceDatabase
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SQL instance name.
		/// </summary>
		/// <value>
		/// The SQL instance name.
		/// </value>
		[FunctionalTestParameter(true)]
		public string SqlInstanceName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to skip tests that specify the <see cref="Relativity.Transfer.WellKnownTransferClient.FileShare"/> transfer client.
		/// </summary>
		/// <value>
		/// <see langword="true" /> to skip the tests; otherwise, <see langword="false" />.
		/// </value>
		[FunctionalTestParameter(true)]
		public bool SkipDirectModeTests
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the test workspace artifact identifier.
		/// </summary>
		/// <value>
		/// The artifact identifier.
		/// </value>
		[FunctionalTestParameter(false)]
		public int WorkspaceId
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the workspace template used when creating a test workspace.
		/// </summary>
		/// <value>
		/// The template name.
		/// </value>
		[FunctionalTestParameter(true)]
		public string WorkspaceTemplate
		{
			get;
			set;
		}

		/// <summary>
		/// Performs a deep copy of this instance.
		/// </summary>
		/// <returns>
		/// The <see cref="FunctionalTestParameters"/> instance.
		/// </returns>
		public FunctionalTestParameters DeepCopy()
		{
			return new FunctionalTestParameters(this);
		}
	}
}