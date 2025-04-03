// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.GMMJobsLogger
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NLog;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public static class GMMJobsLogger
  {
    public static Logger GetLogger() => LogManager.GetCurrentClassLogger();

    public static void WriteMessageToLogger(this Logger logger, NLog.LogLevel logLevel, string message)
    {
      logger.Log(logLevel, message);
    }

    public static void LogJobStarted(Job job)
    {
      GMMJobsLogger.GetLogger().Info<Guid>("Job started: Id={0}", job.JobID);
    }

    public static void LogJobCompleted(Job job)
    {
      GMMJobsLogger.GetLogger().Info<Guid>("Job completed: Id={0}", job.JobID);
    }

    public static void LogJobError(Exception ex)
    {
      GMMJobsLogger.GetLogger().LogException(NLog.LogLevel.Error, ex.Message, ex);
    }

    public static void LogGMMError(Exception ex)
    {
      GMMJobsLogger.GetLogger().LogException(NLog.LogLevel.Error, "GMM error: " + ex.Message, ex);
    }

    public static void LogJobValuesReceived(ValueIdentSet e)
    {
      if (e.Tag is Job tag)
        GMMJobsLogger.GetLogger().Info<Guid>("Job values received for JobId = {0};", tag.JobID);
      GMMJobsLogger.GetLogger().Info<string, string>("Reading values: SerialNumber={0}; Available values: {1}", e.SerialNumber, ValueIdent.ToString(e.AvailableValues, (Dictionary<long, Type>) null));
    }

    public static void LogMinomatConnected(MinomatDevice minomat)
    {
      Logger logger = GMMJobsLogger.GetLogger();
      object[] objArray = new object[6];
      uint? nullable = minomat.GsmID;
      objArray[0] = (object) (uint) ((int) nullable ?? 0);
      nullable = minomat.MinolID;
      objArray[1] = (object) (uint) ((int) nullable ?? 0);
      nullable = minomat.ScenarioNumber;
      objArray[2] = (object) (uint) ((int) nullable ?? 0);
      nullable = minomat.ChallengeKey;
      objArray[3] = (object) (uint) ((int) nullable ?? 0);
      objArray[4] = (object) (ulong) ((long) minomat.SessionKey ?? 0L);
      objArray[5] = (object) minomat.IsKnown;
      logger.Info("Minomat connected:GSMId={0}, MinolId={1}, SerialNumber={2}, ChallengeKey={3}, SessionKey={4}, IsKnown={5}", objArray);
    }

    public static void LogDebug(string message) => GMMJobsLogger.GetLogger().Debug(message);
  }
}
