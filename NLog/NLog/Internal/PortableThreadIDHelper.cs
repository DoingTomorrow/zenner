// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PortableThreadIDHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal class PortableThreadIDHelper : ThreadIDHelper
  {
    private const string UnknownProcessName = "<unknown>";
    private readonly int _currentProcessId;
    private string _currentProcessName;
    private string _currentProcessBaseName;

    public PortableThreadIDHelper() => this._currentProcessId = Process.GetCurrentProcess().Id;

    public override int CurrentProcessID => this._currentProcessId;

    public override string CurrentProcessName
    {
      get
      {
        this.GetProcessName();
        return this._currentProcessName;
      }
    }

    public override string CurrentProcessBaseName
    {
      get
      {
        this.GetProcessName();
        return this._currentProcessBaseName;
      }
    }

    private void GetProcessName()
    {
      if (this._currentProcessName != null)
        return;
      try
      {
        this._currentProcessName = Process.GetCurrentProcess().MainModule.FileName;
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        else
          this._currentProcessName = "<unknown>";
      }
      this._currentProcessBaseName = Path.GetFileNameWithoutExtension(this._currentProcessName);
    }
  }
}
