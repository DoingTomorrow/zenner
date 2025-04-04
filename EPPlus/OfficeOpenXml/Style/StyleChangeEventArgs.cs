// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.StyleChangeEventArgs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Style
{
  internal class StyleChangeEventArgs : EventArgs
  {
    internal eStyleClass StyleClass;
    internal eStyleProperty StyleProperty;
    internal object Value;
    internal string Address;

    internal StyleChangeEventArgs(
      eStyleClass styleclass,
      eStyleProperty styleProperty,
      object value,
      int positionID,
      string address)
    {
      this.StyleClass = styleclass;
      this.StyleProperty = styleProperty;
      this.Value = value;
      this.Address = address;
      this.PositionID = positionID;
    }

    internal int PositionID { get; set; }
  }
}
