using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x0200004A RID: 74
	internal sealed class TdsParser
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0000D47C File Offset: 0x0000C47C
		~TdsParser()
		{
			this.Dispose(false);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D4AC File Offset: 0x0000C4AC
		private void Dispose(bool disposing)
		{
			this.InternalDisconnect();
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000D4B4 File Offset: 0x0000C4B4
		internal TdsParserState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D4BC File Offset: 0x0000C4BC
		public int Connect(string host, SqlInternalConnection connHandler, int timeout)
		{
			if (this.state != TdsParserState.Closed)
			{
				return timeout;
			}
			int num = 0;
			this.server = host;
			this.connHandler = connHandler;
			try
			{
				num = (int)DbNetLib.ConnectionObjectSize();
			}
			catch (TypeLoadException)
			{
				throw SQL.MDAC_WrongVersion();
			}
			this.pConObj = DbNetLib.AllocHGlobal(num);
			byte[] array = new byte[num];
			Marshal.Copy(array, 0, this.pConObj, num);
			string networkname = "tcp:" + host;
			int num2 = 1;
			if (timeout == 0)
			{
				timeout = int.MaxValue;
			}
			TimeSpan timeSpan = new TimeSpan(0, 0, timeout);
			DateTime now = DateTime.Now;
			if (num2 == 1)
			{
				num2 = 0;
				while (TimeSpan.Compare(DateTime.Now.Subtract(now), timeSpan) < 0 && num2 == 0)
				{
					num2 = DbNetLib.ConnectionOpen(this.pConObj, networkname, ref this.errno);
				}
			}
			TimeSpan timeSpan2 = DateTime.Now.Subtract(now);
			int num3 = timeout - (60 * timeSpan2.Minutes + timeSpan2.Seconds);
			if (num2 == 0)
			{
				if (this.exception == null)
				{
					this.exception = new SqlException();
				}
				this.exception.Errors.Add(this.ProcessNetlibError(this.errno));
				DbNetLib.FreeHGlobal(this.pConObj);
				this.ThrowExceptionAndWarning();
				return 0;
			}
			uint num4 = DbNetLib.ConnectionSqlVer(this.pConObj);
			uint num5 = num4 >> 24;
			uint num6 = num4 >> 16 & 255U;
			if ((ulong)num5 < 7UL)
			{
				DbNetLib.FreeHGlobal(this.pConObj);
				throw SQL.InvalidSQLServerVersion(num5 + "." + num6);
			}
			this.state = TdsParserState.OpenNotLoggedIn;
			if (num3 < 0)
			{
				return 0;
			}
			return num3;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D654 File Offset: 0x0000C654
		public void Disconnect()
		{
			this.InternalDisconnect();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D65C File Offset: 0x0000C65C
		private void InternalDisconnect()
		{
			if (this.state != TdsParserState.Closed)
			{
				this.state = TdsParserState.Closed;
				int num = 0;
				try
				{
					num = DbNetLib.ConnectionClose(this.pConObj, ref this.errno);
					if (num == 0)
					{
						if (this.exception == null)
						{
							this.exception = new SqlException();
						}
						this.exception.Errors.Add(this.ProcessNetlibError(this.errno));
					}
				}
				finally
				{
					try
					{
						DbNetLib.FreeHGlobal(this.pConObj);
					}
					catch
					{
					}
				}
				if (num == 0)
				{
					this.ThrowExceptionAndWarning();
				}
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000D6F8 File Offset: 0x0000C6F8
		private void ThrowExceptionAndWarning()
		{
			SqlException ex;
			if (this.exception != null)
			{
				if (this.warning != null)
				{
					int count = this.warning.Errors.Count;
					for (int i = 0; i < count; i++)
					{
						this.exception.Errors.Add(this.warning.Errors[i]);
					}
				}
				ex = this.exception;
				this.exception = null;
				this.warning = null;
				if (this.state != TdsParserState.Closed)
				{
					for (int j = 0; j < ex.Errors.Count; j++)
					{
						if (ex.Errors[j].Class >= 20)
						{
							this.state = TdsParserState.Broken;
							break;
						}
					}
				}
				this.connHandler.OnError(ex, this.state);
				return;
			}
			ex = this.warning;
			this.warning = null;
			this.connHandler.OnError(ex, this.state);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000D7DC File Offset: 0x0000C7DC
		private SqlError ProcessNetlibError(int errno)
		{
			int num = 0;
			string text = null;
			int num2 = 0;
			DbNetLib.ConnectionError(this.pConObj, ref num, ref text, ref num2);
			string procedure = text;
			string errorMessage = string.Empty;
			int infoNumber;
			if (errno >= -3 && errno <= -1)
			{
				infoNumber = errno;
			}
			else
			{
				infoNumber = num2;
			}
			byte errorClass;
			switch (infoNumber)
			{
			case -3:
				errorMessage = SQLMessage.ZeroBytes();
				errorClass = 20;
				goto IL_20E;
			case -2:
				errorMessage = SQLMessage.Timeout();
				errorClass = 10;
				goto IL_20E;
			case -1:
				errorMessage = SQLMessage.Unknown();
				errorClass = 20;
				goto IL_20E;
			case 1:
				errorMessage = SQLMessage.InsufficientMemory();
				errorClass = 20;
				goto IL_20E;
			case 2:
				errorMessage = SQLMessage.AccessDenied();
				errorClass = 20;
				goto IL_20E;
			case 3:
				errorMessage = SQLMessage.ConnectionBusy();
				errorClass = 10;
				goto IL_20E;
			case 4:
				errorMessage = SQLMessage.ConnectionBroken();
				errorClass = 20;
				goto IL_20E;
			case 5:
				errorMessage = SQLMessage.ConnectionLimit();
				errorClass = 10;
				goto IL_20E;
			case 6:
				errorMessage = SQLMessage.ServerNotFound(this.server);
				errorClass = 20;
				goto IL_20E;
			case 7:
				errorMessage = SQLMessage.NetworkNotFound();
				errorClass = 20;
				goto IL_20E;
			case 8:
				errorMessage = SQLMessage.InsufficientResources();
				errorClass = 20;
				goto IL_20E;
			case 9:
				errorMessage = SQLMessage.NetworkBusy();
				errorClass = 10;
				goto IL_20E;
			case 10:
				errorMessage = SQLMessage.NetworkAccessDenied();
				errorClass = 20;
				goto IL_20E;
			case 11:
				errorMessage = SQLMessage.GeneralError();
				errorClass = 20;
				goto IL_20E;
			case 12:
				errorMessage = SQLMessage.IncorrectMode();
				errorClass = 10;
				goto IL_20E;
			case 13:
				errorMessage = SQLMessage.NameNotFound();
				errorClass = 20;
				goto IL_20E;
			case 14:
				errorMessage = SQLMessage.InvalidConnection();
				errorClass = 20;
				goto IL_20E;
			case 15:
				errorMessage = SQLMessage.ReadWriteError();
				errorClass = 20;
				goto IL_20E;
			case 16:
				errorMessage = SQLMessage.TooManyHandles();
				errorClass = 10;
				goto IL_20E;
			case 17:
				errorMessage = SQLMessage.ServerError();
				errorClass = 20;
				goto IL_20E;
			case 18:
				errorMessage = SQLMessage.SSLError();
				errorClass = 20;
				goto IL_20E;
			case 19:
				errorMessage = SQLMessage.EncryptionError();
				errorClass = 10;
				goto IL_20E;
			case 20:
				errorMessage = SQLMessage.EncryptionNotSupported();
				errorClass = 20;
				goto IL_20E;
			}
			infoNumber = num;
			errorMessage = SQLMessage.Unknown();
			errorClass = 20;
			IL_20E:
			return new SqlError(infoNumber, 0, errorClass, this.server, errorMessage, procedure, 0);
		}

		// Token: 0x170000DC RID: 220
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000DA0B File Offset: 0x0000CA0B
		private int PacketSize
		{
			set
			{
				this.SetOutBufferSize(value);
				this.SetInBufferSize(value);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DA1B File Offset: 0x0000CA1B
		private void SetOutBufferSize(int size)
		{
			if (this.outBuff.Length == size)
			{
				return;
			}
			this.FlushBuffer(0);
			this.outBuff = new byte[size];
			this.outBytesUsed = 8;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DA44 File Offset: 0x0000CA44
		private void SetInBufferSize(int size)
		{
			if (size > this.inBuff.Length)
			{
				if (this.inBytesRead > this.inBytesUsed)
				{
					byte[] array = this.inBuff;
					this.inBuff = new byte[size];
					Array.Copy(array, this.inBytesUsed, this.inBuff, 0, this.inBytesRead - this.inBytesUsed);
					this.inBytesRead -= this.inBytesUsed;
					this.inBytesUsed = 0;
					return;
				}
				this.inBuff = new byte[size];
				this.inBytesRead = 0;
				this.inBytesUsed = 0;
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DAD4 File Offset: 0x0000CAD4
		private void FlushBuffer(byte status)
		{
			if (this.state == TdsParserState.Closed || this.state == TdsParserState.Broken)
			{
				return;
			}
			if (this.outBytesUsed == 8 && this.packetNumber == 1)
			{
				return;
			}
			this.outBuff[0] = this.msgType;
			this.outBuff[2] = (byte)(this.outBytesUsed >> 8);
			this.outBuff[3] = (byte)this.outBytesUsed;
			this.outBuff[4] = 0;
			this.outBuff[5] = 0;
			this.outBuff[6] = this.packetNumber;
			this.outBuff[7] = 0;
			if (status == 1)
			{
				this.outBuff[1] = 1;
				this.packetNumber = 1;
			}
			else
			{
				this.outBuff[1] = 4;
				this.packetNumber += 1;
			}
			if (this.fResetConnection)
			{
				this.outBuff[1] = (byte)(this.outBuff[1] | 8);
				this.fResetConnection = false;
			}
			int num = (int)DbNetLib.ConnectionWrite(this.pConObj, this.outBuff, (ushort)this.outBytesUsed, ref this.errno);
			if (num != this.outBytesUsed)
			{
				if (this.exception == null)
				{
					this.exception = new SqlException();
				}
				this.exception.Errors.Add(this.ProcessNetlibError(this.errno));
				this.ThrowExceptionAndWarning();
				return;
			}
			this.ResetBuffer();
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DC11 File Offset: 0x0000CC11
		internal void ResetBuffer()
		{
			this.outBytesUsed = 8;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DC1C File Offset: 0x0000CC1C
		private void ReadBuffer()
		{
			if (this.inBytesRead == this.inBuff.Length && this.inBuff.Length < 65535)
			{
				int num = this.inBuff.Length * 2;
				if (num > 65535)
				{
					num = 65535;
				}
				this.inBuff = new byte[num];
				this.inBytesRead = 0;
				this.inBytesUsed = 0;
			}
			if (this.inBytesPacket > 0)
			{
				this.ReadNetlib(1);
				return;
			}
			if (this.inBytesPacket == 0)
			{
				this.ReadNetlib(8);
				this.ProcessHeader();
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DCA4 File Offset: 0x0000CCA4
		private void ReadNetlib(int bytesExpected)
		{
			DateTime now = DateTime.Now;
			int num = 60 * this.timeRemaining.Minutes + this.timeRemaining.Seconds;
			this.inBytesRead = (int)DbNetLib.ConnectionRead(this.pConObj, this.inBuff, (ushort)bytesExpected, (ushort)this.inBuff.Length, (ushort)num, ref this.errno);
			this.timeRemaining = this.timeRemaining.Subtract(DateTime.Now.Subtract(now));
			this.inBytesUsed = 0;
			if (this.inBytesRead < bytesExpected)
			{
				if (this.errno == -2)
				{
					this.SendAttention();
					this.ProcessAttention();
					if (this.exception == null)
					{
						this.exception = new SqlException();
					}
					this.exception.Errors.Add(this.ProcessNetlibError(-2));
					this.ThrowExceptionAndWarning();
					return;
				}
				if (this.exception == null)
				{
					this.exception = new SqlException();
				}
				this.exception.Errors.Add(this.ProcessNetlibError(this.errno));
				this.ThrowExceptionAndWarning();
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000DDA8 File Offset: 0x0000CDA8
		private void ProcessHeader()
		{
			if (this.inBytesUsed + 8 > this.inBytesRead)
			{
				int num = this.inBytesRead - this.inBytesUsed;
				int num2 = 8 - num;
				byte[] array = new byte[8];
				Array.Copy(this.inBuff, this.inBytesUsed, array, 0, num);
				this.inBytesUsed = this.inBytesRead;
				this.ReadNetlib(num2);
				Array.Copy(this.inBuff, this.inBytesUsed, array, num, num2);
				this.inBytesPacket = (((int)array[2] & 16777215) << 8) + ((int)array[3] & 16777215) - 8;
				this.inBytesUsed = num2;
				this._type = array[0];
				this._status = array[1];
				return;
			}
			this._status = this.inBuff[this.inBytesUsed + 1];
			this._type = this.inBuff[this.inBytesUsed];
			this.inBytesPacket = (((int)this.inBuff[this.inBytesUsed + 2] & 16777215) << 8) + ((int)this.inBuff[this.inBytesUsed + 2 + 1] & 16777215) - 8;
			this.inBytesUsed += 8;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000DEC0 File Offset: 0x0000CEC0
		private void WriteByte(byte b)
		{
			if (this.outBytesUsed == this.outBuff.Length)
			{
				this.FlushBuffer(0);
			}
			this.outBuff[this.outBytesUsed++] = b;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000DF00 File Offset: 0x0000CF00
		internal byte PeekByte()
		{
			byte result = this.ReadByte();
			this.inBytesPacket++;
			this.inBytesUsed--;
			return result;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000DF34 File Offset: 0x0000CF34
		public byte ReadByte()
		{
			if (this.inBytesUsed == this.inBytesRead)
			{
				this.ReadBuffer();
			}
			else if (this.inBytesPacket == 0)
			{
				this.ProcessHeader();
				if (this.inBytesUsed == this.inBytesRead)
				{
					this.ReadBuffer();
				}
			}
			this.inBytesPacket--;
			return this.inBuff[this.inBytesUsed++];
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000DFA0 File Offset: 0x0000CFA0
		public void ReadByteArray(byte[] buff, int offset, int len)
		{
			while (len > 0)
			{
				if (len <= this.inBytesPacket && this.inBytesUsed + len <= this.inBytesRead)
				{
					if (buff != null)
					{
						Array.Copy(this.inBuff, this.inBytesUsed, buff, offset, len);
					}
					this.inBytesUsed += len;
					this.inBytesPacket -= len;
					return;
				}
				if ((len <= this.inBytesPacket && this.inBytesUsed + len > this.inBytesRead) || (len > this.inBytesPacket && this.inBytesUsed + this.inBytesPacket > this.inBytesRead))
				{
					int num = this.inBytesRead - this.inBytesUsed;
					if (buff != null)
					{
						Array.Copy(this.inBuff, this.inBytesUsed, buff, offset, num);
					}
					offset += num;
					this.inBytesUsed += num;
					this.inBytesPacket -= num;
					len -= num;
					this.ReadBuffer();
				}
				else if (len > this.inBytesPacket && this.inBytesUsed + this.inBytesPacket <= this.inBytesRead)
				{
					if (buff != null)
					{
						Array.Copy(this.inBuff, this.inBytesUsed, buff, offset, this.inBytesPacket);
					}
					this.inBytesUsed += this.inBytesPacket;
					offset += this.inBytesPacket;
					len -= this.inBytesPacket;
					this.inBytesPacket = 0;
					if (this.inBytesUsed == this.inBytesRead)
					{
						this.ReadBuffer();
					}
					else
					{
						this.ProcessHeader();
						if (this.inBytesUsed == this.inBytesRead)
						{
							this.ReadBuffer();
						}
					}
				}
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000E130 File Offset: 0x0000D130
		private void WriteByteArray(byte[] b, int len, int offsetBuffer)
		{
			int num = offsetBuffer;
			while (len > 0)
			{
				if (this.outBytesUsed + len <= this.outBuff.Length)
				{
					Array.Copy(b, num, this.outBuff, this.outBytesUsed, len);
					this.outBytesUsed += len;
					return;
				}
				int num2 = this.outBuff.Length - this.outBytesUsed;
				Array.Copy(b, num, this.outBuff, this.outBytesUsed, num2);
				num += num2;
				this.outBytesUsed += num2;
				if (this.outBytesUsed == this.outBuff.Length)
				{
					this.FlushBuffer(0);
				}
				len -= num2;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000E1D4 File Offset: 0x0000D1D4
		private void WriteShort(int v)
		{
			if (this.outBytesUsed + 2 > this.outBuff.Length)
			{
				this.WriteByte((byte)(v & 255));
				this.WriteByte((byte)(v >> 8 & 255));
				return;
			}
			this.outBuff[this.outBytesUsed++] = (byte)(v & 255);
			this.outBuff[this.outBytesUsed++] = (byte)(v >> 8 & 255);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000E254 File Offset: 0x0000D254
		public short ReadShort()
		{
			byte b = this.ReadByte();
			byte b2 = this.ReadByte();
			return (short)(((int)b2 << 8) + (int)b);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000E278 File Offset: 0x0000D278
		public ushort ReadUnsignedShort()
		{
			byte b = this.ReadByte();
			byte b2 = this.ReadByte();
			return (ushort)(((int)b2 << 8) + (int)b);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E29C File Offset: 0x0000D29C
		public uint ReadUnsignedInt()
		{
			if (this.inBytesUsed + 4 > this.inBytesRead || this.inBytesPacket < 4)
			{
				this.ReadByteArray(this.bTmp, 0, 4);
				return BitConverter.ToUInt32(this.bTmp, 0);
			}
			uint result = BitConverter.ToUInt32(this.inBuff, this.inBytesUsed);
			this.inBytesUsed += 4;
			this.inBytesPacket -= 4;
			return result;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000E30C File Offset: 0x0000D30C
		private void WriteUnsignedInt(uint i)
		{
			this.WriteByteArray(BitConverter.GetBytes(i), 4, 0);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000E31C File Offset: 0x0000D31C
		private void WriteInt(int v)
		{
			this.WriteByteArray(BitConverter.GetBytes(v), 4, 0);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000E32C File Offset: 0x0000D32C
		public int ReadInt()
		{
			if (this.inBytesUsed + 4 > this.inBytesRead || this.inBytesPacket < 4)
			{
				this.ReadByteArray(this.bTmp, 0, 4);
				return BitConverter.ToInt32(this.bTmp, 0);
			}
			int result = BitConverter.ToInt32(this.inBuff, this.inBytesUsed);
			this.inBytesUsed += 4;
			this.inBytesPacket -= 4;
			return result;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000E39C File Offset: 0x0000D39C
		private void WriteFloat(float v)
		{
			byte[] bytes = BitConverter.GetBytes(v);
			this.WriteByteArray(bytes, bytes.Length, 0);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E3BC File Offset: 0x0000D3BC
		public float ReadFloat()
		{
			if (this.inBytesUsed + 4 > this.inBytesRead || this.inBytesPacket < 4)
			{
				this.ReadByteArray(this.bTmp, 0, 4);
				return BitConverter.ToSingle(this.bTmp, 0);
			}
			float result = BitConverter.ToSingle(this.inBuff, this.inBytesUsed);
			this.inBytesUsed += 4;
			this.inBytesPacket -= 4;
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000E42C File Offset: 0x0000D42C
		private void WriteLong(long v)
		{
			byte[] bytes = BitConverter.GetBytes(v);
			this.WriteByteArray(bytes, bytes.Length, 0);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000E44C File Offset: 0x0000D44C
		public long ReadLong()
		{
			if (this.inBytesUsed + 8 > this.inBytesRead || this.inBytesPacket < 8)
			{
				this.ReadByteArray(this.bTmp, 0, 8);
				return BitConverter.ToInt64(this.bTmp, 0);
			}
			long result = BitConverter.ToInt64(this.inBuff, this.inBytesUsed);
			this.inBytesUsed += 8;
			this.inBytesPacket -= 8;
			return result;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000E4BC File Offset: 0x0000D4BC
		private void WriteDouble(double v)
		{
			byte[] bytes = BitConverter.GetBytes(v);
			this.WriteByteArray(bytes, bytes.Length, 0);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000E4DC File Offset: 0x0000D4DC
		public double ReadDouble()
		{
			if (this.inBytesUsed + 8 > this.inBytesRead || this.inBytesPacket < 8)
			{
				this.ReadByteArray(this.bTmp, 0, 8);
				return BitConverter.ToDouble(this.bTmp, 0);
			}
			double result = BitConverter.ToDouble(this.inBuff, this.inBytesUsed);
			this.inBytesUsed += 8;
			this.inBytesPacket -= 8;
			return result;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000E54C File Offset: 0x0000D54C
		public void SkipBytes(long num)
		{
			while (num > 0L)
			{
				int num2 = (num > 2147483647L) ? int.MaxValue : ((int)num);
				this.ReadByteArray(null, 0, num2);
				num -= (long)num2;
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E584 File Offset: 0x0000D584
		public void PrepareResetConnection()
		{
			this.fResetConnection = true;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000E58D File Offset: 0x0000D58D
		internal void Run(RunBehavior run)
		{
			this.Run(run, null, null);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E599 File Offset: 0x0000D599
		internal void Run(RunBehavior run, SqlCommand cmdHandler)
		{
			this.Run(run, cmdHandler, null);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000E5A8 File Offset: 0x0000D5A8
		internal bool Run(RunBehavior run, SqlCommand cmdHandler, SqlDataReader dataStream)
		{
			bool result = false;
			bool flag = false;
			_SqlMetaData[] columns = null;
			if (cmdHandler != null)
			{
				if (this.pendingCommandWeakRef != null)
				{
					this.pendingCommandWeakRef.Target = cmdHandler;
				}
				else
				{
					this.pendingCommandWeakRef = new WeakReference(cmdHandler);
				}
			}
			do
			{
				byte b = this.ReadByte();
				int tokenLength = this.GetTokenLength(b);
				byte b2 = b;
				if (b2 <= 173)
				{
					if (b2 <= 129)
					{
						if (b2 != 121)
						{
							if (b2 == 129)
							{
								if (tokenLength != 65535)
								{
									this.cleanupMetaData = this.ProcessMetaData(tokenLength);
								}
								else if (cmdHandler != null)
								{
									this.cleanupMetaData = cmdHandler.MetaData;
								}
								if (dataStream != null)
								{
									byte b3 = this.PeekByte();
									dataStream.SetMetaData(this.cleanupMetaData, 164 == b3 || 165 == b3);
								}
								if (136 == this.PeekByte())
								{
									flag = true;
									this.CleanWire();
								}
							}
						}
						else
						{
							int status = this.ReadInt();
							if (cmdHandler != null)
							{
								cmdHandler.OnReturnStatus(status);
							}
						}
					}
					else if (b2 != 136)
					{
						switch (b2)
						{
						case 164:
							if (dataStream != null)
							{
								if (this.isShilohSP1)
								{
									dataStream.TableNamesShilohSP1 = this.ProcessTableNameShilohSP1(tokenLength);
								}
								else
								{
									dataStream.TableNames = this.ProcessTableName(tokenLength);
								}
							}
							else
							{
								this.SkipBytes((long)tokenLength);
							}
							break;
						case 165:
							if (dataStream != null)
							{
								dataStream.SetMetaData(this.ProcessColInfo(dataStream.MetaData, dataStream), false);
								dataStream.BrowseModeInfoConsumed = true;
							}
							else
							{
								this.SkipBytes((long)tokenLength);
							}
							if (136 == this.PeekByte())
							{
								flag = true;
								this.CleanWire();
							}
							break;
						case 169:
							this.SkipBytes((long)tokenLength);
							break;
						case 170:
						case 171:
						{
							SqlError sqlError = this.ProcessError(b);
							if (RunBehavior.Clean != run)
							{
								if (sqlError.Class < 10)
								{
									if (this.warning == null)
									{
										this.warning = new SqlException();
									}
									this.warning.Errors.Add(sqlError);
								}
								else
								{
									if (this.exception == null)
									{
										this.exception = new SqlException();
									}
									this.exception.Errors.Add(sqlError);
								}
							}
							else if (sqlError.Class >= 20)
							{
								if (this.exception == null)
								{
									this.exception = new SqlException();
								}
								this.exception.Errors.Add(sqlError);
							}
							break;
						}
						case 172:
							cmdHandler.OnReturnValue(this.ProcessReturnValue(tokenLength));
							break;
						case 173:
						{
							SqlLoginAck rec = this.ProcessLoginAck();
							this.connHandler.OnLoginAck(rec);
							break;
						}
						}
					}
					else
					{
						columns = this.SkipAltMetaData(tokenLength);
					}
				}
				else if (b2 <= 227)
				{
					switch (b2)
					{
					case 209:
						if (run != RunBehavior.ReturnImmediately)
						{
							this.SkipRow(this.cleanupMetaData);
						}
						result = true;
						break;
					case 210:
						break;
					case 211:
						this.SkipBytes(2L);
						this.SkipRow(columns);
						break;
					default:
						if (b2 == 227)
						{
							SqlEnvChange rec2 = this.ProcessEnvChange(tokenLength);
							this.connHandler.OnEnvChange(rec2);
						}
						break;
					}
				}
				else if (b2 != 237)
				{
					switch (b2)
					{
					case 253:
					case 254:
					case 255:
						this.ProcessDone(cmdHandler, run);
						break;
					}
				}
				else
				{
					this.ProcessSSPI(tokenLength);
				}
			}
			while (this.pendingData && run != RunBehavior.ReturnImmediately);
			if (this.exception != null || this.warning != null)
			{
				this.ThrowExceptionAndWarning();
			}
			if (flag)
			{
				throw SQL.ComputeByNotSupported();
			}
			return result;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000E9FC File Offset: 0x0000D9FC
		private SqlEnvChange ProcessEnvChange(int tokenLength)
		{
			SqlEnvChange sqlEnvChange = new SqlEnvChange();
			sqlEnvChange.length = tokenLength;
			sqlEnvChange.type = this.ReadByte();
			if (sqlEnvChange.type == 7)
			{
				sqlEnvChange.newLength = this.ReadByte();
				if (sqlEnvChange.newLength == 5)
				{
					sqlEnvChange.newCollation = this.ProcessCollation();
					this.defaultCollation = sqlEnvChange.newCollation;
					this.defaultCodePage = this.GetCodePage(sqlEnvChange.newCollation);
					this.defaultEncoding = Encoding.GetEncoding(this.defaultCodePage);
					this.defaultLCID = (int)(sqlEnvChange.newCollation.info & 1048575U);
				}
				sqlEnvChange.oldLength = this.ReadByte();
				if (sqlEnvChange.oldLength == 5)
				{
					sqlEnvChange.oldCollation = this.ProcessCollation();
				}
			}
			else
			{
				sqlEnvChange.newLength = this.ReadByte();
				sqlEnvChange.newValue = this.ReadString((int)sqlEnvChange.newLength);
				sqlEnvChange.oldLength = this.ReadByte();
				sqlEnvChange.oldValue = this.ReadString((int)sqlEnvChange.oldLength);
				if (sqlEnvChange.type == 5)
				{
					this.defaultLCID = int.Parse(sqlEnvChange.newValue, CultureInfo.InvariantCulture);
				}
				else if (sqlEnvChange.type == 3)
				{
					if (sqlEnvChange.newValue == "iso_1")
					{
						this.defaultCodePage = 1252;
						this.defaultEncoding = Encoding.GetEncoding(this.defaultCodePage);
					}
					else
					{
						string text = sqlEnvChange.newValue.Substring(2);
						this.defaultCodePage = int.Parse(text, CultureInfo.InvariantCulture);
						this.defaultEncoding = Encoding.GetEncoding(this.defaultCodePage);
					}
				}
				if (sqlEnvChange.type == 4)
				{
					this.PacketSize = int.Parse(sqlEnvChange.newValue);
				}
			}
			return sqlEnvChange;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000EBA0 File Offset: 0x0000DBA0
		private void ProcessDone(SqlCommand cmd, RunBehavior run)
		{
			int num = (int)this.ReadUnsignedShort();
			ushort num2 = this.ReadUnsignedShort();
			int recordsAffected = this.ReadInt();
			if (this.attentionSent)
			{
				this.ProcessAttention(num, run);
			}
			if (cmd != null && 16 == (num & 16) && num2 != 193)
			{
				cmd.RecordsAffected = recordsAffected;
			}
			if (2 == (2 & num) && this.exception == null && RunBehavior.Clean != run)
			{
				this.exception = new SqlException();
				this.exception.Errors.Add(new SqlError(0, 0, 10, this.server, SQLMessage.SevereError(), "", 0));
				this.pendingData = false;
				this.ThrowExceptionAndWarning();
			}
			if (256 == (256 & num) && RunBehavior.Clean != run)
			{
				this.exception = new SqlException();
				this.exception.Errors.Add(new SqlError(0, 0, 10, this.server, SQLMessage.SevereError(), "", 0));
				this.pendingData = false;
				this.ThrowExceptionAndWarning();
			}
			if ((num & 1) == 0 && this.inBytesUsed >= this.inBytesRead)
			{
				this.pendingData = false;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000ECB0 File Offset: 0x0000DCB0
		private void ThrowAttentionError()
		{
			this.attentionSent = (this.pendingData = false);
			this.inBytesPacket = (this.inBytesUsed = (this.inBytesRead = 0));
			this.exception = new SqlException();
			this.exception.Errors.Add(new SqlError(0, 0, 10, this.server, SQLMessage.OperationCancelled(), "", 0));
			this.ThrowExceptionAndWarning();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000ED24 File Offset: 0x0000DD24
		private SqlLoginAck ProcessLoginAck()
		{
			SqlLoginAck sqlLoginAck = new SqlLoginAck();
			this.SkipBytes(1L);
			byte[] array = new byte[4];
			this.ReadByteArray(array, 0, array.Length);
			int num = (int)array[0] << 8 | (int)array[1];
			int num2 = (int)array[2] << 8 | (int)array[3];
			sqlLoginAck.isVersion8 = (this.isShiloh = ((1793 == num && num2 == 0) || (num == 28928 && num2 == 1)));
			if (num == 28928 && num2 == 1)
			{
				this.isShilohSP1 = true;
			}
			byte length = this.ReadByte();
			sqlLoginAck.programName = this.ReadString((int)length);
			sqlLoginAck.majorVersion = this.ReadByte();
			sqlLoginAck.minorVersion = this.ReadByte();
			sqlLoginAck.buildNum = (short)(((int)this.ReadByte() << 8) + (int)this.ReadByte());
			this.state = TdsParserState.OpenLoggedIn;
			if (this.fSSPIInit)
			{
				this.TermSSPISession();
			}
			return sqlLoginAck;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000EE00 File Offset: 0x0000DE00
		internal SqlError ProcessError(byte token)
		{
			int infoNumber = this.ReadInt();
			byte errorState = this.ReadByte();
			byte errorClass = this.ReadByte();
			int num = (int)this.ReadUnsignedShort();
			string errorMessage = this.ReadString(num);
			num = (int)this.ReadByte();
			string text;
			if (num == 0)
			{
				text = this.server;
			}
			else
			{
				text = this.ReadString(num);
			}
			num = (int)this.ReadByte();
			string procedure = this.ReadString(num);
			int lineNumber = (int)this.ReadUnsignedShort();
			return new SqlError(infoNumber, errorState, errorClass, text, errorMessage, procedure, lineNumber);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000EE78 File Offset: 0x0000DE78
		internal SqlReturnValue ProcessReturnValue(int length)
		{
			SqlReturnValue sqlReturnValue = new SqlReturnValue();
			sqlReturnValue.length = length;
			byte b = this.ReadByte();
			if (b > 0)
			{
				sqlReturnValue.parameter = this.ReadString((int)b);
			}
			this.ReadByte();
			ushort userType = this.ReadUnsignedShort();
			this.ReadUnsignedShort();
			sqlReturnValue.isNullable = true;
			int num = (int)this.ReadByte();
			int tokenLength = this.GetTokenLength((byte)num);
			sqlReturnValue.type = MetaType.GetSqlDataType(num, (int)userType, tokenLength);
			MetaType metaType = MetaType.GetMetaType(sqlReturnValue.type);
			sqlReturnValue.tdsType = metaType.NullableType;
            if (sqlReturnValue.type == (SqlDbType)5)
			{
				sqlReturnValue.precision = this.ReadByte();
				sqlReturnValue.scale = this.ReadByte();
			}
			if (this.isShiloh && MetaType.IsCharType(sqlReturnValue.type))
			{
				sqlReturnValue.collation = this.ProcessCollation();
				int codePage = this.GetCodePage(sqlReturnValue.collation);
				if (codePage == this.defaultCodePage)
				{
					sqlReturnValue.codePage = this.defaultCodePage;
					sqlReturnValue.encoding = this.defaultEncoding;
				}
				else
				{
					sqlReturnValue.codePage = codePage;
					sqlReturnValue.encoding = Encoding.GetEncoding(sqlReturnValue.codePage);
				}
			}
			_SqlMetaData sqlMetaData = new _SqlMetaData(sqlReturnValue, metaType);
			bool flag = false;
			int length2 = this.ProcessColumnHeader(sqlMetaData, ref flag);
			sqlReturnValue.value = (flag ? this.GetNullSqlValue(sqlMetaData) : this.ReadSqlValue(sqlMetaData, length2));
			return sqlReturnValue;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000EFC8 File Offset: 0x0000DFC8
		internal SqlCollation ProcessCollation()
		{
			return new SqlCollation
			{
				info = this.ReadUnsignedInt(),
				sortId = this.ReadByte()
			};
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000EFF4 File Offset: 0x0000DFF4
		internal int DefaultLCID
		{
			get
			{
				return this.defaultLCID;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000EFFC File Offset: 0x0000DFFC
		internal int GetCodePage(SqlCollation collation)
		{
			int result;
			if (collation.sortId != 0)
			{
				result = (int)TdsEnums.CODE_PAGE_FROM_SORT_ID[(int)collation.sortId];
			}
			else
			{
				int num = (int)(collation.info & 1048575U);
				CultureInfo cultureInfo = null;
				bool flag = false;
				try
				{
					cultureInfo = new CultureInfo(num);
					flag = true;
				}
				catch (ArgumentException)
				{
				}
				if (!flag)
				{
					int num2 = num;
					if (num2 <= 67588)
					{
						if (num2 != 66564)
						{
							switch (num2)
							{
							case 66577:
							case 66578:
								break;
							default:
								if (num2 != 67588)
								{
									goto IL_A5;
								}
								break;
							}
						}
					}
					else if (num2 != 68612 && num2 != 69636 && num2 != 70660)
					{
						goto IL_A5;
					}
					num &= 16383;
					try
					{
						cultureInfo = new CultureInfo(num);
						flag = true;
					}
					catch (ArgumentException)
					{
					}
					IL_A5:
					if (!flag)
					{
						this.CleanWire();
						this.exception = new SqlException();
						this.exception.Errors.Add(new SqlError(0, 0, 10, this.server, SQLMessage.CultureIdError(), "", 0));
						this.pendingData = false;
						this.ThrowExceptionAndWarning();
					}
				}
				TextInfo textInfo = cultureInfo.TextInfo;
				result = textInfo.ANSICodePage;
			}
			return result;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000F128 File Offset: 0x0000E128
		internal _SqlMetaData[] ProcessMetaData(int cColumns)
		{
			_SqlMetaData[] array = new _SqlMetaData[cColumns];
			for (int i = 0; i < cColumns; i++)
			{
				_SqlMetaData sqlMetaData = new _SqlMetaData();
				ushort userType = this.ReadUnsignedShort();
				sqlMetaData.ordinal = i;
				byte b = this.ReadByte();
				sqlMetaData.updatability = (byte)((b & 11) >> 2);
				sqlMetaData.isNullable = (1 == (b & 1));
				sqlMetaData.isIdentity = (16 == (b & 16));
				this.ReadByte();
				byte b2 = this.ReadByte();
				sqlMetaData.length = this.GetTokenLength(b2);
				sqlMetaData.type = MetaType.GetSqlDataType((int)b2, (int)userType, sqlMetaData.length);
				sqlMetaData.metaType = MetaType.GetMetaType(sqlMetaData.type);
				sqlMetaData.tdsType = (sqlMetaData.isNullable ? sqlMetaData.metaType.NullableType : sqlMetaData.metaType.TDSType);
                if (sqlMetaData.type == (SqlDbType)5)
				{
					sqlMetaData.precision = this.ReadByte();
					sqlMetaData.scale = this.ReadByte();
				}
				if (this.isShiloh && MetaType.IsCharType(sqlMetaData.type))
				{
					sqlMetaData.collation = this.ProcessCollation();
					int codePage = this.GetCodePage(sqlMetaData.collation);
					if (codePage == this.defaultCodePage)
					{
						sqlMetaData.codePage = this.defaultCodePage;
						sqlMetaData.encoding = this.defaultEncoding;
					}
					else
					{
						sqlMetaData.codePage = codePage;
						sqlMetaData.encoding = Encoding.GetEncoding(sqlMetaData.codePage);
					}
				}
				int length;
				if (sqlMetaData.metaType.IsLong)
				{
					length = (int)this.ReadUnsignedShort();
					string unparsedTableName = this.ReadString(length);
					this.ParseTableName(sqlMetaData, unparsedTableName);
				}
				length = (int)this.ReadByte();
				sqlMetaData.column = this.ReadString(length);
				array[i] = sqlMetaData;
			}
			return array;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000F2CC File Offset: 0x0000E2CC
		internal string[][] ProcessTableNameShilohSP1(int length)
		{
			int num = 0;
			string[][] array = new string[1][];
			while (length > 0)
			{
				int num2 = (int)this.ReadByte();
				length--;
				string[] array2 = new string[num2];
				for (int i = 0; i < num2; i++)
				{
					int num3 = (int)this.ReadUnsignedShort();
					length -= 2;
					if (num3 != 0)
					{
						array2[i] = this.ReadString(num3);
					}
					else
					{
						array2[i] = "";
					}
					length -= num3 * 2;
				}
				if (num == 0)
				{
					array[num] = array2;
				}
				else
				{
					string[][] array3 = new string[array.Length + 1][];
					Array.Copy(array, 0, array3, 0, array.Length);
					array3[array.Length] = array2;
					array = array3;
				}
				num++;
			}
			return array;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000F374 File Offset: 0x0000E374
		internal string[] ProcessTableName(int length)
		{
			int num = 0;
			string[] array = new string[1];
			while (length > 0)
			{
				int num2 = (int)this.ReadUnsignedShort();
				length -= 2;
				string text = this.ReadString(num2);
				if (num == 0)
				{
					array[num] = text;
				}
				else
				{
					string[] array2 = new string[array.Length + 1];
					Array.Copy(array, 0, array2, 0, array.Length);
					array2[array.Length] = text;
					array = array2;
				}
				num++;
				length -= num2 * 2;
			}
			return array;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000F3DC File Offset: 0x0000E3DC
		private _SqlMetaData[] ProcessColInfo(_SqlMetaData[] columns, SqlDataReader reader)
		{
			foreach (_SqlMetaData sqlMetaData in columns)
			{
				this.ReadByte();
				sqlMetaData.tableNum = this.ReadByte();
				byte b = this.ReadByte();
				sqlMetaData.isDifferentName = (32 == (b & 32));
				sqlMetaData.isExpression = (4 == (b & 4));
				sqlMetaData.isKey = (8 == (b & 8));
				sqlMetaData.isHidden = (16 == (b & 16));
				if (sqlMetaData.isDifferentName)
				{
					byte length = this.ReadByte();
					sqlMetaData.baseColumn = this.ReadString((int)length);
				}
				if (sqlMetaData.tableNum != 0)
				{
					if (this.isShilohSP1)
					{
						if (reader.TableNamesShilohSP1 != null)
						{
							string[] array = reader.TableNamesShilohSP1[(int)(sqlMetaData.tableNum - 1)];
							this.AssignParsedTableName(sqlMetaData, array, array.Length);
						}
					}
					else if (reader.TableNames != null)
					{
						this.ParseTableName(sqlMetaData, reader.TableNames[(int)(sqlMetaData.tableNum - 1)]);
					}
				}
				if (sqlMetaData.isExpression)
				{
					sqlMetaData.updatability = 0;
				}
			}
			return columns;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000F4D4 File Offset: 0x0000E4D4
		private void ParseTableName(_SqlMetaData col, string unparsedTableName)
		{
			if (unparsedTableName == null || unparsedTableName.Length == 0)
			{
				return;
			}
			string[] array = new string[4];
			int num = 0;
			int i;
			for (i = 0; i < 4; i++)
			{
				int num2 = unparsedTableName.IndexOf('.', num);
				if (-1 == num2)
				{
					array[i] = unparsedTableName.Substring(num);
					break;
				}
				array[i] = unparsedTableName.Substring(num, num2 - num);
				num = num2 + 1;
				if (num >= unparsedTableName.Length)
				{
					break;
				}
			}
			this.AssignParsedTableName(col, array, i + 1);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000F544 File Offset: 0x0000E544
		private void AssignParsedTableName(_SqlMetaData col, string[] parsedTableName, int length)
		{
			switch (length)
			{
			case 1:
				col.tableName = parsedTableName[0];
				return;
			case 2:
				col.schemaName = parsedTableName[0];
				col.tableName = parsedTableName[1];
				return;
			case 3:
				col.catalogName = parsedTableName[0];
				col.schemaName = parsedTableName[1];
				col.tableName = parsedTableName[2];
				return;
			case 4:
				col.serverName = parsedTableName[0];
				col.catalogName = parsedTableName[1];
				col.schemaName = parsedTableName[2];
				col.tableName = parsedTableName[3];
				return;
			default:
				return;
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000F5CC File Offset: 0x0000E5CC
		internal int ProcessColumnHeader(_SqlMetaData col, ref bool isNull)
		{
			int num;
			if (col.metaType.IsLong)
			{
				byte b = this.ReadByte();
				if (b != 0)
				{
					this.SkipBytes((long)((ulong)b));
					this.SkipBytes(8L);
					isNull = false;
					num = this.GetTokenLength(col.tdsType);
				}
				else
				{
					isNull = true;
					num = 0;
				}
			}
			else
			{
				num = this.GetTokenLength(col.tdsType);
				isNull = this.IsNull(col, num);
				num = (isNull ? 0 : num);
			}
			return num;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000F63C File Offset: 0x0000E63C
		internal void ProcessRow(_SqlMetaData[] columns, object[] buffer, int[] map, bool useSQLTypes)
		{
			bool flag = false;
			for (int i = 0; i < columns.Length; i++)
			{
				_SqlMetaData sqlMetaData = columns[i];
				int length = this.ProcessColumnHeader(sqlMetaData, ref flag);
				if (flag)
				{
					if (useSQLTypes)
					{
						buffer[map[i]] = this.GetNullSqlValue(sqlMetaData);
					}
					else
					{
						buffer[map[i]] = DBNull.Value;
					}
				}
				else
				{
					try
					{
						buffer[map[i]] = (useSQLTypes ? this.ReadSqlValue(sqlMetaData, length) : this.ReadValue(sqlMetaData, length));
					}
					catch (_ValueException ex)
					{
						buffer[map[i]] = ex.value;
						throw ex.exception;
					}
				}
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000F6CC File Offset: 0x0000E6CC
		internal object GetNullSqlValue(_SqlMetaData md)
		{
			object result = DBNull.Value;
			switch (md.type)
			{
			case (SqlDbType)0:
				result = SqlInt64.Null;
				break;
            case (SqlDbType)1:
            case (SqlDbType)7:
            case (SqlDbType)21:
				result = SqlBinary.Null;
				break;
            case (SqlDbType)2:
				result = SqlBoolean.Null;
				break;
            case (SqlDbType)3:
            case (SqlDbType)10:
            case (SqlDbType)11:
            case (SqlDbType)12:
            case (SqlDbType)18:
            case (SqlDbType)22:
				result = SqlString.Null;
				break;
            case (SqlDbType)4:
            case (SqlDbType)15:
				result = SqlDateTime.Null;
				break;
            case (SqlDbType)5:
				result = SqlDecimal.Null;
				break;
            case (SqlDbType)6:
				result = SqlDouble.Null;
				break;
            case (SqlDbType)8:
				result = SqlInt32.Null;
				break;
            case (SqlDbType)9:
            case (SqlDbType)17:
				result = SqlMoney.Null;
				break;
            case (SqlDbType)13:
				result = SqlSingle.Null;
				break;
            case (SqlDbType)14:
				result = SqlGuid.Null;
				break;
            case (SqlDbType)16:
				result = SqlInt16.Null;
				break;
            case (SqlDbType)20:
				result = SqlByte.Null;
				break;
			}
			return result;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000F804 File Offset: 0x0000E804
		private _SqlMetaData[] SkipAltMetaData(int cColumns)
		{
			_SqlMetaData[] array = new _SqlMetaData[cColumns];
			this.SkipBytes(2L);
			this.SkipBytes((long)(this.ReadByte() * 2));
			for (int i = 0; i < cColumns; i++)
			{
				_SqlMetaData sqlMetaData = new _SqlMetaData();
				this.SkipBytes(3L);
				ushort userType = this.ReadUnsignedShort();
				byte b = this.ReadByte();
				sqlMetaData.isNullable = (1 == (b & 1));
				this.SkipBytes(1L);
				byte b2 = this.ReadByte();
				sqlMetaData.length = this.GetTokenLength(b2);
				sqlMetaData.type = MetaType.GetSqlDataType((int)b2, (int)userType, sqlMetaData.length);
				sqlMetaData.metaType = MetaType.GetMetaType(sqlMetaData.type);
				sqlMetaData.tdsType = (sqlMetaData.isNullable ? sqlMetaData.metaType.NullableType : sqlMetaData.metaType.TDSType);
                if (sqlMetaData.type == (SqlDbType)5)
				{
					this.SkipBytes(2L);
				}
				if (this.isShiloh && MetaType.IsCharType(sqlMetaData.type))
				{
					this.SkipBytes(5L);
				}
				int num;
				if (sqlMetaData.metaType.IsLong)
				{
					num = (int)this.ReadUnsignedShort();
					this.SkipBytes((long)num);
				}
				num = (int)this.ReadByte();
				this.SkipBytes((long)((long)num << 1));
				array[i] = sqlMetaData;
			}
			return array;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000F938 File Offset: 0x0000E938
		internal void SkipRow(_SqlMetaData[] columns)
		{
			this.SkipRow(columns, 0);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000F944 File Offset: 0x0000E944
		internal void SkipRow(_SqlMetaData[] columns, int startCol)
		{
			int i = startCol;
			while (i < columns.Length)
			{
				_SqlMetaData sqlMetaData = columns[i];
				if (!sqlMetaData.metaType.IsLong)
				{
					goto IL_29;
				}
				byte b = this.ReadByte();
				if (b != 0)
				{
					this.SkipBytes((long)(b + 8));
					goto IL_29;
				}
				IL_30:
				i++;
				continue;
				IL_29:
				this.SkipValue(sqlMetaData);
				goto IL_30;
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000F98C File Offset: 0x0000E98C
		internal void SkipValue(_SqlMetaData md)
		{
			int tokenLength = this.GetTokenLength(md.tdsType);
			if (!this.IsNull(md, tokenLength))
			{
				this.SkipBytes((long)tokenLength);
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000F9B8 File Offset: 0x0000E9B8
		private bool IsNull(_SqlMetaData md, int length)
		{
			return 65535 == length || (length == 0 && !MetaType.IsCharType(md.type) && !MetaType.IsBinType(md.type));
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000F9E4 File Offset: 0x0000E9E4
		internal object ReadValue(_SqlMetaData md, int length)
		{
			byte tdsType = md.tdsType;
			object result = null;
			byte b = tdsType;
			if (b <= 127)
			{
				if (b <= 111)
				{
					switch (b)
					{
					case 34:
					case 36:
					case 37:
					case 45:
						goto IL_19F;
					case 35:
					case 39:
					case 47:
						goto IL_276;
					case 38:
						if (length == 1)
						{
							return this.ReadByte();
						}
						if (length == 2)
						{
							return this.ReadShort();
						}
						if (length == 4)
						{
							return this.ReadInt();
						}
						return this.ReadLong();
					case 40:
					case 41:
					case 42:
					case 43:
					case 44:
					case 46:
					case 49:
					case 51:
					case 53:
					case 54:
					case 55:
					case 57:
						return result;
					case 48:
						return this.ReadByte();
					case 50:
						break;
					case 52:
						return this.ReadShort();
					case 56:
						return this.ReadInt();
					case 58:
					case 61:
						goto IL_2CD;
					case 59:
						return this.ReadFloat();
					case 60:
						goto IL_2DC;
					case 62:
						return this.ReadDouble();
					default:
						switch (b)
						{
						case 98:
							return this.ReadSqlVariant(length, false);
						case 99:
							goto IL_286;
						case 100:
						case 101:
						case 102:
						case 103:
						case 105:
						case 107:
							return result;
						case 104:
							break;
						case 106:
						case 108:
						{
							SqlDecimal sqlDecimal = this.ReadSqlDecimal(length, md.precision, md.scale);
							try
							{
								return sqlDecimal.Value;
							}
							catch (Exception ex)
							{
								throw new _ValueException(ex, sqlDecimal.ToString());
							}
							goto IL_2CD;
						}
						case 109:
							if (length == 4)
							{
								return this.ReadFloat();
							}
							return this.ReadDouble();
						case 110:
							goto IL_2DC;
						case 111:
							goto IL_2CD;
						default:
							return result;
						}
						break;
					}
					byte b2 = this.ReadByte();
					return b2 != 0;
					IL_2CD:
					return this.ReadDateTime(length);
				}
				if (b != 122)
				{
					if (b != 127)
					{
						return result;
					}
					return this.ReadLong();
				}
				IL_2DC:
				return this.ReadCurrency(length);
			}
			if (b <= 175)
			{
				switch (b)
				{
				case 165:
					break;
				case 166:
					return result;
				case 167:
					goto IL_276;
				default:
					switch (b)
					{
					case 173:
						break;
					case 174:
						return result;
					case 175:
						goto IL_276;
					default:
						return result;
					}
					break;
				}
			}
			else
			{
				if (b != 231 && b != 239)
				{
					return result;
				}
				goto IL_286;
			}
			IL_19F:
			byte[] array = new byte[length];
			this.ReadByteArray(array, 0, length);
			if (tdsType == 36)
			{
				return new Guid(array);
			}
			return array;
			IL_276:
			return this.ReadEncodingChar(length, md.encoding);
			IL_286:
			result = this.ReadString(length >> 1);
			return result;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000FCF8 File Offset: 0x0000ECF8
		internal object ReadSqlValue(_SqlMetaData md, int length)
		{
			byte tdsType = md.tdsType;
			object result = null;
			byte b = tdsType;
			if (b <= 127)
			{
				if (b <= 111)
				{
					switch (b)
					{
					case 34:
					case 37:
					case 45:
						goto IL_1B9;
					case 35:
					case 39:
					case 47:
						goto IL_2EE;
					case 36:
					{
						byte[] array = new byte[length];
						this.ReadByteArray(array, 0, length);
						return new SqlGuid(array);
					}
					case 38:
                    if (md.type == (SqlDbType)20)
						{
							return new SqlByte(this.ReadByte());
						}
                    if (md.type == (SqlDbType)16)
						{
							return new SqlInt16(this.ReadShort());
						}
                    if (md.type == (SqlDbType)8)
						{
							return new SqlInt32(this.ReadInt());
						}
						return new SqlInt64(this.ReadLong());
					case 40:
					case 41:
					case 42:
					case 43:
					case 44:
					case 46:
					case 49:
					case 51:
					case 53:
					case 54:
					case 55:
					case 57:
						return result;
					case 48:
						return new SqlByte(this.ReadByte());
					case 50:
						break;
					case 52:
						return new SqlInt16(this.ReadShort());
					case 56:
						return new SqlInt32(this.ReadInt());
					case 58:
					case 61:
						goto IL_339;
					case 59:
						return new SqlSingle(this.ReadFloat());
					case 60:
						goto IL_348;
					case 62:
						return new SqlDouble(this.ReadDouble());
					default:
						switch (b)
						{
						case 98:
							return this.ReadSqlVariant(length, true);
						case 99:
							goto IL_308;
						case 100:
						case 101:
						case 102:
						case 103:
						case 105:
						case 107:
							return result;
						case 104:
							break;
						case 106:
						case 108:
							return this.ReadSqlDecimal(length, md.precision, md.scale);
						case 109:
                            if (md.type == (SqlDbType)13)
							{
								return new SqlSingle(this.ReadFloat());
							}
							return new SqlDouble(this.ReadDouble());
						case 110:
							goto IL_348;
						case 111:
							goto IL_339;
						default:
							return result;
						}
						break;
					}
					if (1 == this.ReadByte())
					{
						return SqlBoolean.True;
					}
					return SqlBoolean.False;
					IL_339:
					return this.ReadSqlDateTime(length);
				}
				if (b != 122)
				{
					if (b != 127)
					{
						return result;
					}
					return new SqlInt64(this.ReadLong());
				}
				IL_348:
				return this.ReadSqlMoney(length);
			}
			if (b <= 175)
			{
				switch (b)
				{
				case 165:
					break;
				case 166:
					return result;
				case 167:
					goto IL_2EE;
				default:
					switch (b)
					{
					case 173:
						break;
					case 174:
						return result;
					case 175:
						goto IL_2EE;
					default:
						return result;
					}
					break;
				}
			}
			else
			{
				if (b != 231 && b != 239)
				{
					return result;
				}
				goto IL_308;
			}
			IL_1B9:
			byte[] array2 = new byte[length];
			this.ReadByteArray(array2, 0, length);
			return new SqlBinary(array2);
			IL_2EE:
			return new SqlString(this.ReadEncodingChar(length, md.encoding));
			IL_308:
			result = new SqlString(this.ReadString(length >> 1));
			return result;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010068 File Offset: 0x0000F068
		public object ReadSqlVariant(int lenTotal, bool readAsSQLValue)
		{
			object obj = null;
			byte b = this.ReadByte();
			byte b2 = this.ReadByte();
			SqlDbType sqlDataType = MetaType.GetSqlDataType((int)b, 0, 0);
			MetaType metaType = MetaType.GetMetaType(sqlDataType);
			byte propBytes = metaType.PropBytes;
			int num = (int)(2 + b2);
			int num2 = lenTotal - num;
			byte b3 = b;
			if (b3 <= 122)
			{
				if (b3 <= 62)
				{
					if (b3 != 36)
					{
						switch (b3)
						{
						case 48:
							obj = this.ReadByte();
							if (readAsSQLValue)
							{
								obj = new SqlByte((byte)obj);
							}
							break;
						case 50:
						{
							byte b4 = this.ReadByte();
							if (readAsSQLValue)
							{
								obj = ((b4 == 0) ? SqlBoolean.False : SqlBoolean.True);
							}
							else
							{
								obj = (b4 != 0);
							}
							break;
						}
						case 52:
							obj = this.ReadShort();
							if (readAsSQLValue)
							{
								obj = new SqlInt16((short)obj);
							}
							break;
						case 56:
							obj = this.ReadInt();
							if (readAsSQLValue)
							{
								obj = new SqlInt32((int)obj);
							}
							break;
						case 58:
							if (readAsSQLValue)
							{
								obj = this.ReadSqlDateTime(4);
							}
							else
							{
								obj = this.ReadDateTime(4);
							}
							break;
						case 59:
							obj = this.ReadFloat();
							if (readAsSQLValue)
							{
								obj = new SqlSingle((float)obj);
							}
							break;
						case 60:
							if (readAsSQLValue)
							{
								obj = this.ReadSqlMoney(8);
							}
							else
							{
								obj = this.ReadCurrency(8);
							}
							break;
						case 61:
							if (readAsSQLValue)
							{
								obj = this.ReadSqlDateTime(8);
							}
							else
							{
								obj = this.ReadDateTime(8);
							}
							break;
						case 62:
							obj = this.ReadDouble();
							if (readAsSQLValue)
							{
								obj = new SqlDouble((double)obj);
							}
							break;
						}
					}
					else
					{
						byte[] array = new byte[16];
						this.ReadByteArray(array, 0, 16);
						if (readAsSQLValue)
						{
							obj = new SqlGuid(array);
						}
						else
						{
							obj = new Guid(array);
						}
					}
				}
				else
				{
					switch (b3)
					{
					case 106:
					case 108:
					{
						byte precision = this.ReadByte();
						byte scale = this.ReadByte();
						if (b2 > propBytes)
						{
							this.SkipBytes((long)(b2 - propBytes));
						}
						SqlDecimal sqlDecimal = this.ReadSqlDecimal(17, precision, scale);
						if (readAsSQLValue)
						{
							obj = sqlDecimal;
						}
						else
						{
							obj = sqlDecimal.Value;
						}
						break;
					}
					case 107:
						break;
					default:
						if (b3 == 122)
						{
							if (readAsSQLValue)
							{
								obj = this.ReadSqlMoney(4);
							}
							else
							{
								obj = this.ReadCurrency(4);
							}
						}
						break;
					}
				}
			}
			else
			{
				if (b3 <= 167)
				{
					if (b3 != 127)
					{
						switch (b3)
						{
						case 165:
							break;
						case 166:
							return obj;
						case 167:
							goto IL_2D9;
						default:
							return obj;
						}
					}
					else
					{
						obj = this.ReadLong();
						if (readAsSQLValue)
						{
							return new SqlInt64((long)obj);
						}
						return obj;
					}
				}
				else
				{
					switch (b3)
					{
					case 173:
						break;
					case 174:
						return obj;
					case 175:
						goto IL_2D9;
					default:
						if (b3 != 231 && b3 != 239)
						{
							return obj;
						}
						goto IL_2D9;
					}
				}
				this.ReadUnsignedShort();
				if (b2 > propBytes)
				{
					this.SkipBytes((long)(b2 - propBytes));
				}
				if (65535 != num2)
				{
					byte[] array2 = new byte[num2];
					this.ReadByteArray(array2, 0, num2);
					obj = array2;
				}
				if (readAsSQLValue)
				{
					return new SqlBinary((byte[])obj);
				}
				return obj;
				IL_2D9:
				SqlCollation collation = this.ProcessCollation();
				this.ReadUnsignedShort();
				if (b2 > propBytes)
				{
					this.SkipBytes((long)(b2 - propBytes));
				}
				if (65535 != num2)
				{
					if (b == 175 || b == 167)
					{
						int codePage = this.GetCodePage(collation);
						if (codePage == this.defaultCodePage)
						{
							obj = this.ReadEncodingChar(num2, this.defaultEncoding);
						}
						else
						{
							Encoding encoding = Encoding.GetEncoding(codePage);
							obj = this.ReadEncodingChar(num2, encoding);
						}
					}
					else
					{
						obj = this.ReadString(num2 >> 1);
					}
				}
				if (readAsSQLValue)
				{
					obj = new SqlString((string)obj);
				}
			}
			return obj;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000104BC File Offset: 0x0000F4BC
		private void WriteSqlVariantValue(object value, int length, int offset)
		{
			if (value == null || Convert.IsDBNull(value))
			{
				this.WriteInt(0);
				this.WriteInt(0);
				return;
			}
			MetaType metaType = MetaType.GetMetaType(value);
			if (MetaType.IsAnsiType(metaType.SqlDbType))
			{
				length = this.GetEncodingCharLength((string)value, length, 0, this.defaultEncoding);
			}
			this.WriteInt((int)(2 + metaType.PropBytes) + length);
			this.WriteInt((int)(2 + metaType.PropBytes) + length);
			this.WriteByte(metaType.TDSType);
			this.WriteByte(metaType.PropBytes);
			byte tdstype = metaType.TDSType;
			if (tdstype <= 108)
			{
				if (tdstype == 36)
				{
					byte[] b = ((Guid)value).ToByteArray();
					this.WriteByteArray(b, length, 0);
					return;
				}
				switch (tdstype)
				{
				case 48:
					this.WriteByte((byte)value);
					return;
				case 49:
				case 51:
				case 53:
				case 54:
				case 55:
				case 57:
				case 58:
					break;
				case 50:
					if ((bool)value)
					{
						this.WriteByte(1);
						return;
					}
					this.WriteByte(0);
					return;
				case 52:
					this.WriteShort((int)((short)value));
					return;
				case 56:
					this.WriteInt((int)value);
					return;
				case 59:
					this.WriteFloat((float)value);
					return;
				case 60:
					this.WriteCurrency((decimal)value, 8);
					return;
				case 61:
				{
					TdsDateTime tdsDateTime = MetaDateTime.FromDateTime((DateTime)value, 8);
					this.WriteInt(tdsDateTime.days);
					this.WriteInt(tdsDateTime.time);
					return;
				}
				case 62:
					this.WriteDouble((double)value);
					return;
				default:
					if (tdstype != 108)
					{
						return;
					}
					this.WriteByte(metaType.Precision);
					this.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16));
					this.WriteDecimal((decimal)value);
					break;
				}
			}
			else
			{
				if (tdstype == 127)
				{
					this.WriteLong((long)value);
					return;
				}
				switch (tdstype)
				{
				case 165:
				{
					byte[] b2 = (byte[])value;
					this.WriteShort(length);
					this.WriteByteArray(b2, length, offset);
					return;
				}
				case 166:
					break;
				case 167:
				{
					string s = (string)value;
					this.WriteUnsignedInt(this.defaultCollation.info);
					this.WriteByte(this.defaultCollation.sortId);
					this.WriteShort(length);
					this.WriteEncodingChar(s, this.defaultEncoding);
					return;
				}
				default:
				{
					if (tdstype != 231)
					{
						return;
					}
					string s2 = (string)value;
					this.WriteUnsignedInt(this.defaultCollation.info);
					this.WriteByte(this.defaultCollation.sortId);
					this.WriteShort(length);
					length >>= 1;
					this.WriteString(s2, length, offset);
					return;
				}
				}
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001075C File Offset: 0x0000F75C
		private void WriteSqlMoney(SqlMoney value, int length)
		{
			int[] bits = decimal.GetBits(value.Value);
			bool flag = 0 != (bits[3] & int.MinValue);
			long num = (long)((ulong)bits[1] << 32 | (ulong)bits[0]);
			if (flag)
			{
				num = -num;
			}
			if (length != 4)
			{
				this.WriteInt((int)(num >> 32));
				this.WriteInt((int)num);
				return;
			}
			decimal value2 = value.Value;
			if (value2 < TdsEnums.SQL_SMALL_MONEY_MIN || value2 > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value2.ToString());
			}
			this.WriteInt((int)num);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000107E8 File Offset: 0x0000F7E8
		private void WriteCurrency(decimal value, int length)
		{
			SqlMoney sqlMoney = new SqlMoney(value);
			int[] bits = decimal.GetBits(sqlMoney.Value);
			bool flag = 0 != (bits[3] & int.MinValue);
			long num = Convert.ToInt64((ulong)bits[1] << 32 | (ulong)bits[0]);
			if (flag)
			{
				num = -num;
			}
			if (length != 4)
			{
				this.WriteInt((int)(num >> 32));
				this.WriteInt((int)num);
				return;
			}
			if (value < TdsEnums.SQL_SMALL_MONEY_MIN || value > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value.ToString());
			}
			this.WriteInt((int)num);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00010874 File Offset: 0x0000F874
		private DateTime ReadDateTime(int length)
		{
			if (length == 4)
			{
				return MetaDateTime.ToDateTime((int)this.ReadUnsignedShort(), (int)this.ReadUnsignedShort(), 4);
			}
			return MetaDateTime.ToDateTime(this.ReadInt(), (int)this.ReadUnsignedInt(), 8);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000108A0 File Offset: 0x0000F8A0
		private SqlDecimal ReadSqlDecimal(int length, byte precision, byte scale)
		{
			bool flag = 1 == this.ReadByte();
			length--;
			int[] array = this.ReadDecimalBits(length);
			return new SqlDecimal(precision, scale, flag, array);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000108CD File Offset: 0x0000F8CD
		private SqlDateTime ReadSqlDateTime(int length)
		{
			if (length == 4)
			{
				return new SqlDateTime((int)this.ReadUnsignedShort(), (int)this.ReadUnsignedShort() * SqlDateTime.SQLTicksPerMinute);
			}
			return new SqlDateTime(this.ReadInt(), (int)this.ReadUnsignedInt());
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000108FC File Offset: 0x0000F8FC
		private decimal ReadCurrency(int length)
		{
			bool flag = false;
			decimal result;
			if (length == 4)
			{
				int num = this.ReadInt();
				if (num < 0)
				{
					num = -num;
					flag = true;
				}
                result = new decimal(num, 0, 0, flag, 4);
			}
			else
			{
				int num2 = this.ReadInt();
				uint num3 = this.ReadUnsignedInt();
				long num4 = (Convert.ToInt64(num2) << 32) + Convert.ToInt64(num3);
				if (num2 < 0)
				{
					num4 = -num4;
					flag = true;
				}
				result = new decimal((int)(num4 & unchecked((long)((ulong)-1))), (int)(num4 >> 32 & unchecked((long)((ulong)-1))), 0, flag, 4);
			}
			return result;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00010974 File Offset: 0x0000F974
		private SqlMoney ReadSqlMoney(int length)
		{
			return new SqlMoney(this.ReadCurrency(length));
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00010984 File Offset: 0x0000F984
		private int[] ReadDecimalBits(int length)
		{
			int[] array = this.decimalBits;
			if (array == null)
			{
				array = new int[4];
			}
			else
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0;
				}
			}
			int num = length >> 2;
			for (int i = 0; i < num; i++)
			{
				array[i] = this.ReadInt();
			}
			return array;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000109D0 File Offset: 0x0000F9D0
		private SqlDecimal AdjustSqlDecimalScale(SqlDecimal d, int newScale)
		{
			if ((int)d.Scale != newScale)
			{
				return SqlDecimal.AdjustScale(d, newScale - (int)d.Scale, false);
			}
			return d;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000109F0 File Offset: 0x0000F9F0
		private decimal AdjustDecimalScale(decimal value, int newScale)
		{
			int num = (decimal.GetBits(value)[3] & 16711680) >> 16;
			if (newScale != num)
			{
				SqlDecimal sqlDecimal = new SqlDecimal(value);
				sqlDecimal = SqlDecimal.AdjustScale(sqlDecimal, newScale - num, false);
				return sqlDecimal.Value;
			}
			return value;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00010A30 File Offset: 0x0000FA30
		private void WriteSqlDecimal(SqlDecimal d)
		{
			if (d.IsPositive)
			{
				this.WriteByte(1);
			}
			else
			{
				this.WriteByte(0);
			}
			int[] data = d.Data;
			this.WriteInt(data[0]);
			this.WriteInt(data[1]);
			this.WriteInt(data[2]);
			this.WriteInt(data[3]);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00010A84 File Offset: 0x0000FA84
		private void WriteDecimal(decimal value)
		{
			this.decimalBits = decimal.GetBits(value);
			if (unchecked((ulong)-2147483648) == (ulong)((long)this.decimalBits[3] & unchecked((long)((ulong)-2147483648))))
			{
				this.WriteByte(0);
			}
			else
			{
				this.WriteByte(1);
			}
			this.WriteInt(this.decimalBits[0]);
			this.WriteInt(this.decimalBits[1]);
			this.WriteInt(this.decimalBits[2]);
			this.WriteInt(0);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00010AF8 File Offset: 0x0000FAF8
		public string ReadString(int length)
		{
			int num = length << 1;
			int num2 = 0;
			byte[] array;
			if (this.inBytesUsed + num > this.inBytesRead || this.inBytesPacket < num)
			{
				if (this.byteBuffer == null || this.byteBuffer.Length < num)
				{
					this.byteBuffer = new byte[num];
				}
				this.ReadByteArray(this.byteBuffer, 0, num);
				array = this.byteBuffer;
			}
			else
			{
				array = this.inBuff;
				num2 = this.inBytesUsed;
				this.inBytesUsed += num;
				this.inBytesPacket -= num;
			}
			if (this.charBuffer == null || this.charBuffer.Length < length)
			{
				this.charBuffer = new char[length];
			}
			Buffer.BlockCopy(array, num2, this.charBuffer, 0, num);
			return new string(this.charBuffer, 0, length);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00010BBF File Offset: 0x0000FBBF
		private void WriteString(string s)
		{
			this.WriteString(s, s.Length, 0);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00010BD0 File Offset: 0x0000FBD0
		private void WriteString(string s, int length, int offset)
		{
			char[] array = new char[length];
			s.CopyTo(offset, array, 0, length);
			byte[] array2 = new byte[array.Length * 2];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			this.WriteByteArray(array2, array2.Length, 0);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00010C10 File Offset: 0x0000FC10
		public string ReadEncodingChar(int length, Encoding encoding)
		{
			int num = 0;
			if (encoding == null)
			{
				encoding = this.defaultEncoding;
			}
			byte[] array;
			if (this.inBytesUsed + length > this.inBytesRead || this.inBytesPacket < length)
			{
				if (this.byteBuffer == null || this.byteBuffer.Length < length)
				{
					this.byteBuffer = new byte[length];
				}
				this.ReadByteArray(this.byteBuffer, 0, length);
				array = this.byteBuffer;
			}
			else
			{
				array = this.inBuff;
				num = this.inBytesUsed;
				this.inBytesUsed += length;
				this.inBytesPacket -= length;
			}
			if (this.charBuffer == null || this.charBuffer.Length < length)
			{
				this.charBuffer = new char[length];
			}
			int chars = encoding.GetChars(array, num, length, this.charBuffer, 0);
			return new string(this.charBuffer, 0, chars);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00010CE0 File Offset: 0x0000FCE0
		private void WriteEncodingChar(string s, Encoding encoding)
		{
			this.WriteEncodingChar(s, s.Length, 0, encoding);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00010CF4 File Offset: 0x0000FCF4
		private void WriteEncodingChar(string s, int numChars, int offset, Encoding encoding)
		{
			if (encoding == null)
			{
				encoding = this.defaultEncoding;
			}
			char[] array = new char[numChars];
			s.CopyTo(offset, array, 0, numChars);
			byte[] bytes = encoding.GetBytes(array);
			this.WriteByteArray(bytes, bytes.Length, 0);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00010D34 File Offset: 0x0000FD34
		internal int GetEncodingCharLength(string value, int numChars, int charOffset, Encoding encoding)
		{
			if (value == null || value == string.Empty)
			{
				return 0;
			}
			if (encoding == null)
			{
				encoding = this.defaultEncoding;
			}
			char[] array = new char[numChars];
			value.CopyTo(charOffset, array, 0, numChars);
			byte[] bytes = encoding.GetBytes(array);
			return bytes.Length;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00010D7C File Offset: 0x0000FD7C
		public void Cancel()
		{
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00010D80 File Offset: 0x0000FD80
		public int GetTokenLength(byte token)
		{
			int result = 0;
			int num = (int)(token & 48);
			if (num <= 16)
			{
				if (num != 0)
				{
					if (num != 16)
					{
						return result;
					}
					return 0;
				}
			}
			else if (num != 32)
			{
				if (num == 48)
				{
					return 1 << ((token & 12) >> 2) & 255;
				}
				return result;
			}
			if ((token & 128) != 0)
			{
				result = (int)this.ReadUnsignedShort();
			}
			else if ((token & 12) == 0)
			{
				result = this.ReadInt();
			}
			else
			{
				result = (int)this.ReadByte();
			}
			return result;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00010DEF File Offset: 0x0000FDEF
		private void AttentionTimerCallback(object state)
		{
			if (this.attentionTimer != null)
			{
				this.attentionTimer.Dispose();
				this.attentionTimer = null;
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00010E0C File Offset: 0x0000FE0C
		internal void CleanWire()
		{
			while (this._status != 1)
			{
				int num = this.inBytesRead - this.inBytesUsed;
				if (this.inBytesPacket >= num)
				{
					this.inBytesPacket -= num;
					this.inBytesUsed = this.inBytesRead;
					this.ReadBuffer();
				}
				else
				{
					this.inBytesUsed += this.inBytesPacket;
					this.inBytesPacket = 0;
					this.ProcessHeader();
				}
			}
			this.inBytesUsed = (this.inBytesPacket = (this.inBytesRead = 0));
			this.pendingData = false;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00010EA0 File Offset: 0x0000FEA0
		private void ProcessAttention(int status, RunBehavior run)
		{
			if (32 == (status & 32))
			{
				this.attentionSent = (this.pendingData = false);
			}
			else
			{
				this.ProcessAttention();
			}
			if (RunBehavior.Clean != run)
			{
				this.ThrowAttentionError();
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00010ED8 File Offset: 0x0000FED8
		private void ProcessAttention()
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			this.timeRemaining = new TimeSpan(0, 0, 5);
			this.attentionTimer = new Timer(new TimerCallback(this.AttentionTimerCallback), null, 5000, 0);
			while (this.attentionTimer != null)
			{
				this.inBytesPacket = (this.inBytesUsed = (this.inBytesRead = 0));
				bool flag2 = DbNetLib.ConnectionCheckForData(this.pConObj, ref num, ref num2);
				if (flag2)
				{
					try
					{
						this.ReadBuffer();
						if (2 == this.inBuff[1])
						{
							flag = true;
							break;
						}
						if (this.inBuff[1] == 1)
						{
							ushort num3 = 0;
							if (this.inBuff[8] == 253 || this.inBuff[8] == 254)
							{
								num3 = (ushort)(((int)this.inBuff[10] << 8) + (int)this.inBuff[9]);
							}
							else if (this.inBuff[this.inBytesRead - 9] == 253 || this.inBuff[this.inBytesRead - 9] == 254)
							{
								num3 = (ushort)(((int)this.inBuff[this.inBytesRead - 7] << 8) + (int)this.inBuff[this.inBytesRead - 8]);
							}
							if (num3 == 32)
							{
								flag = true;
								break;
							}
						}
					}
					catch (Exception)
					{
						this.state = TdsParserState.Broken;
						break;
					}
				}
			}
			this.AttentionTimerCallback(null);
			this.attentionSent = (this.pendingData = false);
			if (!flag)
			{
				this.state = TdsParserState.Broken;
				return;
			}
			this.inBytesPacket = (this.inBytesUsed = (this.inBytesRead = 0));
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00011078 File Offset: 0x00010078
		internal void SendAttention()
		{
			if (!this.attentionSent)
			{
				this.attentionSent = true;
				this.msgType = 6;
				this.FlushBufferOOB();
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00011098 File Offset: 0x00010098
		private void FlushBufferOOB()
		{
			if (this.state == TdsParserState.Closed || this.state == TdsParserState.Broken)
			{
				return;
			}
			this.outBuff[0] = this.msgType;
			this.outBuff[1] = 1;
			this.outBuff[2] = 0;
			this.outBuff[3] = 8;
			this.outBuff[4] = 0;
			this.outBuff[5] = 0;
			this.outBuff[6] = 0;
			this.outBuff[7] = 0;
			int num = (int)DbNetLib.ConnectionWriteOOB(this.pConObj, this.outBuff, 8, ref this.errno);
			if (num != 8)
			{
				if (this.exception == null)
				{
					this.exception = new SqlException();
				}
				this.exception.Errors.Add(this.ProcessNetlibError(this.errno));
				this.ThrowExceptionAndWarning();
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011158 File Offset: 0x00010158
		internal void TdsLogin(SqlLogin rec)
		{
			this.inBuff = new byte[rec.packetSize];
			this.outBuff = new byte[rec.packetSize];
			this.timeout = rec.timeout;
			this.ValidateLengths(rec);
			byte[] array = this.EncryptPassword(rec.password);
			this.timeRemaining = new TimeSpan(0, 0, this.timeout);
			this.msgType = 16;
			int num = 86;
			string text = ".Net SqlClient Data Provider";
			num += (rec.hostName.Length + rec.applicationName.Length + rec.serverName.Length + text.Length + rec.language.Length + rec.database.Length) * 2;
			if (!rec.useSSPI)
			{
				num += array.Length + rec.userName.Length;
			}
			try
			{
				this.WriteInt(num);
				this.WriteInt(1895825409);
				this.WriteInt(rec.packetSize);
				this.WriteInt(100663296);
				this.WriteInt(0);
				this.WriteInt(0);
				this.WriteByte(224);
				if (rec.useSSPI)
				{
					this.WriteByte(131);
				}
				else
				{
					this.WriteByte(3);
				}
				this.WriteByte(0);
				this.WriteByte(0);
				this.WriteInt(0);
				this.WriteInt(0);
				num = 86;
				this.WriteShort(num);
				this.WriteShort(rec.hostName.Length);
				num += rec.hostName.Length * 2;
				if (!rec.useSSPI)
				{
					this.WriteShort(num);
					this.WriteShort(rec.userName.Length);
					num += rec.userName.Length * 2;
					this.WriteShort(num);
					this.WriteShort(array.Length / 2);
					num += array.Length;
				}
				else
				{
					this.WriteShort(0);
					this.WriteShort(0);
					this.WriteShort(0);
					this.WriteShort(0);
				}
				this.WriteShort(num);
				this.WriteShort(rec.applicationName.Length);
				num += rec.applicationName.Length * 2;
				this.WriteShort(num);
				this.WriteShort(rec.serverName.Length);
				num += rec.serverName.Length * 2;
				this.WriteShort(num);
				this.WriteShort(0);
				this.WriteShort(num);
				this.WriteShort(text.Length);
				num += text.Length * 2;
				this.WriteShort(num);
				this.WriteShort(rec.language.Length);
				num += rec.language.Length * 2;
				this.WriteShort(num);
				this.WriteShort(rec.database.Length);
				num += rec.database.Length * 2;
				if (TdsParser.s_nicAddress == null)
				{
					TdsParser.s_nicAddress = this.GetNIC();
				}
				this.WriteByteArray(TdsParser.s_nicAddress, TdsParser.s_nicAddress.Length, 0);
				byte[] array2 = null;
				int num2 = 0;
				if (rec.useSSPI && this.fSendSSPI)
				{
					if (!TdsParser.s_fSSPILoaded)
					{
						this.LoadSSPILibrary(ref TdsParser.MAXSSPILENGTH);
					}
					this.InitSSPISession();
					this.SetCredentialsHandle(rec);
					array2 = new byte[TdsParser.MAXSSPILENGTH];
					num2 = TdsParser.MAXSSPILENGTH;
					this.SSPIData(null, 0, array2, ref num2);
					this.WriteShort(num);
					this.WriteShort(num2);
					num += num2;
				}
				else
				{
					this.WriteShort(0);
					this.WriteShort(0);
				}
				this.WriteShort(num);
				this.WriteShort(0);
				this.WriteString(rec.hostName);
				if (!rec.useSSPI)
				{
					this.WriteString(rec.userName);
					this.WriteByteArray(array, array.Length, 0);
				}
				this.WriteString(rec.applicationName);
				this.WriteString(rec.serverName);
				this.WriteString(text);
				this.WriteString(rec.language);
				this.WriteString(rec.database);
				if (rec.useSSPI && this.fSendSSPI)
				{
					this.WriteByteArray(array2, num2, 0);
				}
				this.WriteString("");
			}
			catch (Exception ex)
			{
				this.ResetBuffer();
				throw ex;
			}
			this.FlushBuffer(1);
			this.pendingData = true;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0001156C File Offset: 0x0001056C
		private void ValidateLengths(SqlLogin rec)
		{
			int num = 128;
			Exception ex = null;
			if (rec.serverName.Length > num)
			{
				ex = SQL.InvalidOptionLength("data source", rec.serverName);
			}
			if (rec.userName.Length > num)
			{
				ex = SQL.InvalidOptionLength("user id", rec.userName);
			}
			if (rec.password.Length > num)
			{
				ex = SQL.InvalidOptionLength("password", rec.password);
			}
			if (rec.database.Length > num)
			{
				ex = SQL.InvalidOptionLength("initial catalog", rec.database);
			}
			if (rec.language.Length > num)
			{
				ex = SQL.InvalidOptionLength("current language", rec.language);
			}
			if (rec.hostName.Length > num)
			{
				ex = SQL.InvalidOptionLength("workstation id", rec.hostName);
			}
			if (rec.applicationName.Length > num)
			{
				ex = SQL.InvalidOptionLength("application name", rec.applicationName);
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00011660 File Offset: 0x00010660
		private byte[] GetNIC()
		{
			byte[] array = null;
			if (array == null)
			{
				array = new byte[6];
			}
			return array;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001167C File Offset: 0x0001067C
		private void SSPIData(byte[] receivedBuff, int receivedLength, byte[] sendBuff, ref int sendLength)
		{
			IntPtr intPtr = DbNetLib.AllocHGlobal(512);
			bool flag = false;
			try
			{
				Marshal.WriteByte(intPtr, 0);
				if (!DbNetLib.ConnectionGetSvrUser(this.pConObj, intPtr))
				{
					this.TermSSPISession();
					this.SSPIError(SQLMessage.SSPIGenerateError(), "ConnectionGetSvrUser");
				}
				if (receivedBuff == null && receivedLength > 0)
				{
					if (!DbNetLib.GenClientContext(this.pConObj, null, 0, sendBuff, ref sendLength, ref flag, intPtr))
					{
						this.TermSSPISession();
						this.SSPIError(SQLMessage.SSPIGenerateError(), "GenClientContext");
					}
				}
				else if (!DbNetLib.GenClientContext(this.pConObj, receivedBuff, receivedLength, sendBuff, ref sendLength, ref flag, intPtr))
				{
					this.TermSSPISession();
					this.SSPIError(SQLMessage.SSPIGenerateError(), "GenClientContext");
				}
			}
			finally
			{
				DbNetLib.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0001173C File Offset: 0x0001073C
		private void ProcessSSPI(int receivedLength)
		{
			byte[] array = new byte[receivedLength];
			this.ReadByteArray(array, 0, receivedLength);
			byte[] array2 = new byte[TdsParser.MAXSSPILENGTH];
			int maxsspilength = TdsParser.MAXSSPILENGTH;
			this.SSPIData(array, receivedLength, array2, ref maxsspilength);
			this.WriteByteArray(array2, maxsspilength, 0);
			this.msgType = 17;
			this.FlushBuffer(1);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011790 File Offset: 0x00010790
		private void SSPIError(string error, string procedure)
		{
			if (this.exception == null)
			{
				this.exception = new SqlException();
			}
			this.exception.Errors.Add(new SqlError(0, 0, 10, this.server, error, procedure, 0));
			this.ThrowExceptionAndWarning();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000117D8 File Offset: 0x000107D8
		public void LoadSSPILibrary(ref int maxLength)
		{
			lock (typeof(TdsParser))
			{
				if (!TdsParser.s_fSSPILoaded)
				{
					if (!DbNetLib.InitSSPIPackage(ref maxLength))
					{
						this.SSPIError(SQLMessage.SSPIInitializeError(), "InitSSPIPackage");
					}
					TdsParser.s_fSSPILoaded = true;
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011834 File Offset: 0x00010834
		public void InitSSPISession()
		{
			if (!DbNetLib.InitSession(this.pConObj))
			{
				this.SSPIError(SQLMessage.SSPIInitializeError(), "InitSession");
			}
			this.fSSPIInit = true;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0001185C File Offset: 0x0001085C
		public void SetCredentialsHandle(SqlLogin login)
		{
			string domain = "";
			string user = login.userName;
			string password = login.password;
			int num;
			if ((num = login.userName.IndexOf('\\')) > 0)
			{
				domain = login.userName.Substring(0, num);
				user = login.userName.Substring(num + 1);
			}
			if (!DbNetLib.SetCredentialsHandle(this.pConObj, domain, user, password))
			{
				this.SSPIError(SQLMessage.SSPIInitializeError(), "SetCredentialsHandle");
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000118CE File Offset: 0x000108CE
		private void TermSSPISession()
		{
			DbNetLib.TermSession(this.pConObj);
			this.fSSPIInit = false;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000118E4 File Offset: 0x000108E4
		internal void TdsExecuteSQLBatch(string text, int timeout)
		{
			this.timeRemaining = new TimeSpan(0, 0, timeout);
			this.msgType = 1;
			this.timeout = timeout;
			try
			{
				this.WriteString(text);
			}
			catch (Exception ex)
			{
				this.ResetBuffer();
				throw ex;
			}
			this.FlushBuffer(1);
			this.pendingData = true;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00011940 File Offset: 0x00010940
		internal void TdsExecuteRPC(_SqlRPC rec, int timeout, bool inSchema)
		{
			this.timeRemaining = new TimeSpan(0, 0, timeout);
			this.timeout = timeout;
			this.msgType = 3;
			this.WriteShort(rec.rpcName.Length);
			this.WriteString(rec.rpcName);
			this.WriteShort((int)((short)rec.options));
			SqlParameter[] parameters = rec.parameters;
			try
			{
				foreach (SqlParameter sqlParameter in parameters)
				{
					sqlParameter.Validate();
					SqlDbType sqlDbType = sqlParameter.SqlDbType;
					if (sqlParameter.Direction == (ParameterDirection)2)
					{
						sqlParameter.Value = null;
					}
					bool flag = false;
					bool flag2 = false;
					if (sqlParameter.Value == null || Convert.IsDBNull(sqlParameter.Value))
					{
						flag = true;
					}
					if (sqlParameter.Value is INullable)
					{
						flag2 = true;
						if (((INullable)sqlParameter.Value).IsNull)
						{
							flag = true;
						}
					}
					if (sqlParameter.ParameterName != null && sqlParameter.ParameterName.Length > 0)
					{
						this.WriteByte((byte)(sqlParameter.ParameterName.Length & 255));
						this.WriteString(sqlParameter.ParameterName);
					}
					else
					{
						this.WriteByte(0);
					}
					byte b = 0;
					object obj = sqlParameter.Value;
                    if (sqlParameter.Direction == (ParameterDirection)3 || sqlParameter.Direction == (ParameterDirection)2)
					{
						b = 1;
					}
                    if (sqlParameter.Direction != (ParameterDirection)2 && obj == null && !inSchema)
					{
						b |= 2;
					}
					this.WriteByte(b);
					MetaType metaType = sqlParameter.GetMetaType();
					this.WriteByte(metaType.NullableType);
					if (metaType.TDSType == 98)
					{
						this.WriteSqlVariantValue(obj, sqlParameter.ActualSize, sqlParameter.Offset);
					}
					else
					{
						if (!flag)
						{
							obj = sqlParameter.CoercedValue;
						}
						int num = MetaType.IsSizeInCharacters(sqlParameter.SqlDbType) ? (sqlParameter.Size * 2) : sqlParameter.Size;
						int actualSize = sqlParameter.ActualSize;
						int num2 = 0;
						if (MetaType.IsAnsiType(sqlParameter.SqlDbType))
						{
							if (!flag)
							{
								string value = flag2 ? ((SqlString)obj).Value : ((string)obj);
								num2 = this.GetEncodingCharLength(value, actualSize, sqlParameter.Offset, this.defaultEncoding);
							}
							this.WriteParameterVarLen(metaType, (num > num2) ? num : num2, false);
						}
                        else if (sqlParameter.SqlDbType == (SqlDbType)19)
						{
							this.WriteParameterVarLen(metaType, 8, false);
						}
						else
						{
							this.WriteParameterVarLen(metaType, (num > actualSize) ? num : actualSize, false);
						}
                        if (sqlDbType == (SqlDbType)5)
						{
							if (!flag)
							{
								if (flag2)
								{
									obj = this.AdjustSqlDecimalScale((SqlDecimal)obj, (int)sqlParameter.Scale);
								}
								else
								{
									obj = this.AdjustDecimalScale((decimal)obj, (int)sqlParameter.Scale);
								}
							}
							if (sqlParameter.Precision == 0)
							{
								this.WriteByte(28);
							}
							else
							{
								this.WriteByte(sqlParameter.Precision);
							}
							this.WriteByte(sqlParameter.Scale);
						}
						if (this.isShiloh && MetaType.IsCharType(sqlDbType))
						{
							SqlCollation sqlCollation = (sqlParameter.Collation != null) ? sqlParameter.Collation : this.defaultCollation;
							this.WriteUnsignedInt(sqlCollation.info);
							this.WriteByte(sqlCollation.sortId);
						}
						if (num2 == 0)
						{
							this.WriteParameterVarLen(metaType, actualSize, flag);
						}
						else
						{
							this.WriteParameterVarLen(metaType, num2, flag);
						}
						if (!flag)
						{
							if (flag2)
							{
								this.WriteSqlValue(obj, metaType, actualSize, sqlParameter.Offset);
							}
							else
							{
								this.WriteValue(obj, metaType, actualSize, sqlParameter.Offset);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.ResetBuffer();
				throw ex;
			}
			this.FlushBuffer(1);
			this.pendingData = true;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00011CC0 File Offset: 0x00010CC0
		internal void WriteSqlValue(object value, MetaType type, int actualLength, int offset)
		{
			byte nullableType = type.NullableType;
			if (nullableType <= 111)
			{
				switch (nullableType)
				{
				case 34:
					break;
				case 35:
					goto IL_1A6;
				case 36:
				{
					byte[] b = ((SqlGuid)value).ToByteArray();
					this.WriteByteArray(b, actualLength, 0);
					return;
				}
				case 37:
					return;
				case 38:
					if (type.FixedLength == 1)
					{
						this.WriteByte(((SqlByte)value).Value);
						return;
					}
					if (type.FixedLength == 2)
					{
						this.WriteShort((int)((SqlInt16)value).Value);
						return;
					}
					if (type.FixedLength == 4)
					{
						this.WriteInt(((SqlInt32)value).Value);
						return;
					}
					this.WriteLong(((SqlInt64)value).Value);
					return;
				default:
					if (nullableType == 99)
					{
						goto IL_1C5;
					}
					switch (nullableType)
					{
					case 104:
						if (((SqlBoolean)value).Value)
						{
							this.WriteByte(1);
							return;
						}
						this.WriteByte(0);
						return;
					case 105:
					case 106:
					case 107:
						return;
					case 108:
						this.WriteSqlDecimal((SqlDecimal)value);
						return;
					case 109:
						if (type.FixedLength == 4)
						{
							this.WriteFloat(((SqlSingle)value).Value);
							return;
						}
						this.WriteDouble(((SqlDouble)value).Value);
						return;
					case 110:
						this.WriteSqlMoney((SqlMoney)value, type.FixedLength);
						return;
					case 111:
					{
						SqlDateTime sqlDateTime = (SqlDateTime)value;
						if (type.FixedLength != 4)
						{
							this.WriteInt(sqlDateTime.DayTicks);
							this.WriteInt(sqlDateTime.TimeTicks);
							return;
						}
						if (0 > sqlDateTime.DayTicks || sqlDateTime.DayTicks > 65535)
						{
							throw SQL.SmallDateTimeOverflow(sqlDateTime.ToString());
						}
						this.WriteShort(sqlDateTime.DayTicks);
						this.WriteShort(sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute);
						return;
					}
					default:
						return;
					}
					break;
				}
			}
			else if (nullableType <= 175)
			{
				switch (nullableType)
				{
				case 165:
					break;
				case 166:
					return;
				case 167:
					goto IL_1A6;
				default:
					switch (nullableType)
					{
					case 173:
						break;
					case 174:
						return;
					case 175:
						goto IL_1A6;
					default:
						return;
					}
					break;
				}
			}
			else
			{
				if (nullableType != 231 && nullableType != 239)
				{
					return;
				}
				goto IL_1C5;
			}
			this.WriteByteArray(((SqlBinary)value).Value, actualLength, offset);
			return;
			IL_1A6:
			this.WriteEncodingChar(((SqlString)value).Value, actualLength, offset, this.defaultEncoding);
			return;
			IL_1C5:
			if (actualLength != 0)
			{
				actualLength >>= 1;
			}
			this.WriteString(((SqlString)value).Value, actualLength, offset);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00011F4C File Offset: 0x00010F4C
		internal void WriteValue(object value, MetaType type, int actualLength, int offset)
		{
			byte nullableType = type.NullableType;
			if (nullableType <= 111)
			{
				switch (nullableType)
				{
				case 34:
					break;
				case 35:
					goto IL_16A;
				case 36:
				{
					byte[] b = ((Guid)value).ToByteArray();
					this.WriteByteArray(b, actualLength, 0);
					return;
				}
				case 37:
					return;
				case 38:
					if (type.FixedLength == 1)
					{
						this.WriteByte((byte)value);
						return;
					}
					if (type.FixedLength == 2)
					{
						this.WriteShort((int)((short)value));
						return;
					}
					if (type.FixedLength == 4)
					{
						this.WriteInt((int)value);
						return;
					}
					this.WriteLong((long)value);
					return;
				default:
					if (nullableType == 99)
					{
						goto IL_180;
					}
					switch (nullableType)
					{
					case 104:
						if ((bool)value)
						{
							this.WriteByte(1);
							return;
						}
						this.WriteByte(0);
						return;
					case 105:
					case 106:
					case 107:
						return;
					case 108:
						this.WriteDecimal((decimal)value);
						return;
					case 109:
						if (type.FixedLength == 4)
						{
							this.WriteFloat((float)value);
							return;
						}
						this.WriteDouble((double)value);
						return;
					case 110:
						this.WriteCurrency((decimal)value, type.FixedLength);
						return;
					case 111:
					{
						TdsDateTime tdsDateTime = MetaDateTime.FromDateTime((DateTime)value, (byte)type.FixedLength);
						if (type.FixedLength != 4)
						{
							this.WriteInt(tdsDateTime.days);
							this.WriteInt(tdsDateTime.time);
							return;
						}
						if (0 > tdsDateTime.days || tdsDateTime.days > 65535)
						{
							throw SQL.SmallDateTimeOverflow(MetaDateTime.ToDateTime(tdsDateTime.days, tdsDateTime.time, 4).ToString());
						}
						this.WriteShort(tdsDateTime.days);
						this.WriteShort(tdsDateTime.time);
						return;
					}
					default:
						return;
					}
					break;
				}
			}
			else if (nullableType <= 175)
			{
				switch (nullableType)
				{
				case 165:
					break;
				case 166:
					return;
				case 167:
					goto IL_16A;
				default:
					switch (nullableType)
					{
					case 173:
						break;
					case 174:
						return;
					case 175:
						goto IL_16A;
					default:
						return;
					}
					break;
				}
			}
			else
			{
				if (nullableType != 231 && nullableType != 239)
				{
					return;
				}
				goto IL_180;
			}
			byte[] b2 = (byte[])value;
			this.WriteByteArray(b2, actualLength, offset);
			return;
			IL_16A:
			this.WriteEncodingChar((string)value, actualLength, offset, this.defaultEncoding);
			return;
			IL_180:
			if (actualLength != 0)
			{
				actualLength >>= 1;
			}
			this.WriteString((string)value, actualLength, offset);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000121A4 File Offset: 0x000111A4
		internal void WriteParameterVarLen(MetaType type, int size, bool isNull)
		{
			if (type.IsLong)
			{
				if (isNull)
				{
					this.WriteInt(-1);
					return;
				}
				this.WriteInt(size);
				return;
			}
			else if (!type.IsFixed)
			{
				if (isNull)
				{
					this.WriteShort(65535);
					return;
				}
				this.WriteShort(size);
				return;
			}
			else
			{
				if (isNull)
				{
					this.WriteByte(0);
					return;
				}
				this.WriteByte((byte)(type.FixedLength & 255));
				return;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0001220C File Offset: 0x0001120C
		internal byte[] EncryptPassword(string password)
		{
			byte[] array = new byte[password.Length << 1];
			for (int i = 0; i < password.Length; i++)
			{
				int num = (int)password[i];
				byte b = (byte)(num & 255);
				byte b2 = (byte)(num >> 8 & 255);
				array[i << 1] = (byte)(((int)(b & 15) << 4 | b >> 4) ^ 165);
				array[(i << 1) + 1] = (byte)(((int)(b2 & 15) << 4 | b2 >> 4) ^ 165);
			}
			return array;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0001228C File Offset: 0x0001128C
		internal SqlCommand PendingCommand
		{
			get
			{
				if (this.pendingCommandWeakRef != null)
				{
					SqlCommand result = (SqlCommand)this.pendingCommandWeakRef.Target;
					if (this.pendingCommandWeakRef.IsAlive)
					{
						return result;
					}
				}
				return null;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000363 RID: 867 RVA: 0x000122C2 File Offset: 0x000112C2
		internal bool PendingData
		{
			get
			{
				return this.pendingData;
			}
		}

		// Token: 0x04000285 RID: 645
		private const int ATTENTION_TIMEOUT = 5000;

		// Token: 0x04000286 RID: 646
		private byte[] outBuff;

		// Token: 0x04000287 RID: 647
		private int outBytesUsed = 8;

		// Token: 0x04000288 RID: 648
		private byte[] inBuff;

		// Token: 0x04000289 RID: 649
		private int inBytesUsed;

		// Token: 0x0400028A RID: 650
		private int inBytesRead;

		// Token: 0x0400028B RID: 651
		private int inBytesPacket;

		// Token: 0x0400028C RID: 652
		private bool pendingData;

		// Token: 0x0400028D RID: 653
		private WeakReference pendingCommandWeakRef;

		// Token: 0x0400028E RID: 654
		private byte _status;

		// Token: 0x0400028F RID: 655
		private byte _type;

		// Token: 0x04000290 RID: 656
		private TdsParserState state;

		// Token: 0x04000291 RID: 657
		private byte msgType;

		// Token: 0x04000292 RID: 658
		private byte packetNumber = 1;

		// Token: 0x04000293 RID: 659
		private int timeout;

		// Token: 0x04000294 RID: 660
		private string server;

		// Token: 0x04000295 RID: 661
		private TimeSpan timeRemaining;

		// Token: 0x04000296 RID: 662
		private Timer attentionTimer;

		// Token: 0x04000297 RID: 663
		private bool attentionSent;

		// Token: 0x04000298 RID: 664
		private bool isShiloh;

		// Token: 0x04000299 RID: 665
		private bool isShilohSP1;

		// Token: 0x0400029A RID: 666
		private bool fSSPIInit;

		// Token: 0x0400029B RID: 667
		private bool fSendSSPI = true;

		// Token: 0x0400029C RID: 668
		private bool fResetConnection;

		// Token: 0x0400029D RID: 669
		private static byte[] s_nicAddress;

		// Token: 0x0400029E RID: 670
		private static bool s_fSSPILoaded;

		// Token: 0x0400029F RID: 671
		private static int MAXSSPILENGTH;

		// Token: 0x040002A0 RID: 672
		private IntPtr pConObj = IntPtr.Zero;

		// Token: 0x040002A1 RID: 673
		private int errno;

		// Token: 0x040002A2 RID: 674
		private Encoding defaultEncoding;

		// Token: 0x040002A3 RID: 675
		private char[] charBuffer;

		// Token: 0x040002A4 RID: 676
		private byte[] byteBuffer;

		// Token: 0x040002A5 RID: 677
		private int[] decimalBits;

		// Token: 0x040002A6 RID: 678
		private SqlCollation defaultCollation;

		// Token: 0x040002A7 RID: 679
		private int defaultCodePage;

		// Token: 0x040002A8 RID: 680
		private int defaultLCID;

		// Token: 0x040002A9 RID: 681
		private byte[] bTmp = new byte[8];

		// Token: 0x040002AA RID: 682
		private _SqlMetaData[] cleanupMetaData;

		// Token: 0x040002AB RID: 683
		private SqlInternalConnection connHandler;

		// Token: 0x040002AC RID: 684
		private SqlException exception;

		// Token: 0x040002AD RID: 685
		private SqlException warning;
	}
}
