using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility.DataReader
{

#if !(NETCORE && !COREONDESK)

	/// <summary>Provides platform independent schema information for DataTables Schemas</summary>
	internal class DataTableColumnSchemaProvider : ColumnSchemaProviderBase
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

		public DataTable GetSchemaTable()
		{
			return _schemaTable;
		}

		/// <summary>
		/// Removes the specified column
		/// </summary>
		/// <param name="columnIndex"></param>
		public override void RemoveAt(int columnIndex)
		{
			base.RemoveAt(columnIndex);

			_schemaTable.Rows.RemoveAt(columnIndex);

			for (int i = columnIndex; i < _schemaTable.Rows.Count; i++)
			{
				_schemaTable.Rows[i]["ColumnOrdinal"] = i;
			}
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

			public bool IsNullable
			{
				get
				{
					return _dataRow.Table.Columns.Contains("IsNullable")
						&& !_dataRow.IsNull("IsNullable")
						&& Convert.ToBoolean(_dataRow["IsNullable"], CultureInfo.InvariantCulture);
				}
			}

			public int? NumericScale
			{
				get { return (int?)_dataRow["NumericScale"]; }
				set
				{
					_dataRow["NumericScale"] = value;
				}
			}

		}
	}

#endif
}
