using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility.DataReader
{
	public interface IColumnSchema
	{
		Type DataType { get; }
		string DataTypeName { get; }

		string ColumnName { get; }
		int ColumnOrdinal { get; set; }

		bool IsReadOnly { get; }
		bool IsIdentity { get; }

		int? NumericScale { get; set; }
	}

}
