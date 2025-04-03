// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataFilters.UpdateRuleViewModel
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
using System.Windows.Input;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.DataFilters
{
  internal class UpdateRuleViewModel : ViewModelBase
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
    private readonly Rules _rule;
    private int _ruleIndex;

    [Inject]
    public UpdateRuleViewModel(Rules rule, IRepositoryFactory repositoryFactory)
    {
      this._rule = rule;
      this._repositoryFactory = repositoryFactory;
      this._nhSession = repositoryFactory.GetSession();
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._ruleRepository = repositoryFactory.GetRepository<Rules>();
      this.PhysicalQuantity = rule.PhysicalQuantity;
      this.MeterType = rule.MeterType;
      this.Calculation = rule.Calculation;
      this.CalculationStart = rule.CalculationStart;
      this.StorageInterval = rule.StorageInterval;
      this.Creation = rule.Creation;
      this.RuleIndex = rule.RuleIndex;
    }

    public IEnumerable<ValueIdent.ValueIdPart_PhysicalQuantity> PhysicalQuantitiesEnumerable
    {
      get => ValueIdentHelper.GetPhysicalQuantitiesEnumerableAsValueIdPart();
    }

    public IEnumerable<ValueIdent.ValueIdPart_MeterType> MeterTypeEnumerable
    {
      get => ValueIdentHelper.GetMeterTypeEnumerableAsValueIdPart();
    }

    public IEnumerable<ValueIdent.ValueIdPart_Calculation> CalculationEnumerable
    {
      get => ValueIdentHelper.GetCalculationEnumerableAsValueIdPart();
    }

    public IEnumerable<ValueIdent.ValueIdPart_CalculationStart> CalculationStartEnumerable
    {
      get => ValueIdentHelper.GetCalculationStartEnumerableAsValueIdPart();
    }

    public IEnumerable<ValueIdent.ValueIdPart_StorageInterval> StorageIntervalEnumerable
    {
      get => ValueIdentHelper.GetStorageIntervalEnumerableAsValueIdPart();
    }

    public IEnumerable<ValueIdent.ValueIdPart_Creation> CreationEnumerable
    {
      get => ValueIdentHelper.GetCreationEnumerableAsValueIdPart();
    }

    public ValueIdent.ValueIdPart_PhysicalQuantity PhysicalQuantity
    {
      get => this._physicalQuantity;
      set
      {
        this._physicalQuantity = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_MeterType MeterType
    {
      get => this._meterType;
      set
      {
        this._meterType = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_Calculation Calculation
    {
      get => this._calculation;
      set
      {
        this._calculation = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_CalculationStart CalculationStart
    {
      get => this._calculationStart;
      set
      {
        this._calculationStart = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_StorageInterval StorageInterval
    {
      get => this._storageInterval;
      set
      {
        this._storageInterval = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ValueIdent.ValueIdPart_Creation Creation
    {
      get => this._creation;
      set
      {
        this._creation = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
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
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ICommand UpdateRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          Rules newRule = new Rules()
          {
            Id = this._rule.Id,
            MeterType = this.MeterType,
            Calculation = this.Calculation,
            CalculationStart = this.CalculationStart,
            Creation = this.Creation,
            Filter = this._rule.Filter,
            PhysicalQuantity = this.PhysicalQuantity,
            StorageInterval = this.StorageInterval,
            RuleIndex = this.RuleIndex,
            ValueId = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex)
          };
          this.GetFilterManagerInstance().UpdateRule(newRule);
          this.OnRequestClose(true);
        }));
      }
    }

    private FilterManager GetFilterManagerInstance() => new FilterManager(this._repositoryFactory);
  }
}
