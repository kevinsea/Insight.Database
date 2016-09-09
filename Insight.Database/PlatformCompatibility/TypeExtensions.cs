using System;
using System.Reflection;

namespace Insight.Database.PlatformCompatibility
{

#if NET35 || NET40  //TODO add a flag called NET40

	public static class TypeExtensions
	{
		public static Type GetTypeInfo(this Type type)
		{
			return type;
		}
	}

#else
	// Use the existing .Net Extension method that returns a TypeInfo
#endif

}


