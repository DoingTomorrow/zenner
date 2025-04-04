// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.ICollectionAliases
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Loader
{
  public interface ICollectionAliases
  {
    string[] SuffixedKeyAliases { get; }

    string[] SuffixedIndexAliases { get; }

    string[] SuffixedElementAliases { get; }

    string SuffixedIdentifierAlias { get; }

    string Suffix { get; }
  }
}
