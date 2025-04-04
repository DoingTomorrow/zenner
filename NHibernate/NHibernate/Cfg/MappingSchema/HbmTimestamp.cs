// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmTimestamp
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
  [XmlRoot("timestamp", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmTimestamp : AbstractDecoratable, IColumnsMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string node;
    [XmlAttribute]
    public string column;
    [XmlAttribute]
    public string access;
    [XmlAttribute("unsaved-value")]
    public HbmTimestampUnsavedvalue unsavedvalue;
    [XmlIgnore]
    public bool unsavedvalueSpecified;
    [DefaultValue(HbmTimestampSource.Vm)]
    [XmlAttribute]
    public HbmTimestampSource source;
    [XmlAttribute]
    [DefaultValue(HbmVersionGeneration.Never)]
    public HbmVersionGeneration generated;

    public HbmTimestamp()
    {
      this.source = HbmTimestampSource.Vm;
      this.generated = HbmVersionGeneration.Never;
    }

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get
      {
        if (!string.IsNullOrEmpty(this.column))
          yield return new HbmColumn()
          {
            name = this.column
          };
      }
    }
  }
}
