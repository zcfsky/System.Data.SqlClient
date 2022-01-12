using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x0200000F RID: 15
	internal class MetaBinary : MetaType
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000972C File Offset: 0x0000872C
        public MetaBinary()
            : base(byte.MaxValue, byte.MaxValue, -1, false, false, 173, "binary", typeof(byte[]), (SqlDbType)1, (DbType)1)
		{
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00009762 File Offset: 0x00008762
		public override byte PropBytes
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00009765 File Offset: 0x00008765
		public override Type SqlType
		{
			get
			{
				return typeof(SqlBinary);
			}
		}
	}
}
