/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Text;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Signature type of arrays.
	/// </summary>
	public sealed class ArraySigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ArraySigType"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="rank">The rank.</param>
		/// <param name="sizes">The sizes.</param>
		/// <param name="lowBounds">The low bounds.</param>
		public ArraySigType(SigType type, int rank, int[] sizes, int[] lowBounds)
			: base(CilElementType.Array)
		{
			if (type == null)
				throw new ArgumentNullException(@"type");

			ElementType = type;
			Rank = rank;
			Sizes = sizes;
			LowBounds = lowBounds;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the type of the element.
		/// </summary>
		/// <value>The type of the element.</value>
		public SigType ElementType { get; private set; }

		/// <summary>
		/// Gets the array rank.
		/// </summary>
		/// <value>The rank.</value>
		public int Rank { get; private set; }

		/// <summary>
		/// Gets the sizes of the array ranks.
		/// </summary>
		/// <value>The sizes.</value>
		public int[] Sizes { get; private set; }

		/// <summary>
		/// Gets the lower bounds of the array ranks.
		/// </summary>
		/// <value>The low bounds.</value>
		public int[] LowBounds { get; private set; }

		#endregion Properties

		/// <summary>
		/// Expresses the array type reference in a meaningful, symbol-friendly string form
		/// </summary>
		/// <returns></returns>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(ElementType.ToSymbolPart());
			sb.Append('[');

			// Don't write a comma for rank 1 on purpose...
			for (int x = 1; x < Rank; x++)
			{
				sb.Append(',');
			}
			sb.Append(']');
			return sb.ToString();
		}
	}
}