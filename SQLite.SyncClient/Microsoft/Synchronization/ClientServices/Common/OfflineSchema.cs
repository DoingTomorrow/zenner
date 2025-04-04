// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.Common.OfflineSchema
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.Common
{
  public class OfflineSchema
  {
    private readonly Dictionary<string, Type> collections;

    public OfflineSchema() => this.collections = new Dictionary<string, Type>();

    public ReadOnlyCollection<Type> Collections
    {
      get
      {
        return new ReadOnlyCollection<Type>((IList<Type>) new List<Type>((IEnumerable<Type>) this.collections.Values));
      }
    }

    public void AddCollection<T>() where T : IOfflineEntity
    {
      Type t = typeof (T);
      if (t == typeof (SQLiteOfflineEntity) && SQLiteOfflineEntity.GetEntityPrimaryKeyProperties(t).Length == 0)
        throw new ArgumentException("Type: " + t.FullName + " does not have a primary key specified");
      this.collections.Add(t.FullName, t);
    }
  }
}
