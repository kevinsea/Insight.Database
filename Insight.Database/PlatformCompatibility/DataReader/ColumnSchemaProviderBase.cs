using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility.DataReader
{
	public interface IColumnSchemaProvider : IEnumerable<IColumnSchema>
	{
		int Count { get; }

		IColumnSchema GetColumn(int columnIndex);
		IColumnSchema GetColumn(string columnName);

		int GetColumnIndex(string columnName);

		void RemoveAt(int columnIndex);

		DataTable GetSchemaTable();
	}

	internal abstract class ColumnSchemaProviderBase : IColumnSchemaProvider
	{
		private readonly List<IColumnSchema> _columnSchemas;

		protected ColumnSchemaProviderBase(IEnumerable<IColumnSchema> argColumnSchemas)
		{
			_columnSchemas = new List<IColumnSchema>(argColumnSchemas);
		}

		public int Count => _columnSchemas.Count;

		public IEnumerator<IColumnSchema> GetEnumerator() => ((IEnumerable<IColumnSchema>)_columnSchemas).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IColumnSchema GetColumn(int columnIndex)
		{
			if (columnIndex > _columnSchemas.Count || columnIndex < 0)
				return null;

			return _columnSchemas[columnIndex];
		}

		public IColumnSchema GetColumn(string columnName)
		{
			var index = GetColumnIndex(columnName);

			return GetColumn(index);
		}

		public virtual void RemoveAt(int columnIndex)
		{
			_columnSchemas.RemoveAt(columnIndex);
		}

		public int GetColumnIndex(string columnName)
		{
			for (int i = 0; i < _columnSchemas.Count; i++)
			{
				if (string.Equals(_columnSchemas[i].ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}

			return -1;
		}

		public abstract DataTable GetSchemaTable();
	}

}
