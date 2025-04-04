// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.DetachedQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public class DetachedQuery : AbstractDetachedQuery
  {
    private readonly string hql;

    public DetachedQuery(string hql) => this.hql = hql;

    public string Hql => this.hql;

    public override IQuery GetExecutableQuery(ISession session)
    {
      IQuery query = session.CreateQuery(this.hql);
      this.SetQueryProperties(query);
      return query;
    }

    public DetachedQuery Clone()
    {
      DetachedQuery destination = new DetachedQuery(this.hql);
      this.CopyTo((IDetachedQuery) destination);
      return destination;
    }
  }
}
