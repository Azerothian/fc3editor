using System;
using System.Runtime.InteropServices;
using Nomad.Inventory;
namespace Nomad.Logic
{
	public class Wilderness
	{
		public delegate void MapCallback(int line, IntPtr map);
		public delegate void ErrorCallback(int line, string errorMessage);
		public struct FunctionDef
		{
			private IntPtr m_pointer;
			public string Name
			{
				get
				{
					return Marshal.PtrToStringAnsi(Binding.FCE_ScriptFunction_GetName(this.m_pointer));
				}
			}
			public string Prototype
			{
				get
				{
					return Marshal.PtrToStringAnsi(Binding.FCE_ScriptFunction_GetPrototype(this.m_pointer));
				}
			}
			public string Description
			{
				get
				{
					return Marshal.PtrToStringAnsi(Binding.FCE_ScriptFunction_GetDescription(this.m_pointer));
				}
			}
			public FunctionDef(IntPtr ptr)
			{
				this.m_pointer = ptr;
			}
		}
		public static int NumFunctions
		{
			get
			{
				return Binding.FCE_Script_GetNumFunctions();
			}
		}
		public static void GenerateDesert(float gradientWidth, float gradientHeight, float distorsion, float noiseAdd, float blurRadius)
		{
			Binding.FCE_Wilderness_Desert(gradientWidth, gradientHeight, distorsion, noiseAdd, blurRadius);
		}
		public static void RunScript(string scriptName)
		{
			Binding.FCE_Wilderness_Script(scriptName);
		}
		public static void RunScriptBuffer(string buffer, Wilderness.MapCallback mapCallback, Wilderness.ErrorCallback errorCallback)
		{
			Binding.FCE_Wilderness_ScriptBuffer(buffer, buffer.Length, new Binding.ScriptMapCallback(mapCallback.Invoke), new Binding.ScriptErrorCallback(errorCallback.Invoke));
		}
		public static void RunScriptEntry(WildernessInventoryEntry entry)
		{
			Binding.FCE_Wilderness_ScriptEntry(entry.Pointer);
		}
		public static Wilderness.FunctionDef GetFunction(int index)
		{
			return new Wilderness.FunctionDef(Binding.FCE_Script_GetFunction(index));
		}
	}
}
