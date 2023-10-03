// ----------------------------------------------------------------------------
// <copyright file="AssemblySetup.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;

using NUnit.Framework;

using Relativity.Server.Import.SDK.Samples.Helpers;
using Relativity.Testing.Framework.Configuration;

namespace Relativity.Server.Import.SDK.Samples
{
	/// <summary>
	/// Represents a global setup that's guaranteed to be executed before ANY NUnit test and after all have been completed.
	/// </summary>
	[SetUpFixture]
	public class OneTimeSetup
    {
		/// <summary>
		/// Gets the test parameters used by all integration tests within the current assembly.
		/// </summary>
		/// <value>
		/// The <see cref="FunctionalTestParameters"/> instance.
		/// </value>
		public static FunctionalTestParameters TestParameters
		{
			get;
			private set;
		}

		/// <summary>
		/// The main setup method.
		/// </summary>
		[OneTimeSetUp]
		public void Setup()
		{
			IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddNUnitParameters()
			                                                                 .AddEnvironmentVariables()
			                                                                 .Build();
			Relativity.Testing.Framework.RelativityFacade.Instance.RelyOn(
				new Relativity.Testing.Framework.CoreComponent { ConfigurationRoot = configurationRoot });
			Relativity.Testing.Framework.RelativityFacade.Instance
			          .RelyOn<Relativity.Testing.Framework.Api.ApiComponent>();
			TestParameters = FunctionalTestHelper.Create(Relativity.Testing.Framework.RelativityFacade.Instance.Config);
			TestContext.Progress.WriteLine("OneTimeSetUp: completed.");
        }

		/// <summary>
		/// The main teardown method.
		/// </summary>
		[OneTimeTearDown]
		public void TearDown()
		{
			FunctionalTestHelper.Destroy(TestParameters);
		}
	}
}