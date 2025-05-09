﻿// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteOpenFlags
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace SQLite
{
  [Flags]
  public enum SQLiteOpenFlags
  {
    ReadOnly = 1,
    ReadWrite = 2,
    Create = 4,
    NoMutex = 32768, // 0x00008000
    FullMutex = 65536, // 0x00010000
    SharedCache = 131072, // 0x00020000
    PrivateCache = 262144, // 0x00040000
    ProtectionComplete = 1048576, // 0x00100000
    ProtectionCompleteUnlessOpen = 2097152, // 0x00200000
    ProtectionCompleteUntilFirstUserAuthentication = ProtectionCompleteUnlessOpen | ProtectionComplete, // 0x00300000
    ProtectionNone = 4194304, // 0x00400000
  }
}
