using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000013 RID: 19
	internal sealed class MetaImage : MetaType
	{
		// Token: 0x0600014A RID: 330 RVA: 0x000097DC File Offset: 0x000087DC
        public MetaImage()
            : base(byte.MaxValue, byte.MaxValue, -1, false, true, 34, "image", typeof(byte[]), (SqlDbType)7, (DbType)1)
		{
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000980F File Offset: 0x0000880F
		public override Type SqlType
		{
			get
			{
				return typeof(SqlBinary);
			}
		}
	}
}
