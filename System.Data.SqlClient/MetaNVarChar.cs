using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200001C RID: 28
	internal sealed class MetaNVarChar : MetaNChar
	{
		// Token: 0x06000161 RID: 353 RVA: 0x00009A23 File Offset: 0x00008A23
		public MetaNVarChar()
		{
			base.TDSType = 231;
			base.TypeName = "nvarchar";
            base.SqlDbType = (SqlDbType)12;
            base.DbType = (DbType)16;
		}
	}
}
