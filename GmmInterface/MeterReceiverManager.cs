// Decompiled with JetBrains decompiler
// Type: ZENNER.MeterReceiverManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using NLog;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class MeterReceiverManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeterReceiverManager));
    private CancellationTokenSource cancellationTokenSource;

    public MeterReceiverManager() => this.StoreResultsToDatabase = false;

    public List<Job> Jobs { get; private set; }

    public bool StoreResultsToDatabase { get; set; }

    public event EventHandler<int> OnProgress;

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandler<Exception> OnError;

    public event System.EventHandler ConnectionLost;

    public event EventHandler<Job> OnJobCompleted;

    internal void AddJob(Job job)
    {
      throw new NotImplementedException("Radio jobs are not supported!");
    }

    public void RemoveJob(Guid jobID)
    {
      Job job = (Job) null;
      lock (this.Jobs)
        job = this.Jobs.Find((Predicate<Job>) (x => x.JobID == jobID));
      if (job == null)
        return;
      this.RemoveJob(job);
    }

    internal void RemoveJob(Job job)
    {
      throw new NotImplementedException("Radio jobs are not supported!");
    }

    public void Dispose() => this.StopRead();

    public void StartRead(ZENNER.CommonLibrary.Entities.Meter meter, EquipmentModel equipmentModel, ProfileType profileType)
    {
      DeviceModel system = meter != null ? meter.DeviceModel : throw new ArgumentNullException(nameof (meter));
      List<ZENNER.CommonLibrary.Entities.Meter> meters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      meters.Add(meter);
      EquipmentModel equipmentModel1 = equipmentModel;
      ProfileType profileType1 = profileType;
      this.StartRead(system, meters, equipmentModel1, profileType1);
    }

    public void StartRead(
      DeviceModel system,
      List<ZENNER.CommonLibrary.Entities.Meter> meters,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      if (this.cancellationTokenSource != null)
        return;
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      if (equipmentModel == null)
        throw new ArgumentNullException(nameof (equipmentModel));
      if (system == null && (meters == null || meters.Count == 0))
        throw new ArgumentNullException(nameof (system));
      if (system == null)
        system = meters[0].DeviceModel;
      if (meters == null)
        meters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      this.cancellationTokenSource = new CancellationTokenSource();
      bool receiveAllMeters = meters.Count == 0;
      CultureInfo lang = Thread.CurrentThread.CurrentUICulture;
      Devices.DeviceManager deviceManager = GmmInterface.Devices;
      Task.Factory.StartNew((Action) (() =>
      {
        Thread.CurrentThread.CurrentUICulture = lang;
        EventHandler<ValueIdentSet> eventHandler = (EventHandler<ValueIdentSet>) ((sender, e) =>
        {
          if (deviceManager == null)
            return;
          if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            deviceManager.BreakRequest = this.cancellationTokenSource.IsCancellationRequested;
          }
          else
          {
            ZENNER.CommonLibrary.Entities.Meter meter = meters.Find((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == e.SerialNumber));
            if (this.StoreResultsToDatabase)
            {
              try
              {
                List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, e.SerialNumber);
                DriverTables.MeterMSSRow meterMssRow = meterMss == null || meterMss.Count != 1 ? (DriverTables.MeterMSSRow) null : meterMss[0];
                if (meterMssRow == null)
                {
                  if (meter != null)
                    meterMssRow = MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, meter.ID, e.SerialNumber);
                  else if (receiveAllMeters)
                    meterMssRow = MeterMSS.AddMeterMSS(GmmInterface.Database.BaseDbConnection, Guid.NewGuid(), e.SerialNumber);
                }
                if (meterMssRow != null)
                {
                  if (meter != null)
                  {
                    MeterDatabase.SaveMeterValuesMSS(meterMssRow.MeterID, meterMssRow.SerialNumber, e.AvailableValues);
                  }
                  else
                  {
                    if (receiveAllMeters)
                    {
                      DeviceModel deviceModel = ReadoutConfigFunctions.Manager.DetermineDeviceModel(e);
                      if (deviceModel != null)
                        meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                        {
                          ID = meterMssRow.MeterID,
                          SerialNumber = meterMssRow.SerialNumber,
                          DeviceModel = deviceModel
                        });
                      else
                        meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                        {
                          ID = meterMssRow.MeterID,
                          SerialNumber = meterMssRow.SerialNumber,
                          DeviceModel = system
                        });
                    }
                    MeterDatabase.SaveMeterValuesMSS(meterMssRow.MeterID, meterMssRow.SerialNumber, e.AvailableValues);
                  }
                }
              }
              catch (Exception ex)
              {
                if (meter != null)
                {
                  if (this.OnError != null)
                  {
                    string message = Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message;
                    this.OnError((object) this, (Exception) new InvalidMeterException(meter, message));
                  }
                }
                else if (this.OnError != null)
                  this.OnError((object) this, new Exception(Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message));
              }
            }
            else if (receiveAllMeters && meter == null)
            {
              DeviceModel deviceModel = ReadoutConfigFunctions.Manager.DetermineDeviceModel(e);
              if (deviceModel != null)
                meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                {
                  ID = Guid.NewGuid(),
                  SerialNumber = e.SerialNumber,
                  DeviceModel = deviceModel
                });
              else
                meters.Add(new ZENNER.CommonLibrary.Entities.Meter()
                {
                  ID = Guid.NewGuid(),
                  SerialNumber = e.SerialNumber,
                  DeviceModel = system
                });
            }
            if (meter != null)
              e.AvailableValues = ValueIdent.FilterMeterValues(e.AvailableValues, meter.Filter);
            if (!receiveAllMeters && meter == null || this.ValueIdentSetReceived == null)
              return;
            this.ValueIdentSetReceived(sender, e);
          }
        });
        EventHandlerEx<Exception> eventHandlerEx = (EventHandlerEx<Exception>) ((sender, e) =>
        {
          if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested)
          {
            deviceManager.BreakRequest = this.cancellationTokenSource.IsCancellationRequested;
          }
          else
          {
            if (this.OnError == null)
              return;
            this.OnError(sender, (Exception) new InvalidMeterException((ZENNER.CommonLibrary.Entities.Meter) null, e));
          }
        });
        try
        {
          ZR_ClassLibMessages.RegisterThreadErrorMsgList();
          if (this.OnProgress != null)
            this.OnProgress((object) this, 1);
          ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(system, equipmentModel, profileType);
          if (connectionProfile == null)
          {
            if (this.OnError == null)
              return;
            this.OnError((object) this, new Exception("The connection profile does not exist!"));
          }
          else
          {
            connectionProfile.EquipmentModel.ChangeableParameters = equipmentModel.ChangeableParameters;
            connectionProfile.DeviceModel.ChangeableParameters = system.ChangeableParameters;
            connectionProfile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
            deviceManager.PrepareCommunicationStructure(connectionProfile.GetConfigListObject());
            deviceManager.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
            deviceManager.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
            deviceManager.OnError += eventHandlerEx;
            deviceManager.ValueIdentSetReceived += eventHandler;
            deviceManager.ConnectionLost += new System.EventHandler(this.DeviceManager_ConnectionLost);
            deviceManager.BreakRequest = false;
            if (!deviceManager.Open())
            {
              if (this.OnError == null)
                return;
              this.OnError((object) this, new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription));
            }
            else
            {
              if (this.cancellationTokenSource.IsCancellationRequested)
                return;
              if (meters != null)
              {
                SortedList<string, string> settingsList = connectionProfile.GetSettingsList();
                foreach (ZENNER.CommonLibrary.Entities.Meter meter in meters)
                {
                  if (!string.IsNullOrEmpty(meter.SerialNumber))
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
                      if (this.OnError != null)
                        this.OnError((object) this, (Exception) new InvalidMeterException(meter, errorDescription));
                    }
                  }
                }
              }
              if (deviceManager.DeviceList_ReadAll((List<long>) null))
                return;
              string errorDescription1 = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
              if (this.OnError == null)
                return;
              this.OnError((object) this, new Exception(errorDescription1));
            }
          }
        }
        catch (Exception ex)
        {
          if (this.OnError == null)
            return;
          this.OnError((object) this, ex);
        }
        finally
        {
          deviceManager.OnMessage -= new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress -= new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnError -= eventHandlerEx;
          deviceManager.ValueIdentSetReceived -= eventHandler;
          deviceManager.ConnectionLost -= new System.EventHandler(this.DeviceManager_ConnectionLost);
          ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
          if (this.OnProgress != null)
            this.OnProgress((object) this, 0);
        }
      }), this.cancellationTokenSource.Token).ContinueWith((Action<Task>) (_ => this.cancellationTokenSource = (CancellationTokenSource) null));
    }

    public void StopRead()
    {
      if (this.cancellationTokenSource != null)
        this.cancellationTokenSource.Cancel();
      Devices.DeviceManager devices = GmmInterface.Devices;
      if (devices == null)
        return;
      devices.BreakRequest = true;
    }

    private void OnMessage(object sender, GMM_EventArgs e)
    {
      if (e == null || this.cancellationTokenSource == null)
        return;
      e.Cancel = this.cancellationTokenSource.IsCancellationRequested;
    }

    private void DeviceManager_OnProgress(object sender, int e)
    {
      if (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested || this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void DeviceManager_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }
  }
}
