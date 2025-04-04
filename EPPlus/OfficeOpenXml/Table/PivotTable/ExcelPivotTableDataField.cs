// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableDataField
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style.XmlAccess;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableDataField : XmlHelper
  {
    internal ExcelPivotTableDataField(
      XmlNamespaceManager ns,
      XmlNode topNode,
      ExcelPivotTableField field)
      : base(ns, topNode)
    {
      if (topNode.Attributes.Count == 0)
      {
        this.Index = field.Index;
        this.BaseField = 0;
        this.BaseItem = 0;
      }
      this.Field = field;
    }

    public ExcelPivotTableField Field { get; private set; }

    public int Index
    {
      get => this.GetXmlNodeInt("@fld");
      internal set => this.SetXmlNodeString("@fld", value.ToString());
    }

    public string Name
    {
      get => this.GetXmlNodeString("@name");
      set
      {
        if (this.Field._table.DataFields.ExistsDfName(value, this))
          throw new InvalidOperationException("Duplicate datafield name");
        this.SetXmlNodeString("@name", value);
      }
    }

    public int BaseField
    {
      get => this.GetXmlNodeInt("@baseField");
      set => this.SetXmlNodeString("@baseField", value.ToString());
    }

    public int BaseItem
    {
      get => this.GetXmlNodeInt("@baseItem");
      set => this.SetXmlNodeString("@baseItem", value.ToString());
    }

    internal int NumFmtId
    {
      get => this.GetXmlNodeInt("@numFmtId");
      set => this.SetXmlNodeString("@numFmtId", value.ToString());
    }

    public string Format
    {
      get
      {
        foreach (ExcelNumberFormatXml numberFormat in this.Field._table.WorkSheet.Workbook.Styles.NumberFormats)
        {
          if (numberFormat.NumFmtId == this.NumFmtId)
            return numberFormat.Format;
        }
        return this.Field._table.WorkSheet.Workbook.Styles.NumberFormats[0].Format;
      }
      set
      {
        ExcelStyles styles = this.Field._table.WorkSheet.Workbook.Styles;
        ExcelNumberFormatXml excelNumberFormatXml = (ExcelNumberFormatXml) null;
        if (!styles.NumberFormats.FindByID(value, ref excelNumberFormatXml))
        {
          excelNumberFormatXml = new ExcelNumberFormatXml(this.NameSpaceManager)
          {
            Format = value,
            NumFmtId = styles.NumberFormats.NextId++
          };
          styles.NumberFormats.Add(value, excelNumberFormatXml);
        }
        this.NumFmtId = excelNumberFormatXml.NumFmtId;
      }
    }

    public DataFieldFunctions Function
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("@subtotal");
        return xmlNodeString == "" ? DataFieldFunctions.None : (DataFieldFunctions) Enum.Parse(typeof (DataFieldFunctions), xmlNodeString, true);
      }
      set
      {
        string str;
        switch (value)
        {
          case DataFieldFunctions.CountNums:
            str = "CountNums";
            break;
          case DataFieldFunctions.None:
            this.DeleteNode("@subtotal");
            return;
          case DataFieldFunctions.StdDev:
            str = "stdDev";
            break;
          case DataFieldFunctions.StdDevP:
            str = "stdDevP";
            break;
          default:
            str = value.ToString().ToLower();
            break;
        }
        this.SetXmlNodeString("@subtotal", str);
      }
    }
  }
}
