// ----------------------------------------------------------------------------
// <copyright file="FunctionalTestParameterAttribute.cs" company="Relativity ODA LLC">
//   © Relativity All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System;

namespace Relativity.Server.Import.SDK.Samples.Helpers
{
	/// <summary>
	/// Represents a functional test parameter property-based attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class FunctionalTestParameterAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FunctionalTestParameterAttribute"/> class.
		/// </summary>
		/// <param name="mapped">
		/// <see langword="true" /> if the test parameter is mapped; otherwise, <see langword="false" />.
		/// </param>
		public FunctionalTestParameterAttribute(bool mapped)
			: this(mapped, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FunctionalTestParameterAttribute"/> class.
		/// </summary>
		/// <param name="mapped">
		/// <see langword="true" /> if the test parameter is mapped; otherwise, <see langword="false" />.
		/// </param>
		/// <param name="secret">
		/// <see langword="true" /> if the test parameter represents a secret that should be obfuscated; otherwise, <see langword="false" />.
		/// </param>
		public FunctionalTestParameterAttribute(bool mapped, bool secret)
		{
			this.IsMapped = mapped;
			this.IsSecret = secret;
		}

		/// <summary>
		/// Gets a value indicating whether the associated parameter is mapped to an App.Config setting.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the test parameter is mapped; otherwise, <see langword="false" />.
		/// </value>
		public bool IsMapped
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating whether the associated parameter represents a secret that should be obfuscated.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the test parameter represents a secret; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSecret
		{
			get;
		}
	}
}