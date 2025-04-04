// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.IStorageFactory
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Model;
using EQATEC.Analytics.Monitor.Policy;
using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal interface IStorageFactory : IDisposable
  {
    void SaveSessions(Statistics statistics, MonitorPolicy policy);

    void SavePolicy(MonitorPolicy policy);

    void SaveStatistics(MonitorPolicy policy);

    Statistics LoadFromStorage(MonitorPolicy policy);
  }
}
