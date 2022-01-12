using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200001D RID: 29
	internal sealed class MetaNText : MetaNChar
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00009A51 File Offset: 0x00008A51
		public MetaNText()
		{
			base.IsLong = true;
			base.TDSType = 99;
			base.TypeName = "ntext";
			base.SqlDbType = (SqlDbType)11;
			base.DbType = (DbType)16;
		}
	}
}
