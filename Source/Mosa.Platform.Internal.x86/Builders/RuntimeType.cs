﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.Internal.x86;
using System.Reflection;
using x86Runtime = Mosa.Platform.Internal.x86.Runtime;

namespace System
{
	public sealed unsafe class RuntimeType : Type
	{
		private MetadataTypeStruct* typeStruct;
		private string assemblyQualifiedName;
		private string name;
		private string @namespace;
		private string fullname;
		private RuntimeTypeHandle handle;
		private TypeCode typeCode;
		internal TypeAttributes attributes; // FIXME: this should be private, only temporarily internal
		private Type declaringType;
		private Type elementType;

		public override string AssemblyQualifiedName
		{
			get { return this.assemblyQualifiedName; }
		}

		public override Type DeclaringType
		{
			get { return this.declaringType; }
		}

		public override string FullName
		{
			get { return this.fullname; }
		}

		public override int GenericParameterPosition
		{
			get { throw new NotSupportedException(); }
		}

		public override Type[] GenericTypeArguments
		{
			get { return new Type[0]; }
		}

		public override bool IsConstructedGenericType
		{
			// We don't know so just return false
			get { return false; }
		}

		public override bool IsGenericParameter
		{
			// We don't know so just return false
			get { return false; }
		}

		public override string Name
		{
			get { return this.name; }
		}

		public override string Namespace
		{
			get { return this.@namespace; }
		}

		public override RuntimeTypeHandle TypeHandle
		{
			get { return this.handle; }
		}

		internal RuntimeType(RuntimeTypeHandle handle)
		{
			this.handle = handle;
			this.typeStruct = (MetadataTypeStruct*)((uint**)&handle)[0];

			this.assemblyQualifiedName = x86Runtime.InitializeMetadataString(this.typeStruct->Name);	// TODO
			this.name = x86Runtime.InitializeMetadataString(this.typeStruct->Name);					// TODO
			this.@namespace = x86Runtime.InitializeMetadataString(this.typeStruct->Name);				// TODO
			this.fullname = x86Runtime.InitializeMetadataString(this.typeStruct->Name);

			this.typeCode = (TypeCode)(this.typeStruct->Attributes >> 24);
			this.attributes = (TypeAttributes)(this.typeStruct->Attributes & 0x00FFFFFF);
		}

		internal void FindRelativeTypes()
		{
			if (this.typeStruct->DeclaringType != null)
			{
				RuntimeTypeHandle declaringHandle = new RuntimeTypeHandle();
				((uint**)&declaringHandle)[0] = (uint*)this.typeStruct->DeclaringType;
				this.declaringType = Type.GetTypeFromHandle(declaringHandle);
			}

			if ((*this.typeStruct).ElementType != null)
			{
				RuntimeTypeHandle elementHandle = new RuntimeTypeHandle();
				((uint**)&elementHandle)[0] = (uint*)this.typeStruct->ElementType;
				this.elementType = Type.GetTypeFromHandle(elementHandle);
			}
		}

		public override int GetArrayRank()
		{
			// We don't know so just return 1 if array, 0 otherwise
			return (this.IsArrayImpl() == true) ? 1 : 0;
		}

		public override Type GetElementType()
		{
			return this.elementType;
		}

		public override Type GetGenericTypeDefinition()
		{
			// No planned support
			throw new NotSupportedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return (this.elementType != null);
		}

		protected override bool IsArrayImpl()
		{
			return this.typeCode == TypeCode.Array || this.typeCode == TypeCode.SZArray;
		}

		protected override bool IsByRefImpl()
		{
			// We don't know so just return false
			return false;
		}

		protected override bool IsNestedImpl()
		{
			return (this.attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public;
		}

		protected override bool IsPointerImpl()
		{
			return this.typeCode == TypeCode.ManagedPointer || this.typeCode == TypeCode.UnmanagedPointer;
		}

		public override Type MakeArrayType()
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakeArrayType(int rank)
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakeByRefType()
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakeGenericType(params Type[] typeArguments)
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakePointerType()
		{
			// No planned support
			throw new NotSupportedException();
		}
	}
}