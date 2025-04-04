// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.SessionStatisticsImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Stat
{
  public class SessionStatisticsImpl : ISessionStatistics
  {
    private readonly ISessionImplementor session;

    public SessionStatisticsImpl(ISessionImplementor session) => this.session = session;

    public int EntityCount => this.session.PersistenceContext.EntityEntries.Count;

    public int CollectionCount => this.session.PersistenceContext.CollectionEntries.Count;

    public IList<EntityKey> EntityKeys
    {
      get
      {
        List<EntityKey> entityKeyList = new List<EntityKey>();
        entityKeyList.AddRange((IEnumerable<EntityKey>) this.session.PersistenceContext.EntitiesByKey.Keys);
        return (IList<EntityKey>) entityKeyList.AsReadOnly();
      }
    }

    public IList<CollectionKey> CollectionKeys
    {
      get
      {
        List<CollectionKey> collectionKeyList = new List<CollectionKey>();
        collectionKeyList.AddRange((IEnumerable<CollectionKey>) this.session.PersistenceContext.CollectionsByKey.Keys);
        return (IList<CollectionKey>) collectionKeyList.AsReadOnly();
      }
    }

    public override string ToString()
    {
      return new StringBuilder().Append("SessionStatistics[").Append("entity count=").Append(this.EntityCount).Append("collection count=").Append(this.CollectionCount).Append(']').ToString();
    }
  }
}
