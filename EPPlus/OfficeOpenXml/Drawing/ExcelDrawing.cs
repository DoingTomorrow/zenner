// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelDrawing
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing.Chart;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public class ExcelDrawing : XmlHelper, IDisposable
  {
    private const float STANDARD_DPI = 96f;
    public const int EMU_PER_PIXEL = 9525;
    private const string lockedPath = "xdr:clientData/@fLocksWithSheet";
    private const string printPath = "xdr:clientData/@fPrintsWithSheet";
    protected ExcelDrawings _drawings;
    protected XmlNode _topNode;
    private string _nameXPath;
    protected internal int _id;

    internal ExcelDrawing(ExcelDrawings drawings, XmlNode node, string nameXPath)
      : base(drawings.NameSpaceManager, node)
    {
      this._drawings = drawings;
      this._topNode = node;
      this._id = drawings.Worksheet.Workbook._nextDrawingID++;
      XmlNode node1 = node.SelectSingleNode("xdr:from", drawings.NameSpaceManager);
      if (node != null)
        this.From = new ExcelDrawing.ExcelPosition(drawings.NameSpaceManager, node1);
      XmlNode node2 = node.SelectSingleNode("xdr:to", drawings.NameSpaceManager);
      this.To = node == null ? (ExcelDrawing.ExcelPosition) null : new ExcelDrawing.ExcelPosition(drawings.NameSpaceManager, node2);
      this._nameXPath = nameXPath;
      this.SchemaNodeOrder = new string[5]
      {
        "from",
        "to",
        "graphicFrame",
        "sp",
        "clientData"
      };
    }

    public string Name
    {
      get
      {
        try
        {
          return this._nameXPath == "" ? "" : this.GetXmlNodeString(this._nameXPath);
        }
        catch
        {
          return "";
        }
      }
      set
      {
        try
        {
          if (this._nameXPath == "")
            throw new NotImplementedException();
          this.SetXmlNodeString(this._nameXPath, value);
        }
        catch
        {
          throw new NotImplementedException();
        }
      }
    }

    public eEditAs EditAs
    {
      get
      {
        try
        {
          string xmlNodeString = this.GetXmlNodeString("@editAs");
          return xmlNodeString == "" ? eEditAs.TwoCell : (eEditAs) Enum.Parse(typeof (eEditAs), xmlNodeString, true);
        }
        catch
        {
          return eEditAs.TwoCell;
        }
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("@editAs", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
      }
    }

    public bool Locked
    {
      get => this.GetXmlNodeBool("xdr:clientData/@fLocksWithSheet", true);
      set => this.SetXmlNodeBool("xdr:clientData/@fLocksWithSheet", value);
    }

    public bool Print
    {
      get => this.GetXmlNodeBool("xdr:clientData/@fPrintsWithSheet", true);
      set => this.SetXmlNodeBool("xdr:clientData/@fPrintsWithSheet", value);
    }

    public ExcelDrawing.ExcelPosition From { get; set; }

    public ExcelDrawing.ExcelPosition To { get; set; }

    internal static ExcelDrawing GetDrawing(ExcelDrawings drawings, XmlNode node)
    {
      if (node.SelectSingleNode("xdr:sp", drawings.NameSpaceManager) != null)
        return (ExcelDrawing) new ExcelShape(drawings, node);
      if (node.SelectSingleNode("xdr:pic", drawings.NameSpaceManager) != null)
        return (ExcelDrawing) new ExcelPicture(drawings, node);
      return node.SelectSingleNode("xdr:graphicFrame", drawings.NameSpaceManager) != null ? (ExcelDrawing) ExcelChart.GetChart(drawings, node) : new ExcelDrawing(drawings, node, "");
    }

    internal string Id => this._id.ToString();

    internal static string GetTextAchoringText(eTextAnchoringType value)
    {
      switch (value)
      {
        case eTextAnchoringType.Bottom:
          return "b";
        case eTextAnchoringType.Center:
          return "ctr";
        case eTextAnchoringType.Distributed:
          return "dist";
        case eTextAnchoringType.Justify:
          return "just";
        default:
          return "t";
      }
    }

    internal static eTextAnchoringType GetTextAchoringEnum(string text)
    {
      switch (text)
      {
        case "b":
          return eTextAnchoringType.Bottom;
        case "ctr":
          return eTextAnchoringType.Center;
        case "dist":
          return eTextAnchoringType.Distributed;
        case "just":
          return eTextAnchoringType.Justify;
        default:
          return eTextAnchoringType.Top;
      }
    }

    internal static string GetTextVerticalText(eTextVerticalType value)
    {
      switch (value)
      {
        case eTextVerticalType.EastAsianVertical:
          return "eaVert";
        case eTextVerticalType.MongolianVertical:
          return "mongolianVert";
        case eTextVerticalType.Vertical:
          return "vert";
        case eTextVerticalType.Vertical270:
          return "vert270";
        case eTextVerticalType.WordArtVertical:
          return "wordArtVert";
        case eTextVerticalType.WordArtVerticalRightToLeft:
          return "wordArtVertRtl";
        default:
          return "horz";
      }
    }

    internal static eTextVerticalType GetTextVerticalEnum(string text)
    {
      switch (text)
      {
        case "eaVert":
          return eTextVerticalType.EastAsianVertical;
        case "mongolianVert":
          return eTextVerticalType.MongolianVertical;
        case "vert":
          return eTextVerticalType.Vertical;
        case "vert270":
          return eTextVerticalType.Vertical270;
        case "wordArtVert":
          return eTextVerticalType.WordArtVertical;
        case "wordArtVertRtl":
          return eTextVerticalType.WordArtVerticalRightToLeft;
        default:
          return eTextVerticalType.Horizontal;
      }
    }

    internal int GetPixelLeft()
    {
      Decimal maxFontWidth = this._drawings.Worksheet.Workbook.MaxFontWidth;
      int num = 0;
      for (int index = 0; index < this.From.Column; ++index)
        num += (int) Decimal.Truncate((256M * this.GetColumnWidth(index + 1) + Decimal.Truncate(128M / maxFontWidth)) / 256M * maxFontWidth);
      return num + this.From.ColumnOff / 9525;
    }

    internal int GetPixelTop()
    {
      ExcelWorksheet worksheet = this._drawings.Worksheet;
      int num = 0;
      for (int index = 0; index < this.From.Row; ++index)
        num += (int) (this.GetRowWidth(index + 1) / 0.75);
      return num + this.From.RowOff / 9525;
    }

    internal int GetPixelWidth()
    {
      Decimal maxFontWidth = this._drawings.Worksheet.Workbook.MaxFontWidth;
      int num = -this.From.ColumnOff / 9525;
      for (int col = this.From.Column + 1; col <= this.To.Column; ++col)
        num += (int) Decimal.Truncate((256M * this.GetColumnWidth(col) + Decimal.Truncate(128M / maxFontWidth)) / 256M * maxFontWidth);
      return num + this.To.ColumnOff / 9525;
    }

    internal int GetPixelHeight()
    {
      ExcelWorksheet worksheet = this._drawings.Worksheet;
      int num = -(this.From.RowOff / 9525);
      for (int row = this.From.Row + 1; row <= this.To.Row; ++row)
        num += (int) (this.GetRowWidth(row) / 0.75);
      return num + this.To.RowOff / 9525;
    }

    private Decimal GetColumnWidth(int col)
    {
      ExcelWorksheet worksheet = this._drawings.Worksheet;
      return !(worksheet._values.GetValue(0, col) is ExcelColumn) ? (Decimal) worksheet.DefaultColWidth : (Decimal) worksheet.Column(col).VisualWidth;
    }

    private double GetRowWidth(int row)
    {
      ExcelWorksheet worksheet = this._drawings.Worksheet;
      object obj = (object) null;
      return worksheet._values.Exists(row, 0, ref obj) && obj != null ? ((RowInternal) obj).Height : worksheet.DefaultRowHeight;
    }

    internal void SetPixelTop(int pixels)
    {
      Decimal maxFontWidth = this._drawings.Worksheet.Workbook.MaxFontWidth;
      int num1 = 0;
      int num2 = (int) (this.GetRowWidth(1) / 0.75);
      int num3 = 2;
      for (; num2 < pixels; num2 += (int) (this.GetRowWidth(num3++) / 0.75))
        num1 = num2;
      if (num2 == pixels)
      {
        this.From.Row = num3 - 1;
        this.From.RowOff = 0;
      }
      else
      {
        this.From.Row = num3 - 2;
        this.From.RowOff = (pixels - num1) * 9525;
      }
    }

    internal void SetPixelLeft(int pixels)
    {
      Decimal maxFontWidth = this._drawings.Worksheet.Workbook.MaxFontWidth;
      int num1 = 0;
      int num2 = (int) Decimal.Truncate((256M * this.GetColumnWidth(1) + Decimal.Truncate(128M / maxFontWidth)) / 256M * maxFontWidth);
      int num3 = 2;
      for (; num2 < pixels; num2 += (int) Decimal.Truncate((256M * this.GetColumnWidth(num3++) + Decimal.Truncate(128M / maxFontWidth)) / 256M * maxFontWidth))
        num1 = num2;
      if (num2 == pixels)
      {
        this.From.Column = num3 - 1;
        this.From.ColumnOff = 0;
      }
      else
      {
        this.From.Column = num3 - 2;
        this.From.ColumnOff = (pixels - num1) * 9525;
      }
    }

    internal void SetPixelHeight(int pixels) => this.SetPixelHeight(pixels, 96f);

    internal void SetPixelHeight(int pixels, float dpi)
    {
      ExcelWorksheet worksheet = this._drawings.Worksheet;
      pixels = (int) ((double) pixels / ((double) dpi / 96.0) + 0.5);
      int num1 = pixels - ((int) (worksheet.Row(this.From.Row + 1).Height / 0.75) - this.From.RowOff / 9525);
      int num2 = pixels;
      int num3 = this.From.Row + 1;
      for (; num1 >= 0; num1 -= (int) (this.GetRowWidth(++num3) / 0.75))
        num2 = num1;
      this.To.Row = num3 - 1;
      if (this.From.Row == this.To.Row)
        this.To.RowOff = this.From.RowOff + pixels * 9525;
      else
        this.To.RowOff = num2 * 9525;
    }

    internal void SetPixelWidth(int pixels) => this.SetPixelWidth(pixels, 96f);

    internal void SetPixelWidth(int pixels, float dpi)
    {
      Decimal maxFontWidth = this._drawings.Worksheet.Workbook.MaxFontWidth;
      pixels = (int) ((double) pixels / ((double) dpi / 96.0) + 0.5);
      int num1 = pixels - ((int) Decimal.Truncate((256M * this.GetColumnWidth(this.From.Column + 1) + Decimal.Truncate(128M / maxFontWidth)) / 256M * maxFontWidth) - this.From.ColumnOff / 9525);
      int num2 = this.From.ColumnOff / 9525 + pixels;
      int num3 = this.From.Column + 2;
      for (; num1 >= 0; num1 -= (int) Decimal.Truncate((256M * this.GetColumnWidth(num3++) + Decimal.Truncate(128M / maxFontWidth)) / 256M * maxFontWidth))
        num2 = num1;
      this.To.Column = num3 - 2;
      this.To.ColumnOff = num2 * 9525;
    }

    public void SetPosition(int PixelTop, int PixelLeft)
    {
      int pixelWidth = this.GetPixelWidth();
      int pixelHeight = this.GetPixelHeight();
      this.SetPixelTop(PixelTop);
      this.SetPixelLeft(PixelLeft);
      this.SetPixelWidth(pixelWidth);
      this.SetPixelHeight(pixelHeight);
    }

    public void SetPosition(int Row, int RowOffsetPixels, int Column, int ColumnOffsetPixels)
    {
      int pixelWidth = this.GetPixelWidth();
      int pixelHeight = this.GetPixelHeight();
      this.From.Row = Row;
      this.From.RowOff = RowOffsetPixels * 9525;
      this.From.Column = Column;
      this.From.ColumnOff = ColumnOffsetPixels * 9525;
      this.SetPixelWidth(pixelWidth);
      this.SetPixelHeight(pixelHeight);
    }

    public virtual void SetSize(int Percent)
    {
      int pixelWidth = this.GetPixelWidth();
      int pixelHeight = this.GetPixelHeight();
      int pixels1 = (int) ((Decimal) pixelWidth * ((Decimal) Percent / 100M));
      int pixels2 = (int) ((Decimal) pixelHeight * ((Decimal) Percent / 100M));
      this.SetPixelWidth(pixels1, 96f);
      this.SetPixelHeight(pixels2, 96f);
    }

    public void SetSize(int PixelWidth, int PixelHeight)
    {
      this.SetPixelWidth(PixelWidth);
      this.SetPixelHeight(PixelHeight);
    }

    internal virtual void DeleteMe() => this.TopNode.ParentNode.RemoveChild(this.TopNode);

    public virtual void Dispose() => this._topNode = (XmlNode) null;

    public class ExcelPosition : XmlHelper
    {
      private const string colPath = "xdr:col";
      private const string rowPath = "xdr:row";
      private const string colOffPath = "xdr:colOff";
      private const string rowOffPath = "xdr:rowOff";
      private XmlNode _node;
      private XmlNamespaceManager _ns;

      internal ExcelPosition(XmlNamespaceManager ns, XmlNode node)
        : base(ns, node)
      {
        this._node = node;
        this._ns = ns;
      }

      public int Column
      {
        get => this.GetXmlNodeInt("xdr:col");
        set => this.SetXmlNodeString("xdr:col", value.ToString());
      }

      public int Row
      {
        get => this.GetXmlNodeInt("xdr:row");
        set => this.SetXmlNodeString("xdr:row", value.ToString());
      }

      public int ColumnOff
      {
        get => this.GetXmlNodeInt("xdr:colOff");
        set => this.SetXmlNodeString("xdr:colOff", value.ToString());
      }

      public int RowOff
      {
        get => this.GetXmlNodeInt("xdr:rowOff");
        set => this.SetXmlNodeString("xdr:rowOff", value.ToString());
      }
    }
  }
}
