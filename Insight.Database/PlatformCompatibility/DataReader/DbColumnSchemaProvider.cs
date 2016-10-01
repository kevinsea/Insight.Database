using System;
using System.Collections.Generic;
using System.Data;
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
                return argDbColumns.Select(dbColumn => (IColumnSchema)new ColumnSchema
                {
                    DataType = dbColumn.DataType,
                    ColumnName = dbColumn.ColumnName,
                    DataTypeName = dbColumn.DataTypeName,
                    IsIdentity = dbColumn.IsIdentity.GetValueOrDefault(false),
                    IsReadOnly = dbColumn.IsReadOnly.GetValueOrDefault(false),
                    NumericScale = dbColumn.NumericScale,
					ColumnOrdinal = dbColumn.ColumnOrdinal
                }).ToArray();
            }

			// TODO review implementation of this
			public override DataTable GetSchemaTable()
			{
				throw new NotImplementedException("DataTable does not exist in .NetCore.");
			}


            private class ColumnSchema : IColumnSchema
            {
                public Type DataType { get; set; }
                public string DataTypeName { get; set; }
                public string ColumnName { get; set; }
                public bool IsReadOnly { get; set; }
                public bool IsIdentity { get; set; }
                public int? NumericScale { get; set; }
				public int ColumnOrdinal { get; set; }
            }
        }

#endif
}
