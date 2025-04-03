// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteConnectionPool
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;

#nullable disable
namespace System.Data.SQLite
{
  public interface ISQLiteConnectionPool
  {
    void GetCounts(
      string fileName,
      ref Dictionary<string, int> counts,
      ref int openCount,
      ref int closeCount,
      ref int totalCount);

    void ClearPool(string fileName);

    void ClearAllPools();

    void Add(string fileName, object handle, int version);

    object Remove(string fileName, int maxPoolSize, out int version);
  }
}
