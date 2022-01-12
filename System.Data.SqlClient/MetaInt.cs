using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000017 RID: 23
	internal sealed class MetaInt : MetaType
	{
		// Token: 0x06000155 RID: 341 RVA: 0x000098E4 File Offset: 0x000088E4
        public MetaInt()
            : base(10, byte.MaxValue, 4, true, false, 56, "int", typeof(int), (SqlDbType)8, (DbType)11)
		{
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00009915 File Offset: 0x00008915
		public override byte NullableType
		{
			get
			{
				return 38;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00009919 File Offset: 0x00008919
		public override Type SqlType
		{
			get
			{
				return typeof(SqlInt32);
			}
		}
	}
}
