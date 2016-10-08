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
		string ColumnName { get; }
		Type DataType { get; }
		string DataTypeName { get; }
		bool IsReadOnly { get; }
		bool IsIdentity { get; }
		bool IsNullable { get; }
		int? NumericScale { get; set; }
	}

}
