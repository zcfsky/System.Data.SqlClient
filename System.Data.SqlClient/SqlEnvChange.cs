using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000044 RID: 68
	internal sealed class SqlEnvChange
	{
		// Token: 0x04000258 RID: 600
		public byte type;

		// Token: 0x04000259 RID: 601
		public int length;

		// Token: 0x0400025A RID: 602
		public byte newLength;

		// Token: 0x0400025B RID: 603
		public byte oldLength;

		// Token: 0x0400025C RID: 604
		public string newValue;

		// Token: 0x0400025D RID: 605
		public string oldValue;

		// Token: 0x0400025E RID: 606
		public SqlCollation newCollation;

		// Token: 0x0400025F RID: 607
		public SqlCollation oldCollation;
	}
}
