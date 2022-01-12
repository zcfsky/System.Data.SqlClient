using System;
using System.Text;

namespace System.Data.SqlClient
{
	// Token: 0x02000045 RID: 69
	internal class SqlMetaData
	{
		// Token: 0x04000260 RID: 608
		public SqlDbType type;

		// Token: 0x04000261 RID: 609
		public byte tdsType;

		// Token: 0x04000262 RID: 610
		public int length;

		// Token: 0x04000263 RID: 611
		public byte precision = byte.MaxValue;

		// Token: 0x04000264 RID: 612
		public byte scale = byte.MaxValue;

		// Token: 0x04000265 RID: 613
		public string column;

		// Token: 0x04000266 RID: 614
		public SqlCollation collation;

		// Token: 0x04000267 RID: 615
		public int codePage;

		// Token: 0x04000268 RID: 616
		public Encoding encoding;

		// Token: 0x04000269 RID: 617
		public string baseColumn;

		// Token: 0x0400026A RID: 618
		public string serverName;

		// Token: 0x0400026B RID: 619
		public string catalogName;

		// Token: 0x0400026C RID: 620
		public string schemaName;

		// Token: 0x0400026D RID: 621
		public string tableName;

		// Token: 0x0400026E RID: 622
		public int ordinal;

		// Token: 0x0400026F RID: 623
		public byte updatability;

		// Token: 0x04000270 RID: 624
		public byte tableNum;

		// Token: 0x04000271 RID: 625
		public bool isDifferentName;

		// Token: 0x04000272 RID: 626
		public bool isKey;

		// Token: 0x04000273 RID: 627
		public bool isNullable;

		// Token: 0x04000274 RID: 628
		public bool isHidden;

		// Token: 0x04000275 RID: 629
		public bool isExpression;

		// Token: 0x04000276 RID: 630
		public bool isIdentity;
	}
}
