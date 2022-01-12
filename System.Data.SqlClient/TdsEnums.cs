using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200003D RID: 61
	internal sealed class TdsEnums
	{
		// Token: 0x0400014C RID: 332
		public const short SQL_SERVER_VERSION_SEVEN = 7;

		// Token: 0x0400014D RID: 333
		public const string SQL_PROVIDER_NAME = ".Net SqlClient Data Provider";

		// Token: 0x0400014E RID: 334
		public const string SDCI_MAPFILENAME = "SqlClientSSDebug";

		// Token: 0x0400014F RID: 335
		public const byte SDCI_MAX_MACHINENAME = 32;

		// Token: 0x04000150 RID: 336
		public const byte SDCI_MAX_DLLNAME = 16;

		// Token: 0x04000151 RID: 337
		public const byte SDCI_MAX_DATA = 255;

		// Token: 0x04000152 RID: 338
		public const int SQLDEBUG_OFF = 0;

		// Token: 0x04000153 RID: 339
		public const int SQLDEBUG_ON = 1;

		// Token: 0x04000154 RID: 340
		public const int SQLDEBUG_CONTEXT = 2;

		// Token: 0x04000155 RID: 341
		public const string SP_SDIDEBUG = "sp_sdidebug";

		// Token: 0x04000156 RID: 342
        public const SqlDbType SmallVarBinary = (SqlDbType)24;

		// Token: 0x04000157 RID: 343
		public const short TM_GET_DTC_ADDRESS = 0;

		// Token: 0x04000158 RID: 344
		public const short TM_PROPAGATE_XACT = 1;

		// Token: 0x04000159 RID: 345
		public const string TCP = "tcp";

		// Token: 0x0400015A RID: 346
		public const string INIT_SSPI_PACKAGE = "InitSSPIPackage";

		// Token: 0x0400015B RID: 347
		public const string INIT_SESSION = "InitSession";

		// Token: 0x0400015C RID: 348
		public const string SET_CREDENTIALS = "SetCredentialsHandle";

		// Token: 0x0400015D RID: 349
		public const string CONNECTION_GET_SVR_USER = "ConnectionGetSvrUser";

		// Token: 0x0400015E RID: 350
		public const string GEN_CLIENT_CONTEXT = "GenClientContext";

		// Token: 0x0400015F RID: 351
		public const byte SOFTFLUSH = 0;

		// Token: 0x04000160 RID: 352
		public const byte HARDFLUSH = 1;

		// Token: 0x04000161 RID: 353
		public const int HEADER_LEN = 8;

		// Token: 0x04000162 RID: 354
		public const int HEADER_LEN_FIELD_OFFSET = 2;

		// Token: 0x04000163 RID: 355
		public const int SUCCEED = 1;

		// Token: 0x04000164 RID: 356
		public const int FAIL = 0;

		// Token: 0x04000165 RID: 357
		public const short TYPE_SIZE_LIMIT = 8000;

		// Token: 0x04000166 RID: 358
		public const int MAX_IN_BUFFER_SIZE = 65535;

		// Token: 0x04000167 RID: 359
		public const int MAX_SERVER_USER_NAME = 255;

		// Token: 0x04000168 RID: 360
		public const byte DEFAULT_ERROR_CLASS = 10;

		// Token: 0x04000169 RID: 361
		public const byte FATAL_ERROR_CLASS = 20;

		// Token: 0x0400016A RID: 362
		public const int MIN_ERROR_CLASS = 10;

		// Token: 0x0400016B RID: 363
		public const byte MT_SQL = 1;

		// Token: 0x0400016C RID: 364
		public const byte MT_LOGIN = 2;

		// Token: 0x0400016D RID: 365
		public const byte MT_RPC = 3;

		// Token: 0x0400016E RID: 366
		public const byte MT_TOKENS = 4;

		// Token: 0x0400016F RID: 367
		public const byte MT_BINARY = 5;

		// Token: 0x04000170 RID: 368
		public const byte MT_ATTN = 6;

		// Token: 0x04000171 RID: 369
		public const byte MT_BULK = 7;

		// Token: 0x04000172 RID: 370
		public const byte MT_OPEN = 8;

		// Token: 0x04000173 RID: 371
		public const byte MT_CLOSE = 9;

		// Token: 0x04000174 RID: 372
		public const byte MT_ERROR = 10;

		// Token: 0x04000175 RID: 373
		public const byte MT_ACK = 11;

		// Token: 0x04000176 RID: 374
		public const byte MT_ECHO = 12;

		// Token: 0x04000177 RID: 375
		public const byte MT_LOGOUT = 13;

		// Token: 0x04000178 RID: 376
		public const byte MT_TRANS = 14;

		// Token: 0x04000179 RID: 377
		public const byte MT_OLEDB = 15;

		// Token: 0x0400017A RID: 378
		public const byte MT_LOGIN7 = 16;

		// Token: 0x0400017B RID: 379
		public const byte MT_SSPI = 17;

		// Token: 0x0400017C RID: 380
		public const byte ST_EOM = 1;

		// Token: 0x0400017D RID: 381
		public const byte ST_AACK = 2;

		// Token: 0x0400017E RID: 382
		public const byte ST_BATCH = 4;

		// Token: 0x0400017F RID: 383
		public const byte ST_RESET_CONNECTION = 8;

		// Token: 0x04000180 RID: 384
		public const byte SQLCOLFMT = 161;

		// Token: 0x04000181 RID: 385
		public const byte SQLPROCID = 124;

		// Token: 0x04000182 RID: 386
		public const byte SQLCOLNAME = 160;

		// Token: 0x04000183 RID: 387
		public const byte SQLTABNAME = 164;

		// Token: 0x04000184 RID: 388
		public const byte SQLCOLINFO = 165;

		// Token: 0x04000185 RID: 389
		public const byte SQLALTNAME = 167;

		// Token: 0x04000186 RID: 390
		public const byte SQLALTFMT = 168;

		// Token: 0x04000187 RID: 391
		public const byte SQLERROR = 170;

		// Token: 0x04000188 RID: 392
		public const byte SQLINFO = 171;

		// Token: 0x04000189 RID: 393
		public const byte SQLRETURNVALUE = 172;

		// Token: 0x0400018A RID: 394
		public const byte SQLRETURNSTATUS = 121;

		// Token: 0x0400018B RID: 395
		public const byte SQLRETURNTOK = 219;

		// Token: 0x0400018C RID: 396
		public const byte SQLCONTROL = 174;

		// Token: 0x0400018D RID: 397
		public const byte SQLALTCONTROL = 175;

		// Token: 0x0400018E RID: 398
		public const byte SQLROW = 209;

		// Token: 0x0400018F RID: 399
		public const byte SQLALTROW = 211;

		// Token: 0x04000190 RID: 400
		public const byte SQLDONE = 253;

		// Token: 0x04000191 RID: 401
		public const byte SQLDONEPROC = 254;

		// Token: 0x04000192 RID: 402
		public const byte SQLDONEINPROC = 255;

		// Token: 0x04000193 RID: 403
		public const byte SQLOFFSET = 120;

		// Token: 0x04000194 RID: 404
		public const byte SQLORDER = 169;

		// Token: 0x04000195 RID: 405
		public const byte SQLDEBUG_CMD = 96;

		// Token: 0x04000196 RID: 406
		public const byte SQLLOGINACK = 173;

		// Token: 0x04000197 RID: 407
		public const byte SQLENVCHANGE = 227;

		// Token: 0x04000198 RID: 408
		public const byte SQLSECLEVEL = 237;

		// Token: 0x04000199 RID: 409
		public const byte SQLROWCRC = 57;

		// Token: 0x0400019A RID: 410
		public const byte SQLCOLMETADATA = 129;

		// Token: 0x0400019B RID: 411
		public const byte SQLALTMETADATA = 136;

		// Token: 0x0400019C RID: 412
		public const byte SQLSSPI = 237;

		// Token: 0x0400019D RID: 413
		public const byte ENV_DATABASE = 1;

		// Token: 0x0400019E RID: 414
		public const byte ENV_LANG = 2;

		// Token: 0x0400019F RID: 415
		public const byte ENV_CHARSET = 3;

		// Token: 0x040001A0 RID: 416
		public const byte ENV_PACKETSIZE = 4;

		// Token: 0x040001A1 RID: 417
		public const byte ENV_TRANSACTION = 5;

		// Token: 0x040001A2 RID: 418
		public const byte ENV_LOCALEID = 5;

		// Token: 0x040001A3 RID: 419
		public const byte ENV_COMPFLAGS = 6;

		// Token: 0x040001A4 RID: 420
		public const byte ENV_COLLATION = 7;

		// Token: 0x040001A5 RID: 421
		public const int DONE_MORE = 1;

		// Token: 0x040001A6 RID: 422
		public const int DONE_ERROR = 2;

		// Token: 0x040001A7 RID: 423
		public const int DONE_INXACT = 4;

		// Token: 0x040001A8 RID: 424
		public const int DONE_PROC = 8;

		// Token: 0x040001A9 RID: 425
		public const int DONE_COUNT = 16;

		// Token: 0x040001AA RID: 426
		public const int DONE_ATTN = 32;

		// Token: 0x040001AB RID: 427
		public const int DONE_INPROC = 64;

		// Token: 0x040001AC RID: 428
		public const int DONE_RPCINBATCH = 128;

		// Token: 0x040001AD RID: 429
		public const int DONE_SRVERROR = 256;

		// Token: 0x040001AE RID: 430
		public const int DONE_FMTSENT = 32768;

		// Token: 0x040001AF RID: 431
		public const int DONE_SQLSELECT = 193;

		// Token: 0x040001B0 RID: 432
		public const byte MAX_LOG_NAME = 30;

		// Token: 0x040001B1 RID: 433
		public const byte MAX_PROG_NAME = 10;

		// Token: 0x040001B2 RID: 434
		public const short MAX_LOGIN_FIELD = 256;

		// Token: 0x040001B3 RID: 435
		public const byte SEC_COMP_LEN = 8;

		// Token: 0x040001B4 RID: 436
		public const byte MAX_PK_LEN = 6;

		// Token: 0x040001B5 RID: 437
		public const byte MAX_NIC_SIZE = 6;

		// Token: 0x040001B6 RID: 438
		public const byte SQLVARIANT_SIZE = 2;

		// Token: 0x040001B7 RID: 439
		public const byte VERSION_SIZE = 4;

		// Token: 0x040001B8 RID: 440
		public const int CLIENT_PROG_VER = 100663296;

		// Token: 0x040001B9 RID: 441
		public const int LOG_REC_FIXED_LEN = 86;

		// Token: 0x040001BA RID: 442
		public const int TEXT_TIME_STAMP_LEN = 8;

		// Token: 0x040001BB RID: 443
		public const int COLLATION_INFO_LEN = 4;

		// Token: 0x040001BC RID: 444
		public const int TDS70 = 1792;

		// Token: 0x040001BD RID: 445
		public const int TDS71 = 1793;

		// Token: 0x040001BE RID: 446
		public const int SPHINX_MAJOR = 28672;

		// Token: 0x040001BF RID: 447
		public const int SHILOH_MAJOR = 28928;

		// Token: 0x040001C0 RID: 448
		public const int DEFAULT_MINOR = 0;

		// Token: 0x040001C1 RID: 449
		public const int SHILOH_MINOR_SP1 = 1;

		// Token: 0x040001C2 RID: 450
		public const int ORDER_68000 = 1;

		// Token: 0x040001C3 RID: 451
		public const int USE_DB_ON = 1;

		// Token: 0x040001C4 RID: 452
		public const int INIT_DB_FATAL = 1;

		// Token: 0x040001C5 RID: 453
		public const int SET_LANG_ON = 1;

		// Token: 0x040001C6 RID: 454
		public const int INIT_LANG_FATAL = 1;

		// Token: 0x040001C7 RID: 455
		public const int ODBC_ON = 1;

		// Token: 0x040001C8 RID: 456
		public const int SSPI_ON = 1;

		// Token: 0x040001C9 RID: 457
		public const byte SQLLenMask = 48;

		// Token: 0x040001CA RID: 458
		public const byte SQLFixedLen = 48;

		// Token: 0x040001CB RID: 459
		public const byte SQLVarLen = 32;

		// Token: 0x040001CC RID: 460
		public const byte SQLZeroLen = 16;

		// Token: 0x040001CD RID: 461
		public const byte SQLVarCnt = 0;

		// Token: 0x040001CE RID: 462
		public const byte SQLDifferentName = 32;

		// Token: 0x040001CF RID: 463
		public const byte SQLExpression = 4;

		// Token: 0x040001D0 RID: 464
		public const byte SQLKey = 8;

		// Token: 0x040001D1 RID: 465
		public const byte SQLHidden = 16;

		// Token: 0x040001D2 RID: 466
		public const byte Nullable = 1;

		// Token: 0x040001D3 RID: 467
		public const byte Identity = 16;

		// Token: 0x040001D4 RID: 468
		public const byte Updatability = 11;

		// Token: 0x040001D5 RID: 469
		public const uint VARLONGNULL = 4294967295U;

		// Token: 0x040001D6 RID: 470
		public const int VARNULL = 65535;

		// Token: 0x040001D7 RID: 471
		public const int MAXSIZE = 8000;

		// Token: 0x040001D8 RID: 472
		public const byte FIXEDNULL = 0;

		// Token: 0x040001D9 RID: 473
		public const int SQLVOID = 31;

		// Token: 0x040001DA RID: 474
		public const int SQLTEXT = 35;

		// Token: 0x040001DB RID: 475
		public const int SQLVARBINARY = 37;

		// Token: 0x040001DC RID: 476
		public const int SQLINTN = 38;

		// Token: 0x040001DD RID: 477
		public const int SQLVARCHAR = 39;

		// Token: 0x040001DE RID: 478
		public const int SQLBINARY = 45;

		// Token: 0x040001DF RID: 479
		public const int SQLIMAGE = 34;

		// Token: 0x040001E0 RID: 480
		public const int SQLCHAR = 47;

		// Token: 0x040001E1 RID: 481
		public const int SQLINT1 = 48;

		// Token: 0x040001E2 RID: 482
		public const int SQLBIT = 50;

		// Token: 0x040001E3 RID: 483
		public const int SQLINT2 = 52;

		// Token: 0x040001E4 RID: 484
		public const int SQLINT4 = 56;

		// Token: 0x040001E5 RID: 485
		public const int SQLMONEY = 60;

		// Token: 0x040001E6 RID: 486
		public const int SQLDATETIME = 61;

		// Token: 0x040001E7 RID: 487
		public const int SQLFLT8 = 62;

		// Token: 0x040001E8 RID: 488
		public const int SQLFLTN = 109;

		// Token: 0x040001E9 RID: 489
		public const int SQLMONEYN = 110;

		// Token: 0x040001EA RID: 490
		public const int SQLDATETIMN = 111;

		// Token: 0x040001EB RID: 491
		public const int SQLFLT4 = 59;

		// Token: 0x040001EC RID: 492
		public const int SQLMONEY4 = 122;

		// Token: 0x040001ED RID: 493
		public const int SQLDATETIM4 = 58;

		// Token: 0x040001EE RID: 494
		public const int SQLDECIMALN = 106;

		// Token: 0x040001EF RID: 495
		public const int SQLNUMERICN = 108;

		// Token: 0x040001F0 RID: 496
		public const int SQLUNIQUEID = 36;

		// Token: 0x040001F1 RID: 497
		public const int SQLBIGCHAR = 175;

		// Token: 0x040001F2 RID: 498
		public const int SQLBIGVARCHAR = 167;

		// Token: 0x040001F3 RID: 499
		public const int SQLBIGBINARY = 173;

		// Token: 0x040001F4 RID: 500
		public const int SQLBIGVARBINARY = 165;

		// Token: 0x040001F5 RID: 501
		public const int SQLBITN = 104;

		// Token: 0x040001F6 RID: 502
		public const int SQLNCHAR = 239;

		// Token: 0x040001F7 RID: 503
		public const int SQLNVARCHAR = 231;

		// Token: 0x040001F8 RID: 504
		public const int SQLNTEXT = 99;

		// Token: 0x040001F9 RID: 505
		public const int SQLTIMESTAMP = 80;

		// Token: 0x040001FA RID: 506
		public const int MAX_NUMERIC_LEN = 17;

		// Token: 0x040001FB RID: 507
		public const int DEFAULT_NUMERIC_PRECISION = 28;

		// Token: 0x040001FC RID: 508
		public const int MAX_NUMERIC_PRECISION = 38;

		// Token: 0x040001FD RID: 509
		public const byte UNKNOWN_PRECISION_SCALE = 255;

		// Token: 0x040001FE RID: 510
		public const int SQLINT8 = 127;

		// Token: 0x040001FF RID: 511
		public const int SQLVARIANT = 98;

		// Token: 0x04000200 RID: 512
		public const bool Is68K = false;

		// Token: 0x04000201 RID: 513
		public const bool TraceTDS = false;

		// Token: 0x04000202 RID: 514
		public const string SP_EXECUTESQL = "sp_executesql";

		// Token: 0x04000203 RID: 515
		public const string SP_PREPEXEC = "sp_prepexec";

		// Token: 0x04000204 RID: 516
		public const string SP_PREPARE = "sp_prepare";

		// Token: 0x04000205 RID: 517
		public const string SP_EXECUTE = "sp_execute";

		// Token: 0x04000206 RID: 518
		public const string SP_UNPREPARE = "sp_unprepare";

		// Token: 0x04000207 RID: 519
		public const string SP_PARAMS = "sp_procedure_params_rowset";

		// Token: 0x04000208 RID: 520
		public const string TRANS_BEGIN = "BEGIN TRANSACTION";

		// Token: 0x04000209 RID: 521
		public const string TRANS_COMMIT = "COMMIT TRANSACTION";

		// Token: 0x0400020A RID: 522
		public const string TRANS_ROLLBACK = "ROLLBACK TRANSACTION";

		// Token: 0x0400020B RID: 523
		public const string TRANS_SAVE = "SAVE TRANSACTION";

		// Token: 0x0400020C RID: 524
		public const string TRANS_READ_COMMITTED = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";

		// Token: 0x0400020D RID: 525
		public const string TRANS_READ_UNCOMMITTED = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

		// Token: 0x0400020E RID: 526
		public const string TRANS_REPEATABLE_READ = "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ";

		// Token: 0x0400020F RID: 527
		public const string TRANS_SERIALIZABLE = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";

		// Token: 0x04000210 RID: 528
		public const byte RPC_RECOMPILE = 1;

		// Token: 0x04000211 RID: 529
		public const byte RPC_NOMETADATA = 2;

		// Token: 0x04000212 RID: 530
		public const string PARAM_OUTPUT = "output";

		// Token: 0x04000213 RID: 531
		public const int MAX_PARAMETER_NAME_LENGTH = 127;

		// Token: 0x04000214 RID: 532
		public const string FMTONLY_ON = " SET FMTONLY ON;";

		// Token: 0x04000215 RID: 533
		public const string FMTONLY_OFF = " SET FMTONLY OFF;";

		// Token: 0x04000216 RID: 534
		public const string BROWSE_ON = " SET NO_BROWSETABLE ON;";

		// Token: 0x04000217 RID: 535
		public const string BROWSE_OFF = " SET NO_BROWSETABLE OFF;";

		// Token: 0x04000218 RID: 536
		public const string TABLE = "Table";

		// Token: 0x04000219 RID: 537
		public const int EXEC_THRESHOLD = 3;

		// Token: 0x0400021A RID: 538
		public const short ERRNO_MIN = -3;

		// Token: 0x0400021B RID: 539
		public const short ERRNO_MAX = -1;

		// Token: 0x0400021C RID: 540
		public const short ZERO_BYTES_READ = -3;

		// Token: 0x0400021D RID: 541
		public const short TIMEOUT_EXPIRED = -2;

		// Token: 0x0400021E RID: 542
		public const short UNKNOWN_ERROR = -1;

		// Token: 0x0400021F RID: 543
		public const short NE_E_NOMAP = 0;

		// Token: 0x04000220 RID: 544
		public const short NE_E_NOMEMORY = 1;

		// Token: 0x04000221 RID: 545
		public const short NE_E_NOACCESS = 2;

		// Token: 0x04000222 RID: 546
		public const short NE_E_CONNBUSY = 3;

		// Token: 0x04000223 RID: 547
		public const short NE_E_CONNBROKEN = 4;

		// Token: 0x04000224 RID: 548
		public const short NE_E_TOOMANYCONN = 5;

		// Token: 0x04000225 RID: 549
		public const short NE_E_SERVERNOTFOUND = 6;

		// Token: 0x04000226 RID: 550
		public const short NE_E_NETNOTSTARTED = 7;

		// Token: 0x04000227 RID: 551
		public const short NE_E_NORESOURCE = 8;

		// Token: 0x04000228 RID: 552
		public const short NE_E_NETBUSY = 9;

		// Token: 0x04000229 RID: 553
		public const short NE_E_NONETACCESS = 10;

		// Token: 0x0400022A RID: 554
		public const short NE_E_GENERAL = 11;

		// Token: 0x0400022B RID: 555
		public const short NE_E_CONNMODE = 12;

		// Token: 0x0400022C RID: 556
		public const short NE_E_NAMENOTFOUND = 13;

		// Token: 0x0400022D RID: 557
		public const short NE_E_INVALIDCONN = 14;

		// Token: 0x0400022E RID: 558
		public const short NE_E_NETDATAERR = 15;

		// Token: 0x0400022F RID: 559
		public const short NE_E_TOOMANYFILES = 16;

		// Token: 0x04000230 RID: 560
		public const short NE_E_SERVERERROR = 17;

		// Token: 0x04000231 RID: 561
		public const short NE_E_SSLSECURITYERROR = 18;

		// Token: 0x04000232 RID: 562
		public const short NE_E_ENCRYPTIONON = 19;

		// Token: 0x04000233 RID: 563
		public const short NE_E_ENCRYPTIONNOTSUPPORTED = 20;

		// Token: 0x04000234 RID: 564
		public const string DEFAULT_ENGLISH_CODE_PAGE_STRING = "iso_1";

		// Token: 0x04000235 RID: 565
		public const short DEFAULT_ENGLISH_CODE_PAGE_VALUE = 1252;

		// Token: 0x04000236 RID: 566
		public const short CHARSET_CODE_PAGE_OFFSET = 2;

		// Token: 0x04000237 RID: 567
		public static readonly decimal SQL_SMALL_MONEY_MIN = new decimal(-214748.3648);

		// Token: 0x04000238 RID: 568
		public static readonly decimal SQL_SMALL_MONEY_MAX = new decimal(214748.3647);

		// Token: 0x04000239 RID: 569
		public static readonly string[] SQLDEBUG_MODE_NAMES = new string[]
		{
			"off",
			"on",
			"context"
		};

		// Token: 0x0400023A RID: 570
		public static readonly ushort[] CODE_PAGE_FROM_SORT_ID = new ushort[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			437,
			437,
			437,
			437,
			437,
			0,
			0,
			0,
			0,
			0,
			850,
			850,
			850,
			850,
			850,
			0,
			0,
			0,
			0,
			850,
			1252,
			1252,
			1252,
			1252,
			1252,
			850,
			850,
			850,
			850,
			850,
			850,
			850,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1252,
			1252,
			1252,
			1252,
			1252,
			0,
			0,
			0,
			0,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			1250,
			0,
			0,
			0,
			0,
			0,
			0,
			1251,
			1251,
			1251,
			1251,
			1251,
			0,
			0,
			0,
			1253,
			1253,
			1253,
			0,
			0,
			0,
			0,
			0,
			1253,
			1253,
			0,
			0,
			1253,
			0,
			0,
			0,
			1254,
			1254,
			1254,
			0,
			0,
			0,
			0,
			0,
			1255,
			1255,
			1255,
			0,
			0,
			0,
			0,
			0,
			1256,
			1256,
			1256,
			0,
			0,
			0,
			0,
			0,
			1257,
			1257,
			1257,
			1257,
			1257,
			1257,
			1257,
			1257,
			1257,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1252,
			1252,
			1252,
			1252,
			0,
			0,
			0,
			0,
			0,
			932,
			932,
			949,
			949,
			950,
			950,
			936,
			936,
			932,
			949,
			950,
			936,
			874,
			874,
			874,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
	}
}
