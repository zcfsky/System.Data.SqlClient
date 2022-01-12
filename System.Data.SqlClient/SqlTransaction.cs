using System;

namespace System.Data.SqlClient
{
	/// <summary>表示要在 SQL Server 数据库中处理的 Transact-SQL 事务。无法继承此类。</summary>
	// Token: 0x0200002B RID: 43
	public sealed class SqlTransaction : MarshalByRefObject, IDbTransaction, IDisposable
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x0000B64A File Offset: 0x0000A64A
		internal SqlTransaction(SqlConnection connection, IsolationLevel isoLevel)
		{
			this._sqlConnection = connection;
			this._sqlConnection.LocalTransaction = this;
			this._isolationLevel = isoLevel;
		}

		/// <summary>获取与该事务关联的 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 对象，或者如果该事务不再有效，则为null。</summary>
		/// <returns>与该事务关联的 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 对象。</returns>
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000B677 File Offset: 0x0000A677
		public SqlConnection Connection
		{
			get
			{
				return this._sqlConnection;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000B67F File Offset: 0x0000A67F
		IDbConnection IDbTransaction.Connection
		{
			get
			{
				return this.Connection;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B687 File Offset: 0x0000A687
		internal void Zombie()
		{
			this._sqlConnection.LocalTransaction = null;
			this._sqlConnection = null;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B69C File Offset: 0x0000A69C
		private int GetServerTransactionLevel()
		{
			if (this._transactionLevelCommand == null)
			{
				this._transactionLevelCommand = new SqlCommand("set @out = @@trancount", this._sqlConnection);
				this._transactionLevelCommand.Transaction = this;
				SqlParameter sqlParameter = new SqlParameter("@out", 8);
                sqlParameter.Direction = (ParameterDirection)2;
				this._transactionLevelCommand.Parameters.Add(sqlParameter);
			}
			this._transactionLevelCommand.ExecuteReader(0, RunBehavior.UntilDone, false);
			return (int)this._transactionLevelCommand.Parameters[0].Value;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B722 File Offset: 0x0000A722
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B731 File Offset: 0x0000A731
		private void Dispose(bool disposing)
		{
			if (disposing && this._sqlConnection != null)
			{
				this._disposing = true;
				this.Rollback();
			}
		}

		/// <summary>指定该事务的 <see cref="T:System.Data.IsolationLevel"></see>。</summary>
		/// <returns>该事务的 <see cref="T:System.Data.IsolationLevel"></see>。默认为 ReadCommitted。</returns>
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000B74C File Offset: 0x0000A74C
		public IsolationLevel IsolationLevel
		{
			get
			{
				if (this._sqlConnection == null)
				{
					throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", new object[]
					{
						base.GetType().Name
					}));
				}
				return this._isolationLevel;
			}
		}

		/// <summary>提交数据库事务。</summary>
		// Token: 0x060001FC RID: 508 RVA: 0x0000B790 File Offset: 0x0000A790
		public void Commit()
		{
			if (this._sqlConnection == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", new object[]
				{
					base.GetType().Name
				}));
			}
			try
			{
				this._sqlConnection.ExecuteTransaction("COMMIT TRANSACTION", "CommitTransaction");
				this.Zombie();
			}
			catch (Exception ex)
			{
				if (this._sqlConnection != null && this.GetServerTransactionLevel() == 0)
				{
					this.Zombie();
				}
				throw ex;
			}
		}

		/// <summary>从挂起状态回滚事务。</summary>
		// Token: 0x060001FD RID: 509 RVA: 0x0000B814 File Offset: 0x0000A814
		public void Rollback()
		{
			if (this._sqlConnection == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", new object[]
				{
					base.GetType().Name
				}));
			}
			try
			{
				this._sqlConnection.ExecuteTransaction("ROLLBACK TRANSACTION", "RollbackTransaction");
				this.Zombie();
			}
			catch (Exception ex)
			{
				if (this._sqlConnection != null && this.GetServerTransactionLevel() == 0)
				{
					this.Zombie();
				}
				if (!this._disposing)
				{
					throw ex;
				}
			}
		}

		/// <summary>从挂起状态回滚事务，并指定事务或保存点名称。</summary>
		/// <param name="transactionName">要回滚的事务的名称，或要回滚到的保存点的名称。 </param>
		// Token: 0x060001FE RID: 510 RVA: 0x0000B8A0 File Offset: 0x0000A8A0
		public void Rollback(string transactionName)
		{
			if (this._sqlConnection == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", new object[]
				{
					base.GetType().Name
				}));
			}
			if (ADP.IsEmpty(transactionName))
			{
				throw SQL.NullEmptyTransactionName();
			}
			try
			{
				this._sqlConnection.ExecuteTransaction("ROLLBACK TRANSACTION " + transactionName, "RollbackTransaction");
				if (this.GetServerTransactionLevel() == 0)
				{
					this.Zombie();
				}
			}
			catch (Exception ex)
			{
				if (this._sqlConnection != null && this.GetServerTransactionLevel() == 0)
				{
					this.Zombie();
				}
				throw ex;
			}
		}

		/// <summary>在事务中创建保存点（它可用于回滚事务的一部分），并指定保存点名称。</summary>
		/// <param name="savePointName">保存点的名称。 </param>
		// Token: 0x060001FF RID: 511 RVA: 0x0000B940 File Offset: 0x0000A940
		public void Save(string savePointName)
		{
			if (this._sqlConnection == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", new object[]
				{
					base.GetType().Name
				}));
			}
			if (ADP.IsEmpty(savePointName))
			{
				throw SQL.NullEmptyTransactionName();
			}
			try
			{
				this._sqlConnection.ExecuteTransaction("SAVE TRANSACTION " + savePointName, "SaveTransaction");
			}
			catch (Exception ex)
			{
				if (this._sqlConnection != null && this.GetServerTransactionLevel() == 0)
				{
					this.Zombie();
				}
				throw ex;
			}
		}

		// Token: 0x04000113 RID: 275
        private IsolationLevel _isolationLevel = (IsolationLevel)4096;

		// Token: 0x04000114 RID: 276
		internal SqlConnection _sqlConnection;

		// Token: 0x04000115 RID: 277
		private SqlCommand _transactionLevelCommand;

		// Token: 0x04000116 RID: 278
		private bool _disposing;
	}
}
