// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.ConfigurationExtensions
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate.Cfg;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public static class ConfigurationExtensions
  {
    public static Configuration Configure(
      this Configuration config,
      string fileName,
      string factoryName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);
      return config.Configure((XmlReader) ConfigurationExtensions.PrepareConfiguration(doc, factoryName));
    }

    public static Configuration Configure(
      this Configuration config,
      XmlReader textReader,
      string factoryName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(textReader);
      return config.Configure((XmlReader) ConfigurationExtensions.PrepareConfiguration(doc, factoryName));
    }

    private static XmlTextReader PrepareConfiguration(XmlDocument doc, string factoryName)
    {
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
      nsmgr.AddNamespace("nhibernate", "urn:nhibernate-configuration-2.2-x-factories");
      XmlNode xmlNode = doc.SelectSingleNode("descendant::nhibernate:hibernate-configuration", nsmgr);
      if (xmlNode == null)
        throw new Exception("<hibernate-configuration xmlns=\"urn:nhibernate-configuration-2.2-x-factories\"> element was not found in the configuration file.");
      if (xmlNode.SelectSingleNode("descendant::nhibernate:session-factory[@name='" + factoryName + "']", nsmgr) == null)
        throw new Exception("<session-factory name=\"" + factoryName + "\"> element was not found in the configuration file.");
      foreach (XmlNode selectNode in xmlNode.SelectNodes("descendant::nhibernate:session-factory[@name!='" + factoryName + "']", nsmgr))
        xmlNode.RemoveChild(selectNode);
      return new XmlTextReader((TextReader) new StringReader(xmlNode.OuterXml.Replace("-x-factories", "")));
    }
  }
}
