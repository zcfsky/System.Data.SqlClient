using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200003E RID: 62
	internal enum TdsParserState
	{
		// Token: 0x0400023C RID: 572
		Closed,
		// Token: 0x0400023D RID: 573
		OpenNotLoggedIn,
		// Token: 0x0400023E RID: 574
		OpenLoggedIn,
		// Token: 0x0400023F RID: 575
		Broken
	}
}
