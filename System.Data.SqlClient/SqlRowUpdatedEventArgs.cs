using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	/// <summary>为 <see cref="E:System.Data.SqlClient.SqlDataAdapter.RowUpdated"></see> 事件提供数据。</summary>
	// Token: 0x02000039 RID: 57
	public sealed class SqlRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlRowUpdatedEventArgs"></see> 类的新实例。</summary>
		/// <param name="statementType"><see cref="T:System.Data.StatementType"></see> 值之一，该值指定所执行的查询的类型。 </param>
		/// <param name="row">通过 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 发送的 <see cref="T:System.Data.DataRow"></see>。 </param>
		/// <param name="command">当调用 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 时执行的 <see cref="T:System.Data.IDbCommand"></see>。 </param>
		/// <param name="tableMapping">通过 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 发送的 <see cref="T:System.Data.Common.DataTableMapping"></see>。 </param>
		// Token: 0x060002DA RID: 730 RVA: 0x0000D020 File Offset: 0x0000C020
		public SqlRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) : base(row, command, statementType, tableMapping)
		{
		}

		/// <summary>获取或设置当调用 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 时执行的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>。</summary>
		/// <returns>当调用 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 时执行的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>。</returns>
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000D02D File Offset: 0x0000C02D
		public SqlCommand Command
		{
			get
			{
				return (SqlCommand)base.Command;
			}
		}
	}
}
