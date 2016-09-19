using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Insight.Database.PlatformCompatibility
{

#if NET35 || NET40

/// <summary>
/// Provides Type.GetType() to .Net 3.5 / 4.0
/// </summary>
	internal static class TypeExtensionsLegacy
	{
		internal static Type GetTypeInfo(this Type type)
		{
			return type;
		}

	}
#endif

#if NETCORE
	//#else

	/// <summary>
	/// For .Net 4.5+, ideally use the .Net Extension method that returns a TypeInfo.
	/// These extension of Type keeps us from having to make a bunch up updates to Type.GetTypeInfo()
	/// (this approach does not work for props that were moved, because Extension Props are not a thing
	/// </summary>
	internal static class TypeExtensionsCore
	{
		const BindingFlags PublicOrPrivateInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static bool IsSubclassOf(this Type type, System.Type c)
		{
			return type.GetTypeInfo().IsSubclassOf(c);
		}

		public static ConstructorInfo GetInstanceConstructor(Type t, Type[] desiredConstructorTypes)
		{
			return GetInstanceConstructor(t.GetTypeInfo(), desiredConstructorTypes);
		}

		public static ConstructorInfo GetInstanceConstructor(TypeInfo ti, Type[] desiredConstructorTypes)
		{
			ConstructorInfo matchingContructor;

			// This fails for non-public or maybe for empty/default constructors
			matchingContructor = ti.GetConstructor(desiredConstructorTypes);

			// try with a version that takes binding flags, but not type:
			if (matchingContructor == null)
			{
				var constructors = ti.GetConstructors(PublicOrPrivateInstance).ToList();
				matchingContructor = constructors.Find(c => SignaturesMatch(desiredConstructorTypes, c.GetParameters()));
			}

			if ((matchingContructor != null) && (matchingContructor.IsStatic))
				matchingContructor = null;

			return matchingContructor;
		}

		public static MethodInfo GetInstanceMethod(Type t, string name, Type[] types)
		{

			MethodInfo matchingMethod = t.GetTypeInfo().GetMethod(name, types);

			if (matchingMethod == null)
			{
				List<MethodInfo> methods = t.GetMethods(PublicOrPrivateInstance).ToList();
				methods = methods.FindAll(m => m.Name == name);

				matchingMethod = methods.Find(m => SignaturesMatch(types, m.GetParameters()));
			}

			return matchingMethod;
		}

		private static bool SignaturesMatch(Type[] passedTypes, ParameterInfo[] methodParameters)
		{
			Type[] methodTypes = methodParameters.Select(p => p.ParameterType).ToArray();

			return SignaturesMatch(passedTypes, methodTypes);
		}

		private static bool SignaturesMatch(Type[] passedTypes, Type[] methodTypes)
		{

			//TODO must test this

			if (passedTypes.Count() != methodTypes.Count())
				return false;

			bool isMatchingConstructor = true; // start with true to handle an empty match

			for (int i = 0; i < methodTypes.Count(); i++)
			{
				isMatchingConstructor &= passedTypes[i] == methodTypes[i];
			}

			return isMatchingConstructor;
		}

	}

#endif

}
