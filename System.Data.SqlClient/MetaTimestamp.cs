using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000010 RID: 16
	internal sealed class MetaTimestamp : MetaBinary
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00009771 File Offset: 0x00008771
		public MetaTimestamp()
		{
            base.SqlDbType = (SqlDbType)19;
			base.TypeName = "timestamp";
		}
	}
}
