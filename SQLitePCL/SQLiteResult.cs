﻿// Decompiled with JetBrains decompiler
// Type: SQLitePCL.SQLiteResult
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

#nullable disable
namespace SQLitePCL
{
  public enum SQLiteResult
  {
    OK = 0,
    ERROR = 1,
    INTERNAL = 2,
    PERM = 3,
    ABORT = 4,
    BUSY = 5,
    LOCKED = 6,
    NOMEM = 7,
    READONLY = 8,
    INTERRUPT = 9,
    IOERR = 10, // 0x0000000A
    CORRUPT = 11, // 0x0000000B
    NOTFOUND = 12, // 0x0000000C
    FULL = 13, // 0x0000000D
    CANTOPEN = 14, // 0x0000000E
    PROTOCOL = 15, // 0x0000000F
    EMPTY = 16, // 0x00000010
    SCHEMA = 17, // 0x00000011
    TOOBIG = 18, // 0x00000012
    CONSTRAINT = 19, // 0x00000013
    MISMATCH = 20, // 0x00000014
    MISUSE = 21, // 0x00000015
    NOLFS = 22, // 0x00000016
    AUTH = 23, // 0x00000017
    FORMAT = 24, // 0x00000018
    RANGE = 25, // 0x00000019
    NOTADB = 26, // 0x0000001A
    NOTICE = 27, // 0x0000001B
    WARNING = 28, // 0x0000001C
    ROW = 100, // 0x00000064
    DONE = 101, // 0x00000065
  }
}
