// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataFilters.RemoveRuleViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.DataFilterManagement;
using MSS.Core.Model.DataFilters;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.DataFilters
{
  internal class RemoveRuleViewModel : ViewModelBase
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.DataFilters.Filter> _filterRepository;
    private readonly IRepository<Rules> _ruleRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly Rules _rule;
    private bool _isManualDelete;

    [Inject]
    public RemoveRuleViewModel(
      IRepositoryFactory repositoryFactory,
      Rules rule,
      bool isManualDelete)
    {
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._ruleRepository = repositoryFactory.GetRepository<Rules>();
      this._rule = rule;
      this._isManualDelete = isManualDelete;
    }

    public ICommand RemoveRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!this._isManualDelete)
            this.GetFilterManagerInstance().RemoveRule(this._rule.Id);
          this.OnRequestClose(true);
        }));
      }
    }

    private FilterManager GetFilterManagerInstance() => new FilterManager(this._repositoryFactory);
  }
}
