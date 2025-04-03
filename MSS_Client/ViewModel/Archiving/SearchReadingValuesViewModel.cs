// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.SearchReadingValuesViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Archiving;
using MSS.Core.Model.Archiving;
using MSS.DTO.Archive;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Utils.Utils;
using MSSArchive.Core.Model.Meters;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  public class SearchReadingValuesViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private bool _isBusy;
    private int _archiveNumberOfRecords;
    private string _searchString;
    public VirtualQueryableCollectionView<ArchiveMeterReadingValueDTO> _archiveMeterReadingValuesDTO;

    [Inject]
    public SearchReadingValuesViewModel(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this.ArchiveNumberOfRecords = this.GetReadingValuesCount(ArchiveManagerNHibernate.GetSessionFactoryMSSArchive(), "");
    }

    public IList<ArchiveMeterReadingValueDTO> GetReadingValuesDTO(
      ISessionFactory sessionFactory,
      string searchText,
      int startIndex,
      int pageSize)
    {
      List<ArchiveMeterReadingValueDTO> readingValuesDto = new List<ArchiveMeterReadingValueDTO>();
      using (IStatelessSession statelessSession = sessionFactory.OpenStatelessSession())
        readingValuesDto = (List<ArchiveMeterReadingValueDTO>) statelessSession.CreateCriteria<ArchiveMeterReadingValue>("RV").Add((ICriterion) Restrictions.Like("RV.MeterSerialNumber", (object) ("%" + searchText + "%"))).SetProjection((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.Id)), "Id").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.MeterId)), "MeterId").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => r.MeterSerialNumber)), "MeterSerialNumber").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.Date)), "Date").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.Value)), "Value").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.ValueId)), "ValueId").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.CreatedOn)), "CreatedOn").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.ExportedOn)), "ExportedOn").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.UnitId)), "UnitId").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.PhysicalQuantity)), "PhysicalQuantity").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.MeterType)), "MeterType").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.Calculation)), "Calculation").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.CalculationStart)), "CalculationStart").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.StorageInterval)), "StorageInterval").Add((IProjection) Projections.Property<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, object>>) (r => (object) r.Creation)), "Creation")).AddOrder(new Order("Date", false)).SetFirstResult(startIndex).SetMaxResults(pageSize).SetResultTransformer(Transformers.AliasToBean<ArchiveMeterReadingValueDTO>()).List<ArchiveMeterReadingValueDTO>();
      return (IList<ArchiveMeterReadingValueDTO>) readingValuesDto;
    }

    private int GetReadingValuesCount(ISessionFactory sessionFactory, string searchText)
    {
      int readingValuesCount;
      using (IStatelessSession statelessSession = sessionFactory.OpenStatelessSession())
      {
        if (!string.IsNullOrEmpty(searchText))
          readingValuesCount = LinqExtensionMethods.Query<ArchiveMeterReadingValue>(statelessSession).Count<ArchiveMeterReadingValue>((Expression<Func<ArchiveMeterReadingValue, bool>>) (rv => rv.MeterSerialNumber.Contains(searchText)));
        else
          readingValuesCount = LinqExtensionMethods.Query<ArchiveMeterReadingValue>(statelessSession).Count<ArchiveMeterReadingValue>();
      }
      return readingValuesCount;
    }

    public VirtualQueryableCollectionView<ArchiveMeterReadingValueDTO> LoadCollection(
      string searchText)
    {
      ISessionFactory sessionFactoryMSSArchive = ArchiveManagerNHibernate.GetSessionFactoryMSSArchive();
      VirtualQueryableCollectionView<ArchiveMeterReadingValueDTO> queryableCollectionView = new VirtualQueryableCollectionView<ArchiveMeterReadingValueDTO>();
      queryableCollectionView.LoadSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<int>("LoadSizeForVirtualScrolling");
      queryableCollectionView.VirtualItemCount = this.GetReadingValuesCount(sessionFactoryMSSArchive, searchText);
      VirtualQueryableCollectionView<ArchiveMeterReadingValueDTO> collection = queryableCollectionView;
      collection.ItemsLoading += (EventHandler<VirtualQueryableCollectionViewItemsLoadingEventArgs>) (async (s, args) =>
      {
        this.IsBusy = true;
        IList<ArchiveMeterReadingValueDTO> archiveMeterReadingValueDtos = (IList<ArchiveMeterReadingValueDTO>) null;
        await Task.Run((Action) (() => archiveMeterReadingValueDtos = this.GetReadingValuesDTO(sessionFactoryMSSArchive, searchText, args.StartIndex, args.ItemCount)));
        collection.Load(args.StartIndex, (IEnumerable) archiveMeterReadingValueDtos);
        this.IsBusy = false;
      });
      return collection;
    }

    public ICommand SearchReadingValueCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!this.IsValid)
            return;
          string searchText = parameter as string;
          EventPublisher.Publish<ArchiveSearched>(new ArchiveSearched()
          {
            SearchedText = searchText
          }, (IViewModel) this);
          if (string.IsNullOrEmpty(searchText))
            return;
          this.ArchiveMeterReadingValuesDTO = this.LoadCollection(searchText);
          if (this.ArchiveMeterReadingValuesDTO.Count != 0)
            return;
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.No_Item_found.GetStringValue()
          };
        }));
      }
    }

    private ArchiveJob ArchiveJob { get; set; }

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

    public int ArchiveNumberOfRecords
    {
      get => this._archiveNumberOfRecords;
      set
      {
        this._archiveNumberOfRecords = value;
        this.OnPropertyChanged(nameof (ArchiveNumberOfRecords));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Archiving_FilterSearch_SerialNumber")]
    public string SearchString
    {
      get => this._searchString;
      set
      {
        this._searchString = value;
        this.OnPropertyChanged(nameof (SearchString));
      }
    }

    public VirtualQueryableCollectionView<ArchiveMeterReadingValueDTO> ArchiveMeterReadingValuesDTO
    {
      get => this._archiveMeterReadingValuesDTO;
      set
      {
        this._archiveMeterReadingValuesDTO = value;
        this.OnPropertyChanged(nameof (ArchiveMeterReadingValuesDTO));
      }
    }
  }
}
