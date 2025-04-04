// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.HibernateConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class HibernateConfiguration : IHibernateConfiguration
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (HibernateConfiguration));
    private string byteCodeProviderType = BytecodeProviderType.Lcg.ToConfigurationString();
    private bool useReflectionOptimizer = true;
    private SessionFactoryConfiguration sessionFactory;

    public HibernateConfiguration(XmlReader hbConfigurationReader)
      : this(hbConfigurationReader, false)
    {
    }

    private HibernateConfiguration(XmlReader hbConfigurationReader, bool fromAppSetting)
    {
      XPathNavigator navigator;
      try
      {
        navigator = new XPathDocument(XmlReader.Create(hbConfigurationReader, this.GetSettings())).CreateNavigator();
      }
      catch (HibernateConfigException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new HibernateConfigException(ex);
      }
      this.Parse(navigator, fromAppSetting);
    }

    internal static HibernateConfiguration FromAppConfig(XmlNode node)
    {
      return new HibernateConfiguration((XmlReader) new XmlTextReader(node.OuterXml, XmlNodeType.Document, (XmlParserContext) null), true);
    }

    private XmlReaderSettings GetSettings() => new XmlSchemas().CreateConfigReaderSettings();

    private void Parse(XPathNavigator navigator, bool fromAppConfig)
    {
      this.ParseByteCodeProvider(navigator, fromAppConfig);
      this.ParseReflectionOptimizer(navigator, fromAppConfig);
      if (navigator.SelectSingleNode(CfgXmlHelper.SessionFactoryExpression) != null)
        this.sessionFactory = new SessionFactoryConfiguration(navigator);
      else if (!fromAppConfig)
        throw new HibernateConfigException("<session-factory xmlns='urn:nhibernate-configuration-2.2'> element was not found in the configuration file.");
    }

    private void ParseByteCodeProvider(XPathNavigator navigator, bool fromAppConfig)
    {
      XPathNavigator xpathNavigator = navigator.SelectSingleNode(CfgXmlHelper.ByteCodeProviderExpression);
      if (xpathNavigator == null)
        return;
      if (fromAppConfig)
      {
        xpathNavigator.MoveToFirstAttribute();
        this.byteCodeProviderType = xpathNavigator.Value;
      }
      else
        HibernateConfiguration.LogWarnIgnoredProperty("bytecode-provider");
    }

    private static void LogWarnIgnoredProperty(string propName)
    {
      if (!HibernateConfiguration.log.IsWarnEnabled)
        return;
      HibernateConfiguration.log.Warn((object) string.Format("{0} property is ignored out of application configuration file.", (object) propName));
    }

    private void ParseReflectionOptimizer(XPathNavigator navigator, bool fromAppConfig)
    {
      XPathNavigator xpathNavigator = navigator.SelectSingleNode(CfgXmlHelper.ReflectionOptimizerExpression);
      if (xpathNavigator == null)
        return;
      if (fromAppConfig)
      {
        xpathNavigator.MoveToFirstAttribute();
        this.useReflectionOptimizer = xpathNavigator.ValueAsBoolean;
      }
      else
        HibernateConfiguration.LogWarnIgnoredProperty("reflection-optimizer");
    }

    public string ByteCodeProviderType => this.byteCodeProviderType;

    public bool UseReflectionOptimizer => this.useReflectionOptimizer;

    public ISessionFactoryConfiguration SessionFactory
    {
      get => (ISessionFactoryConfiguration) this.sessionFactory;
    }
  }
}
