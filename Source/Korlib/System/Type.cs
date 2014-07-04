﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Implementation of the "System.Type" class.
	/// </summary>
	public class Type
	{
		private Type(RuntimeTypeHandle handle)
		{
			this.m_handle = handle;
		}

		private RuntimeTypeHandle m_handle;

		public RuntimeTypeHandle TypeHandle
		{
			get
			{
				return m_handle;
			}
		}

		private string m_fullName;

		public string FullName
		{
			get
			{
				//if (m_fullName == null)
				{
					m_fullName = InternalGetFullName(m_handle.Value);
				}
				return m_fullName;
			}
		}

		private unsafe string InternalGetFullName(IntPtr handle)
		{
			int* namePtr = *(int**)(handle.ToInt32() + 8);
			int length = *namePtr;
			namePtr++;

			return new string((sbyte*)namePtr, 0, length);
		}

		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			return new Type(handle);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern RuntimeTypeHandle GetTypeHandle(object obj);

		public override string ToString()
		{
			return FullName;
		}

		public TypeAttributes Attributes
		{
			get;
			private set;
		}

		public Module Module
		{
			get;
			private set;
		}
	}
}