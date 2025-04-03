// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataFilters.UpdateFilterViewModel
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
using System.Windows.Media;

#nullable disable
namespace MSS_Client.ViewModel.DataFilters
{
  internal class UpdateFilterViewModel : ViewModelBase
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.DataFilters.Filter> _filterRepository;
    private readonly IRepository<Rules> _ruleRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private string _nameTextValue = string.Empty;
    private string _descriptionTextValue = string.Empty;
    private MSS.Core.Model.DataFilters.Filter _filter;
    private Brush _nameBrushValue = (Brush) Brushes.LightGray;

    [Inject]
    public UpdateFilterViewModel(IRepositoryFactory repositoryFactory, MSS.Core.Model.DataFilters.Filter filter)
    {
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._filter = filter;
      this._nameTextValue = filter.Name;
      this._descriptionTextValue = filter.Description;
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._ruleRepository = repositoryFactory.GetRepository<Rules>();
    }

    public Brush NameBrushValue
    {
      get => this._nameBrushValue;
      set
      {
        this._nameBrushValue = value;
        this.OnPropertyChanged(nameof (NameBrushValue));
      }
    }

    public string NameTextValue
    {
      get => this._nameTextValue;
      set => this._nameTextValue = value;
    }

    public string DescriptionTextValue
    {
      get => this._descriptionTextValue;
      set => this._descriptionTextValue = value;
    }

    public ICommand UpdateFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (this.NameTextValue != string.Empty)
          {
            MSS.Core.Model.DataFilters.Filter filter = new MSS.Core.Model.DataFilters.Filter()
            {
              Id = this._filter.Id,
              Name = this._nameTextValue,
              Description = this._descriptionTextValue
            };
            this.GetFilterManagerInstance().UpdateFilter(filter);
            this.OnRequestClose(true);
          }
          else
            this.NameBrushValue = (Brush) Brushes.Red;
        }));
      }
    }

    private FilterManager GetFilterManagerInstance() => new FilterManager(this._repositoryFactory);
  }
}
