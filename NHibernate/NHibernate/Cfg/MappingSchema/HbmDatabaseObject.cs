// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmDatabaseObject
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
  [XmlRoot("database-object", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmDatabaseObject : HbmBase
  {
    [XmlElement("create", typeof (HbmCreate))]
    [XmlElement("definition", typeof (HbmDefinition))]
    [XmlElement("drop", typeof (HbmDrop))]
    public object[] Items;
    [XmlElement("dialect-scope")]
    public HbmDialectScope[] dialectscope;

    public string FindCreateText() => HbmBase.JoinString(HbmBase.Find<HbmCreate>(this.Items).Text);

    public HbmDefinition FindDefinition() => HbmBase.Find<HbmDefinition>(this.Items);

    public IList<string> FindDialectScopeNames()
    {
      IList<string> dialectScopeNames = (IList<string>) new List<string>();
      if (this.dialectscope != null)
      {
        foreach (HbmDialectScope hbmDialectScope in this.dialectscope)
          dialectScopeNames.Add(hbmDialectScope.name);
      }
      return dialectScopeNames;
    }

    public string FindDropText() => HbmBase.JoinString(HbmBase.Find<HbmDrop>(this.Items).Text);

    public bool HasDefinition() => this.FindDefinition() != null;
  }
}
