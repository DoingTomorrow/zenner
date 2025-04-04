// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmAny
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
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("any", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmAny : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IColumnsMapping,
    IAnyMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("meta-value")]
    public HbmMetaValue[] metavalue;
    [XmlElement("column")]
    public HbmColumn[] column;
    [XmlAttribute("column")]
    public string column1;
    [XmlAttribute("id-type")]
    public string idtype;
    [XmlAttribute("meta-type")]
    public string metatype;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool insert;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool update;
    [XmlAttribute]
    public string cascade;
    [XmlAttribute]
    public string index;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(true)]
    public bool optimisticlock;
    [XmlAttribute]
    [DefaultValue(false)]
    public bool lazy;
    [XmlAttribute]
    public string node;

    public HbmAny()
    {
      this.insert = true;
      this.update = true;
      this.optimisticlock = true;
      this.lazy = false;
    }

    public string Name => this.name;

    public string Access => this.access;

    public bool OptimisticLock => this.optimisticlock;

    public bool IsLazyProperty => this.lazy;

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get => (IEnumerable<HbmColumn>) this.column ?? this.AsColumns();
    }

    private IEnumerable<HbmColumn> AsColumns()
    {
      if (!string.IsNullOrEmpty(this.column1))
        yield return new HbmColumn()
        {
          name = this.column1,
          index = this.index
        };
    }

    public string MetaType => this.metatype;

    public ICollection<HbmMetaValue> MetaValues
    {
      get
      {
        return (ICollection<HbmMetaValue>) this.metavalue ?? (ICollection<HbmMetaValue>) new HbmMetaValue[0];
      }
    }
  }
}
