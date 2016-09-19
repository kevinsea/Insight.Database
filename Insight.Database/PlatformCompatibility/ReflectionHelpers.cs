using System;
using System.Collections.Generic;
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

#if NETCORE
		private const BindingFlags PublicOrPrivateInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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

#endif
	}
}
