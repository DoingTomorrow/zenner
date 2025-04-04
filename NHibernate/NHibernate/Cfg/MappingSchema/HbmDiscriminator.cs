// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmDiscriminator
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
  [XmlRoot("discriminator", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmDiscriminator : IColumnsMapping, IFormulasMapping
  {
    [XmlElement("column", typeof (HbmColumn))]
    [XmlElement("formula", typeof (HbmFormula))]
    public object Item;
    [XmlAttribute]
    public string column;
    [XmlAttribute]
    public string formula;
    [XmlAttribute]
    [DefaultValue("string")]
    public string type;
    [XmlAttribute("not-null")]
    [DefaultValue(true)]
    public bool notnull;
    [XmlAttribute(DataType = "positiveInteger")]
    public string length;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool force;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool insert;

    public HbmDiscriminator()
    {
      this.type = "string";
      this.notnull = true;
      this.force = false;
      this.insert = true;
    }

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get
      {
        if (this.Item is HbmColumn)
          yield return (HbmColumn) this.Item;
        else if (!string.IsNullOrEmpty(this.column))
          yield return new HbmColumn()
          {
            name = this.column,
            length = this.length,
            notnull = this.notnull,
            notnullSpecified = true
          };
      }
    }

    [XmlIgnore]
    public IEnumerable<HbmFormula> Formulas
    {
      get
      {
        if (this.Item is HbmFormula)
          yield return (HbmFormula) this.Item;
        else if (!string.IsNullOrEmpty(this.formula))
          yield return new HbmFormula()
          {
            Text = new string[1]{ this.formula }
          };
      }
    }
  }
}
