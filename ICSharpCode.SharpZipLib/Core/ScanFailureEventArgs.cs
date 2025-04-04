// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;

#nullable disable
namespace ICSharpCode.SharpZipLib.Core
{
  public class ScanFailureEventArgs : EventArgs
  {
    private string name_;
    private Exception exception_;
    private bool continueRunning_;

    public ScanFailureEventArgs(string name, Exception e)
    {
      this.name_ = name;
      this.exception_ = e;
      this.continueRunning_ = true;
    }

    public string Name => this.name_;

    public Exception Exception => this.exception_;

    public bool ContinueRunning
    {
      get => this.continueRunning_;
      set => this.continueRunning_ = value;
    }
  }
}
