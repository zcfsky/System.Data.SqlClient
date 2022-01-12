using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	/// <summary>提供一种从 SQL Server 数据库读取行的只进流的方式。无法继承此类。</summary>
	// Token: 0x0200000A RID: 10
	public sealed class SqlDataReader : MarshalByRefObject, IEnumerable, IDataReader, IDisposable, IDataRecord
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00006E70 File Offset: 0x00005E70
		internal SqlDataReader(SqlCommand command)
		{
			this._command = command;
			this._dataReady = false;
			this._metaDataConsumed = false;
			this._browseModeInfoConsumed = false;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006EA2 File Offset: 0x00005EA2
		internal void Bind(TdsParser parser)
		{
			this._parser = parser;
			this._defaultLCID = this._parser.DefaultLCID;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006EBC File Offset: 0x00005EBC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DbEnumerator(this, 0 != ((CommandBehavior)32 & this._behavior));
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006ED3 File Offset: 0x00005ED3
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006EE4 File Offset: 0x00005EE4
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				try
				{
					this.Close();
				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>获取一个值，用于指示当前行的嵌套深度。</summary>
		/// <returns>当前行的嵌套深度。</returns>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00006F10 File Offset: 0x00005F10
		public int Depth
		{
			get
			{
				if (this.IsClosed)
				{
					throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", new object[]
					{
						"Depth"
					}));
				}
				return 0;
			}
		}

		/// <summary>获取当前行中的列数。</summary>
		/// <returns>如果未放在有效的记录集中，则为 0；否则为当前行中的列数。默认值为 -1。</returns>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00006F48 File Offset: 0x00005F48
		public int FieldCount
		{
			get
			{
				if (this.IsClosed)
				{
					throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", new object[]
					{
						"FieldCount"
					}));
				}
				if (this.MetaData == null)
				{
					return 0;
				}
				return this._metaData.Length;
			}
		}

		/// <summary>获取当前行的集合中的所有属性列。</summary>
		/// <returns>数组中 <see cref="T:System.Object"></see> 的实例的数目。</returns>
		/// <param name="values">要将属性列复制到的 <see cref="T:System.Object"></see> 数组。 </param>
		// Token: 0x060000CF RID: 207 RVA: 0x00006F90 File Offset: 0x00005F90
		public int GetValues(object[] values)
		{
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			int num = (values.Length < this._visibleColumns) ? values.Length : this._visibleColumns;
			if (((int)this._behavior & 16) != null)
			{
				for (int i = 0; i < num; i++)
				{
					values[this._indexMap[i]] = this.SeqRead(i, false, false);
				}
			}
			else
			{
				this.PrepareRecord(0);
				for (int j = 0; j < num; j++)
				{
					values[j] = this._comBuf[j];
				}
			}
			if (this._rowException != null)
			{
				throw this._rowException;
			}
			return num;
		}

		/// <summary>获取指定列的名称。</summary>
		/// <returns>指定列的名称。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000D0 RID: 208 RVA: 0x00007033 File Offset: 0x00006033
		public string GetName(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			return this._metaData[i].column;
		}

		/// <summary>获取以本机格式表示的指定列的值。</summary>
		/// <returns>此方法对于空数据库列返回 <see cref="T:System.DBNull"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000D1 RID: 209 RVA: 0x00007050 File Offset: 0x00006050
		public object GetValue(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			return this.PrepareRecord(i);
		}

		/// <summary>获取源数据类型的名称。</summary>
		/// <returns>后端数据类型的名称。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000D2 RID: 210 RVA: 0x00007067 File Offset: 0x00006067
		public string GetDataTypeName(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			return this._metaData[i].metaType.TypeName;
		}

		/// <summary>获取是对象的数据类型的 <see cref="T:System.Type"></see>。</summary>
		/// <returns><see cref="T:System.Type"></see>，它是对象的数据类型。在用户定义的类型 (UDT) 从数据库中返回的情况下，如果客户端上不存在该类型，则 GetFieldType 返回空。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000D3 RID: 211 RVA: 0x00007089 File Offset: 0x00006089
		public Type GetFieldType(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			return this._metaData[i].metaType.ClassType;
		}

		/// <summary>在给定列名称的情况下获取列序号。</summary>
		/// <returns>从零开始的列序号。</returns>
		/// <param name="name">列的名称。 </param>
		// Token: 0x060000D4 RID: 212 RVA: 0x000070AB File Offset: 0x000060AB
		public int GetOrdinal(string name)
		{
			if (this._fieldNameLookup == null)
			{
				if (this.MetaData == null)
				{
					throw SQL.InvalidRead();
				}
				this._fieldNameLookup = new FieldNameLookup(this, this._defaultLCID);
			}
			return this._fieldNameLookup.GetOrdinal(name);
		}

		/// <summary>在给定列序号的情况下，获取指定列的以本机格式表示的值。</summary>
		/// <returns>指定列的以本机格式表示的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x17000026 RID: 38
		public object this[int i]
		{
			get
			{
				return this.GetValue(i);
			}
		}

		/// <summary>在给定列名称的情况下，获取指定列的以本机格式表示的值。</summary>
		/// <returns>指定列的以本机格式表示的值。</returns>
		/// <param name="name">列名称。 </param>
		// Token: 0x17000027 RID: 39
		public object this[string name]
		{
			get
			{
				return this.GetValue(this.GetOrdinal(name));
			}
		}

		/// <summary>获取指定列的布尔值形式的值。</summary>
		/// <returns>列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000D7 RID: 215 RVA: 0x000070FC File Offset: 0x000060FC
		public bool GetBoolean(int i)
		{
			return this.GetSqlBoolean(i).Value;
		}

		/// <summary>获取指定列的字节形式的值。</summary>
		/// <returns>指定列的字节形式的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000D8 RID: 216 RVA: 0x00007118 File Offset: 0x00006118
		public byte GetByte(int i)
		{
			return this.GetSqlByte(i).Value;
		}

		/// <summary>从指定的列偏移量将字节流读入缓冲区，并将其作为从给定的缓冲区偏移量开始的数组。</summary>
		/// <returns>读取的实际字节数。</returns>
		/// <param name="buffer">要将字节流读入的缓冲区。 </param>
		/// <param name="dataIndex">字段中的索引，从其开始读取操作。</param>
		/// <param name="i">从零开始的列序号。 </param>
		/// <param name="bufferIndex">开始写操作的 buffer 的索引。 </param>
		/// <param name="length">要复制到缓冲区中的最大长度。 </param>
		// Token: 0x060000D9 RID: 217 RVA: 0x00007134 File Offset: 0x00006134
		public long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			int num = 0;
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (!this._metaData[i].metaType.IsLong && !MetaType.IsBinType(this._metaData[i].metaType.SqlDbType))
			{
				throw SQL.NonBlobColumn(this._metaData[i].column);
			}
			if ((this._behavior & (CommandBehavior)16) != null)
			{
				if (i != this._currCol - 1)
				{
					this._seqBytesLeft = (long)((int)this.SeqRead(i, true, true));
				}
				else if (this._peekLength != -1)
				{
					this._seqBytesLeft = (long)this._peekLength;
					this._peekLength = -1;
				}
				if (buffer == null)
				{
					return this._seqBytesLeft;
				}
				if (dataIndex < this._seqBytesRead)
				{
					throw new InvalidOperationException(Res.GetString("ADP_NonSeqByteAccess", new object[]
					{
						dataIndex.ToString(),
						this._seqBytesRead.ToString(),
						"GetBytes"
					}));
				}
				long num2 = dataIndex - this._seqBytesRead;
				if (num2 > this._seqBytesLeft)
				{
					return 0L;
				}
				if (num2 > 0L)
				{
					this._parser.SkipBytes(num2);
					this._seqBytesRead += num2;
					this._seqBytesLeft -= num2;
				}
				num2 = ((this._seqBytesLeft < (long)length) ? this._seqBytesLeft : ((long)length));
				this._parser.ReadByteArray(buffer, bufferIndex, (int)num2);
				this._seqBytesRead += num2;
				this._seqBytesLeft -= num2;
				return num2;
			}
			else
			{
				if (dataIndex > 2147483647L)
				{
					throw new ArgumentOutOfRangeException();
				}
				byte[] value = this.GetSqlBinary(i).Value;
				int num3 = (int)dataIndex;
				num = value.Length;
				if (buffer == null)
				{
					return (long)num;
				}
				if (num3 < 0 || num3 >= num)
				{
					return 0L;
				}
				try
				{
					if (num3 < num)
					{
						if (num3 + length > num)
						{
							num -= num3;
						}
						else
						{
							num = length;
						}
					}
					Array.Copy(value, num3, buffer, bufferIndex, num);
				}
				catch (Exception ex)
				{
					num = value.Length;
					if (length < 0)
					{
						throw new IndexOutOfRangeException(Res.GetString("SQL_InvalidDataLength", new object[]
						{
							length.ToString()
						}));
					}
					if (bufferIndex < 0 || bufferIndex >= buffer.Length)
					{
						throw new ArgumentOutOfRangeException();
					}
					if (num + bufferIndex > buffer.Length)
					{
						throw new IndexOutOfRangeException(Res.GetString("SQL_InvalidBufferSizeOrIndex", new object[]
						{
							num.ToString(),
							bufferIndex.ToString()
						}));
					}
					throw ex;
				}
				return (long)num;
			}
		}

		/// <summary>获取指定列的单个字符串形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000DA RID: 218 RVA: 0x000073A8 File Offset: 0x000063A8
		public char GetChar(int i)
		{
			throw new NotSupportedException();
		}

		/// <summary>从指定的列偏移量将字符流作为数组从给定的缓冲区偏移量开始读入缓冲区。</summary>
		/// <returns>读取的实际字符数。</returns>
		/// <param name="buffer">要将字节流读入的缓冲区。 </param>
		/// <param name="dataIndex">字段中的索引，从其开始读取操作。</param>
		/// <param name="i">从零开始的列序号。 </param>
		/// <param name="bufferIndex">开始写操作的 buffer 的索引。 </param>
		/// <param name="length">要复制到缓冲区中的最大长度。 </param>
		// Token: 0x060000DB RID: 219 RVA: 0x000073B0 File Offset: 0x000063B0
		public long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			int num = 0;
			string value = this.GetSqlString(i).Value;
			char[] array = value.ToCharArray();
			num = array.Length;
			if (dataIndex > 2147483647L)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num2 = (int)dataIndex;
			if (buffer == null)
			{
				return (long)num;
			}
			if (num2 < 0 || num2 >= num)
			{
				return 0L;
			}
			try
			{
				if (num2 < num)
				{
					if (num2 + length > num)
					{
						num -= num2;
					}
					else
					{
						num = length;
					}
				}
				Array.Copy(array, num2, buffer, bufferIndex, num);
			}
			catch (Exception ex)
			{
				num = array.Length;
				if (length < 0)
				{
					throw new IndexOutOfRangeException(Res.GetString("SQL_InvalidDataLength", new object[]
					{
						length.ToString()
					}));
				}
				if (bufferIndex < 0 || bufferIndex >= buffer.Length)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (num + bufferIndex > buffer.Length)
				{
					throw new IndexOutOfRangeException(Res.GetString("SQL_InvalidBufferSizeOrIndex", new object[]
					{
						num.ToString(),
						bufferIndex.ToString()
					}));
				}
				throw ex;
			}
			return (long)num;
		}

		/// <summary>获取指定列的值作为全局唯一标识符 (GUID)。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000DC RID: 220 RVA: 0x000074B0 File Offset: 0x000064B0
		public Guid GetGuid(int i)
		{
			return this.GetSqlGuid(i).Value;
		}

		/// <summary>获取指定列的 16 位有符号整数形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000DD RID: 221 RVA: 0x000074CC File Offset: 0x000064CC
		public short GetInt16(int i)
		{
			return this.GetSqlInt16(i).Value;
		}

		/// <summary>获取指定列的 32 位有符号整数形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000DE RID: 222 RVA: 0x000074E8 File Offset: 0x000064E8
		public int GetInt32(int i)
		{
			return this.GetSqlInt32(i).Value;
		}

		/// <summary>获取指定列的 64 位有符号整数形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000DF RID: 223 RVA: 0x00007504 File Offset: 0x00006504
		public long GetInt64(int i)
		{
			return this.GetSqlInt64(i).Value;
		}

		/// <summary>获取指定列的单精度浮点数形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E0 RID: 224 RVA: 0x00007520 File Offset: 0x00006520
		public float GetFloat(int i)
		{
			return this.GetSqlSingle(i).Value;
		}

		/// <summary>获取指定列的双精度浮点数形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E1 RID: 225 RVA: 0x0000753C File Offset: 0x0000653C
		public double GetDouble(int i)
		{
			return this.GetSqlDouble(i).Value;
		}

		/// <summary>获取指定列的字符串形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E2 RID: 226 RVA: 0x00007558 File Offset: 0x00006558
		public string GetString(int i)
		{
			return this.GetSqlString(i).Value;
		}

		/// <summary>获取指定列的 <see cref="T:System.Decimal"></see> 对象形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E3 RID: 227 RVA: 0x00007574 File Offset: 0x00006574
		public decimal GetDecimal(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			SqlDbType type = this._metaData[i].type;
            if (type == (SqlDbType)9 || type == (SqlDbType)17)
			{
				return this.GetSqlMoney(i).Value;
			}
			return this.GetSqlDecimal(i).Value;
		}

		/// <summary>获取指定列的 <see cref="T:System.DateTime"></see> 对象形式的值。</summary>
		/// <returns>指定列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E4 RID: 228 RVA: 0x000075C8 File Offset: 0x000065C8
		public DateTime GetDateTime(int i)
		{
			return this.GetSqlDateTime(i).Value;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000075E4 File Offset: 0x000065E4
		public IDataReader GetData(int i)
		{
			throw new NotSupportedException();
		}

		/// <summary>获取一个值，该值指示列中是否包含不存在的或已丢失的值。</summary>
		/// <returns>
		/// 如果指定的列值与 <see cref="T:System.DBNull"></see> 等效，则为 true；否则为 false。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E6 RID: 230 RVA: 0x000075EC File Offset: 0x000065EC
		public bool IsDBNull(int i)
		{
			if (16 != ((int)this._behavior & 16))
			{
				object obj = this.PrepareSQLRecord(i);
				return obj == null || obj == DBNull.Value || ((INullable)obj).IsNull;
			}
			if (!this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			this._peekLength = (int)this.SeqRead(i, true, true, out this._peekIsNull);
			return this._peekIsNull;
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlBoolean"></see> 形式的值。</summary>
		/// <returns>列的值。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E7 RID: 231 RVA: 0x00007655 File Offset: 0x00006655
		public SqlBoolean GetSqlBoolean(int i)
		{
			return (SqlBoolean)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlBinary"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlBinary"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E8 RID: 232 RVA: 0x00007663 File Offset: 0x00006663
		public SqlBinary GetSqlBinary(int i)
		{
			return (SqlBinary)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlByte"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlByte"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000E9 RID: 233 RVA: 0x00007671 File Offset: 0x00006671
		public SqlByte GetSqlByte(int i)
		{
			return (SqlByte)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlInt16"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlInt16"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000EA RID: 234 RVA: 0x0000767F File Offset: 0x0000667F
		public SqlInt16 GetSqlInt16(int i)
		{
			return (SqlInt16)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlInt32"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlInt32"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000EB RID: 235 RVA: 0x0000768D File Offset: 0x0000668D
		public SqlInt32 GetSqlInt32(int i)
		{
			return (SqlInt32)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlInt64"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlInt64"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000EC RID: 236 RVA: 0x0000769B File Offset: 0x0000669B
		public SqlInt64 GetSqlInt64(int i)
		{
			return (SqlInt64)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlSingle"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlSingle"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000ED RID: 237 RVA: 0x000076A9 File Offset: 0x000066A9
		public SqlSingle GetSqlSingle(int i)
		{
			return (SqlSingle)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlDouble"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlDouble"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000EE RID: 238 RVA: 0x000076B7 File Offset: 0x000066B7
		public SqlDouble GetSqlDouble(int i)
		{
			return (SqlDouble)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlString"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlString"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000EF RID: 239 RVA: 0x000076C5 File Offset: 0x000066C5
		public SqlString GetSqlString(int i)
		{
			return (SqlString)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlMoney"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlMoney"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000F0 RID: 240 RVA: 0x000076D3 File Offset: 0x000066D3
		public SqlMoney GetSqlMoney(int i)
		{
			return (SqlMoney)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlDecimal"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlDecimal"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000F1 RID: 241 RVA: 0x000076E1 File Offset: 0x000066E1
		public SqlDecimal GetSqlDecimal(int i)
		{
			return (SqlDecimal)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlDateTime"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlDateTime"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000F2 RID: 242 RVA: 0x000076EF File Offset: 0x000066EF
		public SqlDateTime GetSqlDateTime(int i)
		{
			return (SqlDateTime)this.PrepareSQLRecord(i);
		}

		/// <summary>获取指定列的 <see cref="T:System.Data.SqlTypes.SqlGuid"></see> 形式的值。</summary>
		/// <returns><see cref="T:System.Data.SqlTypes.SqlGuid"></see>。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000F3 RID: 243 RVA: 0x000076FD File Offset: 0x000066FD
		public SqlGuid GetSqlGuid(int i)
		{
			return (SqlGuid)this.PrepareSQLRecord(i);
		}

		/// <summary>获取一个表示基础 <see cref="T:System.Data.SqlDbType"></see> 变量的 <see cref="T:System.Object"></see>。</summary>
		/// <returns>一个 <see cref="T:System.Object"></see>，它表示基础 <see cref="T:System.Data.SqlDbType"></see> 变量。</returns>
		/// <param name="i">从零开始的列序号。 </param>
		// Token: 0x060000F4 RID: 244 RVA: 0x0000770B File Offset: 0x0000670B
		public object GetSqlValue(int i)
		{
			return this.PrepareSQLRecord(i);
		}

		/// <summary>获取当前行中的所有属性列。</summary>
		/// <returns>数组中 <see cref="T:System.Object"></see> 的实例的数目。</returns>
		/// <param name="values">要将属性列复制到其中的 <see cref="T:System.Object"></see> 的数组。 </param>
		// Token: 0x060000F5 RID: 245 RVA: 0x00007714 File Offset: 0x00006714
		public int GetSqlValues(object[] values)
		{
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			int num = (values.Length < this._visibleColumns) ? values.Length : this._visibleColumns;
			if (((int)this._behavior & 16) != null)
			{
				for (int i = 0; i < num; i++)
				{
					values[this._indexMap[i]] = this.SeqRead(i, true, false);
				}
			}
			else
			{
				this.PrepareSQLRecord(0);
				for (int j = 0; j < num; j++)
				{
					values[j] = this._sqlBuf[j];
				}
			}
			return num;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000077A8 File Offset: 0x000067A8
		private bool HasMoreRows
		{
			get
			{
				if (this._parser != null)
				{
					if (this._dataReady)
					{
						return true;
					}
					if (this._parser.PendingData)
					{
						byte b = this._parser.PeekByte();
						if (b == 169)
						{
							this._parser.Run(RunBehavior.ReturnImmediately, null, null);
							b = this._parser.PeekByte();
						}
						if (209 == b)
						{
							return true;
						}
						while (b == 253 || b == 254 || b == 255)
						{
							this._parser.Run(RunBehavior.ReturnImmediately, this._command, this);
							b = this._parser.PeekByte();
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000784C File Offset: 0x0000684C
		private bool HasMoreResults
		{
			get
			{
				if (this._parser != null)
				{
					if (this.HasMoreRows)
					{
						return true;
					}
					while (this._parser.PendingData)
					{
						byte b = this._parser.PeekByte();
						if (129 == b)
						{
							return true;
						}
						this._parser.Run(RunBehavior.ReturnImmediately, this._command);
					}
				}
				return false;
			}
		}

		/// <summary>检索一个布尔值，该值指示是否已关闭指定的 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 实例。 </summary>
		/// <returns>
		/// 如果指定的 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 实例已关闭，则为 true；否则为 false。 </returns>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000078A1 File Offset: 0x000068A1
		public bool IsClosed
		{
			get
			{
				return this._isClosed;
			}
		}

		/// <summary>获取执行 Transact-SQL 语句所更改、插入或删除的行数。</summary>
		/// <returns>已更改、插入或删除的行数；如果没有任何行受到影响或语句失败，则为 0；-1 表示 SELECT 语句。</returns>
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000078A9 File Offset: 0x000068A9
		public int RecordsAffected
		{
			get
			{
				if (this._command != null)
				{
					return this._command.RecordsAffected;
				}
				return this._recordsAffected;
			}
		}

		/// <summary>使 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 前进到下一条记录。</summary>
		/// <returns>
		/// 如果存在多个行，则为 true；否则为 false。</returns>
		// Token: 0x060000FA RID: 250 RVA: 0x000078C8 File Offset: 0x000068C8
		public bool Read()
		{
			bool flag = false;
			if (this._parser != null)
			{
				if (this._dataReady)
				{
					this.CleanPartialRead(this._parser);
				}
				this._dataReady = false;
				this._sqlBuf = (this._comBuf = null);
				this._currCol = -1;
				if (this.HasMoreRows && !this._haltRead)
				{
					try
					{
						while (this._parser.PendingData && !this._dataReady)
						{
							this._dataReady = this._parser.Run(RunBehavior.ReturnImmediately, this._command, this);
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
					flag = this._dataReady;
					if (this._dataReady && 8 == ((int)this._behavior & 8))
					{
						this._haltRead = true;
					}
				}
				if (!flag && !this._parser.PendingData && !this._haltRead)
				{
					this.InternalClose(false);
				}
				if (!flag && this._haltRead)
				{
					while (this.HasMoreRows)
					{
						while (this._parser.PendingData && !this._dataReady)
						{
							this._dataReady = this._parser.Run(RunBehavior.ReturnImmediately, this._command, this);
						}
						if (this._dataReady)
						{
							this.CleanPartialRead(this._parser);
						}
						this._dataReady = false;
						this._sqlBuf = (this._comBuf = null);
						this._currCol = -1;
					}
					this._haltRead = false;
				}
			}
			else if (this.IsClosed)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", new object[]
				{
					"Read"
				}));
			}
			return flag;
		}

		/// <summary>当读取批处理 Transact-SQL 语句的结果时，使数据读取器前进到下一个结果。</summary>
		/// <returns>
		/// 如果存在多个结果集，则为 true；否则为 false。</returns>
		// Token: 0x060000FB RID: 251 RVA: 0x00007A5C File Offset: 0x00006A5C
		public bool NextResult()
		{
			bool result = false;
			if (1 == ((int)this._behavior & 1))
			{
				this.InternalClose(false);
				this.SetMetaData(null, false);
				return result;
			}
			if (this._parser != null)
			{
				while (this.Read())
				{
				}
				if (this._parser != null)
				{
					if (this.HasMoreResults)
					{
						this._metaDataConsumed = false;
						this._browseModeInfoConsumed = false;
						this.ConsumeMetaData();
						result = true;
					}
					else
					{
						this.InternalClose(false);
						this.SetMetaData(null, false);
					}
				}
			}
			else if (this.IsClosed)
			{
				throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", new object[]
				{
					"NextResult"
				}));
			}
			return result;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007AFC File Offset: 0x00006AFC
		private void RestoreServerSettings(TdsParser parser)
		{
			if (parser != null && this._setOptions != null)
			{
				if (parser.State == TdsParserState.OpenLoggedIn)
				{
					parser.TdsExecuteSQLBatch(this._setOptions, (this._command != null) ? this._command.CommandTimeout : 0);
					parser.Run(RunBehavior.UntilDone, this._command, this);
				}
				this._setOptions = null;
			}
		}

		/// <summary>关闭 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 对象。</summary>
		// Token: 0x060000FD RID: 253 RVA: 0x00007B55 File Offset: 0x00006B55
		public void Close()
		{
			if (this.IsClosed)
			{
				return;
			}
			this.InternalClose(true);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007B67 File Offset: 0x00006B67
		private void ResetBlobState(TdsParser parser)
		{
			if (this._seqBytesLeft > 0L)
			{
				parser.SkipBytes(this._seqBytesLeft);
				this._seqBytesLeft = 0L;
			}
			this._seqBytesRead = 0L;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007B90 File Offset: 0x00006B90
		private void CleanPartialRead(TdsParser parser)
		{
			if (((int)this._behavior & 16) == null)
			{
				if (this._sqlBuf == null && this._comBuf == null)
				{
					parser.SkipRow(this._metaData);
					return;
				}
			}
			else
			{
				if (-1 == this._currCol)
				{
					parser.SkipRow(this._metaData);
					return;
				}
				this.ResetBlobState(parser);
				parser.SkipRow(this._metaData, this._currCol);
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007BF4 File Offset: 0x00006BF4
		private void InternalClose(bool closeReader)
		{
			TdsParser parser = this._parser;
			bool flag = 32 == ((int)this._behavior & 32);
			this._parser = null;
			Exception ex = null;
			try
			{
				if (parser != null && parser.PendingData && parser.State == TdsParserState.OpenLoggedIn)
				{
					if (this._dataReady)
					{
						this.CleanPartialRead(parser);
					}
					parser.Run(RunBehavior.Clean, this._command, this);
				}
				this.RestoreServerSettings(parser);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (closeReader)
			{
				if (this._command != null && this._command.Connection != null)
				{
					this._command.Connection.Reader = null;
				}
				this.SetMetaData(null, false);
				this._dataReady = false;
				this._isClosed = true;
				if (flag && this._command != null && this._command.Connection != null)
				{
					try
					{
						this._command.Connection.Close();
					}
					catch (Exception ex3)
					{
						if (ex == null)
						{
							ex = ex3;
						}
					}
				}
				if (this._command != null)
				{
					this._recordsAffected = this._command.RecordsAffected;
				}
				this._command = null;
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007D18 File Offset: 0x00006D18
		internal void SetMetaData(_SqlMetaData[] metaData, bool moreInfo)
		{
			this._metaData = metaData;
			this._tableNames = null;
			this._tableNamesShilohSP1 = null;
			this._schemaTable = null;
			this._sqlBuf = (this._comBuf = null);
			this._fieldNameLookup = null;
			if (metaData != null)
			{
				if (!moreInfo)
				{
					this._metaDataConsumed = true;
				}
			}
			else
			{
				this._metaDataConsumed = false;
			}
			this._browseModeInfoConsumed = false;
		}

		// Token: 0x1700002C RID: 44
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00007D75 File Offset: 0x00006D75
		internal bool BrowseModeInfoConsumed
		{
			set
			{
				this._browseModeInfoConsumed = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00007D80 File Offset: 0x00006D80
		internal _SqlMetaData[] MetaData
		{
			get
			{
				if (this.IsClosed)
				{
					throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", new object[]
					{
						"read data"
					}));
				}
				if (this._metaData == null && !this._metaDataConsumed)
				{
					this.ConsumeMetaData();
				}
				return this._metaData;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007DD4 File Offset: 0x00006DD4
		private void ConsumeMetaData()
		{
			if (this._parser != null)
			{
				while (this._parser.PendingData && !this._metaDataConsumed)
				{
					this._parser.Run(RunBehavior.ReturnImmediately, this._command, this);
				}
			}
			if (this._metaData != null)
			{
				this._visibleColumns = 0;
				this._indexMap = new int[this._metaData.Length];
				for (int i = 0; i < this._metaData.Length; i++)
				{
					this._indexMap[i] = this._visibleColumns;
					if (!this._metaData[i].isHidden)
					{
						this._visibleColumns++;
					}
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00007E74 File Offset: 0x00006E74
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00007E7C File Offset: 0x00006E7C
		internal string[] TableNames
		{
			get
			{
				return this._tableNames;
			}
			set
			{
				this._tableNames = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00007E85 File Offset: 0x00006E85
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00007E8D File Offset: 0x00006E8D
		internal string[][] TableNamesShilohSP1
		{
			get
			{
				return this._tableNamesShilohSP1;
			}
			set
			{
				this._tableNamesShilohSP1 = value;
			}
		}

		/// <summary>返回一个 <see cref="T:System.Data.DataTable"></see>，它描述 <see cref="T:System.Data.SqlClient.SqlDataReader"></see> 的列元数据。</summary>
		/// <returns>一个描述列元数据的 <see cref="T:System.Data.DataTable"></see>。</returns>
		// Token: 0x06000109 RID: 265 RVA: 0x00007E96 File Offset: 0x00006E96
		public DataTable GetSchemaTable()
		{
			if (this._schemaTable == null && this.MetaData != null)
			{
				this._schemaTable = this.BuildSchemaTable();
			}
			return this._schemaTable;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007EBC File Offset: 0x00006EBC
		internal DataTable BuildSchemaTable()
		{
			_SqlMetaData[] metaData = this.MetaData;
			DataTable dataTable = ADP.CreateSchemaTable(null, metaData.Length);
			DBSchemaTable dbschemaTable = new DBSchemaTable(dataTable);
			foreach (_SqlMetaData sqlMetaData in metaData)
			{
				DBSchemaRow dbschemaRow = dbschemaTable.NewRow();
				dbschemaRow.ColumnName = sqlMetaData.column;
				dbschemaRow.Ordinal = sqlMetaData.ordinal;
				dbschemaRow.Size = (MetaType.IsSizeInCharacters(sqlMetaData.type) ? (sqlMetaData.length / 2) : sqlMetaData.length);
				dbschemaRow.ProviderType = (int)sqlMetaData.type;
				dbschemaRow.DataType = sqlMetaData.metaType.ClassType;
				if (255 != sqlMetaData.precision)
				{
					dbschemaRow.Precision = (short)sqlMetaData.precision;
				}
				else
				{
					dbschemaRow.Precision = (short)sqlMetaData.metaType.Precision;
				}
				if (255 != sqlMetaData.scale)
				{
					dbschemaRow.Scale = (short)sqlMetaData.scale;
				}
				else
				{
					dbschemaRow.Scale = (short)sqlMetaData.metaType.Scale;
				}
				dbschemaRow.AllowDBNull = sqlMetaData.isNullable;
				if (this._browseModeInfoConsumed)
				{
					dbschemaRow.IsAliased = sqlMetaData.isDifferentName;
					dbschemaRow.IsKey = sqlMetaData.isKey;
					dbschemaRow.IsHidden = sqlMetaData.isHidden;
					dbschemaRow.IsExpression = sqlMetaData.isExpression;
				}
				dbschemaRow.IsIdentity = sqlMetaData.isIdentity;
				dbschemaRow.IsAutoIncrement = sqlMetaData.isIdentity;
				dbschemaRow.IsLong = sqlMetaData.metaType.IsLong;
				if (19 == (int)sqlMetaData.type)
				{
					dbschemaRow.IsUnique = true;
					dbschemaRow.IsRowVersion = true;
				}
				else
				{
					dbschemaRow.IsUnique = false;
					dbschemaRow.IsRowVersion = false;
				}
				dbschemaRow.IsReadOnly = (0 == sqlMetaData.updatability);
				if (!ADP.IsEmpty(sqlMetaData.serverName))
				{
					dbschemaRow.BaseServerName = sqlMetaData.serverName;
				}
				if (!ADP.IsEmpty(sqlMetaData.catalogName))
				{
					dbschemaRow.BaseCatalogName = sqlMetaData.catalogName;
				}
				if (!ADP.IsEmpty(sqlMetaData.schemaName))
				{
					dbschemaRow.BaseSchemaName = sqlMetaData.schemaName;
				}
				if (!ADP.IsEmpty(sqlMetaData.tableName))
				{
					dbschemaRow.BaseTableName = sqlMetaData.tableName;
				}
				if (!ADP.IsEmpty(sqlMetaData.baseColumn))
				{
					dbschemaRow.BaseColumnName = sqlMetaData.baseColumn;
				}
				else if (!ADP.IsEmpty(sqlMetaData.column))
				{
					dbschemaRow.BaseColumnName = sqlMetaData.column;
				}
				dbschemaTable.AddRow(dbschemaRow);
			}
			DataColumnCollection columns = dataTable.Columns;
			for (int j = 0; j < columns.Count; j++)
			{
				columns[j].ReadOnly = true;
			}
			return dataTable;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008164 File Offset: 0x00007164
		private object PrepareRecord(int i)
		{
			if (!this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (this._comBuf != null)
			{
				return this._comBuf[i];
			}
			if (((int)this._behavior & 16) != null)
			{
				return this.SeqRead(i, false, false);
			}
			if (this._sqlBuf != null)
			{
				this.SqlBufferToComBuffer();
				return this._comBuf[i];
			}
			this._comBuf = new object[this._metaData.Length];
			this._parser.ProcessRow(this._metaData, this._comBuf, this._indexMap, false);
			return this._comBuf[i];
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000081F8 File Offset: 0x000071F8
		private object SeqRead(int i, bool useSQLTypes, bool byteAccess)
		{
			bool flag;
			return this.SeqRead(i, useSQLTypes, byteAccess, out flag);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008210 File Offset: 0x00007210
		private object SeqRead(int i, bool useSQLTypes, bool byteAccess, out bool isNull)
		{
			isNull = false;
			int num = 0;
			bool flag = false;
			_SqlMetaData sqlMetaData = this._metaData[i];
			if (i < this._currCol)
			{
				if (this._peekLength == -1 || i != this._currCol - 1)
				{
					throw new InvalidOperationException(Res.GetString("ADP_NonSequentialColumnAccess", new object[]
					{
						i.ToString(),
						this._currCol.ToString()
					}));
				}
				num = this._peekLength;
				isNull = this._peekIsNull;
				flag = true;
			}
			this._peekLength = -1;
			this.ResetBlobState(this._parser);
			while (i > this._currCol)
			{
				if (this._currCol > -1)
				{
					this._parser.SkipBytes((long)this._parser.ProcessColumnHeader(this._metaData[this._currCol], ref isNull));
				}
				this._currCol++;
			}
			if (!flag)
			{
				num = this._parser.ProcessColumnHeader(sqlMetaData, ref isNull);
			}
			object result;
			if (byteAccess)
			{
				result = num;
			}
			else if (isNull)
			{
				result = (useSQLTypes ? this._parser.GetNullSqlValue(sqlMetaData) : DBNull.Value);
			}
			else
			{
				try
				{
					result = (useSQLTypes ? this._parser.ReadSqlValue(sqlMetaData, num) : this._parser.ReadValue(sqlMetaData, num));
				}
				catch (_ValueException ex)
				{
					result = ex.value;
					this._rowException = ex.exception;
				}
			}
			if (!flag)
			{
				this._currCol++;
			}
			return result;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008388 File Offset: 0x00007388
		private object PrepareSQLRecord(int i)
		{
			if (!this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (this._sqlBuf != null)
			{
				return this._sqlBuf[i];
			}
			if (((int)this._behavior & 16) != null)
			{
				return this.SeqRead(i, true, false);
			}
			if (this._comBuf != null)
			{
				this.ComBufferToSqlBuffer();
				return this._sqlBuf[i];
			}
			this._sqlBuf = new object[this._metaData.Length];
			this._parser.ProcessRow(this._metaData, this._sqlBuf, this._indexMap, true);
			return this._sqlBuf[i];
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000841C File Offset: 0x0000741C
		private void ComBufferToSqlBuffer()
		{
			object[] array = new object[this._comBuf.Length];
			for (int i = 0; i < this._metaData.Length; i++)
			{
				array[i] = MetaType.GetSqlValue(this._metaData[i].type, this._comBuf[i]);
			}
			this._sqlBuf = array;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008470 File Offset: 0x00007470
		private void SqlBufferToComBuffer()
		{
			object[] array = new object[this._sqlBuf.Length];
			for (int i = 0; i < this._metaData.Length; i++)
			{
				array[i] = MetaType.GetComValue(this._metaData[i].type, this._sqlBuf[i]);
			}
			this._comBuf = array;
		}

		// Token: 0x17000030 RID: 48
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000084C2 File Offset: 0x000074C2
		internal string SetOptionsOFF
		{
			set
			{
				this._setOptions = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (set) Token: 0x06000112 RID: 274 RVA: 0x000084CB File Offset: 0x000074CB
		internal CommandBehavior Behavior
		{
			set
			{
				this._behavior = value;
			}
		}

		// Token: 0x040000B6 RID: 182
		private TdsParser _parser;

		// Token: 0x040000B7 RID: 183
		private SqlCommand _command;

		// Token: 0x040000B8 RID: 184
		private int _defaultLCID;

		// Token: 0x040000B9 RID: 185
		private bool _dataReady;

		// Token: 0x040000BA RID: 186
		private bool _metaDataConsumed;

		// Token: 0x040000BB RID: 187
		private bool _browseModeInfoConsumed;

		// Token: 0x040000BC RID: 188
		private bool _isClosed;

		// Token: 0x040000BD RID: 189
		private int _recordsAffected = -1;

		// Token: 0x040000BE RID: 190
		private object[] _comBuf;

		// Token: 0x040000BF RID: 191
		private object[] _sqlBuf;

		// Token: 0x040000C0 RID: 192
		private int[] _indexMap;

		// Token: 0x040000C1 RID: 193
		private int _visibleColumns;

		// Token: 0x040000C2 RID: 194
		private _SqlMetaData[] _metaData;

		// Token: 0x040000C3 RID: 195
		private FieldNameLookup _fieldNameLookup;

		// Token: 0x040000C4 RID: 196
		private string[] _tableNames;

		// Token: 0x040000C5 RID: 197
		private string[][] _tableNamesShilohSP1;

		// Token: 0x040000C6 RID: 198
		private string _setOptions;

		// Token: 0x040000C7 RID: 199
		private CommandBehavior _behavior;

		// Token: 0x040000C8 RID: 200
		private bool _haltRead;

		// Token: 0x040000C9 RID: 201
		private int _currCol;

		// Token: 0x040000CA RID: 202
		private long _seqBytesRead;

		// Token: 0x040000CB RID: 203
		private long _seqBytesLeft;

		// Token: 0x040000CC RID: 204
		private int _peekLength = -1;

		// Token: 0x040000CD RID: 205
		private bool _peekIsNull;

		// Token: 0x040000CE RID: 206
		private Exception _rowException;

		// Token: 0x040000CF RID: 207
		private DataTable _schemaTable;
	}
}
