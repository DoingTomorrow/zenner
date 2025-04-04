// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfNumberFormat
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfNumberFormat(ExcelStyles styles) : DxfStyleBase<ExcelDxfNumberFormat>(styles)
  {
    private int _numFmtID = int.MinValue;
    private string _format = "";

    public int NumFmtID
    {
      get => this._numFmtID;
      internal set => this._numFmtID = value;
    }

    public string Format
    {
      get => this._format;
      set
      {
        this._format = value;
        this.NumFmtID = ExcelNumberFormat.GetFromBuildIdFromFormat(value);
      }
    }

    protected internal override string Id => this.Format;

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      if (this.NumFmtID < 0 && !string.IsNullOrEmpty(this.Format))
        this.NumFmtID = this._styles._nextDfxNumFmtID++;
      helper.CreateNode(path);
      this.SetValue(helper, path + "/@numFmtId", (object) this.NumFmtID);
      this.SetValue(helper, path + "/@formatCode", (object) this.Format);
    }

    protected internal override bool HasValue => !string.IsNullOrEmpty(this.Format);

    protected internal override ExcelDxfNumberFormat Clone()
    {
      return new ExcelDxfNumberFormat(this._styles)
      {
        NumFmtID = this.NumFmtID,
        Format = this.Format
      };
    }
  }
}
