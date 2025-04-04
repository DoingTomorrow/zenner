// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.ISqlLoadable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface ISqlLoadable : ILoadable, IEntityPersister, IOptimisticCacheSource
  {
    IType Type { get; }

    string[] GetSubclassPropertyColumnAliases(string propertyName, string suffix);

    string[] GetSubclassPropertyColumnNames(string propertyName);

    string SelectFragment(string alias, string suffix);
  }
}
