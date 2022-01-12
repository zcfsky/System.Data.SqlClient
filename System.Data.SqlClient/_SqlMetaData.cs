using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000048 RID: 72
	internal sealed class _SqlMetaData : SqlMetaData
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000D3A1 File Offset: 0x0000C3A1
		public _SqlMetaData()
		{
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D3AC File Offset: 0x0000C3AC
		internal _SqlMetaData(SqlReturnValue rec, MetaType mt)
		{
			this.type = rec.type;
			this.tdsType = rec.tdsType;
			this.length = rec.length;
			this.column = rec.parameter;
			this.collation = rec.collation;
			this.codePage = rec.codePage;
			this.encoding = rec.encoding;
			this.precision = rec.precision;
			this.scale = rec.scale;
			this.isNullable = rec.isNullable;
			this.metaType = mt;
		}

		// Token: 0x04000284 RID: 644
		public MetaType metaType;
	}
}
