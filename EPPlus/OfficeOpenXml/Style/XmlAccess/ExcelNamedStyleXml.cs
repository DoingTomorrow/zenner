// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelNamedStyleXml : StyleXmlHelper
  {
    private const string idPath = "@xfId";
    private const string buildInIdPath = "@builtinId";
    private const string customBuiltinPath = "@customBuiltin";
    private const string namePath = "@name";
    private ExcelStyles _styles;
    private int _styleXfId;
    private int _xfId = int.MinValue;
    private string _name;
    private ExcelStyle _style;

    internal ExcelNamedStyleXml(XmlNamespaceManager nameSpaceManager, ExcelStyles styles)
      : base(nameSpaceManager)
    {
      this._styles = styles;
      this.BuildInId = int.MinValue;
    }

    internal ExcelNamedStyleXml(
      XmlNamespaceManager NameSpaceManager,
      XmlNode topNode,
      ExcelStyles styles)
      : base(NameSpaceManager, topNode)
    {
      this.StyleXfId = this.GetXmlNodeInt("@xfId");
      this.Name = this.GetXmlNodeString("@name");
      this.BuildInId = this.GetXmlNodeInt("@builtinId");
      this.CustomBuildin = this.GetXmlNodeBool("@customBuiltin");
      this._styles = styles;
      this._style = new ExcelStyle(styles, new XmlHelper.ChangedEventHandler(styles.NamedStylePropertyChange), -1, this.Name, this._styleXfId);
    }

    internal override string Id => this.Name;

    public int StyleXfId
    {
      get => this._styleXfId;
      set => this._styleXfId = value;
    }

    internal int XfId
    {
      get => this._xfId;
      set => this._xfId = value;
    }

    public int BuildInId { get; set; }

    public bool CustomBuildin { get; set; }

    public string Name
    {
      get => this._name;
      internal set => this._name = value;
    }

    public ExcelStyle Style
    {
      get => this._style;
      internal set => this._style = value;
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      this.SetXmlNodeString("@name", this._name);
      this.SetXmlNodeString("@xfId", this._styles.CellStyleXfs[this.StyleXfId].newID.ToString());
      if (this.BuildInId >= 0)
        this.SetXmlNodeString("@builtinId", this.BuildInId.ToString());
      if (this.CustomBuildin)
        this.SetXmlNodeBool("@customBuiltin", true);
      return this.TopNode;
    }
  }
}
