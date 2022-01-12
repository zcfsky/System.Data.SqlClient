using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000020 RID: 32
	internal class MetaDateTime : MetaType
	{
		// Token: 0x06000167 RID: 359 RVA: 0x00009AC4 File Offset: 0x00008AC4
		public MetaDateTime() : base(23, 3, 8, true, false, 61, "datetime", typeof(DateTime), (SqlDbType)4, (DbType)6)
		{
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00009AF0 File Offset: 0x00008AF0
		public override byte NullableType
		{
			get
			{
				return 111;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00009AF4 File Offset: 0x00008AF4
		public override Type SqlType
		{
			get
			{
				return typeof(SqlDateTime);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009B00 File Offset: 0x00008B00
		public static TdsDateTime FromDateTime(DateTime comDateTime, byte cb)
		{
			SqlDateTime sqlDateTime = new SqlDateTime(comDateTime);
			TdsDateTime result = default(TdsDateTime);
			result.days = sqlDateTime.DayTicks;
			if (cb == 8)
			{
				result.time = sqlDateTime.TimeTicks;
			}
			else
			{
				result.time = sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute;
			}
			return result;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009B54 File Offset: 0x00008B54
		public static DateTime ToDateTime(int sqlDays, int sqlTime, int length)
		{
			if (length == 4)
			{
				return new SqlDateTime(sqlDays, sqlTime * SqlDateTime.SQLTicksPerMinute).Value;
			}
			return new SqlDateTime(sqlDays, sqlTime).Value;
		}
	}
}
