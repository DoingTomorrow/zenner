// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSyncProviders.PartialOrderSyncProvider`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Business.Modules.WCFRelated;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.PartialSync.Filters;
using MSS.PartialSync.Interfaces;
using MSS.PartialSync.PartialSync;
using MSS.Utils.Utils;
using NHibernate;
using NHibernate.Linq;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace MSS.PartialSync.PartialSyncProviders
{
  public class PartialOrderSyncProvider<TEntity> : 
    IPartialSyncOperation<IPartialSynchronizableEntity>
    where TEntity : Order
  {
    public IRepositoryFactory RepositoryFactory { get; set; }

    public List<Order> DownloadedOrders { get; set; }

    public List<OrderUser> DownloadedOrdersToUser { get; set; }

    public List<Structure> StructuresToBeLoaded { get; set; }

    public PartialOrderSyncProvider(IRepositoryFactory repositoryFactory)
    {
      this.RepositoryFactory = repositoryFactory;
      this.DownloadedOrders = new List<Order>();
      this.DownloadedOrdersToUser = new List<OrderUser>();
      this.StructuresToBeLoaded = new List<Structure>();
    }

    private void OnEntitySaved<T>(T entity) where T : IPartialSynchronizableEntity
    {
    }

    private void ProcessEntity(ZippedOrder entity, ISession session)
    {
      session.Save((object) entity.Order);
      entity.OrderUser.Order = entity.Order;
      Order order1 = entity.Order;
      Order order2 = entity.Order;
      DateTime? nullable1 = new DateTime?(DateTime.Now);
      DateTime? nullable2 = nullable1;
      order2.LastChangedOn = nullable2;
      DateTime? nullable3 = nullable1;
      order1.SyncTime = nullable3;
      entity.OrderUser.User = MSS.Business.Utils.AppContext.Current.LoggedUser;
      session.Save((object) entity.OrderUser);
      session.Flush();
      this.OnEntitySaved<Order>(entity.Order);
    }

    private void SaveStructures(ISession sessionn)
    {
      this.StructuresToBeLoaded.GroupBy<Structure, Guid>((Func<Structure, Guid>) (_ => _.RootNodeId)).Select<IGrouping<Guid, Structure>, Structure>((Func<IGrouping<Guid, Structure>, Structure>) (_ => _.First<Structure>())).ForEach<Structure>((Action<Structure>) (_ =>
      {
        _.Nodes.ForEach((Action<StructureNode>) (node => this.SaveEntity<StructureNode>(node, sessionn)));
        _.Links.ForEach((Action<StructureNodeLinks>) (link =>
        {
          if (link.Id == Guid.Empty)
            link.Id = Guid.NewGuid();
          this.SaveEntity<StructureNodeLinks>(link, sessionn);
        }));
        _.Locations.ForEach((Action<Location>) (location => this.SaveEntity<Location>(location, sessionn)));
        _.Tenants.ForEach((Action<Tenant>) (tenant => this.SaveEntity<Tenant>(tenant, sessionn)));
        _.Minomats.ForEach((Action<Minomat>) (minomat => sessionn.Save((object) minomat)));
        _.Meters.ForEach((Action<Meter>) (meter => this.SaveEntity<Meter>(meter, sessionn)));
      }));
      sessionn.Flush();
    }

    private void SaveEntity<TypeEntity>(TypeEntity entity, ISession session) where TypeEntity : IPartialSynchronizableEntity
    {
      session.Save((object) entity);
      this.OnEntitySaved<TypeEntity>(entity);
    }

    public void ApplyUpdate(ISession session, IPartialSyncWCFClient wcfClient)
    {
      Expression predicate1 = new OrderUserFilter<OrderUser>().ApplyReplace();
      IEnumerable<OrderUser> context = wcfClient.DeserializeContext<OrderUser>(wcfClient.GetData<OrderUser>(predicate1));
      context.Where<OrderUser>((Func<OrderUser, bool>) (_ => _.Order.Status != StatusOrderEnum.Closed)).Select<OrderUser, Guid>((Func<OrderUser, Guid>) (_ => _.Order.Id)).Except<Guid>(this.RepositoryFactory.GetRepository<TEntity>().GetAll().Where<TEntity>((Func<TEntity, bool>) (_ => _.Status != StatusOrderEnum.Closed)).Select<TEntity, Guid>((Func<TEntity, Guid>) (_ => _.Id))).ToList<Guid>().ForEach((Action<Guid>) (_ =>
      {
        this.DownloadedOrders.Add(context.Select<OrderUser, Order>((Func<OrderUser, Order>) (o => o.Order)).FirstOrDefault<Order>((Func<Order, bool>) (item => item.Id == _)));
        this.DownloadedOrdersToUser.Add(context.FirstOrDefault<OrderUser>((Func<OrderUser, bool>) (o => o.Order.Id == _)));
      }));
      this.DownloadedOrders.ForEach((Action<Order>) (_ =>
      {
        new Order2User<OrderUser>().ApplyReplace(_.Id);
        if (this.RepositoryFactory.GetRepository<TEntity>().Exists((Expression<Func<TEntity, bool>>) (order => order.RootStructureNodeId == _.RootStructureNodeId)))
          return;
        Expression predicate7 = new StructureNodeLinksFilter<StructureNodeLinks>().ApplyReplace(_.RootStructureNodeId);
        Structure structure = new Structure().InitStructure();
        structure.RootNodeId = _.RootStructureNodeId;
        structure.Links = wcfClient.DeserializeContext<StructureNodeLinks>(new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))).GetData<StructureNodeLinks>(predicate7)).ToList<StructureNodeLinks>();
        structure.Links.Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (p => p.Node.Id)).ForEach<Guid>((Action<Guid>) (n =>
        {
          Expression predicate8 = new StructureNodeFilter<StructureNode>().ApplyReplace(n);
          IEnumerable<StructureNode> source = wcfClient.DeserializeContext<StructureNode>(new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))).GetData<StructureNode>(predicate8));
          if (source.FirstOrDefault<StructureNode>().EndDate.HasValue)
            return;
          structure.Nodes.Add(source.FirstOrDefault<StructureNode>());
        }));
        structure.Nodes.Select<StructureNode, Guid>((Func<StructureNode, Guid>) (n => n.EntityId)).Where<Guid>((Func<Guid, bool>) (r => r != Guid.Empty)).ForEach<Guid>((Action<Guid>) (e =>
        {
          bool found = false;
          Expression predicate9 = new LocationFilter<Location>().ApplyReplace(e);
          wcfClient.DeserializeContext<Location>(new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))).GetData<Location>(predicate9)).FirstOrDefault<Location>().IfNotNull<Location>((Action<Location>) (item =>
          {
            structure.Locations.Add(item);
            found = true;
          }));
          if (!found)
          {
            Expression predicate10 = new TenantFilter<Tenant>().ApplyReplace(e);
            wcfClient.DeserializeContext<Tenant>(new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))).GetData<Tenant>(predicate10)).FirstOrDefault<Tenant>().IfNotNull<Tenant>((Action<Tenant>) (item =>
            {
              structure.Tenants.Add(item);
              found = true;
            }));
          }
          if (!found)
          {
            Expression predicate11 = new MeterFilter<Meter>().ApplyReplace(e);
            wcfClient.DeserializeContext<Meter>(new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))).GetData<Meter>(predicate11)).FirstOrDefault<Meter>().IfNotNull<Meter>((Action<Meter>) (item =>
            {
              structure.Meters.Add(item);
              found = true;
            }));
          }
          if (found)
            return;
          Expression predicate12 = new MinomatFilter<Minomat>().ApplyReplace(e);
          wcfClient.DeserializeContext<Minomat>(new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))).GetData<Minomat>(predicate12)).FirstOrDefault<Minomat>().IfNotNull<Minomat>((Action<Minomat>) (item =>
          {
            structure.Minomats.Add(item);
            found = true;
          }));
        }));
        this.StructuresToBeLoaded.Add(structure);
      }));
      this.DownloadedOrders.Zip<Order, OrderUser, ZippedOrder>(Enumerable.OfType<OrderUser>(this.DownloadedOrdersToUser), (Func<Order, OrderUser, ZippedOrder>) ((o, o2u) => new ZippedOrder()
      {
        Order = o,
        OrderUser = o2u
      })).ForEach<ZippedOrder>((Action<ZippedOrder>) (_ => this.ProcessEntity(_, session)));
      this.SaveStructures(session);
      this.LockRemoteEntities();
    }

    private void LockRemoteEntities()
    {
      new RemoteEntityLocker((IEnumerable<IPartialSynchronizableEntity>) this.DownloadedOrders).ApplyExecutionPlan((IPartialSyncWCFClient) new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))));
      ((IEnumerable<PropertyInfo>) typeof (Structure).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (_ => _.PropertyType.IsIEnumerableOfType<IPartialSynchronizableEntity>())).ForEach<PropertyInfo>((Action<PropertyInfo>) (_ => this.StructuresToBeLoaded.ForEach((Action<Structure>) (structure =>
      {
        IEnumerable<IPartialSynchronizableEntity> synchronizableEntities = _.GetValue((object) structure).SafeCast<IEnumerable<IPartialSynchronizableEntity>>();
        if (synchronizableEntities.Count<IPartialSynchronizableEntity>() <= 0)
          return;
        new RemoteEntityLocker(synchronizableEntities).ApplyExecutionPlan((IPartialSyncWCFClient) new PartialSyncWCFClient(new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"))));
      }))));
    }

    public int GetPriotity() => throw new NotImplementedException();

    public void UploadLocalModifications(ISession session, IPartialSyncWCFClient wcfClient)
    {
      IEnumerable<Order> orders = new MSS.Data.Repository.RepositoryFactory(session).GetRepository<Order>().GetAll().Where<Order>((Func<Order, bool>) (_ =>
      {
        DateTime? nullable;
        if (_.LastChangedOn.HasValue)
        {
          DateTime? lastChangedOn = _.LastChangedOn;
          nullable = _.SyncTime;
          if ((lastChangedOn.HasValue & nullable.HasValue ? (lastChangedOn.GetValueOrDefault() > nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            goto label_4;
        }
        nullable = _.SyncTime;
        if (nullable.HasValue)
        {
          nullable = _.LastChangedOn;
          return !nullable.HasValue;
        }
label_4:
        return true;
      }));
      orders.ForEach<Order>((Action<Order>) (_ =>
      {
        _.Filter = (MSS.Core.Model.DataFilters.Filter) null;
        _.OrderReadingValues = (IList<OrderReadingValues>) null;
        _.OrderUsers.ForEach<OrderUser>((Action<OrderUser>) (ou => ou.User.UserRoles = (IList<UserRole>) null));
      }));
      using (MemoryStream destination = new MemoryStream())
      {
        Serializer.Serialize<IEnumerable<Order>>((Stream) destination, orders.IfNotNull<IEnumerable<Order>, IEnumerable<Order>>((Func<IEnumerable<Order>, IEnumerable<Order>>) (_ => _)));
        byte[] items = Convert.FromBase64String(Convert.ToBase64String(destination.ToArray()));
        wcfClient.SendSerializedContext(typeof (Order).FullName, items);
      }
    }
  }
}
