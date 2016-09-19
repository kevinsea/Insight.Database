using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility
{

#if NETCORE
	class DelegateHelpers
	{
		/// <summary>
		/// A replacment for DelegateHelpers.CreateDelegate.  Method using this could be easily be re-written but this maintains 
		/// comparabitly of old a new versions
		/// </summary>
		/// <param name="type"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static Delegate CreateDelegate(Type type, System.Reflection.MethodInfo method)
		{
			return method.CreateDelegate(type);
		}
	}

#endif

}
