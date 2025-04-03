// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.StructuresManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class StructuresManager
  {
    private readonly IRepository<Channel> _channelRepository;
    private readonly IRepository<ConnectedDeviceType> _connectedDeviceTypeRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<MeasureUnit> _measureUniteRepository;
    private readonly IRepository<Meter> _meterRepository;
    private readonly ISession _nhSession;
    private readonly IRepository<RoomType> _roomTypeRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<StructureNodeType> _structureNodeTypeRepository;
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<Scenario> _repositoryScenario;
    private readonly IRepository<Minomat> _repositoryMinomats;
    private readonly IRepository<MeterReplacementHistory> _meterReplacementHistoryRepository;
    private static DateTime MinDateTime = new DateTime(1800, 1, 1);

    public StructuresManager(
      IRepositoryFactory repositoryFactory,
      bool isConstructorWithoutMappings)
    {
      this._repositoryFactory = repositoryFactory;
      this._meterRepository = repositoryFactory.GetRepository<Meter>();
      this._roomTypeRepository = repositoryFactory.GetRepository<RoomType>();
      this._measureUniteRepository = repositoryFactory.GetRepository<MeasureUnit>();
      this._channelRepository = repositoryFactory.GetRepository<Channel>();
      this._connectedDeviceTypeRepository = repositoryFactory.GetRepository<ConnectedDeviceType>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._repositoryScenario = repositoryFactory.GetRepository<Scenario>();
      this._repositoryMinomats = repositoryFactory.GetRepository<Minomat>();
      repositoryFactory.GetRepository<MeterReadingValue>();
      this._meterReplacementHistoryRepository = repositoryFactory.GetRepository<MeterReplacementHistory>();
      this._nhSession = repositoryFactory.GetSession();
    }

    public StructuresManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._meterRepository = repositoryFactory.GetRepository<Meter>();
      this._roomTypeRepository = repositoryFactory.GetRepository<RoomType>();
      this._measureUniteRepository = repositoryFactory.GetRepository<MeasureUnit>();
      this._channelRepository = repositoryFactory.GetRepository<Channel>();
      this._connectedDeviceTypeRepository = repositoryFactory.GetRepository<ConnectedDeviceType>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._repositoryScenario = repositoryFactory.GetRepository<Scenario>();
      this._repositoryMinomats = this._repositoryFactory.GetRepository<Minomat>();
      this._repositoryFactory.GetRepository<MeterReadingValue>();
      this._meterReplacementHistoryRepository = repositoryFactory.GetRepository<MeterReplacementHistory>();
      this._nhSession = repositoryFactory.GetSession();
      Mapper.CreateMap<MeterDTO, Meter>();
      Mapper.CreateMap<LocationDTO, Location>();
      Mapper.CreateMap<TenantDTO, Tenant>();
      Mapper.CreateMap<MinomatSerializableDTO, Minomat>().ForMember((Expression<Func<Minomat, object>>) (x => x.Provider), (Action<IMemberConfigurationExpression<MinomatSerializableDTO>>) (x => x.ResolveUsing((Func<MinomatSerializableDTO, object>) (y => (object) this._repositoryFactory.GetRepository<Provider>().GetById((object) y.ProviderId)))));
      Mapper.CreateMap<Minomat, MinomatSerializableDTO>().ForMember((Expression<Func<MinomatSerializableDTO, object>>) (x => (object) x.ProviderId), (Action<IMemberConfigurationExpression<Minomat>>) (x => x.ResolveUsing((Func<Minomat, object>) (y => y.Provider != null ? (object) y.Provider.Id : (object) Guid.Empty))));
      Mapper.CreateMap<StructureNodeDTO, StructureNode>().ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.Name), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<string>((Expression<Func<StructureNodeDTO, string>>) (strNodeDTO => strNodeDTO.Name)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.Description), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<string>((Expression<Func<StructureNodeDTO, string>>) (strNodeDTO => strNodeDTO.Description)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => (object) strNode.EntityId), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.ResolveUsing((Func<StructureNodeDTO, object>) (strNodeDTO => (object) this.GetEntityId(strNodeDTO))))).ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.EntityName), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.ResolveUsing((Func<StructureNodeDTO, object>) (strNodeDTO => (object) this.GetEntityName(strNodeDTO))))).ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.NodeType), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<StructureNodeType>((Expression<Func<StructureNodeDTO, StructureNodeType>>) (strNodeDTO => strNodeDTO.NodeType)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => (object) strNode.StartDate), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<DateTime?>((Expression<Func<StructureNodeDTO, DateTime?>>) (strNodeDTO => strNodeDTO.StartDate)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => (object) strNode.EndDate), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<DateTime?>((Expression<Func<StructureNodeDTO, DateTime?>>) (strNodeDTO => strNodeDTO.EndDate))));
      Mapper.CreateMap<StructureNodeDTO, StructureNodeLinks>().ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => (object) strNodeLink.Id), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.Ignore())).ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => strNodeLink.Node), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.Ignore())).ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => (object) strNodeLink.ParentNodeId), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.ResolveUsing((Func<StructureNodeDTO, object>) (strNodeDTO => strNodeDTO.ParentNode != null ? (object) strNodeDTO.ParentNode.Id : (object) Guid.Empty)))).ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => strNodeLink.RootNode), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.Ignore())).ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => (object) strNodeLink.StructureType), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.Ignore())).ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => (object) strNodeLink.StartDate), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<DateTime?>((Expression<Func<StructureNodeDTO, DateTime?>>) (strNodeDTO => strNodeDTO.StartDate)))).ForMember((Expression<Func<StructureNodeLinks, object>>) (strNodeLink => (object) strNodeLink.EndDate), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<DateTime?>((Expression<Func<StructureNodeDTO, DateTime?>>) (strNodeDTO => strNodeDTO.EndDate))));
      Mapper.CreateMap<StructureNodeLinks, StructureNodeLinks>().ForMember((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.Id), (Action<IMemberConfigurationExpression<StructureNodeLinks>>) (s => s.Ignore()));
      Mapper.CreateMap<MeterSerializableDTO, Meter>().ForMember((Expression<Func<Meter, object>>) (x => x.ReadingUnit), (Action<IMemberConfigurationExpression<MeterSerializableDTO>>) (x => x.ResolveUsing((Func<MeterSerializableDTO, object>) (z => (object) this._measureUniteRepository.GetById((object) z.ReadingUnitId))))).ForMember((Expression<Func<Meter, object>>) (x => x.ImpulsUnit), (Action<IMemberConfigurationExpression<MeterSerializableDTO>>) (x => x.ResolveUsing((Func<MeterSerializableDTO, object>) (z => (object) this._measureUniteRepository.GetById((object) z.ImpulsUnitId))))).ForMember((Expression<Func<Meter, object>>) (x => x.Channel), (Action<IMemberConfigurationExpression<MeterSerializableDTO>>) (x => x.ResolveUsing((Func<MeterSerializableDTO, object>) (z => (object) this._channelRepository.GetById((object) z.ChannelId))))).ForMember((Expression<Func<Meter, object>>) (x => x.ConnectedDeviceType), (Action<IMemberConfigurationExpression<MeterSerializableDTO>>) (x => x.ResolveUsing((Func<MeterSerializableDTO, object>) (z => (object) this._connectedDeviceTypeRepository.GetById((object) z.ConnectedDeviceTypeId))))).ForMember((Expression<Func<Meter, object>>) (x => x.Room), (Action<IMemberConfigurationExpression<MeterSerializableDTO>>) (x => x.ResolveUsing((Func<MeterSerializableDTO, object>) (z => (object) this._roomTypeRepository.GetById((object) z.RoomTypeId)))));
      Mapper.CreateMap<LocationSerializableDTO, Location>().ForMember((Expression<Func<Location, object>>) (x => x.Scenario), (Action<IMemberConfigurationExpression<LocationSerializableDTO>>) (x => x.ResolveUsing((Func<LocationSerializableDTO, object>) (z => (object) this._repositoryScenario.GetById((object) z.ScenarioId))))).ForMember((Expression<Func<Location, object>>) (x => x.Country), (Action<IMemberConfigurationExpression<LocationSerializableDTO>>) (x => x.ResolveUsing((Func<LocationSerializableDTO, object>) (z => (object) this._repositoryFactory.GetRepository<Country>().GetById((object) z.CountryId)))));
      Mapper.CreateMap<TenantSerializableDTO, Tenant>();
      Mapper.CreateMap<StructureNodeSerializableDTO, StructureNode>().ForMember((Expression<Func<StructureNode, object>>) (x => x.NodeType), (Action<IMemberConfigurationExpression<StructureNodeSerializableDTO>>) (x => x.ResolveUsing((Func<StructureNodeSerializableDTO, object>) (z => (object) this._structureNodeTypeRepository.GetById((object) z.NodeType)))));
      Mapper.CreateMap<StructureNodeLinksSerializableDTO, StructureNodeLinks>();
      Mapper.CreateMap<MinomatSerializableDTO, Minomat>().ForMember((Expression<Func<Minomat, object>>) (x => x.Provider), (Action<IMemberConfigurationExpression<MinomatSerializableDTO>>) (x => x.ResolveUsing((Func<MinomatSerializableDTO, object>) (y => (object) this._repositoryFactory.GetRepository<Provider>().GetById((object) y.ProviderId))))).ForMember((Expression<Func<Minomat, object>>) (x => x.Country), (Action<IMemberConfigurationExpression<MinomatSerializableDTO>>) (x => x.ResolveUsing((Func<MinomatSerializableDTO, object>) (z => (object) this._repositoryFactory.GetRepository<Country>().GetById((object) z.CountryId)))));
    }

    private Meter TransactionalCreateMeter(MeterDTO meterDTO)
    {
      Meter meter = Mapper.Map<MeterDTO, Meter>(meterDTO);
      meter.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      if (meter.LastChangedOn.HasValue && meter.LastChangedOn.Value < StructuresManager.MinDateTime)
        meter.LastChangedOn = new DateTime?(StructuresManager.MinDateTime);
      this._meterRepository.TransactionalInsert(meter);
      if (meterDTO.MbusRadioMeter != null)
        this.TransactionalSaveOrUpdateMbusRadioMeter(meter);
      if (meterDTO.MeterRadioDetails != null && meterDTO.MeterRadioDetails.Count > 0)
        this.TransactionalSaveOrUpdateMeterRadioDetails(meter, meterDTO.MeterRadioDetails);
      return meter;
    }

    private Minomat TransactionalCreateMinomat(MinomatSerializableDTO minomatDTO)
    {
      Minomat entity = Mapper.Map<MinomatSerializableDTO, Minomat>(minomatDTO);
      entity.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      this._repositoryFactory.GetRepository<Minomat>().TransactionalInsert(entity);
      MessageHandler.LogDebug("Minomat created from structures or structures import. RadioId: " + minomatDTO.RadioId);
      return entity;
    }

    public Meter TransactionalEditMeter(MeterDTO meterDTO)
    {
      Meter meterAlias = (Meter) null;
      MeasureUnit impulsUnitAlias = (MeasureUnit) null;
      MeasureUnit readingUnitAlias = (MeasureUnit) null;
      Channel channelAlias = (Channel) null;
      RoomType roomAlias = (RoomType) null;
      Meter meter = this._repositoryFactory.GetSession().QueryOver<Meter>((Expression<Func<Meter>>) (() => meterAlias)).JoinAlias((Expression<Func<Meter, object>>) (x => x.ReadingUnit), (Expression<Func<object>>) (() => readingUnitAlias), JoinType.LeftOuterJoin).JoinAlias((Expression<Func<Meter, object>>) (x => x.ImpulsUnit), (Expression<Func<object>>) (() => impulsUnitAlias), JoinType.LeftOuterJoin).JoinAlias((Expression<Func<Meter, object>>) (x => x.Channel), (Expression<Func<object>>) (() => channelAlias), JoinType.LeftOuterJoin).JoinAlias((Expression<Func<Meter, object>>) (x => x.Room), (Expression<Func<object>>) (() => roomAlias), JoinType.LeftOuterJoin).WhereRestrictionOn((Expression<Func<object>>) (() => (object) meterAlias.Id)).IsLike((object) meterDTO.Id).List().FirstOrDefault<Meter>();
      Mapper.Map<MeterDTO, Meter>(meterDTO, meter);
      if (meter != null)
        meter.UpdatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      this._meterRepository.TransactionalUpdate(meter);
      if (meterDTO.MbusRadioMeter != null)
        this.TransactionalSaveOrUpdateMbusRadioMeter(meter);
      if (meterDTO.MeterRadioDetails != null && meterDTO.MeterRadioDetails.Count > 0)
        this.TransactionalSaveOrUpdateMeterRadioDetails(meter, meterDTO.MeterRadioDetails);
      return meter;
    }

    private Minomat TransactionalEditMinomat(MinomatSerializableDTO minomatDTO)
    {
      Minomat byId = this._repositoryFactory.GetRepository<Minomat>().GetById((object) minomatDTO.Id);
      Mapper.Map<MinomatSerializableDTO, Minomat>(minomatDTO, byId);
      this._repositoryFactory.GetRepository<Minomat>().TransactionalUpdate(byId);
      return byId;
    }

    private Location TransactionalCreateLocation(LocationDTO locationDTO)
    {
      Location entity = Mapper.Map<LocationDTO, Location>(locationDTO);
      entity.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      DateTime? updateBuildingNo = entity.LastUpdateBuildingNo;
      DateTime minDateTime = StructuresManager.MinDateTime;
      if (updateBuildingNo.HasValue && updateBuildingNo.GetValueOrDefault() < minDateTime)
        entity.LastUpdateBuildingNo = new DateTime?(StructuresManager.MinDateTime);
      entity.Country = MSS.Business.Utils.AppContext.Current.LoggedUser.Country;
      entity.Office = MSS.Business.Utils.AppContext.Current.LoggedUser.Office;
      this._locationRepository.TransactionalInsert(entity);
      return entity;
    }

    private Location TransactionalEditLocation(LocationDTO locationDTO)
    {
      Location byId = this._locationRepository.GetById((object) locationDTO.Id);
      Mapper.Map<LocationDTO, Location>(locationDTO, byId);
      byId.UpdatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      byId.LastUpdateBuildingNo = new DateTime?(DateTime.Now);
      this._locationRepository.TransactionalUpdate(byId);
      return byId;
    }

    private Tenant TransactionalCreateTenant(TenantDTO tenantDTO)
    {
      Tenant entity = Mapper.Map<TenantDTO, Tenant>(tenantDTO);
      entity.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      if (entity.LastChangedOn.HasValue && entity.LastChangedOn.Value < StructuresManager.MinDateTime)
        entity.LastChangedOn = new DateTime?(StructuresManager.MinDateTime);
      this._tenantRepository.TransactionalInsert(entity);
      return entity;
    }

    private Tenant TransactionalEditTenant(TenantDTO tenantDTO)
    {
      Tenant byId = this._tenantRepository.GetById((object) tenantDTO.Id);
      Mapper.Map<TenantDTO, Tenant>(tenantDTO, byId);
      byId.UpdatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
      this._tenantRepository.TransactionalUpdate(byId);
      return byId;
    }

    public Meter TransactionalSaveOrUpdateMeter(MeterDTO meterDTO)
    {
      Meter meter = new Meter();
      return !(meterDTO.Id == Guid.Empty) ? this.TransactionalEditMeter(meterDTO) : this.TransactionalCreateMeter(meterDTO);
    }

    private void TransactionalSaveOrUpdateMeterRadioDetails(
      Meter meter,
      List<MeterRadioDetails> meterRadioDetails)
    {
      IRepository<MeterRadioDetails> repository = this._repositoryFactory.GetRepository<MeterRadioDetails>();
      MeterRadioDetails entity1 = repository.FirstOrDefault((Expression<Func<MeterRadioDetails, bool>>) (m => m.Meter.Id == meter.Id));
      MeterRadioDetails entity2 = meterRadioDetails.FirstOrDefault<MeterRadioDetails>();
      if (entity2 == null)
        return;
      entity2.Meter = meter;
      if (entity1 == null)
      {
        repository.TransactionalInsert(entity2);
      }
      else
      {
        entity1.dgMessbereich = entity2.dgMessbereich;
        repository.TransactionalUpdate(entity1);
      }
    }

    public void TransactionalSaveOrUpdateMbusRadioMeter(Meter meter)
    {
      MbusRadioMeter mbusRadioMeter = this._repositoryFactory.GetRepository<MbusRadioMeter>().FirstOrDefault((Expression<Func<MbusRadioMeter, bool>>) (item => item.Meter.Id == meter.Id));
      if (mbusRadioMeter != null)
      {
        mbusRadioMeter.City = meter.MbusRadioMeter.City;
        mbusRadioMeter.Street = meter.MbusRadioMeter.Street;
        mbusRadioMeter.HouseNumber = meter.MbusRadioMeter.HouseNumber;
        mbusRadioMeter.HouseNumberSupplement = meter.MbusRadioMeter.HouseNumberSupplement;
        mbusRadioMeter.ApartmentNumber = meter.MbusRadioMeter.ApartmentNumber;
        mbusRadioMeter.ZipCode = meter.MbusRadioMeter.ZipCode;
        mbusRadioMeter.FirstName = meter.MbusRadioMeter.FirstName;
        mbusRadioMeter.LastName = meter.MbusRadioMeter.LastName;
        mbusRadioMeter.Location = meter.MbusRadioMeter.Location;
        mbusRadioMeter.RadioSerialNumber = meter.MbusRadioMeter.RadioSerialNumber;
        meter.MbusRadioMeter = mbusRadioMeter;
      }
      if (meter.MbusRadioMeter.Id == Guid.Empty)
      {
        meter.MbusRadioMeter.Meter = meter;
        this._repositoryFactory.GetRepository<MbusRadioMeter>().TransactionalInsert(meter.MbusRadioMeter);
      }
      else
        this._repositoryFactory.GetRepository<MbusRadioMeter>().TransactionalUpdate(meter.MbusRadioMeter);
    }

    public Minomat TransactionalSaveOrUpdateMinomat(MinomatSerializableDTO minomatDto)
    {
      Minomat minomat = new Minomat();
      return minomatDto.Id == Guid.Empty ? this.TransactionalCreateMinomat(minomatDto) : this.TransactionalEditMinomat(minomatDto);
    }

    public Location TransactionalSaveOrUpdateLocation(LocationDTO locationDTO)
    {
      Location location = new Location();
      return !(locationDTO.Id == Guid.Empty) ? this.TransactionalEditLocation(locationDTO) : this.TransactionalCreateLocation(locationDTO);
    }

    public Tenant TransactionalSaveOrUpdateTenant(TenantDTO tenantDTO)
    {
      Tenant tenant = new Tenant();
      return !(tenantDTO.Id == Guid.Empty) ? this.TransactionalEditTenant(tenantDTO) : this.TransactionalCreateTenant(tenantDTO);
    }

    private StructureNode TransactionalCreateStructureNode(StructureNodeDTO newNode)
    {
      StructureNode newStructureNode = Mapper.Map<StructureNodeDTO, StructureNode>(newNode);
      if (!newStructureNode.StartDate.HasValue)
        newStructureNode.StartDate = new DateTime?(DateTime.Now);
      this._structureNodeRepository.TransactionalInsert(newStructureNode);
      if (newNode.AssignedPicture != null)
        newNode.AssignedPicture.ForEach((Action<byte[]>) (_ => this._repositoryFactory.GetRepository<PhotoMeter>().TransactionalInsert(new PhotoMeter()
        {
          StructureNode = newStructureNode,
          Payload = _
        })));
      if (newNode.AssignedNotes != null)
        newNode.AssignedNotes.ForEach((Action<Note>) (_ => this._repositoryFactory.GetRepository<Note>().TransactionalInsert(new Note()
        {
          StructureNode = newStructureNode,
          NoteDescription = _.NoteDescription,
          NoteType = _.NoteType
        })));
      return newStructureNode;
    }

    private StructureNodeLinks TransactionalCreateStructureNodeLink(
      StructureNodeDTO structureNodeDTO,
      StructureTypeEnum structureType)
    {
      StructureNodeLinks entity = Mapper.Map<StructureNodeDTO, StructureNodeLinks>(structureNodeDTO);
      entity.Node = this._repositoryFactory.GetRepository<StructureNode>().SearchForInMemoryOrDb((Expression<Func<StructureNode, bool>>) (sn => sn.Id == structureNodeDTO.Id), (Func<StructureNode, bool>) (sn => sn.Id == structureNodeDTO.Id)).FirstOrDefault<StructureNode>();
      entity.RootNode = this._repositoryFactory.GetRepository<StructureNode>().SearchForInMemoryOrDb((Expression<Func<StructureNode, bool>>) (sn => sn.Id == structureNodeDTO.RootNode.Id), (Func<StructureNode, bool>) (sn => sn.Id == structureNodeDTO.RootNode.Id)).FirstOrDefault<StructureNode>();
      if (structureNodeDTO.ParentNode != null)
        entity.ParentNodeId = structureNodeDTO.ParentNode.Id;
      entity.StructureType = structureType;
      entity.StartDate = new DateTime?(DateTime.Now);
      this._structureNodeLinksRepository.TransactionalInsert(entity);
      return entity;
    }

    private void TransactionalUpdateStructureNode(StructureNodeDTO structureNodeDTO)
    {
      StructureNode structureNode = this._structureNodeRepository.GetById((object) structureNodeDTO.Id);
      Mapper.Map<StructureNodeDTO, StructureNode>(structureNodeDTO, structureNode);
      if (structureNodeDTO.AssignedPicture != null)
      {
        structureNode.Photos = (IList<PhotoMeter>) null;
        structureNode.Photos = (IList<PhotoMeter>) new List<PhotoMeter>();
        TypeHelperExtensionMethods.ForEach<PhotoMeter>(structureNodeDTO.AssignedPicture.Select<byte[], PhotoMeter>((Func<byte[], PhotoMeter>) (_ => new PhotoMeter()
        {
          Payload = _,
          StructureNode = structureNode
        })), (Action<PhotoMeter>) (_ => structureNode.Photos.Add(_)));
      }
      if (structureNodeDTO.AssignedNotes != null)
      {
        structureNode.Notes = (IList<Note>) new List<Note>();
        structureNodeDTO.AssignedNotes.ForEach((Action<Note>) (note =>
        {
          note.StructureNode = structureNode;
          structureNode.Notes.Add(note);
        }));
      }
      this._structureNodeRepository.TransactionalUpdate(structureNode);
    }

    private void TransactionalUpdateStructureNodeLink(StructureNodeDTO structureNodeDTO)
    {
      StructureNodeLinks structureNodeLinks = this._structureNodeLinksRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (s => s.EndDate == new DateTime?() && s.Node.Id == structureNodeDTO.Id && (int?) s.StructureType == (int?) structureNodeDTO.StructureType));
      if (structureNodeLinks != null)
      {
        Mapper.Map<StructureNodeDTO, StructureNodeLinks>(structureNodeDTO, structureNodeLinks);
        structureNodeLinks.RootNode = this._structureNodeRepository.GetById((object) structureNodeDTO.RootNode.Id);
      }
      this._structureNodeLinksRepository.TransactionalUpdate(structureNodeLinks);
    }

    private void TransactionalCreateOrUpdateStructure(
      IList<StructureNodeDTO> nodeCollection,
      StructureTypeEnum structureType)
    {
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeDTO structureNodeDto in nodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (structureNode => !nodeIDs.Contains(structureNode.Id))))
        nodeIDs.Add(structureNodeDto.Id);
      IEnumerable<StructureNodeLinks> source = (IEnumerable<StructureNodeLinks>) this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (l => (int) l.StructureType == (int) structureType && nodeIDs.Contains(l.Node.Id) && l.EndDate == new DateTime?()));
      foreach (StructureNodeDTO node in (IEnumerable<StructureNodeDTO>) nodeCollection)
      {
        StructureNodeDTO nodeDTO = node;
        if (nodeDTO.Id == Guid.Empty)
        {
          StructureNode structureNode = this.TransactionalCreateStructureNode(nodeDTO);
          nodeDTO.Id = structureNode.Id;
        }
        else
          this.TransactionalUpdateStructureNode(nodeDTO);
        StructureNodeLinks structureNodeLinks = source.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l =>
        {
          if (!(l.Node.Id == nodeDTO.Id) || l.EndDate.HasValue)
            return false;
          StructureTypeEnum structureType1 = l.StructureType;
          StructureTypeEnum? structureType2 = nodeDTO.StructureType;
          return structureType1 == structureType2.GetValueOrDefault() && structureType2.HasValue;
        }));
        if (structureNodeLinks == null)
          this.TransactionalCreateStructureNodeLink(nodeDTO, structureType);
        else if (this.HasLinkChanged(structureNodeLinks, nodeDTO))
        {
          StructureNodeLinks entity = Mapper.Map<StructureNodeLinks, StructureNodeLinks>(structureNodeLinks);
          entity.EndDate = new DateTime?(DateTime.Now);
          this._structureNodeLinksRepository.TransactionalInsert(entity);
          nodeDTO.StartDate = structureNodeLinks.StartDate;
          this.TransactionalUpdateStructureNodeLink(nodeDTO);
        }
      }
    }

    private bool HasLinkChanged(StructureNodeLinks nodeLink, StructureNodeDTO nodeDTO)
    {
      bool flag1 = false;
      bool flag2 = nodeDTO.ParentNode == null ? flag1 | nodeLink.ParentNodeId != Guid.Empty : flag1 | nodeDTO.ParentNode.Id != nodeLink.ParentNodeId;
      return (nodeDTO.RootNode == null ? flag2 | nodeLink.RootNode.Id != Guid.Empty : flag2 | nodeDTO.RootNode.Id != nodeLink.RootNode.Id) | nodeDTO.OrderNr != nodeLink.OrderNr;
    }

    public void TransactionalSaveNewStructure(
      IList<StructureNodeDTO> nodeCollection,
      StructureTypeEnum structureType)
    {
      foreach (StructureNodeDTO structureNodeDto in nodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Id == Guid.Empty)))
      {
        StructureNode structureNode = this.TransactionalCreateStructureNode(structureNodeDto);
        structureNodeDto.Id = structureNode.Id;
        this.TransactionalCreateStructureNodeLink(structureNodeDto, structureType);
      }
    }

    private void TransactionalCreateOrUpdateLogicalStructure(IList<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (IEnumerable<StructureNodeDTO>) nodeCollection)
      {
        if (node.IsNewNode)
        {
          StructureNode structureNode = this.TransactionalCreateStructureNode(node);
          node.Id = structureNode.Id;
          this.TransactionalCreateStructureNodeLink(node, StructureTypeEnum.Logical);
        }
        else
        {
          this.TransactionalUpdateStructureNode(node);
          this.TransactionalCreateStructureNodeLink(node, StructureTypeEnum.Logical);
        }
      }
    }

    public void UpdateStructureNodeLink(StructureNodeLinks structureNodeLink)
    {
      this._structureNodeLinksRepository.TransactionalUpdate(structureNodeLink);
    }

    public void TransactionalDeleteAffectedStructureNodes(
      IList<StructureNodeLinks> structureNodeLinks,
      IList<StructureNode> structureNodes)
    {
      if (structureNodeLinks != null)
        TypeHelperExtensionMethods.ForEach<StructureNodeLinks>((IEnumerable<StructureNodeLinks>) structureNodeLinks, (Action<StructureNodeLinks>) (structureNodeLink =>
        {
          structureNodeLink.EndDate = new DateTime?(DateTime.Now);
          this._structureNodeLinksRepository.TransactionalUpdate(structureNodeLink);
        }));
      if (structureNodes == null)
        return;
      foreach (StructureNode structureNode1 in structureNodes.Distinct<StructureNode>())
      {
        StructureNode structureNode = structureNode1;
        StructureNodeType byId = this._structureNodeTypeRepository.GetById((object) structureNode.NodeType.Id);
        if (byId != null)
        {
          StructureNodeTypeEnum structureNodeTypeEnum = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), byId.Name, true);
          if (structureNode.EntityId != Guid.Empty)
          {
            switch (structureNodeTypeEnum)
            {
              case StructureNodeTypeEnum.Location:
                Location entity1 = this.GetEntity<Location>(structureNode.EntityId);
                entity1.IsDeactivated = true;
                this._locationRepository.TransactionalUpdate(entity1);
                break;
              case StructureNodeTypeEnum.Tenant:
                Tenant entity2 = this.GetEntity<Tenant>(structureNode.EntityId);
                entity2.IsDeactivated = true;
                this._tenantRepository.TransactionalUpdate(entity2);
                break;
              case StructureNodeTypeEnum.Meter:
                if (structureNodeLinks != null && structureNodeLinks.Any<StructureNodeLinks>() && structureNodeLinks[0].StructureType != StructureTypeEnum.Fixed)
                {
                  IRepository<StructureNodeEquipmentSettings> repository = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>();
                  StructureNodeEquipmentSettings entity3 = repository.FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (item => item.StructureNode.Id == structureNode.Id));
                  if (entity3 != null)
                    repository.TransactionalDelete(entity3);
                }
                Meter entity4 = this.GetEntity<Meter>(structureNode.EntityId);
                entity4.IsDeactivated = true;
                this._meterRepository.TransactionalUpdate(entity4);
                break;
              case StructureNodeTypeEnum.MinomatMaster:
              case StructureNodeTypeEnum.MinomatSlave:
                Minomat entity5 = this.GetEntity<Minomat>(structureNode.EntityId);
                entity5.IsDeactivated = true;
                this._repositoryFactory.GetRepository<Minomat>().TransactionalUpdate(entity5);
                break;
              case StructureNodeTypeEnum.RadioMeter:
                Meter entity6 = this.GetEntity<Meter>(structureNode.EntityId);
                entity6.IsDeactivated = true;
                this._meterRepository.TransactionalUpdate(entity6);
                break;
            }
          }
        }
        structureNode.EndDate = new DateTime?(DateTime.Now);
        this._structureNodeRepository.TransactionalUpdate(structureNode);
      }
    }

    public void TransactionalSaveEntity(IEnumerable<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in nodeCollection)
      {
        if (node.Entity != null)
        {
          switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), node.NodeType.Name, true))
          {
            case StructureNodeTypeEnum.Location:
              Location entityObj1 = this.TransactionalSaveOrUpdateLocation((LocationDTO) node.Entity);
              node.Entity = StructuresHelper.GetEntityDTO(StructureNodeTypeEnum.Location, (object) entityObj1);
              break;
            case StructureNodeTypeEnum.Tenant:
              Tenant entityObj2 = this.TransactionalSaveOrUpdateTenant((TenantDTO) node.Entity);
              node.Entity = StructuresHelper.GetEntityDTO(StructureNodeTypeEnum.Tenant, (object) entityObj2);
              break;
            case StructureNodeTypeEnum.Meter:
            case StructureNodeTypeEnum.RadioMeter:
              MeterDTO entity = (MeterDTO) node.Entity;
              Meter entityObj3 = this.TransactionalSaveOrUpdateMeter(entity);
              if (entity.ReplacedMeterId.HasValue)
                this.TransactionalSaveMeterReplacementHistory(entityObj3.Id, entity.ReplacedMeterId.Value);
              node.Entity = StructuresHelper.GetEntityDTO(StructureNodeTypeEnum.Meter, (object) entityObj3);
              break;
            case StructureNodeTypeEnum.MinomatMaster:
            case StructureNodeTypeEnum.MinomatSlave:
              Minomat entityObj4 = this.TransactionalSaveOrUpdateMinomat((MinomatSerializableDTO) node.Entity);
              node.Entity = StructuresHelper.GetEntityDTO(StructureNodeTypeEnum.MinomatMaster, (object) entityObj4);
              break;
          }
        }
      }
    }

    private Guid GetEntityId(StructureNodeDTO structureNodeDTO)
    {
      return structureNodeDTO.Entity != null ? (Guid) structureNodeDTO.Entity.GetType().GetProperty("Id").GetValue(structureNodeDTO.Entity) : Guid.Empty;
    }

    private string GetEntityName(StructureNodeDTO structureNodeDTO)
    {
      return structureNodeDTO.Entity != null ? structureNodeDTO.NodeType.Name : string.Empty;
    }

    public void UpdateStructureNode(StructureNode structureNode)
    {
      this._structureNodeRepository.TransactionalUpdate(structureNode);
    }

    public void GetAffectedPhysicalStructureNodes(
      StructureNodeDTO selectedNode,
      StructureTypeEnum structureType,
      out List<StructureNodeLinks> structureNodeLinks,
      out List<StructureNodeLinks> logicalStructureNodeLinks,
      out List<StructureNode> structureNodes,
      bool includeDescendants = true)
    {
      structureNodeLinks = new List<StructureNodeLinks>();
      logicalStructureNodeLinks = new List<StructureNodeLinks>();
      structureNodes = new List<StructureNode>();
      if (includeDescendants)
      {
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(selectedNode))
          this.GetOwnNodesAndLinksForPhysicalStructure(structureType, structureNodeLinks, logicalStructureNodeLinks, structureNodes, descendant);
      }
      else
        this.GetOwnNodesAndLinksForPhysicalStructure(structureType, structureNodeLinks, logicalStructureNodeLinks, structureNodes, selectedNode);
    }

    public void GetFixedStructureNodes(
      StructureNodeDTO selectedNode,
      out List<StructureNodeLinks> structureNodeLinks,
      out List<StructureNode> structureNodes,
      bool includeDescendants = true)
    {
      structureNodes = new List<StructureNode>();
      structureNodeLinks = new List<StructureNodeLinks>();
      if (includeDescendants)
      {
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(selectedNode))
          this.GetOwnNodesAndLinksForFixedStructure(descendant, structureNodeLinks, structureNodes);
      }
      else
        this.GetOwnNodesAndLinksForFixedStructure(selectedNode, structureNodeLinks, structureNodes);
    }

    public void GetLogicalStructureNodes(
      StructureNodeDTO selectedNode,
      out List<StructureNodeLinks> structureNodeLinks,
      out List<StructureNode> structureNodes,
      bool includeDescendants = true)
    {
      structureNodes = new List<StructureNode>();
      structureNodeLinks = new List<StructureNodeLinks>();
      if (includeDescendants)
      {
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(selectedNode))
          this.GetOwnNodesAndLinksForLogicalStructure(structureNodeLinks, structureNodes, descendant);
      }
      else
        this.GetOwnNodesAndLinksForLogicalStructure(structureNodeLinks, structureNodes, selectedNode);
    }

    private void GetOwnNodesAndLinksForLogicalStructure(
      List<StructureNodeLinks> structureNodeLinks,
      List<StructureNode> structureNodes,
      StructureNodeDTO structureNodeDTO)
    {
      TypeHelperExtensionMethods.ForEach<StructureNodeLinks>((IEnumerable<StructureNodeLinks>) this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureNodeDTO.Id && (int) s.StructureType == 1 && s.EndDate == new DateTime?())), new Action<StructureNodeLinks>(structureNodeLinks.Add));
      IList<StructureNodeLinks> source = this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureNodeDTO.Id && (int) s.StructureType == 0 && s.EndDate == new DateTime?()));
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeLinks structureNodeLinks1 in source.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => !nodeIDs.Contains(s.Node.Id))))
        nodeIDs.Add(structureNodeLinks1.Node.Id);
      TypeHelperExtensionMethods.ForEach<StructureNode>(this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => s.Id == structureNodeDTO.Id && s.EndDate == new DateTime?())).Except<StructureNode>((IEnumerable<StructureNode>) this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id)))), new Action<StructureNode>(structureNodes.Add));
    }

    private void GetOwnNodesAndLinksForPhysicalStructure(
      StructureTypeEnum structureType,
      List<StructureNodeLinks> structureNodeLinks,
      List<StructureNodeLinks> logicalStructureNodeLinks,
      List<StructureNode> structureNodes,
      StructureNodeDTO selectedNode)
    {
      TypeHelperExtensionMethods.ForEach<StructureNodeLinks>((IEnumerable<StructureNodeLinks>) this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == selectedNode.Id && (int) s.StructureType == (int) structureType && s.EndDate == new DateTime?())), new Action<StructureNodeLinks>(structureNodeLinks.Add));
      if (structureType == StructureTypeEnum.Physical)
      {
        IRepository<StructureNodeLinks> nodeLinksRepository = this._structureNodeLinksRepository;
        Expression<Func<StructureNodeLinks, bool>> predicate = (Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == selectedNode.Id && (int) s.StructureType == 1 && s.EndDate == new DateTime?());
        foreach (StructureNodeLinks parentStructureNodeLink in (IEnumerable<StructureNodeLinks>) nodeLinksRepository.SearchFor(predicate))
          TypeHelperExtensionMethods.ForEach<StructureNodeLinks>(StructuresHelper.GetDescendantsForStructureNodeLink(this._structureNodeLinksRepository, parentStructureNodeLink), new Action<StructureNodeLinks>(logicalStructureNodeLinks.Add));
      }
      IList<StructureNode> second = this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => s.Id == selectedNode.Id && s.EndDate == new DateTime?()));
      TypeHelperExtensionMethods.ForEach<StructureNode>((IEnumerable<StructureNode>) second, new Action<StructureNode>(structureNodes.Add));
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeLinks structureNodeLinks1 in logicalStructureNodeLinks.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (structureNode => !nodeIDs.Contains(structureNode.Node.Id))))
      {
        StructureNodeLinks logicalStructureNode = structureNodeLinks1;
        if (!this._structureNodeLinksRepository.Exists((Expression<Func<StructureNodeLinks, bool>>) (l => l.Node.Id == logicalStructureNode.Node.Id && (int) l.StructureType == 0 && l.EndDate == new DateTime?())))
          nodeIDs.Add(logicalStructureNode.Node.Id);
      }
      TypeHelperExtensionMethods.ForEach<StructureNode>(this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id))).Except<StructureNode>((IEnumerable<StructureNode>) second), new Action<StructureNode>(structureNodes.Add));
    }

    private void GetOwnNodesAndLinksForFixedStructure(
      StructureNodeDTO structureNodeDTO,
      List<StructureNodeLinks> structureNodeLinks,
      List<StructureNode> structureNodes)
    {
      TypeHelperExtensionMethods.ForEach<StructureNodeLinks>((IEnumerable<StructureNodeLinks>) this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureNodeDTO.Id && (int) s.StructureType == 2 && s.EndDate == new DateTime?())), new Action<StructureNodeLinks>(structureNodeLinks.Add));
      TypeHelperExtensionMethods.ForEach<StructureNode>((IEnumerable<StructureNode>) this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => s.Id == structureNodeDTO.Id && s.EndDate == new DateTime?())), new Action<StructureNode>(structureNodes.Add));
    }

    public IEnumerable<StructureNodeDTO> GetAffectedLogicalStructure(
      StructureNodeDTO selectedNode,
      StructureTypeEnum structureType)
    {
      IEnumerable<StructureNodeDTO> logicalStructure = (IEnumerable<StructureNodeDTO>) new ObservableCollection<StructureNodeDTO>();
      List<StructureNodeLinks> affectedStructureNodeLinks = new List<StructureNodeLinks>();
      foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(selectedNode))
      {
        StructureNodeDTO structureNodeDTO = descendant;
        TypeHelperExtensionMethods.ForEach<StructureNodeLinks>((IEnumerable<StructureNodeLinks>) this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureNodeDTO.Id && (int) s.StructureType == 1 && s.EndDate == new DateTime?())), new Action<StructureNodeLinks>(affectedStructureNodeLinks.Add));
      }
      if (affectedStructureNodeLinks.Count != 0)
        logicalStructure = this.GetLogicalStructure(affectedStructureNodeLinks);
      return logicalStructure;
    }

    private IEnumerable<StructureNodeDTO> GetLogicalStructure(
      List<StructureNodeLinks> affectedStructureNodeLinks)
    {
      List<Guid> rootIDs = new List<Guid>();
      foreach (StructureNodeLinks structureNodeLinks in affectedStructureNodeLinks.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (structureNode => !rootIDs.Contains(structureNode.RootNode.Id))))
        rootIDs.Add(structureNodeLinks.RootNode.Id);
      this._structureNodeRepository.GetAll();
      IList<StructureNodeLinks> structureNodeLinksList = this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => (rootIDs.Contains(s.RootNode.Id) || rootIDs.Contains(s.Node.Id)) && s.EndDate == new DateTime?()));
      IList<StructureNodeType> all = this._structureNodeTypeRepository.GetAll();
      Dictionary<Guid, object> entitiesDictionary = this.GetEntitiesDictionary();
      return (IEnumerable<StructureNodeDTO>) StructuresHelper.GetTreeFromList(all, structureNodeLinksList, entitiesDictionary);
    }

    public void InsertEntitiesGuid(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        if (node.Entity != null)
        {
          switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), node.NodeType.Name, true))
          {
            case StructureNodeTypeEnum.Location:
              LocationDTO entity1 = (LocationDTO) node.Entity;
              entity1.Id = entity1.Id != Guid.Empty ? entity1.Id : Guid.NewGuid();
              break;
            case StructureNodeTypeEnum.Tenant:
              TenantDTO entity2 = (TenantDTO) node.Entity;
              entity2.Id = entity2.Id != Guid.Empty ? entity2.Id : Guid.NewGuid();
              break;
            case StructureNodeTypeEnum.Meter:
            case StructureNodeTypeEnum.RadioMeter:
              MeterDTO entity3 = (MeterDTO) node.Entity;
              entity3.Id = entity3.Id != Guid.Empty ? entity3.Id : Guid.NewGuid();
              break;
            case StructureNodeTypeEnum.MinomatMaster:
            case StructureNodeTypeEnum.MinomatSlave:
              MinomatSerializableDTO entity4 = (MinomatSerializableDTO) node.Entity;
              entity4.Id = entity4.Id != Guid.Empty ? entity4.Id : Guid.NewGuid();
              break;
          }
        }
      }
    }

    public void InsertStructureNodesGuid(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        if (node.Id == Guid.Empty)
          node.Id = Guid.NewGuid();
      }
    }

    public object GetEntity(
      StructureNodeTypeEnum structureNodeTypeName,
      StructureNode structureNode)
    {
      object entity = new object();
      switch (structureNodeTypeName)
      {
        case StructureNodeTypeEnum.Location:
          entity = (object) this.GetEntity<Location>(structureNode.EntityId);
          break;
        case StructureNodeTypeEnum.Tenant:
          entity = (object) this.GetEntity<Tenant>(structureNode.EntityId);
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          entity = (object) this.GetEntity<Meter>(structureNode.EntityId);
          break;
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          entity = (object) this.GetEntity<Minomat>(structureNode.EntityId);
          break;
      }
      return entity;
    }

    private T GetEntity<T>(Guid entityId) => this._nhSession.Get<T>((object) entityId);

    public Dictionary<Guid, object> GetEntitiesDictionary()
    {
      Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
      IList<Meter> all1 = this._meterRepository.GetAll();
      IList<Location> all2 = this._locationRepository.GetAll();
      IList<Tenant> all3 = this._tenantRepository.GetAll();
      IList<Minomat> all4 = this._repositoryFactory.GetRepository<Minomat>().GetAll();
      TypeHelperExtensionMethods.ForEach<Meter>((IEnumerable<Meter>) all1, (Action<Meter>) (m => entitiesDictionary.Add(m.Id, (object) m)));
      TypeHelperExtensionMethods.ForEach<Location>((IEnumerable<Location>) all2, (Action<Location>) (l => entitiesDictionary.Add(l.Id, (object) l)));
      TypeHelperExtensionMethods.ForEach<Tenant>((IEnumerable<Tenant>) all3, (Action<Tenant>) (t => entitiesDictionary.Add(t.Id, (object) t)));
      TypeHelperExtensionMethods.ForEach<Minomat>((IEnumerable<Minomat>) all4, (Action<Minomat>) (m => entitiesDictionary.Add(m.Id, (object) m)));
      return entitiesDictionary;
    }

    public ObservableCollection<StructureNodeDTO> GetStructures(
      string searchText,
      StructureTypeEnum structureTypeEnum)
    {
      List<Guid> metersWithSameSerialNumberAsSearchText = LinqExtensionMethods.Query<Meter>(this._repositoryFactory.GetSession()).Where<Meter>((Expression<Func<Meter, bool>>) (m => m.SerialNumber == searchText)).Select<Meter, Guid>((Expression<Func<Meter, Guid>>) (m => m.Id)).ToList<Guid>();
      List<StructureNodeLinks> list1 = ((IEnumerable<StructureNodeLinks>) EagerFetchingExtensionMethods.Fetch<StructureNodeLinks, StructureNode>((IQueryable<StructureNodeLinks>) EagerFetchingExtensionMethods.Fetch<StructureNodeLinks, StructureNode>(LinqExtensionMethods.Query<StructureNodeLinks>(this._repositoryFactory.GetSession()).Where<StructureNodeLinks>((Expression<Func<StructureNodeLinks, bool>>) (l => (l.Node.Name.Contains(searchText) || l.Node.Description.Contains(searchText) || metersWithSameSerialNumberAsSearchText.Contains(l.Node.EntityId)) && l.EndDate == new DateTime?())), (Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.Node)), (Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.RootNode))).ToList<StructureNodeLinks>();
      List<StructureNode> list2 = list1.Select<StructureNodeLinks, StructureNode>((Func<StructureNodeLinks, StructureNode>) (l => l.Node)).ToList<StructureNode>();
      IEnumerable<Guid> source = list1.Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (l => l.RootNode.Id));
      IList<StructureNodeType> all = this._repositoryFactory.GetRepository<StructureNodeType>().GetAll();
      ObservableCollection<StructureNodeDTO> structures = new ObservableCollection<StructureNodeDTO>();
      foreach (Guid rootNodeId in source.Distinct<Guid>())
      {
        Dictionary<Guid, object> entitiesDictionary;
        List<StructureNodeLinks> structureLinksWithNodes = this._repositoryFactory.GetStructureNodeRepository().GetStructureLinksWithNodes(new StructureTypeEnum?(structureTypeEnum), rootNodeId, out entitiesDictionary, out List<string> _);
        foreach (StructureNodeDTO treeFrom in (Collection<StructureNodeDTO>) StructuresHelper.GetTreeFromList((IEnumerable<StructureNodeType>) all, (IEnumerable<StructureNodeLinks>) structureLinksWithNodes, (IEnumerable<StructureNode>) list2, entitiesDictionary))
          structures.Add(treeFrom);
      }
      return structures;
    }

    public Structure GetStructure(
      OrderSerializableStructure orderserializablestructure)
    {
      Structure structure = new Structure()
      {
        Locations = new List<Location>(),
        Meters = new List<Meter>(),
        Nodes = new List<StructureNode>(),
        Links = new List<StructureNodeLinks>(),
        Tenants = new List<Tenant>(),
        Minomats = new List<Minomat>(),
        MeterReplacementHistory = new List<MeterReplacementHistorySerializableDTO>()
      };
      IList<StructureNodeType> nodeTypes = this._structureNodeTypeRepository.GetAll();
      IList<Channel> channels = this._channelRepository.GetAll();
      IList<MeasureUnit> measureUnits = this._measureUniteRepository.GetAll();
      IList<RoomType> roomTypes = this._roomTypeRepository.GetAll();
      IList<Provider> providers = this._repositoryFactory.GetRepository<Provider>().GetAll();
      IList<ConnectedDeviceType> connectedDeviceTypes = this._repositoryFactory.GetRepository<ConnectedDeviceType>().GetAll();
      orderserializablestructure.minomatList.ForEach((Action<MinomatSerializableDTO>) (m => structure.Minomats.Add(new Minomat()
      {
        Id = m.Id,
        EndDate = m.EndDate,
        IsDeactivated = m.IsDeactivated,
        StartDate = m.StartDate,
        AccessPoint = m.AccessPoint,
        Challenge = m.Challenge,
        CreatedBy = m.CreatedBy,
        CreatedOn = m.CreatedOn,
        GsmId = m.GsmId,
        HostAndPort = m.HostAndPort,
        IsInMasterPool = m.IsInMasterPool,
        IsMaster = m.IsMaster,
        LastUpdatedBy = m.LastUpdatedBy,
        LastChangedOn = m.LastChangedOn,
        RadioId = m.RadioId,
        Provider = providers.FirstOrDefault<Provider>((Func<Provider, bool>) (p => p.Id == m.ProviderId)),
        Polling = m.Polling,
        ProviderName = m.ProviderName,
        Registered = m.Registered,
        SessionKey = m.SessionKey,
        SimPin = m.SimPin,
        Status = m.Status
      })));
      orderserializablestructure.structureNodeList.ForEach((Action<StructureNodeSerializableDTO>) (s => structure.Nodes.Add(new StructureNode()
      {
        Id = s.Id,
        Description = s.Description,
        Name = s.Name,
        EndDate = s.EndDate,
        EntityId = s.EntityId,
        EntityName = s.EntityName,
        NodeType = nodeTypes.FirstOrDefault<StructureNodeType>((Func<StructureNodeType, bool>) (t => t.Id == s.NodeType)),
        StartDate = s.StartDate,
        LastChangedOn = s.LastChangedOn
      })));
      orderserializablestructure.structureNodesLinksList.ForEach((Action<StructureNodeLinksSerializableDTO>) (l => structure.Links.Add(new StructureNodeLinks()
      {
        Id = l.Id,
        EndDate = l.EndDate,
        StartDate = l.StartDate,
        Node = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == l.NodeId)),
        ParentNodeId = l.ParentNodeId,
        RootNode = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == l.RootNodeId)),
        StructureType = l.StructureType,
        OrderNr = l.OrderNr,
        LastChangedOn = l.LastChangedOn
      })));
      orderserializablestructure.locationList.ForEach((Action<LocationSerializableDTO>) (l => structure.Locations.Add(new Location()
      {
        BuildingNr = l.BuildingNr,
        City = l.City,
        Description = l.Description,
        DueDate = l.DueDate,
        Generation = (GenerationEnum) l.Generation,
        HasMaster = l.HasMaster,
        Id = l.Id,
        Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) l.ScenarioId),
        Street = l.Street,
        ZipCode = l.ZipCode,
        Country = l.CountryId != Guid.Empty ? this._repositoryFactory.GetRepository<Country>().GetById((object) l.CountryId) : (Country) null,
        LastChangedOn = l.LastChangedOn
      })));
      orderserializablestructure.tenantList.ForEach((Action<TenantSerializableDTO>) (t => structure.Tenants.Add(new Tenant()
      {
        ApartmentNr = t.ApartmentNr,
        CustomerTenantNo = t.CustomerTenantNo,
        Description = t.Description,
        Direction = t.Direction,
        FloorName = t.FloorName,
        FloorNr = t.FloorNr,
        Id = t.Id,
        IsDeactivated = t.IsDeactivated,
        Name = t.Name,
        TenantNr = t.TenantNr,
        Entrance = t.Entrance,
        LastChangedOn = t.LastChangedOn
      })));
      orderserializablestructure.meterList.ForEach((Action<MeterSerializableDTO>) (m => structure.Meters.Add(new Meter()
      {
        Channel = channels.FirstOrDefault<Channel>((Func<Channel, bool>) (c => c.Id == m.ChannelId)),
        CompletDevice = m.CompletDevice,
        ConnectedDeviceType = connectedDeviceTypes.FirstOrDefault<ConnectedDeviceType>((Func<ConnectedDeviceType, bool>) (c => c.Id == m.ConnectedDeviceTypeId)),
        DeviceType = m.DeviceType,
        Id = m.Id,
        EvaluationFactor = m.EvaluationFactor,
        ImpulsUnit = measureUnits.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (mu => mu.Id == m.ImpulsUnitId)),
        ImpulsValue = m.ImpulsValue,
        IsDeactivated = m.IsDeactivated,
        ReadingUnit = measureUnits.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (mu => mu.Id == m.ReadingUnitId)),
        Room = roomTypes.FirstOrDefault<RoomType>((Func<RoomType, bool>) (r => r.Id == m.RoomTypeId)),
        SerialNumber = m.SerialNumber,
        ShortDeviceNo = m.ShortDeviceNo,
        StartValue = m.StartValue,
        IsConfigured = m.IsConfigured,
        IsReplaced = m.IsReplaced,
        Manufacturer = m.Manufacturer,
        Medium = m.Medium,
        PrimaryAddress = m.PrimaryAddress,
        Generation = m.Generation,
        LastChangedOn = m.LastChangedOn
      })));
      orderserializablestructure.meterReplacementHistoryList.ForEach(new Action<MeterReplacementHistorySerializableDTO>(structure.MeterReplacementHistory.Add));
      orderserializablestructure.locationList.ForEach((Action<LocationSerializableDTO>) (locationDTO => structure.RootNodeId = orderserializablestructure.structureNodesLinksList == null || orderserializablestructure.structureNodesLinksList.Any<StructureNodeLinksSerializableDTO>() ? Guid.Empty : orderserializablestructure.structureNodesLinksList.First<StructureNodeLinksSerializableDTO>().RootNodeId));
      return structure;
    }

    public Structure GetStructure(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      Mapper.CreateMap<MeterDTO, Meter>();
      Mapper.CreateMap<LocationDTO, Location>();
      Mapper.CreateMap<TenantDTO, Tenant>();
      Mapper.CreateMap<MinomatSerializableDTO, Minomat>().ForMember((Expression<Func<Minomat, object>>) (x => x.Provider), (Action<IMemberConfigurationExpression<MinomatSerializableDTO>>) (x => x.ResolveUsing((Func<MinomatSerializableDTO, object>) (y => (object) this._repositoryFactory.GetRepository<Provider>().GetById((object) y.ProviderId)))));
      Mapper.CreateMap<StructureNodeDTO, StructureNode>().ForMember((Expression<Func<StructureNode, object>>) (x => x.NodeType), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (ign => ign.Ignore()));
      Mapper.CreateMap<StructureNodeDTO, StructureNodeLinks>();
      Structure structure = new Structure()
      {
        Locations = new List<Location>(),
        Meters = new List<Meter>(),
        Nodes = new List<StructureNode>(),
        Links = new List<StructureNodeLinks>(),
        Tenants = new List<Tenant>(),
        Minomats = new List<Minomat>(),
        MeterReplacementHistory = new List<MeterReplacementHistorySerializableDTO>(),
        RootNodeId = nodeCollection.First<StructureNodeDTO>().RootNode.Id
      };
      IList<StructureNodeType> all1 = this._structureNodeTypeRepository.GetAll();
      IList<Channel> all2 = this._channelRepository.GetAll();
      IList<MeasureUnit> all3 = this._measureUniteRepository.GetAll();
      IList<RoomType> all4 = this._roomTypeRepository.GetAll();
      StructureNode structureNode1;
      if (nodeCollection.Count <= 0)
        structureNode1 = (StructureNode) null;
      else
        structureNode1 = this._repositoryFactory.GetRepository<StructureNode>().SearchForInMemoryOrDb((Expression<Func<StructureNode, bool>>) (sn => sn.Id == nodeCollection[0].RootNode.Id), (Func<StructureNode, bool>) (sn => sn.Id == nodeCollection[0].RootNode.Id)).FirstOrDefault<StructureNode>();
      StructureNode structureNode2 = structureNode1;
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        StructureNodeDTO structureNodeDTO = node;
        StructureNode destination = new StructureNode();
        Mapper.Map<StructureNodeDTO, StructureNode>(structureNodeDTO, destination);
        destination.NodeType = all1.FirstOrDefault<StructureNodeType>((Func<StructureNodeType, bool>) (t => t.Id == structureNodeDTO.NodeType.Id));
        structure.Nodes.Add(destination);
        StructureNodeLinks structureNodeLinks1 = Mapper.Map<StructureNodeDTO, StructureNodeLinks>(structureNodeDTO);
        structureNodeLinks1.Id = Guid.NewGuid();
        structureNodeLinks1.Node = destination;
        structureNodeLinks1.RootNode = structureNode2;
        structureNodeLinks1.StartDate = new DateTime?(DateTime.Now);
        StructureTypeEnum? structureType = structureNodeDTO.StructureType;
        if (structureType.HasValue)
        {
          StructureNodeLinks structureNodeLinks2 = structureNodeLinks1;
          structureType = structureNodeDTO.StructureType;
          int num = (int) structureType.Value;
          structureNodeLinks2.StructureType = (StructureTypeEnum) num;
        }
        structure.Links.Add(structureNodeLinks1);
        if (structureNodeDTO.Entity != null)
        {
          switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDTO.NodeType.Name, true))
          {
            case StructureNodeTypeEnum.Location:
              LocationDTO entity = (LocationDTO) structureNodeDTO.Entity;
              Location location = Mapper.Map<LocationDTO, Location>(entity);
              location.Scenario = entity.Scenario != null ? this._repositoryScenario.GetById((object) entity.Scenario.Id) : (Scenario) null;
              location.Country = MSS.Business.Utils.AppContext.Current.LoggedUser.Country;
              structure.Locations.Add(location);
              break;
            case StructureNodeTypeEnum.Tenant:
              Tenant tenant = Mapper.Map<TenantDTO, Tenant>((TenantDTO) structureNodeDTO.Entity);
              structure.Tenants.Add(tenant);
              break;
            case StructureNodeTypeEnum.Meter:
            case StructureNodeTypeEnum.RadioMeter:
              MeterDTO meterDTO = (MeterDTO) structureNodeDTO.Entity;
              Meter meter = Mapper.Map<MeterDTO, Meter>(meterDTO);
              meter.Room = meterDTO.Room != null ? all4.FirstOrDefault<RoomType>((Func<RoomType, bool>) (r => r.Id == meterDTO.Room.Id)) : (RoomType) null;
              meter.ReadingUnit = meterDTO.ReadingUnit != null ? all3.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (m => m.Id == meterDTO.ReadingUnit.Id)) : (MeasureUnit) null;
              meter.ImpulsUnit = meterDTO.ImpulsUnit != null ? all3.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (m => m.Id == meterDTO.ImpulsUnit.Id)) : (MeasureUnit) null;
              meter.Channel = meterDTO.Channel != null ? all2.FirstOrDefault<Channel>((Func<Channel, bool>) (c => c.Id == meterDTO.Channel.Id)) : (Channel) null;
              structure.Meters.Add(meter);
              if (meterDTO.IsReplaced)
              {
                StructuresHelper.GetMeterReplacementHistorySerializableDTO(this._meterReplacementHistoryRepository.SearchFor((Expression<Func<MeterReplacementHistory, bool>>) (h => h.CurrentMeter.Id == meterDTO.Id))).ForEach(new Action<MeterReplacementHistorySerializableDTO>(structure.MeterReplacementHistory.Add));
                break;
              }
              break;
            case StructureNodeTypeEnum.MinomatMaster:
            case StructureNodeTypeEnum.MinomatSlave:
              Minomat minomat = Mapper.Map<MinomatSerializableDTO, Minomat>((MinomatSerializableDTO) structureNodeDTO.Entity);
              structure.Minomats.Add(minomat);
              break;
          }
        }
      }
      return structure;
    }

    public Structure LoadStructure(Guid rootNodeId)
    {
      Structure structure = (Structure) null;
      if (rootNodeId != Guid.Empty)
      {
        structure = new Structure();
        structure.RootNodeId = rootNodeId;
        structure.Links = this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.RootNode.Id == rootNodeId && s.EndDate == new DateTime?())).ToList<StructureNodeLinks>();
        List<Guid> nodeIDs = structure.Links.Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (l => l.Node.Id)).ToList<Guid>();
        structure.Nodes = this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?())).ToList<StructureNode>();
        List<Guid> entityIds = structure.Nodes.Where<StructureNode>((Func<StructureNode, bool>) (n =>
        {
          Guid entityId = n.EntityId;
          return true;
        })).Select<StructureNode, Guid>((Func<StructureNode, Guid>) (n => n.EntityId)).ToList<Guid>();
        structure.Locations = this._locationRepository.SearchFor((Expression<Func<Location, bool>>) (l => entityIds.Contains(l.Id))).ToList<Location>();
        structure.Tenants = this._tenantRepository.SearchFor((Expression<Func<Tenant, bool>>) (l => entityIds.Contains(l.Id))).ToList<Tenant>();
        structure.Meters = this._meterRepository.SearchFor((Expression<Func<Meter, bool>>) (l => entityIds.Contains(l.Id))).ToList<Meter>();
        structure.Minomats = this._repositoryMinomats.SearchFor((Expression<Func<Minomat, bool>>) (l => entityIds.Contains(l.Id))).ToList<Minomat>();
      }
      return structure;
    }

    public void UnlockStructure(Guid rootStructureNode)
    {
      if (!(rootStructureNode != Guid.Empty))
        return;
      this._nhSession.FlushMode = FlushMode.Commit;
      ITransaction transaction = this._nhSession.BeginTransaction();
      IList<StructureNodeLinks> structureNodeLinks = this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.RootNode.Id == rootStructureNode && s.EndDate == new DateTime?()));
      List<Guid> nodeIDs = StructuresHelper.GetNodeIdList((IEnumerable<StructureNodeLinks>) structureNodeLinks);
      IList<StructureNode> structureNodeList = this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?()));
      foreach (StructureNodeLinks structureNodeLink in (IEnumerable<StructureNodeLinks>) structureNodeLinks)
        this.UpdateStructureNodeLink(structureNodeLink);
      foreach (StructureNode structureNode in (IEnumerable<StructureNode>) structureNodeList)
        this.UpdateStructureNode(structureNode);
      transaction.Commit();
    }

    public void TransactionalSaveNewFixedStructure(
      IList<StructureNodeDTO> nodeCollection,
      StructureNodeEquipmentSettings equipmentSettings)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        this.TransactionalSaveEntity((IEnumerable<StructureNodeDTO>) nodeCollection);
        this.TransactionalSaveNewStructure(nodeCollection, StructureTypeEnum.Fixed);
        if (equipmentSettings != null)
        {
          equipmentSettings.StructureNode = this._repositoryFactory.GetRepository<StructureNode>().GetById((object) nodeCollection[0].RootNode.Id);
          this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().TransactionalInsert(equipmentSettings);
        }
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void TransactionalSaveNewPhysicalStructure(
      IList<StructureNodeDTO> nodeCollection,
      StructureNodeEquipmentSettings equipmentSettings)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        this.TransactionalSaveEntity((IEnumerable<StructureNodeDTO>) nodeCollection);
        this.TransactionalSaveNewStructure(nodeCollection, StructureTypeEnum.Physical);
        if (equipmentSettings != null)
        {
          equipmentSettings.StructureNode = this._repositoryFactory.GetRepository<StructureNode>().GetById((object) nodeCollection[0].RootNode.Id);
          this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().TransactionalInsert(equipmentSettings);
        }
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void TransactionalSaveNewLogicalStructure(IList<StructureNodeDTO> nodeCollection)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        this.TransactionalSaveEntity((IEnumerable<StructureNodeDTO>) nodeCollection);
        this.TransactionalCreateOrUpdateLogicalStructure(nodeCollection);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void TransactionalUpdateStructure(
      IList<StructureNodeDTO> nodeCollection,
      StructureTypeEnum structureNodeType,
      StructureNodeEquipmentSettings equipmentSettings,
      IList<StructureNodeLinks> structureNodeLinksToBeDeleted = null,
      IList<StructureNode> structureNodesToBeDeleted = null)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        this.TransactionalDeleteAffectedStructureNodes(structureNodeLinksToBeDeleted, structureNodesToBeDeleted);
        this.TransactionalSaveEntity((IEnumerable<StructureNodeDTO>) nodeCollection);
        this.TransactionalCreateOrUpdateStructure(nodeCollection, structureNodeType);
        this.TransactionalUpdateOrderNrForPartialStructure(nodeCollection, structureNodeType);
        if (equipmentSettings != null && nodeCollection.Count > 0)
        {
          equipmentSettings.StructureNode = this._repositoryFactory.GetRepository<StructureNode>().GetById((object) nodeCollection[0].RootNode.Id);
          this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().TransactionalUpdate(equipmentSettings);
        }
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void RemoveStructure(StructureNodeDTO selectedNode, StructureTypeEnum structureType)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        List<StructureNodeLinks> structureNodeLinks = new List<StructureNodeLinks>();
        List<StructureNodeLinks> logicalStructureNodeLinks = new List<StructureNodeLinks>();
        List<StructureNode> structureNodes = new List<StructureNode>();
        List<StructureNodeLinks> structureNodeLinksList = new List<StructureNodeLinks>();
        switch (structureType)
        {
          case StructureTypeEnum.Physical:
            this.GetAffectedPhysicalStructureNodes(selectedNode, structureType, out structureNodeLinks, out logicalStructureNodeLinks, out structureNodes);
            break;
          case StructureTypeEnum.Logical:
            this.GetLogicalStructureNodes(selectedNode, out structureNodeLinks, out structureNodes);
            break;
          case StructureTypeEnum.Fixed:
            this.GetFixedStructureNodes(selectedNode, out structureNodeLinks, out structureNodes);
            break;
        }
        structureNodeLinks.ForEach(new Action<StructureNodeLinks>(structureNodeLinksList.Add));
        logicalStructureNodeLinks.ForEach(new Action<StructureNodeLinks>(structureNodeLinksList.Add));
        foreach (StructureNodeLinks entity in structureNodeLinksList)
          this._structureNodeLinksRepository.TransactionalDelete(entity);
        IList<StructureNodeType> all = this._structureNodeTypeRepository.GetAll();
        foreach (StructureNode structureNode1 in structureNodes.Distinct<StructureNode>())
        {
          StructureNode structureNode = structureNode1;
          StructureNodeType structureNodeType = all.FirstOrDefault<StructureNodeType>((Func<StructureNodeType, bool>) (c => c.Id == structureNode.NodeType.Id));
          if (structureNodeType != null)
          {
            StructureNodeTypeEnum structureNodeTypeEnum = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeType.Name, true);
            if (structureNode.EntityId != Guid.Empty)
            {
              switch (structureNodeTypeEnum)
              {
                case StructureNodeTypeEnum.Location:
                  this._locationRepository.TransactionalUpdate(this.GetEntity<Location>(structureNode.EntityId));
                  this._locationRepository.TransactionalDelete(this.GetEntity<Location>(structureNode.EntityId));
                  break;
                case StructureNodeTypeEnum.Tenant:
                  this._tenantRepository.TransactionalUpdate(this.GetEntity<Tenant>(structureNode.EntityId));
                  this._tenantRepository.TransactionalDelete(this.GetEntity<Tenant>(structureNode.EntityId));
                  break;
                case StructureNodeTypeEnum.Meter:
                case StructureNodeTypeEnum.RadioMeter:
                  this._meterRepository.TransactionalUpdate(this.GetEntity<Meter>(structureNode.EntityId));
                  this._meterRepository.TransactionalDelete(this.GetEntity<Meter>(structureNode.EntityId));
                  break;
                case StructureNodeTypeEnum.MinomatMaster:
                case StructureNodeTypeEnum.MinomatSlave:
                  Minomat entity = this.GetEntity<Minomat>(structureNode.EntityId);
                  this._repositoryFactory.GetRepository<Minomat>().TransactionalUpdate(entity);
                  this._repositoryFactory.GetRepository<Minomat>().TransactionalDelete(this.GetEntity<Minomat>(structureNode.EntityId));
                  break;
              }
            }
          }
          this._structureNodeRepository.TransactionalUpdate(structureNode);
          this._structureNodeRepository.TransactionalDelete(structureNode);
        }
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void DeleteStructureNodeAndDescendants(StructureNodeDTO nodeDTO)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        List<StructureNodeLinks> structureNodeLinks1 = new List<StructureNodeLinks>();
        List<StructureNodeLinks> logicalStructureNodeLinks = new List<StructureNodeLinks>();
        List<StructureNode> structureNodes = new List<StructureNode>();
        List<StructureNodeLinks> structureNodeLinks2 = new List<StructureNodeLinks>();
        StructureTypeEnum? structureType = nodeDTO.StructureType;
        if (structureType.HasValue)
        {
          switch (structureType.GetValueOrDefault())
          {
            case StructureTypeEnum.Physical:
              this.GetAffectedPhysicalStructureNodes(nodeDTO, nodeDTO.StructureType.Value, out structureNodeLinks1, out logicalStructureNodeLinks, out structureNodes);
              break;
            case StructureTypeEnum.Logical:
              this.GetLogicalStructureNodes(nodeDTO, out structureNodeLinks1, out structureNodes);
              break;
            case StructureTypeEnum.Fixed:
              this.GetFixedStructureNodes(nodeDTO, out structureNodeLinks1, out structureNodes);
              break;
          }
        }
        structureNodeLinks1.ForEach(new Action<StructureNodeLinks>(structureNodeLinks2.Add));
        logicalStructureNodeLinks.ForEach(new Action<StructureNodeLinks>(structureNodeLinks2.Add));
        this.TransactionalDeleteAffectedStructureNodes((IList<StructureNodeLinks>) structureNodeLinks2, (IList<StructureNode>) structureNodes);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void DeleteStructure(
      StructureNodeDTO selectedNode,
      StructureTypeEnum structureType,
      bool includeDescendants = true)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        List<StructureNodeLinks> structureNodeLinks1 = new List<StructureNodeLinks>();
        List<StructureNodeLinks> logicalStructureNodeLinks = new List<StructureNodeLinks>();
        List<StructureNode> structureNodes = new List<StructureNode>();
        List<StructureNodeLinks> structureNodeLinks2 = new List<StructureNodeLinks>();
        switch (structureType)
        {
          case StructureTypeEnum.Physical:
            this.GetAffectedPhysicalStructureNodes(selectedNode, structureType, out structureNodeLinks1, out logicalStructureNodeLinks, out structureNodes, includeDescendants);
            break;
          case StructureTypeEnum.Logical:
            this.GetLogicalStructureNodes(selectedNode, out structureNodeLinks1, out structureNodes, includeDescendants);
            break;
          case StructureTypeEnum.Fixed:
            this.GetFixedStructureNodes(selectedNode, out structureNodeLinks1, out structureNodes, includeDescendants);
            break;
        }
        structureNodeLinks1.ForEach(new Action<StructureNodeLinks>(structureNodeLinks2.Add));
        logicalStructureNodeLinks.ForEach(new Action<StructureNodeLinks>(structureNodeLinks2.Add));
        IRepository<Order> orderRepository = this._repositoryFactory.GetRepository<Order>();
        List<Order> orderList = new List<Order>();
        foreach (StructureNode structureNode in structureNodes)
        {
          StructureNode currentStructureNode = structureNode;
          orderList.AddRange((IEnumerable<Order>) orderRepository.SearchFor((Expression<Func<Order, bool>>) (item => item.RootStructureNodeId == currentStructureNode.Id)));
        }
        orderList.ForEach((Action<Order>) (x =>
        {
          Guid? lockedBy = x.LockedBy;
          Guid empty = Guid.Empty;
          int num;
          if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
          {
            lockedBy = x.LockedBy;
            num = !lockedBy.HasValue ? 1 : 0;
          }
          else
            num = 1;
          if (num == 0)
            return;
          x.IsDeactivated = true;
          orderRepository.TransactionalUpdate(x);
        }));
        this.TransactionalDeleteAffectedStructureNodes((IList<StructureNodeLinks>) structureNodeLinks2, (IList<StructureNode>) structureNodes);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public ObservableCollection<StructureNodeDTO> GetStructureNodesCollection(
      StructureTypeEnum structureType,
      bool loadOnDemand = false)
    {
      this._repositoryFactory.GetSession().Clear();
      IStructureNodeRepository structureNodeRepository = this._repositoryFactory.GetStructureNodeRepository();
      IList<StructureNodeType> all1 = this._repositoryFactory.GetRepository<StructureNodeType>().GetAll();
      IList<MeterReplacementHistory> all2 = this._repositoryFactory.GetRepository<MeterReplacementHistory>().GetAll();
      ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
      ObservableCollection<StructureNodeDTO> structureNodesCollection;
      if (loadOnDemand)
      {
        Dictionary<Guid, object> entitiesDictionary;
        List<StructureNodeLinks> structureRootLinks = structureNodeRepository.GetStructureRootLinks(structureType, out entitiesDictionary);
        structureNodesCollection = StructuresHelper.GetTreeFromList(all1, (IList<StructureNodeLinks>) structureRootLinks, entitiesDictionary, loadOnDemand: loadOnDemand);
      }
      else
        structureNodesCollection = this.GetNodeCollectionWithChildren(structureNodeRepository, new StructureTypeEnum?(structureType), all1, all2);
      return structureNodesCollection;
    }

    public ObservableCollection<StructureNodeDTO> GetNodeCollectionWithChildren(
      IStructureNodeRepository structRepository,
      StructureTypeEnum? structureType,
      IList<StructureNodeType> structureNodeTypeList,
      IList<MeterReplacementHistory> meterReplacementHistoryList,
      Guid rootNodeId = default (Guid))
    {
      Dictionary<Guid, object> entitiesDictionary;
      List<string> duplicateMeterSerialNumbers;
      List<StructureNodeLinks> structureLinksWithNodes = structRepository.GetStructureLinksWithNodes(structureType, rootNodeId, out entitiesDictionary, out duplicateMeterSerialNumbers);
      List<MeterReplacementHistorySerializableDTO> historySerializableDto = StructuresHelper.GetMeterReplacementHistorySerializableDTO(meterReplacementHistoryList);
      return StructuresHelper.GetTreeFromList(structureNodeTypeList, (IList<StructureNodeLinks>) structureLinksWithNodes, entitiesDictionary, duplicateMeterSerialNumbers, (IList<MeterReplacementHistorySerializableDTO>) historySerializableDto);
    }

    public string GetNameAndDescriptionForRootNodeID(Guid rootNodeId)
    {
      StructureNode structureNode = this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.Id == rootNodeId));
      return structureNode != null ? structureNode.Name + "\n" + structureNode.Description : string.Empty;
    }

    public string GetNameAndDescriptionRootForStructureBytes(byte[] structureBytes)
    {
      OrderSerializableStructure orderSerializableStructure = StructuresHelper.DeserializeStructure(structureBytes);
      if (orderSerializableStructure.structureNodesLinksList == null || orderSerializableStructure.structureNodesLinksList.Count <= 0 || !(orderSerializableStructure.structureNodesLinksList[0].RootNodeId != Guid.Empty))
        return string.Empty;
      StructureNodeSerializableDTO nodeSerializableDto = orderSerializableStructure.structureNodeList.FirstOrDefault<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (sn => sn.Id == orderSerializableStructure.structureNodesLinksList[0].RootNodeId));
      return nodeSerializableDto != null ? nodeSerializableDto.Name + "\n" + nodeSerializableDto.Description : string.Empty;
    }

    public DateTime? GetStructureDueDate(Guid rootNodeId)
    {
      StructureNode structureNode = this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.Id == rootNodeId));
      if (structureNode != null)
      {
        Location location = this._locationRepository.FirstOrDefault((Expression<Func<Location, bool>>) (l => l.Id == structureNode.EntityId));
        if (location != null)
          return location.DueDate;
      }
      return new DateTime?(DateTime.MinValue);
    }

    public DateTime? GetStructureDueDate(byte[] structureBytes)
    {
      OrderSerializableStructure serializableStructure = StructuresHelper.DeserializeStructure(structureBytes);
      return serializableStructure.locationList != null && serializableStructure.locationList.Count > 0 ? serializableStructure.locationList[0].DueDate : new DateTime?(DateTime.MinValue);
    }

    public DateTime? GetStructureDueDate(Structure structure)
    {
      StructureNodeLinks rootNode = structure.Links.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l => l.ParentNodeId == Guid.Empty && l.RootNode.Id == l.Node.Id));
      if (rootNode != null)
      {
        StructureNode node = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == rootNode.Node.Id));
        if (node != null)
        {
          Location location = structure.Locations.FirstOrDefault<Location>((Func<Location, bool>) (l => l.Id == node.EntityId));
          if (location != null)
            return location.DueDate;
        }
      }
      return new DateTime?(DateTime.MinValue);
    }

    public ObservableCollection<StructureNodeDTO> GetStructureNodeDTO(byte[] structureBytes)
    {
      ObservableCollection<StructureNodeDTO> structureNodeDto = new ObservableCollection<StructureNodeDTO>();
      if (structureBytes != null)
      {
        Structure structure = this.GetStructure(StructuresHelper.DeserializeStructure(structureBytes));
        Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
        structure.Locations.ForEach((Action<Location>) (l => entitiesDictionary.Add(l.Id, (object) l)));
        structure.Tenants.ForEach((Action<Tenant>) (t => entitiesDictionary.Add(t.Id, (object) t)));
        structure.Meters.ForEach((Action<Meter>) (m => entitiesDictionary.Add(m.Id, (object) m)));
        structure.Minomats.ForEach((Action<Minomat>) (m => entitiesDictionary.Add(m.Id, (object) m)));
        structureNodeDto = StructuresHelper.GetTreeFromList(this._structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) structure.Links, entitiesDictionary, meterReplacementHistoryList: (IList<MeterReplacementHistorySerializableDTO>) structure.MeterReplacementHistory);
      }
      return structureNodeDto;
    }

    public byte[] CreateStructureBytes(
      ObservableCollection<StructureNodeDTO> actualNodeCollection,
      byte[] structureBytes)
    {
      this.InsertEntitiesGuid(actualNodeCollection);
      this.InsertStructureNodesGuid(actualNodeCollection);
      structureBytes = StructuresHelper.SerializeStructure(actualNodeCollection, structureBytes);
      return structureBytes;
    }

    public void UpdateReplacedMeter(StructureNodeDTO replacedMeter)
    {
      StructureNode byId1 = this._structureNodeRepository.GetById((object) replacedMeter.Id);
      byId1.Name = replacedMeter.Name;
      byId1.Description = replacedMeter.Description;
      this._structureNodeRepository.Update(byId1);
      MeterDTO entity = replacedMeter.Entity as MeterDTO;
      Meter byId2 = this._meterRepository.GetById((object) entity.Id);
      Mapper.Map<MeterDTO, Meter>(entity, byId2);
      this._meterRepository.Update(byId2);
    }

    public byte[] UpdateReplacedMeter(
      List<StructureNodeDTO> replacedMeterList,
      byte[] structureBytes)
    {
      foreach (StructureNodeDTO replacedMeter1 in replacedMeterList)
      {
        StructureNodeDTO replacedMeter = replacedMeter1;
        OrderSerializableStructure serializableStructure = StructuresHelper.DeserializeStructure(structureBytes);
        serializableStructure.meterReplacementHistoryList.ForEach((Action<MeterReplacementHistorySerializableDTO>) (h =>
        {
          if (!(replacedMeter.Entity is MeterDTO entity2) || !(h.ReplacedMeterId == entity2.Id))
            return;
          h.ReplacedMeterSerialNumber = entity2.SerialNumber;
          h.ReplacedMeterDeviceType = entity2.DeviceType;
        }));
        structureBytes = StructuresHelper.SerializeStructure(serializableStructure);
      }
      return structureBytes;
    }

    public void ReplacePhysicalMeterInLogicalStructure(StructureNodeDTO replacedMeter)
    {
      IRepository<MeterReplacementHistory> repository = this._repositoryFactory.GetRepository<MeterReplacementHistory>();
      MeterDTO brokenMeterDTO = replacedMeter.Entity as MeterDTO;
      if (brokenMeterDTO == null)
        return;
      MeterReplacementHistory replacementHistory = repository.FirstOrDefault((Expression<Func<MeterReplacementHistory, bool>>) (m => m.ReplacedMeter.Id == brokenMeterDTO.Id));
      if (replacementHistory != null)
      {
        Meter currentMeter = replacementHistory.CurrentMeter;
        StructureNode brokeMeterNode = this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.EntityId == brokenMeterDTO.Id && n.EndDate != new DateTime?()));
        StructureNode structureNode = this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.EntityId == currentMeter.Id && n.EndDate == new DateTime?()));
        IRepository<StructureNodeLinks> nodeLinksRepository = this._structureNodeLinksRepository;
        Expression<Func<StructureNodeLinks, bool>> predicate = (Expression<Func<StructureNodeLinks, bool>>) (l => l.Node.Id == brokeMeterNode.Id && (int) l.StructureType == 1 && l.EndDate != new DateTime?());
        foreach (StructureNodeLinks source in (IEnumerable<StructureNodeLinks>) nodeLinksRepository.SearchFor(predicate))
        {
          StructureNodeLinks entity = Mapper.Map<StructureNodeLinks, StructureNodeLinks>(source);
          entity.Node = structureNode;
          entity.EndDate = new DateTime?();
          this._structureNodeLinksRepository.Insert(entity);
        }
      }
    }

    public void TransactionalSaveMeterReplacementHistory(Guid currentMeterId, Guid replacedMeterId)
    {
      Meter byId = this._meterRepository.GetById((object) currentMeterId);
      MeterReplacementHistory entity1 = new MeterReplacementHistory()
      {
        CurrentMeter = byId,
        ReplacedMeter = this._meterRepository.GetById((object) replacedMeterId),
        ReplacementDate = DateTime.Now,
        ReplacedByUser = MSS.Business.Utils.AppContext.Current.LoggedUser
      };
      IRepository<MeterReplacementHistory> repository1 = this._repositoryFactory.GetRepository<MeterReplacementHistory>();
      repository1.TransactionalInsert(entity1);
      IRepository<MeterReplacementHistory> repository2 = repository1;
      Expression<Func<MeterReplacementHistory, bool>> predicate = (Expression<Func<MeterReplacementHistory, bool>>) (m => m.CurrentMeter.Id == replacedMeterId);
      foreach (MeterReplacementHistory entity2 in (IEnumerable<MeterReplacementHistory>) repository2.SearchFor(predicate))
      {
        entity2.CurrentMeter = byId;
        repository1.TransactionalUpdate(entity2);
      }
    }

    public void TransactionalUpdateOrderNrForPartialStructure(
      IList<StructureNodeDTO> nodeCollection,
      StructureTypeEnum structureType)
    {
      if (nodeCollection.Count <= 0 || nodeCollection.First<StructureNodeDTO>().ParentNode == null)
        return;
      ObservableCollection<StructureNodeDTO> subNodes = nodeCollection.First<StructureNodeDTO>().ParentNode.SubNodes;
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeDTO structureNodeDto in subNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (structureNode => !nodeIDs.Contains(structureNode.Id))))
        nodeIDs.Add(structureNodeDto.Id);
      IEnumerable<StructureNodeLinks> source1 = (IEnumerable<StructureNodeLinks>) this._structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (l => (int) l.StructureType == (int) structureType && nodeIDs.Contains(l.Node.Id)));
      foreach (StructureNodeDTO structureNodeDto1 in (Collection<StructureNodeDTO>) subNodes)
      {
        StructureNodeDTO structureNodeDto = structureNodeDto1;
        StructureNodeLinks source2 = source1.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l => l.Node.Id == structureNodeDto.Id && !l.EndDate.HasValue));
        if (source2 != null && structureNodeDto.OrderNr != source2.OrderNr)
        {
          StructureNodeLinks entity = Mapper.Map<StructureNodeLinks, StructureNodeLinks>(source2);
          entity.EndDate = new DateTime?(DateTime.Now);
          this._structureNodeLinksRepository.TransactionalInsert(entity);
          this.TransactionalUpdateStructureNodeLink(structureNodeDto);
        }
      }
    }
  }
}
