// Decompiled with JetBrains decompiler
// Type: MinomatListener.Server
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using AsyncCom;
using GmmDbLib;
using GmmDbLib.DataSets;
using MinomatHandler;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Filters;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatListener
{
  public sealed class Server : IDisposable
  {
    internal static Logger logger = LogManager.GetLogger(nameof (Server));
    private LoggingRule loggingRule;
    private List<uint> loggedMinomats;
    private const int MINOL_DEFAULT_PORT = 1080;
    internal const int MAX_PACKET_SIZE = 200000;
    private const int RECEIVE_TIMEOUT = 30000;
    private const int SEND_TIMEOUT = 30000;
    private SortedList<Guid, uint> jobIDsToGsmIDs;
    private SortedList<ulong, ulong> handshakeKeys;

    public SortedList<uint, List<Job>> Jobs { get; private set; }

    public TcpListener Listener { get; set; }

    public int Port { get; set; }

    public bool IsRunning
    {
      get => this.Listener != null && this.Listener.Server != null && this.Listener.Server.IsBound;
    }

    public Server()
    {
      this.Port = 1080;
      this.Jobs = new SortedList<uint, List<Job>>();
      this.jobIDsToGsmIDs = new SortedList<Guid, uint>();
      this.handshakeKeys = new SortedList<ulong, ulong>();
      this.loggedMinomats = new List<uint>();
      FileTarget fileTarget1 = new FileTarget();
      fileTarget1.Name = "MinomatLogs";
      fileTarget1.Layout = (Layout) "${date:format=yy-MM-dd HH\\:mm\\:ss} #${threadid} ${message} ${onexception:${newline}EXCEPTION\\: ${exception:format=ToString} | ${stacktrace}}";
      fileTarget1.FileName = (Layout) "${basedir}/MinomatLogs/${mdc:item=GsmID}.txt";
      fileTarget1.ArchiveFileName = (Layout) "${basedir}/MinomatLogs/${mdc:item=GsmID}.{#}.txt";
      fileTarget1.ArchiveEvery = FileArchivePeriod.Day;
      fileTarget1.ArchiveNumbering = ArchiveNumberingMode.Rolling;
      fileTarget1.MaxArchiveFiles = 7;
      fileTarget1.ArchiveAboveSize = 1000000L;
      fileTarget1.ConcurrentWrites = true;
      FileTarget fileTarget2 = fileTarget1;
      ConditionBasedFilter conditionBasedFilter = new ConditionBasedFilter();
      conditionBasedFilter.Condition = (ConditionExpression) "length('${mdc:item=GsmID}') <= 0";
      conditionBasedFilter.Action = FilterResult.Ignore;
      this.loggingRule = new LoggingRule(nameof (Server), NLog.LogLevel.Trace, (Target) fileTarget2);
      this.loggingRule.Filters.Add((NLog.Filters.Filter) conditionBasedFilter);
    }

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandler<Exception> OnError;

    public event EventHandler<Job> OnJobStarted;

    public event EventHandler<Job> OnJobCompleted;

    public event EventHandler<MinomatDevice> OnMinomatConnected;

    public void Start()
    {
      if (this.IsRunning)
        throw new Exception("The Minomat listener is already running!");
      SortedList<ulong, ulong> sortedList = MinomatList.LoadHandshakeKeys(DbBasis.PrimaryDB.BaseDbConnection);
      if (sortedList != null && sortedList.Count > 0)
      {
        foreach (KeyValuePair<ulong, ulong> keyValuePair in sortedList)
        {
          if (this.handshakeKeys.ContainsKey(keyValuePair.Key))
            this.handshakeKeys[keyValuePair.Key] = keyValuePair.Value;
          else
            this.handshakeKeys.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      this.Listener = new TcpListener(IPAddress.Any, this.Port);
      this.Listener.Start();
      this.BeginAccept();
      Server.logger.Info("Listening on port " + this.Port.ToString());
    }

    public void Stop()
    {
      if (!this.IsRunning)
        return;
      this.Listener.Stop();
      Server.logger.Info("Listener was stopped");
      this.MinomatNLogDisable();
    }

    public void Dispose()
    {
      this.Stop();
      this.jobIDsToGsmIDs.Clear();
      this.handshakeKeys.Clear();
      this.Jobs.Clear();
    }

    public void AddJob(Job job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof (job));
      if (job.System == null)
        throw new InvalidJobException(job, "The job has no collector! " + job?.ToString());
      if (job.Equipment == null)
        throw new InvalidJobException(job, "No equipment defined for job! " + job?.ToString());
      if (job.ProfileType == null)
        throw new InvalidJobException(job, "No profile type defined for job! " + job?.ToString());
      if (job.System.ChangeableParameters == null || job.System.ChangeableParameters.Count == 0)
        throw new InvalidJobException(job, "The changeable parameter of the collector is missed! " + job?.ToString());
      if (!job.System.ChangeableParameters.Exists((Predicate<ChangeableParameter>) (x => x.Key == "MinomatV4_GSM_ID")))
        throw new InvalidJobException(job, "Missed 'MinomatV4_GSM_ID' parameter! " + job?.ToString());
      uint gsmId = Server.GetGsmID(job);
      lock (this.Jobs)
      {
        if (this.Jobs.ContainsKey(gsmId))
          this.Jobs[gsmId].Add(job);
        else
          this.Jobs.Add(gsmId, new List<Job>(1) { job });
        this.jobIDsToGsmIDs.Add(job.JobID, gsmId);
        ulong? encodedHandshakeKey = this.GetEncodedHandshakeKey(job);
        if (encodedHandshakeKey.HasValue)
        {
          uint? challengeKey = Server.TryGetChallengeKey(job);
          ulong? sessionKey = Server.TryGetSessionKey(job);
          if (this.handshakeKeys.ContainsKey(encodedHandshakeKey.Value))
          {
            if ((long) this.handshakeKeys[encodedHandshakeKey.Value] != (long) sessionKey.Value)
              throw new ArgumentException("The listener has already a job with 'challenge key' = " + challengeKey.Value.ToString() + " and 'gsmID' = " + gsmId.ToString() + ". But the 'session key' is different. Existing value: " + this.handshakeKeys[encodedHandshakeKey.Value].ToString() + ", new value: " + sessionKey.ToString());
          }
          else
            this.handshakeKeys.Add(encodedHandshakeKey.Value, sessionKey.Value);
        }
      }
      if (this.IsRunning)
        return;
      this.Start();
    }

    public void RemoveJob(Guid jobID)
    {
      if (jobID == Guid.Empty)
        throw new ArgumentNullException(nameof (jobID));
      this.RemoveJob(this.jobIDsToGsmIDs[jobID], jobID);
    }

    public void RemoveJob(Job job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof (job));
      if (job.System == null)
        throw new InvalidJobException(job, "The job has no collector! " + job?.ToString());
      if (job.System.ChangeableParameters == null || job.System.ChangeableParameters.Count == 0)
        throw new InvalidJobException(job, "The changeable parameter of the collector is missed! " + job?.ToString());
      if (!job.System.ChangeableParameters.Exists((Predicate<ChangeableParameter>) (x => x.Key == "MinomatV4_GSM_ID")))
        throw new InvalidJobException(job, "Missed 'MinomatV4_GSM_ID' parameter! " + job?.ToString());
      this.RemoveJob(Server.GetGsmID(job), job.JobID);
    }

    private void RemoveJob(uint gsmID, Guid jobID)
    {
      lock (this.Jobs)
      {
        if (!this.Jobs.ContainsKey(gsmID))
          throw new ArgumentException("Can not find the job! GsmID: " + gsmID.ToString() + ", JobID: " + jobID.ToString());
        this.jobIDsToGsmIDs.Remove(jobID);
        List<Job> job1 = this.Jobs[gsmID];
        foreach (Job job2 in job1)
        {
          if (!(job2.JobID != jobID))
          {
            ulong? encodedHandshakeKey = this.GetEncodedHandshakeKey(job2);
            if (encodedHandshakeKey.HasValue)
              this.handshakeKeys.Remove(encodedHandshakeKey.Value);
          }
        }
        if (job1.Count == 1)
          this.Jobs.Remove(gsmID);
        else
          job1.RemoveAll((Predicate<Job>) (x => x.JobID == jobID));
      }
      if (this.Jobs.Count != 0)
        return;
      this.Stop();
    }

    private void MinomatNLogEnable(uint gsmID)
    {
      lock (this.loggedMinomats)
      {
        if (this.loggedMinomats.Count == 0)
        {
          if (LogManager.Configuration == null)
            LogManager.Configuration = new LoggingConfiguration();
          LoggingConfiguration configuration = LogManager.Configuration;
          configuration.LoggingRules.Add(this.loggingRule);
          configuration.Reload();
        }
        MappedDiagnosticsContext.Set("GsmID", gsmID.ToString());
        Server.logger.Info("MinomatNLogEnable GsmID: " + gsmID.ToString());
        this.loggedMinomats.Add(gsmID);
      }
    }

    private void MinomatNLogDisable(uint gsmID)
    {
      lock (this.loggedMinomats)
      {
        this.loggedMinomats.Remove(gsmID);
        MappedDiagnosticsContext.Remove("GsmID");
        if (this.loggedMinomats.Count != 0)
          return;
        Server.logger.Info("MinomatNLogDisable: " + gsmID.ToString());
        LogManager.Configuration.LoggingRules.Remove(this.loggingRule);
        LogManager.Configuration.Reload();
      }
    }

    private void MinomatNLogDisable()
    {
      lock (this.loggedMinomats)
      {
        this.loggedMinomats.Clear();
        MappedDiagnosticsContext.Remove("GsmID");
        Server.logger.Info(nameof (MinomatNLogDisable));
        LogManager.Configuration.LoggingRules.Remove(this.loggingRule);
        LogManager.Configuration.Reload();
      }
    }

    private ulong? GetEncodedHandshakeKey(Job job)
    {
      uint? challengeKey = Server.TryGetChallengeKey(job);
      if (!challengeKey.HasValue)
        return new ulong?();
      ulong? sessionKey = Server.TryGetSessionKey(job);
      if (!sessionKey.HasValue)
        return new ulong?();
      uint gsmId = Server.GetGsmID(job);
      return new ulong?(((ulong) challengeKey.Value << 32 | (ulong) gsmId) ^ sessionKey.Value);
    }

    private static uint GetGsmID(Job job)
    {
      uint result;
      if (!uint.TryParse(job.System.ChangeableParameters.First<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_GSM_ID")).Value, out result))
        throw new InvalidJobException(job, "The parameter 'MinomatV4_GSM_ID' has invalid value! " + job?.ToString());
      return result;
    }

    private static ulong? TryGetSessionKey(Job job)
    {
      ulong result;
      return !ulong.TryParse(job.System.ChangeableParameters.First<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_SessionKey")).Value, out result) ? new ulong?() : new ulong?(result);
    }

    private static uint? TryGetChallengeKey(Job job)
    {
      uint result;
      return !uint.TryParse(job.System.ChangeableParameters.First<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_Challenge")).Value, out result) ? new uint?() : new uint?(result);
    }

    private void BeginAccept()
    {
      if (!this.IsRunning)
        return;
      try
      {
        this.Listener.BeginAcceptTcpClient(new AsyncCallback(this.HandleAsyncConnection), (object) this.Listener);
      }
      catch (ObjectDisposedException ex)
      {
        Server.logger.WarnException("Listen canceled.", (Exception) ex);
      }
      catch (Exception ex)
      {
        string message = "Error accepting TCP connection! " + ex.Message;
        Server.logger.FatalException(message, ex);
        if (this.OnError == null)
          return;
        this.OnError((object) this, (Exception) new ServerException(message, ex));
      }
    }

    private void HandleAsyncConnection(IAsyncResult res)
    {
      if (!this.IsRunning)
        return;
      this.BeginAccept();
      try
      {
        using (TcpClient client = this.Listener.EndAcceptTcpClient(res))
        {
          using (NetworkStream stream = client.GetStream())
          {
            Server.logger.Info(client.Client.RemoteEndPoint?.ToString() + " connected");
            client.ReceiveTimeout = 30000;
            client.SendTimeout = 30000;
            MinomatDevice minomatDevice = this.DoHandshake(client, stream);
            if (minomatDevice == null)
              return;
            if (this.OnMinomatConnected != null)
              this.OnMinomatConnected((object) this, minomatDevice);
            if (minomatDevice.IsKnown && minomatDevice.IsTestConnection)
            {
              Server.logger.Info("Test connection");
              Server.CloseSession(client, stream, minomatDevice.ChallengeKey.Value, minomatDevice.SessionKey.Value);
            }
            else if (!minomatDevice.IsKnown)
            {
              Server.logger.Warn(client.Client.RemoteEndPoint?.ToString() + " was connected but we have no keys to read this Master!");
              Server.WriteNAK(client, stream);
            }
            else
            {
              List<Job> activeJobs = this.GetActiveJobs(minomatDevice.GsmID.Value);
              if (activeJobs != null)
              {
                Server.logger.Debug("Server has " + activeJobs.Count.ToString() + " job(s) for Minomat (" + minomatDevice?.ToString() + ")");
                ZR_ClassLibMessages.RegisterThreadErrorMsgList();
                MinomatAsynCom MyCom = new MinomatAsynCom(this, client, stream, minomatDevice);
                MinomatV4 handler = new MinomatV4(new SCGiConnection((IAsyncFunctions) MyCom));
                handler.MaxAttempt = 1;
                handler.Authentication = (SCGiHeaderEx) null;
                handler.Connection.SourceAddress = SCGiAddress.ServerHTTP;
                handler.OnError += new EventHandlerEx<Exception>(this.RaiseOnError);
                try
                {
                  foreach (Job job in activeJobs)
                    this.TryPerformJob(minomatDevice, handler, job);
                }
                finally
                {
                  handler.OnError -= new EventHandlerEx<Exception>(this.RaiseOnError);
                  handler.Dispose();
                  MyCom.GMM_Dispose();
                  ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
                }
              }
              else
                Server.logger.Debug("No jobs available for " + minomatDevice?.ToString());
              Server.CloseSession(client, stream, minomatDevice.ChallengeKey.Value, minomatDevice.SessionKey.Value);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Server.logger.ErrorException(ex.Message, ex);
        if (this.OnError == null)
          return;
        this.OnError((object) this, ex);
      }
      finally
      {
        Server.logger.Info("Connection closed by server.");
      }
    }

    internal void RaiseOnError(object sender, Exception e)
    {
      if (this.OnError == null)
        return;
      this.OnError(sender, e);
    }

    private List<Job> GetActiveJobs(uint gsmID)
    {
      if (!this.Jobs.ContainsKey(gsmID))
        return (List<Job>) null;
      List<Job> job1 = this.Jobs[gsmID];
      if (job1.Count == 0)
        return (List<Job>) null;
      List<Job> jobList = new List<Job>(job1.Count);
      foreach (Job job2 in job1)
      {
        if (job2.Interval == null)
          jobList.Add(job2);
        else if (job2.Interval.RunCheck(DateTime.Now))
        {
          jobList.Add(job2);
          while (job2.Interval.RunCheck(DateTime.Now))
            ;
        }
        else if (Server.logger.IsDebugEnabled)
          Server.logger.Debug("The job " + job2?.ToString() + " should be performed not before " + job2.Interval.GetNextTriggerTime().ToString());
      }
      return jobList.Count > 0 ? jobList : (List<Job>) null;
    }

    private void TryPerformJob(MinomatDevice minomat, MinomatV4 handler, Job job)
    {
      if (this.OnJobStarted != null)
        this.OnJobStarted((object) this, job);
      job.IsInProcess = true;
      if (job.LoggingToFileEnabled)
        this.MinomatNLogEnable(minomat.GsmID.Value);
      try
      {
        if (job.ServiceTask != null)
          this.TryPerformServiceJob(minomat, handler, job);
        else
          this.TryPerformDataJob(minomat, handler, job);
      }
      catch (Exception ex)
      {
        Server.logger.ErrorException("Failed to perform the job! " + ex.Message, ex);
        if (this.OnError != null)
          this.OnError((object) this, ex);
        if (job.Interval == null)
          return;
        Server.logger.Warn("Go back to the last trigger time point.");
        Server.logger.Warn("Old trigger time point: " + job.Interval.GetNextTriggerTime().ToString());
        job.Interval.RevertToLastTriggerTime();
        Server.logger.Warn("New trigger time point: " + job.Interval.GetNextTriggerTime().ToString());
      }
      finally
      {
        job.IsInProcess = false;
        if (this.OnJobCompleted != null)
          this.OnJobCompleted((object) this, job);
        if (job.LoggingToFileEnabled)
          this.MinomatNLogDisable(minomat.GsmID.Value);
      }
    }

    private void TryPerformDataJob(MinomatDevice minomat, MinomatV4 handler, Job job)
    {
      Server.logger.Debug("Perform job to read measurement values. JobID: " + job.JobID.ToString());
      List<uint> ids = new List<uint>();
      if (job.Meters != null && job.Meters.Count > 0)
      {
        foreach (ZENNER.CommonLibrary.Entities.Meter meter in job.Meters)
        {
          uint result;
          if (uint.TryParse(meter.SerialNumber, out result))
            ids.Add(result);
        }
        if (ids.Count != job.Meters.Count && this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidJobException(job, "Invalid job! Not all meters has valid serial number."));
        Server.logger.Debug("Job expected values from " + ids.Count.ToString() + " meter(s):");
      }
      else
      {
        Server.logger.Debug("Job has no meters. Read first all registered meters from the Minomat.");
        List<uint> registeredMessUnits = handler.GetRegisteredMessUnits();
        if (registeredMessUnits == null || registeredMessUnits.Count == 0)
        {
          Server.logger.Warn("Minomat has no registered meters. " + minomat?.ToString());
          return;
        }
        if (job.Filter != null && !job.Filter.Exists((Predicate<long>) (x => ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(x) == ValueIdent.ValueIdPart_MeterType.Any)))
        {
          if (Server.logger.IsTraceEnabled)
          {
            foreach (long valueId in job.Filter)
              Server.logger.Trace("Filter: " + ValueIdent.GetTranslatedValueNameForValueId(valueId, false));
          }
          foreach (uint funkId in registeredMessUnits)
          {
            DeviceTypes typeOfMinolDevice1 = NumberRanges.GetTypeOfMinolDevice((long) funkId);
            if (typeOfMinolDevice1 == DeviceTypes.None)
            {
              Server.logger.Error("Ignore unknown meter type SN: " + funkId.ToString("00000000") + " " + typeOfMinolDevice1.ToString());
            }
            else
            {
              ValueIdent.ValueIdPart_MeterType typeOfMinolDevice2 = NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice((long) funkId);
              if (ValueIdent.Contains(job.Filter, typeOfMinolDevice2))
                ids.Add(funkId);
              else
                Server.logger.Trace("Ignore filtered SN: " + funkId.ToString("00000000") + " " + typeOfMinolDevice1.ToString());
            }
          }
        }
        else
          ids.AddRange((IEnumerable<uint>) registeredMessUnits);
      }
      if (Server.logger.IsTraceEnabled)
      {
        foreach (uint funkId in ids)
          Server.logger.Trace("SN: " + funkId.ToString("00000000") + " " + NumberRanges.GetTypeOfMinolDevice((long) funkId).ToString());
      }
      if (ids.Count == 0)
      {
        Server.logger.Trace("No meters to read!");
      }
      else
      {
        DateTime start = new DateTime(DateTime.Now.Year - 5, 12, 31);
        DateTime now = DateTime.Now;
        if (job.Filter == null || ValueIdent.Contains(job.Filter, ValueIdent.ValueIdPart_StorageInterval.DueDate))
        {
          Server.logger.Debug("Read due date values of " + ids.Count.ToString() + " meter(s)");
          ChangeableParameter changeableParameter = job.System.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "MinomatV4_DurationDueDate"));
          if (changeableParameter != null)
          {
            TimeSpan result;
            if (TimeSpan.TryParse(changeableParameter.Value, out result))
              start = now.Add(result);
            Server.logger.Debug("Duration: " + result.ToString());
          }
          MeasurementSet measurementData = handler.GetMeasurementData(ids, MeasurementDataType.DueDate, start, now, false);
          this.HandleData(minomat, job, measurementData);
        }
        if (job.Filter == null || ValueIdent.Contains(job.Filter, ValueIdent.ValueIdPart_StorageInterval.Month))
        {
          Server.logger.Debug("Read month values of " + ids.Count.ToString() + " meter(s)");
          ChangeableParameter changeableParameter = job.System.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "MinomatV4_DurationMonth"));
          if (changeableParameter != null)
          {
            TimeSpan result;
            if (TimeSpan.TryParse(changeableParameter.Value, out result))
              start = now.Add(result);
            Server.logger.Debug("Duration: " + result.ToString());
          }
          MeasurementSet measurementData = handler.GetMeasurementData(ids, MeasurementDataType.MonthAndHalfMonth, start, now, false);
          this.HandleData(minomat, job, measurementData);
        }
        if (job.Filter == null || ValueIdent.Contains(job.Filter, ValueIdent.ValueIdPart_StorageInterval.Day))
        {
          Server.logger.Debug("Read day values of " + ids.Count.ToString() + " meter(s)");
          ChangeableParameter changeableParameter = job.System.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "MinomatV4_DurationDay"));
          if (changeableParameter != null)
          {
            TimeSpan result;
            if (TimeSpan.TryParse(changeableParameter.Value, out result))
              start = now.Add(result);
            Server.logger.Debug("Duration: " + result.ToString());
          }
          MeasurementSet measurementData = handler.GetMeasurementData(ids, MeasurementDataType.Day, start, now, false);
          this.HandleData(minomat, job, measurementData);
        }
        if (job.Filter != null && !ValueIdent.Contains(job.Filter, ValueIdent.ValueIdPart_StorageInterval.QuarterHour))
          return;
        Server.logger.Debug("Read 15-min values of " + ids.Count.ToString() + " meter(s)");
        ChangeableParameter changeableParameter1 = job.System.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "MinomatV4_DurationQuarterHour"));
        if (changeableParameter1 != null)
        {
          TimeSpan result;
          if (TimeSpan.TryParse(changeableParameter1.Value, out result))
            start = now.Add(result);
          Server.logger.Debug("Duration: " + result.ToString());
        }
        MeasurementSet measurementData1 = handler.GetMeasurementData(ids, MeasurementDataType.Quarter, start, now, false);
        this.HandleData(minomat, job, measurementData1);
      }
    }

    private void HandleData(MinomatDevice minomat, Job job, MeasurementSet set)
    {
      if (set == null)
        return;
      if (!job.StoreResultsToDatabase && this.ValueIdentSetReceived == null)
      {
        Server.logger.Trace("Discard meter values! (StoreResultsToDatabase == false && ValueIdentSetReceived == null)");
      }
      else
      {
        foreach (KeyValuePair<uint, Dictionary<MeasurementDataType, MeasurementData>> keyValuePair in (SortedList<uint, Dictionary<MeasurementDataType, MeasurementData>>) set)
        {
          uint serialnumber = keyValuePair.Key;
          ValueIdentSet e = new ValueIdentSet()
          {
            SerialNumber = serialnumber.ToString()
          };
          e.DeviceType = NumberRanges.GetTypeOfMinolDevice(e.SerialNumber).ToString();
          e.Tag = (object) job;
          ValueIdent.ValueIdPart_MeterType typeOfMinolDevice = NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice((long) serialnumber);
          e.AvailableValues = new SortedList<long, SortedList<DateTime, ReadingValue>>();
          MinomatV4.AddValues(e.AvailableValues, keyValuePair.Value, typeOfMinolDevice, job.Filter, ValueIdent.ValueIdPart_StorageInterval.Day, MeasurementDataType.Day);
          MinomatV4.AddValues(e.AvailableValues, keyValuePair.Value, typeOfMinolDevice, job.Filter, ValueIdent.ValueIdPart_StorageInterval.Month, MeasurementDataType.MonthAndHalfMonth);
          MinomatV4.AddValues(e.AvailableValues, keyValuePair.Value, typeOfMinolDevice, job.Filter, ValueIdent.ValueIdPart_StorageInterval.DueDate, MeasurementDataType.DueDate);
          MinomatV4.AddValues(e.AvailableValues, keyValuePair.Value, typeOfMinolDevice, job.Filter, ValueIdent.ValueIdPart_StorageInterval.QuarterHour, MeasurementDataType.Quarter);
          ValueIdent.CleanUpEmptyValueIdents(e.AvailableValues);
          if (e.AvailableValues.Count != 0)
          {
            if (job.StoreResultsToDatabase)
            {
              try
              {
                List<DriverTables.MeterMSSRow> meterMss = MeterMSS.GetMeterMSS(DbBasis.PrimaryDB.BaseDbConnection, serialnumber.ToString());
                DriverTables.MeterMSSRow meterMssRow = meterMss == null || meterMss.Count != 1 ? (DriverTables.MeterMSSRow) null : meterMss[0];
                if (meterMssRow == null)
                {
                  ZENNER.CommonLibrary.Entities.Meter meter = job.Meters.Find((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == serialnumber.ToString()));
                  meterMssRow = meter == null ? MeterMSS.AddMeterMSS(DbBasis.PrimaryDB.BaseDbConnection, Guid.NewGuid(), serialnumber.ToString()) : MeterMSS.AddMeterMSS(DbBasis.PrimaryDB.BaseDbConnection, meter.ID, serialnumber.ToString());
                }
                if (meterMssRow != null)
                  MeterDatabase.SaveMeterValuesMSS(meterMssRow.MeterID, meterMssRow.SerialNumber, e.AvailableValues);
              }
              catch (Exception ex)
              {
                ZENNER.CommonLibrary.Entities.Meter meter = job.Meters.Find((Predicate<ZENNER.CommonLibrary.Entities.Meter>) (x => x.SerialNumber == serialnumber.ToString()));
                if (meter != null)
                {
                  if (this.OnError != null)
                  {
                    string message = Ot.Gtm(Tg.DB, "FailedStoreMeterValues", "Failed store meter values to database!") + " " + ex.Message;
                    this.OnError((object) this, (Exception) new InvalidMeterException(meter, message));
                  }
                }
                else if (this.OnError != null)
                  this.OnError((object) this, new Exception("Failed store meter values to database! " + ex.Message));
              }
            }
            if (this.ValueIdentSetReceived != null)
              this.ValueIdentSetReceived((object) this, e);
          }
        }
      }
    }

    private void TryPerformServiceJob(MinomatDevice minomat, MinomatV4 handler, Job job)
    {
      if (job.ServiceTask.Method == (MethodInfo) null)
      {
        if (this.OnError == null)
          return;
        this.OnError((object) this, (Exception) new InvalidJobException(job, "Invalid service job! It is missed the method."));
      }
      else
      {
        if (!(typeof (MinomatV4) == job.ServiceTask.Method.DeclaringType))
          return;
        Server.logger.Debug("Start service job '" + job.ServiceTask.Description + "'. JobID: " + job.JobID.ToString());
        List<byte> buffer = new List<byte>();
        CommunicationEventHandler communicationEventHandler = (CommunicationEventHandler) ((sender, e) =>
        {
          if (e == null)
            return;
          foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) e)
            buffer.AddRange((IEnumerable<byte>) scGiPacket.ToByteArray());
        });
        try
        {
          handler.Connection.OnResponse += communicationEventHandler;
          object obj = job.ServiceTask.Method.Invoke((object) handler, (object[]) null);
          if (obj != null)
          {
            if (obj is byte[])
              Server.logger.Trace("The result of service job: " + Util.ByteArrayToHexString((byte[]) obj));
            else
              Server.logger.Trace("The result of service job: " + obj?.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(job.ServiceTask.Method.ReturnType);
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize((TextWriter) stringWriter, obj);
            string resultObject = stringWriter.ToString();
            if (job.StoreResultsToDatabase)
            {
              Server.logger.Debug("Store service job result to database.");
              uint gsmId = Server.GetGsmID(job);
              Thread.Sleep(1000);
              ServiceTaskResult.SaveServiceTaskResult(DbBasis.PrimaryDB.BaseDbConnection, DateTime.Now, gsmId.ToString(), job.JobID, Guid.Empty, job.ServiceTask.Method.ToString(), job.ServiceTask.Method.ReturnType.AssemblyQualifiedName, resultObject, buffer.ToArray());
            }
          }
          else if (this.OnError != null)
            this.OnError((object) this, (Exception) new InvalidJobException(job, "Failed to perform service job!"));
        }
        catch (Exception ex)
        {
          Server.logger.ErrorException("Failed to perform service job! " + ex.Message, ex);
          if (this.OnError != null)
            this.OnError((object) this, (Exception) new InvalidJobException(job, ex));
        }
        finally
        {
          handler.Connection.OnResponse -= communicationEventHandler;
        }
      }
    }

    private static void CloseSession(
      TcpClient client,
      NetworkStream stream,
      uint challenge,
      ulong session)
    {
      Server.WriteREQU(stream, (byte[]) null, challenge, session);
    }

    private MinomatDevice DoHandshake(TcpClient client, NetworkStream stream)
    {
      HttpPacket packet = this.Read(client, stream);
      if (packet == null)
        return (MinomatDevice) null;
      Server.logger.Debug<HttpPacket>(packet);
      if (packet.Type != 0)
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " was connected but the first packet is wrong! Expected: INIT, received: " + packet.Type.ToString();
        Server.logger.Error(message);
        if (packet.Type == HttpPacketType.RESP)
          return this.CreateMinomatDeviceByRESP(packet);
      }
      InitPacket initPacket = InitPacket.TryParse(packet.Content);
      if (initPacket == null)
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " was connected but sends invalid INIT packet!";
        Server.logger.Error(message);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidConnectionException(message, packet.Content));
        return (MinomatDevice) null;
      }
      Server.logger.Debug("Minomat has contacted the server: " + initPacket?.ToString());
      if (this.handshakeKeys.ContainsKey(initPacket.ChallengeAndGsmIDEncoded))
      {
        ulong handshakeKey = this.handshakeKeys[initPacket.ChallengeAndGsmIDEncoded];
        uint challenge = (uint) ((ulong) initPacket.ChallengeEncoded ^ handshakeKey >> 32);
        Server.logger.Debug("Server has already session key: " + handshakeKey.ToString() + " and challenge key: " + challenge.ToString());
        MinomatDevice minomatDevice = this.ReadMinomatDevice(client, stream, initPacket, challenge, handshakeKey);
        if (minomatDevice != null)
        {
          try
          {
            ulong? sessionKey = MinomatList.GetSessionKey(DbBasis.PrimaryDB.BaseDbConnection, minomatDevice.GsmIDEncoded.Value, minomatDevice.ChallengeKeyEncoded.Value);
            if (!sessionKey.HasValue)
            {
              Server.logger.Debug("Store new valid key in our database. " + minomatDevice?.ToString());
              MinomatList.SaveMinomatList(DbBasis.PrimaryDB.BaseDbConnection, minomatDevice.GsmID, minomatDevice.MinolID, minomatDevice.ChallengeKey, minomatDevice.SessionKey, minomatDevice.ChallengeKeyOld, minomatDevice.SessionKeyOld, minomatDevice.GsmIDEncoded, minomatDevice.ChallengeKeyEncoded, minomatDevice.GsmIDEncodedOld, minomatDevice.ChallengeKeyEncodedOld);
            }
            else if ((long) sessionKey.Value != (long) handshakeKey)
            {
              Server.logger.Warn("The key is stored in our database but with wrong session key. Update " + minomatDevice?.ToString());
              MinomatList.SaveMinomatList(DbBasis.PrimaryDB.BaseDbConnection, minomatDevice.GsmID, minomatDevice.MinolID, minomatDevice.ChallengeKey, minomatDevice.SessionKey, minomatDevice.ChallengeKeyOld, minomatDevice.SessionKeyOld, minomatDevice.GsmIDEncoded, minomatDevice.ChallengeKeyEncoded, minomatDevice.GsmIDEncodedOld, minomatDevice.ChallengeKeyEncodedOld);
            }
          }
          catch (Exception ex)
          {
            string message = "Failed to save the keys (challenge and session) in the database! " + minomatDevice?.ToString();
            Server.logger.ErrorException(message, ex);
            if (this.OnError != null)
              this.OnError((object) this, (Exception) new InvalidDBAccessException(message, ex));
          }
        }
        return minomatDevice;
      }
      Server.logger.Warn("Look in the database for correct keys.");
      ulong? sessionKey1 = MinomatList.GetSessionKey(DbBasis.PrimaryDB.BaseDbConnection, initPacket.GsmIDEncoded, initPacket.ChallengeEncoded);
      if (!sessionKey1.HasValue)
      {
        Server.logger.Warn("No keys found in the database. GsmIDEncoded: " + initPacket.GsmIDEncoded.ToString() + " ChallengeEncoded: " + initPacket.ChallengeEncoded.ToString());
        return new MinomatDevice()
        {
          ChallengeKeyEncoded = new uint?(initPacket.ChallengeEncoded),
          GsmIDEncoded = new uint?(initPacket.GsmIDEncoded),
          ConfigNo = new uint?(initPacket.SapConfigNr),
          IsKnown = false,
          IsTestConnection = false,
          MasterFirmwareVersion = initPacket.VersionMasterFirmware,
          MasterModemFirmwareVersion = initPacket.VersionMasterModemFirmware,
          ScenarioNumber = initPacket.ScenarioNumber,
          FirstHttpPacketType = packet.Type.ToString()
        };
      }
      uint challenge1 = (uint) ((ulong) initPacket.ChallengeEncoded ^ sessionKey1.Value >> 32);
      Server.logger.Debug("Found in database session key: " + sessionKey1.ToString() + " and challenge key: " + challenge1.ToString());
      MinomatDevice minomatDevice1 = this.ReadMinomatDevice(client, stream, initPacket, challenge1, sessionKey1.Value);
      if (minomatDevice1 == null)
        Server.logger.Warn("Invalid session key or challenge!");
      return minomatDevice1;
    }

    private MinomatDevice CreateMinomatDeviceByRESP(HttpPacket packet)
    {
      ResponcePacket responcePacket = ResponcePacket.TryParse(packet.Content);
      if (responcePacket == null)
        return (MinomatDevice) null;
      List<DriverTables.MinomatListRow> challengeEncoded = MinomatList.GetMinomatListByChallengeEncoded(DbBasis.PrimaryDB.BaseDbConnection, responcePacket.ChallengeEncoded);
      if (challengeEncoded == null)
        return (MinomatDevice) null;
      if (challengeEncoded.Count > 1 && this.OnError != null)
        this.OnError((object) this, new Exception("Database contains " + challengeEncoded.Count.ToString() + " Minomats for same encoded challenge key 0x" + responcePacket.ChallengeEncoded.ToString("X8")));
      uint? nullable1 = new uint?();
      if (!challengeEncoded[0].IsGsmIDEncodedNull())
        nullable1 = new uint?(uint.Parse(challengeEncoded[0].GsmIDEncoded, NumberStyles.HexNumber));
      uint? nullable2 = new uint?();
      if (!challengeEncoded[0].IsMinolIDNull())
        nullable1 = new uint?(uint.Parse(challengeEncoded[0].MinolID, NumberStyles.HexNumber));
      uint num1 = 0;
      if (!challengeEncoded[0].IsSessionKeyNull())
        num1 = uint.Parse(challengeEncoded[0].SessionKey, NumberStyles.HexNumber);
      uint num2 = 0;
      if (!challengeEncoded[0].IsChallengeKeyNull())
        num2 = uint.Parse(challengeEncoded[0].ChallengeKey, NumberStyles.HexNumber);
      return new MinomatDevice()
      {
        ChallengeKeyEncoded = new uint?(responcePacket.ChallengeEncoded),
        GsmIDEncoded = nullable1,
        IsKnown = true,
        IsTestConnection = false,
        ChallengeKey = new uint?(num2),
        ChallengeKeyEncodedOld = new uint?(responcePacket.ChallengeEncodedOld),
        ChallengeKeyOld = new uint?(),
        GsmID = new uint?(uint.Parse(challengeEncoded[0].GsmID, NumberStyles.HexNumber)),
        GsmIDEncodedOld = new uint?(),
        MinolID = nullable2,
        SessionKey = new ulong?((ulong) num1),
        SessionKeyOld = new ulong?(),
        FirstHttpPacketType = packet.Type.ToString()
      };
    }

    private static bool IsChallengeValid(
      ResponcePacket responcePacket,
      uint challenge,
      ulong session)
    {
      return (int) responcePacket.ChallengeEncoded == ((int) challenge ^ (int) (uint) (session >> 32));
    }

    private static void WriteNAK(TcpClient client, NetworkStream stream)
    {
      Server.logger.Warn("Write NACK to " + client.Client.RemoteEndPoint?.ToString());
      List<byte> byteList = new List<byte>();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Content-Length: 1");
      stringBuilder.AppendLine();
      byteList.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(stringBuilder.ToString()));
      byteList.Add((byte) 1);
      stream.Write(byteList.ToArray(), 0, byteList.Count);
    }

    private static void WriteREQU(
      NetworkStream stream,
      byte[] scgi,
      uint challengeOld,
      ulong sessionOld)
    {
      Server.WriteREQU(stream, scgi, challengeOld, sessionOld, challengeOld, sessionOld);
    }

    private static void WriteREQU(
      NetworkStream stream,
      byte[] scgi,
      uint challengeOld,
      ulong sessionOld,
      uint challengeNew,
      ulong sessionNew)
    {
      byte[] bytes1 = BitConverter.GetBytes(Util.TwoUInt32ToUInt64(challengeOld, challengeNew) ^ sessionNew);
      byte[] bytes2 = BitConverter.GetBytes(sessionOld ^ sessionNew);
      Array.Reverse((Array) bytes1);
      Array.Reverse((Array) bytes2);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 2);
      byteList.AddRange((IEnumerable<byte>) bytes1);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      if (scgi != null)
        byteList.AddRange((IEnumerable<byte>) scgi);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Content-Length: " + byteList.Count.ToString());
      stringBuilder.AppendLine();
      byteList.InsertRange(0, (IEnumerable<byte>) Encoding.ASCII.GetBytes(stringBuilder.ToString()));
      if (scgi == null)
        Server.logger.Info("Send 'end of session' packet.");
      stream.Write(byteList.ToArray(), 0, byteList.Count);
    }

    private HttpPacket Read(TcpClient client, NetworkStream stream)
    {
      System.Net.EndPoint remoteEndPoint = client.Client.RemoteEndPoint;
      if (stream.CanRead)
      {
        byte[] buffer1 = new byte[1024];
        using (MemoryStream memoryStream = new MemoryStream(1024))
        {
          do
          {
            try
            {
              int count = stream.Read(buffer1, 0, buffer1.Length);
              if (count > 0)
                memoryStream.Write(buffer1, 0, count);
              else
                break;
            }
            catch (IOException ex)
            {
              string message = "Failed to read response! " + ex.Message;
              Server.logger.ErrorException(message, (Exception) ex);
              return (HttpPacket) null;
            }
            if (memoryStream.Length > 200000L)
            {
              string message = remoteEndPoint?.ToString() + " sends too match data! Ignore this client.";
              Server.logger.Error(message);
              if (this.OnError != null)
                this.OnError((object) this, (Exception) new InvalidConnectionException(message, memoryStream.ToArray()));
              return (HttpPacket) null;
            }
          }
          while (stream.DataAvailable);
          byte[] array = memoryStream.ToArray();
          if (array.Length == 0)
          {
            string message = remoteEndPoint?.ToString() + " was connected but sends no data! Ignore this client.";
            Server.logger.Error(message);
            if (this.OnError != null)
              this.OnError((object) this, (Exception) new InvalidConnectionException(message));
            return (HttpPacket) null;
          }
          try
          {
            return HttpPacket.TryParse(array);
          }
          catch (HttpPacketIsNotCompleteException ex)
          {
            Server.logger.WarnException(remoteEndPoint?.ToString() + " sends HTTP packet but " + ex.MissedBytes.ToString() + " bytes are missed! Try to read it.", (Exception) ex);
            byte[] buffer2 = new byte[ex.MissedBytes];
            do
            {
              int count = stream.Read(buffer2, 0, buffer2.Length);
              if (count > 0)
                memoryStream.Write(buffer2, 0, count);
              else
                break;
            }
            while (stream.DataAvailable);
          }
          catch (Exception ex)
          {
            string message = remoteEndPoint?.ToString() + " sends invalid HTTP packet! " + ex.Message;
            Server.logger.ErrorException(message, ex);
            if (this.OnError != null)
              this.OnError((object) this, (Exception) new InvalidConnectionException(message, memoryStream.ToArray()));
            return (HttpPacket) null;
          }
          return HttpPacket.TryParse(memoryStream.ToArray());
        }
      }
      else
      {
        string message = remoteEndPoint?.ToString() + " was connected but the stream can not be read!";
        Server.logger.Error(message);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidConnectionException(message));
        return (HttpPacket) null;
      }
    }

    private MinomatDevice ReadMinomatDevice(
      TcpClient client,
      NetworkStream stream,
      InitPacket initPacket,
      uint challenge,
      ulong session)
    {
      Server.logger.Debug("Try to read the MinolID from Minomat");
      Server.WriteREQU(stream, Util.HexStringToByteArray("AA0101080D01040F95FC"), challenge, session);
      HttpPacket httpPacket = this.Read(client, stream);
      if (httpPacket == null)
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " sends no answer of GetMinolID request!";
        Server.logger.Error(message);
        Server.CloseSession(client, stream, challenge, session);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidConnectionException(message));
        return (MinomatDevice) null;
      }
      if (httpPacket.Type == HttpPacketType.NACK)
      {
        Server.logger.Error("Invalid session key or challenge!");
        return (MinomatDevice) null;
      }
      if (httpPacket.Type != HttpPacketType.RESP)
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " sends wrong response of GetMinolID request! Expected: RESP, Actual: " + httpPacket.Type.ToString();
        Server.logger.Error(message);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidResponceException(message, httpPacket.Content));
        return (MinomatDevice) null;
      }
      ResponcePacket responcePacket = ResponcePacket.TryParse(httpPacket.Content);
      if (responcePacket == null)
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " sends invalid response packet of GetMinolID request!";
        Server.logger.Error(message);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidResponceException(message, httpPacket.Content));
        return (MinomatDevice) null;
      }
      if (!Server.IsChallengeValid(responcePacket, challenge, session))
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " answer with invalid challenge key!";
        Server.logger.Error(message);
        Server.WriteNAK(client, stream);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidResponceException(message, httpPacket.Content));
        return (MinomatDevice) null;
      }
      uint num = initPacket.GsmIDEncoded ^ (uint) session;
      Server.logger.Debug("Handshake with Minomat was successful! Session and challenge are correct.");
      uint? nullable;
      try
      {
        nullable = new uint?(BitConverter.ToUInt32(SCGiPacket.Parse(responcePacket.SCGI).Payload, 2));
        Server.logger.Debug("The MinolID is " + nullable.ToString());
      }
      catch (Exception ex)
      {
        string message = client.Client.RemoteEndPoint?.ToString() + " answer with invalid SCGi frame! " + ex.Message;
        Server.logger.ErrorException(message, ex);
        if (this.OnError != null)
          this.OnError((object) this, (Exception) new InvalidResponceException(message, responcePacket.SCGI));
        return (MinomatDevice) null;
      }
      return new MinomatDevice()
      {
        ChallengeKeyEncoded = new uint?(initPacket.ChallengeEncoded),
        GsmIDEncoded = new uint?(initPacket.GsmIDEncoded),
        ConfigNo = new uint?(initPacket.SapConfigNr),
        IsKnown = true,
        IsTestConnection = initPacket.Additional0x21 != null && initPacket.Additional0x21.IsMinolTestRunning,
        MasterFirmwareVersion = initPacket.VersionMasterFirmware,
        MasterModemFirmwareVersion = initPacket.VersionMasterModemFirmware,
        ScenarioNumber = initPacket.ScenarioNumber,
        ChallengeKey = new uint?(challenge),
        ChallengeKeyEncodedOld = new uint?(),
        ChallengeKeyOld = new uint?(),
        GsmID = new uint?(num),
        GsmIDEncodedOld = new uint?(),
        MinolID = nullable,
        SessionKey = new ulong?(session),
        SessionKeyOld = new ulong?()
      };
    }
  }
}
