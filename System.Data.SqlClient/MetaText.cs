using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200001A RID: 26
	internal class MetaText : MetaType
	{
		// Token: 0x0600015C RID: 348 RVA: 0x0000999C File Offset: 0x0000899C
        public MetaText()
            : base(byte.MaxValue, byte.MaxValue, -1, false, true, 35, "text", typeof(string), (SqlDbType)18, (DbType)0)
		{
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000099D0 File Offset: 0x000089D0
		public override Type SqlType
		{
			get
			{
				return typeof(SqlString);
			}
		}
	}
}
