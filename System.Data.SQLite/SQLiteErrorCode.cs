// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteErrorCode
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public enum SQLiteErrorCode
  {
    Unknown = -1, // 0xFFFFFFFF
    Ok = 0,
    Error = 1,
    Internal = 2,
    Perm = 3,
    Abort = 4,
    Busy = 5,
    Locked = 6,
    NoMem = 7,
    ReadOnly = 8,
    Interrupt = 9,
    IoErr = 10, // 0x0000000A
    Corrupt = 11, // 0x0000000B
    NotFound = 12, // 0x0000000C
    Full = 13, // 0x0000000D
    CantOpen = 14, // 0x0000000E
    Protocol = 15, // 0x0000000F
    Empty = 16, // 0x00000010
    Schema = 17, // 0x00000011
    TooBig = 18, // 0x00000012
    Constraint = 19, // 0x00000013
    Mismatch = 20, // 0x00000014
    Misuse = 21, // 0x00000015
    NoLfs = 22, // 0x00000016
    Auth = 23, // 0x00000017
    Format = 24, // 0x00000018
    Range = 25, // 0x00000019
    NotADb = 26, // 0x0000001A
    Notice = 27, // 0x0000001B
    Warning = 28, // 0x0000001C
    Row = 100, // 0x00000064
    Done = 101, // 0x00000065
    NonExtendedMask = 255, // 0x000000FF
    Ok_Load_Permanently = 256, // 0x00000100
    Busy_Recovery = 261, // 0x00000105
    Locked_SharedCache = 262, // 0x00000106
    ReadOnly_Recovery = 264, // 0x00000108
    IoErr_Read = 266, // 0x0000010A
    Corrupt_Vtab = 267, // 0x0000010B
    CantOpen_NoTempDir = 270, // 0x0000010E
    Constraint_Check = 275, // 0x00000113
    Auth_User = 279, // 0x00000117
    Notice_Recover_Wal = 283, // 0x0000011B
    Warning_AutoIndex = 284, // 0x0000011C
    Abort_Rollback = 516, // 0x00000204
    Busy_Snapshot = 517, // 0x00000205
    ReadOnly_CantLock = 520, // 0x00000208
    IoErr_Short_Read = 522, // 0x0000020A
    CantOpen_IsDir = 526, // 0x0000020E
    Constraint_CommitHook = 531, // 0x00000213
    Notice_Recover_Rollback = 539, // 0x0000021B
    ReadOnly_Rollback = 776, // 0x00000308
    IoErr_Write = 778, // 0x0000030A
    CantOpen_FullPath = 782, // 0x0000030E
    Constraint_ForeignKey = 787, // 0x00000313
    ReadOnly_DbMoved = 1032, // 0x00000408
    IoErr_Fsync = 1034, // 0x0000040A
    CantOpen_ConvPath = 1038, // 0x0000040E
    Constraint_Function = 1043, // 0x00000413
    IoErr_Dir_Fsync = 1290, // 0x0000050A
    Constraint_NotNull = 1299, // 0x00000513
    IoErr_Truncate = 1546, // 0x0000060A
    Constraint_PrimaryKey = 1555, // 0x00000613
    IoErr_Fstat = 1802, // 0x0000070A
    Constraint_Trigger = 1811, // 0x00000713
    IoErr_Unlock = 2058, // 0x0000080A
    Constraint_Unique = 2067, // 0x00000813
    IoErr_RdLock = 2314, // 0x0000090A
    Constraint_Vtab = 2323, // 0x00000913
    IoErr_Delete = 2570, // 0x00000A0A
    Constraint_RowId = 2579, // 0x00000A13
    IoErr_Blocked = 2826, // 0x00000B0A
    IoErr_NoMem = 3082, // 0x00000C0A
    IoErr_Access = 3338, // 0x00000D0A
    IoErr_CheckReservedLock = 3594, // 0x00000E0A
    IoErr_Lock = 3850, // 0x00000F0A
    IoErr_Close = 4106, // 0x0000100A
    IoErr_Dir_Close = 4362, // 0x0000110A
    IoErr_ShmOpen = 4618, // 0x0000120A
    IoErr_ShmSize = 4874, // 0x0000130A
    IoErr_ShmLock = 5130, // 0x0000140A
    IoErr_ShmMap = 5386, // 0x0000150A
    IoErr_Seek = 5642, // 0x0000160A
    IoErr_Delete_NoEnt = 5898, // 0x0000170A
    IoErr_Mmap = 6154, // 0x0000180A
    IoErr_GetTempPath = 6410, // 0x0000190A
    IoErr_ConvPath = 6666, // 0x00001A0A
    IoErr_VNode = 6922, // 0x00001B0A
    IoErr_Auth = 7178, // 0x00001C0A
  }
}
