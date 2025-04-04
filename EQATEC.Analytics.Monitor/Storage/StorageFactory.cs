// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.StorageFactory
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Model;
using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Threading;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal class StorageFactory : IStorageFactory, IDisposable
  {
    private const int StorageFactoryVersion = 4;
    private readonly ILogAnalyticsMonitor m_log;
    private readonly IStorage m_storage;
    private readonly string m_productId;
    private readonly StorageDataDescriptor m_policyData;
    private readonly StorageDataDescriptor m_sessionData;
    private readonly StorageDataDescriptor m_metaData;
    private readonly Random m_random = new Random();

    internal StorageFactory(ILogAnalyticsMonitor log, Guid productId, IStorage storage)
    {
      this.m_log = Guard.IsNotNull<ILogAnalyticsMonitor>(log, nameof (log));
      this.m_storage = Guard.IsNotNull<IStorage>(storage, nameof (storage));
      this.m_productId = productId.ToString("N");
      this.m_policyData = new StorageDataDescriptor(this.m_productId, StorageDataType.Policy);
      this.m_sessionData = new StorageDataDescriptor(this.m_productId, StorageDataType.SessionData);
      this.m_metaData = new StorageDataDescriptor(this.m_productId, StorageDataType.Statistics);
    }

    public void Dispose()
    {
      if (this.m_storage == null || !(this.m_storage is IDisposable))
        return;
      ((IDisposable) this.m_storage).Dispose();
    }

    public void SavePolicy(MonitorPolicy policy)
    {
      TimeSpan uptime = Timing.Uptime;
      DateTime now = Timing.Now;
      this.SaveDataWithRetries(this.m_policyData, StorageSerializer.SerializePolicy(4, policy, now, uptime), 2, false);
    }

    public void SaveStatistics(MonitorPolicy policy)
    {
      this.SaveDataWithRetries(this.m_metaData, StorageSerializer.SerializeStatistics(4, policy), 2, false);
    }

    public void SaveSessions(Statistics statistics, MonitorPolicy policy)
    {
      TimeSpan uptime = Timing.Uptime;
      DateTime now = Timing.Now;
      StorageLevel[] storageLevelArray = new StorageLevel[3]
      {
        StorageLevel.All,
        StorageLevel.CurrentSession,
        StorageLevel.OnlyMonitorSettings
      };
      foreach (StorageLevel level in storageLevelArray)
      {
        byte[] data = StorageSerializer.SerializeSessionStatistics(4, policy, statistics, now, uptime, level);
        int num = data.Length / 1024;
        if (num < policy.SettingsRestrictions.MaxStorageSizeInKB.Value || level == StorageLevel.OnlyMonitorSettings)
        {
          this.SaveDataWithRetries(this.m_sessionData, data, 1, false);
          break;
        }
        this.m_log.LogMessage(string.Format("Cannot save current statistics within storage limitations; {0} KB needed but only {1} KB available; only saving {2}", (object) num, (object) policy.SettingsRestrictions.MaxStorageSizeInKB.Value, level == StorageLevel.CurrentSession ? (object) "current session" : (object) "meta data"));
      }
    }

    private byte[] LoadDataWithRetries(
      StorageDataDescriptor descriptor,
      int retriesLeft,
      bool startWithDelay)
    {
      try
      {
        if (startWithDelay)
          Thread.Sleep(this.m_random.Next(1, 50));
        return this.m_storage.Load(descriptor);
      }
      catch (Exception ex)
      {
        if (retriesLeft > 0)
          return this.LoadDataWithRetries(descriptor, retriesLeft - 1, true);
        this.m_log.LogError("Failed to load " + descriptor.DataType.ToString().ToLower() + " data; " + ex.Message);
        throw;
      }
    }

    private void SaveDataWithRetries(
      StorageDataDescriptor descriptor,
      byte[] data,
      int retriesLeft,
      bool startWithDelay)
    {
      try
      {
        if (startWithDelay)
          Thread.Sleep(this.m_random.Next(1, 50));
        this.m_storage.Save(descriptor, data);
      }
      catch (Exception ex)
      {
        if (retriesLeft > 0)
          this.SaveDataWithRetries(descriptor, data, retriesLeft - 1, true);
        else
          this.m_log.LogError("Failed to save " + descriptor.DataType.ToString().ToLower() + " data. " + ex.Message);
      }
    }

    public Statistics LoadFromStorage(MonitorPolicy policy)
    {
      TimeSpan uptime = Timing.Uptime;
      DateTime now = Timing.Now;
      try
      {
        StorageSerializer.DeserializeIntoPolicy(this.LoadDataWithRetries(this.m_policyData, 2, false), policy, now, uptime);
        StorageSerializer.DeserializeStatistics(this.LoadDataWithRetries(this.m_metaData, 2, false), policy);
        Statistics stats = StorageSerializer.DeserializeSessionStatistics(this.LoadDataWithRetries(this.m_sessionData, 0, false), now, uptime);
        this.LoadAbandonedData(stats);
        return stats;
      }
      catch (Exception ex)
      {
        string name = this.m_storage.GetType().Name;
        this.m_log.LogError("Failed to parse the raw binary data into statistics state: " + ex.Message);
        throw new InternalMonitorException(string.Format("StorageError ({0}), Type: ({1}), Message:{2}", (object) name, (object) ex.GetType().Name, (object) ex.Message), ex);
      }
    }

    private void LoadAbandonedData(Statistics stats)
    {
      try
      {
        int num1 = 5;
        int num2 = 0;
        while (num1 > num2++)
        {
          byte[] storageData = this.m_storage.ReadAbandonedSessionData(this.m_productId);
          if (storageData == null)
            break;
          try
          {
            TimeSpan uptime = Timing.Uptime;
            DateTime now = Timing.Now;
            Statistics statistics = StorageSerializer.DeserializeSessionStatistics(storageData, now, uptime);
            if (statistics != null)
            {
              if (statistics.Sessions.Count > 0)
              {
                foreach (Session session1 in statistics.Sessions)
                {
                  bool flag = false;
                  foreach (Session session2 in stats.Sessions)
                  {
                    if (session2.Id == session1.Id)
                    {
                      flag = true;
                      break;
                    }
                  }
                  if (!flag)
                    stats.Sessions.Add(session1);
                }
              }
            }
          }
          catch (Exception ex)
          {
            this.m_log.LogMessage("Failed to parse abandoned data (no actions are required): " + ex.Message);
          }
        }
      }
      catch (Exception ex)
      {
        this.m_log.LogMessage("Failed to consume abandoned data (no actions are required): " + ex.Message);
      }
    }
  }
}
