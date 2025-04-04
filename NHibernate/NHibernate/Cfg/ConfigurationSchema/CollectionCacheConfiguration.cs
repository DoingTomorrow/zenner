// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.CollectionCacheConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class CollectionCacheConfiguration
  {
    private string collection;
    private string region;
    private EntityCacheUsage usage;

    internal CollectionCacheConfiguration(XPathNavigator collectionCacheElement)
    {
      this.Parse(collectionCacheElement);
    }

    public CollectionCacheConfiguration(string collection, EntityCacheUsage usage)
    {
      this.collection = !string.IsNullOrEmpty(collection) ? collection : throw new ArgumentException("collection is null or empty.", nameof (collection));
      this.usage = usage;
    }

    public CollectionCacheConfiguration(string collection, EntityCacheUsage usage, string region)
      : this(collection, usage)
    {
      this.region = region;
    }

    private void Parse(XPathNavigator collectionCacheElement)
    {
      if (!collectionCacheElement.MoveToFirstAttribute())
        return;
      do
      {
        switch (collectionCacheElement.Name)
        {
          case "collection":
            if (collectionCacheElement.Value.Trim().Length == 0)
              throw new HibernateConfigException("Invalid collection-cache element; the attribute <collection> must be assigned with no empty value");
            this.collection = collectionCacheElement.Value;
            break;
          case "usage":
            this.usage = EntityCacheUsageParser.Parse(collectionCacheElement.Value);
            break;
          case "region":
            this.region = collectionCacheElement.Value;
            break;
        }
      }
      while (collectionCacheElement.MoveToNextAttribute());
    }

    public string Collection => this.collection;

    public string Region => this.region;

    public EntityCacheUsage Usage => this.usage;
  }
}
