// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmKeyProperty
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
  [DesignerCategory("code")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("key-property", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmKeyProperty : 
    AbstractDecoratable,
    IColumnsMapping,
    ITypeMapping,
    IEntityPropertyMapping,
    IDecoratable
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("column")]
    public HbmColumn[] column;
    public HbmType type;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute("type")]
    public string type1;
    [XmlAttribute("column")]
    public string column1;
    [XmlAttribute(DataType = "positiveInteger")]
    public string length;
    [XmlAttribute]
    public string node;

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
          length = this.length
        };
    }

    public HbmType Type
    {
      get
      {
        HbmType type = this.type;
        if (type != null)
          return type;
        if (string.IsNullOrEmpty(this.type1))
          return (HbmType) null;
        return new HbmType() { name = this.type1 };
      }
    }

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    public string Name => this.name;

    public bool IsLazyProperty => false;

    public string Access => this.access;

    public bool OptimisticLock => false;
  }
}
