using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility.DataReader
{
	/// <summary>Describes a column of data from a data provider</summary>
	public interface IColumnSchema
	{
		/// <summary>The name of the column</summary>
		string ColumnName { get; }
		/// <summary>The type of the column</summary>
		Type DataType { get; }
		/// <summary>The type string of the column from the provider</summary> 
		string DataTypeName { get; }
		/// <summary>Indicates if the column is read-only</summary> 
		bool IsReadOnly { get; }
		/// <summary>Indicates if the column is an identity</summary> 
		bool IsIdentity { get; }
		/// <summary>Indicates if the column is nullable</summary> 
		bool IsNullable { get; }
		/// <summary>Indicates if the column's numeric scale </summary> 
		int? NumericScale { get; set; }
	}

}
