using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database.CodeGenerator;

namespace Insight.Database.PlatformCompatibility.DataReader
{
	/// <summary>Platform compatiblity for DataReaders</summary>
	class DataReaderHelpers
	{

#if !(NETCORE && !COREONDESK)
		/// <summary>
		/// Retrieves the column schema in a compatible format
		/// </summary>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public static IColumnSchemaProvider GetColumnSchemaProvider(IDataReader dataReader)
		{
			return new DataTableColumnSchemaProvider(dataReader.GetSchemaTable().Copy());
		}

		/// <summary>
		/// Gets the underlying schema data for the SchemaProvider.  
		/// Use only when you must work with the platforms specific types.
		/// </summary>
		/// <returns>DataTable</returns>
		internal static DataTable GetUnderlyingDataTable(IColumnSchemaProvider schemaProvider)
		{
			var dataTableProvider = schemaProvider as DataTableColumnSchemaProvider;

			if (dataTableProvider != null)  //
				return dataTableProvider.GetSchemaTable();
			else
				throw new ArgumentException("Expected a DataTableColumnSchemaProvider.");
		}

#else
		/// <summary>
		/// Retrieves the column schema in a compatible format
		/// 
		/// The .Net Core version requires that the reader supports the IDbColumnSchemaGenerator interface.
		/// Otherwise it throws a NotSupportedException. it throws a NotSupportedException.
		/// </summary>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public static IColumnSchemaProvider GetColumnSchemaProvider(IDataReader dataReader)
		{
			var schema = GetUnderlyingDbColumnSchema(dataReader);

			return new DbColumnSchemaProvider(schema);
		}

		/// <summary>
		/// Retrieves the column schema in the newer DbColumn format if the reader supports the IDbColumnSchemaGenerator interface.
		/// 
		/// Otherwise it throws a NotSupportedException.
		/// </summary>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public static ReadOnlyCollection<DbColumn> GetUnderlyingDbColumnSchema(IDataReader dataReader)
		{
			var schemaGenerator = dataReader as IDbColumnSchemaGenerator;

			if (schemaGenerator == null)
				throw new NotSupportedException(".Net Core implementation  only supports retrieval of schema information " +
												"from readers implementing IDbColumnSchemaGenerator");

			ReadOnlyCollection<DbColumn> schema = schemaGenerator.GetColumnSchema();
			return schema;
		}

		/// <summary>
		/// Gets the underlying schema data for the SchemaProvider.  
		/// Use only when you must work with the platforms specific types.
		/// </summary>
		/// <returns>ReadOnlyCollection&lt;DbColumn&gt;</returns>
		public static ReadOnlyCollection<DbColumn> GetUnderlyingDbColumnSchema(IColumnSchemaProvider columnSchemaProvider)
		{
			var schemaProvider = columnSchemaProvider as DbColumnSchemaProvider;

			if (schemaProvider == null)
				throw new NullReferenceException("Expected DbColumnSchemaProvider.");

			return schemaProvider.GetColumnSchema();
		}

#endif

	}

}
