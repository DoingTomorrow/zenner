// Decompiled with JetBrains decompiler
// Type: Standard.SafeFindHandle
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Microsoft.Win32.SafeHandles;
using System.Security;

#nullable disable
namespace Standard
{
  internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    [SecurityCritical]
    private SafeFindHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle() => NativeMethods.FindClose(this.handle);
  }
}
