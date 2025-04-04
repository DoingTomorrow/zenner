// Decompiled with JetBrains decompiler
// Type: NLog.Common.AsyncLogEventInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Common
{
  public struct AsyncLogEventInfo(LogEventInfo logEvent, AsyncContinuation continuation)
  {
    public LogEventInfo LogEvent { get; } = logEvent;

    public AsyncContinuation Continuation { get; } = continuation;

    public static bool operator ==(AsyncLogEventInfo eventInfo1, AsyncLogEventInfo eventInfo2)
    {
      return eventInfo1.Continuation == eventInfo2.Continuation && eventInfo1.LogEvent == eventInfo2.LogEvent;
    }

    public static bool operator !=(AsyncLogEventInfo eventInfo1, AsyncLogEventInfo eventInfo2)
    {
      return eventInfo1.Continuation != eventInfo2.Continuation || eventInfo1.LogEvent != eventInfo2.LogEvent;
    }

    public override bool Equals(object obj) => this == (AsyncLogEventInfo) obj;

    public override int GetHashCode()
    {
      return this.LogEvent.GetHashCode() ^ this.Continuation.GetHashCode();
    }
  }
}
