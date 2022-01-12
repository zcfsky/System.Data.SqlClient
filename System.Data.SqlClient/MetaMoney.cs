using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000022 RID: 34
	internal sealed class MetaMoney : MetaType
	{
		// Token: 0x0600016E RID: 366 RVA: 0x00009BC8 File Offset: 0x00008BC8
        public MetaMoney()
            : base(19, byte.MaxValue, 8, true, false, 60, "money", typeof(decimal), (SqlDbType)9, (DbType)4)
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00009BF9 File Offset: 0x00008BF9
		public override byte NullableType
		{
			get
			{
				return 110;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00009BFD File Offset: 0x00008BFD
		public override Type SqlType
		{
			get
			{
				return typeof(SqlMoney);
			}
		}
	}
}
