// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.ProgressEventArgs
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;

#nullable disable
namespace ICSharpCode.SharpZipLib.Core
{
  public class ProgressEventArgs : EventArgs
  {
    private string name_;
    private long processed_;
    private long target_;
    private bool continueRunning_ = true;

    public ProgressEventArgs(string name, long processed, long target)
    {
      this.name_ = name;
      this.processed_ = processed;
      this.target_ = target;
    }

    public string Name => this.name_;

    public bool ContinueRunning
    {
      get => this.continueRunning_;
      set => this.continueRunning_ = value;
    }

    public float PercentComplete
    {
      get
      {
        return this.target_ <= 0L ? 0.0f : (float) ((double) this.processed_ / (double) this.target_ * 100.0);
      }
    }

    public long Processed => this.processed_;

    public long Target => this.target_;
  }
}
