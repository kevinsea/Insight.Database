using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insight.Tests.PlatformCompatibility
{

#if NETCORE
    public class ApplicationException:Exception
    {
    }
#endif

}
