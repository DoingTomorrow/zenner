// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.RepairModeViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MinomatHandler;
using MSS.Business.DTO;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class RepairModeViewModel : ViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly MinomatSerializableDTO _minomatMockup = new MinomatSerializableDTO()
    {
      RadioId = "47201259"
    };
    private bool _isChangeMinomatCommandEnabled;
    private bool _isBusy;
    private string _newMinomatID;
    private string _firmwareVersion;
    private string _minomatId;
    private string _receivedMeters;
    private ViewModelBase _messageUserControlRepairMode;
    private object _gsmState;
    private PhaseDetails _actualPhase;
    private string _minomatPhaseText;
    private string _resetConfigText;
    private string _busyContent;
    private object _readMinolIdText;
    private bool _isRegisteredWithMasterVisible;
    public Func<ConnectionProfile, MinomatV4> InitMinomat = (Func<ConnectionProfile, MinomatV4>) (_ => GmmInterface.HandlerManager.CreateInstance<MinomatHandlerFunctions>(_).MyMinomatV4);

    public MinomatSerializableDTO SelectedMinomat { get; set; }

    public StructureNodeDTO SelectedMinomatStructureNode { get; set; }

    public object Image { get; set; }

    public RepairModeViewModel(
      StructureNodeDTO minomatNode,
      IWindowFactory windowFactory,
      IRepositoryFactory repositoryFactory)
    {
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this.SelectedMinomatStructureNode = minomatNode;
      this.SelectedMinomat = (MinomatSerializableDTO) minomatNode.Entity;
      this.ConfigureParams();
      this._receivedMeters = "1231231";
      this.CurrentMinomat = this.InitMinomat(this._connectionProfile);
      this.CurrentMinomat.OnError += (EventHandlerEx<Exception>) ((s, e) => { });
      this.IsRegisteredWithMasterVisible = this.SelectedMinomat.IsMaster;
    }

    private ConnectionProfile _connectionProfile { get; set; }

    public bool IsChangeMinomatCommandEnabled
    {
      get => this._isChangeMinomatCommandEnabled;
      set
      {
        if (this._isChangeMinomatCommandEnabled == value)
          return;
        this._isChangeMinomatCommandEnabled = value;
        this.OnPropertyChanged(nameof (IsChangeMinomatCommandEnabled));
      }
    }

    public bool IsBusy
    {
      get => this._isBusy;
      set
      {
        if (this._isBusy == value)
          return;
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
      }
    }

    public MinomatV4 CurrentMinomat { get; set; }

    public string NewMinomatID
    {
      get => this._newMinomatID;
      set
      {
        if (this._newMinomatID == value)
          return;
        this._newMinomatID = value;
        this.OnPropertyChanged(nameof (NewMinomatID));
      }
    }

    public string FirmwareVersion
    {
      get => this._firmwareVersion;
      set
      {
        if (this._firmwareVersion == value)
          return;
        this._firmwareVersion = value;
        this.OnPropertyChanged(nameof (FirmwareVersion));
      }
    }

    public string MinomatId
    {
      get => this._minomatId;
      set
      {
        if (this._minomatId == value)
          return;
        this._minomatId = value;
        this.OnPropertyChanged(nameof (MinomatId));
      }
    }

    public string ReceivedMeters
    {
      get => this._receivedMeters;
      set
      {
        if (this._receivedMeters == value)
          return;
        this._receivedMeters = value;
        this.OnPropertyChanged(nameof (ReceivedMeters));
      }
    }

    public ViewModelBase MessageUserControlRepairMode
    {
      get => this._messageUserControlRepairMode;
      set
      {
        this._messageUserControlRepairMode = value;
        this.OnPropertyChanged(nameof (MessageUserControlRepairMode));
      }
    }

    public object GSMState
    {
      get => this._gsmState;
      set
      {
        this._gsmState = value;
        this.OnPropertyChanged(nameof (GSMState));
      }
    }

    public PhaseDetails ActualPhase
    {
      get => this._actualPhase;
      set
      {
        this._actualPhase = value;
        this.OnPropertyChanged("ReadPhaseDetails");
      }
    }

    public string MinomatPhaseText
    {
      get => this._minomatPhaseText;
      set
      {
        this._minomatPhaseText = value;
        this.OnPropertyChanged(nameof (MinomatPhaseText));
      }
    }

    public string ResetConfigText
    {
      get => this._resetConfigText;
      set
      {
        this._resetConfigText = value;
        this.OnPropertyChanged(nameof (ResetConfigText));
      }
    }

    public string BusyContent
    {
      get => this._busyContent;
      set
      {
        this._busyContent = value;
        this.OnPropertyChanged(nameof (BusyContent));
      }
    }

    public object ReadMinolIdText
    {
      get => this._readMinolIdText;
      set
      {
        this._readMinolIdText = value;
        this.OnPropertyChanged(nameof (ReadMinolIdText));
      }
    }

    public bool IsRegisteredWithMasterVisible
    {
      get => this._isRegisteredWithMasterVisible;
      set
      {
        this._isRegisteredWithMasterVisible = value;
        this.OnPropertyChanged(nameof (IsRegisteredWithMasterVisible));
      }
    }

    public void ConfigureParams()
    {
      this._connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(29);
      this.GetChangeableParam("minomatv4_minolid", this._connectionProfile).Value = this.SelectedMinomat.RadioId;
      this.GetChangeableParam("port", this._connectionProfile).Value = "COM114";
    }

    public ChangeableParameter GetChangeableParam(string paramName, ConnectionProfile connection)
    {
      return connection.ChangeableParameters.Find((Predicate<ChangeableParameter>) (_ => _.Key.ToLower() == paramName.ToLower()));
    }

    public System.Windows.Input.ICommand ChangeMinomantCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MinomatIdPopupViewModel idPopupViewModel = DIConfigurator.GetConfigurator().Get<MinomatIdPopupViewModel>();
          this._windowFactory.CreateNewModalDialog((IViewModel) idPopupViewModel);
          idPopupViewModel.MinomatId.IfNull<string>((Action<string>) (v => this.MessageUserControlRepairMode = MessageHandlingManager.ShowValidationMessage("Enter a Minomat ID")), (Action<string>) (v =>
          {
            this.IsBusy = true;
            this.BusyContent = Resources.MSS_Client_RepairMode_ChangingMinomat;
            Task.Delay(5000).ContinueWith<bool>((Func<Task, bool>) (t => this.IsBusy = false));
          }));
        }));
      }
    }

    public System.Windows.Input.ICommand ReadFirmwareVersionCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ => Task.Run((Action) (() =>
        {
          this.IsBusy = true;
          this.BusyContent = Resources.MSS_Client_RepairMode_ReadingFirmwareVersion;
          this.FirmwareVersion = this.CurrentMinomat.GetFirmwareVersion() ?? "null";
        })).ContinueWith<bool>((Func<Task, bool>) (p => this.IsBusy = false))));
      }
    }

    public System.Windows.Input.ICommand ReadGSMState
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.IsBusy = true;
          this.BusyContent = Resources.MSS_Client_RepairMode_ReadingGSMState;
          Task.Run<GsmState>((Func<GsmState>) (() =>
          {
            GsmState gsmState = this.CurrentMinomat.GetGsmState();
            gsmState.IfNull<GsmState>((Action<GsmState>) (p => this.GSMState = (object) "null"), (Action<GsmState>) (p => this.GSMState = (object) p.StateA));
            return gsmState;
          })).ContinueWith((Action<Task<GsmState>>) (gsmStateTask => this.IsBusy = false));
        }));
      }
    }

    public System.Windows.Input.ICommand ReadPhaseDetails
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.IsBusy = true;
          this.BusyContent = Resources.MSS_Client_RepairMode_ReadingPhaseDetails;
          Task.Run((Action) (() => this.CurrentMinomat.GetPhaseDetails().IfNull<PhaseDetails>((Action<PhaseDetails>) (p => { }), (Action<PhaseDetails>) (p => this.ActualPhase = p)))).ContinueWith<bool>((Func<Task, bool>) (t => this.IsBusy = false));
        }));
      }
    }

    public System.Windows.Input.ICommand ResetConfiguration
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.IsBusy = true;
          this.BusyContent = Resources.MSS_Client_RepairMode_ResettingConfiguration;
          Task.Run<string>((Func<string>) (() => this.CurrentMinomat.ResetConfiguration().If<bool, string>((Func<bool, bool>) (p => p), (Func<bool, string>) (p => this.ResetConfigText = string.Format("Done - {0}", (object) DateTime.Now)), (Func<bool, string>) (p => this.ResetConfigText = string.Format("Error - {0}", (object) DateTime.Now))))).ContinueWith((Action<Task<string>>) (t =>
          {
            this.IsBusy = false;
            IRepository<MinomatRadioDetails> repository4 = this._repositoryFactory.GetRepository<MinomatRadioDetails>();
            MinomatRadioDetails minomatRadioDetails = repository4.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == this.SelectedMinomat.Id));
            if (minomatRadioDetails == null)
              return;
            minomatRadioDetails.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.Open);
            minomatRadioDetails.LastStartOn = new DateTime?();
            minomatRadioDetails.LastRegisteredOn = new DateTime?();
            minomatRadioDetails.GSMStatus = new GSMTestReceptionState?();
            minomatRadioDetails.GSMStatusDate = new DateTime?();
            minomatRadioDetails.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.Open);
            minomatRadioDetails.NrOfAssignedDevices = string.Empty;
            minomatRadioDetails.NrOfReceivedDevices = string.Empty;
            minomatRadioDetails.NrOfRegisteredDevices = string.Empty;
            repository4.Update(minomatRadioDetails);
            IRepository<MinomatMeter> repository5 = this._repositoryFactory.GetRepository<MinomatMeter>();
            IRepository<MinomatMeter> repository6 = repository5;
            Expression<Func<MinomatMeter, bool>> predicate = (Expression<Func<MinomatMeter, bool>>) (item => item.Minomat.Id == minomatRadioDetails.Minomat.Id);
            foreach (MinomatMeter entity in repository6.Where(predicate).ToList<MinomatMeter>())
              repository5.TransactionalDelete(entity);
            this._repositoryFactory.GetSession().Clear();
          }));
        }));
      }
    }

    public System.Windows.Input.ICommand SetMinomatPhase
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ => Task.Run<PhaseDetails>((Func<PhaseDetails>) (() =>
        {
          this.IsBusy = true;
          this.BusyContent = Resources.MSS_Client_RepairMode_GettingPhaseDetails;
          return this.ActualPhase ?? this.CurrentMinomat.GetPhaseDetails();
        })).ContinueWith((Action<Task<PhaseDetails>>) (phase =>
        {
          phase.Result.IfNotNull<PhaseDetails>((Action<PhaseDetails>) (td =>
          {
            this.BusyContent = Resources.MSS_Client_RepairMode_SettingPhaseDetails;
            this.CurrentMinomat.SetPhaseDetailsBuffer((object) td).If<bool, string>((Func<bool, bool>) (res => res.IsTrue()), (Func<bool, string>) (res => this.MinomatPhaseText = string.Format("Done - {0}", (object) DateTime.Now)), (Func<bool, string>) (res => this.MinomatPhaseText = string.Format("Error - {0}", (object) DateTime.Now)));
          }));
          this.IsBusy = false;
          this.MinomatPhaseText = this.MinomatPhaseText ?? Resources.MSS_Client_RepairMode_ErrorGettingPhaseDetails;
        }))));
      }
    }

    public System.Windows.Input.ICommand ReadMinolId
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.IsBusy = true;
          this.BusyContent = Resources.MSS_Client_RepairMode_ReadingMinolId;
          Task.Run<object>((Func<object>) (() => this.ReadMinolIdText = (object) string.Format("Minol ID : {0}", (object) this.CurrentMinomat.GetMinolId()))).ContinueWith<bool>((Func<Task<object>, bool>) (t => this.IsBusy = false));
        }));
      }
    }

    public System.Windows.Input.ICommand WriteMinolId
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MinomatIdPopupViewModel idPopupViewModel = DIConfigurator.GetConfigurator().Get<MinomatIdPopupViewModel>();
          this._windowFactory.CreateNewModalDialog((IViewModel) idPopupViewModel);
          string minomatId = idPopupViewModel.MinomatId;
        }));
      }
    }
  }
}
