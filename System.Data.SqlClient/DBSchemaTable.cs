using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000030 RID: 48
	internal sealed class DBSchemaTable
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000C7C4 File Offset: 0x0000B7C4
		internal DBSchemaTable(DataTable dataTable)
		{
			this.dataTable = dataTable;
			this.columns = dataTable.Columns;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000C7F1 File Offset: 0x0000B7F1
		internal DataColumn ColumnName
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.ColumnName);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000C7FA File Offset: 0x0000B7FA
		internal DataColumn Ordinal
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.ColumnOrdinal);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000C803 File Offset: 0x0000B803
		internal DataColumn Size
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.ColumnSize);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000C80C File Offset: 0x0000B80C
		internal DataColumn Precision
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.NumericPrecision);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000C815 File Offset: 0x0000B815
		internal DataColumn Scale
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.NumericScale);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C81E File Offset: 0x0000B81E
		internal DataColumn BaseServerName
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.BaseServerName);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000C827 File Offset: 0x0000B827
		internal DataColumn BaseColumnName
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.BaseColumnName);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C830 File Offset: 0x0000B830
		internal DataColumn BaseTableName
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.BaseTableName);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000C83A File Offset: 0x0000B83A
		internal DataColumn BaseCatalogName
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.BaseCatalogName);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C843 File Offset: 0x0000B843
		internal DataColumn BaseSchemaName
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.BaseSchemaName);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000C84C File Offset: 0x0000B84C
		internal DataColumn IsAutoIncrement
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsAutoIncrement);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C856 File Offset: 0x0000B856
		internal DataColumn IsUnique
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsUnique);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000C860 File Offset: 0x0000B860
		internal DataColumn IsKey
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsKey);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000C86A File Offset: 0x0000B86A
		internal DataColumn IsRowVersion
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsRowVersion);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C874 File Offset: 0x0000B874
		internal DataColumn DataType
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.DataType);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000C87E File Offset: 0x0000B87E
		internal DataColumn AllowDBNull
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.AllowDBNull);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000C888 File Offset: 0x0000B888
		internal DataColumn ProviderType
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.ProviderType);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000C892 File Offset: 0x0000B892
		internal DataColumn IsAliased
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsAliased);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000C89C File Offset: 0x0000B89C
		internal DataColumn IsExpression
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsExpression);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000C8A6 File Offset: 0x0000B8A6
		internal DataColumn IsIdentity
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsIdentity);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000C8B0 File Offset: 0x0000B8B0
		internal DataColumn IsHidden
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsHidden);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000C8BA File Offset: 0x0000B8BA
		internal DataColumn IsLong
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsLong);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000C8C4 File Offset: 0x0000B8C4
		internal DataColumn IsReadOnly
		{
			get
			{
				return this.CachedDataColumn(DBSchemaTable.ColumnEnum.IsReadOnly);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C8CE File Offset: 0x0000B8CE
		internal void AddRow(DBSchemaRow dataRow)
		{
			this.dataTable.Rows.Add(dataRow.DataRow);
			dataRow.DataRow.AcceptChanges();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C8F4 File Offset: 0x0000B8F4
		private DataColumn CachedDataColumn(DBSchemaTable.ColumnEnum column)
		{
			if (this.columnCache[(int)column] == null)
			{
				int num = this.columns.IndexOf(DBSchemaTable.DBCOLUMN_NAME[(int)column]);
				if (-1 != num)
				{
					this.columnCache[(int)column] = this.columns[num];
				}
			}
			return this.columnCache[(int)column];
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000C93E File Offset: 0x0000B93E
		internal DBSchemaRow NewRow()
		{
			return new DBSchemaRow(this, this.dataTable.NewRow());
		}

		// Token: 0x0400011C RID: 284
		private static readonly string[] DBCOLUMN_NAME = new string[]
		{
			"ColumnName",
			"ColumnOrdinal",
			"ColumnSize",
			"NumericPrecision",
			"NumericScale",
			"BaseServerName",
			"BaseCatalogName",
			"BaseColumnName",
			"BaseSchemaName",
			"BaseTableName",
			"IsAutoIncrement",
			"IsUnique",
			"IsKey",
			"IsRowVersion",
			"DataType",
			"AllowDBNull",
			"ProviderType",
			"IsAliased",
			"IsExpression",
			"IsIdentity",
			"IsHidden",
			"IsLong",
			"IsReadOnly",
			"SchemaMapping Unsorted Index"
		};

		// Token: 0x0400011D RID: 285
		internal DataTable dataTable;

		// Token: 0x0400011E RID: 286
		private DataColumnCollection columns;

		// Token: 0x0400011F RID: 287
		private DataColumn[] columnCache = new DataColumn[DBSchemaTable.DBCOLUMN_NAME.Length];

		// Token: 0x02000031 RID: 49
		private enum ColumnEnum
		{
			// Token: 0x04000121 RID: 289
			ColumnName,
			// Token: 0x04000122 RID: 290
			ColumnOrdinal,
			// Token: 0x04000123 RID: 291
			ColumnSize,
			// Token: 0x04000124 RID: 292
			NumericPrecision,
			// Token: 0x04000125 RID: 293
			NumericScale,
			// Token: 0x04000126 RID: 294
			BaseServerName,
			// Token: 0x04000127 RID: 295
			BaseCatalogName,
			// Token: 0x04000128 RID: 296
			BaseColumnName,
			// Token: 0x04000129 RID: 297
			BaseSchemaName,
			// Token: 0x0400012A RID: 298
			BaseTableName,
			// Token: 0x0400012B RID: 299
			IsAutoIncrement,
			// Token: 0x0400012C RID: 300
			IsUnique,
			// Token: 0x0400012D RID: 301
			IsKey,
			// Token: 0x0400012E RID: 302
			IsRowVersion,
			// Token: 0x0400012F RID: 303
			DataType,
			// Token: 0x04000130 RID: 304
			AllowDBNull,
			// Token: 0x04000131 RID: 305
			ProviderType,
			// Token: 0x04000132 RID: 306
			IsAliased,
			// Token: 0x04000133 RID: 307
			IsExpression,
			// Token: 0x04000134 RID: 308
			IsIdentity,
			// Token: 0x04000135 RID: 309
			IsHidden,
			// Token: 0x04000136 RID: 310
			IsLong,
			// Token: 0x04000137 RID: 311
			IsReadOnly,
			// Token: 0x04000138 RID: 312
			SchemaMappingUnsortedIndex
		}
	}
}
