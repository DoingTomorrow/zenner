// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmVersion
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [XmlRoot("version", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmVersion : AbstractDecoratable, IColumnsMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("column")]
    public HbmColumn[] column;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string node;
    [XmlAttribute]
    public string access;
    [XmlAttribute("column")]
    public string column1;
    [DefaultValue("Int32")]
    [XmlAttribute]
    public string type;
    [XmlAttribute("unsaved-value")]
    public string unsavedvalue;
    [XmlAttribute]
    [DefaultValue(HbmVersionGeneration.Never)]
    public HbmVersionGeneration generated;
    [XmlAttribute]
    public bool insert;
    [XmlIgnore]
    public bool insertSpecified;

    public HbmVersion()
    {
      this.type = "Int32";
      this.generated = HbmVersionGeneration.Never;
    }

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get => (IEnumerable<HbmColumn>) this.column ?? this.AsColumns();
    }

    private IEnumerable<HbmColumn> AsColumns()
    {
      if (!string.IsNullOrEmpty(this.column1))
        yield return new HbmColumn() { name = this.column1 };
    }
  }
}
