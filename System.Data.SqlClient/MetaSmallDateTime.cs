using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000021 RID: 33
	internal sealed class MetaSmallDateTime : MetaDateTime
	{
		// Token: 0x0600016C RID: 364 RVA: 0x00009B8A File Offset: 0x00008B8A
		public MetaSmallDateTime()
		{
			base.Precision = 16;
			base.Scale = 0;
			base.FixedLength = 4;
			base.TDSType = 58;
			base.TypeName = "smalldatetime";
			base.SqlDbType = (SqlDbType)15;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00009BC3 File Offset: 0x00008BC3
		public override byte NullableType
		{
			get
			{
				return 111;
			}
		}
	}
}
