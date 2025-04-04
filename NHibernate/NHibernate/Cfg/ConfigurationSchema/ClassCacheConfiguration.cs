// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.ClassCacheConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class ClassCacheConfiguration
  {
    private string clazz;
    private string region;
    private EntityCacheUsage usage;
    private ClassCacheInclude include;

    internal ClassCacheConfiguration(XPathNavigator classCacheElement)
    {
      this.Parse(classCacheElement);
    }

    public ClassCacheConfiguration(string clazz, EntityCacheUsage usage)
    {
      this.clazz = !string.IsNullOrEmpty(clazz) ? clazz : throw new ArgumentException("clazz is null or empty.", nameof (clazz));
      this.usage = usage;
    }

    public ClassCacheConfiguration(string clazz, EntityCacheUsage usage, ClassCacheInclude include)
      : this(clazz, usage)
    {
      this.include = include;
    }

    public ClassCacheConfiguration(string clazz, EntityCacheUsage usage, string region)
      : this(clazz, usage)
    {
      this.region = region;
    }

    public ClassCacheConfiguration(
      string clazz,
      EntityCacheUsage usage,
      ClassCacheInclude include,
      string region)
      : this(clazz, usage, include)
    {
      this.region = region;
    }

    private void Parse(XPathNavigator classCacheElement)
    {
      if (!classCacheElement.MoveToFirstAttribute())
        return;
      do
      {
        switch (classCacheElement.Name)
        {
          case "class":
            if (classCacheElement.Value.Trim().Length == 0)
              throw new HibernateConfigException("Invalid class-cache element; the attribute <class> must be assigned with no empty value");
            this.clazz = classCacheElement.Value;
            break;
          case "usage":
            this.usage = EntityCacheUsageParser.Parse(classCacheElement.Value);
            break;
          case "region":
            this.region = classCacheElement.Value;
            break;
          case "include":
            this.include = CfgXmlHelper.ClassCacheIncludeConvertFrom(classCacheElement.Value);
            break;
        }
      }
      while (classCacheElement.MoveToNextAttribute());
    }

    public string Class => this.clazz;

    public string Region => this.region;

    public EntityCacheUsage Usage => this.usage;

    public ClassCacheInclude Include => this.include;
  }
}
