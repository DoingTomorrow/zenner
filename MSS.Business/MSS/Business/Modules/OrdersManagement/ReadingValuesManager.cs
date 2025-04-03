// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.OrdersManagement.ReadingValuesManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Common.Library.NHibernate.Data;
using GmmDbLib.DataSets;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Structures;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.OrdersManagement
{
  public class ReadingValuesManager
  {
    private IRepositoryFactory _repositoryFactory;
    private readonly ISession _nhSession;
    private IRepository<MeterReadingValue> _meterReadingValueRepository;
    private IRepository<MSS.Core.Model.Meters.Meter> _meterRepository;

    public ReadingValuesManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._nhSession = repositoryFactory.GetSession();
      this._meterReadingValueRepository = repositoryFactory.GetRepository<MeterReadingValue>();
      this._meterRepository = repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>();
    }

    public IEnumerable<MeterReadingValue> GetReadingValuesList(ISession nhSession, string serialNr)
    {
      return nhSession.CreateCriteria<MeterReadingValue>("RV").SetProjection((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property("Date"), "Date").Add((IProjection) Projections.Property("ValueId"), "ValueId").Add((IProjection) Projections.Property("MeterSerialNumber"), "MeterSerialNumber")).Add((ICriterion) Restrictions.Eq("RV.MeterSerialNumber", (object) serialNr)).List<IList>().Select<IList, MeterReadingValue>((Func<IList, MeterReadingValue>) (l => new MeterReadingValue()
      {
        Date = (DateTime) l[0],
        ValueId = (long) l[1],
        MeterSerialNumber = (string) l[2]
      }));
    }

    public bool IsValidReadingValues(MeterReadingValue readingValue)
    {
      if (readingValue.Date <= new DateTime(1753, 1, 1))
        return false;
      return !this._meterReadingValueRepository.ExistsInMemoryOrDb((Expression<Func<MeterReadingValue, bool>>) (x => x.Date == readingValue.Date && x.ValueId == readingValue.ValueId && x.MeterSerialNumber == readingValue.MeterSerialNumber), (Func<MeterReadingValue, bool>) (x => x.Date == readingValue.Date && x.ValueId == readingValue.ValueId && x.MeterSerialNumber == readingValue.MeterSerialNumber));
    }

    public void InsertOrderReadingValues(
      ISession nhSession,
      MSS.Core.Model.Orders.Order order,
      MeterReadingValue meterReadingValue)
    {
      if (this._repositoryFactory.GetRepository<OrderReadingValues>().SearchFor((Expression<Func<OrderReadingValues, bool>>) (x => x.OrderObj.Id == order.Id && x.MeterReadingValue.Id == meterReadingValue.Id)).Count != 0)
        return;
      HibernateMultipleDatabasesManager.TransactionalInsert((object) new OrderReadingValues()
      {
        MeterReadingValue = meterReadingValue,
        OrderObj = order
      }, nhSession);
    }

    public void InsertJobReadingValues(
      ISession nhSession,
      MssReadingJob selectedJob,
      MeterReadingValue meterReadingValue)
    {
      if (selectedJob == null)
        return;
      HibernateMultipleDatabasesManager.TransactionalInsert((object) new JobReadingValues()
      {
        ReadingValue = meterReadingValue,
        Job = selectedJob
      }, nhSession);
    }

    public bool ConvertAndSaveReadingValues(
      string serialNumber,
      List<DriverTables.MeterValuesMSSRow> gmmMeterValues,
      MSS.Core.Model.Orders.Order selectedOrder)
    {
      bool flag = false;
      IRepository<MeasureUnit> repository = this._repositoryFactory.GetRepository<MeasureUnit>();
      MSS.Core.Model.Meters.Meter mssMeter = this._meterRepository.FirstOrDefault((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (x => x.SerialNumber == serialNumber));
      foreach (DriverTables.MeterValuesMSSRow gmmMeterValue in gmmMeterValues)
      {
        MeterReadingValue readingValue = this.CreateReadingValues(serialNumber, mssMeter, repository.GetAll().ToList<MeasureUnit>(), gmmMeterValue);
        if (this.IsValidReadingValues(readingValue))
        {
          this._meterReadingValueRepository.TransactionalInsert(readingValue);
          if (selectedOrder != null)
          {
            this.InsertOrderReadingValues(this._nhSession, selectedOrder, readingValue);
            flag = true;
          }
        }
        else
        {
          MeterReadingValue meterReadingValue = this._meterReadingValueRepository.FirstOrDefault((Expression<Func<MeterReadingValue, bool>>) (rv => rv.Date == readingValue.Date && rv.MeterSerialNumber == readingValue.MeterSerialNumber && rv.ValueId == readingValue.ValueId));
          if (selectedOrder != null && meterReadingValue != null)
          {
            this.InsertOrderReadingValues(this._nhSession, selectedOrder, meterReadingValue);
            flag = true;
          }
        }
      }
      return flag;
    }

    private MeterReadingValue CreateReadingValues(
      string serialNumber,
      MSS.Core.Model.Meters.Meter mssMeter,
      List<MeasureUnit> meaureUnits,
      DriverTables.MeterValuesMSSRow gmmMeterValue)
    {
      MeterReadingValue readingValues = new MeterReadingValue();
      readingValues.CreatedOn = DateTime.Now;
      readingValues.Date = gmmMeterValue.TimePoint;
      readingValues.MeterSerialNumber = serialNumber;
      readingValues.Value = gmmMeterValue.Value;
      readingValues.ValueId = Convert.ToInt64(ValueIdent.GetValueIdent(gmmMeterValue.ValueIdentIndex, gmmMeterValue.PhysicalQuantity, gmmMeterValue.MeterType, gmmMeterValue.Calculation, gmmMeterValue.CalculationStart, gmmMeterValue.StorageInterval, gmmMeterValue.Creation));
      readingValues.MeterId = mssMeter != null ? mssMeter.Id : Guid.Empty;
      string unitName = ValueIdent.GetUnit(Convert.ToInt64(readingValues.ValueId));
      MeasureUnit entity = meaureUnits.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (m => m.Code == unitName));
      if (entity == null && !string.IsNullOrEmpty(unitName))
      {
        entity = new MeasureUnit() { Code = unitName };
        this._repositoryFactory.GetRepository<MeasureUnit>().TransactionalInsert(entity);
        meaureUnits.Add(entity);
      }
      if (entity != null)
        readingValues.Unit = entity;
      long valueId = readingValues.ValueId;
      ValueIdent.ValueIdPart_StorageInterval partStorageInterval = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId);
      readingValues.StorageInterval = (long) partStorageInterval;
      readingValues.PhysicalQuantity = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId);
      readingValues.MeterType = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId);
      readingValues.CalculationStart = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId);
      readingValues.Creation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId);
      readingValues.Calculation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId);
      return readingValues;
    }

    public void ConvertAndSaveReadingValues(
      string serialNumber,
      MSS.Core.Model.Meters.Meter mssMeter,
      List<DriverTables.MeterValuesMSSRow> gmmMeterValues,
      MssReadingJob job,
      List<MeasureUnit> meaureUnits)
    {
      DateTime startDate = DateTime.Today.AddDays(-90.0);
      IList<MeterReadingValue> source = this._repositoryFactory.GetRepository<MeterReadingValue>().SearchFor((Expression<Func<MeterReadingValue, bool>>) (rv => rv.MeterSerialNumber == serialNumber && rv.Date >= startDate));
      foreach (DriverTables.MeterValuesMSSRow gmmMeterValue in gmmMeterValues)
      {
        MeterReadingValue readingValue = this.CreateReadingValues(serialNumber, mssMeter, meaureUnits, gmmMeterValue);
        bool flag = true;
        if (gmmMeterValue.TimePoint.Date >= startDate)
        {
          if (source != null && source.Any<MeterReadingValue>((Func<MeterReadingValue, bool>) (x => x.Date == readingValue.Date && x.ValueId == readingValue.ValueId)))
            flag = false;
        }
        else if (!this.IsValidReadingValues(readingValue))
          flag = false;
        if (flag)
        {
          this._meterReadingValueRepository.TransactionalInsert(readingValue);
          if (job != null)
            this.InsertJobReadingValues(this._nhSession, job, readingValue);
        }
      }
    }

    public Dictionary<Guid, Location> GetLocationForOrderReadingValues(
      IList<OrderReadingValues> orderReadingvalues)
    {
      Dictionary<Guid, Location> dictionary = new Dictionary<Guid, Location>();
      Dictionary<Guid, Location> orderReadingValues1 = new Dictionary<Guid, Location>();
      if (orderReadingvalues != null && orderReadingvalues.Any<OrderReadingValues>())
      {
        foreach (IEnumerable<OrderReadingValues> source in orderReadingvalues.GroupBy<OrderReadingValues, Guid>((Func<OrderReadingValues, Guid>) (o => o.OrderObj.Id)))
        {
          OrderReadingValues orderReadingValues2 = source.First<OrderReadingValues>();
          Structure structure = new StructuresManager(this._repositoryFactory).GetStructure(StructuresHelper.DeserializeStructure(orderReadingValues2.OrderObj.StructureBytes));
          dictionary.Add(orderReadingValues2.OrderObj.Id, structure.Locations.FirstOrDefault<Location>());
        }
      }
      if (orderReadingvalues != null && orderReadingvalues.Any<OrderReadingValues>())
      {
        foreach (OrderReadingValues orderReadingvalue in (IEnumerable<OrderReadingValues>) orderReadingvalues)
        {
          Location location = dictionary[orderReadingvalue.OrderObj.Id];
          if (location != null)
            orderReadingValues1.Add(orderReadingvalue.MeterReadingValue.Id, location);
        }
      }
      return orderReadingValues1;
    }
  }
}
