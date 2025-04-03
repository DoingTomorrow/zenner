// Decompiled with JetBrains decompiler
// Type: ZENNER.MeterReaderManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using AsyncCom;
using GmmDbLib;
using GmmDbLib.DataSets;
using MinomatHandler;
using NLog;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class MeterReaderManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeterReaderManager));
    private Scheduler scheduler;
    private CancellationTokenSource cancellationTokenSource;

    public MeterReaderManager()
    {
      this.StoreResultsToDatabase = false;
      this.Jobs = new List<Job>();
      this.scheduler = new Scheduler();
    }

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandler<Exception> OnError;

    public event EventHandler<Job> OnJobStarted;

    public event EventHandler<Job> OnJobCompleted;

    public event EventHandler<int> OnProgress;

    public event EventHandler<string> OnProgressMessage;

    public event EventHandler<ReadSettings> OnReadFinished;

    public event System.EventHandler BatterieLow;

    public List<Job> Jobs { get; private set; }

    public bool StoreResultsToDatabase { get; set; }

    public bool DontCloseConnectionAfterRead { get; set; }

    public void AddJob(Job job)
    {
      if (job.Interval == null)
      {
        this.ReadJob(job);
      }
      else
      {
        lock (this.Jobs)
        {
          MeterReaderManager.logger.Info("[GMM] AddJob " + job.JobID.ToString());
          this.Jobs.Add(job);
          MeterReaderManager.logger.Info("[GMM] Jobs.Count = " + this.Jobs.Count.ToString());
          job.Interval.OnTrigger += new Scheduler.TriggerItem.OnTriggerEventHandler(this.OnTrigger);
          MeterReaderManager.logger.Info("[GMM] scheduler.AddTrigger");
          this.scheduler.AddTrigger(job.Interval);
          MeterReaderManager.logger.Info("[GMM] scheduler.TriggerItems.Count = " + this.scheduler.TriggerItems.Count.ToString());
        }
      }
    }

    public void RemoveJob(Guid jobID)
    {
      MeterReaderManager.logger.Info("[GMM] RemoveJob " + jobID.ToString());
      Job job = (Job) null;
      lock (this.Jobs)
        job = this.Jobs.Find((Predicate<Job>) (x => x.JobID == jobID));
      if (job != null)
        this.RemoveJob(job);
      else
        MeterReaderManager.logger.Info("[GMM] RemoveJob failed. Cannot find job " + jobID.ToString());
    }

    public void RemoveJob(Job job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof (job));
      if (job.Interval == null)
        return;
      lock (this.Jobs)
      {
        MeterReaderManager.logger.Info("[GMM] RemoveJob: " + job.JobID.ToString());
        this.Jobs.Remove(job);
        job.Interval.OnTrigger -= new Scheduler.TriggerItem.OnTriggerEventHandler(this.OnTrigger);
        this.scheduler.RemoveTrigger(job.Interval);
        MeterReaderManager.logger.Info("[GMM] scheduler.TriggerItems.Count = " + this.scheduler.TriggerItems.Count.ToString());
      }
      MeterReaderManager.logger.Info("GC.Collect");
      GC.Collect();
      MeterReaderManager.logger.Info("GC.WaitForPendingFinalizers");
      GC.WaitForPendingFinalizers();
    }

    public void Dispose()
    {
      MeterReaderManager.logger.Info("[GMM] Dispose (scheduler.TriggerItems.Clear,Jobs.Clear, CancelRead)");
      this.scheduler.TriggerItems.Clear();
      if (this.Jobs != null)
        this.Jobs.Clear();
      this.CancelRead();
    }

    private void OnTrigger(object sender, Scheduler.OnTriggerEventArgs e)
    {
      lock (this.Jobs)
      {
        Job job = this.Jobs.Find((Predicate<Job>) (x => x.Interval == e.Item));
        if (job == null)
          return;
        if (job.IsInProcess)
        {
          MeterReaderManager.logger.Info("[GMM] OnTrigger job. job.IsInProcess = true, (Not need to run it, it is already runs.)");
        }
        else
        {
          string message = "Run job:" + job.JobID.ToString() + e.TriggerDate.ToString() + ": " + e.Item.Tag?.ToString() + ", next trigger: " + e.Item.GetNextTriggerTime().DayOfWeek.ToString() + ", " + e.Item.GetNextTriggerTime().ToString() + "\r\n";
          MeterReaderManager.logger.Trace(message);
          this.ReadJob(job);
        }
      }
    }

    private void ReadJob(Job job)
    {
      job.IsInProcess = true;
      if (this.OnJobStarted != null)
        this.OnJobStarted((object) this, job);
      if (job.System == null && job.Meters == null)
        throw new InvalidJobException(job, "No collector and no meters existing!");
      if (job.Equipment == null)
        throw new InvalidJobException(job, "No equipment defined!");
      if (job.ProfileType == null)
        throw new InvalidJobException(job, "No profile type defined!");
      if (job.Meters != null && job.Meters.Count > 0)
        this.ReadJobByMeters(job);
      else
        this.ReadJobBySystem(job);
    }

    private void ReadJobBySystem(Job job)
    {
      MeterReaderManager.logger.Info("[GMM] ReadJobByMeters " + job.JobID.ToString());
      this.cancellationTokenSource = new CancellationTokenSource();
      Devices.DeviceManager deviceManager = GmmInterface.Devices;
      Task.Factory.StartNew((Action) (() =>
      {
        EventHandlerEx<Exception> eventHandlerEx = (EventHandlerEx<Exception>) ((sender, e) =>
        {
          MeterReaderManager.logger.Error("deviceManager_OnError " + e?.ToString() + " Job: " + job.JobID.ToString());
          if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true " + job.JobID.ToString());
          }
          else
          {
            if (this.OnError == null)
              return;
            this.OnError(sender, (Exception) new InvalidJobException(job, e));
          }
        });
        EventHandler<ValueIdentSet> eventHandler = (EventHandler<ValueIdentSet>) ((sender, e) =>
        {
          if (deviceManager == null)
            MeterReaderManager.logger.Error("[GMM] deviceManager == null " + e?.ToString() + " Job: " + job.JobID.ToString());
          else if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true " + job.JobID.ToString());
            deviceManager.BreakRequest = this.cancellationTokenSource.IsCancellationRequested;
          }
          else
          {
            ZENNER.CommonLibrary.Entities.Meter meter = job.Meters.Find((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == e.SerialNumber));
            Guid jobId;
            if (job.StoreResultsToDatabase)
            {
              try
              {
                List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, e.SerialNumber);
                DriverTables.MeterMSSRow meterMssRow = meterMss == null || meterMss.Count != 1 ? (DriverTables.MeterMSSRow) null : meterMss[0];
                if (meterMssRow == null && meter != null)
                  meterMssRow = MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, meter.ID, e.SerialNumber);
                else if (meterMssRow == null && meter == null)
                  meterMssRow = MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, Guid.NewGuid(), e.SerialNumber);
                if (meter == null)
                  job.Meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                  {
                    ID = meterMssRow.MeterID,
                    SerialNumber = meterMssRow.SerialNumber,
                    DeviceModel = job.System
                  });
                if (meterMssRow != null)
                {
                  e.AvailableValues = ValueIdent.FilterMeterValues(e.AvailableValues, job.Filter);
                  MeterDatabase.SaveMeterValuesMSS(meterMssRow.MeterID, meterMssRow.SerialNumber, e.AvailableValues);
                }
              }
              catch (Exception ex)
              {
                Logger logger = MeterReaderManager.logger;
                string str3 = ex?.ToString();
                jobId = job.JobID;
                string str4 = jobId.ToString();
                string message3 = "[GMM] Exception " + str3 + " Job: " + str4;
                logger.Error(message3);
                if (meter != null)
                {
                  if (this.OnError != null)
                  {
                    string message4 = Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message;
                    this.OnError((object) this, (Exception) new InvalidMeterException(meter, message4));
                  }
                }
                else if (this.OnError != null)
                  this.OnError((object) this, new Exception(Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message));
              }
            }
            e.AvailableValues = ValueIdent.FilterMeterValues(e.AvailableValues, job.Filter);
            e.Tag = (object) job;
            if (this.ValueIdentSetReceived == null)
              return;
            this.ValueIdentSetReceived(sender, e);
            if (this.OnError != null && e.AvailableValues != null)
            {
              foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> availableValue in e.AvailableValues)
              {
                if (ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(availableValue.Key) == ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber && ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentWarning>(availableValue.Key) == ValueIdent.ValueIdentWarning.FailedToRead)
                {
                  Logger logger = MeterReaderManager.logger;
                  string serialNumber = e.SerialNumber;
                  jobId = job.JobID;
                  string str = jobId.ToString();
                  string message = "[GMM] ValueIdentWarning.FailedToRead SN: " + serialNumber + " Job: " + str;
                  logger.Error(message);
                  this.OnError((object) this, (Exception) new FailedToReadException(e.SerialNumber));
                }
              }
            }
          }
        });
        try
        {
          ZR_ClassLibMessages.RegisterThreadErrorMsgList();
          ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(job.System, job.Equipment, job.ProfileType);
          if (connectionProfile == null)
          {
            MeterReaderManager.logger.Error("[GMM] profile == null, Job: " + job.JobID.ToString());
            if (this.OnError == null)
              return;
            this.OnError((object) this, (Exception) new InvalidJobException(job, "The connection profile does not exist!"));
          }
          else
          {
            connectionProfile.EquipmentModel.ChangeableParameters = job.Equipment.ChangeableParameters;
            connectionProfile.DeviceModel.ChangeableParameters = job.System.ChangeableParameters;
            connectionProfile.ProfileType.ChangeableParameters = job.ProfileType.ChangeableParameters;
            ConfigList configListObject = connectionProfile.GetConfigListObject();
            deviceManager.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
            deviceManager.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
            deviceManager.OnProgressMessage += new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
            deviceManager.OnError += eventHandlerEx;
            deviceManager.ValueIdentSetReceived += eventHandler;
            deviceManager.BatterieLow += new System.EventHandler(this.AsynCom_BatterieLow);
            deviceManager.PrepareCommunicationStructure(configListObject);
            deviceManager.BreakRequest = false;
            if (!deviceManager.Open())
            {
              MeterReaderManager.logger.Error("[GMM] deviceManager.Open() == false, Job: " + job.JobID.ToString());
              if (this.OnError == null)
                return;
              this.OnError((object) this, (Exception) new InvalidJobException(job, ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription));
            }
            else if (this.cancellationTokenSource.IsCancellationRequested)
            {
              MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true " + job.JobID.ToString());
            }
            else
            {
              if (job.ServiceTask != null)
              {
                if (!this.PerformServiceTask(job, deviceManager))
                  return;
              }
              else if (!deviceManager.DeviceList_ReadAll(job.Filter))
              {
                string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                MeterReaderManager.logger.Error("[GMM] DeviceList_ReadAll == false " + job.JobID.ToString() + " " + errorDescription);
                if (this.OnError == null)
                  return;
                if (string.IsNullOrEmpty(errorDescription))
                {
                  this.OnError((object) this, (Exception) new InvalidJobException(job, "Can not read the devices!"));
                  return;
                }
                this.OnError((object) this, (Exception) new InvalidJobException(job, errorDescription));
                return;
              }
              if (this.OnJobCompleted == null)
                return;
              this.OnJobCompleted((object) this, job);
            }
          }
        }
        catch (Exception ex)
        {
          MeterReaderManager.logger.Error("[GMM] Exception: " + ex?.ToString());
          if (this.OnError == null)
            return;
          this.OnError((object) this, (Exception) new InvalidJobException(job, "Failed to read the meter list." + ex.Message));
        }
        finally
        {
          deviceManager.OnMessage -= new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress -= new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnProgressMessage -= new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError -= eventHandlerEx;
          deviceManager.ValueIdentSetReceived -= eventHandler;
          deviceManager.BatterieLow -= new System.EventHandler(this.AsynCom_BatterieLow);
          if (!this.DontCloseConnectionAfterRead)
          {
            MeterReaderManager.logger.Info("[GMM] DontCloseConnectionAfterRead: deviceManager.Close, Job: " + job.JobID.ToString());
            deviceManager.Close();
          }
          ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
          job.IsInProcess = false;
        }
      }), this.cancellationTokenSource.Token).ContinueWith((Action<Task>) (_ => this.cancellationTokenSource = (CancellationTokenSource) null));
    }

    private void ReadJobByMeters(Job job)
    {
      MeterReaderManager.logger.Info("[GMM] ReadJobByMeters " + job.JobID.ToString());
      this.cancellationTokenSource = new CancellationTokenSource();
      Task.Factory.StartNew((Action) (() =>
      {
        Devices.DeviceManager deviceManager = GmmInterface.Devices;
        EventHandlerEx<Exception> eventHandlerEx = (EventHandlerEx<Exception>) ((sender, e) =>
        {
          Logger logger3 = MeterReaderManager.logger;
          Guid jobId = job.JobID;
          string message3 = "[GMM] deviceManager_OnError, Job:  " + jobId.ToString() + " Exception: " + e?.ToString();
          logger3.Error(message3);
          if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            Logger logger4 = MeterReaderManager.logger;
            jobId = job.JobID;
            string message4 = "[GMM] cancellationTokenSource.IsCancellationRequested == true, Job: " + jobId.ToString();
            logger4.Info(message4);
          }
          else
          {
            if (this.OnError == null)
              return;
            this.OnError(sender, (Exception) new InvalidJobException(job, e));
          }
        });
        EventHandler<ValueIdentSet> eventHandler = (EventHandler<ValueIdentSet>) ((sender, e) =>
        {
          if (deviceManager == null)
            MeterReaderManager.logger.Info("[GMM] deviceManager == null ");
          else if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            deviceManager.BreakRequest = this.cancellationTokenSource.IsCancellationRequested;
            MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested");
          }
          else
          {
            ZENNER.CommonLibrary.Entities.Meter meter3 = job.Meters.Find((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == e.SerialNumber));
            if (meter3 == null)
            {
              MeterReaderManager.logger.Info("[GMM] expectedMeter == null, e.SerialNumber = " + e.SerialNumber);
              foreach (ZENNER.CommonLibrary.Entities.Meter meter4 in job.Meters)
                MeterReaderManager.logger.Info("[GMM] job.Meters " + meter4.SerialNumber);
            }
            else
            {
              List<long> filter = (List<long>) null;
              if (job.Filter != null && meter3.Filter == null)
                filter = job.Filter;
              else if (job.Filter == null && meter3.Filter != null)
                filter = meter3.Filter;
              else if (job.Filter != null && meter3.Filter != null)
                filter = meter3.Filter;
              e.AvailableValues = ValueIdent.FilterMeterValues(e.AvailableValues, filter);
              e.Tag = (object) job;
              if (job.StoreResultsToDatabase)
              {
                try
                {
                  List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, e.SerialNumber);
                  DriverTables.MeterMSSRow meterMssRow = (meterMss == null || meterMss.Count != 1 ? (DriverTables.MeterMSSRow) null : meterMss[0]) ?? MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, meter3.ID, e.SerialNumber);
                  if (meterMssRow != null)
                    MeterDatabase.SaveMeterValuesMSS(meterMssRow.MeterID, meterMssRow.SerialNumber, e.AvailableValues);
                }
                catch (Exception ex)
                {
                  MeterReaderManager.logger.Error("[GMM] Exception " + ex?.ToString());
                  if (this.OnError != null)
                  {
                    string message = Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message;
                    this.OnError((object) this, (Exception) new InvalidMeterException(meter3, message));
                  }
                }
              }
              if (this.ValueIdentSetReceived == null)
                return;
              this.ValueIdentSetReceived(sender, e);
              if (this.OnError != null && e.AvailableValues != null)
              {
                foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> availableValue in e.AvailableValues)
                {
                  if (ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(availableValue.Key) == ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber && ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentWarning>(availableValue.Key) == ValueIdent.ValueIdentWarning.FailedToRead)
                  {
                    MeterReaderManager.logger.Error("[GMM] ValueIdentWarning.FailedToRead " + e.SerialNumber);
                    this.OnError((object) this, (Exception) new FailedToReadException(e.SerialNumber));
                  }
                }
              }
            }
          }
        });
        try
        {
          ZR_ClassLibMessages.RegisterThreadErrorMsgList();
          deviceManager.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnProgressMessage += new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError += eventHandlerEx;
          deviceManager.ValueIdentSetReceived += eventHandler;
          deviceManager.BatterieLow += new System.EventHandler(this.AsynCom_BatterieLow);
          deviceManager.BreakRequest = false;
          if (job.ServiceTask != null)
          {
            if (!this.PerformServiceTask(job, deviceManager))
            {
              MeterReaderManager.logger.Trace("[GMM] PerformServiceTask == false");
              return;
            }
          }
          else
          {
            foreach (ZENNER.CommonLibrary.Entities.Meter meter in job.Meters)
            {
              if (this.cancellationTokenSource.IsCancellationRequested)
                return;
              if (meter.DeviceModel == null)
              {
                if (this.OnError != null)
                {
                  MeterReaderManager.logger.Error("[GMM] The meter has no device model!");
                  string message = Ot.Gtm(Tg.CommunicationLogic, "DeviceModelMissed", "The meter has no device model!");
                  this.OnError((object) this, (Exception) new InvalidJobException(job, (Exception) new InvalidMeterException(meter, message)));
                }
              }
              else
              {
                ConnectionProfile connectionProfile = (ConnectionProfile) null;
                if (job.System != null)
                  connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(job.System, job.Equipment, job.ProfileType);
                if (connectionProfile == null)
                  connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(meter.DeviceModel, job.Equipment, job.ProfileType);
                if (connectionProfile == null)
                {
                  if (this.OnError != null)
                  {
                    MeterReaderManager.logger.Error("[GMM] No connection profile exists!");
                    string message = Ot.Gtm(Tg.DB, "ConnectionProfileMissed", "No connection profile exists!") + " Equipment: " + job.Equipment?.ToString() + " ProfileType: " + job.ProfileType?.ToString();
                    this.OnError((object) this, (Exception) new InvalidJobException(job, (Exception) new InvalidMeterException(meter, message)));
                  }
                }
                else
                {
                  connectionProfile.EquipmentModel.ChangeableParameters = job.Equipment.ChangeableParameters;
                  connectionProfile.DeviceModel.ChangeableParameters = meter.DeviceModel.ChangeableParameters;
                  connectionProfile.ProfileType.ChangeableParameters = job.ProfileType.ChangeableParameters;
                  SortedList<string, string> settingsList = connectionProfile.GetSettingsList();
                  ConfigList configListObject = connectionProfile.GetConfigListObject();
                  deviceManager.PrepareCommunicationStructure(configListObject);
                  if (!deviceManager.Open())
                  {
                    MeterReaderManager.logger.Error("[GMM] !deviceManager.Open()");
                    if (this.OnError != null)
                    {
                      string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                      this.OnError((object) this, (Exception) new InvalidJobException(job, (Exception) new InvalidMeterException(meter, errorDescription)));
                    }
                  }
                  else
                  {
                    if (this.cancellationTokenSource.IsCancellationRequested)
                    {
                      MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested");
                      return;
                    }
                    if (!string.IsNullOrEmpty(meter.SerialNumber))
                    {
                      deviceManager.DeviceList_Clear();
                      string str = string.Empty;
                      settingsList.TryGetValue("SelectedDeviceMBusType", out str);
                      if (string.IsNullOrEmpty(str))
                      {
                        string empty = string.Empty;
                        settingsList.TryGetValue("BusMode", out empty);
                        if (empty == "MBus")
                          str = "MBus";
                      }
                      if (!deviceManager.DeviceList_AddDevice(new GlobalDeviceId()
                      {
                        Serialnumber = meter.SerialNumber,
                        DeviceTypeName = str
                      }))
                      {
                        string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                        MeterReaderManager.logger.Error("[GMM] DeviceList_AddDevice " + errorDescription);
                        if (this.OnError != null)
                        {
                          this.OnError((object) this, (Exception) new InvalidJobException(job, (Exception) new InvalidMeterException(meter, errorDescription)));
                          continue;
                        }
                        continue;
                      }
                    }
                    List<long> filter = (List<long>) null;
                    if (job.Filter != null && meter.Filter == null)
                      filter = job.Filter;
                    else if (job.Filter == null && meter.Filter != null)
                      filter = meter.Filter;
                    else if (job.Filter != null && meter.Filter != null)
                      filter = meter.Filter;
                    if (!deviceManager.DeviceList_ReadAll(filter))
                    {
                      string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                      MeterReaderManager.logger.Error("[GMM] DeviceList_ReadAll " + errorDescription);
                      if (this.OnError != null)
                        this.OnError((object) this, (Exception) new InvalidJobException(job, (Exception) new InvalidMeterException(meter, errorDescription)));
                    }
                  }
                }
              }
            }
          }
          if (this.OnJobCompleted == null)
            return;
          this.OnJobCompleted((object) this, job);
        }
        catch (Exception ex)
        {
          MeterReaderManager.logger.Error("[GMM] Exception: " + ex?.ToString());
          if (this.OnError == null)
            return;
          this.OnError((object) this, (Exception) new InvalidJobException(job, ex));
        }
        finally
        {
          deviceManager.OnMessage -= new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress -= new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnProgressMessage -= new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError -= eventHandlerEx;
          deviceManager.ValueIdentSetReceived -= eventHandler;
          deviceManager.BatterieLow -= new System.EventHandler(this.AsynCom_BatterieLow);
          if (!this.DontCloseConnectionAfterRead)
          {
            MeterReaderManager.logger.Info("[GMM] DontCloseConnectionAfterRead -> deviceManager.Close, Job: " + job.JobID.ToString());
            deviceManager.Close();
          }
          ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
          job.IsInProcess = false;
        }
      }), this.cancellationTokenSource.Token).ContinueWith((Action<Task>) (_ => this.cancellationTokenSource = (CancellationTokenSource) null));
    }

    private bool PerformServiceTask(Job job, Devices.DeviceManager deviceManager)
    {
      MeterReaderManager.logger.Info("[GMM] PerformServiceTask, job " + job.JobID.ToString());
      if (job.ServiceTask.Method == (MethodInfo) null)
      {
        MeterReaderManager.logger.Error("[GMM] job.ServiceTask.Method == null, job " + job.JobID.ToString());
        this.OnError((object) this, (Exception) new InvalidJobException(job, "Invalid service job!"));
        return false;
      }
      if (typeof (MinomatV4) == job.ServiceTask.Method.DeclaringType)
      {
        List<byte> buffer = (List<byte>) null;
        CommunicationEventHandler communicationEventHandler = (CommunicationEventHandler) ((sender, e) =>
        {
          if (e == null)
            return;
          buffer = new List<byte>();
          foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) e)
            buffer.AddRange((IEnumerable<byte>) scGiPacket.ToByteArray());
        });
        MinomatV4 minomatV4 = new MinomatV4(new SCGiConnection((IAsyncFunctions) deviceManager.MyAsyncCom));
        try
        {
          uint? minolId = minomatV4.GetMinolId();
          if (!minolId.HasValue)
          {
            MeterReaderManager.logger.Error("[GMM] Can not read the MinolID from Minomat. Job " + job.JobID.ToString());
            if (this.OnError != null)
              this.OnError((object) this, (Exception) new InvalidJobException(job, "Can not read the MinolID from Minomat."));
            return false;
          }
          minomatV4.Connection.OnResponse += communicationEventHandler;
          object o = job.ServiceTask.Method.Invoke((object) minomatV4, (object[]) null);
          XmlSerializer xmlSerializer = new XmlSerializer(job.ServiceTask.Method.ReturnType);
          StringWriter stringWriter = new StringWriter();
          xmlSerializer.Serialize((TextWriter) stringWriter, o);
          string resultObject = stringWriter.ToString();
          if (job.StoreResultsToDatabase)
          {
            MeterReaderManager.logger.Info("[GMM] StoreResultsToDatabase. Job " + job.JobID.ToString());
            ServiceTaskResult.SaveServiceTaskResult(GmmInterface.Database.BaseDbConnection, DateTime.Now, minolId.ToString(), job.JobID, Guid.Empty, job.ServiceTask.Method.ToString(), job.ServiceTask.Method.ReturnType.AssemblyQualifiedName, resultObject, buffer.ToArray());
          }
        }
        catch (Exception ex)
        {
          MeterReaderManager.logger.Info("[GMM] Exception: " + ex?.ToString());
          if (this.OnError != null)
            this.OnError((object) this, (Exception) new InvalidJobException(job, ex));
          return false;
        }
        finally
        {
          minomatV4.Connection.OnResponse -= communicationEventHandler;
        }
      }
      return true;
    }

    public void ReadMeter(ZENNER.CommonLibrary.Entities.Meter meter, EquipmentModel equipmentModel, ProfileType profileType)
    {
      if (meter == null)
        throw new ArgumentNullException(nameof (meter));
      this.ReadMeter((DeviceModel) null, (List<long>) null, new List<ZENNER.CommonLibrary.Entities.Meter>()
      {
        meter
      }, equipmentModel, profileType);
    }

    public void ReadMeter(
      List<ZENNER.CommonLibrary.Entities.Meter> meters,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      this.ReadMeter((DeviceModel) null, (List<long>) null, meters, equipmentModel, profileType);
    }

    public void ReadMeter(
      DeviceModel system,
      List<long> filter,
      List<ZENNER.CommonLibrary.Entities.Meter> meters,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      this.ReadMeterAsync(system, filter, meters, equipmentModel, profileType);
    }

    public Task ReadMeterAsync(
      DeviceModel system,
      List<long> filter,
      List<ZENNER.CommonLibrary.Entities.Meter> meters,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      MeterReaderManager.logger.Trace("Start ReadMeter");
      if (this.cancellationTokenSource != null)
        return (Task) null;
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      if (equipmentModel == null)
        throw new ArgumentNullException(nameof (equipmentModel));
      if (system == null && (meters == null || meters.Count == 0))
        throw new ArgumentNullException(nameof (system));
      if (meters == null)
        meters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      this.cancellationTokenSource = new CancellationTokenSource();
      Devices.DeviceManager deviceManager = GmmInterface.Devices;
      bool readAllMeters = meters.Count == 0;
      bool noSerialnumber = meters.Exists((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => string.IsNullOrEmpty(x.SerialNumber)));
      CultureInfo lang = Thread.CurrentThread.CurrentUICulture;
      Task task = Task.Factory.StartNew((Action) (() =>
      {
        Thread.CurrentThread.CurrentUICulture = lang;
        EventHandler<ValueIdentSet> eventHandler = (EventHandler<ValueIdentSet>) ((sender, e) =>
        {
          if (deviceManager == null)
            MeterReaderManager.logger.Error("[GMM] deviceManager == null");
          else if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            deviceManager.BreakRequest = this.cancellationTokenSource.IsCancellationRequested;
            MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true");
          }
          else
          {
            ZENNER.CommonLibrary.Entities.Meter meter2 = meters.Find((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == e.SerialNumber));
            if (this.StoreResultsToDatabase && !string.IsNullOrEmpty(e.SerialNumber))
            {
              try
              {
                List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, e.SerialNumber);
                DriverTables.MeterMSSRow meter = meterMss == null || meterMss.Count != 1 ? (DriverTables.MeterMSSRow) null : meterMss[0];
                if (meter == null)
                {
                  if (meter2 != null)
                    meter = MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, meter2.ID, e.SerialNumber);
                  else if (readAllMeters | noSerialnumber)
                    meter = MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, Guid.NewGuid(), e.SerialNumber);
                }
                if (meter != null)
                {
                  if (meter2 != null)
                  {
                    MeterDatabase.SaveMeterValuesMSS(meter.MeterID, meter.SerialNumber, e.AvailableValues);
                  }
                  else
                  {
                    if (readAllMeters | noSerialnumber)
                    {
                      DeviceModel deviceModel = ReadoutConfigFunctions.Manager.DetermineDeviceModel(e);
                      if (deviceModel != null)
                      {
                        this.UpdateChangableParameters(deviceModel, system);
                        if (noSerialnumber)
                          meters.RemoveAll((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => string.IsNullOrEmpty(x.SerialNumber)));
                        if (!string.IsNullOrEmpty(meter.SerialNumber) && meters.FindIndex((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == meter.SerialNumber)) < 0)
                          meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                          {
                            ID = meter.MeterID,
                            SerialNumber = meter.SerialNumber,
                            DeviceModel = deviceModel
                          });
                      }
                    }
                    MeterDatabase.SaveMeterValuesMSS(meter.MeterID, meter.SerialNumber, e.AvailableValues);
                  }
                }
              }
              catch (Exception ex)
              {
                MeterReaderManager.logger.Error("[GMM] Exception: " + ex?.ToString());
                if (meter2 != null)
                {
                  if (this.OnError != null)
                  {
                    string message = Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message;
                    this.OnError((object) this, (Exception) new InvalidMeterException(meter2, message));
                  }
                }
                else if (this.OnError != null)
                  this.OnError((object) this, new Exception("Failed store meter values to database! " + ex.Message));
              }
            }
            else if (((!readAllMeters ? 0 : (meter2 == null ? 1 : 0)) | (noSerialnumber ? 1 : 0)) != 0)
            {
              DeviceModel deviceModel = ReadoutConfigFunctions.Manager.DetermineDeviceModel(e);
              if (deviceModel != null)
              {
                this.UpdateChangableParameters(deviceModel, system);
                if (noSerialnumber)
                  meters.RemoveAll((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => string.IsNullOrEmpty(x.SerialNumber)));
                if (!string.IsNullOrEmpty(e.SerialNumber) && meters.FindIndex((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == e.SerialNumber)) < 0)
                  meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                  {
                    ID = Guid.NewGuid(),
                    SerialNumber = e.SerialNumber,
                    DeviceModel = deviceModel
                  });
              }
            }
            if (meter2 != null)
              e.AvailableValues = ValueIdent.FilterMeterValues(e.AvailableValues, meter2.Filter);
            if (((readAllMeters ? 1 : (meter2 != null ? 1 : 0)) | (noSerialnumber ? 1 : 0)) == 0 || this.ValueIdentSetReceived == null)
              return;
            this.ValueIdentSetReceived(sender, e);
            if (this.OnError != null && e.AvailableValues != null)
            {
              foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> availableValue in e.AvailableValues)
              {
                if (ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(availableValue.Key) == ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber && ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentWarning>(availableValue.Key) == ValueIdent.ValueIdentWarning.FailedToRead)
                {
                  MeterReaderManager.logger.Error("[GMM] ValueIdentWarning.FailedToRead SN: " + e.SerialNumber);
                  this.OnError((object) this, (Exception) new FailedToReadException(e.SerialNumber));
                }
              }
            }
          }
        });
        try
        {
          ZR_ClassLibMessages.RegisterThreadErrorMsgList();
          if (this.OnProgress != null)
            this.OnProgress((object) this, 1);
          deviceManager.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnProgressMessage += new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError += new EventHandlerEx<Exception>(this.DeviceManager_OnError);
          deviceManager.ValueIdentSetReceived += eventHandler;
          deviceManager.BatterieLow += new System.EventHandler(this.AsynCom_BatterieLow);
          if (system == null && meters.Count > 0)
          {
            foreach (ZENNER.CommonLibrary.Entities.Meter meter in new List<ZENNER.CommonLibrary.Entities.Meter>((IEnumerable<ZENNER.CommonLibrary.Entities.Meter>) meters))
            {
              if (this.cancellationTokenSource.IsCancellationRequested)
              {
                MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true ");
                break;
              }
              if (meter.DeviceModel == null)
              {
                if (this.OnError != null)
                {
                  MeterReaderManager.logger.Error("[GMM] meter.DeviceModel == null");
                  string message = Ot.Gtm(Tg.CommunicationLogic, "DeviceModelMissed", "The meter has no device model!");
                  this.OnError((object) this, (Exception) new InvalidMeterException(meter, message));
                }
              }
              else
              {
                string serialNumber = meter.SerialNumber;
                if (meter.AdditionalInfo != null && meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.MainDeviceSecondaryAddress))
                  serialNumber = meter.AdditionalInfo[AdditionalInfoKey.MainDeviceSecondaryAddress];
                ConfigList configList;
                if (meter.ConnectionAdjuster != null)
                {
                  ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(meter.ConnectionAdjuster.ConnectionProfileID);
                  if (connectionProfile == null)
                  {
                    MeterReaderManager.logger.Error("[GMM] profile == null");
                    if (this.OnError != null)
                    {
                      string message = Ot.Gtm(Tg.DB, "ConnectionProfileForMeterMissed", "No connection profile exists for meter!") + " Equipment: " + equipmentModel?.ToString() + ", ProfileType: " + profileType?.ToString();
                      this.OnError((object) this, (Exception) new InvalidMeterException(meter, message));
                      continue;
                    }
                    continue;
                  }
                  configList = meter.ConnectionAdjuster.GetMergedConfiguration(connectionProfile);
                }
                else
                {
                  ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(meter.DeviceModel, equipmentModel, profileType);
                  if (connectionProfile == null)
                  {
                    MeterReaderManager.logger.Error("[GMM] profile == null");
                    if (this.OnError != null)
                    {
                      string message = Ot.Gtm(Tg.DB, "ConnectionProfileForMeterMissed", "No connection profile exists for meter!") + " Equipment: " + equipmentModel?.ToString() + ", ProfileType: " + profileType?.ToString();
                      this.OnError((object) this, (Exception) new InvalidMeterException(meter, message));
                      continue;
                    }
                    continue;
                  }
                  connectionProfile.EquipmentModel.ChangeableParameters = equipmentModel.ChangeableParameters;
                  connectionProfile.DeviceModel.ChangeableParameters = meter.DeviceModel.ChangeableParameters;
                  connectionProfile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
                  configList = connectionProfile.GetConfigListObject();
                }
                deviceManager.PrepareCommunicationStructure(configList);
                deviceManager.BreakRequest = false;
                if (!deviceManager.Open())
                {
                  MeterReaderManager.logger.Error("[GMM] deviceManager.Open() == false");
                  if (this.OnError != null)
                  {
                    string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                    MeterReaderManager.logger.Error("[GMM] Error: " + errorDescription);
                    this.OnError((object) this, (Exception) new InvalidMeterException(meter, errorDescription));
                  }
                }
                else
                {
                  if (this.cancellationTokenSource.IsCancellationRequested)
                  {
                    MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true");
                    break;
                  }
                  if (!string.IsNullOrEmpty(serialNumber))
                  {
                    string str = string.Empty;
                    if (configList.SelectedDeviceMBusType != null)
                      str = configList.SelectedDeviceMBusType;
                    deviceManager.DeviceList_Clear();
                    if (!deviceManager.DeviceList_AddDevice(new GlobalDeviceId()
                    {
                      Serialnumber = serialNumber,
                      DeviceTypeName = str
                    }))
                    {
                      string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                      MeterReaderManager.logger.Error("[GMM] DeviceList_AddDevice Error: " + errorDescription);
                      if (this.OnError != null)
                        this.OnError((object) this, (Exception) new InvalidMeterException(meter, errorDescription));
                    }
                  }
                  deviceManager.ParameterType = ConfigurationParameter.ValueType.Complete;
                  if (this.OnProgress != null)
                    this.OnProgress((object) this, 2);
                  if (!deviceManager.DeviceList_ReadAll(meter.Filter ?? filter))
                  {
                    string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                    MeterReaderManager.logger.Error("[GMM] DeviceList_ReadAll Error: " + errorDescription);
                    if (this.OnError != null)
                      this.OnError((object) this, (Exception) new InvalidMeterException(meter, errorDescription));
                  }
                  else if (!string.IsNullOrEmpty(meter.SerialNumber))
                  {
                    List<GlobalDeviceId> list = deviceManager.DeviceList_GetList();
                    if (list != null)
                    {
                      StringBuilder stringBuilder = new StringBuilder();
                      bool flag = true;
                      foreach (GlobalDeviceId globalDeviceId in list)
                      {
                        if (globalDeviceId.Serialnumber == meter.SerialNumber)
                          flag = false;
                        else
                          stringBuilder.Append(" ").AppendLine(globalDeviceId.Serialnumber);
                        if (globalDeviceId.SubDevices != null)
                        {
                          foreach (GlobalDeviceId subDevice in globalDeviceId.SubDevices)
                          {
                            if (subDevice.Serialnumber == meter.SerialNumber)
                              flag = false;
                            else
                              stringBuilder.Append(" ").AppendLine(subDevice.Serialnumber);
                          }
                        }
                        if (!flag)
                          break;
                      }
                      if (flag)
                      {
                        MeterReaderManager.logger.Error("[GMM] wrongDevice");
                        if (this.OnError != null)
                        {
                          string message = Ot.Gtm(Tg.CommunicationLogic, "WrongSerialNumberDetected", "The read device has a different serial number than expected! ID:" + stringBuilder.ToString());
                          this.OnError((object) this, (Exception) new InvalidMeterException(meter, message));
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          else if (system != null && meters.Count > 0)
          {
            ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(system, equipmentModel, profileType);
            if (connectionProfile == null)
            {
              MeterReaderManager.logger.Error("[GMM] profile == null");
              if (this.OnError == null)
                return;
              this.OnError((object) this, new Exception("Can not find profile for System: " + system?.ToString() + ", Equipment: " + equipmentModel?.ToString() + ", Profile: " + profileType?.ToString()));
            }
            else
            {
              connectionProfile.EquipmentModel.ChangeableParameters = equipmentModel.ChangeableParameters;
              connectionProfile.DeviceModel.ChangeableParameters = system.ChangeableParameters;
              connectionProfile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
              SortedList<string, string> settingsList = connectionProfile.GetSettingsList();
              deviceManager.PrepareCommunicationStructure(connectionProfile.GetConfigListObject());
              deviceManager.BreakRequest = false;
              if (!deviceManager.Open())
              {
                MeterReaderManager.logger.Error("[GMM] deviceManager.Open == false");
                if (this.OnError == null)
                  return;
                string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                MeterReaderManager.logger.Error("[GMM] gmmError: " + errorDescription);
                this.OnError((object) this, new Exception(errorDescription));
              }
              else if (this.cancellationTokenSource.IsCancellationRequested)
              {
                MeterReaderManager.logger.Info("[GMM] cancellationTokenSource.IsCancellationRequested == true");
              }
              else
              {
                deviceManager.DeviceList_Clear();
                deviceManager.ParameterType = ConfigurationParameter.ValueType.Complete;
                if (this.OnProgress != null)
                  this.OnProgress((object) this, 2);
                foreach (ZENNER.CommonLibrary.Entities.Meter meter in meters)
                {
                  string empty = string.Empty;
                  settingsList.TryGetValue("SelectedDeviceMBusType", out empty);
                  if (!deviceManager.DeviceList_AddDevice(new GlobalDeviceId()
                  {
                    Serialnumber = meter.SerialNumber,
                    DeviceTypeName = empty
                  }))
                  {
                    string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                    MeterReaderManager.logger.Error("[GMM] DeviceList_AddDevice gmmError: " + errorDescription);
                    if (this.OnError == null)
                      return;
                    this.OnError((object) this, new Exception(errorDescription));
                    return;
                  }
                }
                if (deviceManager.DeviceList_ReadAll(filter))
                  return;
                string errorDescription1 = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                MeterReaderManager.logger.Error("[GMM] DeviceList_ReadAll gmmError: " + errorDescription1);
                if (this.OnError == null)
                  return;
                this.OnError((object) this, new Exception(errorDescription1));
              }
            }
          }
          else
          {
            ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(system, equipmentModel, profileType);
            if (connectionProfile == null)
            {
              MeterReaderManager.logger.Error("[GMM] profile == null");
              if (this.OnError == null)
                return;
              this.OnError((object) this, new Exception("Can not find profile for System: " + system?.ToString() + ", Equipment: " + equipmentModel?.ToString() + ", Profile: " + profileType?.ToString()));
            }
            else
            {
              connectionProfile.EquipmentModel.ChangeableParameters = equipmentModel.ChangeableParameters;
              connectionProfile.DeviceModel.ChangeableParameters = system.ChangeableParameters;
              connectionProfile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
              deviceManager.PrepareCommunicationStructure(connectionProfile.GetConfigListObject());
              deviceManager.BreakRequest = false;
              if (!deviceManager.Open())
              {
                MeterReaderManager.logger.Error("[GMM] profile == null");
                if (this.OnError == null)
                  return;
                this.OnError((object) this, new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription));
              }
              else
              {
                if (this.cancellationTokenSource.IsCancellationRequested)
                  return;
                deviceManager.DeviceList_Clear();
                deviceManager.ParameterType = ConfigurationParameter.ValueType.Complete;
                if (this.OnProgress != null)
                  this.OnProgress((object) this, 2);
                if (deviceManager.DeviceList_ReadAll(filter))
                  return;
                string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
                MeterReaderManager.logger.Error("[GMM] DeviceList_ReadAll == false, gmmError: " + errorDescription);
                if (this.OnError == null)
                  return;
                this.OnError((object) this, new Exception(errorDescription));
              }
            }
          }
        }
        catch (Exception ex)
        {
          MeterReaderManager.logger.Error("[GMM] Exception: " + ex?.ToString());
          if (this.OnError == null)
            return;
          this.OnError((object) this, ex);
        }
        finally
        {
          deviceManager.OnMessage -= new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress -= new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnProgressMessage -= new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError -= new EventHandlerEx<Exception>(this.DeviceManager_OnError);
          deviceManager.ValueIdentSetReceived -= eventHandler;
          deviceManager.BatterieLow -= new System.EventHandler(this.AsynCom_BatterieLow);
          if (!this.DontCloseConnectionAfterRead)
          {
            MeterReaderManager.logger.Info("[GMM] DontCloseConnectionAfterRead == false ");
            deviceManager.Close();
          }
          ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
          if (this.OnProgress != null)
            this.OnProgress((object) this, 100);
          if (this.OnProgressMessage != null)
            this.OnProgressMessage((object) this, string.Empty);
          if (this.OnReadFinished != null)
            this.OnReadFinished((object) this, new ReadSettings()
            {
              System = system,
              Filter = filter,
              Meters = meters,
              EquipmentModel = equipmentModel,
              ProfileType = profileType
            });
        }
      }), this.cancellationTokenSource.Token);
      task.ContinueWith((Action<Task>) (_ => this.cancellationTokenSource = (CancellationTokenSource) null));
      MeterReaderManager.logger.Trace("End ReadMeter");
      return task;
    }

    private void UpdateChangableParameters(DeviceModel deviceModel, DeviceModel system)
    {
      if (system == null || system.ChangeableParameters == null || deviceModel == null || deviceModel.ChangeableParameters == null)
        return;
      foreach (ChangeableParameter changeableParameter1 in deviceModel.ChangeableParameters)
      {
        ChangeableParameter p1 = changeableParameter1;
        ChangeableParameter changeableParameter2 = system.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == p1.Key));
        if (changeableParameter2 != null)
          p1.Value = changeableParameter2.Value;
      }
    }

    public void CancelRead()
    {
      MeterReaderManager.logger.Info(nameof (CancelRead));
      if (this.cancellationTokenSource == null)
        return;
      this.cancellationTokenSource.Cancel();
    }

    public void CloseConnection()
    {
      MeterReaderManager.logger.Info(nameof (CloseConnection));
      GmmInterface.Devices.Close();
    }

    private void OnMessage(object sender, GMM_EventArgs e)
    {
      if (e == null || this.cancellationTokenSource == null)
        return;
      e.Cancel = this.cancellationTokenSource.IsCancellationRequested;
    }

    private void DeviceManager_OnError(object sender, Exception e)
    {
      if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested || this.OnError == null)
        return;
      this.OnError(sender, (Exception) new InvalidMeterException((ZENNER.CommonLibrary.Entities.Meter) null, e));
    }

    private void DeviceManager_OnProgress(object sender, int e)
    {
      if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested || this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void DeviceManager_OnProgressMessage(object sender, string e)
    {
      if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested || this.OnProgressMessage == null)
        return;
      this.OnProgressMessage(sender, e);
    }

    private void AsynCom_BatterieLow(object sender, EventArgs e)
    {
      if (this.BatterieLow == null)
        return;
      this.BatterieLow(sender, e);
    }
  }
}
