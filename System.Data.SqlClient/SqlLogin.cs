using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000042 RID: 66
	internal sealed class SqlLogin
	{
		// Token: 0x04000249 RID: 585
		public int timeout;

		// Token: 0x0400024A RID: 586
		public string hostName = "";

		// Token: 0x0400024B RID: 587
		public string userName = "";

		// Token: 0x0400024C RID: 588
		public string password = "";

		// Token: 0x0400024D RID: 589
		public string applicationName = "";

		// Token: 0x0400024E RID: 590
		public string serverName = "";

		// Token: 0x0400024F RID: 591
		public string language = "";

		// Token: 0x04000250 RID: 592
		public string database = "";

		// Token: 0x04000251 RID: 593
		public bool useSSPI;

		// Token: 0x04000252 RID: 594
		public int packetSize = 8192;
	}
}
