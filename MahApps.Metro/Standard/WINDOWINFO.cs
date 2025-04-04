// Decompiled with JetBrains decompiler
// Type: Standard.WINDOWINFO
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

#nullable disable
namespace Standard
{
  internal struct WINDOWINFO
  {
    public int cbSize;
    public RECT rcWindow;
    public RECT rcClient;
    public int dwStyle;
    public int dwExStyle;
    public uint dwWindowStatus;
    public uint cxWindowBorders;
    public uint cyWindowBorders;
    public ushort atomWindowType;
    public ushort wCreatorVersion;
  }
}
