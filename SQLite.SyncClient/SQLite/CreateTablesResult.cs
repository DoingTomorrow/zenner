// Decompiled with JetBrains decompiler
// Type: SQLite.CreateTablesResult
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SQLite
{
  public class CreateTablesResult
  {
    public Dictionary<Type, int> Results { get; private set; }

    internal CreateTablesResult() => this.Results = new Dictionary<Type, int>();
  }
}
