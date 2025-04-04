// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingPicture
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  public class ExcelVmlDrawingPicture : ExcelVmlDrawingBase
  {
    private ExcelWorksheet _worksheet;

    internal ExcelVmlDrawingPicture(XmlNode topNode, XmlNamespaceManager ns, ExcelWorksheet ws)
      : base(topNode, ns)
    {
      this._worksheet = ws;
    }

    public string Position => this.GetXmlNodeString("@id");

    public double Width
    {
      get => this.GetStyleProp("width");
      set
      {
        this.SetStyleProp("width", value.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "pt");
      }
    }

    public double Height
    {
      get => this.GetStyleProp("height");
      set
      {
        this.SetStyleProp("height", value.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "pt");
      }
    }

    public double Left
    {
      get => this.GetStyleProp("left");
      set
      {
        this.SetStyleProp("left", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double Top
    {
      get => this.GetStyleProp("top");
      set
      {
        this.SetStyleProp("top", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public string Title
    {
      get => this.GetXmlNodeString("v:imagedata/@o:title");
      set => this.SetXmlNodeString("v:imagedata/@o:title", value);
    }

    public Image Image
    {
      get
      {
        ZipPackage package = this._worksheet._package.Package;
        return package.PartExists(this.ImageUri) ? Image.FromStream((Stream) package.GetPart(this.ImageUri).GetStream()) : (Image) null;
      }
    }

    internal Uri ImageUri { get; set; }

    internal string RelId
    {
      get => this.GetXmlNodeString("v:imagedata/@o:relid");
      set => this.SetXmlNodeString("v:imagedata/@o:relid", value);
    }

    public bool BiLevel
    {
      get => this.GetXmlNodeString("v:imagedata/@bilevel") == "t";
      set
      {
        if (value)
          this.SetXmlNodeString("v:imagedata/@bilevel", "t");
        else
          this.DeleteNode("v:imagedata/@bilevel");
      }
    }

    public bool GrayScale
    {
      get => this.GetXmlNodeString("v:imagedata/@grayscale") == "t";
      set
      {
        if (value)
          this.SetXmlNodeString("v:imagedata/@grayscale", "t");
        else
          this.DeleteNode("v:imagedata/@grayscale");
      }
    }

    public double Gain
    {
      get => this.GetFracDT(this.GetXmlNodeString("v:imagedata/@gain"), 1.0);
      set
      {
        if (value < 0.0)
          throw new ArgumentOutOfRangeException("Value must be positive");
        if (value == 1.0)
          this.DeleteNode("v:imagedata/@gamma");
        else
          this.SetXmlNodeString("v:imagedata/@gain", value.ToString("#.0#", (IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double Gamma
    {
      get => this.GetFracDT(this.GetXmlNodeString("v:imagedata/@gamma"), 0.0);
      set
      {
        if (value == 0.0)
          this.DeleteNode("v:imagedata/@gamma");
        else
          this.SetXmlNodeString("v:imagedata/@gamma", value.ToString("#.0#", (IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public double BlackLevel
    {
      get => this.GetFracDT(this.GetXmlNodeString("v:imagedata/@blacklevel"), 0.0);
      set
      {
        if (value == 0.0)
          this.DeleteNode("v:imagedata/@blacklevel");
        else
          this.SetXmlNodeString("v:imagedata/@blacklevel", value.ToString("#.0#", (IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    private double GetFracDT(string v, double def)
    {
      double result1;
      if (v.EndsWith("f"))
      {
        v = v.Substring(0, v.Length - 1);
        double result2;
        result1 = !double.TryParse(v, out result2) ? def : result2 / (double) ushort.MaxValue;
      }
      else if (!double.TryParse(v, out result1))
        result1 = def;
      return result1;
    }

    private void SetStyleProp(string propertyName, string value)
    {
      string xmlNodeString = this.GetXmlNodeString("@style");
      string str1 = "";
      bool flag = false;
      string str2 = xmlNodeString;
      char[] chArray = new char[1]{ ';' };
      foreach (string str3 in str2.Split(chArray))
      {
        if (str3.Split(':')[0] == propertyName)
        {
          str1 = str1 + propertyName + ":" + value + ";";
          flag = true;
        }
        else
          str1 = str1 + str3 + ";";
      }
      if (!flag)
        str1 = str1 + propertyName + ":" + value + ";";
      this.SetXmlNodeString("@style", str1.Substring(0, str1.Length - 1));
    }

    private double GetStyleProp(string propertyName)
    {
      string xmlNodeString = this.GetXmlNodeString("@style");
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str in xmlNodeString.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ ':' };
        string[] strArray = str.Split(chArray2);
        if (strArray[0] == propertyName && strArray.Length > 1)
        {
          double result;
          return double.TryParse(strArray[1].EndsWith("pt") ? strArray[1].Substring(0, strArray[1].Length - 2) : strArray[1], NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : 0.0;
        }
      }
      return 0.0;
    }
  }
}
