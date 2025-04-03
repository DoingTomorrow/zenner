// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.ReadingValues.JobReadingValuesManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Common.Library.NHibernate.Data;
using GmmDbLib;
using GmmDbLib.DataSets;
using MSS.Business.Modules.GMM;
using MSS.Business.Utils;
using MSS.Core.Model.Jobs;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ZENNER;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.ReadingValues
{
  public class JobReadingValuesManager
  {
    private List<JobReadingValues> jobReadingValues;

    public JobReadingValuesManager() => this.jobReadingValues = new List<JobReadingValues>();

    public void SaveJobValuesBatchInDb()
    {
      try
      {
        SaveGMMValuesLogger.LogDebug("Start working on batch!");
        string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
        HibernateMultipleDatabasesManager.Initialize(appSetting);
        using (IStatelessSession statelessSession = HibernateMultipleDatabasesManager.DataSessionFactory(appSetting).OpenStatelessSession())
        {
          List<GMMJobValue> list = LinqExtensionMethods.Query<GMMJobValue>(statelessSession).OrderBy<GMMJobValue, DateTime>((Expression<System.Func<GMMJobValue, DateTime>>) (v => v.ReceivedOn)).Take<GMMJobValue>(100).ToList<GMMJobValue>();
          SaveGMMValuesLogger.LogDebug("Get Reading Values from GMM");
          List<Guid> gmmMeterIdsList;
          DataTable readingValuesFromGmm = this.GetReadingValuesFromGMM(list, out gmmMeterIdsList);
          if (readingValuesFromGmm != null && readingValuesFromGmm.Rows.Count > 0)
          {
            SaveGMMValuesLogger.LogDebug("Save Reading Value to MSS");
            statelessSession.CreateSQLQuery("exec sp_ProcessAndInsertReadingValues @readingValueFromGMM = :data ").SetStructuredForReadingValues("data", readingValuesFromGmm).ExecuteUpdate();
            if (gmmMeterIdsList != null && gmmMeterIdsList.Count > 0)
            {
              SaveGMMValuesLogger.LogDebug("Delete reading values from gmm table");
              foreach (Guid meterID in gmmMeterIdsList)
                MeterValuesMSS.DeleteMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterID);
            }
          }
        }
        SaveGMMValuesLogger.LogDebug("Finished working on batch!");
      }
      catch (Exception ex)
      {
        GMMJobsLogger.LogJobError(ex);
      }
    }

    private DataTable GetReadingValuesFromGMM(
      List<GMMJobValue> gmmJobValueFromMssList,
      out List<Guid> gmmMeterIdsList)
    {
      DataTable readingValuesFromGmm = new DataTable();
      gmmMeterIdsList = new List<Guid>();
      foreach (GMMJobValue gmmJobValueFromMss in gmmJobValueFromMssList)
      {
        List<DriverTables.MeterValuesMSSRow> meterValuesMssRowList = new List<DriverTables.MeterValuesMSSRow>();
        List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(GmmInterface.Database.BaseDbConnection, gmmJobValueFromMss.SerialNumber);
        if (meterMss != null && meterMss.Count > 0)
        {
          gmmMeterIdsList.Add(meterMss[0].MeterID);
          List<DriverTables.MeterValuesMSSRow> gmmReadingValues = MeterValuesMSS.LoadMeterValuesMSS(GmmInterface.Database.BaseDbConnection, meterMss[0].MeterID);
          if (gmmReadingValues != null && gmmReadingValues.Count > 0)
          {
            DataTable mssReadingValues = this.ConvertGMMReadingValuesToMssReadingValues(gmmJobValueFromMss, gmmReadingValues);
            if (mssReadingValues != null)
            {
              if (readingValuesFromGmm != null && readingValuesFromGmm.Rows.Count == 0)
              {
                readingValuesFromGmm = mssReadingValues;
              }
              else
              {
                foreach (DataRow row in (InternalDataCollectionBase) mssReadingValues.Rows)
                  readingValuesFromGmm.Rows.Add(row.ItemArray);
              }
            }
          }
          else
            SaveGMMValuesLogger.LogDebug(string.Format("No meter values were found for meter with serial number {0}", (object) gmmJobValueFromMss.SerialNumber));
        }
        else
          SaveGMMValuesLogger.LogDebug(string.Format("Meter with serial number {0} was not found in GMM.", (object) gmmJobValueFromMss.SerialNumber));
      }
      return readingValuesFromGmm;
    }

    private DataTable ConvertGMMReadingValuesToMssReadingValues(
      GMMJobValue gmmJobValueFromMss,
      List<DriverTables.MeterValuesMSSRow> gmmReadingValues)
    {
      DataTable mssReadingValues = new DataTable();
      mssReadingValues.Columns.Add("MeterSerialNumber", typeof (string));
      mssReadingValues.Columns.Add("JobId", typeof (Guid));
      mssReadingValues.Columns.Add("gmmJobValueID", typeof (Guid));
      mssReadingValues.Columns.Add("CreatedOn", typeof (DateTime));
      mssReadingValues.Columns.Add("Date", typeof (DateTime));
      mssReadingValues.Columns.Add("Value", typeof (double));
      mssReadingValues.Columns.Add("ValueId", typeof (long));
      mssReadingValues.Columns.Add("StorageInterval", typeof (long));
      mssReadingValues.Columns.Add("PhysicalQuantity", typeof (long));
      mssReadingValues.Columns.Add("MeterType", typeof (long));
      mssReadingValues.Columns.Add("CalculationStart", typeof (long));
      mssReadingValues.Columns.Add("Creation", typeof (long));
      mssReadingValues.Columns.Add("Calculation", typeof (long));
      mssReadingValues.Columns.Add("UnitName", typeof (string));
      foreach (DriverTables.MeterValuesMSSRow gmmReadingValue in gmmReadingValues)
      {
        DataRow row = mssReadingValues.NewRow();
        row["MeterSerialNumber"] = (object) gmmJobValueFromMss.SerialNumber;
        row["JobId"] = (object) gmmJobValueFromMss.JobId;
        row["gmmJobValueID"] = (object) gmmJobValueFromMss.Id;
        row["CreatedOn"] = (object) DateTime.Now;
        row["Date"] = (object) gmmReadingValue.TimePoint;
        row["Value"] = (object) gmmReadingValue.Value;
        long int64 = Convert.ToInt64(ValueIdent.GetValueIdent(gmmReadingValue.ValueIdentIndex, gmmReadingValue.PhysicalQuantity, gmmReadingValue.MeterType, gmmReadingValue.Calculation, gmmReadingValue.CalculationStart, gmmReadingValue.StorageInterval, gmmReadingValue.Creation));
        row["ValueId"] = (object) int64;
        row["StorageInterval"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(int64);
        row["PhysicalQuantity"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(int64);
        row["MeterType"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(int64);
        row["CalculationStart"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(int64);
        row["Creation"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(int64);
        row["Calculation"] = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(int64);
        row["UnitName"] = (object) ValueIdent.GetUnit(Convert.ToInt64(int64));
        mssReadingValues.Rows.Add(row);
      }
      return mssReadingValues;
    }
  }
}
