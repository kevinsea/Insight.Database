using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using Insight.Tests.Cases;
using NUnit.Framework;
using Insight.Database.Providers.Default.PlatformCompatibility;

namespace Insight.Tests
{
	[TestFixture]
	public class NetCoreCompatibilityTests: BaseTest
	{
		[Test]
		public void SqlClientHelpers_ParseProcName()
		{
			SqlObjectName procNameObj;

			// basic
			procNameObj = new SqlObjectName("dbo.mytable");
			Assert.AreEqual(procNameObj.Database, null);
			Assert.AreEqual(procNameObj.Schema, "dbo");
			Assert.AreEqual(procNameObj.Name, "mytable");
			Assert.AreEqual("[dbo].[mytable]",procNameObj.GetFullNameAsString());

			// basic with brackets
			procNameObj = new SqlObjectName("[dbo].[mytable]");
			Assert.AreEqual(procNameObj.Database, null);
			Assert.AreEqual(procNameObj.Schema, "dbo");
			Assert.AreEqual(procNameObj.Name, "mytable");
			Assert.AreEqual("[dbo].[mytable]", procNameObj.GetFullNameAsString());

			// basic with database name
			procNameObj = new SqlObjectName("[mydb].[dbo].[mytable]");
			Assert.AreEqual(procNameObj.Database, "mydb");
			Assert.AreEqual(procNameObj.Schema, "dbo");
			Assert.AreEqual(procNameObj.Name, "mytable");
			Assert.AreEqual("[mydb].[dbo].[mytable]", procNameObj.GetFullNameAsString());

			// basic with database name
			procNameObj = new SqlObjectName("[mydb]..[mytable]");
			Assert.AreEqual(procNameObj.Database, "mydb");
			Assert.AreEqual(procNameObj.Schema, "");
			Assert.AreEqual(procNameObj.Name, "mytable");
			Assert.AreEqual("[mydb]..[mytable]", procNameObj.GetFullNameAsString());
		}

		[Test]
		public void SqlClientHelpers_ParseProcName_Extreme()
		{
			SqlObjectName procNameObj;

			procNameObj = new SqlObjectName("[my.dbo].[my.table]");
			Assert.AreEqual(procNameObj.Database, null);
			Assert.AreEqual(procNameObj.Schema, "my.dbo");
			Assert.AreEqual(procNameObj.Name, "my.table");
			Assert.AreEqual("[my.dbo].[my.table]", procNameObj.GetFullNameAsString());
		}

		[Test]
		public void SqlCommand_DeriveParameters()
		{
			var cnStd = (SqlConnection)Connection();
			var cnCustom = (SqlConnection) Connection();

			var cmdStd = new SqlCommand("InsertIdentityReturn", cnStd);
			var cmdCustom = new SqlCommand("InsertIdentityReturn", cnCustom);

			cmdStd.CommandType = CommandType.StoredProcedure;
			cmdCustom.CommandType = CommandType.StoredProcedure;

			try
			{
				cnStd.Open();
				cnCustom.Open();

				SqlCommandBuilder.DeriveParameters(cmdStd);
				SqlParameterHelper.DeriveParameters(cmdCustom);
			}
			finally
			{
				cnStd.Close();
				cnCustom.Close();
			}

			Assert.AreEqual(cmdStd.Parameters.Count, cmdCustom.Parameters.Count);
		}

		[Test]
		public void SqlCommand_DeriveParameters_ForGeo()
		{
			var cnStd = (SqlConnection)Connection();
			var cnCustom = (SqlConnection)Connection();

			var cmdStd = new SqlCommand("MappingTestProcGeography", cnStd);
			var cmdCustom = new SqlCommand("MappingTestProcGeography", cnCustom);

			cmdStd.CommandType = CommandType.StoredProcedure;
			cmdCustom.CommandType = CommandType.StoredProcedure;


			try
			{
				cnStd.Open();
				cnCustom.Open();

				SqlCommandBuilder.DeriveParameters(cmdStd);
				SqlParameterHelper.DeriveParameters(cmdCustom);
			}
			finally
			{
				cnStd.Close();
				cnCustom.Close();
			}

			Assert.AreEqual(cmdStd.Parameters.Count, cmdCustom.Parameters.Count);
		}

		[Test]
		public void SqlCommand_DeriveParameters_OutputParameter()
		{
			var cnStd = (SqlConnection)Connection();
			var cnCustom = (SqlConnection)Connection();

			var cmdStd = new SqlCommand("InsertWithOutputParameter", cnStd);
			var cmdCustom = new SqlCommand("InsertWithOutputParameter", cnCustom);

			cmdStd.CommandType = CommandType.StoredProcedure;
			cmdCustom.CommandType = CommandType.StoredProcedure;

			try
			{
				cnStd.Open();
				cnCustom.Open();

				SqlCommandBuilder.DeriveParameters(cmdStd);
				SqlParameterHelper.DeriveParameters(cmdCustom);
			}
			finally
			{
				cnStd.Close();
				cnCustom.Close();
			}

			Assert.AreEqual(cmdStd.Parameters.Count, cmdCustom.Parameters.Count);
		}
		
	}
	
}
