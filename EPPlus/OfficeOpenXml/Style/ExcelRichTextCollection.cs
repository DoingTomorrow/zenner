// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelRichTextCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style
{
  public class ExcelRichTextCollection : XmlHelper, IEnumerable<ExcelRichText>, IEnumerable
  {
    private List<ExcelRichText> _list = new List<ExcelRichText>();
    private ExcelRangeBase _cells;

    internal ExcelRichTextCollection(XmlNamespaceManager ns, XmlNode topNode)
      : base(ns, topNode)
    {
      XmlNodeList xmlNodeList = topNode.SelectNodes("d:r", this.NameSpaceManager);
      if (xmlNodeList == null)
        return;
      foreach (XmlNode topNode1 in xmlNodeList)
        this._list.Add(new ExcelRichText(ns, topNode1));
    }

    internal ExcelRichTextCollection(XmlNamespaceManager ns, XmlNode topNode, ExcelRangeBase cells)
      : this(ns, topNode)
    {
      this._cells = cells;
    }

    public ExcelRichText this[int Index]
    {
      get
      {
        ExcelRichText excelRichText = this._list[Index];
        if (this._cells != null)
          excelRichText.SetCallback(new ExcelRichText.CallbackDelegate(this.UpdateCells));
        return excelRichText;
      }
    }

    public int Count => this._list.Count;

    public ExcelRichText Add(string Text)
    {
      XmlElement element = (!(this.TopNode is XmlDocument) ? this.TopNode.OwnerDocument : this.TopNode as XmlDocument).CreateElement("d", "r", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      this.TopNode.AppendChild((XmlNode) element);
      ExcelRichText excelRichText1 = new ExcelRichText(this.NameSpaceManager, (XmlNode) element);
      if (this._list.Count > 0)
      {
        ExcelRichText excelRichText2 = this._list[this._list.Count - 1];
        excelRichText1.FontName = excelRichText2.FontName;
        excelRichText1.Size = excelRichText2.Size;
        Color color = excelRichText2.Color;
        excelRichText1.Color = !color.IsEmpty ? excelRichText2.Color : Color.Black;
        excelRichText1.PreserveSpace = excelRichText1.PreserveSpace;
        excelRichText1.Bold = excelRichText2.Bold;
        excelRichText1.Italic = excelRichText2.Italic;
        excelRichText1.UnderLine = excelRichText2.UnderLine;
      }
      else if (this._cells == null)
      {
        excelRichText1.FontName = "Calibri";
        excelRichText1.Size = 11f;
      }
      else
      {
        ExcelStyle style = this._cells.Offset(0, 0).Style;
        excelRichText1.FontName = style.Font.Name;
        excelRichText1.Size = style.Font.Size;
        excelRichText1.Bold = style.Font.Bold;
        excelRichText1.Italic = style.Font.Italic;
        this._cells.IsRichText = true;
      }
      excelRichText1.Text = Text;
      excelRichText1.PreserveSpace = true;
      if (this._cells != null)
      {
        excelRichText1.SetCallback(new ExcelRichText.CallbackDelegate(this.UpdateCells));
        this.UpdateCells();
      }
      this._list.Add(excelRichText1);
      return excelRichText1;
    }

    internal void UpdateCells() => this._cells.SetValueRichText((object) this.TopNode.InnerXml);

    public void Clear()
    {
      this._list.Clear();
      this.TopNode.RemoveAll();
      if (this._cells == null)
        return;
      this._cells.IsRichText = false;
    }

    public void RemoveAt(int Index)
    {
      this.TopNode.RemoveChild(this._list[Index].TopNode);
      this._list.RemoveAt(Index);
      if (this._cells == null || this._list.Count != 0)
        return;
      this._cells.IsRichText = false;
    }

    public void Remove(ExcelRichText Item)
    {
      this.TopNode.RemoveChild(Item.TopNode);
      this._list.Remove(Item);
      if (this._cells == null || this._list.Count != 0)
        return;
      this._cells.IsRichText = false;
    }

    public string Text
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (ExcelRichText excelRichText in this._list)
          stringBuilder.Append(excelRichText.Text);
        return stringBuilder.ToString();
      }
      set
      {
        if (this.Count == 0)
        {
          this.Add(value);
        }
        else
        {
          this[0].Text = value;
          for (int Index = 1; Index < this.Count; ++Index)
            this.RemoveAt(Index);
        }
      }
    }

    IEnumerator<ExcelRichText> IEnumerable<ExcelRichText>.GetEnumerator()
    {
      return (IEnumerator<ExcelRichText>) this._list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
  }
}
