// -----------------------------------------------------------------------------------------------------
// <copyright file="RandomHelper.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------

using System;

using FizzWare.NBuilder;

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Represents static helper methods to create random test files and data.
	/// </summary>
	public static class RandomHelper
	{
		/// <summary>
		/// The random instance.
		/// </summary>
		private static readonly Random RandomInstance = new Random();

		/// <summary>
		/// The random generator instance.
		/// </summary>
		private static readonly RandomGenerator RandomGeneratorInstance = new RandomGenerator();

		/// <summary>
		/// Gets the next random integer value between <paramref name="minValue"/> and <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="minValue">
		/// The minimum value.
		/// </param>
		/// <param name="maxValue">
		/// The maximum value.
		/// </param>
		/// <returns>
		/// The random integer value.
		/// </returns>
		public static int NextInt(int minValue, int maxValue)
		{
			return RandomInstance.Next(minValue, maxValue);
		}

		/// <summary>
		/// Gets the next random double value between <paramref name="minValue"/> and <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="minValue">
		/// The minimum value.
		/// </param>
		/// <param name="maxValue">
		/// The maximum value.
		/// </param>
		/// <returns>
		/// The random integer value.
		/// </returns>
		public static decimal NextDecimal(int minValue, int maxValue)
		{
			decimal value = NextInt(minValue, maxValue);
			return value;
		}

		/// <summary>
		/// Gets the next random string value between <paramref name="minValue"/> and <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="minValue">
		/// The minimum value.
		/// </param>
		/// <param name="maxValue">
		/// The maximum value.
		/// </param>
		/// <returns>
		/// The random string value.
		/// </returns>
		public static string NextString(int minValue, int maxValue)
		{
			return RandomGeneratorInstance.NextString(minValue, maxValue);
		}
	}
}