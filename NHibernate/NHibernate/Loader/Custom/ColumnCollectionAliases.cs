// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.ColumnCollectionAliases
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Custom
{
  public class ColumnCollectionAliases : ICollectionAliases
  {
    private readonly string[] keyAliases;
    private readonly string[] indexAliases;
    private readonly string[] elementAliases;
    private readonly string identifierAlias;
    private readonly IDictionary<string, string[]> userProvidedAliases;

    public ColumnCollectionAliases(
      IDictionary<string, string[]> userProvidedAliases,
      ISqlLoadableCollection persister)
    {
      this.userProvidedAliases = userProvidedAliases;
      this.keyAliases = this.GetUserProvidedAliases("key", persister.KeyColumnNames);
      this.indexAliases = this.GetUserProvidedAliases("index", persister.IndexColumnNames);
      this.elementAliases = this.GetUserProvidedAliases("element", persister.ElementColumnNames);
      this.identifierAlias = this.GetUserProvidedAlias("id", persister.IdentifierColumnName);
    }

    public string[] SuffixedKeyAliases => this.keyAliases;

    public string[] SuffixedIndexAliases => this.indexAliases;

    public string[] SuffixedElementAliases => this.elementAliases;

    public string SuffixedIdentifierAlias => this.identifierAlias;

    public string Suffix => string.Empty;

    public override string ToString()
    {
      return base.ToString() + " [ suffixedKeyAliases=[" + ColumnCollectionAliases.Join((IEnumerable) this.keyAliases) + "], suffixedIndexAliases=[" + ColumnCollectionAliases.Join((IEnumerable) this.indexAliases) + "], suffixedElementAliases=[" + ColumnCollectionAliases.Join((IEnumerable) this.elementAliases) + "], suffixedIdentifierAlias=[" + this.identifierAlias + "]]";
    }

    private static string Join(IEnumerable aliases)
    {
      return aliases == null ? (string) null : StringHelper.Join(", ", aliases);
    }

    private string[] GetUserProvidedAliases(string propertyPath, string[] defaultAliases)
    {
      string[] strArray;
      return !this.userProvidedAliases.TryGetValue(propertyPath, out strArray) ? defaultAliases : strArray;
    }

    private string GetUserProvidedAlias(string propertyPath, string defaultAlias)
    {
      string[] strArray;
      return !this.userProvidedAliases.TryGetValue(propertyPath, out strArray) ? defaultAlias : strArray[0];
    }
  }
}
