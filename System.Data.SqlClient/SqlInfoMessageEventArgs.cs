using System;

namespace System.Data.SqlClient
{
	/// <summary>为 <see cref="E:System.Data.SqlClient.SqlConnection.InfoMessage"></see> 事件提供数据。</summary>
	// Token: 0x02000037 RID: 55
	public sealed class SqlInfoMessageEventArgs : EventArgs
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000CFE2 File Offset: 0x0000BFE2
		internal SqlInfoMessageEventArgs(SqlException exception)
		{
			this.exception = exception;
		}

		/// <summary>获取从服务器发送来的警告的集合。</summary>
		/// <returns>从服务器发送来的警告的集合。</returns>
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000CFF1 File Offset: 0x0000BFF1
		public SqlErrorCollection Errors
		{
			get
			{
				return this.exception.Errors;
			}
		}

		/// <summary>获取从数据库发送的全文本错误。</summary>
		/// <returns>错误的完整文本。</returns>
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000CFFE File Offset: 0x0000BFFE
		public string Message
		{
			get
			{
				return this.exception.Message;
			}
		}

		/// <summary>获取生成错误的对象的名称。</summary>
		/// <returns>生成错误的对象的名称。</returns>
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000D00B File Offset: 0x0000C00B
		public string Source
		{
			get
			{
				return this.exception.Source;
			}
		}

		/// <summary>检索 <see cref="E:System.Data.SqlClient.SqlConnection.InfoMessage"></see> 事件的字符串表示形式。</summary>
		/// <returns>表示 <see cref="E:System.Data.SqlClient.SqlConnection.InfoMessage"></see> 事件的字符串。</returns>
		// Token: 0x060002D5 RID: 725 RVA: 0x0000D018 File Offset: 0x0000C018
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x0400014B RID: 331
		private SqlException exception;
	}
}
