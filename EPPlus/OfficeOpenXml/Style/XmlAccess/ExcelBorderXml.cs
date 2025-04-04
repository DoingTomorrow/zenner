// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelBorderXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelBorderXml : StyleXmlHelper
  {
    private const string leftPath = "d:left";
    private const string rightPath = "d:right";
    private const string topPath = "d:top";
    private const string bottomPath = "d:bottom";
    private const string diagonalPath = "d:diagonal";
    private const string diagonalUpPath = "@diagonalUp";
    private const string diagonalDownPath = "@diagonalDown";
    private ExcelBorderItemXml _left;
    private ExcelBorderItemXml _right;
    private ExcelBorderItemXml _top;
    private ExcelBorderItemXml _bottom;
    private ExcelBorderItemXml _diagonal;
    private bool _diagonalUp;
    private bool _diagonalDown;

    internal ExcelBorderXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
    }

    internal ExcelBorderXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      this._left = new ExcelBorderItemXml(nsm, topNode.SelectSingleNode("d:left", nsm));
      this._right = new ExcelBorderItemXml(nsm, topNode.SelectSingleNode("d:right", nsm));
      this._top = new ExcelBorderItemXml(nsm, topNode.SelectSingleNode("d:top", nsm));
      this._bottom = new ExcelBorderItemXml(nsm, topNode.SelectSingleNode("d:bottom", nsm));
      this._diagonal = new ExcelBorderItemXml(nsm, topNode.SelectSingleNode("d:diagonal", nsm));
    }

    internal override string Id
    {
      get
      {
        return this.Left.Id + this.Right.Id + this.Top.Id + this.Bottom.Id + this.Diagonal.Id + this.DiagonalUp.ToString() + this.DiagonalDown.ToString();
      }
    }

    public ExcelBorderItemXml Left
    {
      get => this._left;
      internal set => this._left = value;
    }

    public ExcelBorderItemXml Right
    {
      get => this._right;
      internal set => this._right = value;
    }

    public ExcelBorderItemXml Top
    {
      get => this._top;
      internal set => this._top = value;
    }

    public ExcelBorderItemXml Bottom
    {
      get => this._bottom;
      internal set => this._bottom = value;
    }

    public ExcelBorderItemXml Diagonal
    {
      get => this._diagonal;
      internal set => this._diagonal = value;
    }

    public bool DiagonalUp
    {
      get => this._diagonalUp;
      internal set => this._diagonalUp = value;
    }

    public bool DiagonalDown
    {
      get => this._diagonalDown;
      internal set => this._diagonalDown = value;
    }

    internal ExcelBorderXml Copy()
    {
      return new ExcelBorderXml(this.NameSpaceManager)
      {
        Bottom = this._bottom.Copy(),
        Diagonal = this._diagonal.Copy(),
        Left = this._left.Copy(),
        Right = this._right.Copy(),
        Top = this._top.Copy(),
        DiagonalUp = this._diagonalUp,
        DiagonalDown = this._diagonalDown
      };
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      this.CreateNode("d:left");
      topNode.AppendChild(this._left.CreateXmlNode(this.TopNode.SelectSingleNode("d:left", this.NameSpaceManager)));
      this.CreateNode("d:right");
      topNode.AppendChild(this._right.CreateXmlNode(this.TopNode.SelectSingleNode("d:right", this.NameSpaceManager)));
      this.CreateNode("d:top");
      topNode.AppendChild(this._top.CreateXmlNode(this.TopNode.SelectSingleNode("d:top", this.NameSpaceManager)));
      this.CreateNode("d:bottom");
      topNode.AppendChild(this._bottom.CreateXmlNode(this.TopNode.SelectSingleNode("d:bottom", this.NameSpaceManager)));
      this.CreateNode("d:diagonal");
      topNode.AppendChild(this._diagonal.CreateXmlNode(this.TopNode.SelectSingleNode("d:diagonal", this.NameSpaceManager)));
      if (this._diagonalUp)
        this.SetXmlNodeString("@diagonalUp", "1");
      if (this._diagonalDown)
        this.SetXmlNodeString("@diagonalDown", "1");
      return topNode;
    }
  }
}
