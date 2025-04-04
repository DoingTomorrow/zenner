// Decompiled with JetBrains decompiler
// Type: Standard.SafeGdiplusStartupToken
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;

#nullable disable
namespace Standard
{
  internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeGdiplusStartupToken(IntPtr ptr)
      : base(true)
    {
      this.handle = ptr;
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
      return NativeMethods.GdiplusShutdown(this.handle) == Status.Ok;
    }

    public static SafeGdiplusStartupToken Startup()
    {
      IntPtr token;
      if (NativeMethods.GdiplusStartup(out token, new StartupInput(), out StartupOutput _) == Status.Ok)
        return new SafeGdiplusStartupToken(token);
      throw new Exception("Unable to initialize GDI+");
    }
  }
}
