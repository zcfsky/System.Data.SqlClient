using System;
using System.ComponentModel;

namespace System.Data.SqlClient
{
	/// <summary>自动生成单表命令，用于将对 <see cref="T:System.Data.DataSet"></see> 所做的更改与关联的 SQL Server 数据库的更改相协调。无法继承此类。 </summary>
	// Token: 0x02000009 RID: 9
	public sealed class SqlCommandBuilder : Component
	{
		/// <summary>初始化 <see cref="T:System.Data.SqlClient.SqlCommandBuilder"></see> 类的新实例。</summary>
		// Token: 0x060000B9 RID: 185 RVA: 0x00006D5F File Offset: 0x00005D5F
		public SqlCommandBuilder()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>使用关联的 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 对象初始化 <see cref="T:System.Data.SqlClient.SqlCommandBuilder"></see> 类的新实例。</summary>
		/// <param name="adapter"><see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 的名称。 </param>
		// Token: 0x060000BA RID: 186 RVA: 0x00006D6D File Offset: 0x00005D6D
		public SqlCommandBuilder(SqlDataAdapter adapter)
		{
			GC.SuppressFinalize(this);
			this.DataAdapter = adapter;
		}

		/// <summary>获取或设置自动为其生成 Transact-SQL 语句的一个 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 对象。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlDataAdapter"></see> 对象。</returns>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00006D82 File Offset: 0x00005D82
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00006D94 File Offset: 0x00005D94
		public SqlDataAdapter DataAdapter
		{
			get
			{
				return (SqlDataAdapter)this.GetBuilder().DataAdapter;
			}
			set
			{
				this.GetBuilder().DataAdapter = value;
			}
		}

		/// <summary>获取或设置在指定其名称中包含空格或保留标记等字符的 SQL Server 数据库对象（如表或列）时使用的一个或多个起始字符。</summary>
		/// <returns>要使用的一个或多个起始字符。默认为空字符串。</returns>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00006DA2 File Offset: 0x00005DA2
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00006DAF File Offset: 0x00005DAF
		public string QuotePrefix
		{
			get
			{
				return this.GetBuilder().QuotePrefix;
			}
			set
			{
				this.GetBuilder().QuotePrefix = value;
			}
		}

		/// <summary>获取或设置在指定其名称中包含空格或保留标记等字符的 SQL Server 数据库对象（如表或列）时使用的一个或多个结束字符。</summary>
		/// <returns>要使用的结束字符。默认为空字符串。</returns>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00006DBD File Offset: 0x00005DBD
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00006DCA File Offset: 0x00005DCA
		public string QuoteSuffix
		{
			get
			{
				return this.GetBuilder().QuoteSuffix;
			}
			set
			{
				this.GetBuilder().QuoteSuffix = value;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006DD8 File Offset: 0x00005DD8
		private void Dispose(bool disposing)
		{
			if (disposing && this.cmdBuilder != null)
			{
				this.cmdBuilder.Dispose();
				this.cmdBuilder = null;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006DF7 File Offset: 0x00005DF7
		private CommandBuilder GetBuilder()
		{
			if (this.cmdBuilder == null)
			{
				this.cmdBuilder = new CommandBuilder();
			}
			return this.cmdBuilder;
		}

		/// <summary>从在 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 中指定的存储过程中检索参数信息并填充指定的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象的 <see cref="P:System.Data.SqlClient.SqlCommand.Parameters"></see> 集合。</summary>
		/// <param name="command">引用将从其中导出参数信息的存储过程的 <see cref="T:System.Data.SqlClient.SqlCommand"></see>。将派生参数添加到 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 的 <see cref="P:System.Data.SqlClient.SqlCommand.Parameters"></see> 集合中。 </param>
		// Token: 0x060000C3 RID: 195 RVA: 0x00006E12 File Offset: 0x00005E12
		public static void DeriveParameters(SqlCommand command)
		{
			if (command == null)
			{
				throw new InvalidOperationException(Res.GetString("Data_ArgumentNull"));
			}
			command.DeriveParameters();
		}

		/// <summary>获取自动生成的、对数据库执行插入操作所需的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</summary>
		/// <returns>自动生成的、执行插入操作所需的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</returns>
		// Token: 0x060000C4 RID: 196 RVA: 0x00006E2D File Offset: 0x00005E2D
		public SqlCommand GetInsertCommand()
		{
			return (SqlCommand)this.GetBuilder().GetInsertCommand();
		}

		/// <summary>获取自动生成的、对数据库执行更新操作所需的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</summary>
		/// <returns>自动生成的、执行更新所需的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</returns>
		// Token: 0x060000C5 RID: 197 RVA: 0x00006E3F File Offset: 0x00005E3F
		public SqlCommand GetUpdateCommand()
		{
			return (SqlCommand)this.GetBuilder().GetUpdateCommand();
		}

		/// <summary>获取自动生成的、对数据库执行删除操作所需的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</summary>
		/// <returns>自动生成的、执行删除操作所需的 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 对象。</returns>
		// Token: 0x060000C6 RID: 198 RVA: 0x00006E51 File Offset: 0x00005E51
		public SqlCommand GetDeleteCommand()
		{
			return (SqlCommand)this.GetBuilder().GetDeleteCommand();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006E63 File Offset: 0x00005E63
		public void RefreshSchema()
		{
			this.GetBuilder().RefreshSchema();
		}

		// Token: 0x040000B5 RID: 181
		private CommandBuilder cmdBuilder;
	}
}
