// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.StyleBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Style
{
  public abstract class StyleBase
  {
    protected ExcelStyles _styles;
    internal XmlHelper.ChangedEventHandler _ChangedEvent;
    protected int _positionID;
    protected string _address;

    internal StyleBase(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int PositionID,
      string Address)
    {
      this._styles = styles;
      this._ChangedEvent = ChangedEvent;
      this._address = Address;
      this._positionID = PositionID;
    }

    internal int Index { get; set; }

    internal abstract string Id { get; }

    internal virtual void SetIndex(int index) => this.Index = index;
  }
}
