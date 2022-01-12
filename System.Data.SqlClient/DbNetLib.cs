using System;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient
{
	// Token: 0x0200002E RID: 46
	internal sealed class DbNetLib
	{
		// Token: 0x06000236 RID: 566
		[DllImport("dbnetlib.dll")]
		private static extern IntPtr PAL_LocalAlloc(int uFlags, int dwBytes);

		// Token: 0x06000237 RID: 567
		[DllImport("dbnetlib.dll")]
		private static extern int PAL_LocalFree(IntPtr handle);

		// Token: 0x06000238 RID: 568 RVA: 0x0000BEA0 File Offset: 0x0000AEA0
		public static IntPtr AllocHGlobal(int cb)
		{
			IntPtr intPtr = DbNetLib.PAL_LocalAlloc(0, cb);
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException();
			}
			return intPtr;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000BECC File Offset: 0x0000AECC
		public static void FreeHGlobal(IntPtr hglobal)
		{
			IntPtr zero = IntPtr.Zero;
			if (zero.ToInt32() != (hglobal.ToInt32() & -65536) && DbNetLib.PAL_LocalFree(hglobal) != 0)
			{
				throw new ArgumentException();
			}
		}

		// Token: 0x0600023A RID: 570
		[DllImport("dbnetlib.dll")]
		internal static extern bool ConnectionCheckForData(IntPtr pConnectionObject, ref int bytesAvail, ref int errno);

		// Token: 0x0600023B RID: 571
		[DllImport("dbnetlib.dll")]
		internal static extern int ConnectionClose(IntPtr pConnectionObject, ref int errno);

		// Token: 0x0600023C RID: 572 RVA: 0x0000BF04 File Offset: 0x0000AF04
		internal static bool ConnectionError(IntPtr pConnectionObject, ref int netErr, ref string netMsg, ref int dbErr)
		{
			IntPtr zero = IntPtr.Zero;
			bool result = DbNetLib.ConnectionErrorW(pConnectionObject, ref netErr, ref zero, ref dbErr);
			netMsg = Marshal.PtrToStringUni(zero);
			return result;
		}

		// Token: 0x0600023D RID: 573
		[DllImport("dbnetlib.dll")]
		internal static extern bool ConnectionErrorW(IntPtr pConnectionObject, ref int netErr, ref IntPtr netMsg, ref int dberr);

		// Token: 0x0600023E RID: 574
		[DllImport("dbnetlib.dll")]
		internal static extern ushort ConnectionObjectSize();

		// Token: 0x0600023F RID: 575 RVA: 0x0000BF2B File Offset: 0x0000AF2B
		internal static int ConnectionOpen(IntPtr pConnectionObject, string networkname, ref int errno)
		{
			return DbNetLib.ConnectionOpenW(pConnectionObject, networkname, ref errno);
		}

		// Token: 0x06000240 RID: 576
		[DllImport("dbnetlib.dll")]
		internal static extern int ConnectionOpenW(IntPtr pConnectionObject, string networkname, ref int errno);

		// Token: 0x06000241 RID: 577
		[DllImport("dbnetlib.dll")]
		internal static extern ushort ConnectionRead(IntPtr pConnectionObject, byte[] buffer, ushort readcount, ushort readmax, ushort timeout, ref int errno);

		// Token: 0x06000242 RID: 578
		[DllImport("dbnetlib.dll")]
		internal static extern ushort ConnectionWrite(IntPtr pConnectionObject, byte[] buffer, ushort writecount, ref int errno);

		// Token: 0x06000243 RID: 579
		[DllImport("dbnetlib.dll")]
		internal static extern ushort ConnectionWriteOOB(IntPtr pConnectionObject, byte[] buffer, ushort writecount, ref int errno);

		// Token: 0x06000244 RID: 580
		[DllImport("dbnetlib.dll")]
		internal static extern uint ConnectionSqlVer(IntPtr pConnectionObject);

		// Token: 0x06000245 RID: 581
		[DllImport("dbnetlib.dll")]
		internal static extern bool ConnectionGetSvrUser(IntPtr pConnectionObject, IntPtr pServerUserName);

		// Token: 0x06000246 RID: 582
		[DllImport("dbnetlib.dll")]
		internal static extern bool InitSSPIPackage(ref int maxLength);

		// Token: 0x06000247 RID: 583
		[DllImport("dbnetlib.dll")]
		internal static extern bool SetCredentialsHandle(IntPtr pConnectionObject, string domain, string user, string passwd);

		// Token: 0x06000248 RID: 584
		[DllImport("dbnetlib.dll")]
		internal static extern bool GenClientContext(IntPtr pConnectionObject, byte[] inBuff, int inBuffLength, byte[] outBuff, ref int outBuffLen, ref bool fDone, IntPtr pServerUserName);

		// Token: 0x06000249 RID: 585
		[DllImport("dbnetlib.dll")]
		internal static extern bool InitSession(IntPtr pConnectionObject);

		// Token: 0x0600024A RID: 586
		[DllImport("dbnetlib.dll")]
		internal static extern bool TermSession(IntPtr pConnectionObject);

		// Token: 0x04000118 RID: 280
		private const int GMEM_FIXED = 0;

		// Token: 0x04000119 RID: 281
		private const int HIWORDMASK = -65536;
	}
}
