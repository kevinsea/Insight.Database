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

	//public static class TypeExtensions
	//{
	//	public static TypeInfo GetTypeInfo(this Type type)
	//	{
	//		return IntrospectionExtensions.GetTypeInfo(type);
	//	}
	//}

#endif

	//#if NETCORE
	//public static class TypeExtensions
	//{
	//	public static Assembly Assembly(this Type type)
	//	{
	//		return type.GetTypeInfo().Assembly;
	//	}

	//	public static bool IsValueType(this Type type)
	//	{
	//		return type.GetTypeInfo().IsValueType;
	//	}

	//	public static bool IsPrimitive(this Type type)
	//	{
	//		return type.GetTypeInfo().IsPrimitive;
	//	}

	//	public static bool IsEnum(this Type type)
	//	{
	//		return type.GetTypeInfo().IsEnum;
	//	}

	//	// new

	//	public static bool IsArray(this Type type)
	//	{
	//		return type.GetTypeInfo().IsArray;
	//	}

	//	public static bool IsByRef(this Type type)
	//	{
	//		return type.GetTypeInfo().IsByRef;
	//	}

	//	public static string Name(this Type type)
	//	{
	//		return type.GetTypeInfo().Name;
	//	}

	//	public static object[] GetCustomAttributes(this Type type, bool inherit)
	//	{
	//		return type.GetTypeInfo().GetCustomAttributes(inherit);
	//	}

	//	public static GenericParameterAttributes GenericParameterAttributes(this Type type)
	//	{
	//		return type.GetTypeInfo().GenericParameterAttributes;
	//	}



	//}

	//#else
	//	public static class TypeExtensions
	//	{
	//		public static Assembly Assembly(this Type type)
	//		{
	//			return type.Assembly;
	//		}

	//		public static bool IsValueType(this Type type)
	//		{
	//			return type.IsValueType;
	//		}

	//		public static bool IsPrimitive(this Type type)
	//		{
	//			return type.IsPrimitive;
	//		}

	//		public static bool IsEnum(this Type type)
	//		{
	//			return type.IsEnum;
	//		}

	//	}

	//#endif

	//public static class TypeInfo<t:Type>
	//{
	//	private Type _type;

	//	internal TypeInfo(Type type)
	//	{
	//		_type = type;
	//	}

	//	public Assembly Assembly => _type.Assembly;

	//	public static bool IsValueType(this Type type)
	//{
	//	return type.GetTypeInfo().IsValueType;
	//}

	//public static bool IsPrimitive(this Type type)
	//{
	//	return type.GetTypeInfo().IsPrimitive;
	//}

	//public static bool IsEnum(this Type type)
	//{
	//	return type.GetTypeInfo().IsEnum;
	//}
	//}
}


