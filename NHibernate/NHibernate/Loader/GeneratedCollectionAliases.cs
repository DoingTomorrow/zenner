// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.GeneratedCollectionAliases
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader
{
  public class GeneratedCollectionAliases : ICollectionAliases
  {
    private readonly string suffix;
    private readonly string[] keyAliases;
    private readonly string[] indexAliases;
    private readonly string[] elementAliases;
    private readonly string identifierAlias;
    private readonly IDictionary<string, string[]> userProvidedAliases;

    public GeneratedCollectionAliases(
      IDictionary<string, string[]> userProvidedAliases,
      ICollectionPersister persister,
      string suffix)
    {
      this.suffix = suffix;
      this.userProvidedAliases = userProvidedAliases;
      this.keyAliases = this.GetUserProvidedAliases("key", persister.GetKeyColumnAliases(suffix));
      this.indexAliases = this.GetUserProvidedAliases("index", persister.GetIndexColumnAliases(suffix));
      this.elementAliases = persister.ElementType.IsComponentType ? this.GetUserProvidedCompositeElementAliases(persister.GetElementColumnAliases(suffix)) : this.GetUserProvidedAliases("element", persister.GetElementColumnAliases(suffix));
      this.identifierAlias = this.GetUserProvidedAlias("id", persister.GetIdentifierColumnAlias(suffix));
    }

    public GeneratedCollectionAliases(ICollectionPersister persister, string str)
      : this((IDictionary<string, string[]>) new CollectionHelper.EmptyMapClass<string, string[]>(), persister, str)
    {
    }

    private string[] GetUserProvidedCompositeElementAliases(string[] defaultAliases)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, string[]> userProvidedAlias in (IEnumerable<KeyValuePair<string, string[]>>) this.userProvidedAliases)
      {
        if (userProvidedAlias.Key.StartsWith("element."))
          stringList.AddRange((IEnumerable<string>) userProvidedAlias.Value);
      }
      return stringList.Count <= 0 ? defaultAliases : stringList.ToArray();
    }

    public string[] SuffixedKeyAliases => this.keyAliases;

    public string[] SuffixedIndexAliases => this.indexAliases;

    public string[] SuffixedElementAliases => this.elementAliases;

    public string SuffixedIdentifierAlias => this.identifierAlias;

    public string Suffix => this.suffix;

    public override string ToString()
    {
      return string.Format("{0} [suffix={1}, suffixedKeyAliases=[{2}], suffixedIndexAliases=[{3}], suffixedElementAliases=[{4}], suffixedIdentifierAlias=[{5}]]", (object) base.ToString(), (object) this.suffix, (object) GeneratedCollectionAliases.Join((IEnumerable<string>) this.keyAliases), (object) GeneratedCollectionAliases.Join((IEnumerable<string>) this.indexAliases), (object) GeneratedCollectionAliases.Join((IEnumerable<string>) this.elementAliases), (object) this.identifierAlias);
    }

    private static string Join(IEnumerable<string> aliases)
    {
      return aliases == null ? (string) null : StringHelper.Join(", ", (IEnumerable) aliases);
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
