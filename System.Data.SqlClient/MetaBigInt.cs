using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200000C RID: 12
	internal sealed class MetaBigInt : MetaType
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00009664 File Offset: 0x00008664
		public MetaBigInt() : base(19, byte.MaxValue, 8, true, false, 127, "bigint", typeof(long), (SqlDbType)0, (DbType)12)
		{
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00009695 File Offset: 0x00008695
		public override byte NullableType
		{
			get
			{
				return 38;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00009699 File Offset: 0x00008699
		public override Type SqlType
		{
			get
			{
				return typeof(SqlInt64);
			}
		}
	}
}
