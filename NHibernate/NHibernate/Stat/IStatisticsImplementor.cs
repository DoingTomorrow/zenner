// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.IStatisticsImplementor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Stat
{
  public interface IStatisticsImplementor
  {
    void OpenSession();

    void CloseSession();

    void Flush();

    void Connect();

    void LoadEntity(string entityName, TimeSpan time);

    void FetchEntity(string entityName, TimeSpan time);

    void UpdateEntity(string entityName, TimeSpan time);

    void InsertEntity(string entityName, TimeSpan time);

    void DeleteEntity(string entityName, TimeSpan time);

    void LoadCollection(string role, TimeSpan time);

    void FetchCollection(string role, TimeSpan time);

    void UpdateCollection(string role, TimeSpan time);

    void RecreateCollection(string role, TimeSpan time);

    void RemoveCollection(string role, TimeSpan time);

    void SecondLevelCachePut(string regionName);

    void SecondLevelCacheHit(string regionName);

    void SecondLevelCacheMiss(string regionName);

    void QueryExecuted(string hql, int rows, TimeSpan time);

    void QueryCacheHit(string hql, string regionName);

    void QueryCacheMiss(string hql, string regionName);

    void QueryCachePut(string hql, string regionName);

    void EndTransaction(bool success);

    void CloseStatement();

    void PrepareStatement();

    void OptimisticFailure(string entityName);
  }
}
