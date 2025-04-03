// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataFilters.AddRuleViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.DataFilterManagement;
using MSS.Business.Modules.GMM;
using MSS.Core.Model.DataFilters;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.DataFilters
{
  internal class AddRuleViewModel : ViewModelBase
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.DataFilters.Filter> _filterRepository;
    private readonly IRepository<Rules> _ruleRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private ValueIdent.ValueIdPart_PhysicalQuantity _physicalQuantity = ValueIdent.ValueIdPart_PhysicalQuantity.Any;
    private ValueIdent.ValueIdPart_MeterType _meterType = ValueIdent.ValueIdPart_MeterType.Any;
    private ValueIdent.ValueIdPart_Calculation _calculation = ValueIdent.ValueIdPart_Calculation.Any;
    private ValueIdent.ValueIdPart_CalculationStart _calculationStart = ValueIdent.ValueIdPart_CalculationStart.Any;
    private ValueIdent.ValueIdPart_StorageInterval _storageInterval = ValueIdent.ValueIdPart_StorageInterval.Any;
    private ValueIdent.ValueIdPart_Creation _creation = ValueIdent.ValueIdPart_Creation.Any;
    private string _valueIdentifier;
    private readonly MSS.Core.Model.DataFilters.Filter _parentFilter;
    private int _ruleIndex;

    [Inject]
    public AddRuleViewModel(MSS.Core.Model.DataFilters.Filter filter, IRepositoryFactory repositoryFactory)
    {
      this._parentFilter = filter;
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._ruleRepository = repositoryFactory.GetRepository<Rules>();
    }

    public IEnumerable<string> PhysicalQuantitiesEnumerable
    {
      get => ValueIdentHelper.GetPhysicalQuantitiesEnumerable();
    }

    public IEnumerable<string> MeterTypeEnumerable => ValueIdentHelper.GetMeterTypeEnumerable();

    public IEnumerable<string> CalculationEnumerable => ValueIdentHelper.GetCalculationEnumerable();

    public IEnumerable<string> CalculationStartEnumerable
    {
      get => ValueIdentHelper.GetCalculationStartEnumerable();
    }

    public IEnumerable<string> StorageIntervalEnumerable
    {
      get => ValueIdentHelper.GetStorageIntervalEnumerable();
    }

    public IEnumerable<string> CreationEnumerable => ValueIdentHelper.GetCreationEnumerable();

    public ValueIdent.ValueIdPart_PhysicalQuantity PhysicalQuantity
    {
      get => this._physicalQuantity;
      set
      {
        this._physicalQuantity = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public string GetValueId()
    {
      return ((long) (this.PhysicalQuantity + (long) this.MeterType + (long) this.Calculation + (long) this.CalculationStart + (long) this.StorageInterval + (long) this.Creation + (long) this._ruleIndex * 2147483648L)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public ValueIdent.ValueIdPart_MeterType MeterType
    {
      get => this._meterType;
      set
      {
        this._meterType = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_Calculation Calculation
    {
      get => this._calculation;
      set
      {
        this._calculation = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_CalculationStart CalculationStart
    {
      get => this._calculationStart;
      set
      {
        this._calculationStart = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_StorageInterval StorageInterval
    {
      get => this._storageInterval;
      set
      {
        this._storageInterval = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_Creation Creation
    {
      get => this._creation;
      set
      {
        this._creation = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public string ValueIdentifier
    {
      get => this._valueIdentifier;
      set => this._valueIdentifier = value;
    }

    public int RuleIndex
    {
      get => this._ruleIndex;
      set
      {
        this._ruleIndex = value;
        this._valueIdentifier = this.GetValueId();
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ICommand AddRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          Rules newRule = new Rules()
          {
            MeterType = this.MeterType,
            Calculation = this.Calculation,
            CalculationStart = this.CalculationStart,
            Creation = this.Creation,
            Filter = this._parentFilter,
            PhysicalQuantity = this.PhysicalQuantity,
            StorageInterval = this.StorageInterval,
            RuleIndex = this.RuleIndex,
            ValueId = this.GetValueId()
          };
          this.GetFilterManagerInstance().CreateRule(newRule);
          this.OnRequestClose(true);
        }));
      }
    }

    private FilterManager GetFilterManagerInstance() => new FilterManager(this._repositoryFactory);
  }
}
