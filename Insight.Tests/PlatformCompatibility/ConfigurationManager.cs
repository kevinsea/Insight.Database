using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Insight.Tests.PlatformCompatibility
{

#if NETCORE
	public static class ConfigurationManager
    {
		static ConfigurationManager()
		{
			// HACK from config
			ConnectionStrings = new Dictionary<string, ConnectionStringSettings>();

			string cnStr = @"Data Source=(localdb)\InsightDb; Initial Catalog = InsightDbTests; Integrated Security = true;";

			var cn = new ConnectionStringSettings(cnStr);

			ConnectionStrings.Add("Test", cn);
		}

		public static Dictionary<string, ConnectionStringSettings> ConnectionStrings { get; }
	}

	public class ConnectionStringSettings
	{
		public ConnectionStringSettings(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public string ConnectionString { get; }
	}
#endif

}
