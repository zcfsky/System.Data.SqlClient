using System;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x0200002F RID: 47
	internal sealed class DBSchemaRow
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0000BF40 File Offset: 0x0000AF40
		internal static DBSchemaRow[] GetSortedSchemaRows(DataTable dataTable)
		{
			DataColumn dataColumn = new DataColumn("SchemaMapping Unsorted Index", typeof(int));
			dataTable.Columns.Add(dataColumn);
			int count = dataTable.Rows.Count;
			for (int i = 0; i < count; i++)
			{
				dataTable.Rows[i][dataColumn] = i;
			}
			DBSchemaTable dbschemaTable = new DBSchemaTable(dataTable);
			DataRow[] array = dataTable.Select(null, "ColumnOrdinal ASC", (DataViewRowState)22);
			DBSchemaRow[] array2 = new DBSchemaRow[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = new DBSchemaRow(dbschemaTable, array[j]);
			}
			return array2;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000BFE5 File Offset: 0x0000AFE5
		internal DBSchemaRow(DBSchemaTable schemaTable, DataRow dataRow)
		{
			this.schemaTable = schemaTable;
			this.dataRow = dataRow;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000BFFB File Offset: 0x0000AFFB
		internal DataRow DataRow
		{
			get
			{
				return this.dataRow;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000C004 File Offset: 0x0000B004
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000C041 File Offset: 0x0000B041
		internal string ColumnName
		{
			get
			{
				object obj = this.dataRow[this.schemaTable.ColumnName, (DataRowVersion)1536];
				if (!Convert.IsDBNull(obj))
				{
					return Convert.ToString(obj);
				}
				return "";
			}
			set
			{
				this.dataRow[this.schemaTable.ColumnName] = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000C05A File Offset: 0x0000B05A
		internal int Ordinal
		{
			set
			{
				this.dataRow[this.schemaTable.Ordinal] = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000C078 File Offset: 0x0000B078
		internal int Size
		{
			set
			{
				this.dataRow[this.schemaTable.Size] = value;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000C098 File Offset: 0x0000B098
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000C0D6 File Offset: 0x0000B0D6
		internal short Precision
		{
			get
			{
				object obj = this.dataRow[this.schemaTable.Precision, (DataRowVersion)1536];
				if (!Convert.IsDBNull(obj))
				{
					return Convert.ToInt16(obj, CultureInfo.CurrentCulture);
				}
				return 0;
			}
			set
			{
				this.dataRow[this.schemaTable.Precision] = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000C0F4 File Offset: 0x0000B0F4
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000C132 File Offset: 0x0000B132
		internal short Scale
		{
			get
			{
				object obj = this.dataRow[this.schemaTable.Scale, (DataRowVersion)1536];
				if (!Convert.IsDBNull(obj))
				{
					return Convert.ToInt16(obj, CultureInfo.CurrentCulture);
				}
				return 0;
			}
			set
			{
				this.dataRow[this.schemaTable.Scale] = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000C150 File Offset: 0x0000B150
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000C19A File Offset: 0x0000B19A
		internal string BaseColumnName
		{
			get
			{
				if (this.schemaTable.BaseColumnName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseColumnName, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj);
					}
				}
				return "";
			}
			set
			{
				this.dataRow[this.schemaTable.BaseColumnName] = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000C1B4 File Offset: 0x0000B1B4
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000C1FE File Offset: 0x0000B1FE
		internal string BaseServerName
		{
			get
			{
				if (this.schemaTable.BaseServerName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseServerName, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj);
					}
				}
				return "";
			}
			set
			{
				this.dataRow[this.schemaTable.BaseServerName] = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000C218 File Offset: 0x0000B218
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000C262 File Offset: 0x0000B262
		internal string BaseCatalogName
		{
			get
			{
				if (this.schemaTable.BaseCatalogName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseCatalogName, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj);
					}
				}
				return "";
			}
			set
			{
				this.dataRow[this.schemaTable.BaseCatalogName] = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000C27C File Offset: 0x0000B27C
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000C2C6 File Offset: 0x0000B2C6
		internal string BaseSchemaName
		{
			get
			{
				if (this.schemaTable.BaseSchemaName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseSchemaName, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj);
					}
				}
				return "";
			}
			set
			{
				this.dataRow[this.schemaTable.BaseSchemaName] = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000C2E0 File Offset: 0x0000B2E0
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000C32A File Offset: 0x0000B32A
		internal string BaseTableName
		{
			get
			{
				if (this.schemaTable.BaseTableName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseTableName, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj);
					}
				}
				return "";
			}
			set
			{
				this.dataRow[this.schemaTable.BaseTableName] = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000C344 File Offset: 0x0000B344
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000C38F File Offset: 0x0000B38F
		internal bool IsAutoIncrement
		{
			get
			{
				if (this.schemaTable.IsAutoIncrement != null)
				{
					object obj = this.dataRow[this.schemaTable.IsAutoIncrement, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsAutoIncrement] = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000C3B0 File Offset: 0x0000B3B0
		// (set) Token: 0x06000264 RID: 612 RVA: 0x0000C3FB File Offset: 0x0000B3FB
		internal bool IsUnique
		{
			get
			{
				if (this.schemaTable.IsUnique != null)
				{
					object obj = this.dataRow[this.schemaTable.IsUnique, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsUnique] = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000C41C File Offset: 0x0000B41C
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000C467 File Offset: 0x0000B467
		internal bool IsRowVersion
		{
			get
			{
				if (this.schemaTable.IsRowVersion != null)
				{
                    object obj = this.dataRow[this.schemaTable.IsRowVersion, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsRowVersion] = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000C488 File Offset: 0x0000B488
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000C4D3 File Offset: 0x0000B4D3
		internal bool IsKey
		{
			get
			{
				if (this.schemaTable.IsKey != null)
				{
					object obj = this.dataRow[this.schemaTable.IsKey, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsKey] = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000C4F1 File Offset: 0x0000B4F1
		internal bool IsAliased
		{
			set
			{
				this.dataRow[this.schemaTable.IsAliased] = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000C510 File Offset: 0x0000B510
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000C55B File Offset: 0x0000B55B
		internal bool IsExpression
		{
			get
			{
				if (this.schemaTable.IsExpression != null)
				{
					object obj = this.dataRow[this.schemaTable.IsExpression, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsExpression] = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000C579 File Offset: 0x0000B579
		internal bool IsIdentity
		{
			set
			{
				this.dataRow[this.schemaTable.IsIdentity] = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000C598 File Offset: 0x0000B598
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000C5E3 File Offset: 0x0000B5E3
		internal bool IsHidden
		{
			get
			{
				if (this.schemaTable.IsHidden != null)
				{
                    object obj = this.dataRow[this.schemaTable.IsHidden, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsHidden] = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000C604 File Offset: 0x0000B604
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000C64F File Offset: 0x0000B64F
		internal bool IsLong
		{
			get
			{
				if (this.schemaTable.IsLong != null)
				{
                    object obj = this.dataRow[this.schemaTable.IsLong, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsLong] = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C670 File Offset: 0x0000B670
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000C6BB File Offset: 0x0000B6BB
		internal bool IsReadOnly
		{
			get
			{
				if (this.schemaTable.IsReadOnly != null)
				{
                    object obj = this.dataRow[this.schemaTable.IsReadOnly, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return false;
			}
			set
			{
				this.dataRow[this.schemaTable.IsReadOnly] = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000C6D9 File Offset: 0x0000B6D9
		internal Type DataType
		{
			set
			{
				this.dataRow[this.schemaTable.DataType] = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000C6F4 File Offset: 0x0000B6F4
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0000C73F File Offset: 0x0000B73F
		internal bool AllowDBNull
		{
			get
			{
				if (this.schemaTable.AllowDBNull != null)
				{
                    object obj = this.dataRow[this.schemaTable.AllowDBNull, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.CurrentCulture);
					}
				}
				return true;
			}
			set
			{
				this.dataRow[this.schemaTable.AllowDBNull] = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000C760 File Offset: 0x0000B760
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000C7A6 File Offset: 0x0000B7A6
		internal int ProviderType
		{
			get
			{
				if (this.schemaTable.ProviderType != null)
				{
                    object obj = this.dataRow[this.schemaTable.ProviderType, (DataRowVersion)1536];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToInt32(obj);
					}
				}
				return 0;
			}
			set
			{
				this.dataRow[this.schemaTable.ProviderType] = value;
			}
		}

		// Token: 0x0400011A RID: 282
		private DBSchemaTable schemaTable;

		// Token: 0x0400011B RID: 283
		private DataRow dataRow;
	}
}
