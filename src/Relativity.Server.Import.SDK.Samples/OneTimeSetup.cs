// ----------------------------------------------------------------------------
// <copyright file="AssemblySetup.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using global::NUnit.Framework;

using Relativity.Server.Import.SDK.Samples.Helpers;

namespace Relativity.Server.Import.SDK.Samples
{
	/// <summary>
	/// Represents a global assembly-wide setup routine that's guaranteed to be executed before ANY NUnit test.
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
			TestParameters = FunctionalTestHelper.Create();
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