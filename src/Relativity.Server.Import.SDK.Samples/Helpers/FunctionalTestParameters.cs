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
			this.RelativityPassword = null;
			this.RelativityRestUrl = null;
			this.RelativityUrl = null;
			this.RelativityUserName = null;
			this.RelativityWebServiceUrl = null;
			this.ServerCertificateValidation = false;
            this.WorkspaceId = 0;
			this.WorkspaceTemplate = "Relativity Starter Template";
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

			this.RelativityPassword = copy.RelativityPassword;
			this.RelativityRestUrl = new Uri(copy.RelativityRestUrl.ToString());
			this.RelativityUrl = new Uri(copy.RelativityUrl.ToString());
			this.RelativityUserName = copy.RelativityUserName;
			this.RelativityWebServiceUrl = new Uri(copy.RelativityWebServiceUrl.ToString());
			this.ServerCertificateValidation = copy.ServerCertificateValidation;
			this.WorkspaceId = copy.WorkspaceId;
		}

		/// <summary>
		/// Gets or sets the Relativity password used to authenticate.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
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
		public string RelativityUserName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Relativity web service URL.
		/// </summary>
		/// <value>
		/// The <see cref="Uri"/> instance.
		/// </value>
		public Uri RelativityWebServiceUrl
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
		public bool ServerCertificateValidation
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