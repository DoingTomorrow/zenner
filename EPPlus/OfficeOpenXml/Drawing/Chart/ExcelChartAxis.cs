// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartAxis
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelChartAxis : XmlHelper
  {
    private const string _majorTickMark = "c:majorTickMark/@val";
    private const string _minorTickMark = "c:minorTickMark/@val";
    private const string _crossesPath = "c:crosses/@val";
    private const string _crossBetweenPath = "c:crossBetween/@val";
    private const string _crossesAtPath = "c:crossesAt/@val";
    private const string _formatPath = "c:numFmt/@formatCode";
    private const string _lblPos = "c:tickLblPos/@val";
    private const string _ticLblPos_Path = "c:tickLblPos/@val";
    private const string _minValuePath = "c:scaling/c:min/@val";
    private const string _maxValuePath = "c:scaling/c:max/@val";
    private const string _majorUnitPath = "c:majorUnit/@val";
    private const string _majorUnitCatPath = "c:tickLblSkip/@val";
    private const string _minorUnitPath = "c:minorUnit/@val";
    private const string _minorUnitCatPath = "c:tickMarkSkip/@val";
    private const string _logbasePath = "c:scaling/c:logBase/@val";
    private const string _orientationPath = "c:scaling/c:orientation/@val";
    private string AXIS_POSITION_PATH = "c:axPos/@val";
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;
    private ExcelTextFont _font;
    private ExcelChartTitle _title;

    internal ExcelChartAxis(XmlNamespaceManager nameSpaceManager, XmlNode topNode)
      : base(nameSpaceManager, topNode)
    {
      this.SchemaNodeOrder = new string[27]
      {
        "axId",
        "scaling",
        "logBase",
        "orientation",
        "max",
        "min",
        "delete",
        "axPos",
        "majorGridlines",
        "title",
        "numFmt",
        "majorTickMark",
        "minorTickMark",
        "tickLblPos",
        "spPr",
        "txPr",
        "crossAx",
        "crossesAt",
        "crosses",
        "crossBetween",
        "auto",
        "lblOffset",
        "majorUnit",
        "minorUnit",
        "dispUnits",
        "spPr",
        "txPr"
      };
    }

    internal string Id => this.GetXmlNodeString("c:axId/@val");

    public eAxisTickMark MajorTickMark
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:majorTickMark/@val");
        if (string.IsNullOrEmpty(xmlNodeString))
          return eAxisTickMark.Cross;
        try
        {
          return (eAxisTickMark) Enum.Parse(typeof (eAxisTickMark), xmlNodeString);
        }
        catch
        {
          return eAxisTickMark.Cross;
        }
      }
      set => this.SetXmlNodeString("c:majorTickMark/@val", value.ToString().ToLower());
    }

    public eAxisTickMark MinorTickMark
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:minorTickMark/@val");
        if (string.IsNullOrEmpty(xmlNodeString))
          return eAxisTickMark.Cross;
        try
        {
          return (eAxisTickMark) Enum.Parse(typeof (eAxisTickMark), xmlNodeString);
        }
        catch
        {
          return eAxisTickMark.Cross;
        }
      }
      set => this.SetXmlNodeString("c:minorTickMark/@val", value.ToString().ToLower());
    }

    internal ExcelChartAxis.eAxisType AxisType
    {
      get
      {
        try
        {
          return (ExcelChartAxis.eAxisType) Enum.Parse(typeof (ExcelChartAxis.eAxisType), this.TopNode.LocalName.Substring(0, 3), true);
        }
        catch
        {
          return ExcelChartAxis.eAxisType.Val;
        }
      }
    }

    public eAxisPosition AxisPosition
    {
      get
      {
        switch (this.GetXmlNodeString(this.AXIS_POSITION_PATH))
        {
          case "b":
            return eAxisPosition.Bottom;
          case "r":
            return eAxisPosition.Right;
          case "t":
            return eAxisPosition.Top;
          default:
            return eAxisPosition.Left;
        }
      }
      internal set
      {
        this.SetXmlNodeString(this.AXIS_POSITION_PATH, value.ToString().ToLower().Substring(0, 1));
      }
    }

    public eCrosses Crosses
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:crosses/@val");
        if (string.IsNullOrEmpty(xmlNodeString))
          return eCrosses.AutoZero;
        try
        {
          return (eCrosses) Enum.Parse(typeof (eCrosses), xmlNodeString, true);
        }
        catch
        {
          return eCrosses.AutoZero;
        }
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("c:crosses/@val", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
      }
    }

    public eCrossBetween CrossBetween
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:crossBetween/@val");
        if (string.IsNullOrEmpty(xmlNodeString))
          return eCrossBetween.Between;
        try
        {
          return (eCrossBetween) Enum.Parse(typeof (eCrossBetween), xmlNodeString, true);
        }
        catch
        {
          return eCrossBetween.Between;
        }
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("c:crossBetween/@val", str.Substring(0, 1).ToLower() + str.Substring(1));
      }
    }

    public double? CrossesAt
    {
      get => this.GetXmlNodeDoubleNull("c:crossesAt/@val");
      set
      {
        if (!value.HasValue)
          this.DeleteNode("c:crossesAt/@val");
        else
          this.SetXmlNodeString("c:crossesAt/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public string Format
    {
      get => this.GetXmlNodeString("c:numFmt/@formatCode");
      set => this.SetXmlNodeString("c:numFmt/@formatCode", value);
    }

    public eTickLabelPosition LabelPosition
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:tickLblPos/@val");
        if (string.IsNullOrEmpty(xmlNodeString))
          return eTickLabelPosition.NextTo;
        try
        {
          return (eTickLabelPosition) Enum.Parse(typeof (eTickLabelPosition), xmlNodeString, true);
        }
        catch
        {
          return eTickLabelPosition.NextTo;
        }
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("c:tickLblPos/@val", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
      }
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "c:spPr");
        return this._fill;
      }
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "c:spPr/a:ln");
        return this._border;
      }
    }

    public ExcelTextFont Font
    {
      get
      {
        if (this._font == null)
        {
          if (this.TopNode.SelectSingleNode("c:txPr", this.NameSpaceManager) == null)
          {
            this.CreateNode("c:txPr/a:bodyPr");
            this.CreateNode("c:txPr/a:lstStyle");
          }
          this._font = new ExcelTextFont(this.NameSpaceManager, this.TopNode, "c:txPr/a:p/a:pPr/a:defRPr", new string[9]
          {
            "pPr",
            "defRPr",
            "solidFill",
            "uFill",
            "latin",
            "cs",
            "r",
            "rPr",
            "t"
          });
        }
        return this._font;
      }
    }

    public bool Deleted
    {
      get => this.GetXmlNodeBool("c:delete/@val");
      set => this.SetXmlNodeBool("c:delete/@val", value);
    }

    public eTickLabelPosition TickLabelPosition
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:tickLblPos/@val");
        return xmlNodeString == "" ? eTickLabelPosition.None : (eTickLabelPosition) Enum.Parse(typeof (eTickLabelPosition), xmlNodeString, true);
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("c:tickLblPos/@val", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
      }
    }

    public ExcelChartTitle Title
    {
      get
      {
        if (this._title == null)
        {
          if (this.TopNode.SelectSingleNode("c:title", this.NameSpaceManager) == null)
          {
            this.CreateNode("c:title");
            this.TopNode.SelectSingleNode("c:title", this.NameSpaceManager).InnerXml = "<c:tx><c:rich><a:bodyPr /><a:lstStyle /><a:p><a:r><a:t /></a:r></a:p></c:rich></c:tx><c:layout /><c:overlay val=\"0\" />";
          }
          this._title = new ExcelChartTitle(this.NameSpaceManager, this.TopNode);
        }
        return this._title;
      }
    }

    public double? MinValue
    {
      get => this.GetXmlNodeDoubleNull("c:scaling/c:min/@val");
      set
      {
        if (!value.HasValue)
          this.DeleteNode("c:scaling/c:min/@val");
        else
          this.SetXmlNodeString("c:scaling/c:min/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double? MaxValue
    {
      get => this.GetXmlNodeDoubleNull("c:scaling/c:max/@val");
      set
      {
        if (!value.HasValue)
          this.DeleteNode("c:scaling/c:max/@val");
        else
          this.SetXmlNodeString("c:scaling/c:max/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double? MajorUnit
    {
      get
      {
        return this.AxisType == ExcelChartAxis.eAxisType.Cat ? this.GetXmlNodeDoubleNull("c:tickLblSkip/@val") : this.GetXmlNodeDoubleNull("c:majorUnit/@val");
      }
      set
      {
        if (!value.HasValue)
        {
          this.DeleteNode("c:majorUnit/@val");
          this.DeleteNode("c:tickLblSkip/@val");
        }
        else if (this.AxisType == ExcelChartAxis.eAxisType.Cat)
          this.SetXmlNodeString("c:tickLblSkip/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        else
          this.SetXmlNodeString("c:majorUnit/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double? MinorUnit
    {
      get
      {
        return this.AxisType == ExcelChartAxis.eAxisType.Cat ? this.GetXmlNodeDoubleNull("c:tickMarkSkip/@val") : this.GetXmlNodeDoubleNull("c:minorUnit/@val");
      }
      set
      {
        if (!value.HasValue)
        {
          this.DeleteNode("c:minorUnit/@val");
          this.DeleteNode("c:tickMarkSkip/@val");
        }
        else if (this.AxisType == ExcelChartAxis.eAxisType.Cat)
          this.SetXmlNodeString("c:tickMarkSkip/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        else
          this.SetXmlNodeString("c:minorUnit/@val", value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double? LogBase
    {
      get => this.GetXmlNodeDoubleNull("c:scaling/c:logBase/@val");
      set
      {
        if (!value.HasValue)
        {
          this.DeleteNode("c:scaling/c:logBase/@val");
        }
        else
        {
          double num = value.Value;
          if (num < 2.0 || num > 1000.0)
            throw new ArgumentOutOfRangeException("Value must be between 2 and 1000");
          this.SetXmlNodeString("c:scaling/c:logBase/@val", num.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture));
        }
      }
    }

    public eAxisOrientation Orientation
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:scaling/c:orientation/@val");
        return xmlNodeString == "" ? eAxisOrientation.MinMax : (eAxisOrientation) Enum.Parse(typeof (eAxisOrientation), xmlNodeString, true);
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("c:scaling/c:orientation/@val", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
      }
    }

    internal enum eAxisType
    {
      Val,
      Cat,
      Date,
      Serie,
    }
  }
}
