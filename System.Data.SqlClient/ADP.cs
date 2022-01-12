using System;
using System.Collections;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x02000006 RID: 6
	internal class ADP
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00005D4C File Offset: 0x00004D4C
		internal static DataTable CreateSchemaTable(DataTable schemaTable, int capacity)
		{
			if (schemaTable == null)
			{
				schemaTable = new DataTable("SchemaTable");
				if (0 < capacity)
				{
					schemaTable.MinimumCapacity = capacity;
				}
			}
			DataColumnCollection columns = schemaTable.Columns;
			ADP.AddColumn(columns, null, "ColumnName", typeof(string));
			ADP.AddColumn(columns, 0, "ColumnOrdinal", typeof(int));
			ADP.AddColumn(columns, null, "ColumnSize", typeof(int));
			ADP.AddColumn(columns, null, "NumericPrecision", typeof(short));
			ADP.AddColumn(columns, null, "NumericScale", typeof(short));
			ADP.AddColumn(columns, null, "IsUnique", typeof(bool));
			ADP.AddColumn(columns, null, "IsKey", typeof(bool));
			ADP.AddColumn(columns, null, "BaseServerName", typeof(string));
			ADP.AddColumn(columns, null, "BaseCatalogName", typeof(string));
			ADP.AddColumn(columns, null, "BaseColumnName", typeof(string));
			ADP.AddColumn(columns, null, "BaseSchemaName", typeof(string));
			ADP.AddColumn(columns, null, "BaseTableName", typeof(string));
			ADP.AddColumn(columns, null, "DataType", typeof(object));
			ADP.AddColumn(columns, null, "AllowDBNull", typeof(bool));
			ADP.AddColumn(columns, null, "ProviderType", typeof(int));
			ADP.AddColumn(columns, null, "IsAliased", typeof(bool));
			ADP.AddColumn(columns, null, "IsExpression", typeof(bool));
			ADP.AddColumn(columns, null, "IsIdentity", typeof(bool));
			ADP.AddColumn(columns, null, "IsAutoIncrement", typeof(bool));
			ADP.AddColumn(columns, null, "IsRowVersion", typeof(bool));
			ADP.AddColumn(columns, null, "IsHidden", typeof(bool));
			ADP.AddColumn(columns, false, "IsLong", typeof(bool));
			ADP.AddColumn(columns, null, "IsReadOnly", typeof(bool));
			return schemaTable;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005F80 File Offset: 0x00004F80
		private static void AddColumn(DataColumnCollection columns, object defaultValue, string name, Type type)
		{
			if (!columns.Contains(name))
			{
				DataColumn dataColumn = new DataColumn(name, type);
				if (defaultValue != null)
				{
					dataColumn.DefaultValue = defaultValue;
				}
				columns.Add(dataColumn);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005FB0 File Offset: 0x00004FB0
		internal static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
		{
			int num = columnNameArray.Length;
			Hashtable hashtable = new Hashtable(num);
			int num2 = num;
			int num3 = num - 1;
			while (0 <= num3)
			{
				string text = columnNameArray[num3];
				if (text != null && 0 < text.Length)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					if (hashtable.Contains(text))
					{
						num2 = Math.Min(num2, (int)hashtable[text]);
					}
					hashtable[text] = num3;
				}
				else
				{
					columnNameArray[num3] = "";
					num2 = num3;
				}
				num3--;
			}
			int uniqueIndex = 1;
			for (int i = num2; i < num; i++)
			{
				string text2 = columnNameArray[i];
				if (text2.Length == 0)
				{
					columnNameArray[i] = "Column";
					uniqueIndex = ADP.GenerateUniqueName(hashtable, ref columnNameArray[i], i, uniqueIndex);
				}
				else
				{
					text2 = text2.ToLower(CultureInfo.InvariantCulture);
					if (i != (int)hashtable[text2])
					{
						ADP.GenerateUniqueName(hashtable, ref columnNameArray[i], i, 1);
					}
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000060A4 File Offset: 0x000050A4
		private static int GenerateUniqueName(Hashtable hash, ref string columnName, int index, int uniqueIndex)
		{
			string text;
			string text2;
			for (;;)
			{
				text = columnName + uniqueIndex.ToString();
				text2 = text.ToLower(CultureInfo.InvariantCulture);
				if (!hash.Contains(text2))
				{
					break;
				}
				uniqueIndex++;
			}
			columnName = text;
			hash.Add(text2, index);
			return uniqueIndex;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000060EF File Offset: 0x000050EF
		internal static int SrcCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, 0);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006103 File Offset: 0x00005103
		internal static int DstCompare(string strA, string strB)
		{
            return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, (CompareOptions)25);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006118 File Offset: 0x00005118
		internal static bool IsEmpty(string str)
		{
			return str == null || 0 == str.Length;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00006128 File Offset: 0x00005128
		internal static string[] ParseProcedureName(string procedure)
		{
			string[] array = new string[4];
			if (!ADP.IsEmpty(procedure))
			{
				int i = 0;
				int num = 0;
				while (i < 4)
				{
					int num2 = procedure.IndexOf('.', num);
					if (-1 == num2)
					{
						array[i] = procedure.Substring(num);
						break;
					}
					array[i] = procedure.Substring(num, num2 - num);
					num = num2 + 1;
					if (procedure.Length <= num)
					{
						break;
					}
					i++;
				}
				switch (i)
				{
				case 0:
					array[3] = array[0];
					array[2] = null;
					array[1] = null;
					array[0] = null;
					break;
				case 1:
					array[3] = array[1];
					array[2] = array[0];
					array[1] = null;
					array[0] = null;
					break;
				case 2:
					array[3] = array[2];
					array[2] = array[1];
					array[1] = array[0];
					array[0] = null;
					break;
				}
			}
			return array;
		}

		// Token: 0x04000075 RID: 117
		internal const int MaxConnectionStringCacheSize = 250;

		// Token: 0x04000076 RID: 118
		internal const string BeginTransaction = "BeginTransaction";

		// Token: 0x04000077 RID: 119
		internal const string ChangeDatabase = "ChangeDatabase";

		// Token: 0x04000078 RID: 120
		internal const string Cancel = "Cancel";

		// Token: 0x04000079 RID: 121
		internal const string Clone = "Clone";

		// Token: 0x0400007A RID: 122
		internal const string CommitTransaction = "CommitTransaction";

		// Token: 0x0400007B RID: 123
		internal const string ConnectionString = "ConnectionString";

		// Token: 0x0400007C RID: 124
		internal const string DataSetColumn = "DataSetColumn";

		// Token: 0x0400007D RID: 125
		internal const string DataSetTable = "DataSetTable";

		// Token: 0x0400007E RID: 126
		internal const string Delete = "Delete";

		// Token: 0x0400007F RID: 127
		internal const string DeleteCommand = "DeleteCommand";

		// Token: 0x04000080 RID: 128
		internal const string DeriveParameters = "DeriveParameters";

		// Token: 0x04000081 RID: 129
		internal const string ExecuteReader = "ExecuteReader";

		// Token: 0x04000082 RID: 130
		internal const string ExecuteNonQuery = "ExecuteNonQuery";

		// Token: 0x04000083 RID: 131
		internal const string ExecuteScalar = "ExecuteScalar";

		// Token: 0x04000084 RID: 132
		internal const string ExecuteXmlReader = "ExecuteXmlReader";

		// Token: 0x04000085 RID: 133
		internal const string Fill = "Fill";

		// Token: 0x04000086 RID: 134
		internal const string FillSchema = "FillSchema";

		// Token: 0x04000087 RID: 135
		internal const string GetBytes = "GetBytes";

		// Token: 0x04000088 RID: 136
		internal const string GetChars = "GetChars";

		// Token: 0x04000089 RID: 137
		internal const string Insert = "Insert";

		// Token: 0x0400008A RID: 138
		internal const string Parameter = "Parameter";

		// Token: 0x0400008B RID: 139
		internal const string ParameterName = "ParameterName";

		// Token: 0x0400008C RID: 140
		internal const string Prepare = "Prepare";

		// Token: 0x0400008D RID: 141
		internal const string Remove = "Remove";

		// Token: 0x0400008E RID: 142
		internal const string RollbackTransaction = "RollbackTransaction";

		// Token: 0x0400008F RID: 143
		internal const string SaveTransaction = "SaveTransaction";

		// Token: 0x04000090 RID: 144
		internal const string Select = "Select";

		// Token: 0x04000091 RID: 145
		internal const string SelectCommand = "SelectCommand";

		// Token: 0x04000092 RID: 146
		internal const string SourceColumn = "SourceColumn";

		// Token: 0x04000093 RID: 147
		internal const string SourceVersion = "SourceVersion";

		// Token: 0x04000094 RID: 148
		internal const string SourceTable = "SourceTable";

		// Token: 0x04000095 RID: 149
		internal const string Update = "Update";

		// Token: 0x04000096 RID: 150
		internal const string UpdateCommand = "UpdateCommand";

		// Token: 0x04000097 RID: 151
        internal const CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		// Token: 0x04000098 RID: 152
		internal const int DefaultCommandTimeout = 30;

		// Token: 0x04000099 RID: 153
		internal const int DefaultConnectionTimeout = 15;

		// Token: 0x0400009A RID: 154
		internal static readonly object EventRowUpdated = new object();

		// Token: 0x0400009B RID: 155
		internal static readonly object EventRowUpdating = new object();

		// Token: 0x0400009C RID: 156
		internal static readonly object EventInfoMessage = new object();

		// Token: 0x0400009D RID: 157
		internal static readonly object EventStateChange = new object();

		// Token: 0x0400009E RID: 158
		internal static readonly IntPtr InvalidIntPtr = new IntPtr(-1);
	}
}
