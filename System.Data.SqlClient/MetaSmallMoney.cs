using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000023 RID: 35
	internal sealed class MetaSmallMoney : MetaType
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00009C0C File Offset: 0x00008C0C
        public MetaSmallMoney()
            : base(10, byte.MaxValue, 4, true, false, 122, "smallmoney", typeof(decimal), (SqlDbType)17, (DbType)4)
		{
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00009C3D File Offset: 0x00008C3D
		public override byte NullableType
		{
			get
			{
				return 110;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00009C41 File Offset: 0x00008C41
		public override Type SqlType
		{
			get
			{
				return typeof(SqlMoney);
			}
		}
	}
}
