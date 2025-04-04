// Decompiled with JetBrains decompiler
// Type: NLog.Time.AccurateUtcTimeSource
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Time
{
  [TimeSource("AccurateUTC")]
  public class AccurateUtcTimeSource : TimeSource
  {
    public override DateTime Time => DateTime.UtcNow;

    public override DateTime FromSystemTime(DateTime systemTime) => systemTime.ToUniversalTime();
  }
}
