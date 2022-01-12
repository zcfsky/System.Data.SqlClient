using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data.SqlClient
{
	/// <summary>表示 SQL Server 数据库的一个打开的连接。无法继承此类。 </summary>
	// Token: 0x02000008 RID: 8
	public sealed class SqlConnection : Component, IDbConnection, ICloneable, IDisposable
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 类的新实例。</summary>
		// Token: 0x0600008A RID: 138 RVA: 0x0000622D File Offset: 0x0000522D
		public SqlConnection()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>如果给定包含连接字符串的字符串，则初始化 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 类的新实例。</summary>
		/// <param name="connectionString">用于打开 SQL Server 数据库的连接。 </param>
		// Token: 0x0600008B RID: 139 RVA: 0x0000623B File Offset: 0x0000523B
		public SqlConnection(string connectionString)
		{
			GC.SuppressFinalize(this);
			this.ConnectionString = connectionString;
		}

		/// <summary>获取或设置用于打开 SQL Server 数据库的字符串。</summary>
		/// <returns>连接字符串，其中包含源数据库名称和建立初始连接所需的其他参数。默认值为空字符串。</returns>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00006250 File Offset: 0x00005250
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00006280 File Offset: 0x00005280
		public string ConnectionString
		{
			get
			{
				if (this._connectionString == null)
				{
					return "";
				}
				if (this._fShowPassword)
				{
					return this._connectionString;
				}
				return ConStringUtil.RemoveKeyValuesFromString(this._connectionString, "password");
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				switch (this._objectState)
				{
				case 0:
					try
					{
						Hashtable hashtable = SqlConnection.s_connectionStringCacheHashtable;
						if (hashtable == null)
						{
							lock (typeof(SqlConnection))
							{
								hashtable = SqlConnection.s_connectionStringCacheHashtable;
								if (hashtable == null)
								{
									hashtable = new Hashtable();
									SqlConnection.s_connectionStringCacheHashtable = hashtable;
								}
							}
						}
						if (hashtable.Contains(value))
						{
							Hashtable connectionOptions = (Hashtable)hashtable[value];
							this._connectionOptions = connectionOptions;
						}
						else
						{
							this._connectionOptions = ConStringUtil.ParseConnectionString(value);
							lock (hashtable.SyncRoot)
							{
								if (hashtable.Count >= 250)
								{
									SqlConnection.s_connectionStringCacheHashtable = null;
								}
							}
						}
						this._connectionString = value;
						this._fShowPassword = true;
						return;
					}
					finally
					{
						if (this._connectionOptions == null)
						{
							this._connectionOptions = null;
							this._cachedOptions = null;
							this._connectionString = null;
						}
					}
					break;
				case (ConnectionState)1:
					break;
				default:
					return;
				}
				throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionPropertySet", new object[]
				{
					"ConnectionString",
					this.State.ToString()
				}));
			}
		}

		/// <summary>获取在尝试建立连接时终止尝试并生成错误之前所等待的时间。</summary>
		/// <returns>等待连接打开的时间（以秒为单位）。默认值为 15 秒。</returns>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000063C8 File Offset: 0x000053C8
		public int ConnectionTimeout
		{
			get
			{
				return (int)this.GetOptions()["connect timeout"];
			}
		}

		/// <summary>获取当前数据库或连接打开后要使用的数据库的名称。</summary>
		/// <returns>当前数据库的名称或连接打开后要使用的数据库的名称。默认值为空字符串。</returns>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000063DF File Offset: 0x000053DF
		public string Database
		{
			get
			{
				return (string)this.GetOptions()["initial catalog"];
			}
		}

		/// <summary>获取要连接的 SQL Server 实例的名称。</summary>
		/// <returns>要连接的 SQL Server 实例的名称。默认值为空字符串。</returns>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000063F6 File Offset: 0x000053F6
		public string DataSource
		{
			get
			{
				return (string)this.GetOptions()["data source"];
			}
		}

		/// <summary>获取用来与 SQL Server 的实例通信的网络数据包的大小（以字节为单位）。</summary>
		/// <returns>网络数据包的大小（以字节为单位）。默认值为 8192。</returns>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000640D File Offset: 0x0000540D
		public int PacketSize
		{
			get
			{
				return (int)this.GetOptions()["packet size"];
			}
		}

		/// <summary>获取标识数据库客户端的一个字符串。</summary>
		/// <returns>标识数据库客户端的一个字符串。如果没有指定，则为客户端计算机的名称。如果两个都没有指定，则为空字符串。</returns>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00006424 File Offset: 0x00005424
		public string WorkstationId
		{
			get
			{
				if (this._connectionOptions != null)
				{
					return (string)this._connectionOptions["workstation id"];
				}
				return ConStringUtil.WORKSTATION_ID;
			}
		}

		/// <summary>当 SQL Server 返回一个警告或信息性消息时发生。</summary>
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000093 RID: 147 RVA: 0x00006449 File Offset: 0x00005449
		// (remove) Token: 0x06000094 RID: 148 RVA: 0x00006462 File Offset: 0x00005462
		public event SqlInfoMessageEventHandler InfoMessage
		{
			add
			{
				this._infoMessageEventHandler = (SqlInfoMessageEventHandler)Delegate.Combine(this._infoMessageEventHandler, value);
			}
			remove
			{
				this._infoMessageEventHandler = (SqlInfoMessageEventHandler)Delegate.Remove(this._infoMessageEventHandler, value);
			}
		}

		/// <summary>获取包含客户端连接的 SQL Server 实例的版本的字符串。</summary>
		/// <returns>SQL Server 实例的版本。</returns>
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000647C File Offset: 0x0000547C
		public string ServerVersion
		{
			get
			{
				switch (this._objectState)
				{
                    case (ConnectionState)0:
					throw new InvalidOperationException(Res.GetString("ADP_ClosedConnectionError"));
				case (ConnectionState)1:
					return this._internalConnection.ServerVersion;
				default:
					return null;
				}
			}
		}

		/// <summary>指示 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 的状态。</summary>
		/// <returns><see cref="T:System.Data.ConnectionState"></see> 枚举。</returns>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000064BD File Offset: 0x000054BD
		public ConnectionState State
		{
			get
			{
				return this._objectState;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000097 RID: 151 RVA: 0x000064C5 File Offset: 0x000054C5
		// (remove) Token: 0x06000098 RID: 152 RVA: 0x000064DE File Offset: 0x000054DE
		public event StateChangeEventHandler StateChange
		{
			add
			{
				this._stateChangeEventHandler = (StateChangeEventHandler)Delegate.Combine(this._stateChangeEventHandler, value);
			}
			remove
			{
				this._stateChangeEventHandler = (StateChangeEventHandler)Delegate.Remove(this._stateChangeEventHandler, value);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000064F7 File Offset: 0x000054F7
		internal bool IsClosing
		{
			get
			{
				return this._fIsClosing;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000064FF File Offset: 0x000054FF
		internal bool IsShiloh
		{
			get
			{
				return this._internalConnection.IsShiloh;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000650C File Offset: 0x0000550C
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00006548 File Offset: 0x00005548
		internal SqlTransaction LocalTransaction
		{
			get
			{
				if (this._localTransaction != null)
				{
					SqlTransaction sqlTransaction = (SqlTransaction)this._localTransaction.Target;
					if (sqlTransaction != null && this._localTransaction.IsAlive)
					{
						return sqlTransaction;
					}
				}
				return null;
			}
			set
			{
				this._localTransaction = null;
				if (value != null)
				{
					if (this._internalConnection != null)
					{
						this._internalConnection.InLocalTransaction = true;
					}
					this._localTransaction = new WeakReference(value);
					return;
				}
				if (this._internalConnection != null)
				{
					this._internalConnection.InLocalTransaction = false;
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00006594 File Offset: 0x00005594
		internal TdsParser Parser
		{
			get
			{
				if (this._internalConnection != null)
				{
					return this._internalConnection.Parser;
				}
				return null;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000065AC File Offset: 0x000055AC
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000065E5 File Offset: 0x000055E5
		internal SqlDataReader Reader
		{
			get
			{
				if (this._reader != null)
				{
					SqlDataReader sqlDataReader = (SqlDataReader)this._reader.Target;
					if (sqlDataReader != null && this._reader.IsAlive)
					{
						return sqlDataReader;
					}
				}
				return null;
			}
			set
			{
				this._reader = null;
				if (value != null)
				{
					this._reader = new WeakReference(value);
				}
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000065FD File Offset: 0x000055FD
		private Hashtable GetOptions()
		{
			if (this._connectionOptions == null)
			{
				this._connectionOptions = ConStringUtil.DefaultConnectionOptionsTable();
			}
			return this._connectionOptions;
		}

		/// <summary>使用 <see cref="P:System.Data.SqlClient.SqlConnection.ConnectionString"></see> 所指定的属性设置打开数据库连接。</summary>
		// Token: 0x060000A1 RID: 161 RVA: 0x00006618 File Offset: 0x00005618
		public void Open()
		{
			switch ((int)this._objectState)
			{
			case 0:
				if (ADP.IsEmpty(this._connectionString))
				{
					throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
				}
				try
				{
					this._cachedOptions = this._connectionOptions;
					this._internalConnection = new SqlInternalConnection(this, this._connectionOptions);
					this._objectState = (ConnectionState)1;
					if (!(bool)this._connectionOptions["persist security info"])
					{
						this._fShowPassword = false;
					}
				}
				catch (Exception ex)
				{
					if (this._internalConnection != null)
					{
						if (this._cachedOptions != null)
						{
							this._connectionOptions = this._cachedOptions;
							this._cachedOptions = null;
						}
						this._internalConnection.Close();
						this._internalConnection = null;
						this._objectState = 0;
					}
					throw ex;
				}
				this.FireObjectState((ConnectionState)0, (ConnectionState)1);
				return;
			case 1:
				throw new InvalidOperationException(Res.GetString("ADP_ConnectionAlreadyOpen", new object[]
				{
					this._objectState.ToString()
				}));
			default:
				return;
			}
		}

		/// <summary>关闭与数据库的连接。这是关闭任何打开连接的首选方法。</summary>
		// Token: 0x060000A2 RID: 162 RVA: 0x00006720 File Offset: 0x00005720
		public void Close()
		{
			switch ((int)this._objectState)
			{
			case 0:
				break;
			case 1:
				this.CloseReader();
				if (1 == (int)this._objectState)
				{
					try
					{
						this._fIsClosing = true;
						if (this._preparedCommands != null)
						{
							while (this._preparedCommands.Count > 0)
							{
								SqlCommand sqlCommand = (SqlCommand)this._preparedCommands[0];
								if (sqlCommand.IsPrepared)
								{
									sqlCommand.Unprepare();
								}
							}
							this._preparedCommands = null;
						}
						try
						{
							this._connectionOptions = this._cachedOptions;
							this._cachedOptions = null;
							if (this.LocalTransaction != null)
							{
								this.LocalTransaction.Zombie();
							}
							this._internalConnection.Close();
							this._internalConnection = null;
							this._fIsClosing = false;
						}
						catch (Exception ex)
						{
							this._internalConnection = null;
							this._fIsClosing = false;
							throw ex;
						}
					}
					finally
					{
						this._objectState = 0;
					}
					this.FireObjectState((ConnectionState)1, (ConnectionState)0);
				}
				break;
			default:
				return;
			}
		}

		/// <summary>开始数据库事务。</summary>
		/// <returns>表示新事务的对象。</returns>
		// Token: 0x060000A3 RID: 163 RVA: 0x0000681C File Offset: 0x0000581C
		public SqlTransaction BeginTransaction()
		{
			return this.BeginTransaction((IsolationLevel)4096);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006829 File Offset: 0x00005829
		IDbTransaction IDbConnection.BeginTransaction()
		{
			return this.BeginTransaction();
		}

		/// <summary>以指定的隔离级别启动数据库事务。</summary>
		/// <returns>表示新事务的对象。</returns>
		/// <param name="iso">事务应在其下运行的隔离级别。 </param>
		// Token: 0x060000A5 RID: 165 RVA: 0x00006834 File Offset: 0x00005834
		public SqlTransaction BeginTransaction(IsolationLevel iso)
		{
			if (this._objectState == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_ClosedConnectionError"));
			}
			this.CloseDeadReader();
			this.RollbackDeadTransaction();
			if (this.LocalTransaction != null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_ParallelTransactionsNotSupported", new object[]
				{
					base.GetType().Name
				}));
			}
			string text;
			if ((int)iso <= 4096)
			{
				if ((int)iso == 256)
				{
					text = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
					goto IL_A8;
				}
				if ((int)iso == 4096)
				{
					text = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
					goto IL_A8;
				}
			}
			else
			{
				if ((int)iso == 65536)
				{
					text = "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ";
					goto IL_A8;
				}
				if ((int)iso == 1048576)
				{
					text = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";
					goto IL_A8;
				}
			}
			throw SQL.InvalidIsolationLevelPropertyArg();
			IL_A8:
			this._internalConnection.ExecuteTransaction(text + ";BEGIN TRANSACTION", "BeginTransaction");
			return new SqlTransaction(this, iso);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000690B File Offset: 0x0000590B
		IDbTransaction IDbConnection.BeginTransaction(IsolationLevel iso)
		{
			return this.BeginTransaction(iso);
		}

		/// <summary>以指定的事务名称启动数据库事务。</summary>
		/// <returns>表示新事务的对象。</returns>
		/// <param name="transactionName">事务的名称。 </param>
		// Token: 0x060000A7 RID: 167 RVA: 0x00006914 File Offset: 0x00005914
		public SqlTransaction BeginTransaction(string transactionName)
		{
			return this.BeginTransaction((IsolationLevel)4096, transactionName);
		}

		/// <summary>以指定的隔离级别和事务名称启动数据库事务。</summary>
		/// <returns>表示新事务的对象。</returns>
		/// <param name="iso">事务应在其下运行的隔离级别。 </param>
		/// <param name="transactionName">事务的名称。 </param>
		// Token: 0x060000A8 RID: 168 RVA: 0x00006924 File Offset: 0x00005924
		public SqlTransaction BeginTransaction(IsolationLevel iso, string transactionName)
		{
			if (this._objectState == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_ClosedConnectionError"));
			}
			this.CloseDeadReader();
			this.RollbackDeadTransaction();
			if (this.LocalTransaction != null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_ParallelTransactionsNotSupported", new object[]
				{
					base.GetType().Name
				}));
			}
			if (ADP.IsEmpty(transactionName))
			{
				throw SQL.NullEmptyTransactionName();
			}
			string text;
			if (iso <= (IsolationLevel)4096)
			{
                if (iso == (IsolationLevel)256)
				{
					text = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
					goto IL_B6;
				}
                if (iso == (IsolationLevel)4096)
				{
					text = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
					goto IL_B6;
				}
			}
			else
			{
                if (iso == (IsolationLevel)65536)
				{
					text = "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ";
					goto IL_B6;
				}
                if (iso == (IsolationLevel)1048576)
				{
					text = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";
					goto IL_B6;
				}
			}
			throw SQL.InvalidIsolationLevelPropertyArg();
			IL_B6:
			this._internalConnection.ExecuteTransaction(text + ";BEGIN TRANSACTION " + transactionName, "BeginTransaction");
			return new SqlTransaction(this, iso);
		}

		/// <summary>为打开的 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 更改当前数据库。</summary>
		/// <param name="database">要代替当前数据库加以使用的数据库的名称。 </param>
		// Token: 0x060000A9 RID: 169 RVA: 0x00006A0C File Offset: 0x00005A0C
		public void ChangeDatabase(string database)
		{
			if (1 != (int)this.State)
			{
				throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionRequired_ChangeDatabase", new object[]
				{
					this.State.ToString()
				}));
			}
			if (database == null || database.Trim().Length == 0)
			{
				throw new ArgumentException(Res.GetString("ADP_EmptyDatabaseName"));
			}
			this.CloseDeadReader();
			this.RollbackDeadTransaction();
			this._internalConnection.ChangeDatabase(database);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006A88 File Offset: 0x00005A88
		object ICloneable.Clone()
		{
			SqlConnection sqlConnection = new SqlConnection();
			if (1 == (int)this.State)
			{
				sqlConnection._connectionOptions = (Hashtable)this._cachedOptions.Clone();
			}
			else if (this._connectionOptions != null)
			{
				sqlConnection._connectionOptions = (Hashtable)this._connectionOptions.Clone();
			}
			sqlConnection._connectionString = this._connectionString;
			sqlConnection._fShowPassword = this._fShowPassword;
			return sqlConnection;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006AF3 File Offset: 0x00005AF3
		IDbCommand IDbConnection.CreateCommand()
		{
			return new SqlCommand(null, this);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006AFC File Offset: 0x00005AFC
		void IDisposable.Dispose()
		{
			switch ((int)this._objectState)
			{
			case 0:
				break;
			case 1:
				this.Close();
				this._connectionOptions = null;
				this._cachedOptions = null;
				this._connectionString = null;
				break;
			default:
				return;
			}
		}

		/// <summary>创建并返回一个与 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 关联的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</returns>
		// Token: 0x060000AD RID: 173 RVA: 0x00006B3A File Offset: 0x00005B3A
		public SqlCommand CreateCommand()
		{
			return new SqlCommand(null, this);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006B43 File Offset: 0x00005B43
		internal void AddPreparedCommand(SqlCommand cmd)
		{
			if (this._preparedCommands == null)
			{
				this._preparedCommands = new ArrayList(5);
			}
			this._preparedCommands.Add(cmd);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006B68 File Offset: 0x00005B68
		internal void RemovePreparedCommand(SqlCommand cmd)
		{
			if (this._preparedCommands == null || this._preparedCommands.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._preparedCommands.Count; i++)
			{
				if (this._preparedCommands[i] == cmd)
				{
					this._preparedCommands.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00006BBD File Offset: 0x00005BBD
		internal void CloseDeadReader()
		{
			if (this._reader != null && !this._reader.IsAlive && this.Parser.PendingData)
			{
				this._internalConnection.Parser.CleanWire();
				this._reader = null;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006BF8 File Offset: 0x00005BF8
		internal void RollbackDeadTransaction()
		{
			if (this._localTransaction != null && !this._localTransaction.IsAlive)
			{
				this.InternalRollback();
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00006C15 File Offset: 0x00005C15
		internal void ExecuteTransaction(string sqlBatch, string method)
		{
			if (this._objectState == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_ClosedConnectionError"));
			}
			this.CloseDeadReader();
			this.RollbackDeadTransaction();
			this._internalConnection.ExecuteTransaction(sqlBatch, method);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006C48 File Offset: 0x00005C48
		internal void InternalRollback()
		{
			this._internalConnection.ExecuteTransaction("ROLLBACK TRANSACTION", "RollbackTransaction");
			this.LocalTransaction = null;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006C66 File Offset: 0x00005C66
		internal void OnError(SqlException exception, TdsParserState state)
		{
			if (state == TdsParserState.Broken && (int)this._objectState == 1)
			{
				this.Close();
			}
			if (exception.Class >= 10)
			{
				throw exception;
			}
			this.OnInfoMessage(new SqlInfoMessageEventArgs(exception));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006C94 File Offset: 0x00005C94
		private void CloseReader()
		{
			if (this._reader != null)
			{
				SqlDataReader sqlDataReader = (SqlDataReader)this._reader.Target;
				if (sqlDataReader != null && this._reader.IsAlive)
				{
					if (!sqlDataReader.IsClosed)
					{
						sqlDataReader.Close();
					}
				}
				else if (this.Parser.PendingData)
				{
					this._internalConnection.Parser.CleanWire();
				}
				this._reader = null;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006CFE File Offset: 0x00005CFE
		private void FireObjectState(ConnectionState original, ConnectionState current)
		{
			this.OnStateChange(new StateChangeEventArgs(original, current));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006D10 File Offset: 0x00005D10
		private void OnInfoMessage(SqlInfoMessageEventArgs imevent)
		{
			if (this._infoMessageEventHandler != null)
			{
				try
				{
					this._infoMessageEventHandler(this, imevent);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006D48 File Offset: 0x00005D48
		private void OnStateChange(StateChangeEventArgs scevent)
		{
			if (this._stateChangeEventHandler != null)
			{
				this._stateChangeEventHandler.Invoke(this, scevent);
			}
		}

		// Token: 0x040000A8 RID: 168
		private Hashtable _connectionOptions;

		// Token: 0x040000A9 RID: 169
		private Hashtable _cachedOptions;

		// Token: 0x040000AA RID: 170
		private string _connectionString;

		// Token: 0x040000AB RID: 171
		private bool _fShowPassword;

		// Token: 0x040000AC RID: 172
		private bool _fIsClosing;

		// Token: 0x040000AD RID: 173
		private SqlInternalConnection _internalConnection;

		// Token: 0x040000AE RID: 174
		private WeakReference _reader;

		// Token: 0x040000AF RID: 175
		private WeakReference _localTransaction;

		// Token: 0x040000B0 RID: 176
		private ConnectionState _objectState;

		// Token: 0x040000B1 RID: 177
		private StateChangeEventHandler _stateChangeEventHandler;

		// Token: 0x040000B2 RID: 178
		internal SqlInfoMessageEventHandler _infoMessageEventHandler;

		// Token: 0x040000B3 RID: 179
		private static Hashtable s_connectionStringCacheHashtable;

		// Token: 0x040000B4 RID: 180
		private ArrayList _preparedCommands;
	}
}
