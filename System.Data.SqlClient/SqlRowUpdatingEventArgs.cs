using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	/// <summary>为 <see cref="E:System.Data.SqlClient.SqlDataAdapter.RowUpdating"></see> 事件提供数据。</summary>
	// Token: 0x0200003B RID: 59
	public sealed class SqlRowUpdatingEventArgs : RowUpdatingEventArgs
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlRowUpdatingEventArgs"></see> 类的新实例。</summary>
		/// <param name="statementType"><see cref="T:System.Data.StatementType"></see> 值之一，该值指定所执行的查询的类型。 </param>
		/// <param name="row">要进行 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 的 <see cref="T:System.Data.DataRow"></see>。 </param>
		/// <param name="command">要在 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 过程中执行的 <see cref="T:System.Data.IDbCommand"></see>。 </param>
		/// <param name="tableMapping">通过 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 发送的 <see cref="T:System.Data.Common.DataTableMapping"></see>。 </param>
		// Token: 0x060002E0 RID: 736 RVA: 0x0000D03A File Offset: 0x0000C03A
		public SqlRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) : base(row, command, statementType, tableMapping)
		{
		}

		/// <summary>获取或设置进行 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 时执行的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>。</summary>
		/// <returns>进行 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 时要执行的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>。</returns>
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000D047 File Offset: 0x0000C047
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000D054 File Offset: 0x0000C054
		public SqlCommand Command
		{
			get
			{
				return (SqlCommand)base.Command;
			}
			set
			{
				base.Command = value;
			}
		}
	}
}
