using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200003F RID: 63
	internal sealed class _ValueException : SystemException
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000D2DA File Offset: 0x0000C2DA
		internal _ValueException(Exception ex, object val)
		{
			this.exception = ex;
			this.value = val;
		}

		// Token: 0x04000240 RID: 576
		internal Exception exception;

		// Token: 0x04000241 RID: 577
		internal object value;
	}
}
