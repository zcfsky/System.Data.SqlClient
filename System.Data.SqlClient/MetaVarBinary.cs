using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000011 RID: 17
	internal sealed class MetaVarBinary : MetaBinary
	{
		// Token: 0x06000148 RID: 328 RVA: 0x0000978C File Offset: 0x0000878C
		public MetaVarBinary()
		{
			base.TDSType = 165;
			base.TypeName = "varbinary";
            base.SqlDbType = (SqlDbType)21;
            base.DbType = (DbType)1;
		}
	}
}
