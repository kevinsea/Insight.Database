using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility
{
    class DataReaderHelpers
    {
        public static IColumnSchemaProvider GetColumnSchemaProvider(IDataReader argDataReader)
        {

#if NETCORE
            var dbReader = argDataReader as DbDataReader;
            if (dbReader == null) {
                throw new NotSupportedException("Current dotnet core implementation currently only supports retrieval schema information from DbDataReaders");
            }

            return new DbColumnSchemaProvider(dbReader.GetColumnSchema());
#else
            return new DataTableColumnSchemaProvider(argDataReader.GetSchemaTable().Copy());
#endif
        }


        //TODO: These will need to be moved out into separate files
        private class ColumnSchemaProvider : IColumnSchemaProvider
        {
            private readonly List<IColumnSchema> _columnSchemas;

            public ColumnSchemaProvider(IEnumerable<IColumnSchema> argColumnSchemas)
            {
                _columnSchemas = new List<IColumnSchema>(argColumnSchemas);
            }

            public int Count
            {
                get { return _columnSchemas.Count; }
            }

            public IEnumerator<IColumnSchema> GetEnumerator()
            {
                return ((IEnumerable<IColumnSchema>)_columnSchemas).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IColumnSchema GetColumn(int columnIndex)
            {
                if (columnIndex > _columnSchemas.Count || columnIndex < 0)
                {
                    return null;
                }

                return _columnSchemas[columnIndex];
            }

            public IColumnSchema GetColumn(string columnName)
            {
                var index = GetColumnIndex(columnName);

                return GetColumn(index);
            }

            public void RemoveAt(int columnIndex)
            {
                _columnSchemas.RemoveAt(columnIndex);
            }

            public int GetColumnIndex(string columnName)
            {
                for (int i = 0; i < _columnSchemas.Count; i++)
                {
                    if (string.Equals(_columnSchemas[i].ColumnName, columnName, StringComparison.OrdinalIgnoreCase)) {
                        return i;
                    }
                }

                return -1;
            }
        }

#if NETCORE

        private class DbColumnSchemaProvider : ColumnSchemaProvider
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

#else
		private class DataTableColumnSchemaProvider : ColumnSchemaProvider
        {
            private readonly DataTable _schemaTable;

            public DataTableColumnSchemaProvider(DataTable argSchemaTable)
                : base(GenerateColumnSchemaWrappers(argSchemaTable))
            {
                _schemaTable = argSchemaTable;

                // Remove Readonly flag from writable column
                _schemaTable.Columns["NumericScale"].ReadOnly = false;
	            _schemaTable.Columns["ColumnOrdinal"].ReadOnly = false;
            }

            public DataTable SchemaTable
            {
                get { return _schemaTable; }
            }

            private static IColumnSchema[] GenerateColumnSchemaWrappers(DataTable argSchemaTable)
            {
                return argSchemaTable.Rows.Cast<DataRow>().Select(x => (IColumnSchema)new DataRowColumnSchemaWrapper(x)).ToArray();
            }

            private class DataRowColumnSchemaWrapper : IColumnSchema
            {
                private readonly DataRow _dataRow;

                public DataRowColumnSchemaWrapper(DataRow argRow)
                {
                    _dataRow = argRow;
                }

                public string ColumnName
                {
                    get { return _dataRow["ColumnName"].ToString(); }
                }

                public Type DataType
                {
                    get { return (Type)_dataRow["DataType"]; }
                }

                public string DataTypeName
                {
                    get
                    {
                        // Is the check needed or will it just return null if we try to access column
                        //  that doesnt exist?
                        return _dataRow.Table.Columns.Contains("DataTypeName") 
                            ? _dataRow["DataTypeName"].ToString() : null;
                    }
                }

                public bool IsIdentity
                {
                    get
                    {
                        return _dataRow.Table.Columns.Contains("IsIdentity") 
                            && !_dataRow.IsNull("IsIdentity") 
                            && Convert.ToBoolean(_dataRow["IsIdentity"], CultureInfo.InvariantCulture);
                    }
                }

                public bool IsReadOnly
                {
                    get
                    {
                        return _dataRow.Table.Columns.Contains("IsReadOnly")
                            && !_dataRow.IsNull("IsReadOnly")
                            && Convert.ToBoolean(_dataRow["IsReadOnly"], CultureInfo.InvariantCulture);
                    }
                }

                public int? NumericScale
                {
                    get { return (int?) _dataRow["NumericScale"]; }
                    set
                    {
                        _dataRow["NumericScale"] = value;
                    }
                }

	            public int ColumnOrdinal
	            {
		            get { return (int) _dataRow["ColumnOrdinal"]; }
		            set { _dataRow["ColumnOrdinal"] = value; }
	            }
            }
        }

#endif

	}
}
