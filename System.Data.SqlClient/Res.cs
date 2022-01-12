using System;
using System.Globalization;
using System.Resources;

namespace System.Data.SqlClient
{
	// Token: 0x0200004B RID: 75
	internal sealed class Res
	{
		// Token: 0x06000364 RID: 868 RVA: 0x000122CA File Offset: 0x000112CA
		private Res()
		{
			this.resources = new ResourceManager("System.Data.SqlClient", base.GetType().Module.Assembly);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000122F4 File Offset: 0x000112F4
		private static Res GetLoader()
		{
			if (Res.loader == null)
			{
				lock (typeof(Res))
				{
					if (Res.loader == null)
					{
						Res.loader = new Res();
					}
				}
			}
			return Res.loader;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00012348 File Offset: 0x00011348
		public static string GetString(string name, params object[] args)
		{
			return Res.GetString(null, name, args);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00012354 File Offset: 0x00011354
		public static string GetString(CultureInfo culture, string name, params object[] args)
		{
			string text = null;
			string result;
			try
			{
				Res res = Res.GetLoader();
				if (res == null)
				{
					result = null;
				}
				else
				{
					text = res.resources.GetString(name, culture);
					if (args != null && args.Length > 0)
					{
						result = string.Format(CultureInfo.CurrentCulture, text, args);
					}
					else
					{
						result = text;
					}
				}
			}
			catch (FormatException)
			{
				result = text;
			}
			return result;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000123B0 File Offset: 0x000113B0
		public static string GetString(string name)
		{
			return Res.GetString(null, name);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000123BC File Offset: 0x000113BC
		public static string GetString(CultureInfo culture, string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, culture);
		}

		// Token: 0x040002AE RID: 686
		internal const string ADP_ClosedConnectionError = "ADP_ClosedConnectionError";

		// Token: 0x040002AF RID: 687
		internal const string ADP_CollectionIndexString = "ADP_CollectionIndexString";

		// Token: 0x040002B0 RID: 688
		internal const string ADP_CollectionInvalidType = "ADP_CollectionInvalidType";

		// Token: 0x040002B1 RID: 689
		internal const string ADP_CollectionNullValue = "ADP_CollectionNullValue";

		// Token: 0x040002B2 RID: 690
		internal const string ADP_CollectionRemoveInvalidObject = "ADP_CollectionRemoveInvalidObject";

		// Token: 0x040002B3 RID: 691
		internal const string ADP_CommandIsActive = "ADP_CommandIsActive";

		// Token: 0x040002B4 RID: 692
		internal const string ADP_CommandTextRequired = "ADP_CommandTextRequired";

		// Token: 0x040002B5 RID: 693
		internal const string ADP_ConnectionAlreadyOpen = "ADP_ConnectionAlreadyOpen";

		// Token: 0x040002B6 RID: 694
		internal const string ADP_ConnectionRequired_Cancel = "ADP_ConnectionRequired_Cancel";

		// Token: 0x040002B7 RID: 695
		internal const string ADP_ConnectionRequired_ExecuteNonQuery = "ADP_ConnectionRequired_ExecuteNonQuery";

		// Token: 0x040002B8 RID: 696
		internal const string ADP_ConnectionRequired_ExecuteReader = "ADP_ConnectionRequired_ExecuteReader";

		// Token: 0x040002B9 RID: 697
		internal const string ADP_ConnectionRequired_ExecuteScalar = "ADP_ConnectionRequired_ExecuteScalar";

		// Token: 0x040002BA RID: 698
		internal const string ADP_ConnectionRequired_Prepare = "ADP_ConnectionRequired_Prepare";

		// Token: 0x040002BB RID: 699
		internal const string ADP_DataReaderClosed = "ADP_DataReaderClosed";

		// Token: 0x040002BC RID: 700
		internal const string ADP_DeriveParametersNotSupported = "ADP_DeriveParametersNotSupported";

		// Token: 0x040002BD RID: 701
		internal const string ADP_DynamicSQLJoinUnsupported = "ADP_DynamicSQLJoinUnsupported";

		// Token: 0x040002BE RID: 702
		internal const string ADP_DynamicSQLNestedQuote = "ADP_DynamicSQLNestedQuote";

		// Token: 0x040002BF RID: 703
		internal const string ADP_DynamicSQLNoKeyInfo = "ADP_DynamicSQLNoKeyInfo";

		// Token: 0x040002C0 RID: 704
		internal const string ADP_DynamicSQLNoTableInfo = "ADP_DynamicSQLNoTableInfo";

		// Token: 0x040002C1 RID: 705
		internal const string ADP_DynamicSQLReadOnly = "ADP_DynamicSQLReadOnly";

		// Token: 0x040002C2 RID: 706
		internal const string ADP_EmptyDatabaseName = "ADP_EmptyDatabaseName";

		// Token: 0x040002C3 RID: 707
		internal const string ADP_InvalidCommandTimeout = "ADP_InvalidCommandTimeout";

		// Token: 0x040002C4 RID: 708
		internal const string ADP_InvalidCommandType = "ADP_InvalidCommandType";

		// Token: 0x040002C5 RID: 709
		internal const string ADP_InvalidConnectTimeoutValue = "ADP_InvalidConnectTimeoutValue";

		// Token: 0x040002C6 RID: 710
		internal const string ADP_InvalidDataRowVersion = "ADP_InvalidDataRowVersion";

		// Token: 0x040002C7 RID: 711
		internal const string ADP_InvalidDataType = "ADP_InvalidDataType";

		// Token: 0x040002C8 RID: 712
		internal const string ADP_InvalidParameterDirection = "ADP_InvalidParameterDirection";

		// Token: 0x040002C9 RID: 713
		internal const string ADP_InvalidSizeValue = "ADP_InvalidSizeValue";

		// Token: 0x040002CA RID: 714
		internal const string ADP_InvalidUpdateRowSource = "ADP_InvalidUpdateRowSource";

		// Token: 0x040002CB RID: 715
		internal const string ADP_MissingSourceCommand = "ADP_MissingSourceCommand";

		// Token: 0x040002CC RID: 716
		internal const string ADP_MissingSourceCommandConnection = "ADP_MissingSourceCommandConnection";

		// Token: 0x040002CD RID: 717
		internal const string ADP_NoConnectionString = "ADP_NoConnectionString";

		// Token: 0x040002CE RID: 718
		internal const string ADP_NonSeqByteAccess = "ADP_NonSeqByteAccess";

		// Token: 0x040002CF RID: 719
		internal const string ADP_NonSequentialColumnAccess = "ADP_NonSequentialColumnAccess";

		// Token: 0x040002D0 RID: 720
		internal const string ADP_NoQuoteChange = "ADP_NoQuoteChange";

		// Token: 0x040002D1 RID: 721
		internal const string ADP_NoStoredProcedureExists = "ADP_NoStoredProcedureExists";

		// Token: 0x040002D2 RID: 722
		internal const string ADP_OpenConnectionPropertySet = "ADP_OpenConnectionPropertySet";

		// Token: 0x040002D3 RID: 723
		internal const string ADP_OpenConnectionRequired_BeginTransaction = "ADP_OpenConnectionRequired_BeginTransaction";

		// Token: 0x040002D4 RID: 724
		internal const string ADP_OpenConnectionRequired_Cancel = "ADP_OpenConnectionRequired_Cancel";

		// Token: 0x040002D5 RID: 725
		internal const string ADP_OpenConnectionRequired_ChangeDatabase = "ADP_OpenConnectionRequired_ChangeDatabase";

		// Token: 0x040002D6 RID: 726
		internal const string ADP_OpenConnectionRequired_CommitTransaction = "ADP_OpenConnectionRequired_CommitTransaction";

		// Token: 0x040002D7 RID: 727
		internal const string ADP_OpenConnectionRequired_ExecuteNonQuery = "ADP_OpenConnectionRequired_ExecuteNonQuery";

		// Token: 0x040002D8 RID: 728
		internal const string ADP_OpenConnectionRequired_ExecuteReader = "ADP_OpenConnectionRequired_ExecuteReader";

		// Token: 0x040002D9 RID: 729
		internal const string ADP_OpenConnectionRequired_ExecuteScalar = "ADP_OpenConnectionRequired_ExecuteScalar";

		// Token: 0x040002DA RID: 730
		internal const string ADP_OpenConnectionRequired_Prepare = "ADP_OpenConnectionRequired_Prepare";

		// Token: 0x040002DB RID: 731
		internal const string ADP_OpenConnectionRequired_RollbackTransaction = "ADP_OpenConnectionRequired_RollbackTransaction";

		// Token: 0x040002DC RID: 732
		internal const string ADP_OpenReaderExists = "ADP_OpenReaderExists";

		// Token: 0x040002DD RID: 733
		internal const string ADP_ParallelTransactionsNotSupported = "ADP_ParallelTransactionsNotSupported";

		// Token: 0x040002DE RID: 734
		internal const string ADP_PrepareParameterScale = "ADP_PrepareParameterScale";

		// Token: 0x040002DF RID: 735
		internal const string ADP_PrepareParameterSize = "ADP_PrepareParameterSize";

		// Token: 0x040002E0 RID: 736
		internal const string ADP_PrepareParameterType = "ADP_PrepareParameterType";

		// Token: 0x040002E1 RID: 737
		internal const string ADP_StreamClosed = "ADP_StreamClosed";

		// Token: 0x040002E2 RID: 738
		internal const string ADP_TransactionConnectionMismatch = "ADP_TransactionConnectionMismatch";

		// Token: 0x040002E3 RID: 739
		internal const string ADP_TransactionRequired_Execute = "ADP_TransactionRequired_Execute";

		// Token: 0x040002E4 RID: 740
		internal const string ADP_TransactionZombied = "ADP_TransactionZombied";

		// Token: 0x040002E5 RID: 741
		internal const string ADP_UninitializedParameterSize = "ADP_UninitializedParameterSize";

		// Token: 0x040002E6 RID: 742
		internal const string ADP_UnknownDataType = "ADP_UnknownDataType";

		// Token: 0x040002E7 RID: 743
		internal const string ADP_WhereClauseUnspecifiedValue = "ADP_WhereClauseUnspecifiedValue";

		// Token: 0x040002E8 RID: 744
		internal const string Data_ArgumentNull = "Data_ArgumentNull";

		// Token: 0x040002E9 RID: 745
		internal const string SQL_AccessDenied = "SQL_AccessDenied";

		// Token: 0x040002EA RID: 746
		internal const string SQL_ComputeByNotSupported = "SQL_ComputeByNotSupported";

		// Token: 0x040002EB RID: 747
		internal const string SQL_ConnectionBroken = "SQL_ConnectionBroken";

		// Token: 0x040002EC RID: 748
		internal const string SQL_ConnectionBusy = "SQL_ConnectionBusy";

		// Token: 0x040002ED RID: 749
		internal const string SQL_ConnectionLimit = "SQL_ConnectionLimit";

		// Token: 0x040002EE RID: 750
		internal const string SQL_CultureIdError = "SQL_CultureIdError";

		// Token: 0x040002EF RID: 751
		internal const string SQL_EncryptionError = "SQL_EncryptionError";

		// Token: 0x040002F0 RID: 752
		internal const string SQL_EncryptionNotSupported = "SQL_EncryptionNotSupported";

		// Token: 0x040002F1 RID: 753
		internal const string SQL_GeneralError = "SQL_GeneralError";

		// Token: 0x040002F2 RID: 754
		internal const string SQL_IncorrectMode = "SQL_IncorrectMode";

		// Token: 0x040002F3 RID: 755
		internal const string SQL_InsufficientMemory = "SQL_InsufficientMemory";

		// Token: 0x040002F4 RID: 756
		internal const string SQL_InsufficientResources = "SQL_InsufficientResources";

		// Token: 0x040002F5 RID: 757
		internal const string SQL_InvalidBufferSizeOrIndex = "SQL_InvalidBufferSizeOrIndex";

		// Token: 0x040002F6 RID: 758
		internal const string SQL_InvalidCharInConStringOption = "SQL_InvalidCharInConStringOption";

		// Token: 0x040002F7 RID: 759
		internal const string SQL_InvalidCharInConStringValue = "SQL_InvalidCharInConStringValue";

		// Token: 0x040002F8 RID: 760
		internal const string SQL_InvalidConnection = "SQL_InvalidConnection";

		// Token: 0x040002F9 RID: 761
		internal const string SQL_InvalidConnectionOptionValue = "SQL_InvalidConnectionOptionValue";

		// Token: 0x040002FA RID: 762
		internal const string SQL_InvalidConStringOption = "SQL_InvalidConStringOption";

		// Token: 0x040002FB RID: 763
		internal const string SQL_InvalidDataLength = "SQL_InvalidDataLength";

		// Token: 0x040002FC RID: 764
		internal const string SQL_InvalidIsolationLevelPropertyArg = "SQL_InvalidIsolationLevelPropertyArg";

		// Token: 0x040002FD RID: 765
		internal const string SQL_InvalidOptionLength = "SQL_InvalidOptionLength";

		// Token: 0x040002FE RID: 766
		internal const string SQL_InvalidPacketSizeValue = "SQL_InvalidPacketSizeValue";

		// Token: 0x040002FF RID: 767
		internal const string SQL_InvalidParameterNameLength = "SQL_InvalidParameterNameLength";

		// Token: 0x04000300 RID: 768
		internal const string SQL_InvalidRead = "SQL_InvalidRead";

		// Token: 0x04000301 RID: 769
		internal const string SQL_InvalidSqlDbType = "SQL_InvalidSqlDbType";

		// Token: 0x04000302 RID: 770
		internal const string SQL_InvalidSQLServerVersion = "SQL_InvalidSQLServerVersion";

		// Token: 0x04000303 RID: 771
		internal const string SQL_MatchingEndDelimeterNotFound = "SQL_MatchingEndDelimeterNotFound";

		// Token: 0x04000304 RID: 772
		internal const string SQL_MDAC_WrongVersion = "SQL_MDAC_WrongVersion";

		// Token: 0x04000305 RID: 773
		internal const string SQL_MoneyOverflow = "SQL_MoneyOverflow";

		// Token: 0x04000306 RID: 774
		internal const string SQL_NameNotFound = "SQL_NameNotFound";

		// Token: 0x04000307 RID: 775
		internal const string SQL_NetworkAccessDenied = "SQL_NetworkAccessDenied";

		// Token: 0x04000308 RID: 776
		internal const string SQL_NetworkBusy = "SQL_NetworkBusy";

		// Token: 0x04000309 RID: 777
		internal const string SQL_NetworkNotFound = "SQL_NetworkNotFound";

		// Token: 0x0400030A RID: 778
		internal const string SQL_NoEqualSignInSubstring = "SQL_NoEqualSignInSubstring";

		// Token: 0x0400030B RID: 779
		internal const string SQL_NonBlobColumn = "SQL_NonBlobColumn";

		// Token: 0x0400030C RID: 780
		internal const string SQL_NonXmlResult = "SQL_NonXmlResult";

		// Token: 0x0400030D RID: 781
		internal const string SQL_NoSemiColonInSubstring = "SQL_NoSemiColonInSubstring";

		// Token: 0x0400030E RID: 782
		internal const string SQL_NullEmptyTransactionName = "SQL_NullEmptyTransactionName";

		// Token: 0x0400030F RID: 783
		internal const string SQL_OperationCancelled = "SQL_OperationCancelled";

		// Token: 0x04000310 RID: 784
		internal const string SQL_ParameterInvalidVariant = "SQL_ParameterInvalidVariant";

		// Token: 0x04000311 RID: 785
		internal const string SQL_PrecisionValueOutOfRange = "SQL_PrecisionValueOutOfRange";

		// Token: 0x04000312 RID: 786
		internal const string SQL_ReadWriteError = "SQL_ReadWriteError";

		// Token: 0x04000313 RID: 787
		internal const string SQL_ServerError = "SQL_ServerError";

		// Token: 0x04000314 RID: 788
		internal const string SQL_ServerNotFound = "SQL_ServerNotFound";

		// Token: 0x04000315 RID: 789
		internal const string SQL_SevereError = "SQL_SevereError";

		// Token: 0x04000316 RID: 790
		internal const string SQL_SmallDateTimeOverflow = "SQL_SmallDateTimeOverflow";

		// Token: 0x04000317 RID: 791
		internal const string SQL_SSLError = "SQL_SSLError";

		// Token: 0x04000318 RID: 792
		internal const string SQL_SSPIGenerateError = "SQL_SSPIGenerateError";

		// Token: 0x04000319 RID: 793
		internal const string SQL_SSPIInitializeError = "SQL_SSPIInitializeError";

		// Token: 0x0400031A RID: 794
		internal const string SQL_TableDirectNotSupported = "SQL_TableDirectNotSupported";

		// Token: 0x0400031B RID: 795
		internal const string SQL_Timeout = "SQL_Timeout";

		// Token: 0x0400031C RID: 796
		internal const string SQL_TooManyHandles = "SQL_TooManyHandles";

		// Token: 0x0400031D RID: 797
		internal const string SQL_Unknown = "SQL_Unknown";

		// Token: 0x0400031E RID: 798
		internal const string SQL_ZeroBytes = "SQL_ZeroBytes";

		// Token: 0x0400031F RID: 799
		private static Res loader;

		// Token: 0x04000320 RID: 800
		private ResourceManager resources;
	}
}
