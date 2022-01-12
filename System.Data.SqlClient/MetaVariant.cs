using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000025 RID: 37
	internal sealed class MetaVariant : MetaType
	{
		// Token: 0x06000176 RID: 374 RVA: 0x00009C94 File Offset: 0x00008C94
		public MetaVariant() : base(byte.MaxValue, byte.MaxValue, -1, true, false, 98, "sql_variant", typeof(object), (SqlDbType)23, (DbType)13)
		{
		}
	}
}
