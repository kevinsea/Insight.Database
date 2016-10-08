using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using NUnit.Framework;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
#if !NET35 && !NETCORE
using Insight.Database.Schema;
#endif

#if NETCORE  //NETCORE does not have config files
using Insight.Tests.PlatformCompatibility;
#else
using System.Configuration;
#endif


namespace Insight.Tests
{
	public class BaseTest
	{
		/// <summary>
		/// The connection string for our database.
		/// </summary>
		public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

		public IDbConnection Connection()
		{
			return new SqlConnection(ConnectionString);
		}

		public IDbConnection ConnectionWithTransaction()
		{
			return new SqlConnection(ConnectionString).OpenWithTransaction();
		}
	}


	[SetUpFixture]
	public class TestSetup
	{
#if NETCORE
		[OneTimeSetUp]
#else
		[SetUp]
#endif
		public static void SetUpFixture()
		{
#if !NET35 && !NETCORE
			// insight.schema requires 4.0, so let's assume that in 35, the setup is already done
			// let's do all of our work in the test database
			if (!SchemaInstaller.DatabaseExists(BaseTest.ConnectionString))
				SchemaInstaller.CreateDatabase(BaseTest.ConnectionString);

			var schema = new SchemaObjectCollection(Assembly.GetExecutingAssembly());
			using (var connection = new SqlConnection(BaseTest.ConnectionString))
			{
				connection.Open();
				var installer = new SchemaInstaller(connection);
				installer.Install("Test", schema);
			}
#endif
		}


	}
}
