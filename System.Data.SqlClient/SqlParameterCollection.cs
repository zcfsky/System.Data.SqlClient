using System;
using System.Collections;

namespace System.Data.SqlClient
{
	/// <summary>表示与 <see cref="T:System.Data.SqlClient.SqlCommand"></see> 相关联的参数的集合以及各个参数到 <see cref="T:System.Data.DataSet"></see> 中列的映射。无法继承此类。</summary>
	// Token: 0x02000029 RID: 41
	public sealed class SqlParameterCollection : MarshalByRefObject, IDataParameterCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000AE63 File Offset: 0x00009E63
		internal SqlParameterCollection(SqlCommand parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000AE72 File Offset: 0x00009E72
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000AE75 File Offset: 0x00009E75
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000AE78 File Offset: 0x00009E78
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000AE7B File Offset: 0x00009E7B
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				this.ValidateType(value);
				this.items[index] = (SqlParameter)value;
			}
		}

        object IDataParameterCollection.this[string parameterName]
        {
            get
            {
                int num = this.RangeCheck(parameterName);
                return (SqlParameter)this.items[num];
            }
            set
            {
                this.OnSchemaChanging();
                int index = this.RangeCheck(parameterName);
                this.Replace(index, (SqlParameter)value);
            }
        }

		/// <summary>返回一个整数，其中包含 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中元素的数目。只读。 </summary>
		/// <returns><see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中以整数表示的元素的数目。</returns>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000AEBC File Offset: 0x00009EBC
		public int Count
		{
			get
			{
				if (this.items == null)
				{
					return 0;
				}
				return this.items.Count;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000AED3 File Offset: 0x00009ED3
		private Type ItemType
		{
			get
			{
				return typeof(SqlParameter);
			}
		}

		/// <summary>获取指定索引处的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</summary>
		/// <returns>指定索引处的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</returns>
		/// <param name="index">要检索的参数的从零开始的索引。 </param>
		// Token: 0x17000080 RID: 128
		public SqlParameter this[int index]
		{
			get
			{
				this.RangeCheck(index);
				return (SqlParameter)this.items[index];
			}
			set
			{
				this.OnSchemaChanging();
				this.RangeCheck(index);
				this.Replace(index, value);
			}
		}

		/// <summary>获取具有指定名称的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</summary>
		/// <returns>具有指定名称的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</returns>
		/// <param name="parameterName">要检索的参数的名称。 </param>
		// Token: 0x17000081 RID: 129
		public SqlParameter this[string parameterName]
		{
			get
			{
				int num = this.RangeCheck(parameterName);
				return (SqlParameter)this.items[num];
			}
			set
			{
				this.OnSchemaChanging();
				int index = this.RangeCheck(parameterName);
				this.Replace(index, value);
			}
		}

		/// <summary>将指定的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的索引。</returns>
		/// <param name="value">一个 <see cref="T:System.Object"></see>。</param>
		// Token: 0x060001CB RID: 459 RVA: 0x0000AF5B File Offset: 0x00009F5B
		public int Add(object value)
		{
			this.ValidateType(value);
			this.Add((SqlParameter)value);
			return this.Count - 1;
		}

		/// <summary>将指定的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的索引。</returns>
		/// <param name="value">要添加到集合中的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。 </param>
		// Token: 0x060001CC RID: 460 RVA: 0x0000AF79 File Offset: 0x00009F79
		public SqlParameter Add(SqlParameter value)
		{
			this.OnSchemaChanging();
			this.AddWithoutEvents(value);
			return value;
		}

		/// <summary>将指定的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的索引。使用 <see cref="M:System.Data.SqlClient.SqlParameterCollection.Add"></see> 方法的重载来指定整数参数值时，请务必谨慎。因为此重载接受 <see cref="T:System.Object"></see> 类型的 value，所以当此值为零时，必须将整数值转换为 <see cref="T:System.Object"></see> 类型，如下面的 C# 示例所示。parameters.Add("@pname", Convert.ToInt32(0));如果不执行此转换，编译器将假定您正尝试调用 SqlParameterCollection.Add (string，SqlDbType) 重载。</returns>
		/// <param name="value"><see cref="T:System.Object"></see>。</param>
		/// <param name="parameterName">要添加到集合的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的名称。</param>
		// Token: 0x060001CD RID: 461 RVA: 0x0000AF89 File Offset: 0x00009F89
		public SqlParameter Add(string parameterName, object value)
		{
			return this.Add(new SqlParameter(parameterName, value));
		}

		/// <summary>将一个值添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 的末尾。</summary>
		/// <returns>一个 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象。</returns>
		/// <param name="value">要添加的值。</param>
		/// <param name="parameterName">参数名。</param>
		// Token: 0x060001CE RID: 462 RVA: 0x0000AF98 File Offset: 0x00009F98
		public SqlParameter AddWithValue(string parameterName, object value)
		{
			return this.Add(new SqlParameter(parameterName, value));
		}

		/// <summary>将一个具有指定的参数名和数据类型的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的索引。</returns>
		/// <param name="parameterName">参数名。 </param>
		/// <param name="sqlDbType"><see cref="T:System.Data.SqlDbType"></see> 值之一。 </param>
		// Token: 0x060001CF RID: 463 RVA: 0x0000AFA7 File Offset: 0x00009FA7
		public SqlParameter Add(string parameterName, SqlDbType sqlDbType)
		{
			return this.Add(new SqlParameter(parameterName, sqlDbType));
		}

		/// <summary>将 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中（给定了指定的参数名和值）。</summary>
		/// <returns>新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的索引。</returns>
		/// <param name="size"><see cref="T:System.Int32"></see> 的大小。</param>
		/// <param name="parameterName">参数名。 </param>
		/// <param name="sqlDbType">要添加到集合中的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的 <see cref="P:System.Data.SqlClient.SqlParameter.Value"></see>。 </param>
		// Token: 0x060001D0 RID: 464 RVA: 0x0000AFB6 File Offset: 0x00009FB6
		public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size)
		{
			return this.Add(new SqlParameter(parameterName, sqlDbType, size));
		}

		/// <summary>将 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 及其参数名、数据类型和列宽添加到 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>新 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的索引。</returns>
		/// <param name="size">列长。</param>
		/// <param name="sourceColumn">源列的名称。</param>
		/// <param name="parameterName">参数名。 </param>
		/// <param name="sqlDbType"><see cref="T:System.Data.SqlDbType"></see> 值之一。 </param>
		// Token: 0x060001D1 RID: 465 RVA: 0x0000AFC6 File Offset: 0x00009FC6
		public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, string sourceColumn)
		{
			return this.Add(new SqlParameter(parameterName, sqlDbType, size, sourceColumn));
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000AFD8 File Offset: 0x00009FD8
		private void AddWithoutEvents(SqlParameter value)
		{
			this.Validate(-1, value);
			value.Parent = this;
			this.ArrayList().Add(value);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000AFF6 File Offset: 0x00009FF6
		private ArrayList ArrayList()
		{
			if (this.items == null)
			{
				this.items = new ArrayList();
			}
			return this.items;
		}

		/// <summary>确定指定的 <see cref="T:System.String"></see> 是否在此 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>
		/// 如果 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 包含该值，则为 true；否则为 false。</returns>
		/// <param name="value"><see cref="T:System.String"></see> 值。</param>
		// Token: 0x060001D4 RID: 468 RVA: 0x0000B011 File Offset: 0x0000A011
		public bool Contains(string value)
		{
			return -1 != this.IndexOf(value);
		}

		/// <summary>确定指定的 <see cref="T:System.Object"></see> 是否在此 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中。</summary>
		/// <returns>
		/// 如果 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 包含该值，则为 true；否则为 false。</returns>
		/// <param name="value"><see cref="T:System.Object"></see> 值。</param>
		// Token: 0x060001D5 RID: 469 RVA: 0x0000B020 File Offset: 0x0000A020
		public bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		/// <summary>从 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中移除所有 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象。</summary>
		// Token: 0x060001D6 RID: 470 RVA: 0x0000B02F File Offset: 0x0000A02F
		public void Clear()
		{
			if (0 < this.Count)
			{
				this.OnSchemaChanging();
				this.ClearWithoutEvents();
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B048 File Offset: 0x0000A048
		private void ClearWithoutEvents()
		{
			if (this.items != null)
			{
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					((SqlParameter)this.items[i]).Parent = null;
				}
				this.items.Clear();
			}
		}

		/// <summary>将当前 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 的所有元素复制到指定的一维 <see cref="T:System.Array"></see> 中（从指定的目标 <see cref="T:System.Array"></see> 索引开始）。</summary>
		/// <param name="array">一维 <see cref="T:System.Array"></see>，它是从当前 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 复制的元素的目标位置。</param>
		/// <param name="index">一个 32 位整数，它表示 <see cref="T:System.Array"></see> 中复制开始处的索引。</param>
		// Token: 0x060001D8 RID: 472 RVA: 0x0000B097 File Offset: 0x0000A097
		public void CopyTo(Array array, int index)
		{
			this.ArrayList().CopyTo(array, index);
		}

		/// <summary>返回循环访问 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 的枚举数。 </summary>
		/// <returns>用于 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 的 <see cref="T:System.Collections.IEnumerator"></see>。 </returns>
		// Token: 0x060001D9 RID: 473 RVA: 0x0000B0A6 File Offset: 0x0000A0A6
		public IEnumerator GetEnumerator()
		{
			return this.ArrayList().GetEnumerator();
		}

		/// <summary>获取具有指定名称的指定 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的位置。</summary>
		/// <returns>具有指定的区分大小写名称的指定 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 从零开始的位置。当 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中不存在该对象时，返回 -1。</returns>
		/// <param name="parameterName">要查找的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的区分大小写的名称。</param>
		// Token: 0x060001DA RID: 474 RVA: 0x0000B0B4 File Offset: 0x0000A0B4
		public int IndexOf(string parameterName)
		{
			if (this.items != null)
			{
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					if (ADP.SrcCompare(parameterName, ((SqlParameter)this.items[i]).ParameterName) == 0)
					{
						return i;
					}
				}
				for (int j = 0; j < count; j++)
				{
					if (ADP.DstCompare(parameterName, ((SqlParameter)this.items[j]).ParameterName) == 0)
					{
						return j;
					}
				}
			}
			return -1;
		}

		/// <summary>获取指定的 <see cref="T:System.Object"></see> 在集合中的位置。</summary>
		/// <returns>指定的 <see cref="T:System.Object"></see>（它是 <see cref="T:System.Data.SqlClient.SqlParameter"></see>）在集合中从零开始的位置。当 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中不存在该对象时，返回 -1。</returns>
		/// <param name="value">要查找的 <see cref="T:System.Object"></see>。 </param>
		// Token: 0x060001DB RID: 475 RVA: 0x0000B130 File Offset: 0x0000A130
		public int IndexOf(object value)
		{
			if (value != null)
			{
				this.ValidateType(value);
				if (this.items != null)
				{
					int count = this.items.Count;
					for (int i = 0; i < count; i++)
					{
						if (value == this.items[i])
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		/// <summary>将 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 插入 <see cref="T:System.Object"></see> 中的指定索引位置。</summary>
		/// <param name="value">要在 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中插入的 <see cref="T:System.Object"></see>。</param>
		/// <param name="index">从零开始的索引，应在该位置插入 value。</param>
		// Token: 0x060001DC RID: 476 RVA: 0x0000B179 File Offset: 0x0000A179
		public void Insert(int index, object value)
		{
			this.OnSchemaChanging();
			this.ValidateType(value);
			this.Validate(-1, (SqlParameter)value);
			((SqlParameter)value).Parent = this;
			this.ArrayList().Insert(index, value);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B1AE File Offset: 0x0000A1AE
		internal void OnSchemaChanging()
		{
			if (this.parent != null)
			{
				this.parent.OnSchemaChanging();
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000B1C4 File Offset: 0x0000A1C4
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw new IndexOutOfRangeException(Res.GetString("ADP_CollectionIndexString", new object[]
				{
					index.ToString(),
					base.GetType().Name,
					this.Count.ToString()
				}));
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000B220 File Offset: 0x0000A220
		private int RangeCheck(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw new IndexOutOfRangeException(Res.GetString("ADP_CollectionIndexString", new object[]
				{
					parameterName,
					base.GetType().Name,
					"ParameterName"
				}));
			}
			return num;
		}

		/// <summary>从 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中移除位于指定索引位置的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</summary>
		/// <param name="index">要移除的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 对象的从零开始的索引。</param>
		// Token: 0x060001E0 RID: 480 RVA: 0x0000B26C File Offset: 0x0000A26C
		public void RemoveAt(int index)
		{
			this.OnSchemaChanging();
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000B282 File Offset: 0x0000A282
		private void RemoveIndex(int index)
		{
			((SqlParameter)this.items[index]).Parent = null;
			this.items.RemoveAt(index);
		}

		/// <summary>从 <see cref="T:System.Data.SqlClient.SqlParameterCollection"></see> 中移除位于指定参数名称处的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</summary>
		/// <param name="parameterName">要移除的 <see cref="T:System.Data.SqlClient.SqlParameter"></see> 的名称。</param>
		// Token: 0x060001E2 RID: 482 RVA: 0x0000B2A8 File Offset: 0x0000A2A8
		public void RemoveAt(string parameterName)
		{
			this.OnSchemaChanging();
			int index = this.RangeCheck(parameterName);
			this.RemoveIndex(index);
		}

		/// <summary>从集合中移除指定的 <see cref="T:System.Data.SqlClient.SqlParameter"></see>。</summary>
		/// <param name="value">要从集合中移除的 <see cref="T:System.Object"></see> 对象。 </param>
		// Token: 0x060001E3 RID: 483 RVA: 0x0000B2CC File Offset: 0x0000A2CC
		public void Remove(object value)
		{
			this.OnSchemaChanging();
			this.ValidateType(value);
			int num = this.IndexOf((SqlParameter)value);
			if (-1 != num)
			{
				this.RemoveIndex(num);
				return;
			}
			throw new ArgumentException(Res.GetString("ADP_CollectionRemoveInvalidObject", new object[]
			{
				this.ItemType.Name,
				base.GetType().Name
			}));
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B332 File Offset: 0x0000A332
		private void Replace(int index, SqlParameter newValue)
		{
			this.Validate(index, newValue);
			((SqlParameter)this.items[index]).Parent = null;
			newValue.Parent = this;
			this.items[index] = newValue;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B368 File Offset: 0x0000A368
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", Res.GetString("ADP_CollectionNullValue", new object[]
				{
					base.GetType().Name,
					this.ItemType.Name
				}));
			}
			if (!this.ItemType.IsInstanceOfType(value))
			{
				throw new InvalidCastException(Res.GetString("ADP_CollectionInvalidType", new object[]
				{
					base.GetType().Name,
					this.ItemType.Name,
					value.GetType().Name
				}));
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B404 File Offset: 0x0000A404
		internal void Validate(int index, SqlParameter value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", Res.GetString("ADP_CollectionNullValue", new object[]
				{
					base.GetType().Name,
					this.ItemType.Name
				}));
			}
			if (value.Parent != null)
			{
				if (this != value.Parent)
				{
					throw new ArgumentException();
				}
				if (index != this.IndexOf(value))
				{
					throw new ArgumentException();
				}
			}
			string text = value.ParameterName;
			if (ADP.IsEmpty(text))
			{
				index = 1;
				do
				{
					text = "Parameter" + index.ToString();
					index++;
				}
				while (-1 != this.IndexOf(text));
				value.ParameterName = text;
			}
		}

		// Token: 0x0400010E RID: 270
		private SqlCommand parent;

		// Token: 0x0400010F RID: 271
		private ArrayList items;
	}
}
