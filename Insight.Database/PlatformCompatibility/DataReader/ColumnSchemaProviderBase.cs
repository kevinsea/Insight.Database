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
	/// <summary>Describes a data set</summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public interface IColumnSchemaProvider : IEnumerable<IColumnSchema>
	{
		/// <summary>The number of columns in the data set</summary>
		int Count { get; }

		/// <summary> Returns the Column specified </summary>
		IColumnSchema GetColumn(int columnIndex);

		/// <summary>Returns the Column for the provided name </summary>
		IColumnSchema GetColumn(string columnName);

		/// <summary> Returns the column position of the column specified </summary>
		int GetColumnIndex(string columnName);

		/// <summary> Removes the Column specified </summary>
		void RemoveAt(int columnIndex);
	}

	internal abstract class ColumnSchemaProviderBase : IColumnSchemaProvider
	{
		private readonly List<IColumnSchema> _columnSchemas;

		protected ColumnSchemaProviderBase(IEnumerable<IColumnSchema> argColumnSchemas)
		{
			_columnSchemas = new List<IColumnSchema>(argColumnSchemas);
		}

		/// <summary>The number columns</summary>
		public int Count
		{
			get { return _columnSchemas.Count; }
		}

		/// <summary>Gets and enumerator</summary>
		public IEnumerator<IColumnSchema> GetEnumerator()
		{
			return ((IEnumerable<IColumnSchema>) _columnSchemas).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary> Returns the Column specified </summary>
		public IColumnSchema GetColumn(int columnIndex)
		{
			if (columnIndex > _columnSchemas.Count || columnIndex < 0)
				return null;

			return _columnSchemas[columnIndex];
		}

		/// <summary> Returns the Column for the provided name </summary>
		public IColumnSchema GetColumn(string columnName)
		{
			var index = GetColumnIndex(columnName);

			return GetColumn(index);
		}

		/// <summary> Removes the Column specified </summary>
		public virtual void RemoveAt(int columnIndex)
		{
			_columnSchemas.RemoveAt(columnIndex);
		}

		/// <summary> Returns the column position of the column specified </summary>
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

	}

}
