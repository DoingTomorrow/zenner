// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmMeta
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
  [DebuggerStepThrough]
  [XmlRoot("meta", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmMeta : HbmBase
  {
    [XmlAttribute]
    public string attribute;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool inherit;
    [XmlText]
    public string[] Text;

    public HbmMeta() => this.inherit = true;

    public string GetText() => HbmBase.JoinString(this.Text);
  }
}
