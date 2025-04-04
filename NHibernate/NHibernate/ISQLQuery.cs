// Decompiled with JetBrains decompiler
// Type: NHibernate.ISQLQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate
{
  public interface ISQLQuery : IQuery
  {
    ISQLQuery AddEntity(string entityName);

    ISQLQuery AddEntity(string alias, string entityName);

    ISQLQuery AddEntity(string alias, string entityName, LockMode lockMode);

    ISQLQuery AddEntity(System.Type entityClass);

    ISQLQuery AddEntity(string alias, System.Type entityClass);

    ISQLQuery AddEntity(string alias, System.Type entityClass, LockMode lockMode);

    ISQLQuery AddJoin(string alias, string path);

    ISQLQuery AddJoin(string alias, string path, LockMode lockMode);

    ISQLQuery AddScalar(string columnAlias, IType type);

    ISQLQuery SetResultSetMapping(string name);
  }
}
