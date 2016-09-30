using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility
{
    public interface IColumnSchema
    {
        Type DataType { get; }
        string DataTypeName { get; }

        string ColumnName { get; }

        bool IsReadOnly { get; }
        bool IsIdentity { get; }

        int? NumericScale { get; set; }
		int ColumnOrdinal { get; set; }
    }

    public interface IColumnSchemaProvider : IEnumerable<IColumnSchema>
    {


        int Count { get; }

        IColumnSchema GetColumn(int columnIndex);
        IColumnSchema GetColumn(string columnName);

        void RemoveAt(int columnIndex);

        int GetColumnIndex(string columnName);
    }


}
