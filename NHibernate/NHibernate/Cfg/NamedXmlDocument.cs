// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.NamedXmlDocument
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg
{
  public class NamedXmlDocument
  {
    private readonly string name;
    private readonly HbmMapping document;

    public NamedXmlDocument(
      string name,
      XmlDocument document,
      XmlSerializer mappingDocumentSerializer)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      this.name = name;
      if (document.DocumentElement == null)
        throw new MappingException("Empty XML document:" + name);
      using (StringReader stringReader = new StringReader(document.DocumentElement.OuterXml))
        this.document = (HbmMapping) mappingDocumentSerializer.Deserialize((TextReader) stringReader);
    }

    public string Name => this.name;

    public HbmMapping Document => this.document;
  }
}
