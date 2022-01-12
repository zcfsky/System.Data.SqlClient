using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000015 RID: 21
	internal sealed class MetaTinyInt : MetaType
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00009860 File Offset: 0x00008860
		public MetaTinyInt() : base(3, byte.MaxValue, 1, true, false, 48, "tinyint", typeof(byte), (SqlDbType)20, (DbType)2)
		{
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00009890 File Offset: 0x00008890
		public override byte NullableType
		{
			get
			{
				return 38;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00009894 File Offset: 0x00008894
		public override Type SqlType
		{
			get
			{
				return typeof(SqlByte);
			}
		}
	}
}
