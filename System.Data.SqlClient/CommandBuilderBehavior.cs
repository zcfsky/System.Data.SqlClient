using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000002 RID: 2
	[Flags]
	internal enum CommandBuilderBehavior
	{
		// Token: 0x04000002 RID: 2
		Default = 0,
		// Token: 0x04000003 RID: 3
		UpdateSetSameValue = 1,
		// Token: 0x04000004 RID: 4
		UseRowVersionInUpdateWhereClause = 2,
		// Token: 0x04000005 RID: 5
		UseRowVersionInDeleteWhereClause = 4,
		// Token: 0x04000006 RID: 6
		UseRowVersionInWhereClause = 6,
		// Token: 0x04000007 RID: 7
		PrimaryKeyOnlyUpdateWhereClause = 16,
		// Token: 0x04000008 RID: 8
		PrimaryKeyOnlyDeleteWhereClause = 32,
		// Token: 0x04000009 RID: 9
		PrimaryKeyOnlyWhereClause = 48
	}
}
