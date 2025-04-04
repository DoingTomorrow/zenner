// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmDefinition
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
  [XmlRoot("definition", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmDefinition : HbmBase
  {
    [XmlElement("param")]
    public HbmParam[] param;
    [XmlAttribute]
    public string @class;

    public IDictionary<string, string> FindParameterValues()
    {
      IDictionary<string, string> parameterValues = (IDictionary<string, string>) new Dictionary<string, string>();
      if (this.param != null)
      {
        foreach (HbmParam hbmParam in this.param)
          parameterValues.Add(hbmParam.name, hbmParam.GetText());
      }
      return parameterValues;
    }
  }
}
