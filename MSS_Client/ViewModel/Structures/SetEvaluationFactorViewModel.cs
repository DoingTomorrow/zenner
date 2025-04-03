// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.SetEvaluationFactorViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Reporting;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class SetEvaluationFactorViewModel : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly StructureNodeDTO _selectedNode;
    private IRepository<Heaters> _heatersRepository;
    private IRepository<Meter> _meterRepository;
    private ISession _nhSession;
    private ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private Heaters _selectedHeater;

    [Inject]
    public SetEvaluationFactorViewModel(
      StructureNodeDTO selectedNode,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._selectedNode = StructuresHelper.UnreferencedStructureNode(selectedNode);
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.InitializeRepositories();
      this.NodesCollection.Add(StructuresHelper.GetStructureByDeviceType(this._selectedNode, (IEnumerable<DeviceTypeEnum>) new List<DeviceTypeEnum>()
      {
        DeviceTypeEnum.M7,
        DeviceTypeEnum.M6
      }));
    }

    private void InitializeRepositories()
    {
      this._nhSession = this._repositoryFactory.GetSession();
      this._heatersRepository = this._repositoryFactory.GetRepository<Heaters>();
      this._meterRepository = this._repositoryFactory.GetRepository<Meter>();
    }

    protected StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    public ObservableCollection<StructureNodeDTO> NodesCollection
    {
      get
      {
        StructureImageHelper.SetImageIconPath(this._nodesCollection);
        return this._nodesCollection;
      }
      set
      {
        this._nodesCollection = value;
        this.OnPropertyChanged(nameof (NodesCollection));
      }
    }

    public List<Heaters> HeatersCollection => this._heatersRepository.GetAll().ToList<Heaters>();

    public Heaters SelectedHeater
    {
      get => this._selectedHeater;
      set
      {
        this._selectedHeater = value;
        this.OnPropertyChanged(nameof (SelectedHeater));
      }
    }

    public ICommand SetEvaluationFactorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ObservableCollection<object> observableCollection = parameter as ObservableCollection<object>;
          Heaters selectedHeater = this.SelectedHeater;
          if (observableCollection == null)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_SelectAtLeatADevice, false);
          bool flag = false;
          foreach (object obj in (Collection<object>) observableCollection)
          {
            if (obj is StructureNodeDTO structureNodeDto2 && structureNodeDto2.Entity is MeterDTO)
              flag = true;
          }
          if (!flag)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_SelectAtLeatADevice, false);
          if (selectedHeater == null)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_SelectHeater, false);
          if (!(observableCollection != null & flag) || selectedHeater == null)
            return;
          List<StructureNodeDTO> metersStructureNodes = StructuresHelper.GetMeters(this.NodesCollection);
          bool isNotADeviceMessage = true;
          TypeHelperExtensionMethods.ForEach<object>((IEnumerable<object>) observableCollection, (Action<object>) (structureNode =>
          {
            StructureNodeDTO structureNodeDto = (StructureNodeDTO) structureNode;
            if (structureNodeDto == null)
              return;
            if (structureNodeDto.Entity is MeterDTO entity2)
            {
              if (metersStructureNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (x => x.Id == structureNodeDto.Id)) != null)
                entity2.EvaluationFactor = new double?(this.SelectedHeater.EvaluationFactor);
            }
            else if (isNotADeviceMessage)
            {
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_NotADevice, false);
              isNotADeviceMessage = false;
            }
          }));
        }));
      }
    }

    public ICommand SaveEvaluationFactorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructuresHelper.GetMeters(this.NodesCollection).ForEach((Action<StructureNodeDTO>) (snDto =>
          {
            MeterDTO entity = (MeterDTO) snDto.Entity;
            Meter byId = this._repositoryFactory.GetRepository<Meter>().GetById((object) entity.Id);
            byId.EvaluationFactor = new double?(Convert.ToDouble((object) entity.EvaluationFactor));
            this._repositoryFactory.GetRepository<Meter>().Update(byId);
          }));
          EventPublisher.Publish<RefreshFixedStructuresEvent>(new RefreshFixedStructuresEvent(), (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          EventPublisher.Publish<RefreshFixedStructuresEvent>(new RefreshFixedStructuresEvent(), (IViewModel) this);
          this.OnRequestClose(false);
        }));
      }
    }
  }
}
