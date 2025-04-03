// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.MinomatJobsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Common.Library.NHibernate.Data;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class MinomatJobsManager
  {
    private readonly IRepositoryFactoryCreator _repositoryFactoryCreator;

    public MinomatJobsManager(IRepositoryFactoryCreator repositoryFactoryCreator)
    {
      this._repositoryFactoryCreator = repositoryFactoryCreator;
    }

    public static void AddMinomat(Minomat mssMinomat, uint? challengeold = null, ulong? sessionKeyOld = null)
    {
      if (string.IsNullOrEmpty(mssMinomat.GsmId) || MeterListenerManager.GetMinomat(Convert.ToUInt32(mssMinomat.GsmId)) != null)
        return;
      MinomatDevice minomatDevice = new MinomatDevice()
      {
        GsmID = new uint?(Convert.ToUInt32(mssMinomat.GsmId)),
        MinolID = new uint?(Convert.ToUInt32(mssMinomat.RadioId)),
        SessionKey = new ulong?(Convert.ToUInt64(mssMinomat.SessionKey)),
        ChallengeKey = new uint?(Convert.ToUInt32(mssMinomat.Challenge)),
        ChallengeKeyOld = challengeold,
        SessionKeyOld = sessionKeyOld
      };
      MeterListenerManager.AddMinomat(minomatDevice);
      GMMJobsLogger.LogDebug(string.Format("New minomat added to GMM: RadioId={0}, GSMId={1}", (object) minomatDevice.MinolID, (object) minomatDevice.GsmID));
    }

    public void CheckMinomats()
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          List<Minomat> list = repositoryFactory.GetRepository<Minomat>().SearchFor((Expression<System.Func<Minomat, bool>>) (x => !x.IsDeactivated && x.IsMaster && x.GsmId != default (string))).ToList<Minomat>();
          IJobRepository jobsRepository = repositoryFactory.GetJobRepository();
          list.ForEach((Action<Minomat>) (minomat => MinomatJobsManager.AddMinomat(minomat)));
          jobsRepository.GetMinomatsWithMissingJobs().ForEach((Action<Minomat>) (minomat =>
          {
            if (minomat.Scenario == null)
              return;
            Guid scenarioId = minomat.Scenario.Id;
            repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<System.Func<ScenarioJobDefinition, bool>>) (x => x.Scenario.Id == scenarioId)).Select<ScenarioJobDefinition, Guid>((System.Func<ScenarioJobDefinition, Guid>) (x => x.JobDefinition.Id)).ToList<Guid>().ForEach((Action<Guid>) (jd =>
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (jobsRepository.Exists((Expression<System.Func<MssReadingJob, bool>>) (x => !x.IsDeactivated && x.EndDate == new DateTime?() && x.Scenario != default (object) && x.Scenario.Id == this.scenarioId && x.JobDefinition != default (object) && x.JobDefinition.Id == jd && x.Minomat.Id == this.CS\u0024\u003C\u003E8__locals3.minomat.Id)))
                return;
              new ManageMSSReadingJobs(repositoryFactory).CreateJob(minomat.Id, jd, scenarioId);
            }));
          }));
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public static void SaveReadingValues(ValueIdentSet e)
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> availableValues = e.AvailableValues;
      Job tag = e.Tag as Job;
      DataTable dt = new DataTable();
      dt.Columns.Add("MeterSerialNumber", typeof (string));
      dt.Columns.Add("JobId", typeof (Guid));
      dt.Columns.Add("CreatedOn", typeof (DateTime));
      dt.Columns.Add("Date", typeof (DateTime));
      dt.Columns.Add("Value", typeof (double));
      dt.Columns.Add("ValueId", typeof (long));
      dt.Columns.Add("StorageInterval", typeof (long));
      dt.Columns.Add("PhysicalQuantity", typeof (long));
      dt.Columns.Add("MeterType", typeof (long));
      dt.Columns.Add("CalculationStart", typeof (long));
      dt.Columns.Add("Creation", typeof (long));
      dt.Columns.Add("Calculation", typeof (long));
      dt.Columns.Add("UnitName", typeof (string));
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in availableValues)
      {
        long key = keyValuePair1.Key;
        foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
        {
          DataRow row = dt.NewRow();
          row["MeterSerialNumber"] = (object) e.SerialNumber;
          row["JobId"] = (object) (tag != null ? tag.JobID : Guid.Empty);
          row["CreatedOn"] = (object) DateTime.Now;
          row["Date"] = (object) keyValuePair2.Key;
          row["Value"] = (object) keyValuePair2.Value.value;
          row["ValueId"] = (object) key;
          row["StorageInterval"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(key);
          row["PhysicalQuantity"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(key);
          row["MeterType"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(key);
          row["CalculationStart"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(key);
          row["Creation"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(key);
          row["Calculation"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(key);
          row["UnitName"] = (object) ValueIdent.GetUnit(Convert.ToInt64(key));
          dt.Rows.Add(row);
        }
      }
      lock (dt)
      {
        string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
        HibernateMultipleDatabasesManager.Initialize(appSetting);
        using (IStatelessSession statelessSession = HibernateMultipleDatabasesManager.DataSessionFactory(appSetting).OpenStatelessSession())
          statelessSession.CreateSQLQuery("exec sp_ProcessAndInsertReadingValues @readingValueFromGMM = :data ").SetStructuredForReadingValues("data", dt).ExecuteUpdate();
      }
      dt.Dispose();
    }
  }
}
