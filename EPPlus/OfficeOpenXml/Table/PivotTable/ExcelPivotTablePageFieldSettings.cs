// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTablePageFieldSettings
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTablePageFieldSettings : XmlHelper
  {
    private ExcelPivotTableField _field;

    internal ExcelPivotTablePageFieldSettings(
      XmlNamespaceManager ns,
      XmlNode topNode,
      ExcelPivotTableField field,
      int index)
      : base(ns, topNode)
    {
      if (this.GetXmlNodeString("@hier") == "")
        this.Hier = -1;
      this._field = field;
    }

    internal int Index
    {
      get => this.GetXmlNodeInt("@fld");
      set => this.SetXmlNodeString("@fld", value.ToString());
    }

    public string Name
    {
      get => this.GetXmlNodeString("@name");
      set => this.SetXmlNodeString("@name", value);
    }

    internal int NumFmtId
    {
      get => this.GetXmlNodeInt("@numFmtId");
      set => this.SetXmlNodeString("@numFmtId", value.ToString());
    }

    internal int Hier
    {
      get => this.GetXmlNodeInt("@hier");
      set => this.SetXmlNodeString("@hier", value.ToString());
    }
  }
}
