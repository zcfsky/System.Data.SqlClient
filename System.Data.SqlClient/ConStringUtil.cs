using System;
using System.Collections;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x02000004 RID: 4
	internal sealed class ConStringUtil
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00003730 File Offset: 0x00002730
		public static Hashtable NullConnectionOptionsTable()
		{
			Hashtable hashtable = new Hashtable(21);
			hashtable["application name"] = null;
			hashtable["connect timeout"] = null;
			hashtable["current language"] = null;
			hashtable["data source"] = null;
			hashtable["initial catalog"] = null;
			hashtable["integrated security"] = null;
			hashtable["packet size"] = null;
			hashtable["password"] = null;
			hashtable["persist security info"] = null;
			hashtable["user id"] = null;
			hashtable["workstation id"] = null;
			return hashtable;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000037CC File Offset: 0x000027CC
		public static Hashtable DefaultConnectionOptionsTable()
		{
			Hashtable hashtable = ConStringUtil._defaultConnectionOptions;
			if (hashtable == null)
			{
				hashtable = new Hashtable(21);
				hashtable["application name"] = ".Net SqlClient Data Provider";
				hashtable["connect timeout"] = 15;
				hashtable["current language"] = "";
				hashtable["data source"] = "";
				hashtable["initial catalog"] = "";
				hashtable["integrated security"] = false;
				hashtable["packet size"] = 8192;
				hashtable["password"] = "";
				hashtable["persist security info"] = false;
				hashtable["user id"] = "";
				hashtable["workstation id"] = null;
				hashtable["user set blank password"] = false;
				ConStringUtil._defaultConnectionOptions = hashtable;
			}
			return hashtable;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000038BC File Offset: 0x000028BC
		private static void InitMappingsTables()
		{
			if (ConStringUtil._connectionSynonymMapping == null)
			{
				Hashtable hashtable = new Hashtable(18);
				hashtable["app"] = "application name";
				hashtable["connection timeout"] = "connect timeout";
				hashtable["timeout"] = "connect timeout";
				hashtable["language"] = "current language";
				hashtable["addr"] = "data source";
				hashtable["address"] = "data source";
				hashtable["network address"] = "data source";
				hashtable["server"] = "data source";
				hashtable["database"] = "initial catalog";
				hashtable["trusted_connection"] = "integrated security";
				hashtable["pwd"] = "password";
				hashtable["persistsecurityinfo"] = "persist security info";
				hashtable["uid"] = "user id";
				hashtable["wsid"] = "workstation id";
				ConStringUtil._connectionSynonymMapping = hashtable;
			}
			if (ConStringUtil._netlibMapping == null)
			{
				Hashtable hashtable2 = new Hashtable(8);
				hashtable2["dbmssocn"] = "tcp";
				ConStringUtil._netlibMapping = hashtable2;
			}
			if (ConStringUtil._boolYesNoMapping == null)
			{
				Hashtable hashtable3 = new Hashtable(4);
				hashtable3["yes"] = true;
				hashtable3["no"] = false;
				hashtable3["true"] = true;
				hashtable3["false"] = false;
				ConStringUtil._boolYesNoMapping = hashtable3;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003A40 File Offset: 0x00002A40
		public static string RemoveKeyValuesFromString(string conString, string removeKey)
		{
			string text = conString;
			int i = 0;
			int num = 0;
			ConStringUtil.SkipWhiteSpace(text, ref i);
			while (i < text.Length)
			{
				if (text[i] == ';')
				{
					i++;
					num = i;
					ConStringUtil.SkipWhiteSpace(text, ref i);
				}
				else
				{
					int num2 = text.IndexOf('=', i);
					string text2 = text.Substring(i, num2 - i).TrimEnd(null).ToLower(CultureInfo.InvariantCulture);
					i = num2 + 1;
					if (removeKey == "password" && ("pwd" == text2 || "password" == text2))
					{
						string text3 = text.Substring(0, num);
						ConStringUtil.SkipValue(text, ref i);
						if (i < text.Length && text[i] == ';')
						{
							i++;
						}
						text = text3 + text.Substring(i);
						i = num;
						ConStringUtil.SkipWhiteSpace(text, ref i);
					}
					else
					{
						ConStringUtil.SkipValue(text, ref i);
						if (i < text.Length && text[i] == ';')
						{
							i++;
							num = i;
							ConStringUtil.SkipWhiteSpace(text, ref i);
						}
					}
				}
			}
			return text;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003B54 File Offset: 0x00002B54
		private static void SkipValue(string conString, ref int index)
		{
			ConStringUtil.SkipWhiteSpace(conString, ref index);
			if (index == conString.Length)
			{
				return;
			}
			char c = conString[index];
			char c2 = c;
			if (c2 != '"' && c2 != '\'')
			{
				if (c2 != ';')
				{
					index = conString.IndexOf(';', index + 1);
					if (index == -1)
					{
						index = conString.Length;
					}
				}
				return;
			}
			int num = conString.IndexOf(c, index + 1);
			index = num + 1;
			ConStringUtil.SkipWhiteSpace(conString, ref index);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003BC4 File Offset: 0x00002BC4
		public static Hashtable ParseConnectionString(string connectionString)
		{
			ConStringUtil.InitMappingsTables();
			Hashtable hashtable;
			if (!ADP.IsEmpty(connectionString))
			{
				hashtable = ConStringUtil.NullConnectionOptionsTable();
				ConStringUtil.ParseStringIntoHashtable(connectionString, hashtable);
				ConStringUtil.VerifyValues(hashtable);
			}
			else
			{
				hashtable = ConStringUtil.DefaultConnectionOptionsTable();
			}
			return hashtable;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003BFC File Offset: 0x00002BFC
		private static void ParseStringIntoHashtable(string conString, Hashtable values)
		{
			string text = null;
			string text2 = null;
			int i = 0;
			int num = 0;
			bool flag = false;
			char[] array = new char[1];
			char[] array2 = array;
			conString = conString.TrimEnd(array2);
			ConStringUtil.SkipWhiteSpace(conString, ref i);
			while (i < conString.Length)
			{
				if (conString[i] == ';')
				{
					i++;
					ConStringUtil.SkipWhiteSpace(conString, ref i);
				}
				else
				{
					int num2 = conString.IndexOf('=', i);
					if (num2 == -1)
					{
						throw SQL.NoEqualSignInSubstring(conString.Substring(i));
					}
					for (int j = i; j < num2; j++)
					{
						char c = conString[j];
						if (c == ';' || c == '\'' || c == '"')
						{
							throw SQL.InvalidCharInConStringOption(c.ToString(), conString.Substring(i, num2 - i));
						}
					}
					text = conString.Substring(i, num2 - i).TrimEnd(null).ToLower(CultureInfo.InvariantCulture);
					if (num2 + 1 == conString.Length)
					{
						text2 = null;
						i = conString.Length;
					}
					else
					{
						i = num2 + 1;
						ConStringUtil.SkipWhiteSpace(conString, ref i);
						if (i == conString.Length)
						{
							text2 = null;
						}
						else
						{
							Exception ex = ConStringUtil.ParseValue(conString, ref text2, ref i, ref num);
							if (ex != null)
							{
								throw ex;
							}
						}
					}
					if (!ConStringUtil.InsertKeyValueIntoHash(values, ref text, text2))
					{
						throw SQL.InvalidConStringOption(text);
					}
					flag |= ("password" == text && (text2 == null || 0 == text2.Length));
				}
			}
			values["user set blank password"] = flag;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003D7C File Offset: 0x00002D7C
		private static Exception ParseValue(string conString, ref string value, ref int index, ref int valueIndex)
		{
			Exception ex = null;
			char c = conString[index];
			char c2 = c;
			if (c2 != '"' && c2 != '\'')
			{
				if (c2 == ';')
				{
					value = null;
					index++;
					ConStringUtil.SkipWhiteSpace(conString, ref index);
				}
				else
				{
					int num = conString.IndexOf(';', index + 1);
					int num2;
					if (num == -1)
					{
						num2 = conString.Length;
					}
					else
					{
						num2 = num;
					}
					int i = index;
					while (i < num2)
					{
						char c3 = conString[i];
						if (c3 == '=' || c3 == '\'' || c3 == '"')
						{
							if (num == -1)
							{
								ex = SQL.InvalidCharInConStringValue(c3.ToString(), conString.Substring(index));
								break;
							}
							ex = SQL.InvalidCharInConStringValue(c3.ToString(), conString.Substring(index, num - index));
							break;
						}
						else
						{
							i++;
						}
					}
					if (ex == null)
					{
						if (num == -1)
						{
							value = conString.Substring(index).TrimEnd(null);
							valueIndex = index;
							index = conString.Length;
						}
						else
						{
							value = conString.Substring(index, num - index).TrimEnd(null);
							valueIndex = index;
							index = num + 1;
							ConStringUtil.SkipWhiteSpace(conString, ref index);
						}
					}
				}
			}
			else
			{
				int num = conString.IndexOf(c, index + 1);
				if (num == -1)
				{
					ex = SQL.MatchingEndDelimeterNotFound(c.ToString(), conString.Substring(index));
				}
				else
				{
					value = conString.Substring(index + 1, num - index - 1);
					valueIndex = index + 1;
					index = num + 1;
					ConStringUtil.SkipWhiteSpace(conString, ref index);
					if (index < conString.Length)
					{
						if (conString[index] == ';')
						{
							index++;
							ConStringUtil.SkipWhiteSpace(conString, ref index);
						}
						else
						{
							ex = SQL.NoSemiColonInSubstring(conString, value);
						}
					}
				}
			}
			return ex;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003F14 File Offset: 0x00002F14
		private static void SkipWhiteSpace(string conString, ref int index)
		{
			while (index < conString.Length)
			{
				char c = conString[index];
				if (c != ' ' && c != '\t')
				{
					return;
				}
				index++;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003F48 File Offset: 0x00002F48
		private static bool InsertKeyValueIntoHash(Hashtable values, ref string key, string value)
		{
			bool result = true;
			if (values.ContainsKey(key))
			{
				values[key] = value;
			}
			else if (ConStringUtil._connectionSynonymMapping.ContainsKey(key))
			{
				key = (string)ConStringUtil._connectionSynonymMapping[key];
				values[key] = value;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003F9C File Offset: 0x00002F9C
		private static void VerifyValues(Hashtable values)
		{
			ConStringUtil.SetConnectTimeout(values);
			ConStringUtil.SetPacketSize(values);
			ConStringUtil.SetPersistSecurityInfo(values);
			ConStringUtil.SetTrustedConnection(values);
			ConStringUtil.SetStringType("application name", values, ".Net SqlClient Data Provider");
			ConStringUtil.SetStringType("current language", values, "");
			ConStringUtil.SetStringType("initial catalog", values, "");
			ConStringUtil.SetStringType("password", values, "");
			ConStringUtil.SetStringType("data source", values, "");
			ConStringUtil.SetStringType("user id", values, "");
			if (values["workstation id"] == null)
			{
				values["workstation id"] = ConStringUtil.WORKSTATION_ID;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004040 File Offset: 0x00003040
		private static void SetConnectTimeout(Hashtable table)
		{
			string text = (string)table["connect timeout"];
			if (text == null)
			{
				table["connect timeout"] = 15;
				return;
			}
			if (text != text.Trim())
			{
				throw new ArgumentException(Res.GetString("ADP_InvalidConnectTimeoutValue", new object[]
				{
					text
				}));
			}
			int num;
			try
			{
				num = int.Parse(text, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				throw new ArgumentException(Res.GetString("ADP_InvalidConnectTimeoutValue", new object[]
				{
					text
				}));
			}
			if (num < 0)
			{
				throw new ArgumentException(Res.GetString("ADP_InvalidConnectTimeoutValue", new object[]
				{
					text
				}));
			}
			table["connect timeout"] = num;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004110 File Offset: 0x00003110
		private static void SetPacketSize(Hashtable table)
		{
			string text = (string)table["packet size"];
			if (text == null)
			{
				table["packet size"] = 8192;
				return;
			}
			if (text != text.Trim())
			{
				throw SQL.InvalidPacketSizeValue(text);
			}
			int num;
			try
			{
				num = int.Parse(text, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				throw SQL.InvalidPacketSizeValue(text);
			}
			if (num < 512 || num > 32767)
			{
				throw SQL.InvalidPacketSizeValue(num.ToString());
			}
			table["packet size"] = num;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000041B0 File Offset: 0x000031B0
		private static void SetPersistSecurityInfo(Hashtable table)
		{
			string text = (string)table["persist security info"];
			string text2 = text;
			if (text != null)
			{
				text2 = text.ToLower(CultureInfo.InvariantCulture);
			}
			if (text2 == null)
			{
				table["persist security info"] = false;
				return;
			}
			if (ConStringUtil._boolYesNoMapping.ContainsKey(text2))
			{
				table["persist security info"] = (bool)ConStringUtil._boolYesNoMapping[text2];
				return;
			}
			throw SQL.InvalidConnectionOptionValue("persist security info", text);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004230 File Offset: 0x00003230
		private static void SetTrustedConnection(Hashtable table)
		{
			string text = (string)table["integrated security"];
			string text2 = text;
			if (text != null)
			{
				text2 = text.ToLower(CultureInfo.InvariantCulture);
			}
			if (text2 == null)
			{
				table["integrated security"] = false;
				return;
			}
			if (ConStringUtil._boolYesNoMapping.ContainsKey(text2))
			{
				table["integrated security"] = ConStringUtil._boolYesNoMapping[text2];
				return;
			}
			if (string.Compare(text2, "sspi", false, CultureInfo.InvariantCulture) == 0)
			{
				table["integrated security"] = true;
				return;
			}
			throw SQL.InvalidConnectionOptionValue("integrated security", text);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000042C8 File Offset: 0x000032C8
		private static void SetStringType(string key, Hashtable values, string defaultValue)
		{
			string text = (string)values[key];
			if (text != null)
			{
				values[key] = text;
				return;
			}
			values[key] = defaultValue;
		}

		// Token: 0x04000021 RID: 33
		private const int CONNECTION_OPTIONS = 21;

		// Token: 0x04000022 RID: 34
		private const int CONNECTION_SYNONYMS = 18;

		// Token: 0x04000023 RID: 35
		private const int NETLIB_OPTIONS = 8;

		// Token: 0x04000024 RID: 36
		private const int BOOL_YES_NO_OPTIONS = 4;

		// Token: 0x04000025 RID: 37
		private const string YES_KEY = "yes";

		// Token: 0x04000026 RID: 38
		private const string NO_KEY = "no";

		// Token: 0x04000027 RID: 39
		private const string TRUE_KEY = "true";

		// Token: 0x04000028 RID: 40
		private const string FALSE_KEY = "false";

		// Token: 0x04000029 RID: 41
		private const string DBMSSOCN_KEY = "dbmssocn";

		// Token: 0x0400002A RID: 42
		private const string LOCAL_VALUE_1 = ".";

		// Token: 0x0400002B RID: 43
		private const string LOCAL_VALUE_2 = "(local)";

		// Token: 0x0400002C RID: 44
		private const string _sspiKey = "sspi";

		// Token: 0x0400002D RID: 45
		public const string APPLICATION_NAME_KEY = "application name";

		// Token: 0x0400002E RID: 46
		public const string CONNECT_TIMEOUT_KEY = "connect timeout";

		// Token: 0x0400002F RID: 47
		public const string CURRENT_LANGUAGE_KEY = "current language";

		// Token: 0x04000030 RID: 48
		public const string DATA_SOURCE_KEY = "data source";

		// Token: 0x04000031 RID: 49
		public const string INITIAL_CATALOG_KEY = "initial catalog";

		// Token: 0x04000032 RID: 50
		public const string INTEGRATED_SECURITY_KEY = "integrated security";

		// Token: 0x04000033 RID: 51
		public const string PACKET_SIZE_KEY = "packet size";

		// Token: 0x04000034 RID: 52
		public const string PASSWORD_KEY = "password";

		// Token: 0x04000035 RID: 53
		public const string PERSIST_SECURITY_INFO_KEY = "persist security info";

		// Token: 0x04000036 RID: 54
		public const string USER_ID_KEY = "user id";

		// Token: 0x04000037 RID: 55
		public const string WORKSTATION_ID_KEY = "workstation id";

		// Token: 0x04000038 RID: 56
		public const string PASSWORD_USER_SET_KEY = "user set blank password";

		// Token: 0x04000039 RID: 57
		private const string APP_SYNONYM = "app";

		// Token: 0x0400003A RID: 58
		private const string INITIAL_FILE_NAME_SYNONYM = "initial file name";

		// Token: 0x0400003B RID: 59
		private const string CONNECTION_TIMEOUT_SYNONYM = "connection timeout";

		// Token: 0x0400003C RID: 60
		private const string TIMEOUT_SYNONYM = "timeout";

		// Token: 0x0400003D RID: 61
		private const string LANGUAGE_SYNONYM = "language";

		// Token: 0x0400003E RID: 62
		private const string ADDR_SYNONYM = "addr";

		// Token: 0x0400003F RID: 63
		private const string ADDRESS_SYNONYM = "address";

		// Token: 0x04000040 RID: 64
		private const string SERVER_SYNONYM = "server";

		// Token: 0x04000041 RID: 65
		private const string NETWORK_ADDRESS_SYNONYM = "network address";

		// Token: 0x04000042 RID: 66
		private const string DATABASE_SYNONYM = "database";

		// Token: 0x04000043 RID: 67
		private const string TRUSTED_CONNECTION_SYNONYM = "trusted_connection";

		// Token: 0x04000044 RID: 68
		private const string PWD_SYNONYM = "pwd";

		// Token: 0x04000045 RID: 69
		private const string PERSISTSECURITYINFO_SYNONYM = "persistsecurityinfo";

		// Token: 0x04000046 RID: 70
		private const string UID_SYNONYM = "uid";

		// Token: 0x04000047 RID: 71
		private const string WSID_SYNONYM = "wsid";

		// Token: 0x04000048 RID: 72
		public const string APPLICATION_NAME = ".Net SqlClient Data Provider";

		// Token: 0x04000049 RID: 73
		public const int CONNECTION_LIFETIME = 0;

		// Token: 0x0400004A RID: 74
		public const int CONNECT_TIMEOUT = 15;

		// Token: 0x0400004B RID: 75
		public const bool CONNECTION_RESET = true;

		// Token: 0x0400004C RID: 76
		public const string CURRENT_LANGUAGE = "";

		// Token: 0x0400004D RID: 77
		public const string DATA_SOURCE = "";

		// Token: 0x0400004E RID: 78
		public const bool ENLIST = true;

		// Token: 0x0400004F RID: 79
		public const string INITIAL_CATALOG = "";

		// Token: 0x04000050 RID: 80
		public const bool INTEGRATED_SECURITY = false;

		// Token: 0x04000051 RID: 81
		public const int MIN_PACKET_SIZE = 512;

		// Token: 0x04000052 RID: 82
		public const int MAX_PACKET_SIZE = 32767;

		// Token: 0x04000053 RID: 83
		public const int PACKET_SIZE = 8192;

		// Token: 0x04000054 RID: 84
		public const string PASSWORD = "";

		// Token: 0x04000055 RID: 85
		public const bool PERSIST_SECURITY_INFO = false;

		// Token: 0x04000056 RID: 86
		public const string USER_ID = "";

		// Token: 0x04000057 RID: 87
		public const bool PASSWORD_USER_SET = false;

		// Token: 0x04000058 RID: 88
		private const char EQUAL_SIGN = '=';

		// Token: 0x04000059 RID: 89
		private const char SEMI_COLON = ';';

		// Token: 0x0400005A RID: 90
		private const char SINGLE_QUOTE = '\'';

		// Token: 0x0400005B RID: 91
		private const char DOUBLE_QUOTE = '"';

		// Token: 0x0400005C RID: 92
		private const char SPACE = ' ';

		// Token: 0x0400005D RID: 93
		private const char TAB = '\t';

		// Token: 0x0400005E RID: 94
		private const char BACKSLASH = '\\';

		// Token: 0x0400005F RID: 95
		private static Hashtable _defaultConnectionOptions;

		// Token: 0x04000060 RID: 96
		private static Hashtable _connectionSynonymMapping;

		// Token: 0x04000061 RID: 97
		internal static Hashtable _netlibMapping;

		// Token: 0x04000062 RID: 98
		private static Hashtable _boolYesNoMapping;

		// Token: 0x04000063 RID: 99
		public static string WORKSTATION_ID = ".NET CF SqlClient";
	}
}
