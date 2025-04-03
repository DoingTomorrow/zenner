// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTypeCallbacksMap
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLiteTypeCallbacksMap : Dictionary<string, SQLiteTypeCallbacks>
  {
    public SQLiteTypeCallbacksMap()
      : base((IEqualityComparer<string>) new TypeNameStringComparer())
    {
    }
  }
}
