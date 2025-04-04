// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessageContext
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal struct MessageContext
  {
    internal StatisticsData Statistics { get; private set; }

    internal MonitorPolicy Policy { get; private set; }

    internal MessageContext(StatisticsData statistics, MonitorPolicy policy)
      : this()
    {
      this.Statistics = statistics;
      this.Policy = policy;
    }
  }
}
