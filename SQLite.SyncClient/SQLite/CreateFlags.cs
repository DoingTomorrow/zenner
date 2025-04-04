// Decompiled with JetBrains decompiler
// Type: SQLite.CreateFlags
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace SQLite
{
  [Flags]
  public enum CreateFlags
  {
    None = 0,
    ImplicitPK = 1,
    ImplicitIndex = 2,
    AllImplicit = ImplicitIndex | ImplicitPK, // 0x00000003
    AutoIncPK = 4,
  }
}
