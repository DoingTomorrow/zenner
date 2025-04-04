// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.RowInternal
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml
{
  internal class RowInternal
  {
    internal double Height = -1.0;
    internal bool Hidden;
    internal bool Collapsed;
    internal short OutlineLevel;
    internal bool PageBreak;
    internal bool Phonetic;
    internal bool CustomHeight;

    internal RowInternal Clone()
    {
      return new RowInternal()
      {
        Height = this.Height,
        Hidden = this.Hidden,
        Collapsed = this.Collapsed,
        OutlineLevel = this.OutlineLevel,
        PageBreak = this.PageBreak,
        Phonetic = this.Phonetic,
        CustomHeight = this.CustomHeight
      };
    }
  }
}
