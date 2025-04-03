// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataFilters.RemoveFilterViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.DataFilterManagement;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Orders;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.DataFilters
{
  internal class RemoveFilterViewModel : ViewModelBase
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.DataFilters.Filter> _filterRepository;
    private readonly IRepository<Rules> _ruleRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly MSS.Core.Model.DataFilters.Filter _filter;
    private readonly IWindowFactory _windowFactory;

    [Inject]
    public RemoveFilterViewModel(
      IRepositoryFactory repositoryFactory,
      MSS.Core.Model.DataFilters.Filter filter,
      IWindowFactory windowFactory)
    {
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._ruleRepository = repositoryFactory.GetRepository<Rules>();
      this._filter = filter;
      this.ConfirmFilterDeleteDialog = string.Format(Resources.MSS_Client_DataFilters_RemoveFilterDialogConfirmation, (object) filter.Name);
    }

    public string ConfirmFilterDeleteDialog { get; set; }

    public ICommand RemoveFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          int num;
          if (!this._repositoryFactory.GetRepository<Order>().SearchFor((Expression<Func<Order, bool>>) (item => item.Filter.Id == this._filter.Id)).Any<Order>())
            num = this._repositoryFactory.GetRepository<JobDefinition>().SearchFor((Expression<Func<JobDefinition, bool>>) (item => item.Filter.Id == this._filter.Id)).Any<JobDefinition>() ? 1 : 0;
          else
            num = 1;
          if (num != 0)
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_FilterUsedByAnotherModule), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
          else
            this.GetFilterManagerInstance().RemoveFilter(this._filter.Id);
          this.OnRequestClose(true);
        }));
      }
    }

    private FilterManager GetFilterManagerInstance() => new FilterManager(this._repositoryFactory);
  }
}
