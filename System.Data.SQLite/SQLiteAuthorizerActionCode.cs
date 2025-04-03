// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteAuthorizerActionCode
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public enum SQLiteAuthorizerActionCode
  {
    None = -1, // 0xFFFFFFFF
    Copy = 0,
    CreateIndex = 1,
    CreateTable = 2,
    CreateTempIndex = 3,
    CreateTempTable = 4,
    CreateTempTrigger = 5,
    CreateTempView = 6,
    CreateTrigger = 7,
    CreateView = 8,
    Delete = 9,
    DropIndex = 10, // 0x0000000A
    DropTable = 11, // 0x0000000B
    DropTempIndex = 12, // 0x0000000C
    DropTempTable = 13, // 0x0000000D
    DropTempTrigger = 14, // 0x0000000E
    DropTempView = 15, // 0x0000000F
    DropTrigger = 16, // 0x00000010
    DropView = 17, // 0x00000011
    Insert = 18, // 0x00000012
    Pragma = 19, // 0x00000013
    Read = 20, // 0x00000014
    Select = 21, // 0x00000015
    Transaction = 22, // 0x00000016
    Update = 23, // 0x00000017
    Attach = 24, // 0x00000018
    Detach = 25, // 0x00000019
    AlterTable = 26, // 0x0000001A
    Reindex = 27, // 0x0000001B
    Analyze = 28, // 0x0000001C
    CreateVtable = 29, // 0x0000001D
    DropVtable = 30, // 0x0000001E
    Function = 31, // 0x0000001F
    Savepoint = 32, // 0x00000020
    Recursive = 33, // 0x00000021
  }
}
