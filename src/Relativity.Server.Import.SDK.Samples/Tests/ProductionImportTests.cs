﻿// ----------------------------------------------------------------------------
// <copyright file="ProductionImportTests.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using NUnit.Framework;

using Relativity.Server.Import.SDK.Samples.Helpers;

namespace Relativity.Server.Import.SDK.Samples.Tests
{
	/// <summary>
	/// Represents a test that imports production images and validates the results.
	/// </summary>
	/// <remarks>
	/// This test requires the Relativity.Productions.Client package but hasn't yet been published to nuget.org.
	/// </remarks>
	[TestFixture]
	public class ProductionImportTests : ImageImportTestsBase
	{
		/// <summary>
		/// The first document control number.
		/// </summary>
		private const string FIRST_DOCUMENT_CONTROL_NUMBER = "EDRM-Sample-000001";

		/// <summary>
		/// The second document control number.
		/// </summary>
		private const string SECOND_DOCUMENT_CONTROL_NUMBER = "EDRM-Sample-001002";

		/// <summary>
		/// The total number of images for the first imported document.
		/// </summary>
		/// <remarks>
		/// The last digits in <see cref="SECOND_DOCUMENT_CONTROL_NUMBER"/> must be updated if this value is changed.
		/// </remarks>
		private const int TOTAL_IMAGES_FOR_FIRST_DOCUMENT = 1001;

		[Test]
		public void ShouldImportTheProductionImages()
		{
			// Arrange
			IList<string> controlNumbers = new List<string> { FIRST_DOCUMENT_CONTROL_NUMBER, SECOND_DOCUMENT_CONTROL_NUMBER };
			this.ImportDocuments(controlNumbers);
			string productionSetName = GenerateProductionSetName();
			int productionId = this.CreateProduction(productionSetName, BatesPrefix);

			// Act
			this.ImportProduction(productionId);

			// Assert - the job completed and the report matches the expected values.
			Assert.That(this.PublishedJobReport, Is.Not.Null);
			Assert.That(this.PublishedJobReport.EndTime, Is.GreaterThan(this.PublishedJobReport.StartTime));
			Assert.That(this.PublishedJobReport.ErrorRowCount, Is.Zero);

			Assert.That(this.PublishedJobReport.StartTime, Is.GreaterThan(this.StartTime));
			Assert.That(this.PublishedJobReport.TotalRows, Is.EqualTo(this.DataSource.Rows.Count));

			// Assert - the events match the expected values.
			Assert.That(this.PublishedErrors.Count, Is.Zero);
			Assert.That(this.PublishedFatalException, Is.Null);
			Assert.That(this.PublishedMessages.Count, Is.Positive);
			Assert.That(this.PublishedProcessProgress.Count, Is.Positive);
			Assert.That(this.PublishedProgressRows.Count, Is.Positive);

			// Assert - the first and last bates numbers match the expected values.
			Tuple<string, string> batesNumbers = this.QueryProductionBatesNumbers(productionId);
			string expectedFirstBatesValue = FIRST_DOCUMENT_CONTROL_NUMBER;
			string expectedLastBatesValue = SECOND_DOCUMENT_CONTROL_NUMBER;
			Assert.That(batesNumbers.Item1, Is.EqualTo(expectedFirstBatesValue));
			Assert.That(batesNumbers.Item2, Is.EqualTo(expectedLastBatesValue));

			// Assert the field values match the expected values.
			IList<Relativity.Services.Objects.DataContracts.RelativityObject> documents = this.QueryDocuments();
			Assert.That(documents, Is.Not.Null);
			Relativity.Services.Objects.DataContracts.RelativityObject firstDocument = SearchRelativityObject(
				documents,
				WellKnownFields.ControlNumber,
				FIRST_DOCUMENT_CONTROL_NUMBER);
			Assert.That(firstDocument, Is.Not.Null);
			Relativity.Services.Objects.DataContracts.RelativityObject secondDocument = SearchRelativityObject(
				documents,
				WellKnownFields.ControlNumber,
				SECOND_DOCUMENT_CONTROL_NUMBER);
			Assert.That(secondDocument, Is.Not.Null);
			foreach (var document in new[] { firstDocument, secondDocument })
			{
				Relativity.Services.Objects.DataContracts.Choice hasImagesField = GetChoiceField(
					document,
					WellKnownFields.HasImages);
				Assert.That(hasImagesField, Is.Not.Null);
				Assert.That(hasImagesField.Name, Is.Not.Null);
				Assert.That(hasImagesField.Name, Is.EqualTo("No"));
				bool hasNativeField = GetBooleanFieldValue(document, WellKnownFields.HasNative);
				Assert.That(hasNativeField, Is.False);
				int? relativityImageCount = GetInt32FieldValue(document, WellKnownFields.RelativityImageCount);
				Assert.That(relativityImageCount, Is.Null);
			}

			// Assert that importing doesn't add a file record.
			IList<FileDto> firstDocumentImages = this.QueryImageFileInfo(firstDocument.ArtifactID).ToList();
			Assert.That(firstDocumentImages.Count, Is.Zero);
			IList<FileDto> secondDocumentImages = this.QueryImageFileInfo(secondDocument.ArtifactID).ToList();
			Assert.That(secondDocumentImages.Count, Is.Zero);
		}

		private void ImportProduction(int productionId)
		{
			kCura.Relativity.ImportAPI.ImportAPI importApi = this.CreateImportApiObject();
			IEnumerable<kCura.Relativity.ImportAPI.Data.ProductionSet> productionSets =
				importApi.GetProductionSets(this.TestParameters.WorkspaceId).ToList();
			Assert.That(productionSets.Count, Is.GreaterThan(0));
			kCura.Relativity.ImportAPI.Data.ProductionSet productionSet =
				productionSets.FirstOrDefault(x => x.ArtifactID == productionId);
			Assert.That(productionSet, Is.Not.Null);
			kCura.Relativity.DataReaderClient.ImageImportBulkArtifactJob job =
				importApi.NewProductionImportJob(productionSet.ArtifactID);
			this.ConfigureJobSettings(job);
			job.Settings.NativeFileCopyMode = kCura.Relativity.DataReaderClient.NativeFileCopyModeEnum.DoNotImportNativeFiles;
			this.ConfigureJobEvents(job);
			this.DataSource.Columns.AddRange(new[]
			{
				new DataColumn(this.IdentifierFieldName, typeof(string)),
				new DataColumn(WellKnownFields.BatesNumber, typeof(string)),
				new DataColumn(WellKnownFields.FileLocation, typeof(string)),
			});

			DataRow row;
			for (int i = 1; i <= TOTAL_IMAGES_FOR_FIRST_DOCUMENT; i++)
			{
				row = this.DataSource.NewRow();
				row[this.IdentifierFieldName] = FIRST_DOCUMENT_CONTROL_NUMBER;
				row[WellKnownFields.BatesNumber] = $"EDRM-Sample-{i:D6}";
				row[WellKnownFields.FileLocation] = ResourceFileHelper.GetImagesResourceFilePath(SampleProductionImage1FileName);
				this.DataSource.Rows.Add(row);
			}

			row = this.DataSource.NewRow();
			row[this.IdentifierFieldName] = SECOND_DOCUMENT_CONTROL_NUMBER;
			row[WellKnownFields.BatesNumber] = SECOND_DOCUMENT_CONTROL_NUMBER;
			row[WellKnownFields.FileLocation] = ResourceFileHelper.GetImagesResourceFilePath(SampleProductionImage1FileName);
			this.DataSource.Rows.Add(row);
			job.SourceData.SourceData = this.DataSource;
			job.Execute();
		}
	}
}