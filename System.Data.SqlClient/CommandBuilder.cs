using System;
using System.Data.Common;
using System.Text;

namespace System.Data.SqlClient
{
	// Token: 0x02000003 RID: 3
	internal sealed class CommandBuilder : IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		internal CommandBuilder()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000010D8
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020E0 File Offset: 0x000010E0
		internal IDbDataAdapter DataAdapter
		{
			get
			{
				return this.adapter;
			}
			set
			{
				if (this.adapter != value)
				{
					this.Dispose(true);
					this.adapter = value;
					if (this.adapter != null && this.adapter is SqlDataAdapter)
					{
						this.sqlhandler = new SqlRowUpdatingEventHandler(this.SqlRowUpdating);
						((SqlDataAdapter)this.adapter).RowUpdating += this.sqlhandler;
						this.namedParameters = true;
					}
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002148 File Offset: 0x00001148
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000215E File Offset: 0x0000115E
		internal string QuotePrefix
		{
			get
			{
				if (this.quotePrefix == null)
				{
					return "";
				}
				return this.quotePrefix;
			}
			set
			{
				if (this.dbSchemaTable != null)
				{
					throw new InvalidOperationException(Res.GetString("ADP_NoQuoteChange"));
				}
				this.quotePrefix = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000217F File Offset: 0x0000117F
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002195 File Offset: 0x00001195
		internal string QuoteSuffix
		{
			get
			{
				if (this.quoteSuffix == null)
				{
					return "";
				}
				return this.quoteSuffix;
			}
			set
			{
				if (this.dbSchemaTable != null)
				{
					throw new InvalidOperationException(Res.GetString("ADP_NoQuoteChange"));
				}
				this.quoteSuffix = value;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021B6 File Offset: 0x000011B6
		private bool IsBehavior(CommandBuilderBehavior behavior)
		{
			return behavior == (this.options & behavior);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021C3 File Offset: 0x000011C3
		private bool IsNotBehavior(CommandBuilderBehavior behavior)
		{
			return behavior != (this.options & behavior);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021D3 File Offset: 0x000011D3
		private string QuotedColumn(string column)
		{
			return this.QuotePrefix + column + this.QuoteSuffix;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021E7 File Offset: 0x000011E7
		private string QuotedBaseTableName
		{
			get
			{
				return this.quotedBaseTableName;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000021EF File Offset: 0x000011EF
		internal IDbCommand SourceCommand
		{
			get
			{
				if (this.adapter != null)
				{
					return this.adapter.SelectCommand;
				}
				return null;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002206 File Offset: 0x00001206
		private void ClearHandlers()
		{
			if (this.sqlhandler != null)
			{
				((SqlDataAdapter)this.adapter).RowUpdating -= this.sqlhandler;
				this.sqlhandler = null;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000222D File Offset: 0x0000122D
		private void ClearState()
		{
			this.dbSchemaTable = null;
			this.dbSchemaRows = null;
			this.sourceColumnNames = null;
			this.quotedBaseTableName = null;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000224B File Offset: 0x0000124B
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002254 File Offset: 0x00001254
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.ClearHandlers();
				this.RefreshSchema();
				this.adapter = null;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000226C File Offset: 0x0000126C
		private IDbCommand GetXxxCommand(IDbCommand cmd)
		{
			return cmd;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000226F File Offset: 0x0000126F
		internal IDbCommand GetInsertCommand()
		{
			this.BuildCache(true);
			return this.GetXxxCommand(this.BuildInsertCommand(null, null));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002286 File Offset: 0x00001286
		internal IDbCommand GetUpdateCommand()
		{
			this.BuildCache(true);
			return this.GetXxxCommand(this.BuildUpdateCommand(null, null));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000229D File Offset: 0x0000129D
		internal IDbCommand GetDeleteCommand()
		{
			this.BuildCache(true);
			return this.GetXxxCommand(this.BuildDeleteCommand(null, null));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022B4 File Offset: 0x000012B4
		internal void RefreshSchema()
		{
			this.ClearState();
			if (this.adapter != null)
			{
				if (this.insertCommand == this.adapter.InsertCommand)
				{
					this.adapter.InsertCommand = null;
				}
				if (this.updateCommand == this.adapter.UpdateCommand)
				{
					this.adapter.UpdateCommand = null;
				}
				if (this.deleteCommand == this.adapter.DeleteCommand)
				{
					this.adapter.DeleteCommand = null;
				}
			}
			if (this.insertCommand != null)
			{
				this.insertCommand.Dispose();
			}
			if (this.updateCommand != null)
			{
				this.updateCommand.Dispose();
			}
			if (this.deleteCommand != null)
			{
				this.deleteCommand.Dispose();
			}
			this.insertCommand = null;
			this.updateCommand = null;
			this.deleteCommand = null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000237A File Offset: 0x0000137A
		private void SqlRowUpdating(object sender, SqlRowUpdatingEventArgs ruevent)
		{
			this.RowUpdating(sender, ruevent);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002384 File Offset: 0x00001384
		private void RowUpdating(object sender, RowUpdatingEventArgs ruevent)
		{
			if (ruevent.Status != null)
			{
				return;
			}
			if (ruevent.Command != null)
			{
				switch (ruevent.StatementType)
				{
                    case (StatementType)1:
					if (this.insertCommand != ruevent.Command)
					{
						return;
					}
					break;
				case (StatementType)2:
					if (this.updateCommand != ruevent.Command)
					{
						return;
					}
					break;
                case (StatementType)3:
					if (this.deleteCommand != ruevent.Command)
					{
						return;
					}
					break;
				default:
					return;
				}
			}
			try
			{
				this.BuildCache(false);
				switch (ruevent.StatementType)
				{
				case (StatementType)1:
					ruevent.Command = this.BuildInsertCommand(ruevent.TableMapping, ruevent.Row);
					break;
                case (StatementType)2:
					ruevent.Command = this.BuildUpdateCommand(ruevent.TableMapping, ruevent.Row);
					break;
                case (StatementType)3:
					ruevent.Command = this.BuildDeleteCommand(ruevent.TableMapping, ruevent.Row);
					break;
				}
				if (ruevent.Command == null)
				{
					if (ruevent.Row != null)
					{
						ruevent.Row.AcceptChanges();
					}
					ruevent.Status = (UpdateStatus)2;
				}
			}
			catch (Exception errors)
			{
				ruevent.Errors = errors;
				ruevent.Status = (UpdateStatus)1;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024A0 File Offset: 0x000014A0
		private void BuildCache(bool closeConnection)
		{
			IDbCommand sourceCommand = this.SourceCommand;
			if (sourceCommand == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_MissingSourceCommand"));
			}
			IDbConnection connection = sourceCommand.Connection;
			if (connection == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_MissingSourceCommandConnection"));
			}
			if (this.DataAdapter != null)
			{
				this.missingMapping = this.DataAdapter.MissingMappingAction;
				if (1 != (int)this.missingMapping)
				{
					this.missingMapping = (MissingMappingAction)3;
				}
			}
			if (this.dbSchemaTable == null)
			{
				if ((1 & (int)connection.State) == null)
				{
					connection.Open();
				}
				else
				{
					closeConnection = false;
				}
				try
				{
					DataTable dataTable = null;
					IDataReader dataReader = null;
					try
					{
						dataReader = sourceCommand.ExecuteReader((CommandBehavior)6);
						dataTable = dataReader.GetSchemaTable();
					}
					finally
					{
						if (dataReader != null)
						{
							dataReader.Dispose();
						}
						dataReader = null;
					}
					if (dataTable == null)
					{
						throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNoTableInfo"));
					}
					this.BuildInformation(dataTable);
					this.dbSchemaTable = dataTable;
					int num = this.dbSchemaRows.Length;
					this.sourceColumnNames = new string[num];
					for (int i = 0; i < num; i++)
					{
						if (this.dbSchemaRows[i] != null)
						{
							this.sourceColumnNames[i] = this.dbSchemaRows[i].ColumnName;
						}
					}
					ADP.BuildSchemaTableInfoTableNames(this.sourceColumnNames);
				}
				finally
				{
					if (closeConnection && connection.State != null)
					{
						connection.Close();
					}
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025F4 File Offset: 0x000015F4
		private void BuildInformation(DataTable schemaTable)
		{
			DBSchemaRow[] sortedSchemaRows = DBSchemaRow.GetSortedSchemaRows(schemaTable);
			if (sortedSchemaRows == null || sortedSchemaRows.Length == 0)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNoTableInfo"));
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = null;
			for (int i = 0; i < sortedSchemaRows.Length; i++)
			{
				DBSchemaRow dbschemaRow = sortedSchemaRows[i];
				string baseTableName = dbschemaRow.BaseTableName;
				if (baseTableName == null || baseTableName.Length == 0)
				{
					sortedSchemaRows[i] = null;
				}
				else
				{
					string text5 = dbschemaRow.BaseServerName;
					string text6 = dbschemaRow.BaseCatalogName;
					string text7 = dbschemaRow.BaseSchemaName;
					if (text5 == null)
					{
						text5 = "";
					}
					if (text6 == null)
					{
						text6 = "";
					}
					if (text7 == null)
					{
						text7 = "";
					}
					if (text4 == null)
					{
						text = text5;
						text2 = text6;
						text3 = text7;
						text4 = baseTableName;
					}
					else if (ADP.SrcCompare(text4, baseTableName) != 0 || ADP.SrcCompare(text3, text7) != 0 || ADP.SrcCompare(text2, text6) != 0 || ADP.SrcCompare(text, text5) != 0)
					{
						throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLJoinUnsupported"));
					}
				}
			}
			if (text.Length == 0)
			{
				text = null;
			}
			if (text2.Length == 0)
			{
				text = null;
				text2 = null;
			}
			if (text3.Length == 0)
			{
				text = null;
				text2 = null;
				text3 = null;
			}
			if (text4 == null || text4.Length == 0)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNoTableInfo"));
			}
			if (!ADP.IsEmpty(this.quotePrefix) && -1 != text4.IndexOf(this.quotePrefix))
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNestedQuote", new object[]
				{
					text4,
					this.quotePrefix
				}));
			}
			if (!ADP.IsEmpty(this.quoteSuffix) && -1 != text4.IndexOf(this.quoteSuffix))
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNestedQuote", new object[]
				{
					text4,
					this.quotePrefix
				}));
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (text != null)
			{
				stringBuilder.Append(this.QuotePrefix);
				stringBuilder.Append(text);
				stringBuilder.Append(this.QuoteSuffix);
				stringBuilder.Append(".");
			}
			if (text2 != null)
			{
				stringBuilder.Append(this.QuotePrefix);
				stringBuilder.Append(text2);
				stringBuilder.Append(this.QuoteSuffix);
				stringBuilder.Append(".");
			}
			if (text3 != null)
			{
				stringBuilder.Append(this.QuotePrefix);
				stringBuilder.Append(text3);
				stringBuilder.Append(this.QuoteSuffix);
				stringBuilder.Append(".");
			}
			stringBuilder.Append(this.QuotePrefix);
			stringBuilder.Append(text4);
			stringBuilder.Append(this.QuoteSuffix);
			this.quotedBaseTableName = stringBuilder.ToString();
			this.dbSchemaRows = sortedSchemaRows;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000028AC File Offset: 0x000018AC
		private IDbCommand BuildNewCommand(IDbCommand cmd)
		{
			IDbCommand sourceCommand = this.SourceCommand;
			if (cmd == null)
			{
				cmd = sourceCommand.Connection.CreateCommand();
				cmd.CommandTimeout = sourceCommand.CommandTimeout;
				cmd.Transaction = sourceCommand.Transaction;
			}
			cmd.CommandType = (CommandType)1;
			cmd.UpdatedRowSource = 0;
			return cmd;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000028F8 File Offset: 0x000018F8
		private void ApplyParameterInfo(SqlParameter parameter, int pcount, DBSchemaRow row)
		{
            parameter.SqlDbType = (SqlDbType)row.ProviderType;
			parameter.IsNullable = row.AllowDBNull;
			if ((byte)row.Precision != 255)
			{
				parameter.Precision = (byte)row.Precision;
			}
			if ((byte)row.Scale != 255)
			{
				parameter.Scale = (byte)row.Scale;
			}
			parameter.Size = 0;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000295C File Offset: 0x0000195C
		private IDbCommand BuildInsertCommand(DataTableMapping mappings, DataRow dataRow)
		{
			if (ADP.IsEmpty(this.quotedBaseTableName))
			{
				return null;
			}
			IDbCommand dbCommand = this.BuildNewCommand(this.insertCommand);
			int num = 0;
			int num2 = 1;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO ");
			stringBuilder.Append(this.QuotedBaseTableName);
			int num3 = this.dbSchemaRows.Length;
			for (int i = 0; i < num3; i++)
			{
				DBSchemaRow dbschemaRow = this.dbSchemaRows[i];
				if (dbschemaRow != null && dbschemaRow.BaseColumnName.Length != 0 && !dbschemaRow.IsAutoIncrement && !dbschemaRow.IsHidden && !dbschemaRow.IsExpression && !dbschemaRow.IsRowVersion)
				{
					object obj = null;
					string text = this.sourceColumnNames[i];
					if (mappings != null && dataRow != null)
					{
						obj = this.GetParameterInsertValue(text, mappings, dataRow, dbschemaRow.IsReadOnly);
						if (obj == null)
						{
							if (dbschemaRow.IsReadOnly)
							{
								goto IL_1B3;
							}
							if (dbCommand is SqlCommand)
							{
								goto IL_1B3;
							}
						}
						else if (Convert.IsDBNull(obj) && !dbschemaRow.AllowDBNull)
						{
							goto IL_1B3;
						}
					}
					if (num == 0)
					{
						stringBuilder.Append("( ");
					}
					else
					{
						stringBuilder.Append(" , ");
					}
					stringBuilder.Append(this.QuotedColumn(dbschemaRow.BaseColumnName));
					IDataParameter nextParameter = CommandBuilder.GetNextParameter(dbCommand, num);
					nextParameter.ParameterName = "@p" + num2.ToString();
					nextParameter.Direction = (ParameterDirection)1;
					nextParameter.SourceColumn = text;
					nextParameter.SourceVersion = (DataRowVersion)512;
					nextParameter.Value = obj;
					if (nextParameter is SqlParameter)
					{
						this.ApplyParameterInfo((SqlParameter)nextParameter, num2, dbschemaRow);
					}
					if (!dbCommand.Parameters.Contains(nextParameter))
					{
						dbCommand.Parameters.Add(nextParameter);
					}
					num++;
					num2++;
				}
				IL_1B3:;
			}
			if (num == 0)
			{
				stringBuilder.Append(" DEFAULT VALUES");
			}
			else if (this.namedParameters)
			{
				stringBuilder.Append(" ) VALUES ( @p1");
				for (int j = 2; j <= num; j++)
				{
					stringBuilder.Append(" , @p");
					stringBuilder.Append(j.ToString());
				}
				stringBuilder.Append(" )");
			}
			else
			{
				stringBuilder.Append(" ) VALUES ( ?");
				for (int k = 2; k <= num; k++)
				{
					stringBuilder.Append(" , ?");
				}
				stringBuilder.Append(" )");
			}
			dbCommand.CommandText = stringBuilder.ToString();
			CommandBuilder.RemoveExtraParameters(dbCommand, num);
			this.insertCommand = dbCommand;
			return dbCommand;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002BDC File Offset: 0x00001BDC
		private IDbCommand BuildUpdateCommand(DataTableMapping mappings, DataRow dataRow)
		{
			if (ADP.IsEmpty(this.quotedBaseTableName))
			{
				return null;
			}
			IDbCommand dbCommand = this.BuildNewCommand(this.updateCommand);
			int num = 1;
			int num2 = 0;
			int num3 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE ");
			stringBuilder.Append(this.QuotedBaseTableName);
			stringBuilder.Append(" SET ");
			int num4 = this.dbSchemaRows.Length;
			for (int i = 0; i < num4; i++)
			{
				DBSchemaRow dbschemaRow = this.dbSchemaRows[i];
				if (dbschemaRow != null && dbschemaRow.BaseColumnName.Length != 0 && !this.ExcludeFromUpdateSet(dbschemaRow))
				{
					num2++;
					object obj = null;
					string text = this.sourceColumnNames[i];
					if (mappings != null && dataRow != null)
					{
						obj = this.GetParameterUpdateValue(text, mappings, dataRow, dbschemaRow.IsReadOnly);
						if (obj == null)
						{
							if (dbschemaRow.IsReadOnly)
							{
								num2--;
								goto IL_18E;
							}
							goto IL_18E;
						}
					}
					if (0 < num3)
					{
						stringBuilder.Append(" , ");
					}
					stringBuilder.Append(this.QuotedColumn(dbschemaRow.BaseColumnName));
					this.AppendParameterText(stringBuilder, num);
					IDataParameter nextParameter = CommandBuilder.GetNextParameter(dbCommand, num3);
					nextParameter.ParameterName = "@p" + num.ToString();
                    nextParameter.Direction = (ParameterDirection)1;
					nextParameter.SourceColumn = text;
                    nextParameter.SourceVersion = (DataRowVersion)512;
					nextParameter.Value = obj;
					if (nextParameter is SqlParameter)
					{
						this.ApplyParameterInfo((SqlParameter)nextParameter, num, dbschemaRow);
					}
					if (!dbCommand.Parameters.Contains(nextParameter))
					{
						dbCommand.Parameters.Add(nextParameter);
					}
					num++;
					num3++;
				}
				IL_18E:;
			}
			int num5 = num3;
			stringBuilder.Append(" WHERE ( ");
			string text2 = "";
			int num6 = 0;
			string text3 = null;
			string text4 = null;
			for (int j = 0; j < num4; j++)
			{
				DBSchemaRow dbschemaRow2 = this.dbSchemaRows[j];
				if (dbschemaRow2 != null && dbschemaRow2.BaseColumnName.Length != 0 && this.IncludeForUpdateWhereClause(dbschemaRow2))
				{
					stringBuilder.Append(text2);
					text2 = " AND ";
					object obj2 = null;
					string text5 = this.sourceColumnNames[j];
					if (mappings != null && dataRow != null)
					{
                        obj2 = this.GetParameterValue(text5, mappings, dataRow, (DataRowVersion)256);
					}
					bool flag = this.IsPKey(dbschemaRow2);
					string text6 = this.QuotedColumn(dbschemaRow2.BaseColumnName);
					if (flag)
					{
						if (Convert.IsDBNull(obj2))
						{
							stringBuilder.Append(string.Format("({0} IS NULL)", text6));
						}
						else if (this.namedParameters)
						{
							stringBuilder.Append(string.Format("({0} = @p{1})", text6, num));
						}
						else
						{
							stringBuilder.Append(string.Format("({0} = ?)", text6));
						}
					}
					else if (this.namedParameters)
					{
						string text7 = string.Format("(({0} IS NULL AND @p{1} IS NULL) OR ({0} = ", text6, num);
						text7 += string.Format("@p{0}))", 1 + num);
						stringBuilder.Append(text7);
					}
					else
					{
						stringBuilder.Append(string.Format("((? IS NULL AND {0} IS NULL) OR ({0} = ?))", text6));
					}
					if (!flag || !Convert.IsDBNull(obj2))
					{
						IDataParameter nextParameter2 = CommandBuilder.GetNextParameter(dbCommand, num3);
						nextParameter2.ParameterName = "@p" + num.ToString();
						nextParameter2.Direction = (ParameterDirection)1;
						nextParameter2.SourceColumn = text5;
						nextParameter2.SourceVersion = (DataRowVersion)256;
						nextParameter2.Value = obj2;
						num++;
						num3++;
						if (nextParameter2 is SqlParameter)
						{
							this.ApplyParameterInfo((SqlParameter)nextParameter2, num, dbschemaRow2);
						}
						if (!dbCommand.Parameters.Contains(nextParameter2))
						{
							dbCommand.Parameters.Add(nextParameter2);
						}
					}
					if (!flag)
					{
						IDataParameter nextParameter3 = CommandBuilder.GetNextParameter(dbCommand, num3);
						nextParameter3.ParameterName = "@p" + num.ToString();
						nextParameter3.Direction = (ParameterDirection)1;
						nextParameter3.SourceColumn = text5;
                        nextParameter3.SourceVersion = (DataRowVersion)256;
						nextParameter3.Value = obj2;
						num++;
						num3++;
						if (nextParameter3 is SqlParameter)
						{
							this.ApplyParameterInfo((SqlParameter)nextParameter3, num, dbschemaRow2);
						}
						if (!dbCommand.Parameters.Contains(nextParameter3))
						{
							dbCommand.Parameters.Add(nextParameter3);
						}
					}
					if (this.IncrementUpdateWhereCount(dbschemaRow2))
					{
						num6++;
					}
				}
			}
			stringBuilder.Append(" )");
			dbCommand.CommandText = stringBuilder.ToString();
			CommandBuilder.RemoveExtraParameters(dbCommand, num3);
			this.updateCommand = dbCommand;
			if (num2 == 0)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLReadOnly", new object[]
				{
					"UpdateCommand"
				}));
			}
			if (num5 == 0)
			{
				dbCommand = null;
			}
			if (num6 == 0)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNoKeyInfo", new object[]
				{
					"UpdateCommand"
				}));
			}
			if (text3 != null)
			{
				DataColumn parameterDataColumn = this.GetParameterDataColumn(text4, mappings, dataRow);
				throw new InvalidOperationException(Res.GetString("ADP_WhereClauseUnspecifiedValue", new object[]
				{
					text3,
					text4,
					parameterDataColumn.ColumnName
				}));
			}
			return dbCommand;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000030E4 File Offset: 0x000020E4
		private IDbCommand BuildDeleteCommand(DataTableMapping mappings, DataRow dataRow)
		{
			if (ADP.IsEmpty(this.quotedBaseTableName))
			{
				return null;
			}
			IDbCommand dbCommand = this.BuildNewCommand(this.deleteCommand);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DELETE FROM  ");
			stringBuilder.Append(this.QuotedBaseTableName);
			stringBuilder.Append(" WHERE ( ");
			int num = 1;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			string text2 = null;
			string text3 = null;
			int num4 = this.dbSchemaRows.Length;
			for (int i = 0; i < num4; i++)
			{
				DBSchemaRow dbschemaRow = this.dbSchemaRows[i];
				if (dbschemaRow != null && dbschemaRow.BaseColumnName.Length != 0 && this.IncludeForDeleteWhereClause(dbschemaRow))
				{
					stringBuilder.Append(text);
					text = " AND ";
					object obj = null;
					string text4 = this.sourceColumnNames[i];
					if (mappings != null && dataRow != null)
					{
                        obj = this.GetParameterValue(text4, mappings, dataRow, (DataRowVersion)256);
					}
					bool flag = this.IsPKey(dbschemaRow);
					string text5 = this.QuotedColumn(dbschemaRow.BaseColumnName);
					if (flag)
					{
						if (Convert.IsDBNull(obj))
						{
							stringBuilder.Append(string.Format("({0} IS NULL)", text5));
						}
						else if (this.namedParameters)
						{
							stringBuilder.Append(string.Format("({0} = @p{1})", text5, num));
						}
						else
						{
							stringBuilder.Append(string.Format("({0} = ?)", text5));
						}
					}
					else if (this.namedParameters)
					{
						string text6 = string.Format("(({0} IS NULL AND @p{1} IS NULL) OR ({0} = ", text5, num);
						text6 += string.Format("@p{0}))", 1 + num);
						stringBuilder.Append(text6);
					}
					else
					{
						stringBuilder.Append(string.Format("((? IS NULL AND {0} IS NULL) OR ({0} = ?))", text5));
					}
					if (!flag || !Convert.IsDBNull(obj))
					{
						IDataParameter nextParameter = CommandBuilder.GetNextParameter(dbCommand, num2);
						nextParameter.ParameterName = "@p" + num.ToString();
						nextParameter.Direction = (ParameterDirection)1;
						nextParameter.SourceColumn = text4;
                        nextParameter.SourceVersion = (DataRowVersion)256;
						nextParameter.Value = obj;
						num++;
						num2++;
						if (nextParameter is SqlParameter)
						{
							this.ApplyParameterInfo((SqlParameter)nextParameter, num, dbschemaRow);
						}
						if (!dbCommand.Parameters.Contains(nextParameter))
						{
							dbCommand.Parameters.Add(nextParameter);
						}
					}
					if (!flag)
					{
						IDataParameter nextParameter2 = CommandBuilder.GetNextParameter(dbCommand, num2);
						nextParameter2.ParameterName = "@p" + num.ToString();
						nextParameter2.Direction = (ParameterDirection)1;
						nextParameter2.SourceColumn = text4;
						nextParameter2.SourceVersion = (DataRowVersion)256;
						nextParameter2.Value = obj;
						num++;
						num2++;
						if (nextParameter2 is SqlParameter)
						{
							this.ApplyParameterInfo((SqlParameter)nextParameter2, num, dbschemaRow);
						}
						if (!dbCommand.Parameters.Contains(nextParameter2))
						{
							dbCommand.Parameters.Add(nextParameter2);
						}
					}
					if (this.IncrementDeleteWhereCount(dbschemaRow))
					{
						num3++;
					}
				}
			}
			stringBuilder.Append(" )");
			dbCommand.CommandText = stringBuilder.ToString();
			CommandBuilder.RemoveExtraParameters(dbCommand, num2);
			this.deleteCommand = dbCommand;
			if (num3 == 0)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DynamicSQLNoKeyInfo", new object[]
				{
					"DeleteCommand"
				}));
			}
			if (text2 != null)
			{
				DataColumn parameterDataColumn = this.GetParameterDataColumn(text3, mappings, dataRow);
				throw new InvalidOperationException(Res.GetString("ADP_WhereClauseUnspecifiedValue", new object[]
				{
					text2,
					text3,
					parameterDataColumn.ColumnName
				}));
			}
			return dbCommand;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000345D File Offset: 0x0000245D
		private bool ExcludeFromUpdateSet(DBSchemaRow row)
		{
			return row.IsAutoIncrement || row.IsRowVersion || row.IsHidden;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003478 File Offset: 0x00002478
		private bool IncludeForUpdateWhereClause(DBSchemaRow row)
		{
			if (this.IsBehavior(CommandBuilderBehavior.UseRowVersionInUpdateWhereClause))
			{
				return (row.IsRowVersion || row.IsKey || row.IsUnique) && !row.IsLong && !row.IsHidden;
			}
			return (this.IsNotBehavior(CommandBuilderBehavior.PrimaryKeyOnlyUpdateWhereClause) || row.IsKey || row.IsUnique) && !row.IsLong && (row.IsKey || !row.IsRowVersion) && !row.IsHidden;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000034F8 File Offset: 0x000024F8
		private bool IncludeForDeleteWhereClause(DBSchemaRow row)
		{
			if (this.IsBehavior(CommandBuilderBehavior.UseRowVersionInDeleteWhereClause))
			{
				return (row.IsRowVersion || row.IsKey || row.IsUnique) && !row.IsLong && !row.IsHidden;
			}
			return (this.IsNotBehavior(CommandBuilderBehavior.PrimaryKeyOnlyDeleteWhereClause) || row.IsKey || row.IsUnique) && !row.IsLong && (row.IsKey || !row.IsRowVersion) && !row.IsHidden;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003577 File Offset: 0x00002577
		private bool IsPKey(DBSchemaRow row)
		{
			return row.IsKey;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000357F File Offset: 0x0000257F
		private bool IncrementUpdateWhereCount(DBSchemaRow row)
		{
			return row.IsKey || row.IsUnique;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003591 File Offset: 0x00002591
		private bool IncrementDeleteWhereCount(DBSchemaRow row)
		{
			return row.IsKey || row.IsUnique;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000035A4 File Offset: 0x000025A4
		private DataColumn GetParameterDataColumn(string columnName, DataTableMapping mappings, DataRow row)
		{
			if (!ADP.IsEmpty(columnName))
			{
				DataColumnMapping columnMappingBySchemaAction = mappings.GetColumnMappingBySchemaAction(columnName, this.missingMapping);
				if (columnMappingBySchemaAction != null)
				{
					return columnMappingBySchemaAction.GetDataColumnBySchemaAction(row.Table, null, (MissingSchemaAction)3);
				}
			}
			return null;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000035DC File Offset: 0x000025DC
		private object GetParameterValue(string columnName, DataTableMapping mappings, DataRow row, DataRowVersion version)
		{
			DataColumn parameterDataColumn = this.GetParameterDataColumn(columnName, mappings, row);
			if (parameterDataColumn != null)
			{
				return row[parameterDataColumn, version];
			}
			return null;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003604 File Offset: 0x00002604
		private object GetParameterInsertValue(string columnName, DataTableMapping mappings, DataRow row, bool readOnly)
		{
			DataColumn parameterDataColumn = this.GetParameterDataColumn(columnName, mappings, row);
			if (parameterDataColumn == null)
			{
				return null;
			}
			if (readOnly && parameterDataColumn.ReadOnly)
			{
				return null;
			}
            return row[parameterDataColumn, (DataRowVersion)512];
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000363C File Offset: 0x0000263C
		private object GetParameterUpdateValue(string columnName, DataTableMapping mappings, DataRow row, bool readOnly)
		{
			DataColumn parameterDataColumn = this.GetParameterDataColumn(columnName, mappings, row);
			if (parameterDataColumn != null)
			{
				if (readOnly && parameterDataColumn.ReadOnly)
				{
					return null;
				}
                object obj = row[parameterDataColumn, (DataRowVersion)512];
				if (!this.IsNotBehavior(CommandBuilderBehavior.UpdateSetSameValue))
				{
					return obj;
				}
                object obj2 = row[parameterDataColumn, (DataRowVersion)256];
				if (obj2 != obj && (obj2 == null || !obj2.Equals(obj)))
				{
					return obj;
				}
			}
			return null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000369C File Offset: 0x0000269C
		private void AppendParameterText(StringBuilder builder, int pcount)
		{
			if (this.namedParameters)
			{
				builder.Append(" = @p");
				builder.Append(pcount.ToString());
				return;
			}
			builder.Append(" = ?");
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000036D0 File Offset: 0x000026D0
		private static IDataParameter GetNextParameter(IDbCommand cmd, int pcount)
		{
			if (pcount < cmd.Parameters.Count)
			{
				return (IDataParameter)cmd.Parameters[pcount];
			}
			return cmd.CreateParameter();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003705 File Offset: 0x00002705
		private static void RemoveExtraParameters(IDbCommand cmd, int pcount)
		{
			while (pcount < cmd.Parameters.Count)
			{
				cmd.Parameters.RemoveAt(cmd.Parameters.Count - 1);
			}
		}

		// Token: 0x0400000A RID: 10
		private const string WhereClause1 = "(({0} IS NULL AND @p{1} IS NULL) OR ({0} = @p{2}))";

		// Token: 0x0400000B RID: 11
		private const string WhereClause2 = "((? IS NULL AND {0} IS NULL) OR ({0} = ?))";

		// Token: 0x0400000C RID: 12
		private const string WhereClause1_beg = "(({0} IS NULL AND @p{1} IS NULL) OR ({0} = ";

		// Token: 0x0400000D RID: 13
		private const string WhereClause1_end = "@p{0}))";

		// Token: 0x0400000E RID: 14
		private const string WhereClause1p = "({0} = @p{1})";

		// Token: 0x0400000F RID: 15
		private const string WhereClause2p = "({0} = ?)";

		// Token: 0x04000010 RID: 16
		private const string WhereClausepn = "({0} IS NULL)";

		// Token: 0x04000011 RID: 17
		private const string AndClause = " AND ";

		// Token: 0x04000012 RID: 18
        private const MissingSchemaAction missingSchema = MissingSchemaAction.Error;

		// Token: 0x04000013 RID: 19
		private IDbDataAdapter adapter;

		// Token: 0x04000014 RID: 20
		private MissingMappingAction missingMapping;

		// Token: 0x04000015 RID: 21
		private CommandBuilderBehavior options;

		// Token: 0x04000016 RID: 22
		private SqlRowUpdatingEventHandler sqlhandler;

		// Token: 0x04000017 RID: 23
		private DataTable dbSchemaTable;

		// Token: 0x04000018 RID: 24
		private DBSchemaRow[] dbSchemaRows;

		// Token: 0x04000019 RID: 25
		private string[] sourceColumnNames;

		// Token: 0x0400001A RID: 26
		private string quotePrefix;

		// Token: 0x0400001B RID: 27
		private string quoteSuffix;

		// Token: 0x0400001C RID: 28
		private bool namedParameters;

		// Token: 0x0400001D RID: 29
		private string quotedBaseTableName;

		// Token: 0x0400001E RID: 30
		private IDbCommand insertCommand;

		// Token: 0x0400001F RID: 31
		private IDbCommand updateCommand;

		// Token: 0x04000020 RID: 32
		private IDbCommand deleteCommand;
	}
}
