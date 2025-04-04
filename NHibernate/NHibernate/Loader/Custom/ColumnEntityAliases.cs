// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.ColumnEntityAliases
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Custom
{
  public class ColumnEntityAliases(
    IDictionary<string, string[]> returnProperties,
    ILoadable persister,
    string suffix) : DefaultEntityAliases(returnProperties, persister, suffix)
  {
    protected override string[] GetIdentifierAliases(ILoadable persister, string suffix)
    {
      return persister.IdentifierColumnNames;
    }

    protected override string GetDiscriminatorAlias(ILoadable persister, string suffix)
    {
      return persister.DiscriminatorColumnName;
    }

    protected override string[] GetPropertyAliases(ILoadable persister, int j)
    {
      return persister.GetPropertyColumnNames(j);
    }
  }
}
