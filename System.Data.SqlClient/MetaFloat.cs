using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200000D RID: 13
	internal sealed class MetaFloat : MetaType
	{
		// Token: 0x0600013E RID: 318 RVA: 0x000096A8 File Offset: 0x000086A8
        public MetaFloat()
            : base(15, byte.MaxValue, 8, true, false, 62, "float", typeof(double), (SqlDbType)6, (DbType)8)
		{
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000096D8 File Offset: 0x000086D8
		public override byte NullableType
		{
			get
			{
				return 109;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000096DC File Offset: 0x000086DC
		public override Type SqlType
		{
			get
			{
				return typeof(SqlDouble);
			}
		}
	}
}
