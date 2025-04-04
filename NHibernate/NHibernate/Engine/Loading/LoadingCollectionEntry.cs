// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Loading.LoadingCollectionEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Engine.Loading
{
  public class LoadingCollectionEntry
  {
    private readonly IDataReader resultSet;
    private readonly ICollectionPersister persister;
    private readonly object key;
    private readonly IPersistentCollection collection;

    public LoadingCollectionEntry(
      IDataReader resultSet,
      ICollectionPersister persister,
      object key,
      IPersistentCollection collection)
    {
      this.resultSet = resultSet;
      this.persister = persister;
      this.key = key;
      this.collection = collection;
    }

    public IDataReader ResultSet => this.resultSet;

    public ICollectionPersister Persister => this.persister;

    public object Key => this.key;

    public IPersistentCollection Collection => this.collection;

    public override string ToString()
    {
      return this.GetType().FullName + "<rs=" + (object) this.ResultSet + ", coll=" + MessageHelper.InfoString(this.Persister.Role, this.Key) + ">@" + Convert.ToString(this.GetHashCode(), 16);
    }
  }
}
