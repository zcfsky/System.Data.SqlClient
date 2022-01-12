using System;
using System.Text;

namespace System.Data.SqlClient
{
	// Token: 0x02000046 RID: 70
	internal sealed class SqlReturnValue
	{
		// Token: 0x04000277 RID: 631
		public SqlDbType type;

		// Token: 0x04000278 RID: 632
		public byte tdsType;

		// Token: 0x04000279 RID: 633
		public bool isNullable;

		// Token: 0x0400027A RID: 634
		public SqlCollation collation;

		// Token: 0x0400027B RID: 635
		public int codePage;

		// Token: 0x0400027C RID: 636
		public Encoding encoding;

		// Token: 0x0400027D RID: 637
		public int length;

		// Token: 0x0400027E RID: 638
		public string parameter;

		// Token: 0x0400027F RID: 639
		public byte precision;

		// Token: 0x04000280 RID: 640
		public byte scale;

		// Token: 0x04000281 RID: 641
		public object value;
	}
}
