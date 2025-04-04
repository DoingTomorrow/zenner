// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelStyles
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.Dxf;
using OfficeOpenXml.Style.XmlAccess;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelStyles : XmlHelper
  {
    private const string NumberFormatsPath = "d:styleSheet/d:numFmts";
    private const string FontsPath = "d:styleSheet/d:fonts";
    private const string FillsPath = "d:styleSheet/d:fills";
    private const string BordersPath = "d:styleSheet/d:borders";
    private const string CellStyleXfsPath = "d:styleSheet/d:cellStyleXfs";
    private const string CellXfsPath = "d:styleSheet/d:cellXfs";
    private const string CellStylesPath = "d:styleSheet/d:cellStyles";
    private const string dxfsPath = "d:styleSheet/d:dxfs";
    private XmlDocument _styleXml;
    private ExcelWorkbook _wb;
    private XmlNamespaceManager _nameSpaceManager;
    internal int _nextDfxNumFmtID = 164;
    public ExcelStyleCollection<ExcelNumberFormatXml> NumberFormats = new ExcelStyleCollection<ExcelNumberFormatXml>();
    public ExcelStyleCollection<ExcelFontXml> Fonts = new ExcelStyleCollection<ExcelFontXml>();
    public ExcelStyleCollection<ExcelFillXml> Fills = new ExcelStyleCollection<ExcelFillXml>();
    public ExcelStyleCollection<ExcelBorderXml> Borders = new ExcelStyleCollection<ExcelBorderXml>();
    public ExcelStyleCollection<ExcelXfs> CellStyleXfs = new ExcelStyleCollection<ExcelXfs>();
    public ExcelStyleCollection<ExcelXfs> CellXfs = new ExcelStyleCollection<ExcelXfs>();
    public ExcelStyleCollection<ExcelNamedStyleXml> NamedStyles = new ExcelStyleCollection<ExcelNamedStyleXml>();
    public ExcelStyleCollection<ExcelDxfStyleConditionalFormatting> Dxfs = new ExcelStyleCollection<ExcelDxfStyleConditionalFormatting>();

    internal ExcelStyles(XmlNamespaceManager NameSpaceManager, XmlDocument xml, ExcelWorkbook wb)
      : base(NameSpaceManager, (XmlNode) xml)
    {
      this._styleXml = xml;
      this._wb = wb;
      this._nameSpaceManager = NameSpaceManager;
      this.SchemaNodeOrder = new string[8]
      {
        "numFmts",
        "fonts",
        "fills",
        "borders",
        "cellStyleXfs",
        "cellXfs",
        "cellStyles",
        "dxfs"
      };
      this.LoadFromDocument();
    }

    private void LoadFromDocument()
    {
      ExcelNumberFormatXml.AddBuildIn(this.NameSpaceManager, this.NumberFormats);
      XmlNode xmlNode1 = this._styleXml.SelectSingleNode("d:styleSheet/d:numFmts", this._nameSpaceManager);
      if (xmlNode1 != null)
      {
        foreach (XmlNode topNode in xmlNode1)
        {
          ExcelNumberFormatXml excelNumberFormatXml = new ExcelNumberFormatXml(this._nameSpaceManager, topNode);
          this.NumberFormats.Add(excelNumberFormatXml.Id, excelNumberFormatXml);
          if (excelNumberFormatXml.NumFmtId >= this.NumberFormats.NextId)
            this.NumberFormats.NextId = excelNumberFormatXml.NumFmtId + 1;
        }
      }
      foreach (XmlNode topNode in this._styleXml.SelectSingleNode("d:styleSheet/d:fonts", this._nameSpaceManager))
      {
        ExcelFontXml excelFontXml = new ExcelFontXml(this._nameSpaceManager, topNode);
        this.Fonts.Add(excelFontXml.Id, excelFontXml);
      }
      foreach (XmlNode topNode in this._styleXml.SelectSingleNode("d:styleSheet/d:fills", this._nameSpaceManager))
      {
        ExcelFillXml excelFillXml = topNode.FirstChild == null || !(topNode.FirstChild.LocalName == "gradientFill") ? new ExcelFillXml(this._nameSpaceManager, topNode) : (ExcelFillXml) new ExcelGradientFillXml(this._nameSpaceManager, topNode);
        this.Fills.Add(excelFillXml.Id, excelFillXml);
      }
      foreach (XmlNode topNode in this._styleXml.SelectSingleNode("d:styleSheet/d:borders", this._nameSpaceManager))
      {
        ExcelBorderXml excelBorderXml = new ExcelBorderXml(this._nameSpaceManager, topNode);
        this.Borders.Add(excelBorderXml.Id, excelBorderXml);
      }
      XmlNode xmlNode2 = this._styleXml.SelectSingleNode("d:styleSheet/d:cellStyleXfs", this._nameSpaceManager);
      if (xmlNode2 != null)
      {
        foreach (XmlNode topNode in xmlNode2)
        {
          ExcelXfs excelXfs = new ExcelXfs(this._nameSpaceManager, topNode, this);
          this.CellStyleXfs.Add(excelXfs.Id, excelXfs);
        }
      }
      XmlNode xmlNode3 = this._styleXml.SelectSingleNode("d:styleSheet/d:cellXfs", this._nameSpaceManager);
      for (int i = 0; i < xmlNode3.ChildNodes.Count; ++i)
      {
        ExcelXfs excelXfs = new ExcelXfs(this._nameSpaceManager, xmlNode3.ChildNodes[i], this);
        this.CellXfs.Add(excelXfs.Id, excelXfs);
      }
      XmlNode xmlNode4 = this._styleXml.SelectSingleNode("d:styleSheet/d:cellStyles", this._nameSpaceManager);
      if (xmlNode4 != null)
      {
        foreach (XmlNode topNode in xmlNode4)
        {
          ExcelNamedStyleXml excelNamedStyleXml = new ExcelNamedStyleXml(this._nameSpaceManager, topNode, this);
          this.NamedStyles.Add(excelNamedStyleXml.Name, excelNamedStyleXml);
        }
      }
      XmlNode xmlNode5 = this._styleXml.SelectSingleNode("d:styleSheet/d:dxfs", this._nameSpaceManager);
      if (xmlNode5 == null)
        return;
      foreach (XmlNode topNode in xmlNode5)
      {
        ExcelDxfStyleConditionalFormatting conditionalFormatting = new ExcelDxfStyleConditionalFormatting(this._nameSpaceManager, topNode, this);
        this.Dxfs.Add(conditionalFormatting.Id, conditionalFormatting);
      }
    }

    internal ExcelStyle GetStyleObject(int Id, int PositionID, string Address)
    {
      if (Id < 0)
        Id = 0;
      return new ExcelStyle(this, new XmlHelper.ChangedEventHandler(this.PropertyChange), PositionID, Address, Id);
    }

    internal int PropertyChange(StyleBase sender, StyleChangeEventArgs e)
    {
      ExcelAddressBase address1 = new ExcelAddressBase(e.Address);
      ExcelWorksheet worksheet = this._wb.Worksheets[e.PositionID];
      Dictionary<int, int> styleCashe = new Dictionary<int, int>();
      this.SetStyleAddress(sender, e, address1, worksheet, ref styleCashe);
      if (address1.Addresses != null)
      {
        foreach (ExcelAddress address2 in address1.Addresses)
          this.SetStyleAddress(sender, e, (ExcelAddressBase) address2, worksheet, ref styleCashe);
      }
      return 0;
    }

    private void SetStyleAddress(
      StyleBase sender,
      StyleChangeEventArgs e,
      ExcelAddressBase address,
      ExcelWorksheet ws,
      ref Dictionary<int, int> styleCashe)
    {
      if (address.Start.Column == 0 || address.Start.Row == 0)
        throw new Exception("error address");
      if (address.Start.Row == 1 && address.End.Row == 1048576)
      {
        int column = address.Start.Column;
        int row = 0;
        ExcelColumn c;
        for (c = ws._values.Exists(0, address.Start.Column) ? ws._values.GetValue(0, address.Start.Column) as ExcelColumn : ws.Column(address.Start.Column); c.ColumnMin <= address.End.Column; c = ws._values.GetValue(0, column) as ExcelColumn)
        {
          if (c.ColumnMax > address.End.Column)
          {
            ws.CopyColumn(c, address.End.Column + 1, c.ColumnMax);
            c.ColumnMax = address.End.Column;
          }
          int num = ws._styles.GetValue(0, c.ColumnMin);
          if (styleCashe.ContainsKey(num))
          {
            ws._styles.SetValue(0, c.ColumnMin, styleCashe[num]);
            ws.SetStyle(0, c.ColumnMin, styleCashe[num]);
          }
          else
          {
            int newId = this.CellXfs[num].GetNewID(this.CellXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
            styleCashe.Add(num, newId);
            ws.SetStyle(0, c.ColumnMin, newId);
          }
          if (!ws._values.NextCell(ref row, ref column) || row > 0)
          {
            c._columnMax = address.End.Column;
            break;
          }
        }
        if (c._columnMax < address.End.Column)
        {
          ws.Column(c._columnMax + 1)._columnMax = address.End.Column;
          int num = ws._styles.GetValue(0, c.ColumnMin);
          if (styleCashe.ContainsKey(num))
          {
            ws.SetStyle(0, c.ColumnMin, styleCashe[num]);
          }
          else
          {
            int newId = this.CellXfs[num].GetNewID(this.CellXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
            styleCashe.Add(num, newId);
            ws.SetStyle(0, c.ColumnMin, newId);
          }
          c._columnMax = address.End.Column;
        }
        CellsStoreEnumerator<int> cellsStoreEnumerator = new CellsStoreEnumerator<int>(ws._styles, address._fromRow, address._fromCol, address._toRow, address._toCol);
        while (cellsStoreEnumerator.Next())
        {
          if (cellsStoreEnumerator.Column >= address.Start.Column && cellsStoreEnumerator.Column <= address.End.Column)
          {
            if (styleCashe.ContainsKey(cellsStoreEnumerator.Value))
            {
              ws.SetStyle(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column, styleCashe[cellsStoreEnumerator.Value]);
            }
            else
            {
              int newId = this.CellXfs[cellsStoreEnumerator.Value].GetNewID(this.CellXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
              styleCashe.Add(cellsStoreEnumerator.Value, newId);
              ws.SetStyle(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column, newId);
            }
          }
        }
      }
      else if (address.Start.Column == 1 && address.End.Column == 16384)
      {
        for (int row = address.Start.Row; row <= address.End.Row; ++row)
        {
          int num = ws._styles.GetValue(row, 0);
          if (num == 0)
          {
            CellsStoreEnumerator<int> cellsStoreEnumerator = new CellsStoreEnumerator<int>(ws._styles, 0, 1, 0, 16384);
            while (cellsStoreEnumerator.Next())
            {
              num = cellsStoreEnumerator.Value;
              if (ws._values.GetValue(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column) is ExcelColumn excelColumn && excelColumn.ColumnMax < 16384)
              {
                for (int columnMin = excelColumn.ColumnMin; columnMin < excelColumn.ColumnMax; ++columnMin)
                {
                  if (!ws._styles.Exists(row, columnMin))
                    ws._styles.SetValue(row, columnMin, num);
                }
              }
            }
            ws.SetStyle(row, 0, num);
            cellsStoreEnumerator.Dispose();
          }
          if (styleCashe.ContainsKey(num))
          {
            ws.SetStyle(row, 0, styleCashe[num]);
          }
          else
          {
            int newId = this.CellXfs[num].GetNewID(this.CellXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
            styleCashe.Add(num, newId);
            ws._styles.SetValue(row, 0, newId);
            ws.SetStyle(row, 0, newId);
          }
        }
        CellsStoreEnumerator<int> cellsStoreEnumerator1 = new CellsStoreEnumerator<int>(ws._styles, address._fromRow, address._fromCol, address._toRow, address._toCol);
        while (cellsStoreEnumerator1.Next())
        {
          int num = cellsStoreEnumerator1.Value;
          if (styleCashe.ContainsKey(num))
          {
            ws.SetStyle(cellsStoreEnumerator1.Row, cellsStoreEnumerator1.Column, styleCashe[num]);
          }
          else
          {
            int newId = this.CellXfs[num].GetNewID(this.CellXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
            styleCashe.Add(num, newId);
            cellsStoreEnumerator1.Value = newId;
            ws.SetStyle(cellsStoreEnumerator1.Row, cellsStoreEnumerator1.Column, newId);
          }
        }
      }
      else
      {
        for (int column = address.Start.Column; column <= address.End.Column; ++column)
        {
          for (int row = address.Start.Row; row <= address.End.Row; ++row)
          {
            int styleId = this.GetStyleId(ws, row, column);
            if (styleCashe.ContainsKey(styleId))
            {
              ws.SetStyle(row, column, styleCashe[styleId]);
            }
            else
            {
              int newId = this.CellXfs[styleId].GetNewID(this.CellXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
              styleCashe.Add(styleId, newId);
              ws.SetStyle(row, column, newId);
            }
          }
        }
      }
    }

    internal int GetStyleId(ExcelWorksheet ws, int row, int col)
    {
      int styleId = 0;
      if (ws._styles.Exists(row, col, ref styleId) || ws._styles.Exists(row, 0, ref styleId) || ws._styles.Exists(0, col, ref styleId))
        return styleId;
      int row1 = 0;
      int col1 = col;
      return ws._values.PrevCell(ref row1, ref col1) && (ws._values.GetValue(0, col1) as ExcelColumn).ColumnMax >= col ? ws._styles.GetValue(0, col1) : 0;
    }

    internal int NamedStylePropertyChange(StyleBase sender, StyleChangeEventArgs e)
    {
      int indexById = this.NamedStyles.FindIndexByID(e.Address);
      if (indexById >= 0)
      {
        int newId = this.CellStyleXfs[this.NamedStyles[indexById].StyleXfId].GetNewID(this.CellStyleXfs, sender, e.StyleClass, e.StyleProperty, e.Value);
        int styleXfId = this.NamedStyles[indexById].StyleXfId;
        this.NamedStyles[indexById].StyleXfId = newId;
        this.NamedStyles[indexById].Style.Index = newId;
        this.NamedStyles[indexById].XfId = int.MinValue;
        foreach (ExcelXfs cellXf in this.CellXfs)
        {
          if (cellXf.XfId == styleXfId)
            cellXf.XfId = newId;
        }
      }
      return 0;
    }

    internal string Id => "";

    public ExcelNamedStyleXml CreateNamedStyle(string name)
    {
      return this.CreateNamedStyle(name, (ExcelStyle) null);
    }

    public ExcelNamedStyleXml CreateNamedStyle(string name, ExcelStyle Template)
    {
      if (this._wb.Styles.NamedStyles.ExistsKey(name))
        throw new Exception(string.Format("Key {0} already exists in collection", (object) name));
      ExcelNamedStyleXml namedStyle = new ExcelNamedStyleXml(this.NameSpaceManager, this);
      int styleID;
      int positionID;
      ExcelStyles style;
      if (Template == null)
      {
        styleID = 0;
        positionID = -1;
        style = this;
      }
      else if (Template.PositionID < 0 && Template.Styles == this)
      {
        styleID = Template.Index;
        positionID = Template.PositionID;
        style = this;
      }
      else
      {
        styleID = Template.XfId;
        positionID = -1;
        style = Template.Styles;
      }
      int num = this.CloneStyle(style, styleID, true);
      this.CellStyleXfs[num].XfId = this.CellStyleXfs.Count - 1;
      this.CellXfs[this.CloneStyle(style, styleID, false, true)].XfId = num;
      namedStyle.Style = new ExcelStyle(this, new XmlHelper.ChangedEventHandler(this.NamedStylePropertyChange), positionID, name, num);
      namedStyle.StyleXfId = num;
      namedStyle.Name = name;
      int index = this._wb.Styles.NamedStyles.Add(namedStyle.Name, namedStyle);
      namedStyle.Style.SetIndex(index);
      return namedStyle;
    }

    public void UpdateXml()
    {
      this.RemoveUnusedStyles();
      XmlNode xmlNode1 = this._styleXml.SelectSingleNode("d:styleSheet/d:numFmts", this._nameSpaceManager);
      if (xmlNode1 == null)
      {
        this.CreateNode("d:styleSheet/d:numFmts", true);
        xmlNode1 = this._styleXml.SelectSingleNode("d:styleSheet/d:numFmts", this._nameSpaceManager);
      }
      else
        xmlNode1.RemoveAll();
      int num1 = 0;
      int indexById1 = this.NamedStyles.FindIndexByID("Normal");
      if (this.NamedStyles.Count > 0 && indexById1 >= 0 && this.NamedStyles[indexById1].Style.Numberformat.NumFmtID >= 164)
      {
        ExcelNumberFormatXml numberFormat = this.NumberFormats[this.NumberFormats.FindIndexByID(this.NamedStyles[indexById1].Style.Numberformat.Id)];
        xmlNode1.AppendChild(numberFormat.CreateXmlNode((XmlNode) this._styleXml.CreateElement("numFmt", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
        numberFormat.newID = num1++;
      }
      foreach (ExcelNumberFormatXml numberFormat in this.NumberFormats)
      {
        if (!numberFormat.BuildIn)
        {
          xmlNode1.AppendChild(numberFormat.CreateXmlNode((XmlNode) this._styleXml.CreateElement("numFmt", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
          numberFormat.newID = num1;
          ++num1;
        }
      }
      (xmlNode1 as XmlElement).SetAttribute("count", num1.ToString());
      num1 = 0;
      XmlNode xmlNode2 = this._styleXml.SelectSingleNode("d:styleSheet/d:fonts", this._nameSpaceManager);
      xmlNode2.RemoveAll();
      if (this.NamedStyles.Count > 0 && indexById1 >= 0 && this.NamedStyles[indexById1].Style.Font.Index > 0)
      {
        ExcelFontXml font = this.Fonts[this.NamedStyles[indexById1].Style.Font.Index];
        xmlNode2.AppendChild(font.CreateXmlNode((XmlNode) this._styleXml.CreateElement("font", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
        font.newID = num1++;
      }
      foreach (ExcelFontXml font in this.Fonts)
      {
        if (font.useCnt > 0L)
        {
          xmlNode2.AppendChild(font.CreateXmlNode((XmlNode) this._styleXml.CreateElement("font", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
          font.newID = num1;
          ++num1;
        }
      }
      (xmlNode2 as XmlElement).SetAttribute("count", num1.ToString());
      num1 = 0;
      XmlNode xmlNode3 = this._styleXml.SelectSingleNode("d:styleSheet/d:fills", this._nameSpaceManager);
      xmlNode3.RemoveAll();
      this.Fills[0].useCnt = 1L;
      this.Fills[1].useCnt = 1L;
      foreach (ExcelFillXml fill in this.Fills)
      {
        if (fill.useCnt > 0L)
        {
          xmlNode3.AppendChild(fill.CreateXmlNode((XmlNode) this._styleXml.CreateElement("fill", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
          fill.newID = num1;
          ++num1;
        }
      }
      (xmlNode3 as XmlElement).SetAttribute("count", num1.ToString());
      num1 = 0;
      XmlNode xmlNode4 = this._styleXml.SelectSingleNode("d:styleSheet/d:borders", this._nameSpaceManager);
      xmlNode4.RemoveAll();
      this.Borders[0].useCnt = 1L;
      foreach (ExcelBorderXml border in this.Borders)
      {
        if (border.useCnt > 0L)
        {
          xmlNode4.AppendChild(border.CreateXmlNode((XmlNode) this._styleXml.CreateElement("border", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
          border.newID = num1;
          ++num1;
        }
      }
      (xmlNode4 as XmlElement).SetAttribute("count", num1.ToString());
      XmlNode styleXfsNode = this._styleXml.SelectSingleNode("d:styleSheet/d:cellStyleXfs", this._nameSpaceManager);
      if (styleXfsNode == null && this.NamedStyles.Count > 0)
      {
        this.CreateNode("d:styleSheet/d:cellStyleXfs");
        styleXfsNode = this._styleXml.SelectSingleNode("d:styleSheet/d:cellStyleXfs", this._nameSpaceManager);
      }
      if (this.NamedStyles.Count > 0)
        styleXfsNode.RemoveAll();
      int num2 = indexById1 > -1 ? 1 : 0;
      XmlNode xmlNode5 = this._styleXml.SelectSingleNode("d:styleSheet/d:cellStyles", this._nameSpaceManager);
      xmlNode5?.RemoveAll();
      XmlNode cellXfsNode = this._styleXml.SelectSingleNode("d:styleSheet/d:cellXfs", this._nameSpaceManager);
      cellXfsNode.RemoveAll();
      if (this.NamedStyles.Count > 0 && indexById1 >= 0)
      {
        this.NamedStyles[indexById1].newID = 0;
        this.AddNamedStyle(0, styleXfsNode, cellXfsNode, this.NamedStyles[indexById1]);
      }
      foreach (ExcelNamedStyleXml namedStyle in this.NamedStyles)
      {
        if (namedStyle.Name.ToLower() != "normal")
          this.AddNamedStyle(num2++, styleXfsNode, cellXfsNode, namedStyle);
        else
          namedStyle.newID = 0;
        xmlNode5.AppendChild(namedStyle.CreateXmlNode((XmlNode) this._styleXml.CreateElement("cellStyle", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
      }
      if (xmlNode5 != null)
        (xmlNode5 as XmlElement).SetAttribute("count", num2.ToString());
      if (styleXfsNode != null)
        (styleXfsNode as XmlElement).SetAttribute("count", num2.ToString());
      int num3 = 0;
      foreach (ExcelXfs cellXf in this.CellXfs)
      {
        if (cellXf.useCnt > 0L && indexById1 != num3)
        {
          cellXfsNode.AppendChild(cellXf.CreateXmlNode((XmlNode) this._styleXml.CreateElement("xf", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
          cellXf.newID = num2;
          ++num2;
        }
        ++num3;
      }
      (cellXfsNode as XmlElement).SetAttribute("count", num2.ToString());
      XmlNode xmlNode6 = this._styleXml.SelectSingleNode("d:styleSheet/d:dxfs", this._nameSpaceManager);
      foreach (ExcelWorksheet worksheet in this._wb.Worksheets)
      {
        if (!(worksheet is ExcelChartsheet))
        {
          foreach (IExcelConditionalFormattingRule conditionalFormattingRule in (IEnumerable<IExcelConditionalFormattingRule>) worksheet.ConditionalFormatting)
          {
            if (conditionalFormattingRule.Style.HasValue)
            {
              int indexById2 = this.Dxfs.FindIndexByID(conditionalFormattingRule.Style.Id);
              if (indexById2 < 0)
              {
                ((ExcelConditionalFormattingRule) conditionalFormattingRule).DxfId = this.Dxfs.Count;
                this.Dxfs.Add(conditionalFormattingRule.Style.Id, conditionalFormattingRule.Style);
                XmlElement element = ((XmlDocument) this.TopNode).CreateElement("d", "dxf", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                conditionalFormattingRule.Style.CreateNodes((XmlHelper) new XmlHelperInstance(this.NameSpaceManager, (XmlNode) element), "");
                xmlNode6.AppendChild((XmlNode) element);
              }
              else
                ((ExcelConditionalFormattingRule) conditionalFormattingRule).DxfId = indexById2;
            }
          }
        }
      }
    }

    private void AddNamedStyle(
      int id,
      XmlNode styleXfsNode,
      XmlNode cellXfsNode,
      ExcelNamedStyleXml style)
    {
      ExcelXfs cellStyleXf = this.CellStyleXfs[style.StyleXfId];
      styleXfsNode.AppendChild(cellStyleXf.CreateXmlNode((XmlNode) this._styleXml.CreateElement("xf", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"), true));
      cellStyleXf.newID = id;
      cellStyleXf.XfId = style.StyleXfId;
      int indexById = this.CellXfs.FindIndexByID(cellStyleXf.Id);
      if (indexById < 0)
      {
        cellXfsNode.AppendChild(cellStyleXf.CreateXmlNode((XmlNode) this._styleXml.CreateElement("xf", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
      }
      else
      {
        if (id < 0)
          this.CellXfs[indexById].XfId = id;
        cellXfsNode.AppendChild(this.CellXfs[indexById].CreateXmlNode((XmlNode) this._styleXml.CreateElement("xf", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")));
        this.CellXfs[indexById].useCnt = 0L;
        this.CellXfs[indexById].newID = id;
      }
      if (style.XfId >= 0)
        style.XfId = this.CellXfs[style.XfId].newID;
      else
        style.XfId = 0;
    }

    private void RemoveUnusedStyles()
    {
      this.CellXfs[0].useCnt = 1L;
      foreach (ExcelWorksheet worksheet in this._wb.Worksheets)
      {
        CellsStoreEnumerator<int> cellsStoreEnumerator = new CellsStoreEnumerator<int>(worksheet._styles);
        while (cellsStoreEnumerator.Next())
          ++this.CellXfs[cellsStoreEnumerator.Value].useCnt;
      }
      foreach (ExcelNamedStyleXml namedStyle in this.NamedStyles)
        ++this.CellStyleXfs[namedStyle.StyleXfId].useCnt;
      foreach (ExcelXfs cellXf in this.CellXfs)
      {
        if (cellXf.useCnt > 0L)
        {
          if (cellXf.FontId >= 0)
            ++this.Fonts[cellXf.FontId].useCnt;
          if (cellXf.FillId >= 0)
            ++this.Fills[cellXf.FillId].useCnt;
          if (cellXf.BorderId >= 0)
            ++this.Borders[cellXf.BorderId].useCnt;
        }
      }
      foreach (ExcelXfs cellStyleXf in this.CellStyleXfs)
      {
        if (cellStyleXf.useCnt > 0L)
        {
          if (cellStyleXf.FontId >= 0)
            ++this.Fonts[cellStyleXf.FontId].useCnt;
          if (cellStyleXf.FillId >= 0)
            ++this.Fills[cellStyleXf.FillId].useCnt;
          if (cellStyleXf.BorderId >= 0)
            ++this.Borders[cellStyleXf.BorderId].useCnt;
        }
      }
    }

    internal int GetStyleIdFromName(string Name)
    {
      int indexById = this.NamedStyles.FindIndexByID(Name);
      if (indexById < 0)
        return 0;
      int styleIdFromName = this.NamedStyles[indexById].XfId;
      if (styleIdFromName < 0)
      {
        int styleXfId = this.NamedStyles[indexById].StyleXfId;
        ExcelXfs excelXfs = this.CellStyleXfs[styleXfId].Copy();
        excelXfs.XfId = styleXfId;
        styleIdFromName = this.CellXfs.FindIndexByID(excelXfs.Id);
        if (styleIdFromName < 0)
          styleIdFromName = this.CellXfs.Add(excelXfs.Id, excelXfs);
        this.NamedStyles[indexById].XfId = styleIdFromName;
      }
      return styleIdFromName;
    }

    private int GetXmlNodeInt(XmlNode node)
    {
      int result;
      return int.TryParse(this.GetXmlNode(node), out result) ? result : 0;
    }

    private string GetXmlNode(XmlNode node) => node == null || node.Value == null ? "" : node.Value;

    internal int CloneStyle(ExcelStyles style, int styleID)
    {
      return this.CloneStyle(style, styleID, false, false);
    }

    internal int CloneStyle(ExcelStyles style, int styleID, bool isNamedStyle)
    {
      return this.CloneStyle(style, styleID, isNamedStyle, false);
    }

    internal int CloneStyle(ExcelStyles style, int styleID, bool isNamedStyle, bool allwaysAdd)
    {
      ExcelXfs excelXfs1 = !isNamedStyle ? style.CellXfs[styleID] : style.CellStyleXfs[styleID];
      ExcelXfs excelXfs2 = excelXfs1.Copy(this);
      if (excelXfs1.NumberFormatId > 0)
      {
        string key = "";
        foreach (ExcelNumberFormatXml numberFormat in style.NumberFormats)
        {
          if (numberFormat.NumFmtId == excelXfs1.NumberFormatId)
          {
            key = numberFormat.Format;
            break;
          }
        }
        int num = this.NumberFormats.FindIndexByID(key);
        if (num < 0)
        {
          ExcelNumberFormatXml excelNumberFormatXml = new ExcelNumberFormatXml(this.NameSpaceManager)
          {
            Format = key,
            NumFmtId = this.NumberFormats.NextId++
          };
          this.NumberFormats.Add(key, excelNumberFormatXml);
          num = excelNumberFormatXml.NumFmtId;
        }
        excelXfs2.NumberFormatId = num;
      }
      if (excelXfs1.FontId > -1)
      {
        int num = this.Fonts.FindIndexByID(excelXfs1.Font.Id);
        if (num < 0)
        {
          ExcelFontXml excelFontXml = style.Fonts[excelXfs1.FontId].Copy();
          num = this.Fonts.Add(excelXfs1.Font.Id, excelFontXml);
        }
        excelXfs2.FontId = num;
      }
      if (excelXfs1.BorderId > -1)
      {
        int num = this.Borders.FindIndexByID(excelXfs1.Border.Id);
        if (num < 0)
        {
          ExcelBorderXml excelBorderXml = style.Borders[excelXfs1.BorderId].Copy();
          num = this.Borders.Add(excelXfs1.Border.Id, excelBorderXml);
        }
        excelXfs2.BorderId = num;
      }
      if (excelXfs1.FillId > -1)
      {
        int num = this.Fills.FindIndexByID(excelXfs1.Fill.Id);
        if (num < 0)
        {
          ExcelFillXml excelFillXml = style.Fills[excelXfs1.FillId].Copy();
          num = this.Fills.Add(excelXfs1.Fill.Id, excelFillXml);
        }
        excelXfs2.FillId = num;
      }
      if (excelXfs1.XfId > 0)
      {
        int indexById = this.CellStyleXfs.FindIndexByID(style.CellStyleXfs[excelXfs1.XfId].Id);
        excelXfs2.XfId = indexById;
      }
      int num1;
      if (isNamedStyle)
        num1 = this.CellStyleXfs.Add(excelXfs2.Id, excelXfs2);
      else if (allwaysAdd)
      {
        num1 = this.CellXfs.Add(excelXfs2.Id, excelXfs2);
      }
      else
      {
        num1 = this.CellXfs.FindIndexByID(excelXfs2.Id);
        if (num1 < 0)
          num1 = this.CellXfs.Add(excelXfs2.Id, excelXfs2);
      }
      return num1;
    }
  }
}
