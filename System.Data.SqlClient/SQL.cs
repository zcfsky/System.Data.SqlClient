using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200002C RID: 44
	internal sealed class SQL : ADP
	{
		// Token: 0x06000200 RID: 512 RVA: 0x0000B9D0 File Offset: 0x0000A9D0
		internal static Exception InvalidCharInConStringOption(string delimeter, string key)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidCharInConStringOption", new object[]
			{
				delimeter,
				key
			}));
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000B9FC File Offset: 0x0000A9FC
		internal static Exception InvalidCharInConStringValue(string delimeter, string value)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidCharInConStringValue", new object[]
			{
				delimeter,
				value
			}));
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000BA28 File Offset: 0x0000AA28
		internal static Exception InvalidConnectionOptionValue(string key, string value)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", new object[]
			{
				key,
				value
			}));
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000BA54 File Offset: 0x0000AA54
		internal static Exception InvalidConStringOption(string key)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidConStringOption", new object[]
			{
				key
			}));
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000BA7C File Offset: 0x0000AA7C
		internal static Exception InvalidIsolationLevelPropertyArg()
		{
			return new ArgumentException(Res.GetString("SQL_InvalidIsolationLevelPropertyArg"));
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000BA90 File Offset: 0x0000AA90
		internal static Exception InvalidOptionLength(string key, string value)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidOptionLength", new object[]
			{
				key,
				value
			}));
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000BABC File Offset: 0x0000AABC
		internal static Exception InvalidPacketSizeValue(string value)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidPacketSizeValue", new object[]
			{
				value
			}));
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000BAE4 File Offset: 0x0000AAE4
		internal static Exception MatchingEndDelimeterNotFound(string delimeter, string value)
		{
			return new ArgumentException(Res.GetString("SQL_MatchingEndDelimeterNotFound", new object[]
			{
				delimeter,
				value
			}));
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000BB10 File Offset: 0x0000AB10
		internal static Exception NoEqualSignInSubstring(string substring)
		{
			return new ArgumentException(Res.GetString("SQL_NoEqualSignInSubstring", new object[]
			{
				substring
			}));
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000BB38 File Offset: 0x0000AB38
		internal static Exception NoSemiColonInSubstring(string substring, string value)
		{
			return new ArgumentException(Res.GetString("SQL_NoSemiColonInSubstring", new object[]
			{
				substring,
				value
			}));
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000BB64 File Offset: 0x0000AB64
		internal static Exception NullEmptyTransactionName()
		{
			return new ArgumentException(Res.GetString("SQL_NullEmptyTransactionName"));
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000BB78 File Offset: 0x0000AB78
		internal static Exception InvalidSQLServerVersion(string version)
		{
			return new InvalidOperationException(Res.GetString("SQL_InvalidSQLServerVersion", new object[]
			{
				version
			}));
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000BBA0 File Offset: 0x0000ABA0
		internal static Exception TableDirectNotSupported()
		{
			return new ArgumentException(Res.GetString("SQL_TableDirectNotSupported"));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000BBB1 File Offset: 0x0000ABB1
		internal static Exception NonXmlResult()
		{
			return new InvalidOperationException(Res.GetString("SQL_NonXmlResult"));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000BBC4 File Offset: 0x0000ABC4
		internal static Exception InvalidParameterNameLength(string value)
		{
			return new ArgumentException(Res.GetString("SQL_InvalidParameterNameLength", new object[]
			{
				value
			}));
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000BBEC File Offset: 0x0000ABEC
		internal static Exception PrecisionValueOutOfRange(int precision)
		{
			return new ArgumentException(Res.GetString("SQL_PrecisionValueOutOfRange", new object[]
			{
				precision.ToString()
			}));
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000BC1C File Offset: 0x0000AC1C
		internal static Exception InvalidSqlDbType(string pmName, int value)
		{
			return new ArgumentOutOfRangeException(Res.GetString("SQL_InvalidSqlDbType", new object[]
			{
				pmName,
				value.ToString()
			}));
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000BC50 File Offset: 0x0000AC50
		internal static Exception ParameterInvalidVariant(string paramName)
		{
			return new InvalidOperationException(Res.GetString("SQL_ParameterInvalidVariant", new object[]
			{
				paramName
			}));
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000BC78 File Offset: 0x0000AC78
		internal static Exception MDAC_WrongVersion()
		{
			return new InvalidOperationException(Res.GetString("SQL_MDAC_WrongVersion"));
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000BC89 File Offset: 0x0000AC89
		internal static Exception ComputeByNotSupported()
		{
			return new InvalidOperationException(Res.GetString("SQL_ComputeByNotSupported"));
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000BC9C File Offset: 0x0000AC9C
		internal static Exception MoneyOverflow(string moneyValue)
		{
			return new OverflowException(Res.GetString("SQL_MoneyOverflow", new object[]
			{
				moneyValue
			}));
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000BCC4 File Offset: 0x0000ACC4
		internal static Exception SmallDateTimeOverflow(string datetime)
		{
			return new OverflowException(Res.GetString("SQL_SmallDateTimeOverflow", new object[]
			{
				datetime
			}));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000BCEC File Offset: 0x0000ACEC
		internal static Exception InvalidRead()
		{
			return new InvalidOperationException(Res.GetString("SQL_InvalidRead"));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000BD00 File Offset: 0x0000AD00
		internal static Exception NonBlobColumn(string columnName)
		{
			return new InvalidCastException(Res.GetString("SQL_NonBlobColumn", new object[]
			{
				columnName
			}));
		}

		// Token: 0x04000117 RID: 279
		private const string Global = "_Global_";
	}
}
