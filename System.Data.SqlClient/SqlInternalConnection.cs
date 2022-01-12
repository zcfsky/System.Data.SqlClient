using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace System.Data.SqlClient
{
	internal sealed class SqlInternalConnection
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00009CD1 File Offset: 0x00008CD1
		public SqlInternalConnection(SqlConnection connection, Hashtable connectionOptions)
		{
			this._connectionWeakRef = new WeakReference(connection);
			this._connectionOptions = connectionOptions;
			this.OpenAndLogin();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00009CF4 File Offset: 0x00008CF4
		private SqlConnection Connection
		{
			get
			{
				if (this._connectionWeakRef != null)
				{
					SqlConnection result = (SqlConnection)this._connectionWeakRef.Target;
					if (this._connectionWeakRef.IsAlive)
					{
						return result;
					}
				}
				return null;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00009D2A File Offset: 0x00008D2A
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00009D32 File Offset: 0x00008D32
		public WeakReference ConnectionWeakRef
		{
			get
			{
				return this._connectionWeakRef;
			}
			set
			{
				this._connectionWeakRef = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00009D3B File Offset: 0x00008D3B
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00009D43 File Offset: 0x00008D43
		public Hashtable ConnectionOptions
		{
			get
			{
				return this._connectionOptions;
			}
			set
			{
				this._connectionOptions = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00009D4C File Offset: 0x00008D4C
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00009D54 File Offset: 0x00008D54
		public bool InLocalTransaction
		{
			get
			{
				return this._fInLocalTransaction;
			}
			set
			{
				this._fInLocalTransaction = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00009D5D File Offset: 0x00008D5D
		public bool IsShiloh
		{
			get
			{
				return this._loginAck.isVersion8;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00009D6A File Offset: 0x00008D6A
		public TdsParser Parser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00009D74 File Offset: 0x00008D74
		public string ServerVersion
		{
			get
			{
				string text = string.Format("{0:00}.{1:00}", this._loginAck.majorVersion, (short)this._loginAck.minorVersion);
				return text + string.Format(".{0:0000}", this._loginAck.buildNum);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00009DD0 File Offset: 0x00008DD0
		public void OnEnvChange(SqlEnvChange rec)
		{
			switch (rec.type)
			{
			case 1:
				if (!this._fConnectionOpen)
				{
					this._originalDatabase = rec.newValue;
				}
				this._connectionOptions["initial catalog"] = rec.newValue;
				return;
			case 2:
				if (!this._fConnectionOpen)
				{
					this._originalLanguage = rec.newValue;
				}
				this._connectionOptions["current language"] = rec.newValue;
				return;
			case 3:
			case 5:
			case 6:
			case 7:
				break;
			case 4:
				this._connectionOptions["packet size"] = int.Parse(rec.newValue, CultureInfo.InvariantCulture);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00009E84 File Offset: 0x00008E84
		public void OnError(SqlException exception, TdsParserState state)
		{
			if (this.Connection != null)
			{
				this.Connection.OnError(exception, state);
				return;
			}
			if (exception.Class >= 10)
			{
				throw exception;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00009EA8 File Offset: 0x00008EA8
		public void OnLoginAck(SqlLoginAck rec)
		{
			this._loginAck = rec;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00009EB4 File Offset: 0x00008EB4
		public void ChangeDatabase(string database)
		{
			if (this._parser.PendingData)
			{
				throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionRequired_ChangeDatabase", new object[]
				{
					this.Connection.State
				}));
			}
			this._parser.TdsExecuteSQLBatch("use [" + database + "]", (int)this._connectionOptions["connect timeout"]);
			this._parser.Run(RunBehavior.UntilDone);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00009F38 File Offset: 0x00008F38
		public void Cleanup()
		{
			try
			{
				if (this._parser.State == TdsParserState.OpenLoggedIn)
				{
					if (this._parser.PendingData)
					{
						this._parser.CleanWire();
					}
					if (this._fInLocalTransaction)
					{
						this._fInLocalTransaction = false;
						this.ExecuteTransaction("ROLLBACK TRANSACTION", "RollbackTransaction");
					}
				}
			}
			catch (Exception)
			{
				this.Close();
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00009FA8 File Offset: 0x00008FA8
		public void Close()
		{
			if (this._fConnectionOpen && Interlocked.CompareExchange(ref this._lock, 1, 0) == 0)
			{
				if (this._fConnectionOpen)
				{
					try
					{
						this._parser.Disconnect();
						return;
					}
					finally
					{
						this._connectionWeakRef = null;
						this._connectionOptions = null;
						this._parser = null;
						this._loginAck = null;
						this._fConnectionOpen = false;
						this._lock = 0;
					}
				}
				this._lock = 0;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000A024 File Offset: 0x00009024
		public void ExecuteTransaction(string sqlBatch, string method)
		{
			if (this._parser.PendingData)
			{
				string name = string.Empty;
				if (method != null)
				{
					if (PrivateImplementationDetails.method2 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
						dictionary.Add("Prepare", 0);
						dictionary.Add("ExecuteReader", 1);
						dictionary.Add("ExecuteNonQuery", 2);
						dictionary.Add("ExecuteScalar", 3);
						dictionary.Add("BeginTransaction", 4);
						dictionary.Add("ChangeDatabase", 5);
						dictionary.Add("CommitTransaction", 6);
						dictionary.Add("RollbackTransaction", 7);
                        PrivateImplementationDetails.method2 = dictionary;
					}
					int num;
                    if (PrivateImplementationDetails.method2.TryGetValue(method, out num))
					{
						switch (num)
						{
						case 0:
							name = "ADP_OpenConnectionRequired_Prepare";
							break;
						case 1:
							name = "ADP_OpenConnectionRequired_ExecuteReader";
							break;
						case 2:
							name = "ADP_OpenConnectionRequired_ExecuteNonQuery";
							break;
						case 3:
							name = "ADP_OpenConnectionRequired_ExecuteScalar";
							break;
						case 4:
							name = "ADP_OpenConnectionRequired_BeginTransaction";
							break;
						case 5:
							name = "ADP_OpenConnectionRequired_ChangeDatabase";
							break;
						case 6:
							name = "ADP_OpenConnectionRequired_CommitTransaction";
							break;
						case 7:
							name = "ADP_OpenConnectionRequired_RollbackTransaction";
							break;
						}
					}
				}
				throw new InvalidOperationException(Res.GetString(name, new object[]
				{
					method,
					this.Connection.State
				}));
			}
			this._parser.TdsExecuteSQLBatch(sqlBatch, (int)this._connectionOptions["connect timeout"]);
			this._parser.Run(RunBehavior.UntilDone);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000A194 File Offset: 0x00009194
		private void Login(int timeout)
		{
			SqlLogin sqlLogin = new SqlLogin();
			sqlLogin.timeout = timeout;
			sqlLogin.hostName = (string)this._connectionOptions["workstation id"];
			sqlLogin.userName = (string)this._connectionOptions["user id"];
			sqlLogin.password = (string)this._connectionOptions["password"];
			sqlLogin.applicationName = (string)this._connectionOptions["application name"];
			sqlLogin.language = (string)this._connectionOptions["current language"];
			sqlLogin.database = (string)this._connectionOptions["initial catalog"];
			sqlLogin.serverName = (string)this._connectionOptions["data source"];
			sqlLogin.useSSPI = (bool)this._connectionOptions["integrated security"];
			sqlLogin.packetSize = (int)this._connectionOptions["packet size"];
			this._parser.TdsLogin(sqlLogin);
			this._parser.Run(RunBehavior.UntilDone);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000A2BC File Offset: 0x000092BC
		private void OpenAndLogin()
		{
			try
			{
				int timeout = (int)this._connectionOptions["connect timeout"];
				this._parser = new TdsParser();
				timeout = this._parser.Connect((string)this._connectionOptions["data source"], this, timeout);
				this.Login(timeout);
				this._fConnectionOpen = true;
			}
			catch (Exception ex)
			{
				if (this._parser != null)
				{
					this._parser.Disconnect();
				}
				throw ex;
			}
		}

		// Token: 0x040000F5 RID: 245
		private WeakReference _connectionWeakRef;

		// Token: 0x040000F6 RID: 246
		private Hashtable _connectionOptions;

		// Token: 0x040000F7 RID: 247
		private TdsParser _parser;

		// Token: 0x040000F8 RID: 248
		private SqlLoginAck _loginAck;

		// Token: 0x040000F9 RID: 249
		private bool _fConnectionOpen;

		// Token: 0x040000FA RID: 250
		private int _lock;

		// Token: 0x040000FB RID: 251
		private string _originalDatabase;

		// Token: 0x040000FC RID: 252
		private string _originalLanguage;

		// Token: 0x040000FD RID: 253
		private bool _fInLocalTransaction;
	}
}
