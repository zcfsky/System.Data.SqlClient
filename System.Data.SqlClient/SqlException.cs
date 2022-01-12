using System;
using System.Text;

namespace System.Data.SqlClient
{
	/// <summary>当 SQL Server 返回警告或错误时引发的异常。无法继承此类。</summary>
	// Token: 0x02000036 RID: 54
	public sealed class SqlException : SystemException
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000CEE1 File Offset: 0x0000BEE1
		internal SqlException()
		{
		}

		/// <summary>获取由一个或多个 <see cref="T:System.Data.SqlClient.SqlError"></see> 对象组成的集合，这些对象提供有关 SQL Server .NET Framework 数据提供程序所生成的异常的详细信息。</summary>
		/// <returns><see cref="T:System.Data.SqlClient.SqlError"></see> 类的收集的实例。</returns>
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000CEE9 File Offset: 0x0000BEE9
		public SqlErrorCollection Errors
		{
			get
			{
				if (this._errors == null)
				{
					this._errors = new SqlErrorCollection();
				}
				return this._errors;
			}
		}

		/// <summary>获取从 SQL Server .NET Framework 数据提供程序返回的错误的严重级别。</summary>
		/// <returns>一个 1 至 25 的值，它指示错误的严重程度。</returns>
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000CF04 File Offset: 0x0000BF04
		public byte Class
		{
			get
			{
				return this.Errors[0].Class;
			}
		}

		/// <summary>获取生成错误的 Transact-SQL 批命令或存储过程内的行号。</summary>
		/// <returns>生成错误的 Transact-SQL 批命令或存储过程内的行号。</returns>
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000CF17 File Offset: 0x0000BF17
		public int LineNumber
		{
			get
			{
				return this.Errors[0].LineNumber;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000CF2C File Offset: 0x0000BF2C
		public string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.Errors.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append("\r\n");
					}
					stringBuilder.Append(this.Errors[i].Message);
				}
				return stringBuilder.ToString();
			}
		}

		/// <summary>获取一个标识错误类型的数字。</summary>
		/// <returns>标识错误类型的数字。</returns>
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000CF83 File Offset: 0x0000BF83
		public int Number
		{
			get
			{
				return this.Errors[0].Number;
			}
		}

		/// <summary>获取生成错误的存储过程或远程过程调用 (RPC) 的名称。</summary>
		/// <returns>存储过程或 RPC 的名称。</returns>
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000CF96 File Offset: 0x0000BF96
		public string Procedure
		{
			get
			{
				return this.Errors[0].Procedure;
			}
		}

		/// <summary>获取正在运行生成错误的 SQL Server 实例的计算机的名称。</summary>
		/// <returns>运行 SQL Server 实例的计算机的名称。</returns>
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000CFA9 File Offset: 0x0000BFA9
		public string Server
		{
			get
			{
				return this.Errors[0].Server;
			}
		}

		/// <summary>从 SQL Server 中获取一个数值错误代码，它表示错误、警告或“未找到数据”消息。有关如何将这些值解码的更多信息，请参见“SQL Server 联机丛书”。</summary>
		/// <returns>表示错误代码的数字。</returns>
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000CFBC File Offset: 0x0000BFBC
		public byte State
		{
			get
			{
				return this.Errors[0].State;
			}
		}

		/// <summary>获取生成错误的提供程序的名称。</summary>
		/// <returns>生成错误的提供程序的名称。</returns>
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000CFCF File Offset: 0x0000BFCF
		public string Source
		{
			get
			{
				return this.Errors[0].Source;
			}
		}

		// Token: 0x0400014A RID: 330
		private SqlErrorCollection _errors;
	}
}
