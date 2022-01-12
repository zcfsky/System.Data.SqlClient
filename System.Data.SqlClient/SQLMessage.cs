using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200002D RID: 45
	internal sealed class SQLMessage
	{
		// Token: 0x06000219 RID: 537 RVA: 0x0000BD30 File Offset: 0x0000AD30
		internal static string ZeroBytes()
		{
			return Res.GetString("SQL_ZeroBytes");
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000BD3C File Offset: 0x0000AD3C
		internal static string Timeout()
		{
			return Res.GetString("SQL_Timeout");
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000BD48 File Offset: 0x0000AD48
		internal static string Unknown()
		{
			return Res.GetString("SQL_Unknown");
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000BD54 File Offset: 0x0000AD54
		internal static string InsufficientMemory()
		{
			return Res.GetString("SQL_InsufficientMemory");
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000BD60 File Offset: 0x0000AD60
		internal static string AccessDenied()
		{
			return Res.GetString("SQL_AccessDenied");
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000BD6C File Offset: 0x0000AD6C
		internal static string ConnectionBusy()
		{
			return Res.GetString("SQL_ConnectionBusy");
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000BD78 File Offset: 0x0000AD78
		internal static string ConnectionBroken()
		{
			return Res.GetString("SQL_ConnectionBroken");
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000BD84 File Offset: 0x0000AD84
		internal static string ConnectionLimit()
		{
			return Res.GetString("SQL_ConnectionLimit");
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000BD90 File Offset: 0x0000AD90
		internal static string ServerNotFound(string server)
		{
			return Res.GetString("SQL_ServerNotFound", new object[]
			{
				server
			});
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000BDB3 File Offset: 0x0000ADB3
		internal static string NetworkNotFound()
		{
			return Res.GetString("SQL_NetworkNotFound");
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000BDBF File Offset: 0x0000ADBF
		internal static string InsufficientResources()
		{
			return Res.GetString("SQL_InsufficientResources");
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000BDCB File Offset: 0x0000ADCB
		internal static string NetworkBusy()
		{
			return Res.GetString("SQL_NetworkBusy");
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000BDD7 File Offset: 0x0000ADD7
		internal static string NetworkAccessDenied()
		{
			return Res.GetString("SQL_NetworkAccessDenied");
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000BDE3 File Offset: 0x0000ADE3
		internal static string GeneralError()
		{
			return Res.GetString("SQL_GeneralError");
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000BDEF File Offset: 0x0000ADEF
		internal static string IncorrectMode()
		{
			return Res.GetString("SQL_IncorrectMode");
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000BDFB File Offset: 0x0000ADFB
		internal static string NameNotFound()
		{
			return Res.GetString("SQL_NameNotFound");
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000BE07 File Offset: 0x0000AE07
		internal static string InvalidConnection()
		{
			return Res.GetString("SQL_InvalidConnection");
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000BE13 File Offset: 0x0000AE13
		internal static string ReadWriteError()
		{
			return Res.GetString("SQL_ReadWriteError");
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000BE1F File Offset: 0x0000AE1F
		internal static string TooManyHandles()
		{
			return Res.GetString("SQL_TooManyHandles");
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000BE2B File Offset: 0x0000AE2B
		internal static string ServerError()
		{
			return Res.GetString("SQL_ServerError");
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000BE37 File Offset: 0x0000AE37
		internal static string SSLError()
		{
			return Res.GetString("SQL_SSLError");
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000BE43 File Offset: 0x0000AE43
		internal static string EncryptionError()
		{
			return Res.GetString("SQL_EncryptionError");
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000BE4F File Offset: 0x0000AE4F
		internal static string EncryptionNotSupported()
		{
			return Res.GetString("SQL_EncryptionNotSupported");
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000BE5B File Offset: 0x0000AE5B
		internal static string SSPIInitializeError()
		{
			return Res.GetString("SQL_SSPIInitializeError");
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000BE67 File Offset: 0x0000AE67
		internal static string SSPIGenerateError()
		{
			return Res.GetString("SQL_SSPIGenerateError");
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000BE73 File Offset: 0x0000AE73
		internal static string SevereError()
		{
			return Res.GetString("SQL_SevereError");
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000BE7F File Offset: 0x0000AE7F
		internal static string OperationCancelled()
		{
			return Res.GetString("SQL_OperationCancelled");
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000BE8B File Offset: 0x0000AE8B
		internal static string CultureIdError()
		{
			return Res.GetString("SQL_CultureIdError");
		}
	}
}
