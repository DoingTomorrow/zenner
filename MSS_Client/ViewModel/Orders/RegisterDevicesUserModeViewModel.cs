// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.RegisterDevicesUserModeViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.GMM;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class RegisterDevicesUserModeViewModel : ViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private StructureNodeDTO _structureNode;
    private IRepositoryFactory _repositoryFactory;
    private IRepository<MinomatMeter> _minomatMeterRepository;
    private bool _showProgressCircle;
    private ViewModelBase _messageUserControl;

    public RegisterDevicesUserModeViewModel(
      IWindowFactory windowFactory,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory)
    {
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this._structureNode = node;
      this._minomatMeterRepository = this._repositoryFactory.GetRepository<MinomatMeter>();
      Mapper.CreateMap<MeterDTO, Meter>();
      Mapper.CreateMap<MinomatSerializableDTO, Minomat>();
    }

    public BitmapImage Image => this._structureNode.Image;

    public string Name => this._structureNode.Name;

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

    public ICommand RegisterMetersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          List<MeterDTO> metersInStructure = this.GetMetersInLocation();
          if (metersInStructure.Count <= 0)
            return;
          if (metersInStructure.Count <= 300)
          {
            this.ShowProgressCircle = true;
            ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
            {
              bool success = this.SaveAndRegisterMinomatMeters(metersInStructure);
              this.ShowProgressCircle = false;
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = success ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_ExecuteInstallationOrder_MeterRegistrationSuccessful) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_ExecuteInstallationOrder_MeterRegistrationError)));
            }));
          }
          else
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ManuallyAssignMetersViewModel>((IParameter) new ConstructorArgument("node", (object) this._structureNode), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory)));
        }));
      }
    }

    private List<MeterDTO> GetMetersInLocation()
    {
      List<MeterDTO> meterList = new List<MeterDTO>();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) this._structureNode.RootNode.SubNodes)
        this.WalkStructure(subNode, ref meterList);
      return meterList;
    }

    private void WalkStructure(StructureNodeDTO node, ref List<MeterDTO> meterList)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        if (subNode.NodeType.Name == StructureNodeTypeEnum.Meter.ToString())
          meterList.Add(subNode.Entity as MeterDTO);
        else
          this.WalkStructure(subNode, ref meterList);
      }
    }

    private bool SaveAndRegisterMinomatMeters(List<MeterDTO> meterList)
    {
      bool flag = true;
      ISession session = this._repositoryFactory.GetSession();
      try
      {
        Minomat minomat = Mapper.Map<MinomatSerializableDTO, Minomat>((MinomatSerializableDTO) this._structureNode.Entity);
        GMMMinomatConfigurator.GetInstance(minomat.IsMaster, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory")).RegisterDevicesOnMinomat(meterList, minomat);
        session.BeginTransaction();
        foreach (MeterDTO meter in meterList)
          this._minomatMeterRepository.TransactionalInsert(new MinomatMeter()
          {
            SignalStrength = 0,
            Status = new MeterStatusEnum?(MeterStatusEnum.Registered),
            Meter = Mapper.Map<MeterDTO, Meter>(meter),
            Minomat = minomat
          });
        MinomatRadioDetails radioDetails = minomat.RadioDetails;
        radioDetails.NrOfRegisteredDevices = meterList.Count.ToString();
        radioDetails.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.DevicesRegistered);
        this._repositoryFactory.GetRepository<MinomatRadioDetails>().TransactionalUpdate(radioDetails);
        session.Transaction.Commit();
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        flag = false;
        if (session.IsOpen && session.Transaction.IsActive)
          session.Transaction.Rollback();
      }
      return flag;
    }
  }
}
