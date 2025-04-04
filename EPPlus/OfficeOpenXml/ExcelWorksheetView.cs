// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelWorksheetView
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelWorksheetView : XmlHelper
  {
    private ExcelWorksheet _worksheet;
    private XmlElement _selectionNode;
    private string _paneNodePath = "d:pane";
    private string _selectionNodePath = "d:selection";

    internal ExcelWorksheetView(XmlNamespaceManager ns, XmlNode node, ExcelWorksheet xlWorksheet)
      : base(ns, node)
    {
      this._worksheet = xlWorksheet;
      this.SchemaNodeOrder = new string[4]
      {
        "sheetViews",
        "sheetView",
        "pane",
        "selection"
      };
      this.Panes = this.LoadPanes();
    }

    private ExcelWorksheetView.ExcelWorksheetPanes[] LoadPanes()
    {
      XmlNodeList xmlNodeList = this.TopNode.SelectNodes("//d:selection", this.NameSpaceManager);
      if (xmlNodeList.Count == 0)
        return new ExcelWorksheetView.ExcelWorksheetPanes[1]
        {
          new ExcelWorksheetView.ExcelWorksheetPanes(this.NameSpaceManager, this.TopNode)
        };
      ExcelWorksheetView.ExcelWorksheetPanes[] excelWorksheetPanesArray = new ExcelWorksheetView.ExcelWorksheetPanes[xmlNodeList.Count];
      int num = 0;
      foreach (XmlElement topNode in xmlNodeList)
        excelWorksheetPanesArray[num++] = new ExcelWorksheetView.ExcelWorksheetPanes(this.NameSpaceManager, (XmlNode) topNode);
      return excelWorksheetPanesArray;
    }

    protected internal XmlElement SheetViewElement => (XmlElement) this.TopNode;

    private XmlElement SelectionNode
    {
      get
      {
        this._selectionNode = this.SheetViewElement.SelectSingleNode("//d:selection", this._worksheet.NameSpaceManager) as XmlElement;
        if (this._selectionNode == null)
        {
          this._selectionNode = this._worksheet.WorksheetXml.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
          this.SheetViewElement.AppendChild((XmlNode) this._selectionNode);
        }
        return this._selectionNode;
      }
    }

    public string ActiveCell
    {
      get => this.Panes[this.Panes.GetUpperBound(0)].ActiveCell;
      set => this.Panes[this.Panes.GetUpperBound(0)].ActiveCell = value;
    }

    public string SelectedRange
    {
      get => this.Panes[this.Panes.GetUpperBound(0)].SelectedRange;
      set => this.Panes[this.Panes.GetUpperBound(0)].SelectedRange = value;
    }

    public bool TabSelected
    {
      get => this.GetXmlNodeBool("@tabSelected");
      set
      {
        if (value)
        {
          foreach (ExcelWorksheet worksheet in this._worksheet._package.Workbook.Worksheets)
            worksheet.View.TabSelected = false;
          this.SheetViewElement.SetAttribute("tabSelected", "1");
          if (!(this._worksheet.Workbook.WorkbookXml.SelectSingleNode("//d:workbookView", this._worksheet.NameSpaceManager) is XmlElement xmlElement))
            return;
          string str = (this._worksheet.PositionID - 1).ToString();
          xmlElement.SetAttribute("activeTab", str);
        }
        else
          this.SetXmlNodeString("@tabSelected", "0");
      }
    }

    public bool PageLayoutView
    {
      get => this.GetXmlNodeString("@view") == "pageLayout";
      set
      {
        if (value)
          this.SetXmlNodeString("@view", "pageLayout");
        else
          this.SheetViewElement.RemoveAttribute("view");
      }
    }

    public bool PageBreakView
    {
      get => this.GetXmlNodeString("@view") == "pageBreakPreview";
      set
      {
        if (value)
          this.SetXmlNodeString("@view", "pageBreakPreview");
        else
          this.SheetViewElement.RemoveAttribute("view");
      }
    }

    public bool ShowGridLines
    {
      get => this.GetXmlNodeBool("@showGridLines");
      set => this.SetXmlNodeString("@showGridLines", value ? "1" : "0");
    }

    public bool ShowHeaders
    {
      get => this.GetXmlNodeBool("@showRowColHeaders");
      set => this.SetXmlNodeString("@showRowColHeaders", value ? "1" : "0");
    }

    public int ZoomScale
    {
      get => this.GetXmlNodeInt("@zoomScale");
      set
      {
        if (value < 10 || value > 400)
          throw new ArgumentOutOfRangeException("Zoome scale out of range (10-400)");
        this.SetXmlNodeString("@zoomScale", value.ToString());
      }
    }

    public bool RightToLeft
    {
      get => this.GetXmlNodeBool("@rightToLeft");
      set => this.SetXmlNodeString("@rightToLeft", value ? "1" : "0");
    }

    internal bool WindowProtection
    {
      get => this.GetXmlNodeBool("@windowProtection", false);
      set => this.SetXmlNodeBool("@windowProtection", value, false);
    }

    public ExcelWorksheetView.ExcelWorksheetPanes[] Panes { get; internal set; }

    public void FreezePanes(int Row, int Column)
    {
      if (Row == 1 && Column == 1)
        this.UnFreezePanes();
      string selectedRange = this.SelectedRange;
      string activeCell = this.ActiveCell;
      if (!(this.TopNode.SelectSingleNode(this._paneNodePath, this.NameSpaceManager) is XmlElement refChild))
      {
        this.CreateNode(this._paneNodePath);
        refChild = this.TopNode.SelectSingleNode(this._paneNodePath, this.NameSpaceManager) as XmlElement;
      }
      refChild.RemoveAll();
      if (Column > 1)
        refChild.SetAttribute("xSplit", (Column - 1).ToString());
      if (Row > 1)
        refChild.SetAttribute("ySplit", (Row - 1).ToString());
      refChild.SetAttribute("topLeftCell", ExcelCellBase.GetAddress(Row, Column));
      refChild.SetAttribute("state", "frozen");
      this.RemoveSelection();
      if (Row > 1 && Column == 1)
      {
        refChild.SetAttribute("activePane", "bottomLeft");
        XmlElement element = this.TopNode.OwnerDocument.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element.SetAttribute("pane", "bottomLeft");
        if (activeCell != "")
          element.SetAttribute("activeCell", activeCell);
        if (selectedRange != "")
          element.SetAttribute("sqref", selectedRange);
        element.SetAttribute("sqref", selectedRange);
        this.TopNode.InsertAfter((XmlNode) element, (XmlNode) refChild);
      }
      else if (Column > 1 && Row == 1)
      {
        refChild.SetAttribute("activePane", "topRight");
        XmlElement element = this.TopNode.OwnerDocument.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element.SetAttribute("pane", "topRight");
        if (activeCell != "")
          element.SetAttribute("activeCell", activeCell);
        if (selectedRange != "")
          element.SetAttribute("sqref", selectedRange);
        this.TopNode.InsertAfter((XmlNode) element, (XmlNode) refChild);
      }
      else
      {
        refChild.SetAttribute("activePane", "bottomRight");
        XmlElement element1 = this.TopNode.OwnerDocument.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.SetAttribute("pane", "topRight");
        string address1 = ExcelCellBase.GetAddress(1, Column);
        element1.SetAttribute("activeCell", address1);
        element1.SetAttribute("sqref", address1);
        refChild.ParentNode.InsertAfter((XmlNode) element1, (XmlNode) refChild);
        XmlElement element2 = this.TopNode.OwnerDocument.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        string address2 = ExcelCellBase.GetAddress(Row, 1);
        element2.SetAttribute("pane", "bottomLeft");
        element2.SetAttribute("activeCell", address2);
        element2.SetAttribute("sqref", address2);
        element1.ParentNode.InsertAfter((XmlNode) element2, (XmlNode) element1);
        XmlElement element3 = this.TopNode.OwnerDocument.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element3.SetAttribute("pane", "bottomRight");
        if (activeCell != "")
          element3.SetAttribute("activeCell", activeCell);
        if (selectedRange != "")
          element3.SetAttribute("sqref", selectedRange);
        element2.ParentNode.InsertAfter((XmlNode) element3, (XmlNode) element2);
      }
      this.Panes = this.LoadPanes();
    }

    private void RemoveSelection()
    {
      foreach (XmlNode selectNode in this.TopNode.SelectNodes(this._selectionNodePath, this.NameSpaceManager))
        selectNode.ParentNode.RemoveChild(selectNode);
    }

    public void UnFreezePanes()
    {
      string selectedRange = this.SelectedRange;
      string activeCell = this.ActiveCell;
      if (this.TopNode.SelectSingleNode(this._paneNodePath, this.NameSpaceManager) is XmlElement oldChild)
        oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
      this.RemoveSelection();
      this.Panes = this.LoadPanes();
      this.SelectedRange = selectedRange;
      this.ActiveCell = activeCell;
    }

    public class ExcelWorksheetPanes : XmlHelper
    {
      private const string _activeCellPath = "@activeCell";
      private const string _selectionRangePath = "@sqref";
      private XmlElement _selectionNode;

      internal ExcelWorksheetPanes(XmlNamespaceManager ns, XmlNode topNode)
        : base(ns, topNode)
      {
        if (!(topNode.Name == "selection"))
          return;
        this._selectionNode = topNode as XmlElement;
      }

      public string ActiveCell
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@activeCell");
          return xmlNodeString == "" ? "A1" : xmlNodeString;
        }
        set
        {
          if (this._selectionNode == null)
            this.CreateSelectionElement();
          int FromRow;
          int FromColumn;
          ExcelCellBase.GetRowColFromAddress(value, out FromRow, out FromColumn, out int _, out int _);
          this.SetXmlNodeString("@activeCell", value);
          if (!(((XmlElement) this.TopNode).GetAttribute("sqref") == ""))
            return;
          this.SelectedRange = ExcelCellBase.GetAddress(FromRow, FromColumn);
        }
      }

      private void CreateSelectionElement()
      {
        this._selectionNode = this.TopNode.OwnerDocument.CreateElement("selection", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        this.TopNode.AppendChild((XmlNode) this._selectionNode);
        this.TopNode = (XmlNode) this._selectionNode;
      }

      public string SelectedRange
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@sqref");
          return xmlNodeString == "" ? "A1" : xmlNodeString;
        }
        set
        {
          if (this._selectionNode == null)
            this.CreateSelectionElement();
          int FromRow;
          int FromColumn;
          ExcelCellBase.GetRowColFromAddress(value, out FromRow, out FromColumn, out int _, out int _);
          this.SetXmlNodeString("@sqref", value);
          if (!(((XmlElement) this.TopNode).GetAttribute("activeCell") == ""))
            return;
          this.ActiveCell = ExcelCellBase.GetAddress(FromRow, FromColumn);
        }
      }
    }
  }
}
