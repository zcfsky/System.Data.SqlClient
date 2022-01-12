using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000018 RID: 24
	internal class MetaChar : MetaType
	{
		// Token: 0x06000158 RID: 344 RVA: 0x00009928 File Offset: 0x00008928
		public MetaChar() : base(byte.MaxValue, byte.MaxValue, -1, false, false, 175, "char", typeof(string), (SqlDbType)3, (DbType)22)
		{
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000995F File Offset: 0x0000895F
		public override byte PropBytes
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00009962 File Offset: 0x00008962
		public override Type SqlType
		{
			get
			{
				return typeof(SqlString);
			}
		}
	}
}
