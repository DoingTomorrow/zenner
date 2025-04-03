// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.OrdersManagement.OrdersManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.Business.Modules.OrdersManagement
{
  public class OrdersManager
  {
    private readonly ISession _nhSession;
    private readonly IRepository<MSS.Core.Model.Orders.Order> _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IRepository<OrderUser> _orderUserRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<OrderReadingValues> _orderReadingValuesRepository;
    private readonly IRepository<MeterReadingValue> _meterReadingValueRepository;
    private List<string> deviceModelsInLicense;

    [Inject]
    public OrdersManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._nhSession = repositoryFactory.GetSession();
      this._orderRepository = repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>();
      this._userRepository = repositoryFactory.GetUserRepository();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._orderUserRepository = repositoryFactory.GetRepository<OrderUser>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._orderReadingValuesRepository = repositoryFactory.GetRepository<OrderReadingValues>();
      this._meterReadingValueRepository = repositoryFactory.GetRepository<MeterReadingValue>();
      this.deviceModelsInLicense = LicenseHelper.GetDeviceTypes().ToList<string>();
      OrdersManager.InitalizeMappings();
    }

    private static void InitalizeMappings()
    {
      Mapper.CreateMap<MeterSerializableDTO, MeterDTO>();
      Mapper.CreateMap<TenantSerializableDTO, TenantDTO>();
      Mapper.CreateMap<LocationSerializableDTO, LocationDTO>();
      Mapper.CreateMap<MinomatSerializableDTO, MinomatDTO>();
    }

    public IEnumerable<OrderDTO> GetReadingOrdersDTO()
    {
      List<OrderDTO> readingOrdersDto = new List<OrderDTO>();
      IRepository<MSS.Core.Model.Orders.Order> orderRepository = this._orderRepository;
      Expression<Func<MSS.Core.Model.Orders.Order, bool>> predicate = (Expression<Func<MSS.Core.Model.Orders.Order, bool>>) (r => r.IsDeactivated == false && (int) r.OrderType == 0);
      Expression<Func<MSS.Core.Model.Orders.Order, IList<OrderUser>>> fetchExpression = (Expression<Func<MSS.Core.Model.Orders.Order, IList<OrderUser>>>) (o => o.OrderUsers);
      foreach (MSS.Core.Model.Orders.Order order in orderRepository.SearchWithFetch<IList<OrderUser>>(predicate, fetchExpression).ToList<MSS.Core.Model.Orders.Order>())
      {
        StructureNodeSerializableDTO nodeSerializableDto = (StructureNodeSerializableDTO) null;
        int num1 = this.GetNodes(order.StructureBytes, order.Id, new List<Guid>(), this.deviceModelsInLicense).Item2;
        if (order.StructureBytes != null)
        {
          OrderSerializableStructure serializableStructure = StructuresHelper.DeserializeStructure(order.StructureBytes);
          if (serializableStructure.structureNodesLinksList.Count > 0)
          {
            Guid rootId = serializableStructure.structureNodesLinksList[0].RootNodeId;
            nodeSerializableDto = serializableStructure.structureNodeList.FirstOrDefault<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (sn => sn.Id == rootId));
          }
        }
        List<OrderDTO> orderDtoList = readingOrdersDto;
        OrderDTO orderDto1 = new OrderDTO();
        orderDto1.Id = order.Id;
        orderDto1.InstallationNumber = order.InstallationNumber;
        orderDto1.RootStructureNodeId = order.RootStructureNodeId;
        orderDto1.StructureType = order.StructureType;
        orderDto1.Details = order.Details;
        orderDto1.Status = order.Status;
        orderDto1.UserIds = order.OrderUsers != null ? order.OrderUsers.Select<OrderUser, Guid>((Func<OrderUser, Guid>) (ou => ou.User.Id)).ToList<Guid>() : (List<Guid>) null;
        orderDto1.Exported = order.Exported;
        orderDto1.TestConfig = order.TestConfig;
        orderDto1.DeviceNumber = order.DeviceNumber;
        orderDto1.LockedBy = order.LockedBy;
        orderDto1.IsDeactivated = order.IsDeactivated;
        orderDto1.DueDate = order.DueDate;
        orderDto1.OrderType = order.OrderType;
        orderDto1.CloseOrderReason = order.CloseOrderReason;
        OrderDTO orderDto2 = orderDto1;
        Guid? lockedBy = order.LockedBy;
        Guid empty = Guid.Empty;
        int num2;
        if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() != empty ? 1 : 0) : 0) : 1) != 0)
        {
          lockedBy = order.LockedBy;
          num2 = lockedBy.HasValue ? 1 : 0;
        }
        else
          num2 = 0;
        orderDto2.IsLocked = num2 != 0;
        orderDto1.FilterId = order.Filter != null ? order.Filter.Id : Guid.Empty;
        orderDto1.StructureBytes = order.StructureBytes;
        orderDto1.RootNodeName = nodeSerializableDto?.Name;
        orderDto1.RootNodeDescription = nodeSerializableDto?.Description;
        orderDto1.DevicesCount = num1;
        orderDto1.AssignedUserName = order.OrderUsers == null || order.OrderUsers.Count <= 0 ? string.Empty : order.OrderUsers[0].User.LastName + " " + order.OrderUsers[0].User.FirstName;
        OrderDTO orderDto3 = orderDto1;
        orderDtoList.Add(orderDto3);
      }
      return (IEnumerable<OrderDTO>) readingOrdersDto;
    }

    public IEnumerable<OrderDTO> GetInstallationOrdersDTO()
    {
      List<OrderDTO> installationOrdersDto = new List<OrderDTO>();
      List<MSS.Core.Model.Orders.Order> list = this._orderRepository.SearchWithFetch<IList<OrderUser>>((Expression<Func<MSS.Core.Model.Orders.Order, bool>>) (r => r.IsDeactivated == false && (int) r.OrderType == 1), (Expression<Func<MSS.Core.Model.Orders.Order, IList<OrderUser>>>) (o => o.OrderUsers)).ToList<MSS.Core.Model.Orders.Order>();
      Guid[] orderIds = list.Select<MSS.Core.Model.Orders.Order, Guid>((Func<MSS.Core.Model.Orders.Order, Guid>) (o => o.RootStructureNodeId)).ToArray<Guid>();
      IList<StructureNode> source = this._repositoryFactory.GetRepository<StructureNode>().SearchFor((Expression<Func<StructureNode, bool>>) (sn => orderIds.Contains<Guid>(sn.Id)));
      foreach (MSS.Core.Model.Orders.Order order in list)
      {
        MSS.Core.Model.Orders.Order r = order;
        StructureNode structureNode = source.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == r.RootStructureNodeId));
        List<OrderDTO> orderDtoList = installationOrdersDto;
        OrderDTO orderDto1 = new OrderDTO();
        orderDto1.Id = r.Id;
        orderDto1.InstallationNumber = r.InstallationNumber;
        orderDto1.RootStructureNodeId = r.RootStructureNodeId;
        orderDto1.StructureType = r.StructureType;
        orderDto1.Details = r.Details;
        orderDto1.Status = r.Status;
        orderDto1.UserIds = r.OrderUsers != null ? r.OrderUsers.Select<OrderUser, Guid>((Func<OrderUser, Guid>) (ou => ou.User.Id)).ToList<Guid>() : (List<Guid>) null;
        orderDto1.Exported = r.Exported;
        orderDto1.TestConfig = r.TestConfig;
        orderDto1.DeviceNumber = r.DeviceNumber;
        orderDto1.LockedBy = r.LockedBy;
        orderDto1.IsDeactivated = r.IsDeactivated;
        orderDto1.DueDate = r.DueDate;
        orderDto1.OrderType = r.OrderType;
        orderDto1.CloseOrderReason = r.CloseOrderReason;
        OrderDTO orderDto2 = orderDto1;
        Guid? lockedBy = r.LockedBy;
        Guid empty = Guid.Empty;
        int num;
        if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() != empty ? 1 : 0) : 0) : 1) != 0)
        {
          lockedBy = r.LockedBy;
          num = lockedBy.HasValue ? 1 : 0;
        }
        else
          num = 0;
        orderDto2.IsLocked = num != 0;
        orderDto1.FilterId = r.Filter != null ? r.Filter.Id : Guid.Empty;
        orderDto1.StructureBytes = r.StructureBytes;
        orderDto1.RootNodeName = structureNode?.Name;
        orderDto1.RootNodeDescription = structureNode?.Description;
        OrderDTO orderDto3 = orderDto1;
        orderDtoList.Add(orderDto3);
      }
      return (IEnumerable<OrderDTO>) installationOrdersDto;
    }

    public void DeleteOrder(Guid orderId)
    {
      MSS.Core.Model.Orders.Order byId = this._orderRepository.GetById((object) orderId);
      byId.IsDeactivated = true;
      byId.Status = StatusOrderEnum.Closed;
      this._orderRepository.Update(byId);
    }

    public void CreateOrder(OrderDTO orderDTO, bool? updateDueDateStructureValue)
    {
      try
      {
        this._nhSession.BeginTransaction();
        MSS.Core.Model.Orders.Order entity1 = new MSS.Core.Model.Orders.Order()
        {
          InstallationNumber = orderDTO.InstallationNumber,
          RootStructureNodeId = orderDTO.RootStructureNodeId,
          Details = orderDTO.Details,
          Exported = orderDTO.Exported,
          Status = orderDTO.Status,
          DeviceNumber = orderDTO.DeviceNumber,
          LockedBy = orderDTO.LockedBy,
          TestConfig = orderDTO.TestConfig,
          IsDeactivated = orderDTO.IsDeactivated,
          DueDate = orderDTO.DueDate,
          OrderType = orderDTO.OrderType,
          CloseOrderReason = orderDTO.CloseOrderReason,
          Filter = this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetById((object) orderDTO.FilterId),
          StructureBytes = orderDTO.StructureBytes,
          StructureType = orderDTO.StructureType,
          CreatedOn = DateTime.Now
        };
        this._orderRepository.TransactionalInsert(entity1);
        foreach (Guid userId in orderDTO.UserIds)
        {
          User byId = this._userRepository.GetById((object) userId);
          this._orderUserRepository.TransactionalInsert(new OrderUser()
          {
            User = byId,
            Order = entity1
          });
        }
        bool? nullable = updateDueDateStructureValue;
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag && nullable.HasValue)
        {
          Location entity2 = this.UpdateDueDateStructureValue(orderDTO);
          if (entity2 != null)
            this._locationRepository.TransactionalUpdate(entity2);
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void EditOrder(MSS.Core.Model.Orders.Order order)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        this._orderRepository.TransactionalUpdate(order);
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void EditOrder(OrderDTO orderDTO, bool? updateDueDateStructureValue)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        MSS.Core.Model.Orders.Order order = this._orderRepository.GetById((object) orderDTO.Id);
        if (order != null)
        {
          order.InstallationNumber = orderDTO.InstallationNumber;
          order.RootStructureNodeId = orderDTO.RootStructureNodeId;
          order.Details = orderDTO.Details;
          order.Exported = orderDTO.Exported;
          order.Status = orderDTO.Status;
          order.DeviceNumber = orderDTO.DeviceNumber;
          order.LockedBy = orderDTO.LockedBy;
          order.TestConfig = orderDTO.TestConfig;
          order.DueDate = orderDTO.DueDate;
          order.IsDeactivated = orderDTO.IsDeactivated;
          order.OrderType = orderDTO.OrderType;
          order.CloseOrderReason = orderDTO.CloseOrderReason;
          order.Filter = this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetById((object) orderDTO.FilterId);
          order.StructureBytes = orderDTO.StructureBytes;
          order.StructureType = orderDTO.StructureType;
          this._orderRepository.TransactionalUpdate(order);
        }
        if (orderDTO.UserIds != null)
        {
          List<User> userList = orderDTO.UserIds.Select<Guid, User>((Func<Guid, User>) (userId => this._userRepository.GetById((object) userId))).ToList<User>();
          foreach (OrderUser entity in order.OrderUsers.Where<OrderUser>((Func<OrderUser, bool>) (ou => userList.All<User>((Func<User, bool>) (u => u.Id != ou.User.Id)))).ToList<OrderUser>())
            this._orderUserRepository.TransactionalDelete(entity);
          foreach (User user in userList.Where<User>((Func<User, bool>) (u => order.OrderUsers.All<OrderUser>((Func<OrderUser, bool>) (ou => ou.User.Id != u.Id)))))
          {
            User byId = this._userRepository.GetById((object) user.Id);
            this._orderUserRepository.TransactionalInsert(new OrderUser()
            {
              User = byId,
              Order = order
            });
          }
        }
        else
        {
          IList<OrderUser> orderUsers = order.OrderUsers;
          if (orderUsers != null)
          {
            foreach (OrderUser entity in (IEnumerable<OrderUser>) orderUsers)
              this._orderUserRepository.TransactionalDelete(entity);
          }
        }
        bool? nullable = updateDueDateStructureValue;
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag && nullable.HasValue)
        {
          Location entity = this.UpdateDueDateStructureValue(orderDTO);
          if (entity != null)
            this._locationRepository.TransactionalUpdate(entity);
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public IEnumerable<OrderDTO> GetOrders(string searchText, OrderTypeEnum orderType)
    {
      List<Guid> guidList = new List<Guid>();
      IList<MSS.Core.Model.Orders.Order> orderList = this._orderRepository.SearchFor((Expression<Func<MSS.Core.Model.Orders.Order, bool>>) (s => s.InstallationNumber.Contains(searchText) && (int) s.OrderType == (int) orderType));
      List<OrderDTO> orderDtos = new List<OrderDTO>();
      TypeHelperExtensionMethods.ForEach<MSS.Core.Model.Orders.Order>((IEnumerable<MSS.Core.Model.Orders.Order>) orderList, (Action<MSS.Core.Model.Orders.Order>) (r => orderDtos.Add(new OrderDTO()
      {
        Id = r.Id,
        InstallationNumber = r.InstallationNumber,
        RootStructureNodeId = r.RootStructureNodeId,
        StructureType = r.StructureType,
        Details = r.Details,
        Status = r.Status,
        UserIds = r.OrderUsers.Select<OrderUser, Guid>((Func<OrderUser, Guid>) (ou => ou.User.Id)).ToList<Guid>(),
        Exported = r.Exported,
        TestConfig = r.TestConfig,
        DeviceNumber = r.DeviceNumber,
        LockedBy = r.LockedBy,
        IsDeactivated = r.IsDeactivated,
        DueDate = r.DueDate,
        OrderType = r.OrderType,
        CloseOrderReason = r.CloseOrderReason,
        StructureBytes = r.StructureBytes
      })));
      return (IEnumerable<OrderDTO>) orderDtos;
    }

    public Location GetLocationBasedOnRootNodeId(Guid rootNodeId)
    {
      StructureNodeLinks link = this._structureNodeLinksRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (l => l.ParentNodeId == Guid.Empty && l.RootNode.Id == l.Node.Id && l.RootNode.Id == rootNodeId && l.EndDate == new DateTime?()));
      if (link != null && link.StructureType == StructureTypeEnum.Fixed)
      {
        StructureNode node = this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.Id == link.Node.Id && n.EndDate == new DateTime?()));
        if (node != null)
        {
          Location basedOnRootNodeId = this._locationRepository.FirstOrDefault((Expression<Func<Location, bool>>) (l => l.Id == node.EntityId));
          if (basedOnRootNodeId != null)
            return basedOnRootNodeId;
        }
      }
      return (Location) null;
    }

    private Location UpdateDueDateStructureValue(OrderDTO orderDTO)
    {
      if (orderDTO.OrderType == OrderTypeEnum.InstallationOrder && orderDTO.RootStructureNodeId != Guid.Empty)
      {
        Location basedOnRootNodeId = this.GetLocationBasedOnRootNodeId(orderDTO.RootStructureNodeId);
        if (basedOnRootNodeId != null)
        {
          basedOnRootNodeId.DueDate = new DateTime?(orderDTO.DueDate);
          return basedOnRootNodeId;
        }
      }
      return (Location) null;
    }

    public System.Tuple<ObservableCollection<ExecuteOrderStructureNode>, int> GetNodes(
      byte[] structureBytes,
      Guid orderId,
      List<Guid> metersReadByWalkBy,
      List<string> importableDeviceModelNameList)
    {
      OrderSerializableStructure orderSerializableStructure = StructuresHelper.DeserializeStructure(structureBytes);
      if (orderSerializableStructure.structureNodesLinksList == null || orderSerializableStructure.structureNodesLinksList.Count == 0)
        return (System.Tuple<ObservableCollection<ExecuteOrderStructureNode>, int>) null;
      if (metersReadByWalkBy != null && metersReadByWalkBy.Any<Guid>())
      {
        foreach (MeterSerializableDTO meterSerializableDto in orderSerializableStructure.meterList.Where<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => metersReadByWalkBy.Contains(m.Id))))
          meterSerializableDto.Status = new ReadingValueStatusEnum?(ReadingValueStatusEnum.ok);
      }
      List<string> list = orderSerializableStructure.meterList.Select<MeterSerializableDTO, string>((Func<MeterSerializableDTO, string>) (m => m.SerialNumber)).ToList<string>();
      IList<MeterReadingValue> readingValues = this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("RV").CreateCriteria("RV.OrderReadingValues", "ORV", JoinType.InnerJoin).Add((ICriterion) Restrictions.Eq("ORV.OrderObj.Id", (object) orderId)).Add((ICriterion) Restrictions.In("RV.MeterSerialNumber", (ICollection) list)).List<MeterReadingValue>();
      Guid rootId = orderSerializableStructure.structureNodesLinksList.FirstOrDefault<StructureNodeLinksSerializableDTO>().RootNodeId;
      StructureNodeSerializableDTO nodeSerializableDto = orderSerializableStructure.structureNodeList.FirstOrDefault<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (n => n.Id == rootId));
      IList<StructureNodeType> all = this._repositoryFactory.GetRepository<StructureNodeType>().GetAll();
      DateTime dueDate = this._orderRepository.GetById((object) orderId).DueDate;
      return this.ConstructExecuteOrderStructureNodeCollection(new List<StructureNodeSerializableDTO>()
      {
        nodeSerializableDto
      }, all, readingValues, orderSerializableStructure, metersReadByWalkBy, dueDate, importableDeviceModelNameList);
    }

    public System.Tuple<ObservableCollection<ExecuteOrderStructureNode>, int> ConstructExecuteOrderStructureNodeCollection(
      List<StructureNodeSerializableDTO> nodesList,
      IList<StructureNodeType> nodeTypes,
      IList<MeterReadingValue> readingValues,
      OrderSerializableStructure orderSerializableStructure,
      List<Guid> metersReadByWalkBy,
      DateTime orderDueDate,
      List<string> importableDeviceModelNameList)
    {
      ObservableCollection<ExecuteOrderStructureNode> observableCollection = new ObservableCollection<ExecuteOrderStructureNode>();
      int num1 = 0;
      StructureTypeEnum? nullable = new StructureTypeEnum?();
      if (orderSerializableStructure != null && orderSerializableStructure.structureNodesLinksList.Count > 0)
        nullable = new StructureTypeEnum?(orderSerializableStructure.structureNodesLinksList[0].StructureType);
      foreach (StructureNodeSerializableDTO nodes in nodesList)
      {
        StructureNodeSerializableDTO node = nodes;
        if (node == null)
          return (System.Tuple<ObservableCollection<ExecuteOrderStructureNode>, int>) null;
        StructureNodeType nodeType = nodeTypes.FirstOrDefault<StructureNodeType>((Func<StructureNodeType, bool>) (t => t.Id == node.NodeType));
        StructureNodeTypeEnum structureNodeTypeName = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), nodeType.Name);
        StructureNodeTypeEnum nodeTypeEnum = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), nodeType.Name);
        object nodeEntity = OrdersManager.GetNodeEntity(orderSerializableStructure, nodeTypeEnum, node);
        ExecuteOrderStructureNode node1 = new ExecuteOrderStructureNode()
        {
          Id = node.Id,
          Name = node.Name,
          NodeType = nodeTypeEnum,
          Image = StructuresHelper.GetImageForNode(nodeType, OrdersManager.GetEntityDTOFromSerializableDTO(structureNodeTypeName, nodeEntity), node.EntityId != Guid.Empty)
        };
        switch (node1.NodeType)
        {
          case StructureNodeTypeEnum.Tenant:
            TenantSerializableDTO tenantSerializableDto = orderSerializableStructure.tenantList.FirstOrDefault<TenantSerializableDTO>((Func<TenantSerializableDTO, bool>) (t => t.Id == node.EntityId));
            if (tenantSerializableDto != null)
            {
              node1.TenantFloor = string.Format("{0}/{1}/{2}/{3}", (object) tenantSerializableDto.FloorNr, (object) tenantSerializableDto.FloorName, (object) tenantSerializableDto.ApartmentNr, (object) tenantSerializableDto.Direction);
              break;
            }
            break;
          case StructureNodeTypeEnum.Meter:
          case StructureNodeTypeEnum.RadioMeter:
            MeterSerializableDTO meter = orderSerializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == node.EntityId));
            if (meter != null)
            {
              node1.MeterId = meter.Id;
              node1.SerialNumber = meter.SerialNumber;
              node1.DeviceType = new DeviceTypeEnum?(meter.DeviceType);
              node1.Room = meter.RoomTypeCode;
              node1.PrimaryAddres = meter.PrimaryAddress;
              node1.Manufacturer = meter.Manufacturer;
              node1.Medium = meter.Medium;
              node1.ReadingEnabled = meter.ReadingEnabled;
              node1.ShortDeviceNo = meter.ShortDeviceNo;
              node1.Generation = meter.Generation;
              node1.InputNumber = meter.InputNumber;
              node1.DeviceInfo = meter.DeviceInfo;
              if (meter.MbusRadioMeterId != Guid.Empty)
                node1.MbusRadioMeter = new MbusRadioMeter()
                {
                  City = meter.City,
                  Street = meter.Street,
                  HouseNumber = meter.HouseNumber,
                  HouseNumberSupplement = meter.HouseNumberSupplement,
                  ApartmentNumber = meter.ApartmentNumber,
                  ZipCode = meter.ZipCode,
                  FirstName = meter.FirstName,
                  LastName = meter.LastName,
                  Location = meter.Location,
                  RadioSerialNumber = meter.RadioSerialNumber
                };
              if (orderDueDate == DateTime.MinValue && readingValues != null && readingValues.Any<MeterReadingValue>((Func<MeterReadingValue, bool>) (r => r.MeterSerialNumber == meter.SerialNumber)))
                meter.Status = new ReadingValueStatusEnum?(ReadingValueStatusEnum.ok);
              node1.Status = meter.Status;
              ReadingValueStatusEnum? status = meter.Status;
              ReadingValueStatusEnum readingValueStatusEnum1 = ReadingValueStatusEnum.notavailable;
              int num2;
              if ((status.GetValueOrDefault() == readingValueStatusEnum1 ? (status.HasValue ? 1 : 0) : 0) == 0)
              {
                status = meter.Status;
                ReadingValueStatusEnum readingValueStatusEnum2 = ReadingValueStatusEnum.nok;
                num2 = status.GetValueOrDefault() == readingValueStatusEnum2 ? (status.HasValue ? 1 : 0) : 0;
              }
              else
                num2 = 1;
              if (num2 != 0)
              {
                OrdersManager.SetImageAndColor(node1, Brushes.OrangeRed);
              }
              else
              {
                status = meter.Status;
                ReadingValueStatusEnum readingValueStatusEnum3 = ReadingValueStatusEnum.MissingTranslationRules;
                if (status.GetValueOrDefault() == readingValueStatusEnum3 && status.HasValue)
                {
                  OrdersManager.SetImageAndColor(node1, Brushes.Yellow);
                }
                else
                {
                  status = meter.Status;
                  ReadingValueStatusEnum readingValueStatusEnum4 = ReadingValueStatusEnum.ok;
                  if (status.GetValueOrDefault() == readingValueStatusEnum4 && status.HasValue)
                  {
                    OrdersManager.SetImageAndColor(node1, Brushes.LightGreen);
                  }
                  else
                  {
                    status = meter.Status;
                    if (!status.HasValue)
                      node1.ColorState = (Brush) Brushes.Transparent;
                  }
                }
              }
              break;
            }
            break;
        }
        IEnumerable<Guid> linkIds = orderSerializableStructure.structureNodesLinksList.Where<StructureNodeLinksSerializableDTO>((Func<StructureNodeLinksSerializableDTO, bool>) (l => l.ParentNodeId == node.Id)).Select<StructureNodeLinksSerializableDTO, Guid>((Func<StructureNodeLinksSerializableDTO, Guid>) (l => l.NodeId));
        List<StructureNodeSerializableDTO> list = orderSerializableStructure.structureNodeList.Where<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (n => linkIds.Contains<Guid>(n.Id))).ToList<StructureNodeSerializableDTO>();
        if (!node1.IsMeter() || this.IsReadableMeter(node1))
        {
          System.Tuple<ObservableCollection<ExecuteOrderStructureNode>, int> tuple = this.ConstructExecuteOrderStructureNodeCollection(list, nodeTypes, readingValues, orderSerializableStructure, metersReadByWalkBy, orderDueDate, importableDeviceModelNameList);
          node1.Children = tuple.Item1;
          num1 += tuple.Item2;
        }
        if (node1.NodeType == StructureNodeTypeEnum.Tenant)
        {
          BitmapImage image = node1.Image;
          node1.Image = node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n =>
          {
            ReadingValueStatusEnum? status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.ok;
            return status.GetValueOrDefault() == readingValueStatusEnum && status.HasValue;
          })) ? StructuresHelper.Combine2Images(image, Brushes.LightGreen) : (node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n =>
          {
            ReadingValueStatusEnum? status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum5 = ReadingValueStatusEnum.nok;
            if ((status.GetValueOrDefault() == readingValueStatusEnum5 ? (status.HasValue ? 1 : 0) : 0) == 0)
            {
              status = n.Status;
              ReadingValueStatusEnum readingValueStatusEnum6 = ReadingValueStatusEnum.notavailable;
              if ((status.GetValueOrDefault() == readingValueStatusEnum6 ? (status.HasValue ? 1 : 0) : 0) == 0)
              {
                status = n.Status;
                return !status.HasValue;
              }
            }
            return true;
          })) ? StructuresHelper.Combine2Images(image, Brushes.OrangeRed) : (node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n =>
          {
            ReadingValueStatusEnum? status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.MissingTranslationRules;
            return status.GetValueOrDefault() == readingValueStatusEnum && status.HasValue;
          })) ? StructuresHelper.Combine2Images(image, Brushes.Yellow) : StructuresHelper.Combine2Images(image, (SolidColorBrush) null)));
          node1.ColorState = node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n =>
          {
            ReadingValueStatusEnum? status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.ok;
            return status.GetValueOrDefault() == readingValueStatusEnum && status.HasValue;
          })) ? (Brush) Brushes.LightGreen : (node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n =>
          {
            ReadingValueStatusEnum? status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.notavailable;
            return status.GetValueOrDefault() == readingValueStatusEnum && status.HasValue;
          })) || node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n =>
          {
            ReadingValueStatusEnum? status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum7 = ReadingValueStatusEnum.nok;
            if ((status.GetValueOrDefault() == readingValueStatusEnum7 ? (status.HasValue ? 1 : 0) : 0) != 0)
              return true;
            status = n.Status;
            ReadingValueStatusEnum readingValueStatusEnum8 = ReadingValueStatusEnum.MissingTranslationRules;
            return status.GetValueOrDefault() == readingValueStatusEnum8 && status.HasValue;
          })) || node1.Children.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.ReadingEnabled)).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => !n.Status.HasValue)) ? (Brush) Brushes.OrangeRed : (Brush) Brushes.Transparent);
        }
        if (!node1.IsMeter() || this.IsReadableMeterIncludedInLicense(node1, importableDeviceModelNameList))
          observableCollection.Add(node1);
        if (this.IsReadableMeterIncludedInLicense(node1, importableDeviceModelNameList))
          ++num1;
      }
      return new System.Tuple<ObservableCollection<ExecuteOrderStructureNode>, int>(observableCollection, num1);
    }

    private bool IsReadableMeter(ExecuteOrderStructureNode node)
    {
      return node.IsMeter() && node.ReadingEnabled;
    }

    private bool IsReadableMeterIncludedInLicense(
      ExecuteOrderStructureNode node,
      List<string> importableDeviceModelNameList)
    {
      return this.IsReadableMeter(node) && GMMHelper.IsDeviceIncludedInLicense(node.DeviceType, importableDeviceModelNameList);
    }

    private static object GetNodeEntity(
      OrderSerializableStructure orderSerializableStructure,
      StructureNodeTypeEnum nodeTypeEnum,
      StructureNodeSerializableDTO node)
    {
      switch (nodeTypeEnum)
      {
        case StructureNodeTypeEnum.Location:
          return (object) orderSerializableStructure.locationList.FirstOrDefault<LocationSerializableDTO>((Func<LocationSerializableDTO, bool>) (l => l.Id == node.EntityId));
        case StructureNodeTypeEnum.Tenant:
          return (object) orderSerializableStructure.tenantList.FirstOrDefault<TenantSerializableDTO>((Func<TenantSerializableDTO, bool>) (t => t.Id == node.EntityId));
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          return (object) orderSerializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == node.EntityId));
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          return (object) orderSerializableStructure.minomatList.FirstOrDefault<MinomatSerializableDTO>((Func<MinomatSerializableDTO, bool>) (m => m.Id == node.EntityId));
        default:
          return (object) null;
      }
    }

    public static object GetEntityDTOFromSerializableDTO(
      StructureNodeTypeEnum structureNodeTypeName,
      object entityObj)
    {
      object obj = new object();
      object fromSerializableDto = new object();
      switch (structureNodeTypeName)
      {
        case StructureNodeTypeEnum.Location:
          fromSerializableDto = (object) Mapper.Map<LocationSerializableDTO, LocationDTO>(entityObj as LocationSerializableDTO);
          break;
        case StructureNodeTypeEnum.Tenant:
          fromSerializableDto = (object) Mapper.Map<TenantSerializableDTO, TenantDTO>(entityObj as TenantSerializableDTO);
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          fromSerializableDto = (object) Mapper.Map<MeterSerializableDTO, MeterDTO>(entityObj as MeterSerializableDTO);
          break;
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          fromSerializableDto = (object) Mapper.Map<MinomatSerializableDTO, MinomatSerializableDTO>(entityObj as MinomatSerializableDTO);
          break;
      }
      return fromSerializableDto;
    }

    public static ExecuteOrderStructureNode FindNodeById(
      Guid id,
      ObservableCollection<ExecuteOrderStructureNode> nodes)
    {
      ExecuteOrderStructureNode nodeById = nodes.FirstOrDefault<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (n => n.Id == id));
      if (nodeById != null)
        return nodeById;
      using (IEnumerator<ExecuteOrderStructureNode> enumerator = nodes.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          ExecuteOrderStructureNode current = enumerator.Current;
          return OrdersManager.FindNodeById(id, current.Children);
        }
      }
      return (ExecuteOrderStructureNode) null;
    }

    public static void SetImageAndColor(
      ExecuteOrderStructureNode node,
      SolidColorBrush brush,
      BitmapImage image = null)
    {
      node.Image = StructuresHelper.Combine2Images(image ?? node.Image, brush);
      node.ColorState = (Brush) brush;
    }
  }
}
