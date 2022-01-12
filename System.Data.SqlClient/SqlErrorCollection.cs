using System;
using System.Collections;

namespace System.Data.SqlClient
{
	/// <summary>收集 SQL Server .NET Framework 数据提供程序生成的所有错误。无法继承此类。</summary>
	// Token: 0x02000035 RID: 53
	public sealed class SqlErrorCollection : ICollection, IEnumerable
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000CE7D File Offset: 0x0000BE7D
		internal SqlErrorCollection()
		{
		}

		/// <summary>将 <see cref="T:System.Data.SqlClient.SqlErrorCollection"></see> 集合的元素复制到 <see cref="T:System.Array"></see> 中，从指定索引开始。</summary>
		/// <param name="array">向其中复制元素的 <see cref="T:System.Array"></see>。 </param>
		/// <param name="index">从该处开始复制到 array 参数中的索引。 </param>
		// Token: 0x060002C0 RID: 704 RVA: 0x0000CE90 File Offset: 0x0000BE90
		public void CopyTo(Array array, int index)
		{
			this.errors.CopyTo(array, index);
		}

		/// <summary>获取集合中错误的数目。</summary>
		/// <returns>集合中错误的总数。</returns>
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000CE9F File Offset: 0x0000BE9F
		public int Count
		{
			get
			{
				return this.errors.Count;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000CEAC File Offset: 0x0000BEAC
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000CEAF File Offset: 0x0000BEAF
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>获取位于指定索引处的错误。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlError"></see>，包含指定索引位置的错误。</returns>
		/// <param name="index">要检索的错误的从零开始的索引。 </param>
		// Token: 0x170000CC RID: 204
		public SqlError this[int index]
		{
			get
			{
				return (SqlError)this.errors[index];
			}
		}

		/// <summary>该成员支持 .NET Framework 结构，不应从代码直接使用。</summary>
		/// <returns>一个 <see cref="T:System.Collections.IEnumerator"></see>。</returns>
		// Token: 0x060002C5 RID: 709 RVA: 0x0000CEC5 File Offset: 0x0000BEC5
		public IEnumerator GetEnumerator()
		{
			return this.errors.GetEnumerator();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000CED2 File Offset: 0x0000BED2
		internal void Add(SqlError error)
		{
			this.errors.Add(error);
		}

		// Token: 0x04000149 RID: 329
		private ArrayList errors = new ArrayList();
	}
}
