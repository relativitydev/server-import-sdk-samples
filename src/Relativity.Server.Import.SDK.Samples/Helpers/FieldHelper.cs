// ----------------------------------------------------------------------------
// <copyright file="FieldHelper.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Relativity.Services.Interfaces.Field;
using Relativity.Services.Interfaces.Field.Models;
using Relativity.Services.Objects;
using Relativity.Services.Objects.DataContracts;

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Defines static helper methods to manage fields.
	/// </summary>
	public static class FieldHelper
	{
		public static void CreateField(FunctionalTestParameters parameters, BaseFieldRequest field)
		{
			parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
			field = field ?? throw new ArgumentNullException(nameof(field));

			try
			{
				using (IFieldManager fieldManager = ServiceHelper.GetServiceProxy<IFieldManager>(parameters))
				{
					switch (field)
					{
						case SingleObjectFieldRequest singleObjectFieldRequest:
							fieldManager.CreateSingleObjectFieldAsync(parameters.WorkspaceId, singleObjectFieldRequest).GetAwaiter().GetResult();
							break;
						case MultipleObjectFieldRequest multipleObjectFieldRequest:
							fieldManager.CreateMultipleObjectFieldAsync(parameters.WorkspaceId, multipleObjectFieldRequest).GetAwaiter().GetResult();
							break;
						case FixedLengthFieldRequest fixedLengthFieldRequest:
							fieldManager.CreateFixedLengthFieldAsync(parameters.WorkspaceId, fixedLengthFieldRequest).GetAwaiter().GetResult();
							break;
						case DateFieldRequest dateFieldRequest:
							fieldManager.CreateDateFieldAsync(parameters.WorkspaceId, dateFieldRequest).GetAwaiter().GetResult();
							break;
						case DecimalFieldRequest decimalFieldRequest:
							fieldManager.CreateDecimalFieldAsync(parameters.WorkspaceId, decimalFieldRequest).GetAwaiter().GetResult();
							break;
						default:
							throw new InvalidOperationException("Unsupported field type");
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public static int QueryIdentifierFieldId(FunctionalTestParameters parameters, string artifactTypeName)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			RelativityObject ro = QueryIdentifierRelativityObject(parameters, artifactTypeName);
			return ro.ArtifactID;
		}

		public static string QueryIdentifierFieldName(FunctionalTestParameters parameters, string artifactTypeName)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			RelativityObject ro = QueryIdentifierRelativityObject(parameters, artifactTypeName);
			return ro.FieldValues[0].Value as string;
		}

		private static RelativityObject QueryIdentifierRelativityObject(FunctionalTestParameters parameters, string artifactTypeName)
		{
			using (IObjectManager client = ServiceHelper.GetServiceProxy<IObjectManager>(parameters))
			{
				QueryRequest queryRequest = new QueryRequest
					                            {
						                            Condition =
							                            $"'Object Type' == '{artifactTypeName}' AND 'Is Identifier' == true",
						                            Fields = new List<FieldRef>
							                                     {
								                                     new FieldRef { Name = "Name" },
							                                     },
						                            ObjectType = new ObjectTypeRef { ArtifactTypeID = WellKnownArtifactTypes.FieldArtifactTypeId },
					                            };
				Relativity.Services.Objects.DataContracts.QueryResult result = client.QueryAsync(
					parameters.WorkspaceId,
					queryRequest,
					1,
					ServiceHelper.MaxItemsToFetch).GetAwaiter().GetResult();
				if (result.TotalCount != 1)
				{
					throw new InvalidOperationException(
						$"Failed to retrieve the identifier Relativity object for the '{artifactTypeName}' artifact type.");
				}

				return result.Objects[0];
			}
		}
	}
}