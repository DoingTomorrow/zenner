// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.AssignMetersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.DIConfiguration;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  internal class AssignMetersViewModel : ViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private StructureNodeDTO _structureNode;
    private IRepositoryFactory _repositoryFactory;
    private string _subtitle;
    private string _busyContent;
    private bool _showProgressCircle;
    private ViewModelBase _messageUserControl;
    private string _lastStartedOn;
    private string _lastReadResultOn;
    private string _lastMeterAssignedOn;
    private string _lastMeterFromEntranceAssigned;

    public AssignMetersViewModel(
      IWindowFactory windowFactory,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory)
    {
      this._windowFactory = windowFactory;
      this._structureNode = node;
      this._repositoryFactory = repositoryFactory;
      if (node.NodeType.Name == "MinomatMaster")
        this._subtitle = Resources.MSS_Master + ": " + (node.Entity != null ? ((MinomatSerializableDTO) node.Entity).RadioId : "");
      if (node.NodeType.Name == "MinomatSlave")
        this._subtitle = Resources.MSS_Slave + ": " + (node.Entity != null ? ((MinomatSerializableDTO) node.Entity).RadioId : "");
      this._lastStartedOn = Resources.AssignMeters_Expert_LastStartedOn;
      this._lastReadResultOn = Resources.AssignMeters_Expert_LastReadResultOn;
      this._lastMeterAssignedOn = Resources.AssignMeters_Expert_LastMeterAssignedOn;
      this._lastMeterFromEntranceAssigned = Resources.AssignMeters_User_LastAssignedMeterOn;
    }

    public ICommand ManuallyAssignMetersExpertCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ManuallyAssignMetersViewModel>((IParameter) new ConstructorArgument("node", (object) this._structureNode), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory)))));
      }
    }

    public List<string> GetEntrancesList => new List<string>();

    public ICommand StartTestReceptionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = "Starting test reception...";
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            Thread.Sleep(4000);
            this.ShowProgressCircle = false;
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_SuccessMessage);
              this.LastStartedOn = Resources.AssignMeters_Expert_LastStartedOn + " " + (object) DateTime.Now;
            }));
          }));
        }));
      }
    }

    public ICommand ReadTestReceptionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = "Reading test reception...";
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            Thread.Sleep(4000);
            this.ShowProgressCircle = false;
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_SuccessMessage);
              this.LastReadResultOn = Resources.AssignMeters_Expert_LastReadResultOn + " " + (object) DateTime.Now;
            }));
          }));
        }));
      }
    }

    public ICommand AssignMetersExpertCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = "Assigning meters...";
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            Thread.Sleep(4000);
            this.ShowProgressCircle = false;
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_SuccessMessage);
              this.LastMeterAssignedOn = Resources.AssignMeters_Expert_LastMeterAssignedOn + " " + (object) DateTime.Now;
            }));
          }));
        }));
      }
    }

    public ICommand AssignMetersFromEntrancesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = "Assigning meters from selected entrances...";
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            Thread.Sleep(4000);
            this.ShowProgressCircle = false;
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_SuccessMessage);
              this.LastMeterFromEntranceAssigned = Resources.AssignMeters_User_LastAssignedMeterOn + " " + (object) DateTime.Now;
            }));
          }));
        }));
      }
    }

    public string Subtitle => this._subtitle;

    public string BusyContent
    {
      get => this._busyContent;
      set
      {
        this._busyContent = value;
        this.OnPropertyChanged(nameof (BusyContent));
      }
    }

    public bool ShowProgressCircle
    {
      get => this._showProgressCircle;
      set
      {
        this._showProgressCircle = value;
        this.OnPropertyChanged(nameof (ShowProgressCircle));
      }
    }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public string LastStartedOn
    {
      get => this._lastStartedOn;
      set
      {
        this._lastStartedOn = value;
        this.OnPropertyChanged(nameof (LastStartedOn));
      }
    }

    public string LastReadResultOn
    {
      get => this._lastStartedOn;
      set
      {
        this._lastStartedOn = value;
        this.OnPropertyChanged(nameof (LastReadResultOn));
      }
    }

    public string LastMeterAssignedOn
    {
      get => this._lastStartedOn;
      set
      {
        this._lastStartedOn = value;
        this.OnPropertyChanged(nameof (LastMeterAssignedOn));
      }
    }

    public string LastMeterFromEntranceAssigned
    {
      get => this._lastMeterFromEntranceAssigned;
      set
      {
        this._lastMeterFromEntranceAssigned = value;
        this.OnPropertyChanged(nameof (LastMeterFromEntranceAssigned));
      }
    }
  }
}
