﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

using Mosa.Platform.x86.Intrinsic;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.DeviceSystem;

namespace Mosa.CoolWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{
		public struct Struct1U1
		{
			public byte One;
		}

		static bool Set1U1(byte one)
		{
			Struct1U1 structure;

			structure.One = one;

			return (structure.One == one);
		}

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();

			Screen.GotoTop();
			Screen.Color = Colors.Yellow;

			Screen.Write(@"MOSA OS CoolWorld Version 1.0");

			Screen.NextLine();
			Screen.NextLine();
			
			Screen.Write("Initializing...");
			Setup.Initialize();
			Screen.NextLine();
			Screen.NextLine();

			Screen.NextLine();
			Screen.NextLine();

			Screen.Write("Starting...");
			Setup.Start();
			Screen.NextLine();
			Screen.NextLine();

			Screen.Write("Done!");
			Screen.NextLine();
			Screen.NextLine();

			while (true)
			{
				Native.Hlt();
			}
		}

	}
}