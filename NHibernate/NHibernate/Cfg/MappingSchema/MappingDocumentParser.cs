// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.MappingDocumentParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.IO;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  public class MappingDocumentParser : IMappingDocumentParser
  {
    private readonly XmlSerializer serializer = new XmlSerializer(typeof (HbmMapping));

    public HbmMapping Parse(Stream stream)
    {
      return stream != null ? (HbmMapping) this.serializer.Deserialize(stream) : throw new ArgumentNullException(nameof (stream));
    }
  }
}
