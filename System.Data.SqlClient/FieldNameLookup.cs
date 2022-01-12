using System;
using System.Collections;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x02000032 RID: 50
	internal sealed class FieldNameLookup
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0000CA40 File Offset: 0x0000BA40
		internal FieldNameLookup(IDataReader reader, int defaultLCID)
		{
			int fieldCount = reader.FieldCount;
			string[] array = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++)
			{
				array[i] = reader.GetName(i);
			}
			this._fieldNames = array;
			this._defaultLCID = defaultLCID;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CA88 File Offset: 0x0000BA88
		internal int GetOrdinal(string fieldName)
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			int num = this.IndexOf(fieldName);
			if (-1 == num)
			{
				throw new IndexOutOfRangeException(fieldName);
			}
			return num;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000CAB8 File Offset: 0x0000BAB8
		internal int IndexOf(string fieldName)
		{
			if (this._fieldNameLookup == null)
			{
				this.GenerateLookup();
			}
			int num;
			if (this._fieldNameLookup.Contains(fieldName))
			{
				num = (int)this._fieldNameLookup[fieldName];
			}
			else
			{
				num = this.LinearIndexOf(fieldName, (CompareOptions)1);
				if (-1 == num)
				{
                    num = this.LinearIndexOf(fieldName, (CompareOptions)25);
				}
			}
			return num;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CB10 File Offset: 0x0000BB10
		private int LinearIndexOf(string fieldName, CompareOptions compareOptions)
		{
			CompareInfo compareInfo = this._compareInfo;
			if (compareInfo == null)
			{
				if (-1 != this._defaultLCID)
				{
					compareInfo = CompareInfo.GetCompareInfo(this._defaultLCID);
				}
				if (compareInfo == null)
				{
					compareInfo = CultureInfo.CurrentCulture.CompareInfo;
				}
				this._compareInfo = compareInfo;
			}
			int num = this._fieldNames.Length;
			for (int i = 0; i < num; i++)
			{
				if (compareInfo.Compare(fieldName, this._fieldNames[i], compareOptions) == 0)
				{
					this._fieldNameLookup[fieldName] = i;
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000CB90 File Offset: 0x0000BB90
		private void GenerateLookup()
		{
			int num = this._fieldNames.Length;
			Hashtable hashtable = new Hashtable(num);
			int num2 = num - 1;
			while (0 <= num2)
			{
				string text = this._fieldNames[num2];
				hashtable[text] = num2;
				num2--;
			}
			this._fieldNameLookup = hashtable;
		}

		// Token: 0x04000139 RID: 313
		private Hashtable _fieldNameLookup;

		// Token: 0x0400013A RID: 314
		private string[] _fieldNames;

		// Token: 0x0400013B RID: 315
		private CompareInfo _compareInfo;

		// Token: 0x0400013C RID: 316
		private int _defaultLCID;
	}
}
