using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database.PlatformCompatibility.DataReader;

namespace Insight.Database.PlatformCompatibility.DataReader
{

#if NETCORE && !COREONDESK

	/// <summary>Provides platform independent schema information for DbColumn Schemas</summary>
	internal class DbColumnSchemaProvider : ColumnSchemaProviderBase
	{
		Dictionary<string, DbColumn> _columnSchemas;

		public DbColumnSchemaProvider(IEnumerable<DbColumn> argDbColumns)
			: base(GenerateColumnSchemas(argDbColumns))
		{
			_columnSchemas = new Dictionary<string, DbColumn>();

			foreach (DbColumn dbColumn in argDbColumns.ToList())
			{
				if (_columnSchemas.ContainsKey(dbColumn.ColumnName) == false)
					_columnSchemas.Add(dbColumn.ColumnName, dbColumn);
			}
		}

		private static IColumnSchema[] GenerateColumnSchemas(IEnumerable<DbColumn> argDbColumns)
		{
			return argDbColumns.Select(dbColumn => (IColumnSchema)new ColumnSchema(dbColumn)).ToArray();
		}

		internal ReadOnlyCollection<DbColumn> GetColumnSchema()
		{
			// TODO Review.  This honors removed columns but does not respect changes to other attributes
			// because its a second read only copy of the data.  We should subclass DbColumn and use it to hold the data?

			//return new ReadOnlyCollection<DbColumn>(_columnSchemas.Values.ToList());

			var list = new List<DbColumn>();

			foreach (IColumnSchema item in this)
			{
				DbColumn dbColumn;

				if (_columnSchemas.TryGetValue(item.ColumnName, out dbColumn))
					list.Add(dbColumn);
			}

			return new ReadOnlyCollection<DbColumn>(list);
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
				IsNullable = dbColumn.AllowDBNull.GetValueOrDefault(true);
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
