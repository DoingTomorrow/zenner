// Decompiled with JetBrains decompiler
// Type: Standard.NONCLIENTMETRICS
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  internal struct NONCLIENTMETRICS
  {
    public int cbSize;
    public int iBorderWidth;
    public int iScrollWidth;
    public int iScrollHeight;
    public int iCaptionWidth;
    public int iCaptionHeight;
    public LOGFONT lfCaptionFont;
    public int iSmCaptionWidth;
    public int iSmCaptionHeight;
    public LOGFONT lfSmCaptionFont;
    public int iMenuWidth;
    public int iMenuHeight;
    public LOGFONT lfMenuFont;
    public LOGFONT lfStatusFont;
    public LOGFONT lfMessageFont;
    public int iPaddedBorderWidth;

    public static NONCLIENTMETRICS VistaMetricsStruct
    {
      get
      {
        return new NONCLIENTMETRICS()
        {
          cbSize = Marshal.SizeOf(typeof (NONCLIENTMETRICS))
        };
      }
    }

    public static NONCLIENTMETRICS XPMetricsStruct
    {
      get
      {
        return new NONCLIENTMETRICS()
        {
          cbSize = Marshal.SizeOf(typeof (NONCLIENTMETRICS)) - 4
        };
      }
    }
  }
}
