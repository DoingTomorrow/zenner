// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableFieldNumericGroup
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableFieldNumericGroup : ExcelPivotTableFieldGroup
  {
    private const string startPath = "d:fieldGroup/d:rangePr/@startNum";
    private const string endPath = "d:fieldGroup/d:rangePr/@endNum";
    private const string groupIntervalPath = "d:fieldGroup/d:rangePr/@groupInterval";

    internal ExcelPivotTableFieldNumericGroup(XmlNamespaceManager ns, XmlNode topNode)
      : base(ns, topNode)
    {
    }

    public double Start
    {
      get => this.GetXmlNodeDoubleNull("d:fieldGroup/d:rangePr/@startNum").Value;
      private set
      {
        this.SetXmlNodeString("d:fieldGroup/d:rangePr/@startNum", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double End
    {
      get => this.GetXmlNodeDoubleNull("d:fieldGroup/d:rangePr/@endNum").Value;
      private set
      {
        this.SetXmlNodeString("d:fieldGroup/d:rangePr/@endNum", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double Interval
    {
      get => this.GetXmlNodeDoubleNull("d:fieldGroup/d:rangePr/@groupInterval").Value;
      private set
      {
        this.SetXmlNodeString("d:fieldGroup/d:rangePr/@groupInterval", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }
  }
}
