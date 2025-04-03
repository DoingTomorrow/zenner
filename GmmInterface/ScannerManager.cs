// Decompiled with JetBrains decompiler
// Type: ZENNER.ScannerManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using NLog;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class ScannerManager
  {
    private static Logger logger = LogManager.GetLogger(nameof (ScannerManager));
    private CancellationTokenSource cancellationTokenSource;

    public event EventHandler<ZENNER.CommonLibrary.Entities.Meter> OnMeterFound;

    public event EventHandler<Exception> OnError;

    public event EventHandler<int> OnProgress;

    public event EventHandler<string> OnProgressMessage;

    public List<ZENNER.CommonLibrary.Entities.Meter> Meters { get; private set; }

    public event System.EventHandler BatterieLow;

    public void BeginScan(EquipmentModel equipment, DeviceModel system, ProfileType profileType)
    {
      if (this.cancellationTokenSource != null && !this.cancellationTokenSource.IsCancellationRequested)
        return;
      if (system == null)
        throw new ArgumentNullException(nameof (system));
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      if (equipment == null)
        throw new ArgumentNullException(nameof (equipment));
      ConnectionProfile profile = GmmInterface.DeviceManager.GetConnectionProfile(system, equipment, profileType);
      if (profile == null)
        throw new ArgumentNullException("profile");
      profile.EquipmentModel.ChangeableParameters = equipment.ChangeableParameters;
      profile.DeviceModel.ChangeableParameters = system.ChangeableParameters;
      profile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
      this.cancellationTokenSource = new CancellationTokenSource();
      this.Meters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      Devices.DeviceManager deviceManager = GmmInterface.Devices;
      CultureInfo lang = Thread.CurrentThread.CurrentUICulture;
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
            ZENNER.CommonLibrary.Entities.Meter e2 = new ZENNER.CommonLibrary.Entities.Meter();
            e2.DeviceModel = ReadoutConfigFunctions.Manager.DetermineDeviceModel(e);
            if (e2.DeviceModel == null)
              e2.DeviceModel = system;
            e2.SerialNumber = e.SerialNumber;
            if (!string.IsNullOrEmpty(e.Manufacturer))
            {
              if (e2.AdditionalInfo == null)
                e2.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
              e2.AdditionalInfo.Add(AdditionalInfoKey.Manufacturer, e.Manufacturer);
            }
            if (!string.IsNullOrEmpty(e.Version))
            {
              if (e2.AdditionalInfo == null)
                e2.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
              e2.AdditionalInfo.Add(AdditionalInfoKey.Version, e.Version);
            }
            if (!string.IsNullOrEmpty(e.DeviceType))
            {
              if (e2.AdditionalInfo == null)
                e2.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
              e2.AdditionalInfo.Add(AdditionalInfoKey.Medium, e.DeviceType);
            }
            if (!string.IsNullOrEmpty(e.ZDF))
            {
              if (e2.AdditionalInfo == null)
                e2.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
              e2.AdditionalInfo.Add(AdditionalInfoKey.ZDF, e.ZDF);
            }
            if (!string.IsNullOrEmpty(e.MainDeviceSerialNumber))
            {
              if (e2.AdditionalInfo == null)
                e2.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
              e2.AdditionalInfo.Add(AdditionalInfoKey.MainDeviceSecondaryAddress, e.MainDeviceSerialNumber);
              e2.AdditionalInfo.Add(AdditionalInfoKey.InputNumber, e.Channel.ToString());
            }
            if (!string.IsNullOrEmpty(e.PrimaryAddress))
            {
              if (e2.AdditionalInfo == null)
                e2.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
              e2.AdditionalInfo.Add(AdditionalInfoKey.PrimaryAddress, e.PrimaryAddress);
            }
            if (!this.Meters.Exists((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == e.SerialNumber)))
              this.Meters.Add(e2);
            if (this.OnMeterFound == null)
              return;
            this.OnMeterFound((object) this, e2);
          }
        });
        try
        {
          ZR_ClassLibMessages.RegisterThreadErrorMsgList();
          if (this.OnProgress != null)
            this.OnProgress((object) this, 1);
          ConfigList configListObject = profile.GetConfigListObject();
          deviceManager.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
          deviceManager.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
          deviceManager.OnProgressMessage += new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError += new EventHandlerEx<Exception>(this.DeviceManager_OnError);
          deviceManager.ValueIdentSetReceived += eventHandler;
          deviceManager.BatterieLow += new System.EventHandler(this.AsynCom_BatterieLow);
          deviceManager.PrepareCommunicationStructure(configListObject);
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
            if (this.OnProgress != null)
              this.OnProgress((object) this, 2);
            if (deviceManager.BeginSearchDevices())
              return;
            string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
            if (this.OnError == null)
              return;
            this.OnError((object) this, new Exception(errorDescription));
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
          deviceManager.OnProgressMessage -= new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
          deviceManager.OnError -= new EventHandlerEx<Exception>(this.DeviceManager_OnError);
          deviceManager.ValueIdentSetReceived -= eventHandler;
          deviceManager.BatterieLow -= new System.EventHandler(this.AsynCom_BatterieLow);
          this.cancellationTokenSource = (CancellationTokenSource) null;
          ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
          if (this.OnProgress != null)
            this.OnProgress((object) this, 100);
          if (this.OnProgressMessage != null)
            this.OnProgressMessage((object) this, string.Empty);
        }
      }), this.cancellationTokenSource.Token);
    }

    public void CancelScan()
    {
      if (this.cancellationTokenSource == null)
        return;
      this.cancellationTokenSource.Cancel();
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
      this.OnError(sender, e);
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
