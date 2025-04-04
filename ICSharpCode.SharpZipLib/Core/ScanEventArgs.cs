// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.ScanEventArgs
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;

#nullable disable
namespace ICSharpCode.SharpZipLib.Core
{
  public class ScanEventArgs : EventArgs
  {
    private string name_;
    private bool continueRunning_ = true;

    public ScanEventArgs(string name) => this.name_ = name;

    public string Name => this.name_;

    public bool ContinueRunning
    {
      get => this.continueRunning_;
      set => this.continueRunning_ = value;
    }
  }
}
