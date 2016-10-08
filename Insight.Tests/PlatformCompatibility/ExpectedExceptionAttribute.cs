using System;

namespace Insight.Tests.PlatformCompatibility
{

#if NETCORE

	// HACK - Upgrade to NUNIT 3
	internal class ExpectedExceptionAttribute : Attribute
	{
		public string ExpectedMessage { get; set; }

		internal ExpectedExceptionAttribute(object o)
		{
		}

		internal ExpectedExceptionAttribute()
		{
		}

	}
#endif

}
