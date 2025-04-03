// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataCollectors.MasterPoolViewModelBase
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.Events;
using MSS.Core.Model.Orders;
using MSS.DTO.Minomat;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.DataCollectors
{
  public class MasterPoolViewModelBase : ViewModelBase
  {
    internal IRepositoryFactory _repositoryFactory;

    [Inject]
    public MasterPoolViewModelBase(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      Mapper.CreateMap<MSS.Core.Model.DataCollectors.Minomat, MinomatDTO>();
      Mapper.CreateMap<MinomatDTO, MSS.Core.Model.DataCollectors.Minomat>().ForMember((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, object>>) (x => x.Provider), (Action<IMemberConfigurationExpression<MinomatDTO>>) (y => y.Ignore()));
    }

    public RadObservableCollection<MinomatDTO> GetDataCollectors
    {
      get
      {
        RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat> source = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>();
        IList<MSS.Core.Model.DataCollectors.Minomat> minomatList = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => !x.IsDeactivated && !x.IsInMasterPool && x.Status == StatusOrderEnum.New.ToString()));
        if (minomatList.Any<MSS.Core.Model.DataCollectors.Minomat>())
          source = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) minomatList);
        return Mapper.Map<RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>, RadObservableCollection<MinomatDTO>>(source);
      }
    }

    public RadObservableCollection<MinomatDTO> GetDataCollectorsMaster
    {
      get
      {
        RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat> source = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>();
        IList<MSS.Core.Model.DataCollectors.Minomat> minomatList = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => !x.IsDeactivated && x.IsInMasterPool && x.Status == StatusOrderEnum.New.ToString()));
        if (minomatList.Any<MSS.Core.Model.DataCollectors.Minomat>())
          source = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) minomatList);
        return Mapper.Map<RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>, RadObservableCollection<MinomatDTO>>(source);
      }
    }

    public ICommand AddToPoolCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (SelectedItems => this.UpdateMasterPool((IEnumerable<MinomatDTO>) ((IEnumerable<object>) SelectedItems).ToList<object>().Cast<MinomatDTO>().ToList<MinomatDTO>(), true)));
      }
    }

    public ICommand RemoveFromPoolCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (SelectedItems => this.UpdateMasterPool((IEnumerable<MinomatDTO>) ((IEnumerable<object>) SelectedItems).ToList<object>().Cast<MinomatDTO>().ToList<MinomatDTO>(), false)));
      }
    }

    private void UpdateMasterPool(IEnumerable<MinomatDTO> minoListDTO, bool isInMasterPool)
    {
      List<Guid> ids = minoListDTO.Select<MinomatDTO, Guid>((Func<MinomatDTO, Guid>) (m => m.Id)).ToList<Guid>();
      TypeHelperExtensionMethods.ForEach<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (m => ids.Contains(m.Id))), (Action<MSS.Core.Model.DataCollectors.Minomat>) (m =>
      {
        if (m.IsInMasterPool == isInMasterPool)
          return;
        m.IsInMasterPool = isInMasterPool;
        this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().Update(m);
      }));
      EventPublisher.Publish<MinomatUpdate>(new MinomatUpdate()
      {
        IsUpdate = true,
        Ids = ids
      }, (IViewModel) this);
      this.OnRequestClose(true);
    }
  }
}
