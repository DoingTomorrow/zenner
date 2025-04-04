// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.DefaultEntityAliases
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader
{
  public class DefaultEntityAliases : IEntityAliases
  {
    private readonly string[] suffixedKeyColumns;
    private readonly string[] suffixedVersionColumn;
    private readonly string[][] suffixedPropertyColumns;
    private readonly string suffixedDiscriminatorColumn;
    private readonly string suffix;
    private readonly string rowIdAlias;
    private readonly IDictionary<string, string[]> userProvidedAliases;

    public DefaultEntityAliases(ILoadable persister, string suffix)
      : this((IDictionary<string, string[]>) new CollectionHelper.EmptyMapClass<string, string[]>(), persister, suffix)
    {
    }

    public DefaultEntityAliases(
      IDictionary<string, string[]> userProvidedAliases,
      ILoadable persister,
      string suffix)
    {
      this.suffix = suffix;
      this.userProvidedAliases = userProvidedAliases;
      this.suffixedKeyColumns = this.GetUserProvidedAliases(persister.IdentifierPropertyName, (string[]) null) ?? this.GetUserProvidedAliases(EntityPersister.EntityID, this.GetIdentifierAliases(persister, suffix));
      DefaultEntityAliases.Intern(this.suffixedKeyColumns);
      this.suffixedPropertyColumns = this.GetSuffixedPropertyAliases(persister);
      this.suffixedDiscriminatorColumn = this.GetUserProvidedAlias("class", this.GetDiscriminatorAlias(persister, suffix));
      this.suffixedVersionColumn = !((IEntityPersister) persister).IsVersioned ? (string[]) null : this.suffixedPropertyColumns[persister.VersionProperty];
      this.rowIdAlias = Loadable.RowIdAlias + suffix;
    }

    protected virtual string GetDiscriminatorAlias(ILoadable persister, string suffix)
    {
      return persister.GetDiscriminatorAlias(suffix);
    }

    protected virtual string[] GetIdentifierAliases(ILoadable persister, string suffix)
    {
      return persister.GetIdentifierAliases(suffix);
    }

    protected virtual string[] GetPropertyAliases(ILoadable persister, int j)
    {
      return persister.GetPropertyAliases(this.suffix, j);
    }

    private string[] GetUserProvidedAliases(string propertyPath, string[] defaultAliases)
    {
      return (propertyPath == null ? (string[]) null : this.GetUserProvidedAlias(propertyPath)) ?? defaultAliases;
    }

    private string[] GetUserProvidedAlias(string propertyPath)
    {
      string[] userProvidedAlias;
      this.userProvidedAliases.TryGetValue(propertyPath, out userProvidedAlias);
      return userProvidedAlias;
    }

    private string GetUserProvidedAlias(string propertyPath, string defaultAlias)
    {
      string[] userProvidedAlias = propertyPath == null ? (string[]) null : this.GetUserProvidedAlias(propertyPath);
      return userProvidedAlias == null ? defaultAlias : userProvidedAlias[0];
    }

    public string[][] GetSuffixedPropertyAliases(ILoadable persister)
    {
      int length = persister.PropertyNames.Length;
      string[][] suffixedPropertyAliases = new string[length][];
      for (int j = 0; j < length; ++j)
      {
        suffixedPropertyAliases[j] = this.GetUserProvidedAliases(persister.PropertyNames[j], this.GetPropertyAliases(persister, j));
        DefaultEntityAliases.Intern(suffixedPropertyAliases[j]);
      }
      return suffixedPropertyAliases;
    }

    public string[] SuffixedVersionAliases => this.suffixedVersionColumn;

    public string[][] SuffixedPropertyAliases => this.suffixedPropertyColumns;

    public string SuffixedDiscriminatorAlias => this.suffixedDiscriminatorColumn;

    public string[] SuffixedKeyAliases => this.suffixedKeyColumns;

    public string RowIdAlias => this.rowIdAlias;

    private static void Intern(string[] strings)
    {
      for (int index = 0; index < strings.Length; ++index)
        strings[index] = StringHelper.InternedIfPossible(strings[index]);
    }
  }
}
