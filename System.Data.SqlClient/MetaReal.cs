using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200000E RID: 14
	internal sealed class MetaReal : MetaType
	{
		// Token: 0x06000141 RID: 321 RVA: 0x000096E8 File Offset: 0x000086E8
        public MetaReal()
            : base(7, byte.MaxValue, 4, true, false, 59, "real", typeof(float), (SqlDbType)13, (DbType)15)
		{
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00009719 File Offset: 0x00008719
		public override byte NullableType
		{
			get
			{
				return 109;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000971D File Offset: 0x0000871D
		public override Type SqlType
		{
			get
			{
				return typeof(SqlSingle);
			}
		}
	}
}
