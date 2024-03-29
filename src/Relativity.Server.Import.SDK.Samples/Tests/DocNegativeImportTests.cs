﻿// ----------------------------------------------------------------------------
// <copyright file="DocNegativeImportTests.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using NUnit.Framework;

using Relativity.Server.Import.SDK.Samples.Helpers;

namespace Relativity.Server.Import.SDK.Samples.Tests
{
	/// <summary>
	/// Represents tests that fails to import documents and validates the results.
	/// </summary>
	[TestFixture]
	public class DocNegativeImportTests : DocImportTestsBase
	{
		[Test]
		public void ShouldNotImportWhenTheFolderExceedsTheMaxLength()
		{
			// Arrange
			string controlNumber = GenerateControlNumber();
			string folder = "\\" + new string('x', FunctionalTestParameters.MaxFolderLength + 1);
			kCura.Relativity.DataReaderClient.ImportBulkArtifactJob job =
				this.ArrangeImportJob(controlNumber, folder, SampleDocPdfFileName);

			// Act
			job.Execute();

			// Assert - the job failed with a single non-fatal exception.
			this.AssertImportFailed(1);

			// Assert - exceeding the max folder length yields a doc-level error.
			this.AssertError(0, 1, controlNumber, FunctionalTestParameters.MaxFolderLength.ToString());
		}
	}
}