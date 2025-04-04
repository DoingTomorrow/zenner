// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelPrinterSettings
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelPrinterSettings : XmlHelper
  {
    private const string _leftMarginPath = "d:pageMargins/@left";
    private const string _rightMarginPath = "d:pageMargins/@right";
    private const string _topMarginPath = "d:pageMargins/@top";
    private const string _bottomMarginPath = "d:pageMargins/@bottom";
    private const string _headerMarginPath = "d:pageMargins/@header";
    private const string _footerMarginPath = "d:pageMargins/@footer";
    private const string _orientationPath = "d:pageSetup/@orientation";
    private const string _fitToWidthPath = "d:pageSetup/@fitToWidth";
    private const string _fitToHeightPath = "d:pageSetup/@fitToHeight";
    private const string _scalePath = "d:pageSetup/@scale";
    private const string _fitToPagePath = "d:sheetPr/d:pageSetUpPr/@fitToPage";
    private const string _headersPath = "d:printOptions/@headings";
    private const string _gridLinesPath = "d:printOptions/@gridLines";
    private const string _horizontalCenteredPath = "d:printOptions/@horizontalCentered";
    private const string _verticalCenteredPath = "d:printOptions/@verticalCentered";
    private const string _pageOrderPath = "d:pageSetup/@pageOrder";
    private const string _blackAndWhitePath = "d:pageSetup/@blackAndWhite";
    private const string _draftPath = "d:pageSetup/@draft";
    private const string _paperSizePath = "d:pageSetup/@paperSize";
    private ExcelWorksheet _ws;
    private bool _marginsCreated;

    internal ExcelPrinterSettings(XmlNamespaceManager ns, XmlNode topNode, ExcelWorksheet ws)
      : base(ns, topNode)
    {
      this._ws = ws;
      this.SchemaNodeOrder = ws.SchemaNodeOrder;
    }

    public Decimal LeftMargin
    {
      get => this.GetXmlNodeDecimal("d:pageMargins/@left");
      set
      {
        this.CreateMargins();
        this.SetXmlNodeString("d:pageMargins/@left", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal RightMargin
    {
      get => this.GetXmlNodeDecimal("d:pageMargins/@right");
      set
      {
        this.CreateMargins();
        this.SetXmlNodeString("d:pageMargins/@right", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal TopMargin
    {
      get => this.GetXmlNodeDecimal("d:pageMargins/@top");
      set
      {
        this.CreateMargins();
        this.SetXmlNodeString("d:pageMargins/@top", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal BottomMargin
    {
      get => this.GetXmlNodeDecimal("d:pageMargins/@bottom");
      set
      {
        this.CreateMargins();
        this.SetXmlNodeString("d:pageMargins/@bottom", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal HeaderMargin
    {
      get => this.GetXmlNodeDecimal("d:pageMargins/@header");
      set
      {
        this.CreateMargins();
        this.SetXmlNodeString("d:pageMargins/@header", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal FooterMargin
    {
      get => this.GetXmlNodeDecimal("d:pageMargins/@footer");
      set
      {
        this.CreateMargins();
        this.SetXmlNodeString("d:pageMargins/@footer", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public eOrientation Orientation
    {
      get
      {
        return (eOrientation) Enum.Parse(typeof (eOrientation), this.GetXmlNodeString("d:pageSetup/@orientation"), true);
      }
      set => this.SetXmlNodeString("d:pageSetup/@orientation", value.ToString().ToLower());
    }

    public int FitToWidth
    {
      get => this.GetXmlNodeInt("d:pageSetup/@fitToWidth");
      set => this.SetXmlNodeString("d:pageSetup/@fitToWidth", value.ToString());
    }

    public int FitToHeight
    {
      get => this.GetXmlNodeInt("d:pageSetup/@fitToHeight");
      set => this.SetXmlNodeString("d:pageSetup/@fitToHeight", value.ToString());
    }

    public int Scale
    {
      get => this.GetXmlNodeInt("d:pageSetup/@scale");
      set => this.SetXmlNodeString("d:pageSetup/@scale", value.ToString());
    }

    public bool FitToPage
    {
      get => this.GetXmlNodeBool("d:sheetPr/d:pageSetUpPr/@fitToPage");
      set => this.SetXmlNodeString("d:sheetPr/d:pageSetUpPr/@fitToPage", value ? "1" : "0");
    }

    public bool ShowHeaders
    {
      get => this.GetXmlNodeBool("d:printOptions/@headings", false);
      set => this.SetXmlNodeBool("d:printOptions/@headings", value, false);
    }

    public ExcelAddress RepeatRows
    {
      get
      {
        if (!this._ws.Names.ContainsKey("_xlnm.Print_Titles"))
          return (ExcelAddress) null;
        ExcelRangeBase name = (ExcelRangeBase) this._ws.Names["_xlnm.Print_Titles"];
        if (name.Start.Column == 1 && name.End.Column == 16384)
          return new ExcelAddress(name.FirstAddress);
        return name._addresses != null && name.Addresses[0].Start.Column == 1 && name.Addresses[0].End.Column == 16384 ? name._addresses[0] : (ExcelAddress) null;
      }
      set
      {
        if (value.Start.Column != 1 || value.End.Column != 16384)
          throw new InvalidOperationException("Address must span full columns only (for ex. Address=\"A:A\" for the first column).");
        ExcelAddress repeatColumns = this.RepeatColumns;
        string address = repeatColumns != null ? repeatColumns.Address + "," + value.Address : value.Address;
        if (this._ws.Names.ContainsKey("_xlnm.Print_Titles"))
          this._ws.Names["_xlnm.Print_Titles"].Address = address;
        else
          this._ws.Names.Add("_xlnm.Print_Titles", new ExcelRangeBase(this._ws, address));
      }
    }

    public ExcelAddress RepeatColumns
    {
      get
      {
        if (!this._ws.Names.ContainsKey("_xlnm.Print_Titles"))
          return (ExcelAddress) null;
        ExcelRangeBase name = (ExcelRangeBase) this._ws.Names["_xlnm.Print_Titles"];
        if (name.Start.Row == 1 && name.End.Row == 1048576)
          return new ExcelAddress(name.FirstAddress);
        return name._addresses != null && name._addresses[0].Start.Row == 1 && name._addresses[0].End.Row == 1048576 ? name._addresses[0] : (ExcelAddress) null;
      }
      set
      {
        if (value.Start.Row != 1 || value.End.Row != 1048576)
          throw new InvalidOperationException("Address must span rows only (for ex. Address=\"1:1\" for the first row).");
        ExcelAddress repeatRows = this.RepeatRows;
        string address = repeatRows != null ? value.Address + "," + repeatRows.Address : value.Address;
        if (this._ws.Names.ContainsKey("_xlnm.Print_Titles"))
          this._ws.Names["_xlnm.Print_Titles"].Address = address;
        else
          this._ws.Names.Add("_xlnm.Print_Titles", new ExcelRangeBase(this._ws, address));
      }
    }

    public ExcelRangeBase PrintArea
    {
      get
      {
        return this._ws.Names.ContainsKey("_xlnm.Print_Area") ? (ExcelRangeBase) this._ws.Names["_xlnm.Print_Area"] : (ExcelRangeBase) null;
      }
      set
      {
        if (value == null)
          this._ws.Names.Remove("_xlnm.Print_Area");
        else if (this._ws.Names.ContainsKey("_xlnm.Print_Area"))
          this._ws.Names["_xlnm.Print_Area"].Address = value.Address;
        else
          this._ws.Names.Add("_xlnm.Print_Area", value);
      }
    }

    public bool ShowGridLines
    {
      get => this.GetXmlNodeBool("d:printOptions/@gridLines", false);
      set => this.SetXmlNodeBool("d:printOptions/@gridLines", value, false);
    }

    public bool HorizontalCentered
    {
      get => this.GetXmlNodeBool("d:printOptions/@horizontalCentered", false);
      set => this.SetXmlNodeBool("d:printOptions/@horizontalCentered", value, false);
    }

    public bool VerticalCentered
    {
      get => this.GetXmlNodeBool("d:printOptions/@verticalCentered", false);
      set => this.SetXmlNodeBool("d:printOptions/@verticalCentered", value, false);
    }

    public ePageOrder PageOrder
    {
      get
      {
        return this.GetXmlNodeString("d:pageSetup/@pageOrder") == "overThenDown" ? ePageOrder.OverThenDown : ePageOrder.DownThenOver;
      }
      set
      {
        if (value == ePageOrder.OverThenDown)
          this.SetXmlNodeString("d:pageSetup/@pageOrder", "overThenDown");
        else
          this.DeleteNode("d:pageSetup/@pageOrder");
      }
    }

    public bool BlackAndWhite
    {
      get => this.GetXmlNodeBool("d:pageSetup/@blackAndWhite", false);
      set => this.SetXmlNodeBool("d:pageSetup/@blackAndWhite", value, false);
    }

    public bool Draft
    {
      get => this.GetXmlNodeBool("d:pageSetup/@draft", false);
      set => this.SetXmlNodeBool("d:pageSetup/@draft", value, false);
    }

    public ePaperSize PaperSize
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:pageSetup/@paperSize");
        return xmlNodeString != "" ? (ePaperSize) int.Parse(xmlNodeString) : ePaperSize.Letter;
      }
      set => this.SetXmlNodeString("d:pageSetup/@paperSize", ((int) value).ToString());
    }

    private void CreateMargins()
    {
      if (this._marginsCreated || this.TopNode.SelectSingleNode("d:pageMargins/@left", this.NameSpaceManager) != null)
        return;
      this._marginsCreated = true;
      this.LeftMargin = 0.7087M;
      this.RightMargin = 0.7087M;
      this.TopMargin = 0.7480M;
      this.BottomMargin = 0.7480M;
      this.HeaderMargin = 0.315M;
      this.FooterMargin = 0.315M;
    }
  }
}
