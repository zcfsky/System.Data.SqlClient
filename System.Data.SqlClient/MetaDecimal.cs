using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200001E RID: 30
	internal class MetaDecimal : MetaType
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00009A84 File Offset: 0x00008A84
		public MetaDecimal() : base(38, 4, 17, true, false, 108, "decimal", typeof(decimal), (SqlDbType)5, (DbType)7)
		{
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00009AB1 File Offset: 0x00008AB1
		public override byte NullableType
		{
			get
			{
				return 108;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00009AB5 File Offset: 0x00008AB5
		public override byte PropBytes
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00009AB8 File Offset: 0x00008AB8
		public override Type SqlType
		{
			get
			{
				return typeof(SqlDecimal);
			}
		}
	}
}
