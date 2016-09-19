using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Insight.Database.PlatformCompatibility
{
	internal class ReflectionHelpers
	{
		internal static ModuleBuilder CreateDynamicModule()
		{
			// make a new assembly for the generated types
#if NETCORE
			AssemblyName an = typeof(ReflectionHelpers).GetTypeInfo().Assembly.GetName();
#else
			AssemblyName an = Assembly.GetExecutingAssembly().GetName();
#endif

			// Make the dynamic assembly have a unique name.  Fixes debugger issue #224.  
			an.Name = an.Name + ".DynamicAssembly";

			AssemblyBuilder ab = ReflectionHelpers.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

			ModuleBuilder builder = ab.DefineDynamicModule(an.Name);
			return builder;
		}

		private static AssemblyBuilder DefineDynamicAssembly(AssemblyName assemblyNameClass, AssemblyBuilderAccess assemblyBuilderAccess)
		{

#if NETCORE // && !COREONDESK
			return AssemblyBuilder.DefineDynamicAssembly(assemblyNameClass, assemblyBuilderAccess);
#else
			return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyNameClass, AssemblyBuilderAccess.Run);
#endif

		}

	}
}
