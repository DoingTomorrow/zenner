// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmMapKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlRoot("map-key", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmMapKey : IColumnsMapping, ITypeMapping
  {
    [XmlElement("formula", typeof (HbmFormula))]
    [XmlElement("column", typeof (HbmColumn))]
    public object[] Items;
    [XmlAttribute]
    public string column;
    [XmlAttribute]
    public string formula;
    [XmlAttribute]
    public string type;
    [XmlAttribute(DataType = "positiveInteger")]
    public string length;
    [XmlAttribute]
    public string node;

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get => this.Items == null ? this.AsColumns() : this.Items.OfType<HbmColumn>();
    }

    private IEnumerable<HbmColumn> AsColumns()
    {
      if (!string.IsNullOrEmpty(this.column))
        yield return new HbmColumn()
        {
          name = this.column,
          length = this.length
        };
    }

    [XmlIgnore]
    public IEnumerable<HbmFormula> Formulas
    {
      get => this.Items == null ? this.AsFormulas() : this.Items.OfType<HbmFormula>();
    }

    private IEnumerable<HbmFormula> AsFormulas()
    {
      if (!string.IsNullOrEmpty(this.formula))
        yield return new HbmFormula()
        {
          Text = new string[1]{ this.formula }
        };
    }

    public HbmType Type
    {
      get
      {
        if (string.IsNullOrEmpty(this.type))
          return (HbmType) null;
        return new HbmType() { name = this.type };
      }
    }
  }
}
