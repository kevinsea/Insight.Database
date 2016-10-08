using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database.PlatformCompatibility.DataReader;

namespace Insight.Database.PlatformCompatibility.DataReader
{

#if NETCORE && !COREONDESK

	internal class DbColumnSchemaProvider : ColumnSchemaProviderBase
	{
		public DbColumnSchemaProvider(IEnumerable<DbColumn> argDbColumns)
			: base(GenerateColumnSchemas(argDbColumns))
		{
		}

		private static IColumnSchema[] GenerateColumnSchemas(IEnumerable<DbColumn> argDbColumns)
		{
			return argDbColumns.Select(dbColumn => (IColumnSchema)new ColumnSchema(dbColumn)).ToArray();
		}

		private class ColumnSchema : IColumnSchema
		{
			internal ColumnSchema(DbColumn dbColumn)
			{
				ColumnName = dbColumn.ColumnName;
				DataType = dbColumn.DataType;
				DataTypeName = dbColumn.DataTypeName;
				IsIdentity = dbColumn.IsIdentity.GetValueOrDefault(false);
				IsReadOnly = dbColumn.IsReadOnly.GetValueOrDefault(false);
				IsNullable = dbColumn.AllowDBNull.GetValueOrDefault(true) ;
				NumericScale = dbColumn.NumericScale;
			}

			public Type DataType { get; set; }
			public string DataTypeName { get; set; }
			public string ColumnName { get; set; }
			public bool IsReadOnly { get; set; }
			public bool IsIdentity { get; set; }
			public bool IsNullable { get; set; }
			public int? NumericScale { get; set; }
		}
	}

#endif
}
