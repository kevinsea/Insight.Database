using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Insight.Database.CodeGenerator;


namespace Insight.Database.PlatformCompatibility
{
	class Application
	{

		internal static List<string> GetSearchPaths()
		{
			var paths = new List<string>();

#if NETCORE
			paths.Add(AppContext.BaseDirectory);
#else
			string relativeSearchPath = System.AppDomain.CurrentDomain.RelativeSearchPath ?? String.Empty;
			paths.AddRange(relativeSearchPath.Split(';').Select(p => Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, p)));
#endif
			return paths;
		}

	}
}
