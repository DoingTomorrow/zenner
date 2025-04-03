// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.ReadingValues.StructureReadingValuesInitializer
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.Interfaces;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.ReadingValues
{
  public class StructureReadingValuesInitializer(
    StructureNodeDTO structureNode,
    IRepositoryFactory repositoryFactory) : ReadingValuesInitializer(structureNode, repositoryFactory)
  {
    public override List<Guid> GetReplacedMeters()
    {
      List<MeterDTO> meterDtoList = new List<MeterDTO>();
      List<MeterDTO> metersFromList = this.GetMetersFromList(this.StructureNode, new List<MeterDTO>());
      List<MeterDTO> replacedMeters = this.GetReplacedMeters((IEnumerable<MeterDTO>) metersFromList);
      replacedMeters.ForEach(new Action<MeterDTO>(metersFromList.Add));
      this.StructureMeters = metersFromList;
      return replacedMeters.Select<MeterDTO, Guid>((Func<MeterDTO, Guid>) (x => x.Id)).ToList<Guid>();
    }

    public override List<MeterReadingValueDTO> GetReadingValues()
    {
      List<MeterReadingValueDTO> readingValues = new List<MeterReadingValueDTO>();
      string s = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory).GetAppParam("BatchSize").Value;
      int result;
      if (string.IsNullOrEmpty(s) || !int.TryParse(s, out result))
        result = 500;
      int count1 = this.StructureMeters.Count;
      int count2 = 0;
      int count3 = result;
      for (; count2 < count1; count2 += count3)
      {
        if (count3 > count1 - count2)
          count3 = count1 - count2;
        readingValues.AddRange((IEnumerable<MeterReadingValueDTO>) this.GetMeterReadingValueDtos(this.StructureMeters.Skip<MeterDTO>(count2).Take<MeterDTO>(count3).ToList<MeterDTO>()));
      }
      return readingValues;
    }

    private List<MeterReadingValueDTO> GetMeterReadingValueDtos(List<MeterDTO> meterList)
    {
      List<string> list = meterList.Select<MeterDTO, string>((Func<MeterDTO, string>) (x => x.SerialNumber)).ToList<string>();
      IProjection projection = Projections.Conditional((ICriterion) Subqueries.Exists(DetachedCriteria.For<OrderReadingValues>("ORV").SetProjection((IProjection) Projections.Property("ORV.MeterReadingValue.Id")).Add((ICriterion) Restrictions.EqProperty("ORV.MeterReadingValue.Id", "RV.Id"))), Projections.Constant((object) 'R'), Projections.Constant((object) 'S'));
      return this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("RV").CreateAlias("RV.Unit", "U", JoinType.LeftOuterJoin).Add((ICriterion) Restrictions.In("RV.MeterSerialNumber", (ICollection) list)).SetProjection((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.Id)), "Id").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.MeterId)), "MeterId").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => r.MeterSerialNumber)), "MeterSerialNumber").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.Date)), "Date").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.Value)), "Value").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.PhysicalQuantity)), "PhysicalQuantity").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.MeterType)), "MeterType").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.Calculation)), "Calculation").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.CalculationStart)), "CalculationStart").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.StorageInterval)), "StorageInterval").Add((IProjection) Projections.Property<MeterReadingValue>((Expression<Func<MeterReadingValue, object>>) (r => (object) r.Creation)), "Creation").Add((IProjection) Projections.Property("U.Id"), "UnitId").Add((IProjection) Projections.Property("U.Code"), "UnitCode").Add(projection, "ReadingType")).AddOrder(Order.Desc("Date")).SetResultTransformer(Transformers.AliasToBean<MeterReadingValueDTO>()).List<MeterReadingValueDTO>().ToList<MeterReadingValueDTO>();
    }

    private List<MeterDTO> StructureMeters { get; set; }

    private List<MeterDTO> GetReplacedMeters(IEnumerable<MeterDTO> foundMeters)
    {
      List<MeterDTO> replacedMeterDTOList = new List<MeterDTO>();
      foreach (MeterDTO foundMeter in foundMeters)
      {
        if (foundMeter.MeterReplacementHistoryList != null)
          foundMeter.MeterReplacementHistoryList.ForEach((Action<MeterReplacementHistorySerializableDTO>) (h => replacedMeterDTOList.Add(new MeterDTO()
          {
            SerialNumber = h.ReplacedMeterSerialNumber,
            Id = h.ReplacedMeterId
          })));
      }
      return replacedMeterDTOList;
    }

    private List<MeterDTO> GetMetersFromList(StructureNodeDTO node, List<MeterDTO> foundMeters)
    {
      if (node.Entity is MeterDTO)
        foundMeters.Add(node.Entity as MeterDTO);
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        if (subNode.Entity is MeterDTO)
          foundMeters.Add(subNode.Entity as MeterDTO);
        if (subNode.SubNodes.Count != 0)
          this.GetMetersFromList(subNode, foundMeters);
      }
      return foundMeters;
    }
  }
}
