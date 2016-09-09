using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Insight.Database.CodeGenerator;

namespace Insight.Database.PlatformCompatibility
{
	internal class ReflectionHelpers
	{
		internal static ModuleBuilder CreateDynamicModule()
		{
			// make a new assembly for the generated types
#if !NETCORE
			AssemblyName an = typeof(Reflection).GetTypeInfo().Assembly.GetName();
#else
			AssemblyName an = Assembly.GetExecutingAssembly().GetName();
#endif

			// TODO remove debugger condition for v6
			if (StaticFieldStorage.DebuggerIsAttached())
				// Make the dynamic assembly have a unique name.  Fixes debugger issue #224.  
				an.Name = an.Name + ".DynamicAssembly";

			AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
			Debug.WriteLine("Assemby created*************************");

			ModuleBuilder builder = ab.DefineDynamicModule(an.Name);
			return builder;
		}


		internal static AssemblyBuilder DefineDynamicAssembly(AssemblyName assemblyNameClass, AssemblyBuilderAccess assemblyBuilderAccess)
		{

#if NETCORE
			return AssemblyBuilder.DefineDynamicAssembly(assemblyNameClass, assemblyBuilderAccess);
#else
			return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyNameClass, AssemblyBuilderAccess.Run);
#endif

		}

	}
}
