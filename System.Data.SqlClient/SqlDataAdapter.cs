using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	/// <summary>表示用于填充 <see cref="T:System.Data.DataSet"></see> 和更新 SQL Server 数据库的一组数据命令和一个数据库连接。无法继承此类。</summary>
	// Token: 0x02000033 RID: 51
	public sealed class SqlDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 类的新实例。</summary>
		// Token: 0x06000299 RID: 665 RVA: 0x0000CBD8 File Offset: 0x0000BBD8
		public SqlDataAdapter()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 类的新实例，用指定的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 作为 <see cref="P:System.Data.SqlClient.SqlDataAdapter.SelectCommand"></see> 的属性。</summary>
		/// <param name="selectCommand">一个 <see cref="T:System.Data.SqlClient.SqlCommand"></see>（可以是 Transact-SQL SELECT 语句或存储过程），已设置为 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 的 <see cref="P:System.Data.SqlClient.SqlDataAdapter.SelectCommand"></see> 属性。 </param>
		// Token: 0x0600029A RID: 666 RVA: 0x0000CBE6 File Offset: 0x0000BBE6
		public SqlDataAdapter(SqlCommand selectCommand)
		{
			GC.SuppressFinalize(this);
			this.SelectCommand = selectCommand;
		}

		/// <summary>用 <see cref="P:System.Data.SqlClient.SqlDataAdapter.SelectCommand"></see> 和一个连接字符串初始化 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 类的新实例。</summary>
		/// <param name="selectCommandText">一个 <see cref="T:System.String"></see>，它是将要由 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 的 <see cref="P:System.Data.SqlClient.SqlDataAdapter.SelectCommand"></see> 属性使用的 Transact-SQL SELECT 语句或存储过程。 </param>
		/// <param name="selectConnectionString">连接字符串。 </param>
		// Token: 0x0600029B RID: 667 RVA: 0x0000CBFB File Offset: 0x0000BBFB
		public SqlDataAdapter(string selectCommandText, string selectConnectionString)
		{
			GC.SuppressFinalize(this);
			this.SelectCommand = new SqlCommand(selectCommandText, new SqlConnection(selectConnectionString));
		}

		/// <summary>使用 <see cref="P:System.Data.SqlClient.SqlDataAdapter.SelectCommand"></see> 和 <see cref="T:System.Data.SqlClient.SqlConnection"></see> 对象初始化 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 类的新实例。</summary>
		/// <param name="selectCommandText">一个 <see cref="T:System.String"></see>，它是将要由 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 的 <see cref="P:System.Data.SqlClient.SqlDataAdapter.SelectCommand"></see> 属性使用的 Transact-SQL SELECT 语句或存储过程。 </param>
		/// <param name="selectConnection">表示该连接的 <see cref="T:System.Data.SqlClient.SqlConnection"></see>。 </param>
		// Token: 0x0600029C RID: 668 RVA: 0x0000CC1B File Offset: 0x0000BC1B
		public SqlDataAdapter(string selectCommandText, SqlConnection selectConnection)
		{
			GC.SuppressFinalize(this);
			this.SelectCommand = new SqlCommand(selectCommandText, selectConnection);
		}

		/// <summary>获取或设置一个 Transact-SQL 语句或存储过程，以从数据集删除记录。</summary>
		/// <returns>在 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 过程中使用 <see cref="T:System.Data.SqlClient.SqlCommand"></see>，以在数据库中删除对应于 <see cref="T:System.Data.DataSet"></see> 中已删除行的记录。</returns>
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000CC36 File Offset: 0x0000BC36
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0000CC3E File Offset: 0x0000BC3E
		public SqlCommand DeleteCommand
		{
			get
			{
				return this.cmdDelete;
			}
			set
			{
				this.cmdDelete = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000CC47 File Offset: 0x0000BC47
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000CC4F File Offset: 0x0000BC4F
		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return this.cmdDelete;
			}
			set
			{
				this.DeleteCommand = (SqlCommand)value;
			}
		}

		/// <summary>获取或设置一个 Transact-SQL 语句或存储过程，以在数据源中插入新记录。</summary>
		/// <returns>在 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 过程中使用 <see cref="T:System.Data.SqlClient.SqlCommand"></see>，以在数据库中插入对应于 <see cref="T:System.Data.DataSet"></see> 中的新行的记录。</returns>
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000CC5D File Offset: 0x0000BC5D
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000CC65 File Offset: 0x0000BC65
		public SqlCommand InsertCommand
		{
			get
			{
				return this.cmdInsert;
			}
			set
			{
				this.cmdInsert = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000CC6E File Offset: 0x0000BC6E
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000CC76 File Offset: 0x0000BC76
		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return this.cmdInsert;
			}
			set
			{
				this.InsertCommand = (SqlCommand)value;
			}
		}

		/// <summary>获取或设置一个 Transact-SQL 语句或存储过程，用于在数据源中选择记录。</summary>
		/// <returns>在 <see cref="M:System.Data.Common.DbDataAdapter.Fill(System.Data.DataSet)"></see> 过程中使用的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>，用来从数据库中为 <see cref="T:System.Data.DataSet"></see> 中的位置选择记录。</returns>
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000CC84 File Offset: 0x0000BC84
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000CC8C File Offset: 0x0000BC8C
		public SqlCommand SelectCommand
		{
			get
			{
				return this.cmdSelect;
			}
			set
			{
				this.cmdSelect = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000CC95 File Offset: 0x0000BC95
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000CC9D File Offset: 0x0000BC9D
		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return this.cmdSelect;
			}
			set
			{
				this.SelectCommand = (SqlCommand)value;
			}
		}

		/// <summary>获取或设置一个 Transact-SQL 语句或存储过程，用于更新数据源中的记录。</summary>
		/// <returns>在 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 过程中使用的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>，用于在数据库中更新对应于 <see cref="T:System.Data.DataSet"></see> 中已修改行的记录。</returns>
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000CCAB File Offset: 0x0000BCAB
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000CCB3 File Offset: 0x0000BCB3
		public SqlCommand UpdateCommand
		{
			get
			{
				return this.cmdUpdate;
			}
			set
			{
				this.cmdUpdate = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000CCBC File Offset: 0x0000BCBC
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000CCC4 File Offset: 0x0000BCC4
		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return this.cmdUpdate;
			}
			set
			{
				this.UpdateCommand = (SqlCommand)value;
			}
		}

		/// <summary>在对数据源执行命令后的 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 过程中发生。试图进行更新，因此激发了该事件。</summary>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002AD RID: 685 RVA: 0x0000CCD2 File Offset: 0x0000BCD2
		// (remove) Token: 0x060002AE RID: 686 RVA: 0x0000CCE5 File Offset: 0x0000BCE5
		public event SqlRowUpdatedEventHandler RowUpdated
		{
			add
			{
				base.Events.AddHandler(ADP.EventRowUpdated, value);
			}
			remove
			{
				base.Events.RemoveHandler(ADP.EventRowUpdated, value);
			}
		}

		/// <summary>在对数据源执行命令前的 <see cref="M:System.Data.Common.DbDataAdapter.Update(System.Data.DataSet)"></see> 过程中发生。试图进行更新，因此激发了该事件。</summary>
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002AF RID: 687 RVA: 0x0000CCF8 File Offset: 0x0000BCF8
		// (remove) Token: 0x060002B0 RID: 688 RVA: 0x0000CD21 File Offset: 0x0000BD21
		public event SqlRowUpdatingEventHandler RowUpdating
		{
			add
			{
				SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler = (SqlRowUpdatingEventHandler)base.Events[ADP.EventRowUpdating];
				base.Events.AddHandler(ADP.EventRowUpdating, value);
			}
			remove
			{
				base.Events.RemoveHandler(ADP.EventRowUpdating, value);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000CD34 File Offset: 0x0000BD34
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new SqlRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000CD40 File Offset: 0x0000BD40
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new SqlRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000CD4C File Offset: 0x0000BD4C
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			SqlRowUpdatedEventHandler sqlRowUpdatedEventHandler = (SqlRowUpdatedEventHandler)base.Events[ADP.EventRowUpdated];
			if (sqlRowUpdatedEventHandler != null && value is SqlRowUpdatedEventArgs)
			{
				sqlRowUpdatedEventHandler(this, (SqlRowUpdatedEventArgs)value);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000CD88 File Offset: 0x0000BD88
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler = (SqlRowUpdatingEventHandler)base.Events[ADP.EventRowUpdating];
			if (sqlRowUpdatingEventHandler != null && value is SqlRowUpdatingEventArgs)
			{
				sqlRowUpdatingEventHandler(this, (SqlRowUpdatingEventArgs)value);
			}
		}

		// Token: 0x0400013D RID: 317
		private SqlCommand cmdSelect;

		// Token: 0x0400013E RID: 318
		private SqlCommand cmdInsert;

		// Token: 0x0400013F RID: 319
		private SqlCommand cmdUpdate;

		// Token: 0x04000140 RID: 320
		private SqlCommand cmdDelete;
	}
}
