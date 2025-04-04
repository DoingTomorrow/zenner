// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.CollectionStatistics
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Text;

#nullable disable
namespace NHibernate.Stat
{
  [Serializable]
  public class CollectionStatistics : CategorizedStatistics
  {
    internal long loadCount;
    internal long fetchCount;
    internal long updateCount;
    internal long removeCount;
    internal long recreateCount;

    internal CollectionStatistics(string categoryName)
      : base(categoryName)
    {
    }

    public long LoadCount => this.loadCount;

    public long FetchCount => this.fetchCount;

    public long UpdateCount => this.updateCount;

    public long RemoveCount => this.removeCount;

    public long RecreateCount => this.recreateCount;

    public override string ToString()
    {
      return new StringBuilder().Append("CollectionStatistics[").Append("loadCount=").Append(this.loadCount).Append(",fetchCount=").Append(this.fetchCount).Append(",recreateCount=").Append(this.recreateCount).Append(",removeCount=").Append(this.removeCount).Append(",updateCount=").Append(this.updateCount).Append(']').ToString();
    }
  }
}
