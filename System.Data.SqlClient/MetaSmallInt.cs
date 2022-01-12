using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000016 RID: 22
	internal sealed class MetaSmallInt : MetaType
	{
		// Token: 0x06000152 RID: 338 RVA: 0x000098A0 File Offset: 0x000088A0
        public MetaSmallInt()
            : base(5, byte.MaxValue, 2, true, false, 52, "smallint", typeof(short), (SqlDbType)16, (DbType)10)
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000098D1 File Offset: 0x000088D1
		public override byte NullableType
		{
			get
			{
				return 38;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000098D5 File Offset: 0x000088D5
		public override Type SqlType
		{
			get
			{
				return typeof(SqlInt16);
			}
		}
	}
}
