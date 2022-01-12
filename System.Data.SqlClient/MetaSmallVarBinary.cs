using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000012 RID: 18
	internal sealed class MetaSmallVarBinary : MetaBinary
	{
		// Token: 0x06000149 RID: 329 RVA: 0x000097B9 File Offset: 0x000087B9
		public MetaSmallVarBinary()
		{
			base.TDSType = 37;
			base.TypeName = string.Empty;
            base.SqlDbType = (SqlDbType)24;
		}
	}
}
