using System;
using System.Data.SqlTypes;
using System.Globalization;

namespace System.Data.SqlClient
{
	/// <summary>表示 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 的参数，也可以是它到 <see cref="T:System.Data.DataSet"></see> 列的映射。无法继承此类。</summary>
	// Token: 0x02000028 RID: 40
	public sealed class SqlParameter : MarshalByRefObject, IDbDataParameter, IDataParameter, ICloneable
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 类的新实例。</summary>
		// Token: 0x0600018C RID: 396 RVA: 0x0000A344 File Offset: 0x00009344
		public SqlParameter()
		{
		}

		/// <summary>用参数名称和新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的一个值初始化 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 类的新实例。</summary>
		/// <param name="value">一个 <see cref="T:System.Object"></see>，它是 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的值。 </param>
		/// <param name="parameterName">要映射的参数的名称。 </param>
		// Token: 0x0600018D RID: 397 RVA: 0x0000A378 File Offset: 0x00009378
		public SqlParameter(string parameterName, object value)
		{
			this.ParameterName = parameterName;
			this.Value = value;
		}

		/// <summary>用参数名称和数据类型初始化 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 类的新实例。</summary>
		/// <param name="dbType"><see cref="T:System.Data.SqlDbType"></see> 值之一。 </param>
		/// <param name="parameterName">要映射的参数的名称。 </param>
		// Token: 0x0600018E RID: 398 RVA: 0x0000A3C4 File Offset: 0x000093C4
		public SqlParameter(string parameterName, SqlDbType dbType)
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
		}

		/// <summary>用参数名称、<see cref="T:System.Data.SqlDbType"></see> 和大小初始化 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 类的新实例。</summary>
		/// <param name="size">参数的长度。 </param>
		/// <param name="dbType"><see cref="T:System.Data.SqlDbType"></see> 值之一。 </param>
		/// <param name="parameterName">要映射的参数的名称。 </param>
		// Token: 0x0600018F RID: 399 RVA: 0x0000A410 File Offset: 0x00009410
		public SqlParameter(string parameterName, SqlDbType dbType, int size)
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
		}

		/// <summary>用参数名称、<see cref="T:System.Data.SqlDbType"></see>、大小和源列名称初始化 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 类的新实例。</summary>
		/// <param name="size">参数的长度。 </param>
		/// <param name="sourceColumn">源列的名称。 </param>
		/// <param name="dbType"><see cref="T:System.Data.SqlDbType"></see> 值之一。 </param>
		/// <param name="parameterName">要映射的参数的名称。 </param>
		// Token: 0x06000190 RID: 400 RVA: 0x0000A464 File Offset: 0x00009464
		public SqlParameter(string parameterName, SqlDbType dbType, int size, string sourceColumn)
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
			this.SourceColumn = sourceColumn;
		}

		/// <summary>用参数名称、参数的类型、参数的大小、<see cref="T:System.Data.ParameterDirection"></see>、参数的精度、参数的小数位数、源列、要使用的 <see cref="T:System.Data.DataRowVersion"></see> 和参数的值初始化 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 类的新实例。</summary>
		/// <param name="size">参数的长度。 </param>
		/// <param name="sourceVersion"><see cref="T:System.Data.DataRowVersion"></see> 值之一。 </param>
		/// <param name="isNullable">
		/// 如果字段的值可为空，则为 true；否则为 false。 </param>
		/// <param name="scale">要将 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 解析为的总小数位数。 </param>
		/// <param name="precision">要将 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 解析为的小数点左右两侧的总位数。 </param>
		/// <param name="dbType"><see cref="T:System.Data.SqlDbType"></see> 值之一。 </param>
		/// <param name="sourceColumn">源列的名称。 </param>
		/// <param name="value">一个 <see cref="T:System.Object"></see>，它是 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的值。 </param>
		/// <param name="direction"><see cref="T:System.Data.ParameterDirection"></see> 值之一。 </param>
		/// <param name="parameterName">要映射的参数的名称。 </param>
		// Token: 0x06000191 RID: 401 RVA: 0x0000A4C0 File Offset: 0x000094C0
		public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
			this.Direction = direction;
			this.IsNullable = isNullable;
			this.Precision = precision;
			this.Scale = scale;
			this.SourceColumn = sourceColumn;
			this.SourceVersion = sourceVersion;
			this.Value = value;
		}

		/// <summary>获取一个包含 <see cref="P:System.Data.SqlClient.SqlParameter.ParameterName"></see> 的字符串。</summary>
		/// <returns>一个包含 <see cref="P:System.Data.SqlClient.SqlParameter.ParameterName"></see> 的字符串。</returns>
		// Token: 0x06000192 RID: 402 RVA: 0x0000A54B File Offset: 0x0000954B
		public override string ToString()
		{
			return this.ParameterName;
		}

		/// <summary>获取或设置参数的 <see cref="T:System.Data.SqlDbType"></see>。</summary>
		/// <returns><see cref="T:System.Data.SqlDbType"></see> 值之一。默认为 NVarChar。</returns>
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000A553 File Offset: 0x00009553
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000A560 File Offset: 0x00009560
		public DbType DbType
		{
			get
			{
				return this._metaType.DbType;
			}
			set
			{
				this.SetTypeInfoFromDbType(value);
				this._inferType = false;
			}
		}

		/// <summary>获取或设置参数的 <see cref="T:System.Data.SqlDbType"></see>。</summary>
		/// <returns><see cref="T:System.Data.SqlDbType"></see> 值之一。默认为 NVarChar。</returns>
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000A570 File Offset: 0x00009570
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000A580 File Offset: 0x00009580
		public SqlDbType SqlDbType
		{
			get
			{
				return this._metaType.SqlDbType;
			}
			set
			{
				try
				{
					this._metaType = MetaType.GetMetaType(value);
				}
				catch (IndexOutOfRangeException)
				{
					throw SQL.InvalidSqlDbType(this.ParameterName, (int)value);
				}
				this._inferType = false;
			}
		}

		/// <summary>获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数。</summary>
		/// <returns><see cref="T:System.Data.ParameterDirection"></see> 值之一。默认为 Input。</returns>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000A5C0 File Offset: 0x000095C0
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000A5C8 File Offset: 0x000095C8
		public ParameterDirection Direction
		{
			get
			{
				return this._direction;
			}
			set
			{
				int direction = (int)value;
                if ((int)value != 1 && (int)value != 2 && (int)value != 3 && (int)value != 6)
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidParameterDirection", new object[]
					{
						direction.ToString(),
						this.ParameterName
					}));
				}
				this._direction = (ParameterDirection)direction;
			}
		}

		/// <summary>获取或设置一个值，该值指示参数是否接受空值。</summary>
		/// <returns>
		/// 如果接受空值，则为 true；否则为 false。默认为 false。</returns>
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000A619 File Offset: 0x00009619
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000A621 File Offset: 0x00009621
		public bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
			set
			{
				this._isNullable = value;
			}
		}

		/// <summary>获取或设置对 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 属性的偏移量。</summary>
		/// <returns>对 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 的偏移量。默认值为 0。</returns>
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000A62A File Offset: 0x0000962A
		// (set) Token: 0x0600019C RID: 412 RVA: 0x0000A632 File Offset: 0x00009632
		public int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		/// <summary>获取或设置 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的名称。</summary>
		/// <returns><see cref="T:System.Data.SqlClient.SqlParameter"></see> 的名称。默认为空字符串。</returns>
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000A63B File Offset: 0x0000963B
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000A651 File Offset: 0x00009651
		public string ParameterName
		{
			get
			{
				if (this._name == null)
				{
					return string.Empty;
				}
				return this._name;
			}
			set
			{
				if (string.Compare(value, this._name, false, CultureInfo.InvariantCulture) != 0)
				{
					if (value != null && value.Length > 127)
					{
						throw SQL.InvalidParameterNameLength(value);
					}
					this._name = value;
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000A682 File Offset: 0x00009682
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000A68A File Offset: 0x0000968A
		internal SqlParameterCollection Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		/// <summary>获取或设置用来表示 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 属性的最大位数。</summary>
		/// <returns>用于表示 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 属性的最大位数。默认值为 0，它指示数据提供程序设置 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 的精度。</returns>
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000A694 File Offset: 0x00009694
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x0000A6D3 File Offset: 0x000096D3
		public byte Precision
		{
			get
			{
				byte precision = this._precision;
				if (precision == 0 && this.SqlDbType == (SqlDbType)5)
				{
					object value = this.Value;
					if (value is SqlDecimal)
					{
						precision = ((SqlDecimal)value).Precision;
					}
				}
				return precision;
			}
			set
			{
				if (value > 38)
				{
					throw SQL.PrecisionValueOutOfRange((int)value);
				}
				this._precision = value;
			}
		}

		/// <summary>获取或设置 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 解析为的小数位数。</summary>
		/// <returns>要将 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 解析为的小数位数。默认值为 0。</returns>
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000A6E8 File Offset: 0x000096E8
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000A76E File Offset: 0x0000976E
		public byte Scale
		{
			get
			{
				byte b = this._scale;
                if (b == 0 && this.SqlDbType == (SqlDbType)5)
				{
					object obj = this.Value;
					if (this._value != null && !Convert.IsDBNull(this._value))
					{
						if (obj is SqlDecimal)
						{
							b = ((SqlDecimal)obj).Scale;
						}
						else
						{
							IFormatProvider currentCulture = CultureInfo.CurrentCulture;
							obj = Convert.ChangeType(obj, typeof(decimal), currentCulture);
							b = (byte)((decimal.GetBits((decimal)obj)[3] & 16711680) >> 16);
						}
					}
				}
				return b;
			}
			set
			{
				this._scale = value;
			}
		}

		/// <summary>获取或设置列中数据的最大大小（以字节为单位）。</summary>
		/// <returns>列中数据的最大大小（以字节为单位）。默认值是从参数值推导出的。</returns>
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000A777 File Offset: 0x00009777
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x0000A78C File Offset: 0x0000978C
		public int Size
		{
			get
			{
				if (this._forceSize)
				{
					return this._size;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidSizeValue", new object[]
					{
						value.ToString()
					}));
				}
				if (value != 0)
				{
					this._forceSize = true;
					this._size = value;
					return;
				}
				this._forceSize = false;
				this._size = -1;
			}
		}

		/// <summary>获取或设置源列的名称，该源列映射到 <see cref="T:System.Data.DataSet"></see> 并用于加载或返回 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see></summary>
		/// <returns>映射到 <see cref="T:System.Data.DataSet"></see> 的源列的名称。默认为空字符串。</returns>
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000A7DF File Offset: 0x000097DF
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000A7F5 File Offset: 0x000097F5
		public string SourceColumn
		{
			get
			{
				if (this._sourceColumn == null)
				{
					return string.Empty;
				}
				return this._sourceColumn;
			}
			set
			{
				this._sourceColumn = value;
			}
		}

		/// <summary>获取或设置在加载 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see> 时要使用的 <see cref="T:System.Data.DataRowVersion"></see>。</summary>
		/// <returns><see cref="T:System.Data.DataRowVersion"></see> 值之一。默认为 Current。</returns>
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000A7FE File Offset: 0x000097FE
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000A808 File Offset: 0x00009808
		public DataRowVersion SourceVersion
		{
			get
			{
				return this._version;
			}
			set
			{
				int num = (int)value;
				if ((int)value != 256 && (int)value != 512 && (int)value != 1024 && (int)value != 1536)
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataRowVersion", new object[]
					{
						this.ParameterName,
						num.ToString()
					}));
				}
				this._version = value;
			}
		}

		/// <summary>获取或设置该参数的值。</summary>
		/// <returns>一个 <see cref="T:System.Object"></see>，它是该参数的值。默认值为 null。</returns>
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000A869 File Offset: 0x00009869
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000A871 File Offset: 0x00009871
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				if (this._inferType && !Convert.IsDBNull(this._value) && this._value != null)
				{
					this.SetTypeInfoFromComType(value.GetType());
				}
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A8A4 File Offset: 0x000098A4
		internal void Prepare(SqlCommand cmd)
		{
			if (this._inferType)
			{
				throw new InvalidOperationException(Res.GetString("ADP_PrepareParameterType", new object[]
				{
					cmd.GetType().Name
				}));
			}
			if (this.Size == 0 && !this._metaType.IsFixed)
			{
				throw new InvalidOperationException(Res.GetString("ADP_PrepareParameterSize", new object[]
				{
					cmd.GetType().Name
				}));
			}
            if (this.Precision == 0 && this.Scale == 0 && this._metaType.SqlDbType == (SqlDbType)5)
			{
				throw new InvalidOperationException(Res.GetString("ADP_PrepareParameterScale", new object[]
				{
					cmd.GetType().Name
				}));
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A95E File Offset: 0x0000995E
		internal void SetTypeInfoFromDbType(DbType type)
		{
			this._metaType = MetaType.GetMetaType(type);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000A96C File Offset: 0x0000996C
		internal void SetTypeInfoFromComType(Type type)
		{
			this._metaType = MetaType.GetMetaType(type);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000A97A File Offset: 0x0000997A
		internal MetaType GetMetaType()
		{
			return this._metaType;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000A982 File Offset: 0x00009982
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000A98A File Offset: 0x0000998A
		internal SqlCollation Collation
		{
			get
			{
				return this._collation;
			}
			set
			{
				this._collation = value;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000A994 File Offset: 0x00009994
		internal int GetBinSize(object val)
		{
			Type type = val.GetType();
			if (type == typeof(byte[]))
			{
				return ((byte[])val).Length;
			}
			if (type == typeof(SqlBinary))
			{
				return ((SqlBinary)val).Value.Length;
			}
			return 0;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000A9E0 File Offset: 0x000099E0
		internal int GetStringSize(object val)
		{
			Type type = val.GetType();
			if (type == typeof(string))
			{
				return ((string)val).Length;
			}
			if (type == typeof(SqlString))
			{
				return ((SqlString)val).Value.Length;
			}
			if (type == typeof(char[]))
			{
				return ((char[])val).Length;
			}
			if (type == typeof(byte[]))
			{
				return ((byte[])val).Length;
			}
			if (type == typeof(byte) || type == typeof(char))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000AA78 File Offset: 0x00009A78
		internal int ActualSize
		{
			get
			{
				int num = 0;
				SqlDbType sqlDbType = this._metaType.SqlDbType;
				MetaType metaType = this._metaType;
				object obj = this.Value;
				bool flag = false;
				if (obj == null || Convert.IsDBNull(obj))
				{
					return 0;
				}
				if (obj is INullable && ((INullable)obj).IsNull)
				{
					return 0;
				}
                if (sqlDbType == (SqlDbType)23)
				{
					metaType = MetaType.GetMetaType(obj);
					sqlDbType = MetaType.GetSqlDataType((int)metaType.TDSType, 0, 0);
					flag = true;
				}
				else if (obj.GetType() != metaType.ClassType && obj.GetType() != metaType.SqlType && obj.GetType() != typeof(Guid))
				{
					IFormatProvider currentCulture = CultureInfo.CurrentCulture;
					obj = Convert.ChangeType(obj, metaType.ClassType, currentCulture);
				}
				if (metaType.IsFixed)
				{
					return metaType.FixedLength;
				}
				int num2 = 0;
				SqlDbType sqlDbType2 = sqlDbType;
				switch (sqlDbType2)
				{
				case (SqlDbType)1:
					goto IL_165;
                case (SqlDbType)2:
					goto IL_18B;
                case (SqlDbType)3:
					break;
				default:
					switch (sqlDbType2)
					{
                        case (SqlDbType)7:
						goto IL_165;
                        case (SqlDbType)8:
                        case (SqlDbType)9:
						goto IL_18B;
                        case (SqlDbType)10:
                        case (SqlDbType)11:
                        case (SqlDbType)12:
						num2 = this.GetStringSize(obj);
						num = ((this._forceSize && this._size <= num2) ? this._size : num2);
						num <<= 1;
						goto IL_18B;
					default:
						switch (sqlDbType2)
						{
                            case (SqlDbType)18:
                            case (SqlDbType)22:
							break;
                            case (SqlDbType)19:
                            case (SqlDbType)21:
							goto IL_165;
                            case (SqlDbType)20:
							goto IL_18B;
						default:
							goto IL_18B;
						}
						break;
					}
					break;
				}
				num2 = this.GetStringSize(obj);
				num = ((this._forceSize && this._size <= num2) ? this._size : num2);
				goto IL_18B;
				IL_165:
				num2 = this.GetBinSize(obj);
				num = ((this._forceSize && this._size <= num2) ? this._size : num2);
				IL_18B:
				if (flag && num2 > 8000)
				{
					throw SQL.ParameterInvalidVariant(this.ParameterName);
				}
				return num;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000AC2A File Offset: 0x00009C2A
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000AC32 File Offset: 0x00009C32
		internal bool Suppress
		{
			get
			{
				return this._suppress;
			}
			set
			{
				this._suppress = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000AC3C File Offset: 0x00009C3C
		internal object CoercedValue
		{
			get
			{
				if (this._value == null || Convert.IsDBNull(this._value))
				{
					return null;
				}
				Type type = this._value.GetType();
				if (type == this._metaType.SqlType || type == this._metaType.ClassType)
				{
					return this._value;
				}
				if (type == typeof(Guid))
				{
					return this._value;
				}
				IFormatProvider currentCulture = CultureInfo.CurrentCulture;
				return Convert.ChangeType(this._value, this._metaType.ClassType, currentCulture);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000ACC4 File Offset: 0x00009CC4
		internal void SetProperties(string name, string column, DataRowVersion version, byte precision, byte scale, int size, bool forceSize, int offset, ParameterDirection direction, object value, SqlDbType type, bool suppress, bool inferType)
		{
			this.ParameterName = name;
			this._sourceColumn = column;
			this.SourceVersion = version;
			this.Precision = precision;
			this._scale = scale;
			this._size = size;
			this._forceSize = forceSize;
			this._offset = offset;
			this.Direction = direction;
			this._metaType = MetaType.GetMetaType(type);
			if (value is ICloneable)
			{
				value = ((ICloneable)value).Clone();
			}
			this._value = value;
			this.Suppress = suppress;
			this._inferType = inferType;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000AD54 File Offset: 0x00009D54
		object ICloneable.Clone()
		{
			SqlParameter sqlParameter = new SqlParameter();
			sqlParameter.SetProperties(this._name, this._sourceColumn, this._version, this._precision, this._scale, this._size, this._forceSize, this._offset, this._direction, this._value, this.SqlDbType, this._suppress, this._inferType);
			return sqlParameter;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000ADBC File Offset: 0x00009DBC
		internal void Validate()
		{
			object value = this.Value;
			if ((this.Direction & (ParameterDirection)2) != null && !this._metaType.IsFixed && !this._forceSize && (value == null || Convert.IsDBNull(value)) && this.SqlDbType != (SqlDbType)19)
			{
				throw new InvalidOperationException(Res.GetString("ADP_UninitializedParameterSize", new object[]
				{
					this._parent.IndexOf(this).ToString(),
					this.ParameterName,
					this._metaType.ClassType.Name,
					this.Size.ToString()
				}));
			}
		}

		// Token: 0x040000FE RID: 254
		private SqlParameterCollection _parent;

		// Token: 0x040000FF RID: 255
		private object _value;

		// Token: 0x04000100 RID: 256
		internal MetaType _metaType = MetaType.GetDefaultMetaType();

		// Token: 0x04000101 RID: 257
		private string _sourceColumn;

		// Token: 0x04000102 RID: 258
		private string _name;

		// Token: 0x04000103 RID: 259
		private byte _precision;

		// Token: 0x04000104 RID: 260
		private byte _scale;

		// Token: 0x04000105 RID: 261
		private ParameterDirection _direction = (ParameterDirection)1;

		// Token: 0x04000106 RID: 262
		private int _size = -1;

		// Token: 0x04000107 RID: 263
		private DataRowVersion _version = (DataRowVersion)512;

		// Token: 0x04000108 RID: 264
		private bool _isNullable;

		// Token: 0x04000109 RID: 265
		private SqlCollation _collation;

		// Token: 0x0400010A RID: 266
		private bool _forceSize;

		// Token: 0x0400010B RID: 267
		private int _offset;

		// Token: 0x0400010C RID: 268
		private bool _suppress;

		// Token: 0x0400010D RID: 269
		private bool _inferType = true;
	}
}
