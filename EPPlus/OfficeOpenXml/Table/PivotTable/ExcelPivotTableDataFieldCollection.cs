// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableDataFieldCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableDataFieldCollection : 
    ExcelPivotTableFieldCollectionBase<ExcelPivotTableDataField>
  {
    internal ExcelPivotTableDataFieldCollection(ExcelPivotTable table)
      : base(table)
    {
    }

    public ExcelPivotTableDataField Add(ExcelPivotTableField field)
    {
      XmlNode xmlNode = field.TopNode.SelectSingleNode("../../d:dataFields", field.NameSpaceManager);
      if (xmlNode == null)
      {
        this._table.CreateNode("d:dataFields");
        xmlNode = field.TopNode.SelectSingleNode("../../d:dataFields", field.NameSpaceManager);
      }
      XmlElement element = this._table.PivotTableXml.CreateElement("dataField", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element.SetAttribute("fld", field.Index.ToString());
      xmlNode.AppendChild((XmlNode) element);
      field.SetXmlNodeBool("@dataField", true, false);
      ExcelPivotTableDataField dataField = new ExcelPivotTableDataField(field.NameSpaceManager, (XmlNode) element, field);
      this.ValidateDupName(dataField);
      this._list.Add(dataField);
      return dataField;
    }

    private void ValidateDupName(ExcelPivotTableDataField dataField)
    {
      if (!this.ExistsDfName(dataField.Field.Name, (ExcelPivotTableDataField) null))
        return;
      int num = 2;
      string name;
      do
      {
        name = dataField.Field.Name + "_" + num++.ToString();
      }
      while (this.ExistsDfName(name, (ExcelPivotTableDataField) null));
      dataField.Name = name;
    }

    internal bool ExistsDfName(string name, ExcelPivotTableDataField datafield)
    {
      name = name.ToLower();
      foreach (ExcelPivotTableDataField pivotTableDataField in this._list)
      {
        if ((!string.IsNullOrEmpty(pivotTableDataField.Name) && pivotTableDataField.Name.ToLower() == name || string.IsNullOrEmpty(pivotTableDataField.Name) && pivotTableDataField.Field.Name.ToLower() == name) && datafield != pivotTableDataField)
          return true;
      }
      return false;
    }

    public void Remove(ExcelPivotTableDataField dataField)
    {
      if (dataField.Field.TopNode.SelectSingleNode(string.Format("../../d:dataFields/d:dataField[@fld={0}]", (object) dataField.Index), dataField.NameSpaceManager) is XmlElement oldChild)
        oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
      this._list.Remove(dataField);
    }
  }
}
