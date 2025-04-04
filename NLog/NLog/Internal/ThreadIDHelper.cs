// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ThreadIDHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Internal
{
  internal abstract class ThreadIDHelper
  {
    static ThreadIDHelper()
    {
      if (PlatformDetector.IsWin32)
        ThreadIDHelper.Instance = (ThreadIDHelper) new Win32ThreadIDHelper();
      else
        ThreadIDHelper.Instance = (ThreadIDHelper) new PortableThreadIDHelper();
    }

    public static ThreadIDHelper Instance { get; private set; }

    public abstract int CurrentProcessID { get; }

    public abstract string CurrentProcessName { get; }

    public abstract string CurrentProcessBaseName { get; }
  }
}
