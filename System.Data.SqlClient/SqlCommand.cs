using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;
using System.Xml;

namespace System.Data.SqlClient
{
	/// <summary>表示要对 SQL Server 数据库执行的一个 Transact-SQL 语句或存储过程。无法继承此类。</summary>
	// Token: 0x02000005 RID: 5
	public sealed class SqlCommand : Component, IDbCommand, IDisposable, ICloneable
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 类的新实例。</summary>
		// Token: 0x0600003E RID: 62 RVA: 0x0000430A File Offset: 0x0000330A
		public SqlCommand()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>用查询文本初始化 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 类的新实例。</summary>
		/// <param name="cmdText">查询的文本。 </param>
		// Token: 0x0600003F RID: 63 RVA: 0x00004348 File Offset: 0x00003348
		public SqlCommand(string cmdText)
		{
			GC.SuppressFinalize(this);
			this.CommandText = cmdText;
		}

		/// <summary>初始化具有查询文本和 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 类的新实例。</summary>
		/// <param name="cmdText">查询的文本。 </param>
		/// <param name="connection">一个 <see cref="T:System.Data.SqlClient.SqlConnection"></see>，它表示到 SQL Server 实例的连接。 </param>
		// Token: 0x06000040 RID: 64 RVA: 0x00004398 File Offset: 0x00003398
		public SqlCommand(string cmdText, SqlConnection connection)
		{
			GC.SuppressFinalize(this);
			this.CommandText = cmdText;
			this.Connection = connection;
		}

		/// <summary>使用查询文本、一个 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 以及 <see cref="T:System.Data.SqlClient.SqlTransaction"></see> 来初始化 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 类的新实例。</summary>
		/// <param name="cmdText">查询的文本。 </param>
		/// <param name="connection">一个 <see cref="T:System.Data.SqlClient.SqlConnection"></see>，它表示到 SQL Server 实例的连接。 </param>
		/// <param name="transaction">将在其中执行 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 的 <see cref="T:System.Data.SqlClient.SqlTransaction"></see>。 </param>
		// Token: 0x06000041 RID: 65 RVA: 0x000043F0 File Offset: 0x000033F0
		public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction)
		{
			GC.SuppressFinalize(this);
			this.CommandText = cmdText;
			this.Connection = connection;
			this.Transaction = transaction;
		}

		/// <summary>获取或设置 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 的此实例使用的 <see cref="T:System.Data.SqlClient.SqlConnection"></see>。</summary>
		/// <returns>与数据源的连接。默认值为null。</returns>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000444D File Offset: 0x0000344D
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00004458 File Offset: 0x00003458
		public SqlConnection Connection
		{
			get
			{
				return this._activeConnection;
			}
			set
			{
				if (this._transaction != null && this._transaction._sqlConnection == null)
				{
					this._transaction = null;
				}
				if (this._activeConnection != null && this._activeConnection.Reader != null)
				{
					throw new InvalidOperationException(Res.GetString("ADP_CommandIsActive", new object[]
					{
						base.GetType().Name,
						"Open, Fetching"
					}));
				}
				this._activeConnection = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000044CB File Offset: 0x000034CB
		// (set) Token: 0x06000045 RID: 69 RVA: 0x000044D3 File Offset: 0x000034D3
		IDbConnection IDbCommand.Connection
		{
			get
			{
				return this._activeConnection;
			}
			set
			{
				this.Connection = (SqlConnection)value;
			}
		}

		/// <summary>获取或设置将在其中执行 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 的 <see cref="T:System.Data.SqlClient.SqlTransaction"></see>。</summary>
		/// <returns><see cref="T:System.Data.SqlClient.SqlTransaction"></see>。默认值为null。</returns>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000044E1 File Offset: 0x000034E1
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00004508 File Offset: 0x00003508
		public SqlTransaction Transaction
		{
			get
			{
				if (this._transaction != null && this._transaction.Connection == null)
				{
					this._transaction = null;
				}
				return this._transaction;
			}
			set
			{
				if (this._activeConnection != null && this._activeConnection.Reader != null)
				{
					throw new InvalidOperationException(Res.GetString("ADP_CommandIsActive", new object[]
					{
						base.GetType().Name,
						"Open, Fetching"
					}));
				}
				this._transaction = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000048 RID: 72 RVA: 0x0000455F File Offset: 0x0000355F
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00004567 File Offset: 0x00003567
		IDbTransaction IDbCommand.Transaction
		{
			get
			{
				return this._transaction;
			}
			set
			{
				this.Transaction = (SqlTransaction)value;
			}
		}

		/// <summary>获取或设置要对数据源执行的 Transact-SQL 语句或存储过程。</summary>
		/// <returns>要执行的 Transact-SQL 语句或存储过程。默认为空字符串。</returns>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00004575 File Offset: 0x00003575
		// (set) Token: 0x0600004B RID: 75 RVA: 0x0000457D File Offset: 0x0000357D
		public string CommandText
		{
			get
			{
				return this.cmdText;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (ADP.SrcCompare(this.cmdText, value) != 0)
				{
					this.OnSchemaChanging();
					this.cmdText = value;
				}
			}
		}

		/// <summary>获取或设置一个值，该值指示如何解释 <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> 属性。</summary>
		/// <returns><see cref="T:System.Data.CommandType"></see> 值之一。默认为 Text。</returns>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000045A4 File Offset: 0x000035A4
		// (set) Token: 0x0600004D RID: 77 RVA: 0x000045AC File Offset: 0x000035AC
		public CommandType CommandType
		{
			get
			{
				return this.cmdType;
			}
			set
			{
				if (this.cmdType != value)
				{
					this.OnSchemaChanging();
					if ((CommandType)512 == value)
					{
						throw SQL.TableDirectNotSupported();
					}
                    if (value != (CommandType)1 && value != (CommandType)4)
					{
						throw new ArgumentException(Res.GetString("ADP_InvalidCommandType", new object[]
						{
							value.ToString()
						}));
					}
					this.cmdType = value;
				}
			}
		}

		/// <summary>获取或设置在终止执行命令的尝试并生成错误之前的等待时间。</summary>
		/// <returns>等待命令执行的时间（以秒为单位）。默认为 30 秒。</returns>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000460B File Offset: 0x0000360B
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00004614 File Offset: 0x00003614
		public int CommandTimeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidCommandTimeout", new object[]
					{
						value.ToString()
					}));
				}
				this._timeout = value;
			}
		}

		/// <summary>获取 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see>。</summary>
		/// <returns>Transact-SQL 语句或存储过程的参数。默认为空集合。</returns>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000464E File Offset: 0x0000364E
		public SqlParameterCollection Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = new SqlParameterCollection(this);
				}
				return this._parameters;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000466A File Offset: 0x0000366A
		IDataParameterCollection IDbCommand.Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = new SqlParameterCollection(this);
				}
				return this._parameters;
			}
		}

		/// <summary>将 <see cref="P:System.Data.SqlClient.SqlCommand.CommandTimeout"></see> 属性重置为其默认值。</summary>
		// Token: 0x06000052 RID: 82 RVA: 0x00004686 File Offset: 0x00003686
		public void ResetCommandTimeout()
		{
			this._timeout = 30;
		}

		/// <summary>获取或设置命令结果在由 <see cref="T:System.Data.Common.DbDataAdapter"></see> 的“Update”方法使用时，如何应用于 <see cref="T:System.Data.DataRow"></see>。</summary>
		/// <returns><see cref="T:System.Data.UpdateRowSource"></see> 值之一。</returns>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004690 File Offset: 0x00003690
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00004698 File Offset: 0x00003698
		public UpdateRowSource UpdatedRowSource
		{
			get
			{
				return this._updatedRowSource;
			}
			set
			{
                if (value < 0 || value > (UpdateRowSource)3)
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidUpdateRowSource", new object[]
					{
						value.ToString()
					}));
				}
				this._updatedRowSource = value;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000046DA File Offset: 0x000036DA
		internal void OnSchemaChanging()
		{
			this.IsDirty = true;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000046E3 File Offset: 0x000036E3
		void IDisposable.Dispose()
		{
		}

		/// <summary>在 SQL Server 的实例上创建命令的一个准备版本。</summary>
		// Token: 0x06000057 RID: 87 RVA: 0x000046E8 File Offset: 0x000036E8
		public void Prepare()
		{
			if (this.IsPrepared || this.CommandType == (CommandType)4)
			{
				return;
			}
			this.ValidateCommand("Prepare", true);
			if (this._parameters != null)
			{
				int count = this._parameters.Count;
				for (int i = 0; i < count; i++)
				{
					this._parameters[i].Prepare(this);
				}
			}
			SqlDataReader sqlDataReader = this.Prepare(0);
			if (sqlDataReader != null)
			{
				this._cachedMetaData = sqlDataReader.MetaData;
				sqlDataReader.Close();
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004764 File Offset: 0x00003764
		private SqlDataReader Prepare(CommandBehavior behavior)
		{
			SqlDataReader sqlDataReader = null;
			if (this.IsDirty)
			{
				this.Unprepare();
				this.IsDirty = false;
			}
			if (this._activeConnection.IsShiloh)
			{
				this._execType = 2;
			}
			else
			{
				_SqlRPC rec = this.BuildPrepare(behavior);
				this._inPrepare = true;
				sqlDataReader = new SqlDataReader(this);
				try
				{
					this._activeConnection.Parser.TdsExecuteRPC(rec, this.CommandTimeout, false);
					this._activeConnection.Parser.Run(RunBehavior.UntilDone, this, sqlDataReader);
				}
				catch (Exception ex)
				{
					this._inPrepare = false;
					throw ex;
				}
				sqlDataReader.Bind(this._activeConnection.Parser);
				this._execType = 1;
			}
			this._activeConnection.AddPreparedCommand(this);
			return sqlDataReader;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004824 File Offset: 0x00003824
		internal void Unprepare()
		{
			if (this._activeConnection.IsShiloh && !this._activeConnection.IsClosing)
			{
				this._execType = 2;
			}
			else
			{
				if (this._handle != -1)
				{
					_SqlRPC rec = this.BuildUnprepare();
					this._activeConnection.Parser.TdsExecuteRPC(rec, this.CommandTimeout, false);
					this._activeConnection.Parser.Run(RunBehavior.UntilDone, this);
					this._handle = -1;
				}
				this._execType = 0;
			}
			this._cachedMetaData = null;
			this._activeConnection.RemovePreparedCommand(this);
		}

		/// <summary>尝试取消 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 的执行。</summary>
		// Token: 0x0600005A RID: 90 RVA: 0x000048B0 File Offset: 0x000038B0
		public void Cancel()
		{
			if (this._activeConnection == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_ConnectionRequired_Cancel", new object[]
				{
					base.GetType().Name
				}));
			}
			if ((ConnectionState)1 != this._activeConnection.State)
			{
				throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionRequired_Cancel", new object[]
				{
					this._activeConnection.State
				}));
			}
			if (this == this._activeConnection.Parser.PendingCommand && this._activeConnection.Parser.PendingData)
			{
				this._activeConnection.Parser.SendAttention();
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004959 File Offset: 0x00003959
		IDbDataParameter IDbCommand.CreateParameter()
		{
			return new SqlParameter();
		}

		/// <summary>创建 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的新实例。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象。</returns>
		// Token: 0x0600005C RID: 92 RVA: 0x00004960 File Offset: 0x00003960
		public SqlParameter CreateParameter()
		{
			return new SqlParameter();
		}

		/// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
		/// <returns>结果集中第一行的第一列或空引用（如果结果集为空）。</returns>
		// Token: 0x0600005D RID: 93 RVA: 0x00004968 File Offset: 0x00003968
		public object ExecuteScalar()
		{
			object result = null;
			SqlDataReader sqlDataReader = this.ExecuteReader(0, RunBehavior.ReturnImmediately, true);
			try
			{
				if (sqlDataReader.Read() && sqlDataReader.FieldCount > 0)
				{
					result = sqlDataReader.GetValue(0);
				}
			}
			finally
			{
				sqlDataReader.Close();
			}
			return result;
		}

		/// <summary>对连接执行 Transact-SQL 语句并返回受影响的行数。</summary>
		/// <returns>受影响的行数。</returns>
		// Token: 0x0600005E RID: 94 RVA: 0x000049B4 File Offset: 0x000039B4
		public int ExecuteNonQuery()
		{
			if ((CommandType)1 == this.CommandType && this.GetParameterCount() == 0)
			{
				this._rowsAffected = -1;
				this.ValidateCommand("ExecuteNonQuery", true);
				try
				{
					this._activeConnection.Parser.TdsExecuteSQLBatch(this.CommandText, this.CommandTimeout);
					this._activeConnection.Parser.Run(RunBehavior.UntilDone, this, null);
					goto IL_72;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			SqlDataReader sqlDataReader = this.ExecuteReader(0, RunBehavior.UntilDone, false);
			if (sqlDataReader != null)
			{
				sqlDataReader.Close();
				GC.SuppressFinalize(sqlDataReader);
			}
			IL_72:
			return this._rowsAffected;
		}

		/// <summary>将 <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> 发送到 <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see> 并生成一个 <see cref="T:System.Xml.XmlReader"></see> 对象。</summary>
		/// <returns>一个 <see cref="T:System.Xml.XmlReader"></see> 对象。</returns>
		// Token: 0x0600005F RID: 95 RVA: 0x00004A4C File Offset: 0x00003A4C
		public XmlReader ExecuteXmlReader()
		{
			SqlDataReader sqlDataReader = this.ExecuteReader((CommandBehavior)16, RunBehavior.ReturnImmediately, true);
			_SqlMetaData[] metaData = sqlDataReader.MetaData;
			if (metaData.Length == 1 && metaData[0].type == (SqlDbType)11)
			{
				try
				{
					SqlStream sqlStream = new SqlStream(sqlDataReader, true);
					return new XmlTextReader(sqlStream, (XmlNodeType)1, null);
				}
				catch (Exception ex)
				{
					sqlDataReader.Close();
					throw ex;
				}
			}
			sqlDataReader.Close();
			throw SQL.NonXmlResult();
		}

		/// <summary>将 <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> 发送到 <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see> 并生成一个 <see cref="T:System.Data.SqlClient.SqlDataReader"></see>。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 对象。</returns>
		// Token: 0x06000060 RID: 96 RVA: 0x00004ABC File Offset: 0x00003ABC
		public SqlDataReader ExecuteReader()
		{
			return this.ExecuteReader(0, RunBehavior.ReturnImmediately, true);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004AC7 File Offset: 0x00003AC7
		IDataReader IDbCommand.ExecuteReader()
		{
			return this.ExecuteReader(0, RunBehavior.ReturnImmediately, true);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004AD2 File Offset: 0x00003AD2
		IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior, RunBehavior.ReturnImmediately, true);
		}

		/// <summary>将 <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> 发送到 <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see>，并使用 <see cref="T:System.Data.CommandBehavior"></see> 值之一生成一个 <see cref="T:System.Data.SqlClient.SqlDataReader"></see>。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 对象。</returns>
		/// <param name="behavior"><see cref="T:System.Data.CommandBehavior"></see> 值之一。 </param>
		// Token: 0x06000063 RID: 99 RVA: 0x00004ADD File Offset: 0x00003ADD
		public SqlDataReader ExecuteReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior, RunBehavior.ReturnImmediately, true);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004AE8 File Offset: 0x00003AE8
		internal void DeriveParameters()
		{
			CommandType commandType = this.CommandType;
            if (commandType == (CommandType)1)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DeriveParametersNotSupported", new object[]
				{
					base.GetType().Name,
					this.CommandType.ToString()
				}));
			}
			if (commandType != (CommandType)4)
			{
                if (commandType != (CommandType)512)
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidCommandType", new object[]
					{
						this.CommandType.ToString()
					}));
				}
				throw new InvalidOperationException(Res.GetString("ADP_DeriveParametersNotSupported", new object[]
				{
					base.GetType().Name,
					this.CommandType.ToString()
				}));
			}
			else
			{
				this.ValidateCommand("DeriveParameters", false);
				string[] array = ADP.ParseProcedureName(this.CommandText);
				SqlCommand sqlCommand = null;
				if (array[1] != null)
				{
					this.cmdText = "[" + array[1] + "]..sp_procedure_params_rowset";
					if (array[0] != null)
					{
						this.cmdText = array[0] + "." + this.cmdText;
					}
					sqlCommand = new SqlCommand(this.cmdText, this.Connection);
				}
				else
				{
					sqlCommand = new SqlCommand("sp_procedure_params_rowset", this.Connection);
				}
                sqlCommand.CommandType = (CommandType)4;
				sqlCommand.Parameters.Add(new SqlParameter("@procedure_name", (SqlDbType)12, 255));
				sqlCommand.Parameters[0].Value = array[3];
				SqlDataReader sqlDataReader = null;
				ArrayList arrayList = new ArrayList();
				try
				{
					sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						SqlParameter sqlParameter = new SqlParameter();
						sqlParameter.ParameterName = (string)sqlDataReader["PARAMETER_NAME"];
						sqlParameter.SqlDbType = MetaType.GetSqlDbTypeFromOleDbType((short)sqlDataReader["DATA_TYPE"], (string)sqlDataReader["TYPE_NAME"]);
						object obj = sqlDataReader["CHARACTER_MAXIMUM_LENGTH"];
						if (obj is int)
						{
							sqlParameter.Size = (int)obj;
						}
						sqlParameter.Direction = this.ParameterDirectionFromOleDbDirection((short)sqlDataReader["PARAMETER_TYPE"]);
						if (sqlParameter.SqlDbType == (SqlDbType)5)
						{
							sqlParameter.Scale = (byte)((short)sqlDataReader["NUMERIC_SCALE"] & 255);
							sqlParameter.Precision = (byte)((short)sqlDataReader["NUMERIC_PRECISION"] & 255);
						}
						arrayList.Add(sqlParameter);
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
					}
					sqlCommand.Connection = null;
				}
				if (arrayList.Count == 0)
				{
					throw new InvalidOperationException(Res.GetString("ADP_NoStoredProcedureExists", new object[]
					{
						this.CommandText
					}));
				}
				this.Parameters.Clear();
				foreach (object value in arrayList)
				{
					this._parameters.Add(value);
				}
				return;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004E14 File Offset: 0x00003E14
		private ParameterDirection ParameterDirectionFromOleDbDirection(short oledbDirection)
		{
			switch (oledbDirection)
			{
			case 2:
                    return (ParameterDirection)3;
			case 3:
                    return (ParameterDirection)2;
			case 4:
                    return (ParameterDirection)6;
			default:
                    return (ParameterDirection)1;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00004E40 File Offset: 0x00003E40
		internal _SqlMetaData[] MetaData
		{
			get
			{
				return this._cachedMetaData;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004E48 File Offset: 0x00003E48
		internal SqlDataReader ExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream)
		{
			SqlDataReader sqlDataReader = null;
			this._rowsAffected = -1;
			bool inSchema = 0 != ((int)cmdBehavior & 2);
			if ((8 & (int)cmdBehavior) != null)
			{
				cmdBehavior |= (CommandBehavior)1;
			}
			this.ValidateCommand("ExecuteReader", true);
			string text = null;
			try
			{
				if (1 == (int)this.CommandType)
				{
					if (this.IsDirty)
					{
						this.Unprepare();
						this.IsDirty = false;
					}
					_SqlRPC sqlRPC;
					if (this._execType == 1)
					{
						sqlRPC = this.BuildExecute();
					}
					else if (this._execType == 2)
					{
						sqlRPC = this.BuildPrepExec(cmdBehavior);
						this._execType = 1;
						this._inPrepare = true;
					}
					else
					{
						sqlRPC = this.BuildExecuteSql(cmdBehavior);
					}
					if (this._activeConnection.IsShiloh)
					{
						sqlRPC.options = 2;
					}
					this._activeConnection.Parser.TdsExecuteRPC(sqlRPC, this.CommandTimeout, inSchema);
				}
				else
				{
					_SqlRPC sqlRPC = this.BuildRPC();
					text = this.GetSetOptionsON(cmdBehavior);
					if (text != null)
					{
						this._activeConnection.Parser.TdsExecuteSQLBatch(text, this.CommandTimeout);
						this._activeConnection.Parser.Run(RunBehavior.UntilDone, this, null);
						text = this.GetSetOptionsOFF(cmdBehavior);
					}
					this._activeConnection.Parser.TdsExecuteRPC(sqlRPC, this.CommandTimeout, inSchema);
				}
				if (returnStream)
				{
					sqlDataReader = new SqlDataReader(this);
				}
				if (runBehavior == RunBehavior.UntilDone)
				{
					this._activeConnection.Parser.Run(RunBehavior.UntilDone, this, sqlDataReader);
				}
				if (sqlDataReader != null)
				{
					sqlDataReader.Bind(this._activeConnection.Parser);
					sqlDataReader.Behavior = cmdBehavior;
					sqlDataReader.SetOptionsOFF = text;
					this._activeConnection.Reader = sqlDataReader;
					try
					{
						this._cachedMetaData = sqlDataReader.MetaData;
					}
					catch (Exception ex)
					{
						sqlDataReader.Close();
						throw ex;
					}
				}
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
			return sqlDataReader;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005008 File Offset: 0x00004008
		object ICloneable.Clone()
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandText = this.CommandText;
			sqlCommand.CommandType = this.CommandType;
			sqlCommand.UpdatedRowSource = this.UpdatedRowSource;
			IDataParameterCollection parameters = sqlCommand.Parameters;
			foreach (object obj in this.Parameters)
			{
				ICloneable cloneable = (ICloneable)obj;
				parameters.Add(cloneable.Clone());
			}
			sqlCommand.Connection = this.Connection;
			sqlCommand.Transaction = this.Transaction;
			sqlCommand.CommandTimeout = this.CommandTimeout;
			return sqlCommand;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000050C0 File Offset: 0x000040C0
		private void ValidateCommand(string method, bool executing)
		{
			if (this._activeConnection == null)
			{
				string name = string.Empty;
				if (method != null)
				{
					if (!(method == "Prepare"))
					{
						if (!(method == "ExecuteReader"))
						{
							if (!(method == "ExecuteNonQuery"))
							{
								if (method == "ExecuteScalar")
								{
									name = "ADP_ConnectionRequired_ExecuteScalar";
								}
							}
							else
							{
								name = "ADP_ConnectionRequired_ExecuteNonQuery";
							}
						}
						else
						{
							name = "ADP_ConnectionRequired_ExecuteReader";
						}
					}
					else
					{
						name = "ADP_ConnectionRequired_Prepare";
					}
				}
				throw new InvalidOperationException(Res.GetString(name));
			}
			if (ConnectionState.Open != this._activeConnection.State)
			{
				string name2 = string.Empty;
				if (method != null)
				{
					if (PrivateImplementationDetails.method1 == null)
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
						PrivateImplementationDetails.method1 = dictionary;
					}
					int num;
                    if (PrivateImplementationDetails.method1.TryGetValue(method, out num))
					{
						switch (num)
						{
						case 0:
							name2 = "ADP_OpenConnectionRequired_Prepare";
							break;
						case 1:
							name2 = "ADP_OpenConnectionRequired_ExecuteReader";
							break;
						case 2:
							name2 = "ADP_OpenConnectionRequired_ExecuteNonQuery";
							break;
						case 3:
							name2 = "ADP_OpenConnectionRequired_ExecuteScalar";
							break;
						case 4:
							name2 = "ADP_OpenConnectionRequired_BeginTransaction";
							break;
						case 5:
							name2 = "ADP_OpenConnectionRequired_ChangeDatabase";
							break;
						case 6:
							name2 = "ADP_OpenConnectionRequired_CommitTransaction";
							break;
						case 7:
							name2 = "ADP_OpenConnectionRequired_RollbackTransaction";
							break;
						}
					}
				}
				throw new InvalidOperationException(Res.GetString(name2, new object[]
				{
					method,
					this._activeConnection.State
				}));
			}
			this._activeConnection.CloseDeadReader();
			if (this._activeConnection.Reader != null || this._activeConnection.Parser.PendingData)
			{
				throw new InvalidOperationException(Res.GetString("ADP_OpenReaderExists"));
			}
			if (executing)
			{
				this._activeConnection.RollbackDeadTransaction();
				if (this._transaction != null && this._transaction._sqlConnection == null)
				{
					this._transaction = null;
				}
				if (this._activeConnection.LocalTransaction != null && this._transaction == null)
				{
					throw new InvalidOperationException(Res.GetString("ADP_TransactionRequired_Execute"));
				}
				if (this._transaction != null && this._activeConnection != this._transaction._sqlConnection)
				{
					throw new InvalidOperationException(Res.GetString("ADP_TransactionConnectionMismatch"));
				}
				if (ADP.IsEmpty(this.CommandText))
				{
					throw new InvalidOperationException(Res.GetString("ADP_CommandTextRequired"));
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005350 File Offset: 0x00004350
		internal void OnReturnStatus(int status)
		{
			if (this._inPrepare)
			{
				return;
			}
			int parameterCount = this.GetParameterCount();
			int i = 0;
			while (i < parameterCount)
			{
				SqlParameter sqlParameter = this._parameters[i];
				if (sqlParameter.Direction == (ParameterDirection)6)
				{
					object value = sqlParameter.Value;
					if (value != null && value.GetType() == typeof(SqlInt32))
					{
						sqlParameter.Value = new SqlInt32(status);
						return;
					}
					sqlParameter.Value = status;
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000053CC File Offset: 0x000043CC
		internal void OnReturnValue(SqlReturnValue rec)
		{
			if (this._inPrepare)
			{
				SqlInt32 sqlInt = (SqlInt32)rec.value;
				if (!sqlInt.IsNull)
				{
					this._handle = sqlInt.Value;
				}
				this._inPrepare = false;
				return;
			}
			SqlParameter sqlParameter = null;
			int parameterCount = this.GetParameterCount();
			for (int i = this._currentParam; i < parameterCount; i++)
			{
				sqlParameter = this._parameters[i];
				if ((int)sqlParameter.Direction != 1 && (rec.parameter == null || sqlParameter.ParameterName == null || string.Compare(rec.parameter, sqlParameter.ParameterName, false, CultureInfo.InvariantCulture) == 0))
				{
					break;
				}
			}
			if (rec.parameter == null)
			{
				this._currentParam++;
			}
			if (sqlParameter != null)
			{
				object value = sqlParameter.Value;
				if (value is INullable)
				{
					sqlParameter.Value = rec.value;
				}
				else
				{
					sqlParameter.Value = MetaType.GetComValue(rec.type, rec.value);
				}
				MetaType.GetMetaType(rec.type);
                if (rec.type == (SqlDbType)5)
				{
					sqlParameter.Scale = rec.scale;
					sqlParameter.Precision = rec.precision;
				}
				if (rec.collation != null)
				{
					sqlParameter.Collation = rec.collation;
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000054F4 File Offset: 0x000044F4
		private _SqlRPC BuildPrepExec(CommandBehavior behavior)
		{
			int i = 0;
			int num = 3;
			_SqlRPC sqlRPC = new _SqlRPC();
			sqlRPC.rpcName = "sp_prepexec";
			sqlRPC.parameters = new SqlParameter[this.CountParameters() + num];
			SqlParameter sqlParameter = new SqlParameter(null, 8);
			sqlParameter.Direction = (ParameterDirection)3;
			sqlParameter.Value = this._handle;
			sqlRPC.parameters[0] = sqlParameter;
			string text = this.BuildParamList();
            sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? (SqlDbType)12 : (SqlDbType)11, text.Length);
			sqlParameter.Value = text;
			sqlRPC.parameters[1] = sqlParameter;
			string commandText = this.GetCommandText(behavior);
            sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? (SqlDbType)12 : (SqlDbType)11, commandText.Length);
			sqlParameter.Value = commandText;
			sqlRPC.parameters[2] = sqlParameter;
			int parameterCount = this.GetParameterCount();
			while (i < parameterCount)
			{
				SqlParameter sqlParameter2 = this._parameters[i];
				if (this.ShouldSendParameter(sqlParameter2))
				{
					sqlRPC.parameters[num++] = sqlParameter2;
				}
				i++;
			}
			return sqlRPC;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005608 File Offset: 0x00004608
		private bool ShouldSendParameter(SqlParameter p)
		{
			switch ((int)p.Direction)
			{
			case 1:
				return !p.Suppress;
			case 2:
			case 3:
				return true;
			case 6:
				return false;
			}
			return false;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005650 File Offset: 0x00004650
		private int CountParameters()
		{
			int num = 0;
			int parameterCount = this.GetParameterCount();
			for (int i = 0; i < parameterCount; i++)
			{
				if (this.ShouldSendParameter(this._parameters[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000568B File Offset: 0x0000468B
		private int GetParameterCount()
		{
			if (this._parameters == null)
			{
				return 0;
			}
			return this._parameters.Count;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000056A4 File Offset: 0x000046A4
		private _SqlRPC BuildRPC()
		{
			int i = 0;
			int num = 0;
			_SqlRPC sqlRPC = new _SqlRPC();
			sqlRPC.rpcName = this.CommandText;
			sqlRPC.parameters = new SqlParameter[this.CountParameters()];
			int parameterCount = this.GetParameterCount();
			while (i < parameterCount)
			{
				SqlParameter sqlParameter = this._parameters[i];
				if (this.ShouldSendParameter(sqlParameter))
				{
					sqlRPC.parameters[num++] = sqlParameter;
				}
				i++;
			}
			return sqlRPC;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005710 File Offset: 0x00004710
		private _SqlRPC BuildUnprepare()
		{
			_SqlRPC sqlRPC = new _SqlRPC();
			sqlRPC.rpcName = "sp_unprepare";
			sqlRPC.parameters = new SqlParameter[1];
			SqlParameter sqlParameter = new SqlParameter(null, 8);
			sqlParameter.Value = this._handle;
			sqlRPC.parameters[0] = sqlParameter;
			return sqlRPC;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005760 File Offset: 0x00004760
		private _SqlRPC BuildExecute()
		{
			int i = 0;
			int num = 1;
			_SqlRPC sqlRPC = new _SqlRPC();
			sqlRPC.rpcName = "sp_execute";
			sqlRPC.parameters = new SqlParameter[this.CountParameters() + num];
			SqlParameter sqlParameter = new SqlParameter(null, 8);
			sqlParameter.Value = this._handle;
			sqlRPC.parameters[0] = sqlParameter;
			int parameterCount = this.GetParameterCount();
			while (i < parameterCount)
			{
				SqlParameter sqlParameter2 = this._parameters[i];
				if (this.ShouldSendParameter(sqlParameter2))
				{
					sqlRPC.parameters[num++] = sqlParameter2;
				}
				i++;
			}
			return sqlRPC;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000057F4 File Offset: 0x000047F4
		private _SqlRPC BuildExecuteSql(CommandBehavior behavior)
		{
			_SqlRPC sqlRPC = new _SqlRPC();
			sqlRPC.rpcName = "sp_executesql";
			int num = this.CountParameters();
			SqlParameter sqlParameter;
			if (num > 0)
			{
				int i = 0;
				int num2 = 2;
				sqlRPC.parameters = new SqlParameter[this.CountParameters() + num2];
				string text = this.BuildParamList();
                sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? (SqlDbType)12 : (SqlDbType)11, text.Length);
				sqlParameter.Value = text;
				sqlRPC.parameters[1] = sqlParameter;
				int parameterCount = this.GetParameterCount();
				while (i < parameterCount)
				{
					SqlParameter sqlParameter2 = this._parameters[i];
					if (this.ShouldSendParameter(sqlParameter2))
					{
						sqlRPC.parameters[num2++] = sqlParameter2;
					}
					i++;
				}
			}
			else
			{
				sqlRPC.parameters = new SqlParameter[1];
			}
			string commandText = this.GetCommandText(behavior);
            sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? (SqlDbType)12 : (SqlDbType)11, commandText.Length);
			sqlParameter.Value = commandText;
			sqlRPC.parameters[0] = sqlParameter;
			return sqlRPC;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005900 File Offset: 0x00004900
		private string BuildParamList()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			int parameterCount = this.GetParameterCount();
			for (int i = 0; i < parameterCount; i++)
			{
				SqlParameter sqlParameter = this._parameters[i];
				if (this.ShouldSendParameter(sqlParameter))
				{
					if (flag)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(sqlParameter.ParameterName);
					MetaType metaType = sqlParameter.GetMetaType();
					this.ValidateTypeLengths(sqlParameter, ref metaType);
					stringBuilder.Append(" " + metaType.TypeName);
					flag = true;
					if (sqlParameter.SqlDbType == (SqlDbType)5)
					{
						byte b = sqlParameter.Precision;
						byte scale = sqlParameter.Scale;
						stringBuilder.Append('(');
						if (b == 0)
						{
							b = 28;
						}
						stringBuilder.Append(b);
						stringBuilder.Append(',');
						stringBuilder.Append(scale);
						stringBuilder.Append(')');
					}
                    else if (!metaType.IsFixed && !metaType.IsLong && metaType.SqlDbType != (SqlDbType)19)
					{
						int num = sqlParameter.Size;
						stringBuilder.Append('(');
						if (MetaType.IsAnsiType(sqlParameter.SqlDbType))
						{
							object coercedValue = sqlParameter.CoercedValue;
							string text = null;
							if (coercedValue is INullable)
							{
								if (!((INullable)coercedValue).IsNull)
								{
									text = ((SqlString)coercedValue).Value;
								}
							}
							else if (coercedValue != null && !Convert.IsDBNull(coercedValue))
							{
								text = (string)coercedValue;
							}
							if (text != null)
							{
								int encodingCharLength = this._activeConnection.Parser.GetEncodingCharLength(text, sqlParameter.ActualSize, sqlParameter.Offset, null);
								if (encodingCharLength > num)
								{
									num = encodingCharLength;
								}
							}
						}
						if (num == 0)
						{
							num = (MetaType.IsSizeInCharacters(metaType.SqlDbType) ? 4000 : 8000);
						}
						stringBuilder.Append(num);
						stringBuilder.Append(')');
					}
					if ((int)sqlParameter.Direction != 1)
					{
						stringBuilder.Append(" output");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005AF8 File Offset: 0x00004AF8
		private void ValidateTypeLengths(SqlParameter sqlParam, ref MetaType mt)
		{
			if (!mt.IsFixed && !mt.IsLong)
			{
				int actualSize = sqlParam.ActualSize;
				int size = sqlParam.Size;
				int num = (size > actualSize) ? size : actualSize;
				if (num > 8000)
				{
					SqlDbType sqlDbType = mt.SqlDbType;
					switch (sqlDbType)
					{
                        case (SqlDbType)1:
						break;
                        case (SqlDbType)2:
						return;
                        case (SqlDbType)3:
						goto IL_8A;
					default:
						switch (sqlDbType)
						{
                            case (SqlDbType)10:
                            case (SqlDbType)12:
                                sqlParam.SqlDbType = (SqlDbType)11;
							mt = sqlParam.GetMetaType();
							return;
                            case (SqlDbType)11:
							return;
						default:
							switch (sqlDbType)
							{
                                case (SqlDbType)21:
								break;
                                case (SqlDbType)22:
								goto IL_8A;
							default:
								return;
							}
							break;
						}
						break;
					}
                    sqlParam.SqlDbType = (SqlDbType)7;
					mt = sqlParam.GetMetaType();
					return;
					IL_8A:
                    sqlParam.SqlDbType = (SqlDbType)18;
					mt = sqlParam.GetMetaType();
					return;
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005BB0 File Offset: 0x00004BB0
		private string GetSetOptionsON(CommandBehavior behavior)
		{
			string text = null;
            if ((CommandBehavior)2 == (behavior & (CommandBehavior)2) || (CommandBehavior)4 == (behavior & (CommandBehavior)4))
			{
				text = " SET FMTONLY OFF;";
                if ((CommandBehavior)4 == (behavior & (CommandBehavior)4))
				{
					text += " SET NO_BROWSETABLE ON;";
				}
                if ((CommandBehavior)2 == (behavior & (CommandBehavior)2))
				{
					text += " SET FMTONLY ON;";
				}
			}
			return text;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005BF8 File Offset: 0x00004BF8
		private string GetSetOptionsOFF(CommandBehavior behavior)
		{
			string text = null;
            if ((CommandBehavior)4 == (behavior & (CommandBehavior)4))
			{
				text = " SET NO_BROWSETABLE OFF;";
			}
            if ((CommandBehavior)2 == (behavior & (CommandBehavior)2))
			{
				text += " SET FMTONLY OFF;";
			}
			return text;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005C26 File Offset: 0x00004C26
		private string GetCommandText(CommandBehavior behavior)
		{
			return this.GetSetOptionsON(behavior) + this.CommandText;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005C3C File Offset: 0x00004C3C
		private _SqlRPC BuildPrepare(CommandBehavior behavior)
		{
			_SqlRPC sqlRPC = new _SqlRPC();
			sqlRPC.rpcName = "sp_prepare";
			sqlRPC.parameters = new SqlParameter[3];
			SqlParameter sqlParameter = new SqlParameter(null, 8);
			sqlParameter.Direction = (ParameterDirection)2;
			sqlRPC.parameters[0] = sqlParameter;
			string text = this.BuildParamList();
            sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? (SqlDbType)12 : (SqlDbType)11, text.Length);
			sqlParameter.Value = text;
			sqlRPC.parameters[1] = sqlParameter;
			string commandText = this.GetCommandText(behavior);
            sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? (SqlDbType)12 : (SqlDbType)11, commandText.Length);
			sqlParameter.Value = commandText;
			sqlRPC.parameters[2] = sqlParameter;
			return sqlRPC;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00005CF2 File Offset: 0x00004CF2
		internal bool IsPrepared
		{
			get
			{
				return this._execType != 0;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00005D00 File Offset: 0x00004D00
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00005D08 File Offset: 0x00004D08
		internal bool IsDirty
		{
			get
			{
				return this._dirty;
			}
			set
			{
				this._dirty = (value && this.IsPrepared);
				this._cachedMetaData = null;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00005D23 File Offset: 0x00004D23
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00005D2B File Offset: 0x00004D2B
		internal int RecordsAffected
		{
			get
			{
				return this._rowsAffected;
			}
			set
			{
				if (-1 == this._rowsAffected)
				{
					this._rowsAffected = value;
					return;
				}
				this._rowsAffected += value;
			}
		}

		// Token: 0x04000064 RID: 100
		private const byte xtEXECSQL = 0;

		// Token: 0x04000065 RID: 101
		private const byte xtEXEC = 1;

		// Token: 0x04000066 RID: 102
		private const byte xtPREPEXEC = 2;

		// Token: 0x04000067 RID: 103
		private string cmdText = string.Empty;

		// Token: 0x04000068 RID: 104
		private CommandType cmdType = (CommandType)1;

		// Token: 0x04000069 RID: 105
		private int _timeout = 30;

		// Token: 0x0400006A RID: 106
		private UpdateRowSource _updatedRowSource = (UpdateRowSource)3;

		// Token: 0x0400006B RID: 107
		private bool _inPrepare;

		// Token: 0x0400006C RID: 108
		private int _handle = -1;

		// Token: 0x0400006D RID: 109
		private SqlParameterCollection _parameters;

		// Token: 0x0400006E RID: 110
		private SqlConnection _activeConnection;

		// Token: 0x0400006F RID: 111
		private int _currentParam;

		// Token: 0x04000070 RID: 112
		private bool _dirty;

		// Token: 0x04000071 RID: 113
		private byte _execType;

		// Token: 0x04000072 RID: 114
		private _SqlMetaData[] _cachedMetaData;

		// Token: 0x04000073 RID: 115
		internal int _rowsAffected = -1;

		// Token: 0x04000074 RID: 116
		private SqlTransaction _transaction;
	}
}
