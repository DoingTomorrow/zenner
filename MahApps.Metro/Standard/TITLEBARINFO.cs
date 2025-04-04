// Decompiled with JetBrains decompiler
// Type: Standard.TITLEBARINFO
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

#nullable disable
namespace Standard
{
  internal struct TITLEBARINFO
  {
    public int cbSize;
    public RECT rcTitleBar;
    public STATE_SYSTEM rgstate_TitleBar;
    public STATE_SYSTEM rgstate_Reserved;
    public STATE_SYSTEM rgstate_MinimizeButton;
    public STATE_SYSTEM rgstate_MaximizeButton;
    public STATE_SYSTEM rgstate_HelpButton;
    public STATE_SYSTEM rgstate_CloseButton;
  }
}
