// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ProgressTimeArg
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary
{
  public sealed class ProgressTimeArg
  {
    public uint ReportMarkCounter = 0;

    public long PrograssTime_ms { get; private set; }

    public string Message { get; private set; }

    public object Tag { get; private set; }

    public ProgressTimeArg(uint reportMarkCounter) => this.ReportMarkCounter = reportMarkCounter;

    public ProgressTimeArg(long progressTime_ms, string message, object tag)
    {
      this.PrograssTime_ms = progressTime_ms;
      this.Message = message;
      this.Tag = tag;
    }

    public override string ToString()
    {
      if (this.ReportMarkCounter > 0U)
        return "LoggerMark: " + this.ReportMarkCounter.ToString();
      return this.Message != null ? this.PrograssTime_ms.ToString("d06") + "ms: " + this.Message : this.PrograssTime_ms.ToString("d06") + "ms: ";
    }
  }
}
