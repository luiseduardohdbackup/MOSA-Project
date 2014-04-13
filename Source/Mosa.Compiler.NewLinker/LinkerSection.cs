﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.NewLinker
{
	/// <summary>
	///
	/// </summary>
	public class LinkerSection
	{
		private Dictionary<string, LinkerObject> linkerObjects; 
		
		public string Name { get; private set; }
		public SectionKind SectionKind { get; private set; }

		public bool IsResolved { get; private set; }
		public ulong ResolvedSectionOffset { get; private set; }
		public ulong ResolvedVirtualAddress { get; private set; }

		public int Size { get; private set; }

		public LinkerSection(string name, SectionKind sectionKind)
		{
			Name = name;
			SectionKind = sectionKind;
			IsResolved = false;
			linkerObjects = new Dictionary<string, LinkerObject>();
			
			Size = 0;
		}

		public void AddLinkerObject(LinkerObject linkerObject)
		{
			linkerObjects.Add(linkerObject.Name, linkerObject);
		}

		public LinkerObject GetLinkerObject(string name)
		{
			LinkerObject linkerObject = null;

			linkerObjects.TryGetValue(name, out linkerObject);

			return null;
		}

		public void ResolveLayout()
		{
			foreach(var linkerObject in linkerObjects.Values)
			{
				if (linkerObject.IsResolved)
					continue;

				linkerObject.SetSectionOffset(Size);

				Size = Size + linkerObject.Size;
			}
		}
	}
}