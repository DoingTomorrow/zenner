// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Win32ThreadIDHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.IO;
using System.Security;
using System.Text;

#nullable disable
namespace NLog.Internal
{
  [SecuritySafeCritical]
  internal class Win32ThreadIDHelper : ThreadIDHelper
  {
    private readonly int currentProcessID;
    private readonly string currentProcessName;
    private readonly string currentProcessBaseName;

    public Win32ThreadIDHelper()
    {
      this.currentProcessID = NativeMethods.GetCurrentProcessId();
      StringBuilder lpFilename = new StringBuilder(512);
      if (NativeMethods.GetModuleFileName(IntPtr.Zero, lpFilename, lpFilename.Capacity) == 0U)
        throw new InvalidOperationException("Cannot determine program name.");
      this.currentProcessName = lpFilename.ToString();
      this.currentProcessBaseName = Path.GetFileNameWithoutExtension(this.currentProcessName);
    }

    public override int CurrentProcessID => this.currentProcessID;

    public override string CurrentProcessName => this.currentProcessName;

    public override string CurrentProcessBaseName => this.currentProcessBaseName;
  }
}
