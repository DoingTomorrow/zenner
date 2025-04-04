// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartTrendline
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartTrendline : XmlHelper
  {
    private const string TRENDLINEPATH = "c:trendlineType/@val";
    private const string NAMEPATH = "c:name";
    private const string ORDERPATH = "c:order/@val";
    private const string PERIODPATH = "c:period/@val";
    private const string FORWARDPATH = "c:forward/@val";
    private const string BACKWARDPATH = "c:backward/@val";
    private const string INTERCEPTPATH = "c:intercept/@val";
    private const string DISPLAYRSQUAREDVALUEPATH = "c:dispRSqr/@val";
    private const string DISPLAYEQUATIONPATH = "c:dispEq/@val";

    internal ExcelChartTrendline(XmlNamespaceManager namespaceManager, XmlNode topNode)
      : base(namespaceManager, topNode)
    {
      this.SchemaNodeOrder = new string[10]
      {
        "name",
        "trendlineType",
        "order",
        "period",
        "forward",
        "backward",
        "intercept",
        "dispRSqr",
        "dispEq",
        "trendlineLbl"
      };
    }

    public eTrendLine Type
    {
      get
      {
        switch (this.GetXmlNodeString("c:trendlineType/@val").ToLower())
        {
          case "exp":
            return eTrendLine.Exponential;
          case "log":
            return eTrendLine.Logarithmic;
          case "poly":
            return eTrendLine.Polynomial;
          case "movingavg":
            return eTrendLine.MovingAvgerage;
          case "power":
            return eTrendLine.Power;
          default:
            return eTrendLine.Linear;
        }
      }
      set
      {
        switch (value)
        {
          case eTrendLine.Exponential:
            this.SetXmlNodeString("c:trendlineType/@val", "exp");
            break;
          case eTrendLine.Logarithmic:
            this.SetXmlNodeString("c:trendlineType/@val", "log");
            break;
          case eTrendLine.MovingAvgerage:
            this.SetXmlNodeString("c:trendlineType/@val", "movingAvg");
            this.Period = 2M;
            break;
          case eTrendLine.Polynomial:
            this.SetXmlNodeString("c:trendlineType/@val", "poly");
            this.Order = 2M;
            break;
          case eTrendLine.Power:
            this.SetXmlNodeString("c:trendlineType/@val", "power");
            break;
          default:
            this.SetXmlNodeString("c:trendlineType/@val", "linear");
            break;
        }
      }
    }

    public string Name
    {
      get => this.GetXmlNodeString("c:name");
      set => this.SetXmlNodeString("c:name", value, true);
    }

    public Decimal Order
    {
      get => this.GetXmlNodeDecimal("c:order/@val");
      set
      {
        if (this.Type == eTrendLine.MovingAvgerage)
          throw new ArgumentException("Can't set period for trendline type MovingAvgerage");
        this.DeleteAllNode("c:period/@val");
        this.SetXmlNodeString("c:order/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal Period
    {
      get => this.GetXmlNodeDecimal("c:period/@val");
      set
      {
        if (this.Type == eTrendLine.Polynomial)
          throw new ArgumentException("Can't set period for trendline type Polynomial");
        this.DeleteAllNode("c:order/@val");
        this.SetXmlNodeString("c:period/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal Forward
    {
      get => this.GetXmlNodeDecimal("c:forward/@val");
      set
      {
        this.SetXmlNodeString("c:forward/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal Backward
    {
      get => this.GetXmlNodeDecimal("c:backward/@val");
      set
      {
        this.SetXmlNodeString("c:backward/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal Intercept
    {
      get => this.GetXmlNodeDecimal("c:intercept/@val");
      set
      {
        this.SetXmlNodeString("c:intercept/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public bool DisplayRSquaredValue
    {
      get => this.GetXmlNodeBool("c:dispRSqr/@val", false);
      set => this.SetXmlNodeBool("c:dispRSqr/@val", value, false);
    }

    public bool DisplayEquation
    {
      get => this.GetXmlNodeBool("c:dispEq/@val", false);
      set => this.SetXmlNodeBool("c:dispEq/@val", value, false);
    }
  }
}
