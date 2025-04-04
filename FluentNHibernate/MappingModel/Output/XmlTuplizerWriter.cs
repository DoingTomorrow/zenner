// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlTuplizerWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlTuplizerWriter : NullMappingModelVisitor, IXmlWriter<TuplizerMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(TuplizerMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessTuplizer(TuplizerMapping tuplizerMapping)
    {
      this.document = new XmlDocument();
      XmlElement element = this.document.CreateElement("tuplizer");
      if (tuplizerMapping.IsSpecified("Mode"))
        element.WithAtt("entity-mode", XmlTuplizerWriter.GetModeString(tuplizerMapping.Mode));
      if (tuplizerMapping.IsSpecified("Type"))
        element.WithAtt("class", tuplizerMapping.Type);
      if (tuplizerMapping.IsSpecified("EntityName"))
        element.WithAtt("entity-name", tuplizerMapping.EntityName);
      this.document.AppendChild((XmlNode) element);
    }

    private static string GetModeString(TuplizerMode mode)
    {
      switch (mode)
      {
        case TuplizerMode.Poco:
          return "poco";
        case TuplizerMode.Xml:
          return "xml";
        case TuplizerMode.DynamicMap:
          return "dynamic-map";
        default:
          throw new ArgumentException(string.Format("Unknown tuplizer entity mode '{0}'.", (object) mode));
      }
    }
  }
}
