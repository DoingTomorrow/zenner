// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlCacheWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlCacheWriter : NullMappingModelVisitor, IXmlWriter<CacheMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(CacheMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessCache(CacheMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "cache");
      if (mapping.IsSpecified("Region"))
        element.WithAtt("region", mapping.Region);
      if (!mapping.IsSpecified("Usage"))
        return;
      element.WithAtt("usage", mapping.Usage);
    }
  }
}
