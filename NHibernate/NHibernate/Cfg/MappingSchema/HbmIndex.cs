// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmIndex
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
  [DebuggerStepThrough]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("index", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmIndex : IColumnsMapping, ITypeMapping
  {
    [XmlElement("column")]
    public HbmColumn[] column;
    [XmlAttribute("column")]
    public string column1;
    [XmlAttribute]
    public string type;
    [XmlAttribute(DataType = "positiveInteger")]
    public string length;

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
        if (string.IsNullOrEmpty(this.type))
          return (HbmType) null;
        return new HbmType() { name = this.type };
      }
    }
  }
}
