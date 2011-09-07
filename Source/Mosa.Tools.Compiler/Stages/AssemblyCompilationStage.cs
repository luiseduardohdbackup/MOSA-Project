﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.IO;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Pdb;
using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.TypeInitializers;

namespace Mosa.Tools.Compiler.Stages
{
	public class AssemblyCompilationStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{

		private IAssemblyLinker linker;

		private ITypeInitializerSchedulerStage typeInitializerSchedulerStage;

		public AssemblyCompilationStage()
		{
		}

		#region IAssemblyCompilerStage

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			typeInitializerSchedulerStage = compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();

			if (typeInitializerSchedulerStage == null)
				throw new InvalidOperationException(@"AssemblyCompilationStage needs a ITypeInitializerSchedulerStage.");

			this.linker = RetrieveAssemblyLinkerFromCompiler();
		}

		void IAssemblyCompilerStage.Run()
		{
			CompileAssembly();
		}

		#endregion IAssemblyCompilerStage

		private static void LoadAssemblyDebugInfo(string assemblyFileName)
		{
			string dbgFile = Path.Combine(Path.GetDirectoryName(assemblyFileName), Path.GetFileNameWithoutExtension(assemblyFileName) + ".pdb");

			if (File.Exists(dbgFile))
			{
				using (FileStream fileStream = new FileStream(dbgFile, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (PdbReader reader = new PdbReader(fileStream))
					{
						Debug.WriteLine(@"Global symbols:");
						foreach (CvSymbol symbol in reader.GlobalSymbols)
						{
							Debug.WriteLine("\t" + symbol.ToString());
						}

						Debug.WriteLine(@"Types:");
						foreach (PdbType type in reader.Types)
						{
							Debug.WriteLine("\t" + type.Name);
							Debug.WriteLine("\t\tSymbols:");
							foreach (CvSymbol symbol in type.Symbols)
							{
								Debug.WriteLine("\t\t\t" + symbol.ToString());
							}

							Debug.WriteLine("\t\tLines:");
							foreach (CvLine line in type.LineNumbers)
							{
								Debug.WriteLine("\t\t\t" + line.ToString());
							}
						}
					}
				}
			}
		}

		private void CompileAssembly()
		{
			using (AotAssemblyCompiler assemblyCompiler = new AotAssemblyCompiler(architecture, typeInitializerSchedulerStage, linker, typeSystem, typeLayout, compiler.InternalLog, compiler.CompilerOptionSet))
			{
				assemblyCompiler.Run();
			}
		}
	}
}