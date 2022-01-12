using System;

namespace System.Data.SqlClient
{
	/// <summary>收集与 SQL Server 返回的警告或错误有关的信息。</summary>
	// Token: 0x02000034 RID: 52
	public sealed class SqlError
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000CDC4 File Offset: 0x0000BDC4
		internal SqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber)
		{
			this.number = infoNumber;
			this.state = errorState;
			this.errorClass = errorClass;
			this.server = server.ToUpper();
			this.message = errorMessage;
			this.procedure = procedure;
			this.lineNumber = lineNumber;
		}

		/// <summary>获取错误信息的完整文本。</summary>
		/// <returns>错误信息的完整文本。</returns>
		// Token: 0x060002B6 RID: 694 RVA: 0x0000CE1C File Offset: 0x0000BE1C
		public override string ToString()
		{
			return typeof(SqlError).ToString() + ": " + this.message;
		}

		/// <summary>获取生成错误的提供程序的名称。</summary>
		/// <returns>生成错误的提供程序的名称。</returns>
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000CE3D File Offset: 0x0000BE3D
		public string Source
		{
			get
			{
				return this.source;
			}
		}

		/// <summary>获取一个标识错误类型的数字。</summary>
		/// <returns>标识错误类型的数字。</returns>
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000CE45 File Offset: 0x0000BE45
		public int Number
		{
			get
			{
				return this.number;
			}
		}

		/// <summary>从 SQL Server 中获取一个数值错误代码，它表示错误、警告或“未找到数据”消息。</summary>
		/// <returns>表示错误代码的数字。</returns>
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000CE4D File Offset: 0x0000BE4D
		public byte State
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>获取从 SQL Server 返回的错误的严重程度。</summary>
		/// <returns>一个 1 至 25 的值，它指示错误的严重程度。默认值为 0。</returns>
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000CE55 File Offset: 0x0000BE55
		public byte Class
		{
			get
			{
				return this.errorClass;
			}
		}

		/// <summary>获取生成错误的 SQL Server 实例的名称。</summary>
		/// <returns>SQL Server 实例的名称。</returns>
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000CE5D File Offset: 0x0000BE5D
		public string Server
		{
			get
			{
				return this.server;
			}
		}

		/// <summary>获取对错误进行描述的文本。</summary>
		/// <returns>对错误进行描述的文本。</returns>
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000CE65 File Offset: 0x0000BE65
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		/// <summary>获取生成错误的存储过程或远程过程调用 (RPC) 的名称。</summary>
		/// <returns>存储过程或 RPC 的名称。</returns>
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000CE6D File Offset: 0x0000BE6D
		public string Procedure
		{
			get
			{
				return this.procedure;
			}
		}

		/// <summary>从包含错误的 Transact-SQL 批命令或存储过程中获取行号。</summary>
		/// <returns>Transact-SQL 批命令或存储过程内包含错误的行号。</returns>
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000CE75 File Offset: 0x0000BE75
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x04000141 RID: 321
		private string source = ".Net SqlClient Data Provider";

		// Token: 0x04000142 RID: 322
		private int number;

		// Token: 0x04000143 RID: 323
		private byte state;

		// Token: 0x04000144 RID: 324
		private byte errorClass;

		// Token: 0x04000145 RID: 325
		private string server;

		// Token: 0x04000146 RID: 326
		private string message;

		// Token: 0x04000147 RID: 327
		private string procedure;

		// Token: 0x04000148 RID: 328
		private int lineNumber;
	}
}
