using System;
using System.IO;

namespace System.Data.SqlClient
{
	// Token: 0x0200002A RID: 42
	internal sealed class SqlStream : Stream
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x0000B4AE File Offset: 0x0000A4AE
		internal SqlStream(SqlDataReader reader, bool addByteOrderMark)
		{
			this._reader = reader;
			this._bom = (addByteOrderMark ? 65279 : 0);
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000B4CE File Offset: 0x0000A4CE
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000B4D1 File Offset: 0x0000A4D1
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000B4D4 File Offset: 0x0000A4D4
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000B4D7 File Offset: 0x0000A4D7
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000B4DE File Offset: 0x0000A4DE
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000B4E5 File Offset: 0x0000A4E5
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000B4EC File Offset: 0x0000A4EC
		public override void Close()
		{
			if (this._reader != null)
			{
				if (!this._reader.IsClosed)
				{
					this._reader.Close();
				}
				this._reader = null;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000B515 File Offset: 0x0000A515
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B51C File Offset: 0x0000A51C
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool flag = true;
			int num = 0;
			if (this._reader == null)
			{
				throw new InvalidOperationException(Res.GetString("ADP_StreamClosed", new object[]
				{
					"Read"
				}));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("count");
			}
			while (count > 0)
			{
				if (this._bom <= 0)
				{
					while (count > 0)
					{
						if (0L == this._bytesCol)
						{
							flag = false;
							while (!this._reader.Read())
							{
								if (!this._reader.NextResult())
								{
									goto IL_C0;
								}
							}
							flag = true;
						}
						IL_C0:
						if (!flag)
						{
							break;
						}
						long bytes = this._reader.GetBytes(0, this._bytesCol, buffer, offset, count);
						if (bytes < (long)count)
						{
							this._bytesCol = 0L;
						}
						else
						{
							this._bytesCol += bytes;
						}
						count -= (int)bytes;
						offset += (int)bytes;
						num += (int)bytes;
					}
					return num;
				}
				buffer[offset] = (byte)this._bom;
				this._bom >>= 8;
				offset++;
				count--;
				num++;
			}
            return num;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000B635 File Offset: 0x0000A635
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000B63C File Offset: 0x0000A63C
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000B643 File Offset: 0x0000A643
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000110 RID: 272
		private SqlDataReader _reader;

		// Token: 0x04000111 RID: 273
		private long _bytesCol;

		// Token: 0x04000112 RID: 274
		private int _bom;
	}
}
