// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableFieldItem
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableFieldItem : XmlHelper
  {
    private ExcelPivotTableField _field;

    internal ExcelPivotTableFieldItem(
      XmlNamespaceManager ns,
      XmlNode topNode,
      ExcelPivotTableField field)
      : base(ns, topNode)
    {
      this._field = field;
    }

    public string Text
    {
      get => this.GetXmlNodeString("@n");
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          this.DeleteNode("@n");
        }
        else
        {
          foreach (ExcelPivotTableFieldItem pivotTableFieldItem in this._field.Items)
          {
            if (pivotTableFieldItem.Text == value)
              throw new ArgumentException("Duplicate Text");
          }
          this.SetXmlNodeString("@n", value);
        }
      }
    }

    internal int X => this.GetXmlNodeInt("@x");

    internal string T => this.GetXmlNodeString("@t");
  }
}
