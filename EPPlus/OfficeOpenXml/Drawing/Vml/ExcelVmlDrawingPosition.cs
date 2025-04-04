// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingPosition
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  public class ExcelVmlDrawingPosition : XmlHelper
  {
    private int _startPos;

    internal ExcelVmlDrawingPosition(XmlNamespaceManager ns, XmlNode topNode, int startPos)
      : base(ns, topNode)
    {
      this._startPos = startPos;
    }

    public int Row
    {
      get => this.GetNumber(2);
      set => this.SetNumber(2, value);
    }

    public int RowOffset
    {
      get => this.GetNumber(3);
      set => this.SetNumber(3, value);
    }

    public int Column
    {
      get => this.GetNumber(0);
      set => this.SetNumber(0, value);
    }

    public int ColumnOffset
    {
      get => this.GetNumber(1);
      set => this.SetNumber(1, value);
    }

    private void SetNumber(int pos, int value)
    {
      string[] strArray = this.GetXmlNodeString("x:Anchor").Split(',');
      if (strArray.Length != 8)
        throw new Exception("Anchor element is invalid in vmlDrawing");
      strArray[this._startPos + pos] = value.ToString();
      this.SetXmlNodeString("x:Anchor", string.Join(",", strArray));
    }

    private int GetNumber(int pos)
    {
      string[] strArray = this.GetXmlNodeString("x:Anchor").Split(',');
      int result;
      if (strArray.Length == 8 && int.TryParse(strArray[this._startPos + pos], out result))
        return result;
      throw new Exception("Anchor element is invalid in vmlDrawing");
    }
  }
}
