// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.EntityStatistics
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Text;

#nullable disable
namespace NHibernate.Stat
{
  [Serializable]
  public class EntityStatistics : CategorizedStatistics
  {
    internal long loadCount;
    internal long updateCount;
    internal long insertCount;
    internal long deleteCount;
    internal long fetchCount;
    internal long optimisticFailureCount;

    internal EntityStatistics(string categoryName)
      : base(categoryName)
    {
    }

    public long LoadCount => this.loadCount;

    public long UpdateCount => this.updateCount;

    public long InsertCount => this.insertCount;

    public long DeleteCount => this.deleteCount;

    public long FetchCount => this.fetchCount;

    public long OptimisticFailureCount => this.optimisticFailureCount;

    public override string ToString()
    {
      return new StringBuilder().Append("EntityStatistics[").Append("loadCount=").Append(this.loadCount).Append(",updateCount=").Append(this.updateCount).Append(",insertCount=").Append(this.insertCount).Append(",deleteCount=").Append(this.deleteCount).Append(",fetchCount=").Append(this.fetchCount).Append(",optimisticLockFailureCount=").Append(this.optimisticFailureCount).Append(']').ToString();
    }
  }
}
