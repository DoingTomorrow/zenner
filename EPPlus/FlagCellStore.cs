// Decompiled with JetBrains decompiler
// Type: FlagCellStore
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml;

#nullable disable
internal class FlagCellStore : CellStore<byte>
{
  internal void SetFlagValue(int Row, int Col, bool value, CellFlags cellFlags)
  {
    this.SetValue(Row, Col, (byte) ((uint) this.GetValue(Row, Col) | (uint) (byte) cellFlags));
  }

  internal bool GetFlagValue(int Row, int Col, CellFlags cellFlags)
  {
    return ((int) (byte) cellFlags & (int) this.GetValue(Row, Col)) != 0;
  }
}
