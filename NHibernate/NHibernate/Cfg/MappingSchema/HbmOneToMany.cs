// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmOneToMany
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [XmlRoot("one-to-many", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmOneToMany : IRelationship
  {
    [XmlAttribute]
    public string @class;
    [XmlAttribute("not-found")]
    [DefaultValue(HbmNotFoundMode.Exception)]
    public HbmNotFoundMode notfound;
    [XmlAttribute]
    public string node;
    [XmlAttribute("embed-xml")]
    [DefaultValue(true)]
    public bool embedxml;
    [XmlAttribute("entity-name")]
    public string entityname;

    public HbmOneToMany()
    {
      this.notfound = HbmNotFoundMode.Exception;
      this.embedxml = true;
    }

    public string EntityName => this.entityname;

    public string Class => this.@class;

    public HbmNotFoundMode NotFoundMode => this.notfound;
  }
}
