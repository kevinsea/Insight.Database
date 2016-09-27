using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Insight.Database.CodeGenerator;
using System.Runtime;
#if NETCORE && !COREONDESK
using System.Runtime.Loader;
#endif


namespace Insight.Database.PlatformCompatibility
{
	class ApplicationHelpers
	{

		internal static List<string> GetSearchPaths()
		{
			var paths = new List<string>();

#if NETCORE && !COREONDESK
			paths.Add(AppContext.BaseDirectory);
#else
			string relativeSearchPath = System.AppDomain.CurrentDomain.RelativeSearchPath ?? String.Empty;
			paths.AddRange(relativeSearchPath.Split(';').Select(p => Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, p)));
#endif
			return paths;
		}

		internal static Assembly LoadAssemby(string assemblyFile)
		{
#if NETCORE && !COREONDESK
			Assembly assembly = new AssemblyLoader().LoadFromAssemblyPath(assemblyFile);
#else
			Assembly assembly = Assembly.LoadFrom(assemblyFile);
#endif
			return assembly;
		}
		//
	}

#if NETCORE && !COREONDESK

	class AssemblyLoader : AssemblyLoadContext  // TODO CORE review
	{

		protected override Assembly Load(AssemblyName assemblyName)
		{
			return Assembly.Load(assemblyName);
		}

	}
#endif

}
