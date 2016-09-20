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
            // This fails for non-public or maybe for empty/default constructors
            var matchingContructor = ti.GetConstructor(desiredConstructorTypes);

            if (matchingContructor != null && !matchingContructor.IsStatic)
                return matchingContructor;

            // try with a version that takes binding flags, but not type:
            return ti.GetConstructors(PublicOrPrivateInstance).FirstOrDefault(c => SignaturesMatch(desiredConstructorTypes, c.GetParameters()));
        }

        public static MethodInfo GetInstanceMethod(Type t, string name, Type[] types)
        {
            return GetInstanceMethod(t.GetTypeInfo(), name, types);
        }

        public static MethodInfo GetInstanceMethod(TypeInfo ti, string name, Type[] types)
        {
            var matchingMethod = ti.GetMethod(name, types);

            if (matchingMethod != null && !matchingMethod.IsStatic)
                return matchingMethod;

            return ti.GetMethods(PublicOrPrivateInstance).FirstOrDefault(m => m.Name == name && SignaturesMatch(types, m.GetParameters()));
        }

        private static bool SignaturesMatch(IEnumerable<Type> passedTypes, IEnumerable<ParameterInfo> methodParameters)
        {
            return methodParameters.Select(p => p.ParameterType).SequenceEqual(passedTypes);
        }

#endif
    }
}
