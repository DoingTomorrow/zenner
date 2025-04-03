// Decompiled with JetBrains decompiler
// Type: ZENNER.JobManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class JobManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (JobManager));
    private MeterReaderManager reader;
    private MeterListenerManager listener;
    private MeterReceiverManager receiver;

    public List<Job> Jobs
    {
      get
      {
        List<Job> jobs = new List<Job>();
        if (this.reader != null && this.reader.Jobs != null)
          jobs.AddRange((IEnumerable<Job>) this.reader.Jobs);
        if (this.receiver != null && this.receiver.Jobs != null)
          jobs.AddRange((IEnumerable<Job>) this.receiver.Jobs);
        if (this.listener != null && this.listener.Server.Jobs != null)
        {
          foreach (KeyValuePair<uint, List<Job>> job in this.listener.Server.Jobs)
            jobs.AddRange((IEnumerable<Job>) job.Value);
        }
        return jobs;
      }
    }

    public MeterReaderManager Reader
    {
      get
      {
        if (this.reader == null)
        {
          this.reader = new MeterReaderManager();
          this.reader.OnError += new EventHandler<Exception>(this.OnErrorForwarding);
          this.reader.OnJobCompleted += new EventHandler<Job>(this.OnJobCompletedForwarding);
          this.reader.OnJobStarted += new EventHandler<Job>(this.OnJobStartedForwarding);
          this.reader.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.ValueIdentSetReceivedForwarding);
        }
        return this.reader;
      }
    }

    public MeterListenerManager Listener
    {
      get
      {
        if (this.listener == null)
        {
          this.listener = new MeterListenerManager();
          this.listener.OnError += new EventHandler<Exception>(this.OnErrorForwarding);
          this.listener.OnJobCompleted += new EventHandler<Job>(this.OnJobCompletedForwarding);
          this.listener.OnJobStarted += new EventHandler<Job>(this.OnJobStartedForwarding);
          this.listener.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.ValueIdentSetReceivedForwarding);
          this.listener.OnMinomatConnected += new EventHandler<MinomatDevice>(this.OnMinomatConnectedForwarding);
        }
        return this.listener;
      }
    }

    public MeterReceiverManager Receiver
    {
      get
      {
        if (this.receiver == null)
        {
          this.receiver = new MeterReceiverManager();
          this.receiver.OnError += new EventHandler<Exception>(this.OnErrorForwarding);
          this.receiver.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.ValueIdentSetReceivedForwarding);
        }
        return this.receiver;
      }
    }

    public event EventHandler<Job> OnJobStarted;

    public event EventHandler<Job> OnJobCompleted;

    public event EventHandler<Exception> OnError;

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandler<MinomatDevice> OnMinomatConnected;

    public void AddJob(Job job)
    {
      JobManager.logger.Info("[GMM] Add job: " + job.ToString());
      JobManager.logger.Info("[GMM] Job next trigger time : " + job.Interval.GetNextTriggerTime().ToString("G"));
      JobManager.logger.Info("[GMM] Job interval is enabled: " + job.Interval.Enabled.ToString());
      JobManager.logger.Info("[GMM] Job interval StartDate: " + job.Interval.StartDate.ToString());
      JobManager.logger.Info("[GMM] Job interval EndDate: " + job.Interval.EndDate.ToString());
      if (job == null)
        throw new NullReferenceException(nameof (job));
      bool flag1 = job.Meters != null && job.Meters.Count > 0;
      bool flag2 = job.System != null;
      if (!flag1 && !flag2)
        throw new InvalidJobException(job, "The job has no meters and no collectors!");
      if (job.ProfileType == null)
        throw new InvalidJobException(job, "The job has no profile type!");
      TransceiverType type;
      if (!JobManager.TryDetermineTransceiverType(job, out type))
        throw new InvalidJobException(job, "Can not determine the transceiver type!");
      JobManager.logger.Info("[GMM] Job type is " + type.ToString());
      switch (type)
      {
        case TransceiverType.Listener:
          this.Listener.AddJob(job);
          break;
        case TransceiverType.Reader:
          this.Reader.AddJob(job);
          break;
        case TransceiverType.Receiver:
          this.Receiver.AddJob(job);
          break;
        default:
          throw new NotImplementedException("Unknown transceiver type: " + type.ToString());
      }
    }

    public void RemoveJob(Guid jobID)
    {
      JobManager.logger.Info("[GMM] Remove Job: " + jobID.ToString());
      if (this.reader != null)
        this.reader.RemoveJob(jobID);
      if (this.listener != null)
        this.listener.RemoveJob(jobID);
      if (this.receiver == null)
        return;
      this.receiver.RemoveJob(jobID);
    }

    public void RemoveJob(Job job)
    {
      JobManager.logger.Info("[GMM] Remove Job: " + job.ToString());
      TransceiverType type;
      if (!JobManager.TryDetermineTransceiverType(job, out type))
        throw new InvalidJobException(job, "Can not determine the transceiver type!");
      switch (type)
      {
        case TransceiverType.Listener:
          this.Listener.RemoveJob(job);
          break;
        case TransceiverType.Reader:
          this.Reader.RemoveJob(job);
          break;
        case TransceiverType.Receiver:
          this.Receiver.RemoveJob(job);
          break;
        default:
          throw new NotImplementedException("Unknown transceiver type: " + type.ToString());
      }
    }

    public void Dispose()
    {
      JobManager.logger.Info("[GMM] Dispose JobManager");
      if (this.reader != null)
      {
        this.reader.OnError -= new EventHandler<Exception>(this.OnErrorForwarding);
        this.reader.OnJobCompleted -= new EventHandler<Job>(this.OnJobCompletedForwarding);
        this.reader.OnJobStarted -= new EventHandler<Job>(this.OnJobStartedForwarding);
        this.reader.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.ValueIdentSetReceivedForwarding);
        this.reader.Dispose();
      }
      if (this.receiver != null)
      {
        this.receiver.OnError -= new EventHandler<Exception>(this.OnErrorForwarding);
        this.receiver.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.ValueIdentSetReceivedForwarding);
        this.receiver.Dispose();
      }
      if (this.listener != null)
      {
        this.listener.OnError -= new EventHandler<Exception>(this.OnErrorForwarding);
        this.listener.OnJobCompleted -= new EventHandler<Job>(this.OnJobCompletedForwarding);
        this.listener.OnJobStarted -= new EventHandler<Job>(this.OnJobStartedForwarding);
        this.listener.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.ValueIdentSetReceivedForwarding);
        this.listener.Dispose();
      }
      this.reader = (MeterReaderManager) null;
      this.receiver = (MeterReceiverManager) null;
      this.listener = (MeterListenerManager) null;
    }

    private static bool TryDetermineTransceiverType(Job job, out TransceiverType type)
    {
      if (job.System != null && job.System.Name.StartsWith("Minomat") && job.ProfileType != null && job.ProfileType.Name == "GSM")
      {
        type = TransceiverType.Listener;
        return true;
      }
      bool flag = job.Meters != null && job.Meters.Count > 0;
      if (job.System != null)
      {
        ConnectionProfile connectionProfile1 = GmmInterface.DeviceManager.GetConnectionProfile(job.System, job.Equipment, job.ProfileType);
        if (connectionProfile1 != null)
        {
          type = connectionProfile1.ConnectionSettings.TransceiverType;
          if (!flag)
            return true;
          foreach (ZENNER.CommonLibrary.Entities.Meter meter in job.Meters)
          {
            ConnectionProfile connectionProfile2 = GmmInterface.DeviceManager.GetConnectionProfile(meter.DeviceModel, job.Equipment, job.ProfileType);
            if (connectionProfile2 == null)
            {
              string message = Ot.Gtm(Tg.DB, "ConnectionProfileMissed", "No connection profile exists!") + " Meter: " + meter?.ToString() + ", Equipment: " + job.Equipment?.ToString() + ", ProfileType: " + job.ProfileType?.ToString();
              throw new InvalidJobException(job, (Exception) new InvalidMeterException(meter, message));
            }
            if (type != connectionProfile2.ConnectionSettings.TransceiverType)
            {
              string message = Ot.Gtm(Tg.CommunicationLogic, "InvalidTranseiverType", "Not all meters in job has the same transceiver type!") + " Expected: " + type.ToString() + ", Actual: " + connectionProfile2.ConnectionSettings.TransceiverType.ToString() + ", Meter: " + meter?.ToString();
              throw new InvalidJobException(job, (Exception) new InvalidMeterException(meter, message));
            }
          }
          return true;
        }
      }
      if (flag)
      {
        TransceiverType? nullable1 = new TransceiverType?();
        foreach (ZENNER.CommonLibrary.Entities.Meter meter in job.Meters)
        {
          ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(meter.DeviceModel, job.Equipment, job.ProfileType);
          if (connectionProfile == null)
          {
            string message = Ot.Gtm(Tg.DB, "ConnectionProfileForMeterMissed", "No connection profile exists for meter!") + " Equipment: " + job.Equipment?.ToString() + ", ProfileType: " + job.ProfileType?.ToString();
            throw new InvalidJobException(job, (Exception) new InvalidMeterException(meter, message));
          }
          if (!nullable1.HasValue)
          {
            nullable1 = new TransceiverType?(connectionProfile.ConnectionSettings.TransceiverType);
          }
          else
          {
            TransceiverType? nullable2 = nullable1;
            TransceiverType transceiverType = connectionProfile.ConnectionSettings.TransceiverType;
            if (!(nullable2.GetValueOrDefault() == transceiverType & nullable2.HasValue))
            {
              string message = Ot.Gtm(Tg.CommunicationLogic, "InvalidTranseiverType", "Not all meters in job has the same transceiver type!") + " Expected: " + nullable1.ToString() + ", Actual: " + connectionProfile.ConnectionSettings.TransceiverType.ToString();
              throw new InvalidJobException(job, (Exception) new InvalidMeterException(meter, message));
            }
          }
        }
        if (!nullable1.HasValue)
        {
          type = TransceiverType.None;
          return false;
        }
        type = nullable1.Value;
        return true;
      }
      type = TransceiverType.None;
      return false;
    }

    private void OnErrorForwarding(object sender, Exception e)
    {
      JobManager.logger.Info("[GMM] Event OnError: " + e?.ToString());
      if (this.OnError == null)
        return;
      this.OnError(sender, e);
    }

    private void OnJobStartedForwarding(object sender, Job e)
    {
      JobManager.logger.Info("[GMM] Event OnJobStarted: " + e?.ToString());
      if (this.OnJobStarted == null)
        return;
      this.OnJobStarted(sender, e);
    }

    private void OnJobCompletedForwarding(object sender, Job e)
    {
      JobManager.logger.Info("[GMM] Event OnJobCompleted: " + e?.ToString());
      if (this.OnJobCompleted == null)
        return;
      this.OnJobCompleted(sender, e);
    }

    private void ValueIdentSetReceivedForwarding(object sender, ValueIdentSet e)
    {
      JobManager.logger.Info("[GMM] Event ValueIdentSetReceived: " + e?.ToString());
      if (this.ValueIdentSetReceived == null)
        return;
      this.ValueIdentSetReceived(sender, e);
    }

    private void OnMinomatConnectedForwarding(object sender, MinomatDevice e)
    {
      JobManager.logger.Info("[GMM] Event OnMinomatConnected: " + e?.ToString());
      if (this.OnMinomatConnected == null)
        return;
      this.OnMinomatConnected(sender, e);
    }

    public void StartListener()
    {
      JobManager.logger.Info("[GMM] StartListener");
      if (this.Listener.Server == null)
        return;
      this.Listener.Server.Start();
    }

    public void StopListener()
    {
      JobManager.logger.Info("[GMM] StartListener");
      if (this.Listener.Server == null)
        return;
      this.Listener.Server.Stop();
    }
  }
}
