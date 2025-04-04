// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.CollectionProperties
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal static class CollectionProperties
  {
    public static Dictionary<string, string> HQL_COLLECTION_PROPERTIES;
    private static readonly string COLLECTION_INDEX_LOWER = "index".ToLowerInvariant();

    static CollectionProperties()
    {
      CollectionProperties.HQL_COLLECTION_PROPERTIES = new Dictionary<string, string>();
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("elements".ToLowerInvariant(), "elements");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("indices".ToLowerInvariant(), "indices");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("size".ToLowerInvariant(), "size");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("maxIndex".ToLowerInvariant(), "maxIndex");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("minIndex".ToLowerInvariant(), "minIndex");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("maxElement".ToLowerInvariant(), "maxElement");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add("minElement".ToLowerInvariant(), "minElement");
      CollectionProperties.HQL_COLLECTION_PROPERTIES.Add(CollectionProperties.COLLECTION_INDEX_LOWER, "index");
    }

    public static bool IsCollectionProperty(string name)
    {
      string lowerInvariant = name.ToLowerInvariant();
      return !(CollectionProperties.COLLECTION_INDEX_LOWER == lowerInvariant) && CollectionProperties.HQL_COLLECTION_PROPERTIES.ContainsKey(lowerInvariant);
    }

    public static string GetNormalizedPropertyName(string name)
    {
      return CollectionProperties.HQL_COLLECTION_PROPERTIES[name];
    }

    public static bool IsAnyCollectionProperty(string name)
    {
      string lowerInvariant = name.ToLowerInvariant();
      return CollectionProperties.HQL_COLLECTION_PROPERTIES.ContainsKey(lowerInvariant);
    }
  }
}
