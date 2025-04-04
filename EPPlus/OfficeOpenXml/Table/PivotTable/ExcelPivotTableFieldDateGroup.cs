// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableFieldDateGroup
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableFieldDateGroup : ExcelPivotTableFieldGroup
  {
    private const string groupByPath = "d:fieldGroup/d:rangePr/@groupBy";

    internal ExcelPivotTableFieldDateGroup(XmlNamespaceManager ns, XmlNode topNode)
      : base(ns, topNode)
    {
    }

    public eDateGroupBy GroupBy
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:fieldGroup/d:rangePr/@groupBy");
        return xmlNodeString != "" ? (eDateGroupBy) Enum.Parse(typeof (eDateGroupBy), xmlNodeString, true) : throw new Exception("Invalid date Groupby");
      }
      private set
      {
        this.SetXmlNodeString("d:fieldGroup/d:rangePr/@groupBy", value.ToString().ToLower());
      }
    }

    public bool AutoStart => this.GetXmlNodeBool("@autoStart", false);

    public bool AutoEnd => this.GetXmlNodeBool("@autoStart", false);
  }
}
