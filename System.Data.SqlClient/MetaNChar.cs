using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200001B RID: 27
	internal class MetaNChar : MetaType
	{
		// Token: 0x0600015E RID: 350 RVA: 0x000099DC File Offset: 0x000089DC
        public MetaNChar()
            : base(byte.MaxValue, byte.MaxValue, -1, false, false, 239, "nchar", typeof(string), (SqlDbType)10, (DbType)23)
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00009A14 File Offset: 0x00008A14
		public override byte PropBytes
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00009A17 File Offset: 0x00008A17
		public override Type SqlType
		{
			get
			{
				return typeof(SqlString);
			}
		}
	}
}
