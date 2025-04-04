// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.QueryStatistics
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Text;

#nullable disable
namespace NHibernate.Stat
{
  [Serializable]
  public class QueryStatistics(string categoryName) : CategorizedStatistics(categoryName)
  {
    internal long cacheHitCount;
    internal long cacheMissCount;
    internal long cachePutCount;
    private long executionCount;
    private long executionRowCount;
    private TimeSpan executionAvgTime;
    private TimeSpan executionMaxTime;
    private TimeSpan executionMinTime = TimeSpan.MaxValue;

    public long CacheHitCount => this.cacheHitCount;

    public long CacheMissCount => this.cacheMissCount;

    public long CachePutCount => this.cachePutCount;

    public long ExecutionCount => this.executionCount;

    public long ExecutionRowCount => this.executionRowCount;

    public TimeSpan ExecutionAvgTime => this.executionAvgTime;

    public TimeSpan ExecutionMaxTime => this.executionMaxTime;

    public TimeSpan ExecutionMinTime => this.executionMinTime;

    internal void Executed(long rows, TimeSpan time)
    {
      if (time < this.executionMinTime)
        this.executionMinTime = time;
      if (time > this.executionMaxTime)
        this.executionMaxTime = time;
      ++this.executionCount;
      this.executionRowCount += rows;
      this.executionAvgTime = TimeSpan.FromTicks((this.executionAvgTime.Ticks * this.executionCount + time.Ticks) / this.executionCount);
    }

    public override string ToString()
    {
      return new StringBuilder().Append("QueryStatistics[").Append("cacheHitCount=").Append(this.cacheHitCount).Append(",cacheMissCount=").Append(this.cacheMissCount).Append(",cachePutCount=").Append(this.cachePutCount).Append(",executionCount=").Append(this.executionCount).Append(",executionRowCount=").Append(this.executionRowCount).Append(",executionAvgTime=").Append((object) this.executionAvgTime).Append(",executionMaxTime=").Append((object) this.executionMaxTime).Append(",executionMinTime=").Append((object) this.executionMinTime).Append(']').ToString();
    }
  }
}
