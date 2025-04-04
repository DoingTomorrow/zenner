// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  public class ExcelVmlDrawingBase : XmlHelper
  {
    internal ExcelVmlDrawingBase(XmlNode topNode, XmlNamespaceManager ns)
      : base(ns, topNode)
    {
      this.SchemaNodeOrder = new string[17]
      {
        "fill",
        "stroke",
        "shadow",
        "path",
        "textbox",
        "ClientData",
        "MoveWithCells",
        "SizeWithCells",
        "Anchor",
        "Locked",
        "AutoFill",
        "LockText",
        "TextHAlign",
        "TextVAlign",
        "Row",
        "Column",
        "Visible"
      };
    }

    public string Id
    {
      get => this.GetXmlNodeString("@id");
      set => this.SetXmlNodeString("@id", value);
    }

    protected bool GetStyle(string style, string key, out string value)
    {
      string str1 = style;
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (str2.IndexOf(':') > 0)
        {
          string[] strArray = str2.Split(':');
          if (strArray[0] == key)
          {
            value = strArray[1];
            return true;
          }
        }
        else if (str2 == key)
        {
          value = "";
          return true;
        }
      }
      value = "";
      return false;
    }

    protected string SetStyle(string style, string key, string value)
    {
      string[] strArray = style.Split(';');
      string str1 = "";
      bool flag = false;
      foreach (string str2 in strArray)
      {
        if (str2.Split(':')[0].Trim() == key)
        {
          if (value.Trim() != "")
            str1 = str1 + key + (object) ':' + value;
          flag = true;
        }
        else
          str1 += str2;
        str1 += (string) (object) ';';
      }
      string str3;
      if (!flag)
        str3 = str1 + key + (object) ':' + value;
      else
        str3 = style.Substring(0, style.Length - 1);
      return str3;
    }
  }
}
