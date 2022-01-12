using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000024 RID: 36
	internal sealed class MetaUniqueId : MetaType
	{
		// Token: 0x06000174 RID: 372 RVA: 0x00009C50 File Offset: 0x00008C50
        public MetaUniqueId()
            : base(byte.MaxValue, byte.MaxValue, 16, true, false, 36, "uniqueidentifier", typeof(Guid), (SqlDbType)14, (DbType)9)
		{
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00009C86 File Offset: 0x00008C86
		public override Type SqlType
		{
			get
			{
				return typeof(SqlGuid);
			}
		}
	}
}
