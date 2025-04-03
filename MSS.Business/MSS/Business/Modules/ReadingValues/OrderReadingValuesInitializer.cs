// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.ReadingValues.OrderReadingValuesInitializer
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.Interfaces;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.ReadingValues
{
  public class OrderReadingValuesInitializer : ReadingValuesInitializer
  {
    public OrderReadingValuesInitializer(
      OrderDTO selectedOrder,
      StructureNodeDTO structureNode,
      IRepositoryFactory repositoryFactory)
      : base(structureNode, repositoryFactory)
    {
      this.SelectedOrder = selectedOrder;
    }

    public override List<Guid> GetReplacedMeters()
    {
      return StructuresHelper.DeserializeStructure(this.SelectedOrder.StructureBytes).meterReplacementHistoryList.Select<MeterReplacementHistorySerializableDTO, Guid>((Func<MeterReplacementHistorySerializableDTO, Guid>) (x => x.ReplacedMeterId)).ToList<Guid>();
    }

    public List<MeterReadingValue> GetReadingValuesList()
    {
      return this.StructureNode.Entity is MeterDTO entity ? (List<MeterReadingValue>) this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("RV").CreateCriteria("RV.OrderReadingValues", "ORV", JoinType.InnerJoin).Add((ICriterion) Restrictions.Eq("ORV.OrderObj.Id", (object) this.SelectedOrder.Id)).Add((ICriterion) Restrictions.Eq("RV.MeterSerialNumber", (object) entity.SerialNumber)).List<MeterReadingValue>() : (List<MeterReadingValue>) this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("RV").CreateCriteria("RV.OrderReadingValues", "ORV", JoinType.InnerJoin).Add((ICriterion) Restrictions.Eq("ORV.OrderObj.Id", (object) this.SelectedOrder.Id)).List<MeterReadingValue>();
    }

    public override List<MeterReadingValueDTO> GetReadingValues()
    {
      ObservableCollection<MeterReadingValueDTO> source = Mapper.Map<IEnumerable<MeterReadingValue>, ObservableCollection<MeterReadingValueDTO>>((IEnumerable<MeterReadingValue>) this.GetReadingValuesList());
      source.ToList<MeterReadingValueDTO>().ForEach((Action<MeterReadingValueDTO>) (rv => rv.ReadingType = ReadingTypeEnum.R));
      return source.ToList<MeterReadingValueDTO>();
    }
  }
}
