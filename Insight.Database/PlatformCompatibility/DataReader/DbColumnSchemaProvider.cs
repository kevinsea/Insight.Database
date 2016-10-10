using System;
using System.Linq;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;


namespace Insight.Database.PlatformCompatibility.DataReader
{

#if NETCORE && !COREONDESK

    /// <summary>Provides platform independent schema information for DbColumn Schemas</summary>
    internal class DbColumnSchemaProvider : ColumnSchemaProviderBase
    {
        public DbColumnSchemaProvider(IEnumerable<DbColumn> argDbColumns)
            : base(GenerateColumnSchemas(argDbColumns))
        {
        }

        private static IColumnSchema[] GenerateColumnSchemas(IEnumerable<DbColumn> argDbColumns)
        {
            return argDbColumns.Select(dbColumn => (IColumnSchema)new DbColumnSchema(dbColumn)).ToArray();
        }

        internal ReadOnlyCollection<DbColumn> GetColumnSchema()
        {
            // TODO Review.  This honors removed columns but does not respect changes to other attributes
            // because its a second read only copy of the data.  We should subclass DbColumn and use it to hold the data?

            return new ReadOnlyCollection<DbColumn>(this.Cast<DbColumn>().ToList());
        }

        private class DbColumnSchema : DbColumn, IColumnSchema
        {
            private readonly DbColumn _dbColumn;

            public DbColumnSchema(DbColumn dbColumn)
            {
                AllowDBNull = dbColumn.AllowDBNull;
                BaseCatalogName = dbColumn.BaseCatalogName;
                BaseColumnName = dbColumn.BaseColumnName;
                BaseSchemaName = dbColumn.BaseSchemaName;
                BaseServerName = dbColumn.BaseServerName;
                BaseTableName = dbColumn.BaseTableName;
                ColumnName = dbColumn.ColumnName;
                ColumnOrdinal = dbColumn.ColumnOrdinal;
                ColumnSize = dbColumn.ColumnSize;
                IsAliased = dbColumn.IsAliased;
                IsAutoIncrement = dbColumn.IsAutoIncrement;
                IsExpression = dbColumn.IsExpression;
                IsHidden = dbColumn.IsHidden;
                IsIdentity = dbColumn.IsIdentity;
                IsKey = dbColumn.IsKey;
                IsLong = dbColumn.IsLong;
                IsReadOnly = dbColumn.IsReadOnly;
                IsUnique = dbColumn.IsUnique;
                NumericPrecision = dbColumn.NumericPrecision;
                NumericScale = dbColumn.NumericScale;
                UdtAssemblyQualifiedName = dbColumn.UdtAssemblyQualifiedName;
                DataType = dbColumn.DataType;
                DataTypeName = dbColumn.DataTypeName;

                // Unfortunately we need this for the property indexer.
                //  Is it even used?
                _dbColumn = dbColumn;
            }

            bool IColumnSchema.IsReadOnly
            {
                get { return IsReadOnly.GetValueOrDefault(false); }
            }

            bool IColumnSchema.IsIdentity
            {
                get { return IsIdentity.GetValueOrDefault(false); }
            }

            bool IColumnSchema.IsNullable
            {
                get { return AllowDBNull.GetValueOrDefault(true); }
            }

            int? IColumnSchema.NumericScale
            {
                get { return NumericScale; }
                set { NumericScale = value; }
            }

            public override object this[string property]
            {
                get { return _dbColumn[property]; }
            }
        }
    }

#endif
}
