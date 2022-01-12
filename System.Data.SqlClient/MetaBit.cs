using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000014 RID: 20
	internal sealed class MetaBit : MetaType
	{
		// Token: 0x0600014C RID: 332 RVA: 0x0000981C File Offset: 0x0000881C
		public MetaBit() : base(byte.MaxValue, byte.MaxValue, 1, true, false, 50, "bit", typeof(bool), (SqlDbType)2, (DbType)3)
		{
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000984F File Offset: 0x0000884F
		public override byte NullableType
		{
			get
			{
				return 104;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00009853 File Offset: 0x00008853
		public override Type SqlType
		{
			get
			{
				return typeof(SqlBoolean);
			}
		}
	}
}
