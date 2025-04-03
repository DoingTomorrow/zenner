// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Configuration.FirmwareUpdateViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using EDC_Handler;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using PDC_Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Configuration
{
  public class FirmwareUpdateViewModel : ViewModelBase
  {
    private Dictionary<string, IFirmwareConfigurator> _firmwareCache;
    private DeviceModel _selectedDeviceModel;
    private readonly IWindowFactory _windowFactory;
    private List<EDC_Handler.Firmware> _EDCFirmwareUpdates;
    private List<PDC_Handler.Firmware> _PDCFirmwareUpdates;
    private List<string> _firmwareUpdatesCollection;
    private string _selectedFirmwareUpdate;
    private bool _isFirmwareDropDownVisible;
    private bool _isProgressBarVisible;
    private bool _areButtonsEnabled;
    private int _progressBarValue;

    public FirmwareUpdateViewModel(
      IWindowFactory windowFactory,
      Dictionary<string, IFirmwareConfigurator> firmwareCache,
      DeviceModel selectedDeviceModel)
    {
      this._windowFactory = windowFactory;
      this._firmwareCache = firmwareCache;
      this._selectedDeviceModel = selectedDeviceModel;
      this.IsFirmwareDropDownVisible = true;
      this.IsProgressBarVisible = false;
      this.AreButtonsEnabled = true;
      EventPublisher.Register<MSS.Business.Events.ShowMessage>(new Action<MSS.Business.Events.ShowMessage>(this.ShowMessage));
      this.InitializeFirmwareUpdatesCollection();
    }

    public List<string> FirmwareUpdatesCollection
    {
      get => this._firmwareUpdatesCollection;
      set
      {
        this._firmwareUpdatesCollection = value;
        this.OnPropertyChanged(nameof (FirmwareUpdatesCollection));
      }
    }

    public string SelectedFirmwareUpdate
    {
      get => this._selectedFirmwareUpdate;
      set
      {
        this._selectedFirmwareUpdate = value;
        this.OnPropertyChanged(nameof (SelectedFirmwareUpdate));
      }
    }

    public bool IsFirmwareDropDownVisible
    {
      get => this._isFirmwareDropDownVisible;
      set
      {
        this._isFirmwareDropDownVisible = value;
        this.OnPropertyChanged(nameof (IsFirmwareDropDownVisible));
      }
    }

    public bool IsProgressBarVisible
    {
      get => this._isProgressBarVisible;
      set
      {
        this._isProgressBarVisible = value;
        this.OnPropertyChanged(nameof (IsProgressBarVisible));
      }
    }

    public bool AreButtonsEnabled
    {
      get => this._areButtonsEnabled;
      set
      {
        this._areButtonsEnabled = value;
        this.OnPropertyChanged(nameof (AreButtonsEnabled));
      }
    }

    public int ProgressBarValue
    {
      get => this._progressBarValue;
      set
      {
        this._progressBarValue = value;
        this.OnPropertyChanged(nameof (ProgressBarValue));
      }
    }

    public ICommand UpdateFirmwareCommad
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (!this._firmwareCache.TryGetValue(this._selectedDeviceModel.Name, out IFirmwareConfigurator _))
            return;
          bool hasAnyExceptionOccurred = false;
          bool wasFirmwareSuccessfullyUpdated = false;
          Task.Run<bool>((Func<bool>) (() =>
          {
            this.IsFirmwareDropDownVisible = false;
            this.IsProgressBarVisible = true;
            this.AreButtonsEnabled = false;
            firmwareCfg.InitializeFirmwareConfigurator(this._selectedDeviceModel.Name == "EDC Radio" ? this._EDCFirmwareUpdates.FirstOrDefault<EDC_Handler.Firmware>((Func<EDC_Handler.Firmware, bool>) (item => item.Version.ToString() == this.SelectedFirmwareUpdate)) : (EDC_Handler.Firmware) null, this._selectedDeviceModel.Name == "PDC Radio" ? this._PDCFirmwareUpdates.FirstOrDefault<PDC_Handler.Firmware>((Func<PDC_Handler.Firmware, bool>) (item => item.Version.ToString() == this.SelectedFirmwareUpdate)) : (PDC_Handler.Firmware) null);
            firmwareCfg.OnProgress += new EventHandler<int>(this.FirmwareCfg_OnProgress);
            bool updateFirmwareCommad;
            try
            {
              firmwareCfg.UpgradeFirmWare();
              updateFirmwareCommad = true;
            }
            catch (Exception ex)
            {
              hasAnyExceptionOccurred = true;
              updateFirmwareCommad = false;
              MessageHandler.LogException(ex);
              Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Error.ToString(), ex.Message, false)));
            }
            return updateFirmwareCommad;
          })).ContinueWith((Action<Task<bool>>) (result =>
          {
            firmwareCfg?.Dispose();
            this.IsFirmwareDropDownVisible = true;
            this.IsProgressBarVisible = false;
            this.AreButtonsEnabled = true;
            wasFirmwareSuccessfullyUpdated = result.Result && !hasAnyExceptionOccurred;
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              GenericMessageViewModel messageViewModel;
              if (!wasFirmwareSuccessfullyUpdated)
                messageViewModel = DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_MessageCodes_ErrorUpgradingFirmware), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false));
              else
                messageViewModel = DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Client_SuccessMessage), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_MessageCodes_SuccessOperation), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false));
              this._windowFactory.CreateNewModalDialog((IViewModel) messageViewModel);
            }));
          }));
        }));
      }
    }

    private void FirmwareCfg_OnProgress(object sender, int e) => this.ProgressBarValue = e;

    private void InitializeFirmwareUpdatesCollection()
    {
      if (this._selectedDeviceModel.Name == "EDC Radio")
      {
        this._EDCFirmwareUpdates = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_Radio));
        this.FirmwareUpdatesCollection = this._EDCFirmwareUpdates.Select<EDC_Handler.Firmware, string>((Func<EDC_Handler.Firmware, string>) (item => item.Version.ToString())).ToList<string>();
        this.SelectedFirmwareUpdate = this._EDCFirmwareUpdates.FirstOrDefault<EDC_Handler.Firmware>((Func<EDC_Handler.Firmware, bool>) (item => item.Version.ToString() == "1.11.0:EDC_Radio"))?.Version.ToString();
      }
      else
      {
        if (!(this._selectedDeviceModel.Name == "PDC Radio"))
          return;
        this._PDCFirmwareUpdates = PDC_Database.LoadFirmware(new PDC_DeviceIdentity?(PDC_DeviceIdentity.PDC_WmBus));
        this.FirmwareUpdatesCollection = this._PDCFirmwareUpdates.Select<PDC_Handler.Firmware, string>((Func<PDC_Handler.Firmware, string>) (item => item.Version.ToString())).ToList<string>();
        this.SelectedFirmwareUpdate = this._PDCFirmwareUpdates.FirstOrDefault<PDC_Handler.Firmware>((Func<PDC_Handler.Firmware, bool>) (item => item.Version.ToString() == "1.5.9:PDC_WmBus"))?.Version.ToString();
      }
    }

    private void ShowMessage(MSS.Business.Events.ShowMessage msg)
    {
      MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), msg.Message.MessageText, false);
    }
  }
}
