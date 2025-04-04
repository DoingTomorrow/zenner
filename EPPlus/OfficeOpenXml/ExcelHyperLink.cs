// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelHyperLink
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelHyperLink : Uri
  {
    private string _referenceAddress;
    private string _display = "";
    private int _colSpann;
    private int _rowSpann;

    public ExcelHyperLink(string uriString)
      : base(uriString)
    {
      this.OriginalUri = (Uri) this;
    }

    [Obsolete("base constructor 'System.Uri.Uri(string, bool)' is obsolete: 'The constructor has been deprecated. Please use new ExcelHyperLink(string). The dontEscape parameter is deprecated and is always false.")]
    public ExcelHyperLink(string uriString, bool dontEscape)
      : base(uriString, dontEscape)
    {
      this.OriginalUri = (Uri) this;
    }

    public ExcelHyperLink(string uriString, UriKind uriKind)
      : base(uriString, uriKind)
    {
      this.OriginalUri = (Uri) this;
    }

    public ExcelHyperLink(string referenceAddress, string display)
      : base("xl://internal")
    {
      this._referenceAddress = referenceAddress;
      this._display = display;
    }

    public string ReferenceAddress
    {
      get => this._referenceAddress;
      set => this._referenceAddress = value;
    }

    public string Display
    {
      get => this._display;
      set => this._display = value;
    }

    public string ToolTip { get; set; }

    public int ColSpann
    {
      get => this._colSpann;
      set => this._colSpann = value;
    }

    public int RowSpann
    {
      get => this._rowSpann;
      set => this._rowSpann = value;
    }

    public Uri OriginalUri { get; internal set; }

    internal string RId { get; set; }
  }
}
