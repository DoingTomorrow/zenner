// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlSchemas
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

#nullable disable
namespace NHibernate.Cfg
{
  internal class XmlSchemas
  {
    private const string CfgSchemaResource = "NHibernate.nhibernate-configuration.xsd";
    private const string MappingSchemaResource = "NHibernate.nhibernate-mapping.xsd";
    private static readonly XmlSchemaSet ConfigSchemaSet = XmlSchemas.ReadXmlSchemaFromEmbeddedResource("NHibernate.nhibernate-configuration.xsd");
    private static readonly XmlSchemaSet MappingSchemaSet = XmlSchemas.ReadXmlSchemaFromEmbeddedResource("NHibernate.nhibernate-mapping.xsd");

    public XmlReaderSettings CreateConfigReaderSettings()
    {
      XmlReaderSettings xmlReaderSettings = XmlSchemas.CreateXmlReaderSettings(XmlSchemas.ConfigSchemaSet);
      xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(XmlSchemas.ConfigSettingsValidationEventHandler);
      xmlReaderSettings.IgnoreComments = true;
      return xmlReaderSettings;
    }

    public XmlReaderSettings CreateMappingReaderSettings()
    {
      return XmlSchemas.CreateXmlReaderSettings(XmlSchemas.MappingSchemaSet);
    }

    private static XmlSchemaSet ReadXmlSchemaFromEmbeddedResource(string resourceName)
    {
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
      {
        XmlSchema schema = XmlSchema.Read(manifestResourceStream, (ValidationEventHandler) null);
        XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
        xmlSchemaSet.Add(schema);
        xmlSchemaSet.Compile();
        return xmlSchemaSet;
      }
    }

    private static XmlReaderSettings CreateXmlReaderSettings(XmlSchemaSet xmlSchemaSet)
    {
      return new XmlReaderSettings()
      {
        ValidationType = ValidationType.Schema,
        Schemas = xmlSchemaSet
      };
    }

    private static void ConfigSettingsValidationEventHandler(object sender, ValidationEventArgs e)
    {
      throw new HibernateConfigException("An exception occurred parsing configuration :" + e.Message, (Exception) e.Exception);
    }
  }
}
