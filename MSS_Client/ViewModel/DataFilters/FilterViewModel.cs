// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataFilters.FilterViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Core.Model.DataFilters;
using MSS.DIConfiguration;
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
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.DataFilters
{
  public class FilterViewModel : ViewModelBase
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.DataFilters.Filter> _filterRepository;
    private readonly IRepository<Rules> _ruleRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private ViewModelBase _messageUserControl;
    private MSS.Core.Model.DataFilters.Filter _selectedFilter;
    private IEnumerable<MSS.Core.Model.DataFilters.Filter> _getFilters;
    private IEnumerable<Rules> _getRules;
    private string _pageSize = string.Empty;

    [Inject]
    public FilterViewModel(
      IRepositoryFactory repositoryFactory,
      IEnumerable<MSS.Core.Model.DataFilters.Filter> getRules,
      IWindowFactory windowFactory)
    {
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._ruleRepository = repositoryFactory.GetRepository<Rules>();
      this._getFilters = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll();
      this.PageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
    }

    public MSS.Core.Model.DataFilters.Filter SelectedFilter
    {
      get => this._selectedFilter;
      set
      {
        this._selectedFilter = value;
        this._getRules = (IEnumerable<Rules>) this._ruleRepository.SearchFor((Expression<Func<Rules, bool>>) (x => x.Filter == this._selectedFilter));
        this.OnPropertyChanged("GetRules");
      }
    }

    public IEnumerable<MSS.Core.Model.DataFilters.Filter> GetFilters => this._getFilters;

    public IEnumerable<Rules> GetRules => this._getRules;

    public ICommand CreateFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddFilterViewModel>());
          this.MessageUserControl = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this._getFilters = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll();
          this.OnPropertyChanged("GetFilters");
        }));
      }
    }

    public ICommand UpdateFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          MSS.Core.Model.DataFilters.Filter filter = parameter as MSS.Core.Model.DataFilters.Filter;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<UpdateFilterViewModel>((IParameter) new ConstructorArgument("filter", (object) filter)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this._filterRepository.Refresh((object) filter.Id);
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this._getFilters = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll();
          this.OnPropertyChanged("GetFilters");
        }));
      }
    }

    public ICommand RemoveFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          MSS.Core.Model.DataFilters.Filter filter = parameter as MSS.Core.Model.DataFilters.Filter;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<RemoveFilterViewModel>((IParameter) new ConstructorArgument("filter", (object) filter)));
          this.MessageUserControl = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this._getFilters = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll();
          this.OnPropertyChanged("GetFilters");
        }));
      }
    }

    public ICommand CreateRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          MSS.Core.Model.DataFilters.Filter filter = parameter as MSS.Core.Model.DataFilters.Filter;
          if (filter == null)
            return;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddRuleViewModel>((IParameter) new ConstructorArgument("filter", (object) filter)));
          this.MessageUserControl = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this._getRules = (IEnumerable<Rules>) this._ruleRepository.SearchFor((Expression<Func<Rules, bool>>) (x => x.Filter == filter));
          this.OnPropertyChanged("GetRules");
        }));
      }
    }

    public ICommand UpdateRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          Rules rules = parameter as Rules;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<UpdateRuleViewModel>((IParameter) new ConstructorArgument("rule", (object) rules), (IParameter) new ConstructorArgument("isManualDelete", (object) false)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this._ruleRepository.Refresh((object) rules.Id);
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this._getRules = (IEnumerable<Rules>) this._ruleRepository.SearchFor((Expression<Func<Rules, bool>>) (x => x.Filter == this._selectedFilter));
          this.OnPropertyChanged("GetRules");
        }));
      }
    }

    public ICommand RemoveRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          Rules rules = parameter as Rules;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<RemoveRuleViewModel>((IParameter) new ConstructorArgument("rule", (object) rules), (IParameter) new ConstructorArgument("isManualDelete", (object) false)));
          this.MessageUserControl = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this._getRules = (IEnumerable<Rules>) this._ruleRepository.SearchFor((Expression<Func<Rules, bool>>) (x => x.Filter == this._selectedFilter));
          this.OnPropertyChanged("GetRules");
        }));
      }
    }

    public ICommand CloseWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          this.OnRequestClose(true);
          EventPublisher.Publish<RefreshFilters>(new RefreshFilters()
          {
            isRefresh = true
          }, (IViewModel) this);
        }));
      }
    }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
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
  }
}
