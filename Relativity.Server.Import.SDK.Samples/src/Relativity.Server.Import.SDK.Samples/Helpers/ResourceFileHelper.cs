// ----------------------------------------------------------------------------
// <copyright file="ResourceFileHelper.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Defines static helper methods to manage resource test files.
	/// </summary>
	public static class ResourceFileHelper
	{
		public static string GetBasePath()
		{
			string basePath = System.IO.Path.GetDirectoryName(typeof(FieldHelper).Assembly.Location);
			return basePath;
		}

		public static string GetDocsResourceFilePath(string fileName)
		{
			return GetResourceFilePath("Docs", fileName);
		}

		public static string GetImagesResourceFilePath(string fileName)
		{
			return GetResourceFilePath("Images", fileName);
		}

		public static string GetResourceFolderDirectory(string folder)
		{
			string basePath = System.IO.Path.GetDirectoryName(typeof(FieldHelper).Assembly.Location);
			string folderPath =
				System.IO.Path.Combine(System.IO.Path.Combine(basePath, "Resources"), folder);
			return folderPath;
		}

		public static string GetResourceFilePath(string folder, string fileName)
		{
			string sourceFile = System.IO.Path.Combine(GetResourceFolderDirectory(folder), fileName);
			return sourceFile;
		}
	}
}