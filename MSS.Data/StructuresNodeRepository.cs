// Decompiled with JetBrains decompiler
// Type: MSS.Data.StructuresNodeRepository
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using Common.Library.NHibernate.Data;
using MSS.Business.DTO;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Utils.Utils;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#nullable disable
namespace MSS.Data
{
  public class StructuresNodeRepository(ISession session) : 
    MSS.Data.Repository.Repository<StructureNode>(session),
    IStructureNodeRepository
  {
    public IList<StructureNode> SearchStructureNodes(string text) => (IList<StructureNode>) null;

    public Structure LoadStructure(Guid rootNodeId)
    {
      Structure structure1 = (Structure) null;
      if (rootNodeId != Guid.Empty)
      {
        Structure structure2 = new Structure();
        structure2.RootNodeId = rootNodeId;
        structure2.Links = this._session.Query<StructureNodeLinks>().Where<StructureNodeLinks>((Expression<Func<StructureNodeLinks, bool>>) (s => s.RootNode.Id == rootNodeId && s.EndDate == new DateTime?())).ToList<StructureNodeLinks>();
        structure1 = structure2;
        List<Guid> nodeIDs = structure1.Links.Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (l => l.Node.Id)).ToList<Guid>();
        structure1.Nodes = this._session.Query<StructureNode>().Where<StructureNode>((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?())).Fetch<StructureNode, StructureNodeType>((Expression<Func<StructureNode, StructureNodeType>>) (s => s.NodeType)).ToList<StructureNode>();
        List<Guid> entityIds = structure1.Nodes.Where<StructureNode>((Func<StructureNode, bool>) (n =>
        {
          Guid entityId = n.EntityId;
          return true;
        })).Select<StructureNode, Guid>((Func<StructureNode, Guid>) (n => n.EntityId)).ToList<Guid>();
        structure1.Locations = this._session.Query<Location>().Where<Location>((Expression<Func<Location, bool>>) (l => entityIds.Contains(l.Id))).Fetch<Location, Scenario>((Expression<Func<Location, Scenario>>) (l => l.Scenario)).ToList<Location>();
        structure1.Tenants = this._session.Query<Tenant>().Where<Tenant>((Expression<Func<Tenant, bool>>) (l => entityIds.Contains(l.Id))).ToList<Tenant>();
        structure1.Meters = this._session.Query<Meter>().Where<Meter>((Expression<Func<Meter, bool>>) (l => entityIds.Contains(l.Id))).Fetch<Meter, MeasureUnit>((Expression<Func<Meter, MeasureUnit>>) (m => m.ReadingUnit)).Fetch<Meter, RoomType>((Expression<Func<Meter, RoomType>>) (m => m.Room)).Fetch<Meter, MeasureUnit>((Expression<Func<Meter, MeasureUnit>>) (m => m.ImpulsUnit)).Fetch<Meter, Channel>((Expression<Func<Meter, Channel>>) (m => m.Channel)).Fetch<Meter, ConnectedDeviceType>((Expression<Func<Meter, ConnectedDeviceType>>) (m => m.ConnectedDeviceType)).ToList<Meter>();
        structure1.Minomats = this._session.Query<Minomat>().Where<Minomat>((Expression<Func<Minomat, bool>>) (l => entityIds.Contains(l.Id))).Fetch<Minomat, Provider>((Expression<Func<Minomat, Provider>>) (m => m.Provider)).ToList<Minomat>();
      }
      return structure1;
    }

    public IEnumerable<StructureNodeDTO> Descendants(StructureNodeDTO root)
    {
      Stack<StructureNodeDTO> nodes = new Stack<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) new StructureNodeDTO[1]
      {
        root
      });
      while (nodes.Any<StructureNodeDTO>())
      {
        StructureNodeDTO node = nodes.Pop();
        yield return node;
        foreach (StructureNodeDTO n in (Collection<StructureNodeDTO>) node.SubNodes)
          nodes.Push(n);
        node = (StructureNodeDTO) null;
      }
    }

    public Dictionary<Guid, Location> GetLocationsForMeters(List<Guid> meterIds)
    {
      IList<StructuresNodeRepository.RootEntity> rootEntities = this._session.CreateCriteria<StructureNodeLinks>().CreateAlias("Node", "sn").CreateAlias("RootNode", "rn").Add((ICriterion) Restrictions.In("sn.EntityId", (ICollection) meterIds)).Add((ICriterion) Restrictions.IsNull("EndDate")).SetProjection((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property("sn.EntityId"), "EntityId").Add((IProjection) Projections.Property("rn.EntityId"), "RootEntityId")).SetResultTransformer(Transformers.AliasToBean<StructuresNodeRepository.RootEntity>()).List<StructuresNodeRepository.RootEntity>();
      List<Guid> locationIds = rootEntities.Select<StructuresNodeRepository.RootEntity, Guid>((Func<StructuresNodeRepository.RootEntity, Guid>) (e => e.RootEntityId)).Distinct<Guid>().ToList<Guid>();
      List<Guid> list = rootEntities.Select<StructuresNodeRepository.RootEntity, Guid>((Func<StructuresNodeRepository.RootEntity, Guid>) (e => e.EntityId)).Distinct<Guid>().ToList<Guid>();
      List<Location> locations = this._session.Query<Location>().Where<Location>((Expression<Func<Location, bool>>) (l => locationIds.Contains(l.Id))).ToList<Location>();
      return list.ToDictionary<Guid, Guid, Location>((Func<Guid, Guid>) (entityId => entityId), (Func<Guid, Location>) (entityId => locations.FirstOrDefault<Location>((Func<Location, bool>) (l =>
      {
        StructuresNodeRepository.RootEntity rootEntity = rootEntities.FirstOrDefault<StructuresNodeRepository.RootEntity>((Func<StructuresNodeRepository.RootEntity, bool>) (re => re.EntityId == entityId));
        return rootEntity != null && l.Id == rootEntity.RootEntityId;
      }))));
    }

    public Dictionary<Guid, Location> GetLocationsForMinomats()
    {
      DetachedCriteria alias1 = DetachedCriteria.For<StructureNodeLinks>().CreateAlias("Node", "sn").CreateAlias("RootNode", "rn");
      alias1.Add((ICriterion) Subqueries.PropertyIn("sn.EntityId", DetachedCriteria.For<Minomat>().Add((ICriterion) Restrictions.Eq("IsDeactivated", (object) false)).SetProjection((IProjection) Projections.Property("Id"))));
      alias1.Add((ICriterion) Restrictions.IsNull("EndDate")).SetProjection((IProjection) Projections.Property("rn.EntityId"));
      IList<Location> locations = this._session.CreateCriteria<Location>("l").Add((ICriterion) Subqueries.PropertyIn("l.Id", alias1)).List<Location>();
      ICriteria alias2 = this._session.CreateCriteria<StructureNodeLinks>().CreateAlias("Node", "sn").CreateAlias("RootNode", "rn");
      alias2.Add((ICriterion) Subqueries.PropertyIn("sn.EntityId", DetachedCriteria.For<Minomat>().Add((ICriterion) Restrictions.Eq("IsDeactivated", (object) false)).SetProjection((IProjection) Projections.Property("Id"))));
      alias2.Add((ICriterion) Restrictions.IsNull("EndDate")).SetProjection((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property("sn.EntityId"), "EntityId").Add((IProjection) Projections.Property("rn.EntityId"), "RootEntityId")).SetResultTransformer(Transformers.AliasToBean<StructuresNodeRepository.RootEntity>()).List<StructuresNodeRepository.RootEntity>();
      return alias2.List<StructuresNodeRepository.RootEntity>().ToDictionary<StructuresNodeRepository.RootEntity, Guid, Location>((Func<StructuresNodeRepository.RootEntity, Guid>) (rootEntity => rootEntity.EntityId), (Func<StructuresNodeRepository.RootEntity, Location>) (rootEntity => locations.FirstOrDefault<Location>((Func<Location, bool>) (l => l.Id == rootEntity.RootEntityId))));
    }

    public List<StructureNodeLinks> GetStructureLinksWithNodes(
      StructureTypeEnum? structureType,
      Guid rootNodeId,
      out Dictionary<Guid, object> entitiesDictionary,
      out List<string> duplicateMeterSerialNumbers)
    {
      string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
      HibernateMultipleDatabasesManager.Initialize(appSetting);
      ISessionFactory sessionFactory = HibernateMultipleDatabasesManager.DataSessionFactory(appSetting);
      entitiesDictionary = new Dictionary<Guid, object>();
      duplicateMeterSerialNumbers = (List<string>) null;
      IStatelessSession session = sessionFactory.OpenStatelessSession();
      List<StructureNodeLinks> list1;
      if (!(rootNodeId == new Guid()))
        list1 = session.Query<StructureNodeLinks>().Fetch<StructureNodeLinks, StructureNode>((Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.Node)).ThenFetch<StructureNodeLinks, StructureNode, StructureNodeType>((Expression<Func<StructureNode, StructureNodeType>>) (n => n.NodeType)).Where<StructureNodeLinks>((Expression<Func<StructureNodeLinks, bool>>) (s => (int?) s.StructureType == (int?) structureType && s.EndDate == new DateTime?() && s.RootNode.Id == rootNodeId)).ToList<StructureNodeLinks>();
      else
        list1 = session.Query<StructureNodeLinks>().Fetch<StructureNodeLinks, StructureNode>((Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.Node)).ThenFetch<StructureNodeLinks, StructureNode, StructureNodeType>((Expression<Func<StructureNode, StructureNodeType>>) (n => n.NodeType)).Where<StructureNodeLinks>((Expression<Func<StructureNodeLinks, bool>>) (s => (int?) s.StructureType == (int?) structureType && s.EndDate == new DateTime?())).ToList<StructureNodeLinks>();
      List<StructureNodeLinks> structureLinksWithNodes = list1;
      if (structureLinksWithNodes == null || structureLinksWithNodes.Count == 0)
        return structureLinksWithNodes;
      StringBuilder linkIds = new StringBuilder();
      structureLinksWithNodes.ForEach((Action<StructureNodeLinks>) (l => linkIds.AppendFormat("'{0}',", (object) l.Id)));
      string queryString1 = rootNodeId != Guid.Empty ? string.Format("Select m.* from [t_Meter] as m\r\n                            left outer join [t_RoomType] on m.RoomTypeId = [t_RoomType].Id\r\n                            inner join [t_StructureNode] on m.[Id] = [t_StructureNode].[EntityId]\r\n                            inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  where [t_StructureNodeLinks].Id In\r\n                            (select snl.Id from t_StructureNodeLinks as snl \r\n                            left join [t_StructureNode] as sn on snl.[NodeId] = sn.id\r\n                            where snl.StructureType ='{0}' AND snl.RootNodeId = '{1}' AND snl.EndDate is NULL and (sn.EntityName = 'Meter' or sn.EntityName = 'RadioMeter'))", (object) structureType, (object) rootNodeId) : string.Format("Select m.* from [t_Meter] as m\r\n                            left outer join [t_RoomType] on m.RoomTypeId = [t_RoomType].Id\r\n                            inner join [t_StructureNode] on m.[Id] = [t_StructureNode].[EntityId]\r\n                            inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  where [t_StructureNodeLinks].Id In\r\n                            (select snl.Id from t_StructureNodeLinks as snl \r\n                            left join [t_StructureNode] as sn on snl.[NodeId] = sn.id\r\n                            where snl.StructureType ='{0}' AND snl.EndDate is NULL and (sn.EntityName = 'Meter' or sn.EntityName = 'RadioMeter'))", (object) structureType);
      ISQLQuery sqlQuery1 = session.CreateSQLQuery(queryString1);
      sqlQuery1.AddEntity("m", typeof (Meter));
      IList<Meter> meterList = sqlQuery1.List<Meter>();
      IEnumerable<Guid> guids = meterList.Select<Meter, Guid>((Func<Meter, Guid>) (item => item.Id)).Distinct<Guid>();
      StringBuilder meterGuids = new StringBuilder();
      if (guids != null && guids.Any<Guid>())
      {
        guids.ForEach<Guid>((Action<Guid>) (item => meterGuids.AppendFormat("'{0}',", (object) item)));
        string queryString2 = "SELECT b.* FROM [t_MeterMbusRadio] as b \r\n                        WHERE b.[MeterId] IN (" + meterGuids.ToString().TrimEnd(',') + ")";
        ISQLQuery sqlQuery2 = session.CreateSQLQuery(queryString2);
        sqlQuery2.AddEntity("b", typeof (MbusRadioMeter));
        IList<MbusRadioMeter> meterMbusRadioList = sqlQuery2.List<MbusRadioMeter>();
        meterList.ForEach<Meter>((Action<Meter>) (meter =>
        {
          MbusRadioMeter mbusRadioMeter = meterMbusRadioList.FirstOrDefault<MbusRadioMeter>((Func<MbusRadioMeter, bool>) (item => item.Meter.Id == meter.Id));
          if (mbusRadioMeter == null)
            return;
          meter.MbusRadioMeter = mbusRadioMeter;
        }));
      }
      StructureTypeEnum? nullable1 = structureType;
      StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Logical;
      if ((nullable1.GetValueOrDefault() == structureTypeEnum1 ? (nullable1.HasValue ? 1 : 0) : 0) != 0 && meterList.Count > 0)
      {
        StringBuilder meterSerialNumbers = new StringBuilder();
        meterList.ForEach<Meter>((Action<Meter>) (m => meterSerialNumbers.AppendFormat("'{0}',", (object) m.SerialNumber)));
        string queryString3 = "Select SNGroups.[SerialNumber] from (Select m.[SerialNumber], COUNT(m.[SerialNumber]) as cnt   From [t_Meter] as m   where m.[SerialNumber] In (" + meterSerialNumbers.ToString().TrimEnd(',') + ")  and m.[IsDeactivated] = 0   group by m.[SerialNumber]) as SNGroups   Where cnt > 1";
        ISQLQuery sqlQuery3 = session.CreateSQLQuery(queryString3);
        duplicateMeterSerialNumbers = sqlQuery3.List<string>().ToList<string>();
      }
      string queryString4 = "Select distinct {l.*}  from [t_Location] as l \r\n                    inner join [t_StructureNode] on l.[Id] = [t_StructureNode].[EntityId]  \r\n                    inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  \r\n                    where [t_StructureNodeLinks].Id In (" + linkIds.ToString().TrimEnd(',') + ") and [t_StructureNode].[EndDate] is null and   [t_StructureNodeLinks].[StructureType] = '" + (object) structureType + "'";
      ISQLQuery sqlQuery4 = session.CreateSQLQuery(queryString4);
      sqlQuery4.AddEntity("l", typeof (Location));
      IList<Location> source1 = sqlQuery4.List<Location>();
      string queryString5 = rootNodeId != Guid.Empty ? string.Format("Select distinct t.* from [t_Tenant] as t \r\n                    inner join [t_StructureNode] on t.[Id] = [t_StructureNode].[EntityId]  \r\n                    inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  \r\n                    where [t_StructureNodeLinks].Id In (\r\n                            select snl.Id from t_StructureNodeLinks as snl\r\n                            where snl.StructureType = '{0}' and snl.RootNodeId = '{1}' AND snl.EndDate IS NULL)", (object) structureType, (object) rootNodeId) : string.Format("Select distinct t.* from [t_Tenant] as t \r\n                    inner join [t_StructureNode] on t.[Id] = [t_StructureNode].[EntityId]  \r\n                    inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  \r\n                    where [t_StructureNodeLinks].Id In (\r\n                            select snl.Id from t_StructureNodeLinks as snl\r\n                            where snl.StructureType = '{0}' AND snl.EndDate IS NULL)", (object) structureType);
      ISQLQuery sqlQuery5 = session.CreateSQLQuery(queryString5);
      sqlQuery5.AddEntity("t", typeof (Tenant));
      IList<Tenant> source2 = sqlQuery5.List<Tenant>();
      string queryString6 = "Select m.*  from [t_Minomat] as m \r\n                    inner join [t_StructureNode] on m.[Id] = [t_StructureNode].[EntityId]  \r\n                    inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  \r\n                    where [t_StructureNodeLinks].Id In (" + linkIds.ToString().TrimEnd(',') + ") and [t_StructureNode].[EndDate] is null and [t_StructureNodeLinks].[StructureType] = '" + (object) structureType + "'";
      ISQLQuery sqlQuery6 = session.CreateSQLQuery(queryString6);
      sqlQuery6.AddEntity("m", typeof (Minomat));
      IEnumerable<Minomat> source3 = sqlQuery6.List<Minomat>().DistinctBy<Minomat, Guid>((Func<Minomat, Guid>) (item => item.Id));
      List<Meter> list2 = meterList.ToList<Meter>();
      StructureTypeEnum? nullable2 = structureType;
      StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Logical;
      if (nullable2.GetValueOrDefault() == structureTypeEnum2 && nullable2.HasValue)
        list2 = meterList.GroupBy<Meter, Guid>((Func<Meter, Guid>) (item => item.Id)).Select<IGrouping<Guid, Meter>, Meter>((Func<IGrouping<Guid, Meter>, Meter>) (x => x.First<Meter>())).ToList<Meter>();
      entitiesDictionary.AddRange<Guid, object>(list2.ToDictionary<Meter, Guid, object>((Func<Meter, Guid>) (x => x.Id), (Func<Meter, object>) (x => (object) x)));
      entitiesDictionary.AddRange<Guid, object>(source1.ToDictionary<Location, Guid, object>((Func<Location, Guid>) (x => x.Id), (Func<Location, object>) (x => (object) x)));
      entitiesDictionary.AddRange<Guid, object>(source2.ToDictionary<Tenant, Guid, object>((Func<Tenant, Guid>) (x => x.Id), (Func<Tenant, object>) (x => (object) x)));
      entitiesDictionary.AddRange<Guid, object>(source3.ToDictionary<Minomat, Guid, object>((Func<Minomat, Guid>) (x => x.Id), (Func<Minomat, object>) (x => (object) x)));
      return structureLinksWithNodes;
    }

    public List<StructureNodeLinks> GetStructureRootLinks(
      StructureTypeEnum structureType,
      out Dictionary<Guid, object> entitiesDictionary)
    {
      string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
      HibernateMultipleDatabasesManager.Initialize(appSetting);
      IStatelessSession session = HibernateMultipleDatabasesManager.DataSessionFactory(appSetting).OpenStatelessSession();
      string queryString = "Select {l.*}  From [t_Location] as l inner join [t_StructureNode] on l.[Id] = [t_StructureNode].[EntityId]  inner join [t_StructureNodeLinks]  on [t_StructureNode] .Id = [t_StructureNodeLinks].NodeId  where[t_StructureNode].[EndDate] is null and   [t_StructureNodeLinks].[StructureType] = '" + (object) structureType + "'";
      ISQLQuery sqlQuery = session.CreateSQLQuery(queryString);
      sqlQuery.AddEntity("l", typeof (Location));
      IList<Location> source = sqlQuery.List<Location>();
      List<StructureNodeLinks> list = session.Query<StructureNodeLinks>().Fetch<StructureNodeLinks, StructureNode>((Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.Node)).ThenFetch<StructureNodeLinks, StructureNode, StructureNodeType>((Expression<Func<StructureNode, StructureNodeType>>) (n => n.NodeType)).Where<StructureNodeLinks>((Expression<Func<StructureNodeLinks, bool>>) (s => (int) s.StructureType == (int) structureType && s.EndDate == new DateTime?() && s.Node.Id == s.RootNode.Id)).ToList<StructureNodeLinks>();
      entitiesDictionary = new Dictionary<Guid, object>();
      entitiesDictionary = entitiesDictionary.Concat<KeyValuePair<Guid, object>>((IEnumerable<KeyValuePair<Guid, object>>) source.ToDictionary<Location, Guid, object>((Func<Location, Guid>) (x => x.Id), (Func<Location, object>) (x => (object) x))).ToDictionary<KeyValuePair<Guid, object>, Guid, object>((Func<KeyValuePair<Guid, object>, Guid>) (x => x.Key), (Func<KeyValuePair<Guid, object>, object>) (x => x.Value));
      return list.ToList<StructureNodeLinks>();
    }

    private class RootEntity
    {
      public Guid EntityId { get; set; }

      public Guid RootEntityId { get; set; }
    }
  }
}
