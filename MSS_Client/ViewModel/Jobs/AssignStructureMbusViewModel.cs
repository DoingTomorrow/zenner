// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.AssignStructureMbusViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Structures;
using MSS.Interfaces;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class AssignStructureMbusViewModel : ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly StructuresManager _structuresManager;
    private StructureNodeDTO _selectedStructureNodeDto;
    private bool _isLocation;
    private IEnumerable<StructureNodeDTO> _structureNodeCollection;
    private bool _isExpanded;
    private bool _isBusy;

    public AssignStructureMbusViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._structuresManager = new StructuresManager(this._repositoryFactory);
      this.IsLocation = false;
      this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) this._structuresManager.GetStructureNodesCollection(StructureTypeEnum.Physical, true);
      EventPublisher.Register<LoadSubNodesForRoot>(new Action<LoadSubNodesForRoot>(this.LoadSubNodesForRootNodeEvent));
    }

    private StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private void LoadSubNodesForRootNodeEvent(LoadSubNodesForRoot rootNodeEv)
    {
      this.IsBusy = true;
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) => StructuresHelper.LoadSubNodesForRootNode(this._repositoryFactory, rootNodeEv.RootNode, this.GetStructuresManagerInstance()));
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
      {
        if (!args.Cancelled && args.Error != null)
        {
          MessageHandler.LogException(args.Error);
          MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
        }
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => this.IsBusy = false));
      });
      backgroundWorker.RunWorkerAsync((object) this._repositoryFactory);
    }

    public StructureNodeDTO SelectedStructureNodeItem
    {
      get => this._selectedStructureNodeDto;
      set
      {
        this._selectedStructureNodeDto = value;
        this.IsLocation = this._selectedStructureNodeDto != null && this._selectedStructureNodeDto.RootNode.Id == this._selectedStructureNodeDto.Id && this._selectedStructureNodeDto.ParentNode == null;
        this.OnPropertyChanged(nameof (SelectedStructureNodeItem));
      }
    }

    public ICommand AssignStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          EventPublisher.Publish<AssignStructureEvent>(new AssignStructureEvent()
          {
            StructureNode = this.SelectedStructureNodeItem
          }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    public ICommand SearchCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          string searchText = parameter as string;
          ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
          if (searchText != string.Empty)
            observableCollection = this._structuresManager.GetStructures(searchText, StructureTypeEnum.Physical);
          else
            this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) this._structuresManager.GetStructureNodesCollection(StructureTypeEnum.Physical, true);
          this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) observableCollection;
        }));
      }
    }

    public bool IsLocation
    {
      get => this._isLocation;
      set
      {
        this._isLocation = value;
        this.OnPropertyChanged(nameof (IsLocation));
      }
    }

    public IEnumerable<StructureNodeDTO> StructureNodeCollection
    {
      get => this._structureNodeCollection;
      set
      {
        this._structureNodeCollection = value;
        this.OnPropertyChanged(nameof (StructureNodeCollection));
      }
    }

    public bool IsExpanded
    {
      get => this._isExpanded;
      set
      {
        this._isExpanded = value;
        this.OnPropertyChanged(nameof (IsExpanded));
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
  }
}
