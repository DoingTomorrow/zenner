// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Criteria.ScalarCollectionCriteriaInfoProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Util;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Loader.Criteria
{
  public class ScalarCollectionCriteriaInfoProvider : ICriteriaInfoProvider
  {
    private readonly string role;
    private readonly IQueryableCollection persister;
    private readonly SessionFactoryHelper helper;

    public ScalarCollectionCriteriaInfoProvider(SessionFactoryHelper helper, string role)
    {
      this.role = role;
      this.helper = helper;
      this.persister = helper.RequireQueryableCollection(role);
    }

    public string Name => this.role;

    public string[] Spaces => this.persister.CollectionSpaces;

    public IPropertyMapping PropertyMapping => this.helper.GetCollectionPropertyMapping(this.role);

    public IType GetType(string relativePath) => this.PropertyMapping.ToType(relativePath);
  }
}
