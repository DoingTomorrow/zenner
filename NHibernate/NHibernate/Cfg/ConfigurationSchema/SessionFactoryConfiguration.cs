// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.SessionFactoryConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class SessionFactoryConfiguration : SessionFactoryConfigurationBase
  {
    internal SessionFactoryConfiguration(XPathNavigator hbConfigurationSection)
    {
      if (hbConfigurationSection == null)
        throw new ArgumentNullException(nameof (hbConfigurationSection));
      this.Parse(hbConfigurationSection);
    }

    public SessionFactoryConfiguration(string name) => this.Name = name;

    private void Parse(XPathNavigator navigator)
    {
      this.ParseName(navigator);
      this.ParseProperties(navigator);
      this.ParseMappings(navigator);
      this.ParseClassesCache(navigator);
      this.ParseCollectionsCache(navigator);
      this.ParseListeners(navigator);
      this.ParseEvents(navigator);
    }

    private void ParseName(XPathNavigator navigator)
    {
      XPathNavigator xpathNavigator = navigator.SelectSingleNode(CfgXmlHelper.SessionFactoryExpression);
      if (xpathNavigator == null || !xpathNavigator.MoveToFirstAttribute())
        return;
      this.Name = xpathNavigator.Value;
    }

    private void ParseProperties(XPathNavigator navigator)
    {
      XPathNodeIterator xpathNodeIterator = navigator.Select(CfgXmlHelper.SessionFactoryPropertiesExpression);
      while (xpathNodeIterator.MoveNext())
      {
        string str = xpathNodeIterator.Current.Value != null ? xpathNodeIterator.Current.Value.Trim() : string.Empty;
        XPathNavigator xpathNavigator = xpathNodeIterator.Current.Clone();
        xpathNavigator.MoveToFirstAttribute();
        string key = xpathNavigator.Value;
        if (!string.IsNullOrEmpty(key))
          this.Properties[key] = str;
      }
    }

    private void ParseMappings(XPathNavigator navigator)
    {
      XPathNodeIterator xpathNodeIterator = navigator.Select(CfgXmlHelper.SessionFactoryMappingsExpression);
      while (xpathNodeIterator.MoveNext())
      {
        MappingConfiguration mappingConfiguration = new MappingConfiguration(xpathNodeIterator.Current);
        if (!mappingConfiguration.IsEmpty())
          this.Mappings.Add(mappingConfiguration);
      }
    }

    private void ParseClassesCache(XPathNavigator navigator)
    {
      XPathNodeIterator xpathNodeIterator = navigator.Select(CfgXmlHelper.SessionFactoryClassesCacheExpression);
      while (xpathNodeIterator.MoveNext())
        this.ClassesCache.Add(new ClassCacheConfiguration(xpathNodeIterator.Current));
    }

    private void ParseCollectionsCache(XPathNavigator navigator)
    {
      XPathNodeIterator xpathNodeIterator = navigator.Select(CfgXmlHelper.SessionFactoryCollectionsCacheExpression);
      while (xpathNodeIterator.MoveNext())
        this.CollectionsCache.Add(new CollectionCacheConfiguration(xpathNodeIterator.Current));
    }

    private void ParseListeners(XPathNavigator navigator)
    {
      XPathNodeIterator xpathNodeIterator = navigator.Select(CfgXmlHelper.SessionFactoryListenersExpression);
      while (xpathNodeIterator.MoveNext())
        this.Listeners.Add(new ListenerConfiguration(xpathNodeIterator.Current));
    }

    private void ParseEvents(XPathNavigator navigator)
    {
      XPathNodeIterator xpathNodeIterator = navigator.Select(CfgXmlHelper.SessionFactoryEventsExpression);
      while (xpathNodeIterator.MoveNext())
        this.Events.Add(new EventConfiguration(xpathNodeIterator.Current));
    }
  }
}
