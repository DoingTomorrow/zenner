// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.MBusJobsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using MSS.Business.Modules.OrdersManagement;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class MBusJobsManager
  {
    private readonly IRepositoryFactoryCreator _repositoryFactoryCreator;

    public MBusJobsManager(IRepositoryFactoryCreator repositoryFactoryCreator)
    {
      this._repositoryFactoryCreator = repositoryFactoryCreator;
    }

    public void SaveReadingValues(Job job)
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      ISession session = repositoryFactory.GetSession();
      session.BeginTransaction();
      session.FlushMode = FlushMode.Commit;
      job.Meters?.ForEach((Action<ZENNER.CommonLibrary.Entities.Meter>) (x =>
      {
        if (string.IsNullOrEmpty(x.SerialNumber))
          return;
        List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, x.SerialNumber);
        if (meterMss != null)
        {
          Guid meterId = meterMss[0].MeterID;
          List<DriverTables.MeterValuesMSSRow> gmmMeterValues = new List<DriverTables.MeterValuesMSSRow>();
          List<DriverTables.MeterValuesMSSRow> collection = MeterValuesMSS.LoadMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterId);
          if (collection != null)
          {
            gmmMeterValues.AddRange((IEnumerable<DriverTables.MeterValuesMSSRow>) collection);
            MeterValuesMSS.DeleteMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterId);
          }
          else
            GMMJobsLogger.GetLogger().Info(string.Format("No meter values were found for meter with serial number {0}", (object) x.SerialNumber));
          new ReadingValuesManager(repositoryFactory).ConvertAndSaveReadingValues(x.SerialNumber, repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>().GetById((object) x.ID), gmmMeterValues, repositoryFactory.GetRepository<MssReadingJob>().GetById((object) job.JobID), repositoryFactory.GetRepository<MeasureUnit>().GetAll().ToList<MeasureUnit>());
        }
        else
          GMMJobsLogger.GetLogger().Info(string.Format("Meter with serial number {0} was not found in GMM.", (object) x.SerialNumber));
      }));
      session.Transaction.Commit();
    }
  }
}
