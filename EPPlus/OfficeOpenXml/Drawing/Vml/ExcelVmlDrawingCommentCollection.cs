// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingCommentCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  internal class ExcelVmlDrawingCommentCollection : ExcelVmlDrawingBaseCollection, IEnumerable
  {
    internal RangeCollection _drawings;
    private int _nextID;

    internal ExcelVmlDrawingCommentCollection(ExcelPackage pck, ExcelWorksheet ws, Uri uri)
      : base(pck, ws, uri)
    {
      if (uri == (Uri) null)
      {
        this.VmlDrawingXml.LoadXml(this.CreateVmlDrawings());
        this._drawings = new RangeCollection(new List<IRangeID>());
      }
      else
        this.AddDrawingsFromXml(ws);
    }

    protected void AddDrawingsFromXml(ExcelWorksheet ws)
    {
      XmlNodeList xmlNodeList = this.VmlDrawingXml.SelectNodes("//v:shape", this.NameSpaceManager);
      List<IRangeID> cells = new List<IRangeID>();
      foreach (XmlNode topNode in xmlNodeList)
      {
        XmlNode xmlNode1 = topNode.SelectSingleNode("x:ClientData/x:Row", this.NameSpaceManager);
        XmlNode xmlNode2 = topNode.SelectSingleNode("x:ClientData/x:Column", this.NameSpaceManager);
        if (xmlNode1 != null && xmlNode2 != null)
        {
          int Row = int.Parse(xmlNode1.InnerText) + 1;
          int Col = int.Parse(xmlNode2.InnerText) + 1;
          cells.Add((IRangeID) new ExcelVmlDrawingComment(topNode, (ExcelRangeBase) ws.Cells[Row, Col], this.NameSpaceManager));
        }
        else
          cells.Add((IRangeID) new ExcelVmlDrawingComment(topNode, (ExcelRangeBase) ws.Cells[1, 1], this.NameSpaceManager));
      }
      cells.Sort((Comparison<IRangeID>) ((r1, r2) =>
      {
        if (r1.RangeID < r2.RangeID)
          return -1;
        return r1.RangeID <= r2.RangeID ? 0 : 1;
      }));
      this._drawings = new RangeCollection(cells);
    }

    private string CreateVmlDrawings()
    {
      return string.Format("<xml xmlns:v=\"{0}\" xmlns:o=\"{1}\" xmlns:x=\"{2}\">", (object) "urn:schemas-microsoft-com:vml", (object) "urn:schemas-microsoft-com:office:office", (object) "urn:schemas-microsoft-com:office:excel") + "<o:shapelayout v:ext=\"edit\">" + "<o:idmap v:ext=\"edit\" data=\"1\"/>" + "</o:shapelayout>" + "<v:shapetype id=\"_x0000_t202\" coordsize=\"21600,21600\" o:spt=\"202\" path=\"m,l,21600r21600,l21600,xe\">" + "<v:stroke joinstyle=\"miter\" />" + "<v:path gradientshapeok=\"t\" o:connecttype=\"rect\" />" + "</v:shapetype>" + "</xml>";
    }

    internal ExcelVmlDrawingComment Add(ExcelRangeBase cell)
    {
      ExcelVmlDrawingComment cell1 = new ExcelVmlDrawingComment(this.AddDrawing(cell), cell, this.NameSpaceManager);
      this._drawings.Add((IRangeID) cell1);
      return cell1;
    }

    private XmlNode AddDrawing(ExcelRangeBase cell)
    {
      int row = cell.Start.Row;
      int column = cell.Start.Column;
      XmlElement element = this.VmlDrawingXml.CreateElement("v", "shape", "urn:schemas-microsoft-com:vml");
      int num = this._drawings.IndexOf(ExcelCellBase.GetCellID(cell.Worksheet.SheetID, cell._fromRow, cell._fromCol));
      if (num < 0 && ~num < this._drawings.Count)
      {
        ExcelVmlDrawingBase drawing = this._drawings[~num] as ExcelVmlDrawingBase;
        drawing.TopNode.ParentNode.InsertBefore((XmlNode) element, drawing.TopNode);
      }
      else
        this.VmlDrawingXml.DocumentElement.AppendChild((XmlNode) element);
      element.SetAttribute("id", this.GetNewId());
      element.SetAttribute("type", "#_x0000_t202");
      element.SetAttribute("style", "position:absolute;z-index:1; visibility:hidden");
      element.SetAttribute("fillcolor", "#ffffe1");
      element.SetAttribute("insetmode", "urn:schemas-microsoft-com:office:office", "auto");
      string str = "<v:fill color2=\"#ffffe1\" />" + "<v:shadow on=\"t\" color=\"black\" obscured=\"t\" />" + "<v:path o:connecttype=\"none\" />" + "<v:textbox style=\"mso-direction-alt:auto\">" + "<div style=\"text-align:left\" />" + "</v:textbox>" + "<x:ClientData ObjectType=\"Note\">" + "<x:MoveWithCells />" + "<x:SizeWithCells />" + string.Format("<x:Anchor>{0}, 15, {1}, 2, {2}, 31, {3}, 1</x:Anchor>", (object) column, (object) (row - 1), (object) (column + 2), (object) (row + 3)) + "<x:AutoFill>False</x:AutoFill>" + string.Format("<x:Row>{0}</x:Row>", (object) (row - 1)) + string.Format("<x:Column>{0}</x:Column>", (object) (column - 1)) + "</x:ClientData>";
      element.InnerXml = str;
      return (XmlNode) element;
    }

    internal string GetNewId()
    {
      if (this._nextID == 0)
      {
        foreach (ExcelVmlDrawingComment vmlDrawingComment in this)
        {
          int result;
          if (vmlDrawingComment.Id.Length > 3 && vmlDrawingComment.Id.StartsWith("vml") && int.TryParse(vmlDrawingComment.Id.Substring(3, vmlDrawingComment.Id.Length - 3), out result) && result > this._nextID)
            this._nextID = result;
        }
      }
      ++this._nextID;
      return "vml" + this._nextID.ToString();
    }

    internal ExcelVmlDrawingBase this[ulong rangeID]
    {
      get => (ExcelVmlDrawingBase) (this._drawings[rangeID] as ExcelVmlDrawingComment);
    }

    internal bool ContainsKey(ulong rangeID) => this._drawings.ContainsKey(rangeID);

    internal int Count => this._drawings.Count;

    public IEnumerator GetEnumerator() => (IEnumerator) this._drawings;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._drawings;
  }
}
