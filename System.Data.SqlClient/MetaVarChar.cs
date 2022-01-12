using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000019 RID: 25
	internal sealed class MetaVarChar : MetaChar
	{
		// Token: 0x0600015B RID: 347 RVA: 0x0000996E File Offset: 0x0000896E
		public MetaVarChar()
		{
			base.TDSType = 167;
			base.TypeName = "varchar";
            base.SqlDbType = (SqlDbType)22;
			base.DbType = 0;
		}
	}
}
