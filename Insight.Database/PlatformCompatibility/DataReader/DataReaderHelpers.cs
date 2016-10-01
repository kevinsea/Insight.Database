using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database.PlatformCompatibility.DataReader
{
    class DataReaderHelpers
    {
        public static IColumnSchemaProvider GetColumnSchemaProvider(IDataReader argDataReader)
        {

#if NETCORE && !COREONDESK
            var dbReader = argDataReader as DbDataReader;
            if (dbReader == null) {
                throw new NotSupportedException("Current dotnet core implementation currently only supports retrieval schema information from DbDataReaders");
            }

            return new DbColumnSchemaProvider(dbReader.GetColumnSchema());
#else
			return new DataTableColumnSchemaProvider(argDataReader.GetSchemaTable().Copy());
#endif
        }

	}
}
