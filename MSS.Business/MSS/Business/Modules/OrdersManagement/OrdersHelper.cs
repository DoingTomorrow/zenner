// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.OrdersManagement.OrdersHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.OrdersManagement
{
  public class OrdersHelper
  {
    public static OrderResult GetOrderData(
      IEnumerable<Guid> orderIds,
      IRepository<Order> orderRepository,
      IRepository<StructureNodeLinks> structureNodeLinksRepository,
      IRepository<StructureNode> structureNodeReporsitory,
      IRepository<StructureNodeType> structureNodeTypeRepository,
      ISession session)
    {
      OrderResult orderResult = new OrderResult()
      {
        Orders = new List<Order>(),
        Locations = new List<Location>(),
        Meters = new List<Meter>(),
        Tenants = new List<Tenant>(),
        StructureNodeLinks = new List<StructureNodeLinks>(),
        StructureNodes = new List<StructureNode>()
      };
      foreach (Guid orderId1 in orderIds)
      {
        Guid orderId = orderId1;
        Order order = orderRepository.FirstOrDefault((Expression<Func<Order, bool>>) (o => o.Id == orderId));
        if (order != null)
        {
          orderResult.Orders.Add(order);
          Guid rootStructureNodeId = order.RootStructureNodeId;
          if (rootStructureNodeId != Guid.Empty)
          {
            IEnumerable<StructureNodeLinks> structureNodeLinks = OrdersHelper.GetStructureNodeLinks(structureNodeLinksRepository, rootStructureNodeId);
            TypeHelperExtensionMethods.ForEach<StructureNodeLinks>(structureNodeLinks, (Action<StructureNodeLinks>) (s => orderResult.StructureNodeLinks.Add(s)));
            IList<StructureNode> structureNodes = OrdersHelper.GetStructureNodes(structureNodeReporsitory, structureNodeLinks);
            TypeHelperExtensionMethods.ForEach<StructureNode>((IEnumerable<StructureNode>) structureNodes, (Action<StructureNode>) (s => orderResult.StructureNodes.Add(s)));
            foreach (StructureNode structureNode in (IEnumerable<StructureNode>) structureNodes)
            {
              if (structureNode.EntityId != Guid.Empty)
              {
                StructureNodeTypeEnum structureNodeTypeEnum = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNode.EntityName, true);
                object obj = new object();
                switch (structureNodeTypeEnum)
                {
                  case StructureNodeTypeEnum.Location:
                    object entity1 = (object) StructuresHelper.GetEntity<Location>(structureNode.EntityId, session);
                    orderResult.Locations.Add(entity1 as Location);
                    break;
                  case StructureNodeTypeEnum.Tenant:
                    object entity2 = (object) StructuresHelper.GetEntity<Tenant>(structureNode.EntityId, session);
                    orderResult.Tenants.Add(entity2 as Tenant);
                    break;
                  case StructureNodeTypeEnum.Meter:
                  case StructureNodeTypeEnum.RadioMeter:
                    object entity3 = (object) StructuresHelper.GetEntity<Meter>(structureNode.EntityId, session);
                    orderResult.Meters.Add(entity3 as Meter);
                    break;
                }
              }
            }
          }
        }
      }
      return orderResult;
    }

    private static IList<StructureNode> GetStructureNodes(
      IRepository<StructureNode> structureNodeReporsitory,
      IEnumerable<StructureNodeLinks> validStructureNodeLinks)
    {
      List<Guid> nodeIDs = StructuresHelper.GetNodeIdList(validStructureNodeLinks);
      return structureNodeReporsitory.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?()));
    }

    private static IEnumerable<StructureNodeLinks> GetStructureNodeLinks(
      IRepository<StructureNodeLinks> structureNodeLinksRepository,
      Guid rootStructureNodeId)
    {
      StructureNodeLinks structureNodeLinks = structureNodeLinksRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == rootStructureNodeId));
      return structureNodeLinks != null ? StructuresHelper.GetStructureNodesForRootNode(structureNodeLinksRepository, structureNodeLinks.Node.Id).Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => !s.EndDate.HasValue)) : (IEnumerable<StructureNodeLinks>) new List<StructureNodeLinks>();
    }

    public static IEnumerable<StructureNodeDTO> GetStructureNodeDTOForRootNode(
      Guid structureRootNodeId,
      IRepository<StructureNodeLinks> structureNodeLinksRepository,
      IRepository<StructureNode> structureNodeReporsitory,
      IRepository<StructureNodeType> structureNodeTypeRepository,
      ISession session,
      IList<MeterReplacementHistorySerializableDTO> meterReplacementHistoryRepository = null)
    {
      return StructuresHelper.GetStructureNodeDTOForRootNode(structureRootNodeId, structureNodeLinksRepository, structureNodeReporsitory, structureNodeTypeRepository, session, meterReplacementHistoryRepository);
    }

    public static IEnumerable<Tenant> GetTenants(
      IEnumerable<StructureNodeDTO> structureDTOCollection,
      ISession session)
    {
      ObservableCollection<Tenant> tenants = new ObservableCollection<Tenant>();
      foreach (StructureNodeDTO structureDto in structureDTOCollection)
      {
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) structureDto.SubNodes)
        {
          if ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), subNode.NodeType.Name, true) == StructureNodeTypeEnum.Tenant && subNode.Entity != null)
          {
            Tenant entity = StructuresHelper.GetEntity<Tenant>((subNode.Entity as TenantDTO).Id, session);
            tenants.Add(entity);
          }
        }
      }
      return (IEnumerable<Tenant>) tenants;
    }

    public static IEnumerable<Meter> GetMetersForTenant(
      Tenant tenant,
      IEnumerable<StructureNodeDTO> structureDTOCollection,
      ISession session)
    {
      ObservableCollection<Meter> metersForTenant = new ObservableCollection<Meter>();
      foreach (StructureNodeDTO structureDto in structureDTOCollection)
      {
        foreach (StructureNodeDTO subNode1 in (Collection<StructureNodeDTO>) structureDto.SubNodes)
        {
          if ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), subNode1.NodeType.Name, true) == StructureNodeTypeEnum.Tenant && subNode1.Entity != null && (subNode1.Entity as TenantDTO).Id == tenant.Id)
          {
            foreach (StructureNodeDTO subNode2 in (Collection<StructureNodeDTO>) subNode1.SubNodes)
            {
              if (subNode2.Entity != null)
              {
                Meter entity = StructuresHelper.GetEntity<Meter>((subNode2.Entity as MeterDTO).Id, session);
                metersForTenant.Add(entity);
              }
            }
          }
        }
      }
      return (IEnumerable<Meter>) metersForTenant;
    }

    public static int GetNumberOfMetersByDeviceType(
      IEnumerable<Meter> meterList,
      DeviceTypeEnum deviceType)
    {
      return meterList.Count<Meter>((Func<Meter, bool>) (m => m.DeviceType == deviceType));
    }

    public static IEnumerable<ExecuteOrderStructureNode> Descendants(ExecuteOrderStructureNode root)
    {
      Stack<ExecuteOrderStructureNode> nodes = new Stack<ExecuteOrderStructureNode>((IEnumerable<ExecuteOrderStructureNode>) new ExecuteOrderStructureNode[1]
      {
        root
      });
      while (nodes.Any<ExecuteOrderStructureNode>())
      {
        ExecuteOrderStructureNode node = nodes.Pop();
        yield return node;
        foreach (ExecuteOrderStructureNode n in (Collection<ExecuteOrderStructureNode>) node.Children)
          nodes.Push(n);
        node = (ExecuteOrderStructureNode) null;
      }
    }

    private bool StructureContainsAllMandatoryExportableData(
      Structure structure,
      out List<Meter> notExportableMeters)
    {
      List<Meter> meters = structure.Meters;
      List<Meter> source = new List<Meter>();
      notExportableMeters = new List<Meter>();
      foreach (Meter meter in meters)
      {
        if (meter.DeviceType == DeviceTypeEnum.MinotelContactRadio3 || meter.DeviceType == DeviceTypeEnum.MinomessMicroRadio3 || meter.DeviceType == DeviceTypeEnum.M7 || meter.DeviceType == DeviceTypeEnum.M6 || meter.DeviceType == DeviceTypeEnum.C5MBus || meter.DeviceType == DeviceTypeEnum.C5Radio || meter.DeviceType == DeviceTypeEnum.Minoprotect3)
          source.Add(meter);
      }
      notExportableMeters = source.Where<Meter>((Func<Meter, bool>) (x => x.SerialNumber == string.Empty)).ToList<Meter>();
      return source.All<Meter>((Func<Meter, bool>) (meter => meter.SerialNumber != string.Empty));
    }

    public bool IsReadingOrderExportable(
      Dictionary<Order, Structure> structures,
      out List<OrderErrorMessages> errorMessages,
      IRepository<StructureNodeType> _structureNodeTypeRepository)
    {
      errorMessages = new List<OrderErrorMessages>();
      List<OrderErrorMessages> messages = errorMessages;
      bool isNotExportable = true;
      TypeHelperExtensionMethods.ForEach<KeyValuePair<Order, Structure>>((IEnumerable<KeyValuePair<Order, Structure>>) structures, (Action<KeyValuePair<Order, Structure>>) (x =>
      {
        if (x.Key == null)
          return;
        StructureTypeEnum? structureType = x.Key.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
        {
          Guid? lockedBy = x.Key.LockedBy;
          Guid empty = Guid.Empty;
          int num;
          if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
          {
            lockedBy = x.Key.LockedBy;
            num = !lockedBy.HasValue ? 1 : 0;
          }
          else
            num = 1;
          if (num != 0)
          {
            string error;
            if (!this.ValidExportableCompleteStructure(x.Value, x.Key, out error))
            {
              messages.Add(new OrderErrorMessages()
              {
                Order = x.Key,
                Message = error
              });
              isNotExportable = false;
            }
            List<Meter> totalNrOfNonExportableMeters;
            if (this.IsNoMeterExportableInStructure(x.Value, out totalNrOfNonExportableMeters))
            {
              string invalidMeters = string.Empty;
              totalNrOfNonExportableMeters.ForEach((Action<Meter>) (y =>
              {
                Guid tenantId = StructuresHelper.GetParentEntityIdForNode(x.Value, y.Id);
                Tenant tenant = tenantId != Guid.Empty ? x.Value.Tenants.FirstOrDefault<Tenant>((Func<Tenant, bool>) (t => t.Id == tenantId)) : (Tenant) null;
                string[] strArray = new string[5]
                {
                  invalidMeters,
                  Environment.NewLine,
                  null,
                  null,
                  null
                };
                string str;
                if (tenant == null)
                  str = string.Empty;
                else
                  str = "Tenant:" + (object) tenant.TenantNr + " - " + tenant.Name + ";";
                strArray[2] = str;
                strArray[3] = " SerialNumber: ";
                strArray[4] = y.SerialNumber;
                invalidMeters = string.Concat(strArray);
              }));
              invalidMeters += Environment.NewLine;
              string str1 = string.Format(Resources.VALIDATION_READING_ORDER_DEVICE_TYPES, (object) invalidMeters);
              messages.Add(new OrderErrorMessages()
              {
                Order = x.Key,
                Message = str1
              });
              isNotExportable = false;
            }
          }
          else
          {
            messages.Add(new OrderErrorMessages()
            {
              Order = x.Key,
              Message = Resources.MSS_ExportOrder_Error_Message_FixedStructure
            });
            isNotExportable = false;
          }
        }
        else
        {
          messages.Add(new OrderErrorMessages()
          {
            Order = x.Key,
            Message = Resources.MSS_ExportOrder_Error_Message_FixedStructure
          });
          isNotExportable = false;
        }
        List<Meter> notExportableMeters;
        if (this.StructureContainsAllMandatoryExportableData(x.Value, out notExportableMeters))
        {
          List<string> errorListUnique;
          if (!this.TenantAndMeterUniqueness(x.Value, OrderTypeEnum.ReadingOrder, out errorListUnique, _structureNodeTypeRepository))
          {
            string uniqueError = string.Empty;
            errorListUnique.ForEach((Action<string>) (error => uniqueError += error));
            messages.Add(new OrderErrorMessages()
            {
              Order = x.Key,
              Message = uniqueError
            });
            isNotExportable = false;
          }
        }
        else
        {
          string metersWithProblems = string.Empty;
          notExportableMeters.ForEach((Action<Meter>) (m =>
          {
            Guid tenantId = StructuresHelper.GetParentEntityIdForNode(x.Value, m.Id);
            Tenant tenant = tenantId != Guid.Empty ? x.Value.Tenants.FirstOrDefault<Tenant>((Func<Tenant, bool>) (t => t.Id == tenantId)) : (Tenant) null;
            string[] strArray = new string[5]
            {
              metersWithProblems,
              Environment.NewLine,
              null,
              null,
              null
            };
            string str;
            if (tenant == null)
              str = string.Empty;
            else
              str = "Tenant: " + (object) tenant.TenantNr + " - " + tenant.Name + ";";
            strArray[2] = str;
            strArray[3] = " SerialNumber: ";
            strArray[4] = m.SerialNumber;
            metersWithProblems = string.Concat(strArray);
          }));
          metersWithProblems += Environment.NewLine;
          string str2 = string.Format(Resources.MSS_ExportOrder_Error_Message_MissingMeterMandatoryData, (object) metersWithProblems);
          messages.Add(new OrderErrorMessages()
          {
            Order = x.Key,
            Message = str2
          });
          isNotExportable = false;
        }
      }));
      return isNotExportable;
    }

    private bool ValidExportableCompleteStructure(
      Structure structure,
      Order selectedOrder,
      out string error)
    {
      if (selectedOrder.StructureBytes != null)
      {
        if (structure == null)
        {
          error = Resources.VALIDATION_READING_ORDER_STRUCTURE_NULL;
          return false;
        }
        StructureNodeLinks rootNode = structure.Links.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == Guid.Empty && s.RootNode.Id == s.Node.Id));
        if (rootNode != null)
        {
          StructureNode structureNode1 = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == rootNode.Node.Id));
          if (structureNode1.EntityId == Guid.Empty)
          {
            error = string.Format(Resources.VALIDATION_READING_ORDER_LOCATION_IS_NULL, (object) structureNode1.Description);
            return false;
          }
          IEnumerable<StructureNodeLinks> source = structure.Links.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == rootNode.Node.Id && s.RootNode.Id == rootNode.Node.Id));
          if (source.Any<StructureNodeLinks>())
          {
            foreach (StructureNodeLinks structureNodeLinks1 in source)
            {
              StructureNodeLinks tenantLink = structureNodeLinks1;
              StructureNode structureNode2 = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == tenantLink.Node.Id));
              if (structureNode2.EntityId == Guid.Empty)
              {
                error = string.Format(Resources.VALIDATION_READING_ORDER_TENANT_IS_NULL, (object) structureNode2.Description);
                return false;
              }
              if ((structureNode2 == null || !(structureNode2.EntityName == "Minomat")) && (structureNode2 == null || !(structureNode2.EntityName == "MinomatSerializableDTO")))
              {
                foreach (StructureNodeLinks structureNodeLinks2 in structure.Links.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == tenantLink.Node.Id && s.RootNode.Id == rootNode.Node.Id)))
                {
                  StructureNodeLinks link = structureNodeLinks2;
                  StructureNode structureNode3 = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (x => x.Id == link.Node.Id));
                  if (structureNode3 != null && structureNode3.EntityId == Guid.Empty)
                  {
                    error = string.Format(Resources.VALIDATION_READING_ORDER_METER_IS_NOT_VALID, (object) structureNode3.Description);
                    return false;
                  }
                }
              }
            }
          }
          else
          {
            error = Resources.VALIDATION_READING_ORDER_NOTENANT;
            return false;
          }
        }
      }
      error = string.Empty;
      return true;
    }

    private bool IsAnyTenantWithNoValidExportableMeterInStructure(
      Structure structure,
      out List<StructureNode> tenantsWithoutMeters)
    {
      bool flag = false;
      tenantsWithoutMeters = new List<StructureNode>();
      List<StructureNodeLinks> structureNodeLinksList = new List<StructureNodeLinks>();
      StructureNodeLinks rootNode = structure.Links.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == Guid.Empty && s.RootNode.Id == s.Node.Id));
      if (rootNode != null)
      {
        IEnumerable<StructureNodeLinks> source = structure.Links.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == rootNode.Node.Id && s.RootNode.Id == rootNode.Node.Id));
        if (source.Any<StructureNodeLinks>())
        {
          foreach (StructureNodeLinks structureNodeLinks in source)
          {
            StructureNodeLinks tenantLink = structureNodeLinks;
            StructureNode structureNode = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == tenantLink.Node.Id));
            if ((structureNode == null || !(structureNode.EntityName == "Minomat")) && (structureNode == null || !(structureNode.EntityName == "MinomatSerializableDTO")) && !structure.Links.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == tenantLink.Node.Id && s.RootNode.Id == rootNode.Node.Id)).Select<StructureNodeLinks, StructureNode>((Func<StructureNodeLinks, StructureNode>) (meterLink => structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (s => s.Id == meterLink.Node.Id)))).ToList<StructureNode>().Select<StructureNode, Meter>((Func<StructureNode, Meter>) (meterNode => structure.Meters.FirstOrDefault<Meter>((Func<Meter, bool>) (m => m.Id == meterNode.EntityId && meterNode.EntityId != Guid.Empty)))).Where<Meter>((Func<Meter, bool>) (meter =>
            {
              if (meter == null)
                return false;
              return meter.DeviceType == DeviceTypeEnum.MinotelContactRadio3 || meter.DeviceType == DeviceTypeEnum.MinomessMicroRadio3 || meter.DeviceType == DeviceTypeEnum.M7 || meter.DeviceType == DeviceTypeEnum.M6 || meter.DeviceType == DeviceTypeEnum.C5MBus || meter.DeviceType == DeviceTypeEnum.C5Radio || meter.DeviceType == DeviceTypeEnum.Minoprotect3;
            })).ToList<Meter>().Any<Meter>())
            {
              flag = true;
              structureNodeLinksList.Add(tenantLink);
            }
          }
        }
      }
      List<StructureNode> meters = tenantsWithoutMeters;
      structureNodeLinksList.ForEach((Action<StructureNodeLinks>) (x => meters.Add(structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == x.Node.Id)))));
      tenantsWithoutMeters = meters;
      return flag;
    }

    private bool IsNoMeterExportableInStructure(
      Structure structure,
      out List<Meter> totalNrOfNonExportableMeters)
    {
      List<Meter> meters = structure.Meters;
      List<Meter> list = meters.Where<Meter>((Func<Meter, bool>) (meter => meter.DeviceType == DeviceTypeEnum.MinotelContactRadio3 || meter.DeviceType == DeviceTypeEnum.MinomessMicroRadio3 || meter.DeviceType == DeviceTypeEnum.M7 || meter.DeviceType == DeviceTypeEnum.M6 || meter.DeviceType == DeviceTypeEnum.C5MBus || meter.DeviceType == DeviceTypeEnum.C5Radio || meter.DeviceType == DeviceTypeEnum.Minoprotect3)).ToList<Meter>();
      totalNrOfNonExportableMeters = meters.Where<Meter>((Func<Meter, bool>) (meter => meter.DeviceType != DeviceTypeEnum.MinotelContactRadio3 && meter.DeviceType != DeviceTypeEnum.MinomessMicroRadio3 && meter.DeviceType != DeviceTypeEnum.M7 && meter.DeviceType != DeviceTypeEnum.M6 && meter.DeviceType != DeviceTypeEnum.C5MBus && meter.DeviceType != DeviceTypeEnum.C5Radio && meter.DeviceType != DeviceTypeEnum.Minoprotect3)).ToList<Meter>();
      return list.Count == 0;
    }

    private bool TenantAndMeterUniqueness(
      Structure structure,
      OrderTypeEnum orderType,
      out List<string> errorListUnique,
      IRepository<StructureNodeType> _structureNodeTypeRepository)
    {
      StructureNodeDTO rootNodeDTO = this.CreateStructureDTOFromStructure(structure, _structureNodeTypeRepository).FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.ParentNode == null && n.RootNode.Id == n.Id));
      errorListUnique = new List<string>();
      List<TenantDTO> duplicates;
      if (StructuresHelper.ValidateTenantUniqueness(rootNodeDTO, out duplicates))
      {
        List<MeterDTO> notUniqueReadingMeters;
        List<MeterDTO> notUniqueInstallationMeters;
        if (StructuresHelper.ValidationMeterUniqueness(rootNodeDTO, orderType, out notUniqueReadingMeters, out notUniqueInstallationMeters))
          return true;
        if (notUniqueInstallationMeters.Any<MeterDTO>())
        {
          string error = string.Empty;
          notUniqueInstallationMeters.ForEach((Action<MeterDTO>) (x => error = error + Resources.METER_NOT_UNIQUE_TENANT_DECRIPTION + x.TenantNo + "  " + Resources.METER_NOT_UNIQUE_TENANT_DECRIPTION + x.TenantNo + "  " + string.Format(Resources.VALIDATION_SERIAL_NUMBER_NOT_UNIQUE, (object) x.SerialNumber) + Environment.NewLine));
          errorListUnique.Add(error);
        }
        if (notUniqueReadingMeters.Any<MeterDTO>())
        {
          string error = string.Empty;
          notUniqueReadingMeters.ForEach((Action<MeterDTO>) (x => error = error + Resources.METER_NOT_UNIQUE_TENANT_DECRIPTION + x.TenantNo + "  " + Resources.METER_NOT_UNIQUE_TENANT_DECRIPTION + string.Format(Resources.VALIDATION_SERIAL_NUMBER_NOT_UNIQUE, (object) x.SerialNumber) + Environment.NewLine));
          errorListUnique.Add(error);
        }
        return false;
      }
      if (duplicates.Any<TenantDTO>())
      {
        string error = string.Empty;
        duplicates.ForEach((Action<TenantDTO>) (x => error = error + string.Format(Resources.VALIDATION_TENANT_NOT_UNIQUE, (object) x.Description) + Environment.NewLine));
        errorListUnique.Add(error);
      }
      return false;
    }

    private ObservableCollection<StructureNodeDTO> CreateStructureDTOFromStructure(
      Structure structure,
      IRepository<StructureNodeType> _structureNodeTypeRepository)
    {
      Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
      structure.Locations.ForEach((Action<Location>) (l => entitiesDictionary.Add(l.Id, (object) l)));
      structure.Tenants.ForEach((Action<Tenant>) (t => entitiesDictionary.Add(t.Id, (object) t)));
      structure.Meters.ForEach((Action<Meter>) (m => entitiesDictionary.Add(m.Id, (object) m)));
      structure.Minomats.ForEach((Action<Minomat>) (m => entitiesDictionary.Add(m.Id, (object) m)));
      return StructuresHelper.GetTreeFromList(_structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) structure.Links, entitiesDictionary);
    }
  }
}
