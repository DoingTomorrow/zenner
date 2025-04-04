// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmListIndex
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
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlRoot("list-index", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmListIndex : IColumnsMapping
  {
    public HbmColumn column;
    [XmlAttribute("column")]
    public string column1;
    [XmlAttribute(DataType = "positiveInteger")]
    public string @base;

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get
      {
        if (this.column != null)
          yield return this.column;
        else if (!string.IsNullOrEmpty(this.column1))
          yield return new HbmColumn()
          {
            name = this.column1
          };
      }
    }
  }
}
