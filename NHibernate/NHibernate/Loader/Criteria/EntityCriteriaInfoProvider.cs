// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Criteria.EntityCriteriaInfoProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Loader.Criteria
{
  public class EntityCriteriaInfoProvider : ICriteriaInfoProvider
  {
    private readonly IQueryable persister;

    public EntityCriteriaInfoProvider(IQueryable persister) => this.persister = persister;

    public string Name => this.persister.EntityName;

    public string[] Spaces => this.persister.QuerySpaces;

    public IPropertyMapping PropertyMapping => (IPropertyMapping) this.persister;

    public IType GetType(string relativePath) => this.persister.ToType(relativePath);
  }
}
