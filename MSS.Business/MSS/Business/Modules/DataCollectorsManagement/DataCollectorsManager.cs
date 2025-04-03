// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.DataCollectorsManagement.DataCollectorsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.Errors;
using MSS.Business.Modules.GMM;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Minomat;
using MSS.Interfaces;
using MSS.Localisation;
using NHibernate;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Windows.Data;
using ZENNER;

#nullable disable
namespace MSS.Business.Modules.DataCollectorsManagement
{
  public class DataCollectorsManager
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.DataCollectors.Minomat> _minomatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepositoryFactory _repositoryFactory;

    [Inject]
    public DataCollectorsManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._nhSession = repositoryFactory.GetSession();
      this._minomatRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>();
      this._userRepository = repositoryFactory.GetUserRepository();
    }

    public RadObservableCollection<MinomatDTO> GetDataCollectorsItems(string searchText)
    {
      RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat> source1 = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>();
      IList<MSS.Core.Model.DataCollectors.Minomat> minomatList = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => !x.IsDeactivated));
      if (minomatList.Any<MSS.Core.Model.DataCollectors.Minomat>())
        source1 = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) minomatList);
      RadObservableCollection<MinomatDTO> source2 = Mapper.Map<RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>, RadObservableCollection<MinomatDTO>>(source1);
      RadObservableCollection<MinomatDTO> dataCollectorsItems = new RadObservableCollection<MinomatDTO>();
      foreach (MinomatDTO minomatDto in source2.Where<MinomatDTO>((Func<MinomatDTO, bool>) (minomatDto =>
      {
        if (minomatDto.RadioId != null && minomatDto.RadioId.Contains(searchText) || minomatDto.ProviderName != null && minomatDto.ProviderName.Contains(searchText) || minomatDto.HostAndPort != null && minomatDto.HostAndPort.Contains(searchText) || minomatDto.Url != null && minomatDto.Url.Contains(searchText) || minomatDto.SessionKey != null && minomatDto.SessionKey.Contains(searchText) || minomatDto.Challenge != null && minomatDto.Challenge.Contains(searchText) || minomatDto.GsmId != null && minomatDto.GsmId.Contains(searchText))
          return true;
        return minomatDto.LastUpdatedBy != null && minomatDto.LastUpdatedBy.Contains(searchText);
      })))
        dataCollectorsItems.Add(minomatDto);
      return dataCollectorsItems;
    }

    public RadObservableCollection<MinomatDTO> GetDataCollectorsItemsPool(string searchText)
    {
      RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat> source1 = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>();
      IList<MSS.Core.Model.DataCollectors.Minomat> source2 = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => !x.IsDeactivated && x.IsInMasterPool));
      IList<MSS.Core.Model.DataCollectors.Minomat> minomatList = source2 ?? (IList<MSS.Core.Model.DataCollectors.Minomat>) source2.ToList<MSS.Core.Model.DataCollectors.Minomat>();
      if (minomatList.Any<MSS.Core.Model.DataCollectors.Minomat>())
        source1 = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) minomatList);
      RadObservableCollection<MinomatDTO> source3 = Mapper.Map<RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>, RadObservableCollection<MinomatDTO>>(source1);
      RadObservableCollection<MinomatDTO> collectorsItemsPool = new RadObservableCollection<MinomatDTO>();
      foreach (MinomatDTO minomatDto in source3.Where<MinomatDTO>((Func<MinomatDTO, bool>) (minomatDto =>
      {
        if (minomatDto.RadioId != null && minomatDto.RadioId.Contains(searchText) || minomatDto.ProviderName != null && minomatDto.ProviderName.Contains(searchText) || minomatDto.HostAndPort != null && minomatDto.HostAndPort.Contains(searchText) || minomatDto.Url != null && minomatDto.Url.Contains(searchText) || minomatDto.SessionKey != null && minomatDto.SessionKey.Contains(searchText) || minomatDto.Challenge != null && minomatDto.Challenge.Contains(searchText) || minomatDto.GsmId != null && minomatDto.GsmId.Contains(searchText))
          return true;
        return minomatDto.LastUpdatedBy != null && minomatDto.LastUpdatedBy.Contains(searchText);
      })))
        collectorsItemsPool.Add(minomatDto);
      return collectorsItemsPool;
    }

    public ObservableCollection<MinomatDTO> GetMinomatDTOs()
    {
      RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat> source = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>();
      List<MSS.Core.Model.DataCollectors.Minomat> list = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().Where((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => !x.IsDeactivated)).ToList<MSS.Core.Model.DataCollectors.Minomat>();
      if (list.Any<MSS.Core.Model.DataCollectors.Minomat>())
        source = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) list);
      RadObservableCollection<MinomatDTO> minomats = Mapper.Map<RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>, RadObservableCollection<MinomatDTO>>(source);
      this.GetLocationForMinomat(minomats);
      return (ObservableCollection<MinomatDTO>) minomats;
    }

    public ObservableCollection<MinomatDTO> GetMinomatPoolDTOs()
    {
      RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat> source1 = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>();
      IList<MSS.Core.Model.DataCollectors.Minomat> source2 = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => !x.IsDeactivated && x.IsInMasterPool));
      IList<MSS.Core.Model.DataCollectors.Minomat> minomatList = source2 ?? (IList<MSS.Core.Model.DataCollectors.Minomat>) source2.ToList<MSS.Core.Model.DataCollectors.Minomat>();
      if (minomatList.Any<MSS.Core.Model.DataCollectors.Minomat>())
        source1 = new RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>((IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) minomatList);
      return (ObservableCollection<MinomatDTO>) Mapper.Map<RadObservableCollection<MSS.Core.Model.DataCollectors.Minomat>, RadObservableCollection<MinomatDTO>>(source1);
    }

    private RadObservableCollection<MinomatDTO> GetLocationForMinomat(
      RadObservableCollection<MinomatDTO> minomats)
    {
      Dictionary<Guid, Location> locationsForMinomats = this._repositoryFactory.GetStructureNodeRepository().GetLocationsForMinomats();
      foreach (MinomatDTO minomat in (Collection<MinomatDTO>) minomats)
        minomat.LocationDescription = locationsForMinomats.ContainsKey(minomat.Id) ? locationsForMinomats[minomat.Id].BuildingNr : string.Empty;
      return minomats;
    }

    public MSS.Core.Model.DataCollectors.Minomat CreateAndGetMinomat(
      string radioId,
      string countryCode)
    {
      MSS.Core.Model.DataCollectors.Minomat entity = this._minomatRepository.FirstOrDefault((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (m => m.RadioId == radioId));
      if (entity == null)
      {
        Country country = this._repositoryFactory.GetRepository<Country>().FirstOrDefault((Expression<Func<Country, bool>>) (c => c.Code == countryCode));
        entity = new MSS.Core.Model.DataCollectors.Minomat()
        {
          CreatedOn = new DateTime?(DateTime.Now),
          GsmId = radioId,
          RadioId = radioId,
          HostAndPort = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("HostAndPort"),
          Url = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("Url"),
          Polling = MSS.Business.Utils.AppContext.Current.GetParameterValue<int>("Polling"),
          Challenge = MeterListenerManager.CreateRandomChallengeKey().ToString(),
          SessionKey = MeterListenerManager.CreateRandomSessionKey().ToString(),
          IsMaster = true,
          IsDeactivated = false,
          CreatedByName = "SAPWebService",
          Status = StatusMinomatEnum.BuiltIn.ToString(),
          IsInMasterPool = false,
          Country = country
        };
        this._minomatRepository.Insert(entity);
        MessageHandler.LogDebug("Minomat created. RadioId: " + radioId);
      }
      return entity;
    }

    public List<string> GetExistingMinomatsByRadioId(List<string> radioIds)
    {
      return this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (m => radioIds.Contains(m.RadioId))).Select<MSS.Core.Model.DataCollectors.Minomat, string>((Func<MSS.Core.Model.DataCollectors.Minomat, string>) (m => m.RadioId)).ToList<string>();
    }

    public void ImportMinomats(
      List<MinomatImportDTO> minomatsImportList,
      IRepositoryFactoryCreator repositoryFactoryCreator,
      List<string> ignoredRadioIds)
    {
      ISession session = this._repositoryFactory.GetSession();
      session.FlushMode = FlushMode.Commit;
      try
      {
        ITransaction transaction = session.BeginTransaction();
        IList<Country> all = this._repositoryFactory.GetRepository<Country>().GetAll();
        IRepository<MSS.Core.Model.DataCollectors.Minomat> repository = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>();
        foreach (MinomatImportDTO minomatsImport in minomatsImportList)
        {
          MinomatImportDTO mi = minomatsImport;
          if (!ignoredRadioIds.Contains(mi.MINOLID))
          {
            MinomatImportDTO mi1 = mi;
            MSS.Core.Model.DataCollectors.Minomat minomat;
            if (repository.FirstOrDefault((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (m => m.RadioId == mi1.MINOLID)) != null)
            {
              minomat = repository.FirstOrDefault((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (m => m.RadioId == mi1.MINOLID));
            }
            else
            {
              minomat = new MSS.Core.Model.DataCollectors.Minomat();
              MessageHandler.LogDebug("Minomat created by import. RadioId: " + mi.MINOLID);
            }
            minomat.AccessPoint = mi.AccessPoint;
            minomat.Challenge = Convert.ToUInt32(mi.CHALLENGE_NEW, 16).ToString((IFormatProvider) CultureInfo.InvariantCulture);
            minomat.CreatedOn = new DateTime?(DateTime.Now);
            minomat.GsmId = Convert.ToUInt32(mi.GSMID, 16).ToString((IFormatProvider) CultureInfo.InvariantCulture);
            minomat.HostAndPort = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("HostAndPort");
            minomat.Url = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("Url");
            minomat.Polling = mi.Polling != 0 ? mi.Polling : MSS.Business.Utils.AppContext.Current.GetParameterValue<int>("Polling");
            minomat.RadioId = mi.MINOLID;
            minomat.ProviderName = mi.ProviderName;
            minomat.StartDate = new DateTime?();
            minomat.EndDate = new DateTime?();
            minomat.Status = StatusMinomatEnum.BuiltIn.ToString();
            minomat.Registered = true;
            minomat.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
            minomat.CreatedByName = string.Format("{0} {1}", (object) MSS.Business.Utils.AppContext.Current.LoggedUser.FirstName, (object) MSS.Business.Utils.AppContext.Current.LoggedUser.LastName);
            minomat.IsDeactivated = false;
            minomat.IsInMasterPool = false;
            minomat.SimPin = mi.SimPin;
            minomat.UserId = mi.UserId;
            minomat.UserPassword = mi.UserPassword;
            minomat.SessionKey = Convert.ToUInt64(mi.SESSIONKEY_NEW, 16).ToString((IFormatProvider) CultureInfo.InvariantCulture);
            minomat.IsMaster = true;
            minomat.Country = all.FirstOrDefault<Country>((Func<Country, bool>) (c => c.Code == mi.SASID));
            repository.TransactionalUpdate(minomat);
            MinomatJobsManager.AddMinomat(minomat, new uint?(Convert.ToUInt32(mi.CHALLENGE_OLD, 16)), new ulong?(Convert.ToUInt64(mi.SESSIONKEY_OLD, 16)));
          }
        }
        transaction.Commit();
      }
      catch (Exception ex)
      {
        session.Transaction.Rollback();
        MessageHandler.LogException(ex);
        throw new BaseApplicationException(Resources.MSS_ImportMinomats_ImportError, ex);
      }
    }
  }
}
