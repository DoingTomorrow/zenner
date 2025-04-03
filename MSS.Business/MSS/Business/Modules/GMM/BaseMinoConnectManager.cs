// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.BaseMinoConnectManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using MSS.Business.Events;
using MSS.Business.Modules.OrdersManagement;
using MSS.Core.Model.Orders;
using MSS.Core.Utils;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public abstract class BaseMinoConnectManager
  {
    protected IRepositoryFactory _repositoryFactory;
    protected IRepository<Order> _orderRepository;
    protected Guid _orderId;
    protected ProfileType _profileType;

    protected ReadingValuesManager GetReadingValuesManagerInstance()
    {
      return new ReadingValuesManager(this._repositoryFactory);
    }

    protected void SaveErrorMessage(
      string errorMessage,
      MessageLevelsEnum level,
      string serialNumber)
    {
      MSS.Core.Model.Meters.Meter meter = this._repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>().FirstOrDefault((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (item => item.SerialNumber == serialNumber && !item.IsDeactivated));
      Order order = this._orderRepository.FirstOrDefault((Expression<Func<Order, bool>>) (item => item.Id == this._orderId));
      OrderMessage entity = new OrderMessage()
      {
        Level = level,
        Message = errorMessage,
        Meter = meter,
        Order = order,
        Timepoint = DateTime.Now
      };
      this._repositoryFactory.GetRepository<OrderMessage>().Insert(entity);
    }

    protected void ShowMessage(MessageTypeEnum messageType, string messageText)
    {
      MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
      {
        MessageType = messageType,
        MessageText = messageText
      };
      EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
      {
        Message = message
      }, (object) this);
    }

    protected List<DriverTables.MeterValuesMSSRow> GetMeterValues(
      List<DriverTables.MeterMSSRow> gmmMeterList)
    {
      List<DriverTables.MeterValuesMSSRow> meterValues = new List<DriverTables.MeterValuesMSSRow>();
      if (gmmMeterList.Count > 0)
      {
        Guid meterId = gmmMeterList[0].MeterID;
        if (MeterValuesMSS.LoadMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterId) != null)
        {
          meterValues.AddRange((IEnumerable<DriverTables.MeterValuesMSSRow>) MeterValuesMSS.LoadMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterId));
          MeterValuesMSS.DeleteMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterId);
        }
      }
      return meterValues;
    }

    protected bool SaveReadingValues(string serialNumber)
    {
      List<DriverTables.MeterValuesMSSRow> meterValues = this.GetMeterValues(MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, serialNumber));
      bool db = this.SaveReadingValuesToDb(serialNumber, meterValues);
      if (this.ContainsOnlyWarningNumber(meterValues))
        return false;
      if (!this.ContainsOnlySignalStrength(meterValues))
        return db;
      this.SaveErrorMessage(string.Format(Resources.MSS_NoValidReadingValuesReceived, (object) serialNumber), MessageLevelsEnum.Warning, serialNumber);
      EventPublisher.Publish<ErrorDuringReading>(new ErrorDuringReading()
      {
        SerialNumber = serialNumber
      }, (object) this);
      return false;
    }

    private bool SaveReadingValuesToDb(
      string serialNumber,
      List<DriverTables.MeterValuesMSSRow> gmmMeterValues)
    {
      ISession session = this._repositoryFactory.GetSession();
      session.BeginTransaction();
      session.FlushMode = FlushMode.Commit;
      Order byId = this._orderRepository.GetById((object) this._orderId);
      bool db = this.GetReadingValuesManagerInstance().ConvertAndSaveReadingValues(serialNumber, gmmMeterValues, byId);
      byId.Status = StatusOrderEnum.InProgress;
      this._orderRepository.TransactionalUpdate(byId);
      session.Transaction.Commit();
      if (db)
        EventPublisher.Publish<OrderReadingValuesSavedEvent>(new OrderReadingValuesSavedEvent()
        {
          SerialNumber = serialNumber
        }, (object) this);
      return db;
    }

    private bool ContainsOnlyWarningNumber(
      List<DriverTables.MeterValuesMSSRow> gmmMeterValues)
    {
      return gmmMeterValues != null && gmmMeterValues.Count == 1 && gmmMeterValues[0].PhysicalQuantity == (byte) 22;
    }

    private bool ContainsOnlySignalStrength(
      List<DriverTables.MeterValuesMSSRow> gmmMeterValues)
    {
      foreach (DriverTables.MeterValuesMSSRow gmmMeterValue in gmmMeterValues)
      {
        if (gmmMeterValue.PhysicalQuantity != (byte) 18)
          return false;
      }
      return true;
    }
  }
}
