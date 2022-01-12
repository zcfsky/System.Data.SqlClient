using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200000B RID: 11
	internal class MetaType
	{
		// Token: 0x06000113 RID: 275 RVA: 0x000084D4 File Offset: 0x000074D4
		public MetaType(byte precision, byte scale, int fixedLength, bool isFixed, bool isLong, byte tdsType, string typeName, Type classType, SqlDbType sqlType, DbType dbType)
		{
			this.Precision = precision;
			this.Scale = scale;
			this.FixedLength = fixedLength;
			this.IsFixed = isFixed;
			this.IsLong = isLong;
			this.TDSType = tdsType;
			this.TypeName = typeName;
			this.ClassType = classType;
			this.SqlDbType = sqlType;
			this.DbType = dbType;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00008534 File Offset: 0x00007534
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000853C File Offset: 0x0000753C
		public SqlDbType SqlDbType
		{
			get
			{
				return this.sqlType;
			}
			set
			{
				this.sqlType = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00008545 File Offset: 0x00007545
		// (set) Token: 0x06000117 RID: 279 RVA: 0x0000854D File Offset: 0x0000754D
		public Type ClassType
		{
			get
			{
				return this.classType;
			}
			set
			{
				this.classType = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00008556 File Offset: 0x00007556
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000855E File Offset: 0x0000755E
		public DbType DbType
		{
			get
			{
				return this.dbType;
			}
			set
			{
				this.dbType = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00008567 File Offset: 0x00007567
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000856F File Offset: 0x0000756F
		public int FixedLength
		{
			get
			{
				return this.fixedLength;
			}
			set
			{
				this.fixedLength = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00008578 File Offset: 0x00007578
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00008580 File Offset: 0x00007580
		public bool IsFixed
		{
			get
			{
				return this.isFixed;
			}
			set
			{
				this.isFixed = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00008589 File Offset: 0x00007589
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00008591 File Offset: 0x00007591
		public bool IsLong
		{
			get
			{
				return this.isLong;
			}
			set
			{
				this.isLong = value;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000859A File Offset: 0x0000759A
		public static bool IsAnsiType(SqlDbType type)
		{
			return type == (SqlDbType)3 || type == (SqlDbType)22 || type == (SqlDbType)18;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000085AC File Offset: 0x000075AC
		public static bool IsSizeInCharacters(SqlDbType type)
		{
            return type == (SqlDbType)10 || type == (SqlDbType)12 || type == (SqlDbType)11;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000085BF File Offset: 0x000075BF
		public static bool IsCharType(SqlDbType type)
		{
            return type == (SqlDbType)10 || type == (SqlDbType)12 || type == (SqlDbType)11 || type == (SqlDbType)3 || type == (SqlDbType)22 || type == (SqlDbType)18;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000085E0 File Offset: 0x000075E0
		public static bool IsBinType(SqlDbType type)
		{
			return type == (SqlDbType)7 || type == (SqlDbType)1 || type == (SqlDbType)21 || type == (SqlDbType)19 || type == (SqlDbType)24;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000085FB File Offset: 0x000075FB
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00008603 File Offset: 0x00007603
		public byte Precision
		{
			get
			{
				return this.precision;
			}
			set
			{
				this.precision = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000860C File Offset: 0x0000760C
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00008614 File Offset: 0x00007614
		public byte Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000861D File Offset: 0x0000761D
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00008625 File Offset: 0x00007625
		public byte TDSType
		{
			get
			{
				return this.tdsType;
			}
			set
			{
				this.tdsType = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600012A RID: 298 RVA: 0x0000862E File Offset: 0x0000762E
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00008636 File Offset: 0x00007636
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
			set
			{
				this.typeName = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600012C RID: 300 RVA: 0x0000863F File Offset: 0x0000763F
		public virtual byte NullableType
		{
			get
			{
				return this.tdsType;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00008647 File Offset: 0x00007647
		public virtual byte PropBytes
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000864A File Offset: 0x0000764A
		public virtual Type SqlType
		{
			get
			{
				return this.ClassType;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008652 File Offset: 0x00007652
		internal static MetaType GetMetaType(SqlDbType target)
		{
			return MetaType.metaTypeMap[(int)target];
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000865C File Offset: 0x0000765C
		internal static MetaType GetMetaType(DbType target)
		{
			MetaType metaType = null;
			switch (target)
			{
			case (DbType)0:
				metaType = MetaType.metaTypeMap[22];
				break;
            case (DbType)1:
				metaType = MetaType.metaTypeMap[21];
				break;
            case (DbType)2:
				metaType = MetaType.metaTypeMap[20];
				break;
            case (DbType)3:
				metaType = MetaType.metaTypeMap[2];
				break;
			case (DbType)4:
				metaType = MetaType.metaTypeMap[9];
				break;
            case (DbType)5:
            case (DbType)6:
				metaType = MetaType.metaTypeMap[4];
				break;
            case (DbType)7:
				metaType = MetaType.metaTypeMap[5];
				break;
            case (DbType)8:
				metaType = MetaType.metaTypeMap[6];
				break;
            case (DbType)9:
				metaType = MetaType.metaTypeMap[14];
				break;
            case (DbType)10:
				metaType = MetaType.metaTypeMap[16];
				break;
            case (DbType)11:
				metaType = MetaType.metaTypeMap[8];
				break;
            case (DbType)12:
				metaType = MetaType.metaTypeMap[0];
				break;
            case (DbType)13:
				metaType = MetaType.metaTypeMap[23];
				break;
            case (DbType)15:
				metaType = MetaType.metaTypeMap[13];
				break;
            case (DbType)16:
				metaType = MetaType.metaTypeMap[12];
				break;
            case (DbType)17:
				metaType = MetaType.metaTypeMap[4];
				break;
            case (DbType)22:
				metaType = MetaType.metaTypeMap[3];
				break;
            case (DbType)23:
				metaType = MetaType.metaTypeMap[10];
				break;
			}
			if (metaType == null)
			{
				throw new ArgumentException(Res.GetString("ADP_UnknownDataType", new object[]
				{
					target.ToString(),
					typeof(SqlDbType).Name
				}));
			}
			return metaType;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000087E0 File Offset: 0x000077E0
		internal static MetaType GetMetaType(object value)
		{
			bool flag = false;
			Type type;
			if (value is Type)
			{
				type = (Type)value;
			}
			else
			{
				type = value.GetType();
				flag = true;
			}
			if (type == typeof(byte[]))
			{
				return MetaType.metaTypeMap[21];
			}
			if (type == typeof(Guid))
			{
				return MetaType.metaTypeMap[14];
			}
			if (type == typeof(object))
			{
				return MetaType.metaTypeMap[23];
			}
			if (type == typeof(SqlBinary))
			{
				return MetaType.metaTypeMap[21];
			}
			if (type == typeof(SqlBoolean))
			{
				return MetaType.metaTypeMap[2];
			}
			if (type == typeof(SqlBoolean))
			{
				return MetaType.metaTypeMap[2];
			}
			if (type == typeof(SqlByte))
			{
				return MetaType.metaTypeMap[20];
			}
			if (type == typeof(SqlDateTime))
			{
				return MetaType.metaTypeMap[4];
			}
			if (type == typeof(SqlGuid))
			{
				return MetaType.metaTypeMap[14];
			}
			if (type == typeof(SqlInt16))
			{
				return MetaType.metaTypeMap[16];
			}
			if (type == typeof(SqlInt32))
			{
				return MetaType.metaTypeMap[8];
			}
			if (type == typeof(SqlInt64))
			{
				return MetaType.metaTypeMap[0];
			}
			if (type == typeof(SqlMoney))
			{
				return MetaType.metaTypeMap[9];
			}
			if (type == typeof(SqlDecimal))
			{
				return MetaType.metaTypeMap[5];
			}
			if (type == typeof(SqlString))
			{
				if (!flag)
				{
					return MetaType.metaTypeMap[12];
				}
				return MetaType.PromoteStringType(((SqlString)value).Value);
			}
			else
			{
				if (type == typeof(SqlSingle))
				{
					return MetaType.metaTypeMap[13];
				}
				if (type == typeof(SqlDouble))
				{
					return MetaType.metaTypeMap[6];
				}
				if (type == typeof(DBNull))
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataType", new object[]
					{
						2.ToString()
					}));
				}
				if (type == typeof(bool))
				{
					return MetaType.metaTypeMap[2];
				}
				if (type == typeof(char))
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataType", new object[]
					{
						4.ToString()
					}));
				}
				if (type == typeof(sbyte))
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataType", new object[]
					{
						5.ToString()
					}));
				}
				if (type == typeof(byte))
				{
					return MetaType.metaTypeMap[20];
				}
				if (type == typeof(short))
				{
					return MetaType.metaTypeMap[16];
				}
				if (type == typeof(ushort))
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataType", new object[]
					{
						8.ToString()
					}));
				}
				if (type == typeof(int))
				{
					return MetaType.metaTypeMap[8];
				}
				if (type == typeof(uint))
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataType", new object[]
					{
						10.ToString()
					}));
				}
				if (type == typeof(long))
				{
					return MetaType.metaTypeMap[0];
				}
				if (type == typeof(ulong))
				{
					throw new ArgumentException(Res.GetString("ADP_InvalidDataType", new object[]
					{
						12.ToString()
					}));
				}
				if (type == typeof(float))
				{
					return MetaType.metaTypeMap[13];
				}
				if (type == typeof(double))
				{
					return MetaType.metaTypeMap[6];
				}
				if (type == typeof(decimal))
				{
					return MetaType.metaTypeMap[5];
				}
				if (type == typeof(DateTime))
				{
					return MetaType.metaTypeMap[4];
				}
				if (type != typeof(string))
				{
					throw new ArgumentException(Res.GetString("ADP_UnknownDataType", new object[]
					{
						type.FullName
					}));
				}
				if (!flag)
				{
					return MetaType.metaTypeMap[12];
				}
				return MetaType.PromoteStringType((string)value);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00008BDC File Offset: 0x00007BDC
		internal static MetaType PromoteStringType(string s)
		{
			int length = s.Length;
			if (length << 1 > 8000)
			{
				return MetaType.metaTypeMap[22];
			}
			return MetaType.metaTypeMap[12];
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008C0C File Offset: 0x00007C0C
		internal static object GetComValue(SqlDbType type, object sqlVal)
		{
			object result = DBNull.Value;
			if (Convert.IsDBNull(sqlVal) || ((INullable)sqlVal).IsNull)
			{
				return result;
			}
			switch (type)
			{
                case (SqlDbType)0:
				result = ((SqlInt64)sqlVal).Value;
				break;
            case (SqlDbType)1:
            case (SqlDbType)7:
            case (SqlDbType)19:
            case (SqlDbType)21:
            case (SqlDbType)24:
				result = ((SqlBinary)sqlVal).Value;
				break;
            case (SqlDbType)2:
				result = ((SqlBoolean)sqlVal).Value;
				break;
            case (SqlDbType)3:
            case (SqlDbType)10:
            case (SqlDbType)11:
            case (SqlDbType)12:
            case (SqlDbType)18:
            case (SqlDbType)22:
				result = ((SqlString)sqlVal).Value;
				break;
            case (SqlDbType)4:
            case (SqlDbType)15:
				result = ((SqlDateTime)sqlVal).Value;
				break;
            case (SqlDbType)5:
				result = ((SqlDecimal)sqlVal).Value;
				break;
            case (SqlDbType)6:
				result = ((SqlDouble)sqlVal).Value;
				break;
            case (SqlDbType)8:
				result = ((SqlInt32)sqlVal).Value;
				break;
            case (SqlDbType)9:
            case (SqlDbType)17:
				result = ((SqlMoney)sqlVal).Value;
				break;
            case (SqlDbType)13:
				result = ((SqlSingle)sqlVal).Value;
				break;
            case (SqlDbType)14:
				result = ((SqlGuid)sqlVal).Value;
				break;
            case (SqlDbType)16:
				result = ((SqlInt16)sqlVal).Value;
				break;
            case (SqlDbType)20:
				result = ((SqlByte)sqlVal).Value;
				break;
            case (SqlDbType)23:
				result = MetaType.GetComValueFromSqlVariant(sqlVal);
				break;
			}
			return result;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008DE4 File Offset: 0x00007DE4
		internal static object GetSqlValue(SqlDbType type, object comVal)
		{
			object result = null;
			bool flag = comVal == null || Convert.IsDBNull(comVal);
			switch (type)
			{
                case (SqlDbType)0:
				result = (flag ? SqlInt64.Null : new SqlInt64((long)comVal));
				break;
            case (SqlDbType)1:
            case (SqlDbType)7:
            case (SqlDbType)19:
            case (SqlDbType)21:
            case (SqlDbType)24:
				result = (flag ? SqlBinary.Null : new SqlBinary((byte[])comVal));
				break;
            case (SqlDbType)2:
				result = (flag ? SqlBoolean.Null : new SqlBoolean((bool)comVal));
				break;
            case (SqlDbType)3:
            case (SqlDbType)10:
            case (SqlDbType)11:
            case (SqlDbType)12:
            case (SqlDbType)18:
            case (SqlDbType)22:
				result = (flag ? SqlString.Null : new SqlString((string)comVal));
				break;
            case (SqlDbType)4:
            case (SqlDbType)15:
				result = (flag ? SqlDateTime.Null : new SqlDateTime((DateTime)comVal));
				break;
            case (SqlDbType)5:
				result = (flag ? SqlDecimal.Null : new SqlDecimal((decimal)comVal));
				break;
            case (SqlDbType)6:
				result = (flag ? SqlDouble.Null : new SqlDouble((double)comVal));
				break;
            case (SqlDbType)8:
				result = (flag ? SqlInt32.Null : new SqlInt32((int)comVal));
				break;
            case (SqlDbType)9:
            case (SqlDbType)17:
				result = (flag ? SqlMoney.Null : new SqlMoney((decimal)comVal));
				break;
            case (SqlDbType)13:
				result = (flag ? SqlSingle.Null : new SqlSingle((float)comVal));
				break;
            case (SqlDbType)14:
				result = (flag ? SqlGuid.Null : new SqlGuid((Guid)comVal));
				break;
            case (SqlDbType)16:
				result = (flag ? SqlInt16.Null : new SqlInt16((short)comVal));
				break;
            case (SqlDbType)20:
				result = (flag ? SqlByte.Null : new SqlByte((byte)comVal));
				break;
            case (SqlDbType)23:
				result = (flag ? DBNull.Value : MetaType.GetSqlValueFromComVariant(comVal));
				break;
			}
			return result;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00009018 File Offset: 0x00008018
		internal static object GetComValueFromSqlVariant(object sqlVal)
		{
			object result = null;
			if (sqlVal is SqlSingle)
			{
				result = ((SqlSingle)sqlVal).Value;
			}
			else if (sqlVal is SqlString)
			{
				result = ((SqlString)sqlVal).Value;
			}
			else if (sqlVal is SqlDouble)
			{
				result = ((SqlDouble)sqlVal).Value;
			}
			else if (sqlVal is SqlBinary)
			{
				result = ((SqlBinary)sqlVal).Value;
			}
			else if (sqlVal is SqlGuid)
			{
				result = ((SqlGuid)sqlVal).Value;
			}
			else if (sqlVal is SqlBoolean)
			{
				result = ((SqlBoolean)sqlVal).Value;
			}
			else if (sqlVal is SqlByte)
			{
				result = ((SqlByte)sqlVal).Value;
			}
			else if (sqlVal is SqlInt16)
			{
				result = ((SqlInt16)sqlVal).Value;
			}
			else if (sqlVal is SqlInt32)
			{
				result = ((SqlInt32)sqlVal).Value;
			}
			else if (sqlVal is SqlInt64)
			{
				result = ((SqlInt64)sqlVal).Value;
			}
			else if (sqlVal is SqlDecimal)
			{
				result = ((SqlDecimal)sqlVal).Value;
			}
			else if (sqlVal is SqlDateTime)
			{
				result = ((SqlDateTime)sqlVal).Value;
			}
			else if (sqlVal is SqlMoney)
			{
				result = ((SqlMoney)sqlVal).Value;
			}
			return result;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000091C4 File Offset: 0x000081C4
		internal static object GetSqlValueFromComVariant(object comVal)
		{
			object result = null;
			if (comVal is float)
			{
				result = new SqlSingle((float)comVal);
			}
			else if (comVal is string)
			{
				result = new SqlString((string)comVal);
			}
			else if (comVal is double)
			{
				result = new SqlDouble((double)comVal);
			}
			else if (comVal is byte[])
			{
				result = new SqlBinary((byte[])comVal);
			}
			else if (comVal is Guid)
			{
				result = new SqlGuid((Guid)comVal);
			}
			else if (comVal is bool)
			{
				result = new SqlBoolean((bool)comVal);
			}
			else if (comVal is byte)
			{
				result = new SqlByte((byte)comVal);
			}
			else if (comVal is short)
			{
				result = new SqlInt16((short)comVal);
			}
			else if (comVal is int)
			{
				result = new SqlInt32((int)comVal);
			}
			else if (comVal is long)
			{
				result = new SqlInt64((long)comVal);
			}
			else if (comVal is decimal)
			{
				result = new SqlDecimal((decimal)comVal);
			}
			else if (comVal is DateTime)
			{
				result = new SqlDateTime((DateTime)comVal);
			}
			return result;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000932C File Offset: 0x0000832C
		internal static SqlDbType GetSqlDbTypeFromOleDbType(short dbType, string typeName)
		{
            return (SqlDbType)23;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009340 File Offset: 0x00008340
		internal static SqlDbType GetSqlDataType(int tdsType, int userType, int length)
		{
            SqlDbType result = (SqlDbType)23;
			int num = tdsType;
			if (num != 38)
			{
				switch (num)
				{
				case 109:
					tdsType = ((length == 4) ? 59 : 62);
					break;
				case 110:
					tdsType = ((length == 4) ? 122 : 60);
					break;
				case 111:
					tdsType = ((length == 4) ? 58 : 61);
					break;
				}
			}
			else if (length == 1)
			{
				tdsType = 48;
			}
			else if (length == 2)
			{
				tdsType = 52;
			}
			else if (length == 4)
			{
				tdsType = 56;
			}
			else
			{
				tdsType = 127;
			}
			int num2 = tdsType;
			if (num2 <= 122)
			{
				if (num2 <= 99)
				{
					switch (num2)
					{
					case 31:
					case 32:
					case 33:
					case 38:
					case 40:
					case 41:
					case 42:
					case 43:
					case 44:
					case 46:
					case 49:
					case 51:
					case 53:
					case 54:
					case 55:
					case 57:
						return result;
					case 34:
                        return (SqlDbType)7;
					case 35:
                        return (SqlDbType)18;
					case 36:
                        return (SqlDbType)14;
					case 37:
                        return (SqlDbType)24;
					case 39:
						goto IL_1AE;
					case 45:
						goto IL_1B3;
					case 47:
						goto IL_1C5;
					case 48:
                        return (SqlDbType)20;
					case 50:
						break;
					case 52:
                        return (SqlDbType)16;
					case 56:
                        return (SqlDbType)8;
					case 58:
                        return (SqlDbType)15;
					case 59:
                        return (SqlDbType)13;
					case 60:
                        return (SqlDbType)9;
					case 61:
                        return (SqlDbType)4;
					case 62:
                        return (SqlDbType)6;
					default:
						switch (num2)
						{
						case 98:
                                return (SqlDbType)23;
						case 99:
                                return (SqlDbType)11;
						default:
							return result;
						}
						break;
					}
				}
				else
				{
					switch (num2)
					{
					case 104:
						break;
					case 105:
					case 107:
						return result;
					case 106:
					case 108:
                        return (SqlDbType)5;
					default:
						if (num2 != 122)
						{
							return result;
						}
						return (SqlDbType)17;
					}
				}
                return (SqlDbType)2;
			}
			if (num2 <= 167)
			{
				if (num2 == 127)
				{
					return 0;
				}
				switch (num2)
				{
				case 165:
                        return (SqlDbType)21;
				case 166:
					return result;
				case 167:
					break;
				default:
					return result;
				}
			}
			else
			{
				switch (num2)
				{
				case 173:
					goto IL_1B3;
				case 174:
					return result;
				case 175:
					goto IL_1C5;
				default:
					if (num2 == 231)
					{
                        return (SqlDbType)12;
					}
					if (num2 != 239)
					{
						return result;
					}
                    return (SqlDbType)10;
				}
			}
			IL_1AE:
            return (SqlDbType)22;
			IL_1B3:
			if (userType == 80)
			{
                return (SqlDbType)19;
			}
            return (SqlDbType)1;
			IL_1C5:
            result = (SqlDbType)3;
			return result;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009564 File Offset: 0x00008564
		internal static MetaType GetDefaultMetaType()
		{
			return MetaType.metaTypeMap[12];
		}

		// Token: 0x040000D0 RID: 208
		private Type classType;

		// Token: 0x040000D1 RID: 209
		private int fixedLength;

		// Token: 0x040000D2 RID: 210
		private bool isFixed;

		// Token: 0x040000D3 RID: 211
		private bool isLong;

		// Token: 0x040000D4 RID: 212
		private byte precision;

		// Token: 0x040000D5 RID: 213
		private byte scale;

		// Token: 0x040000D6 RID: 214
		private byte tdsType;

		// Token: 0x040000D7 RID: 215
		private string typeName;

		// Token: 0x040000D8 RID: 216
		private SqlDbType sqlType;

		// Token: 0x040000D9 RID: 217
		private DbType dbType;

		// Token: 0x040000DA RID: 218
		internal static readonly MetaType[] metaTypeMap = new MetaType[]
		{
			new MetaBigInt(),
			new MetaBinary(),
			new MetaBit(),
			new MetaChar(),
			new MetaDateTime(),
			new MetaDecimal(),
			new MetaFloat(),
			new MetaImage(),
			new MetaInt(),
			new MetaMoney(),
			new MetaNChar(),
			new MetaNText(),
			new MetaNVarChar(),
			new MetaReal(),
			new MetaUniqueId(),
			new MetaSmallDateTime(),
			new MetaSmallInt(),
			new MetaSmallMoney(),
			new MetaText(),
			new MetaTimestamp(),
			new MetaTinyInt(),
			new MetaVarBinary(),
			new MetaVarChar(),
			new MetaVariant(),
			new MetaSmallVarBinary()
		};
	}
}
