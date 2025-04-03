// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.WCFRelated.SynchronizationServiceClient
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using MSS.Business.Utils;
using MSS.Interfaces;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace MSS.Business.Modules.WCFRelated
{
  public class SynchronizationServiceClient : KnowledgeSyncProvider, IDisposable
  {
    protected SyncIdFormatGroup idFormatGroup;
    protected string scopeName;
    protected DirectoryInfo localBatchingDirectory;
    protected bool disposed = false;
    private ISyncService proxy;
    protected string serviceIP;
    private string batchingDirectory = Environment.ExpandEnvironmentVariables("%TEMP%");

    public string BatchingDirectory
    {
      get => this.batchingDirectory;
      set
      {
        if (string.IsNullOrEmpty(value))
          throw new ArgumentException("value cannot be null or empty");
        try
        {
          Uri uri = new Uri(value);
          if (!uri.IsFile || uri.IsUnc)
            throw new ArgumentException("value must be a local directory");
          this.batchingDirectory = value;
        }
        catch (Exception ex)
        {
          throw new ArgumentException("Invalid batching directory.", ex);
        }
      }
    }

    public SynchronizationServiceClient(string scopeName, string serviceIP)
    {
      this.scopeName = scopeName;
      this.serviceIP = serviceIP;
      this.CreateProxy();
      this.proxy.Initialize(scopeName);
    }

    public override void BeginSession(
      SyncProviderPosition position,
      SyncSessionContext syncSessionContext)
    {
      this.proxy.BeginSession(position);
    }

    public override void EndSession(SyncSessionContext syncSessionContext)
    {
      this.proxy.EndSession();
      if (this.localBatchingDirectory == null)
        return;
      this.localBatchingDirectory.Refresh();
      if (this.localBatchingDirectory.Exists)
        this.localBatchingDirectory.Delete(true);
    }

    public bool IsDataChanged(SyncKnowledge sourceKnowledge)
    {
      DataSet dataSet = (this.proxy.GetChanges(this.proxy.GetKnowledge().BatchSize, sourceKnowledge).DataRetriever as DbSyncContext).DataSet;
      return dataSet.Tables.Count > 0 && dataSet.Tables.Cast<DataTable>().Any<DataTable>((System.Func<DataTable, bool>) (table => table.Rows.Count > 0));
    }

    public override ChangeBatch GetChangeBatch(
      uint batchSize,
      SyncKnowledge destinationKnowledge,
      out object changeDataRetriever)
    {
      GetChangesParameters changes = this.proxy.GetChanges(batchSize, destinationKnowledge);
      changeDataRetriever = changes.DataRetriever;
      if (changeDataRetriever is DbSyncContext dbSyncContext && dbSyncContext.IsDataBatched)
      {
        if (this.localBatchingDirectory == null)
        {
          this.localBatchingDirectory = new DirectoryInfo(Path.Combine(this.batchingDirectory, "WebSync_" + ((object) dbSyncContext.MadeWithKnowledge.ReplicaId).ToString()));
          if (!this.localBatchingDirectory.Exists)
            this.localBatchingDirectory.Create();
        }
        string str = Path.Combine(this.localBatchingDirectory.FullName, dbSyncContext.BatchFileName);
        if (!new FileInfo(str).Exists)
        {
          byte[] buffer = this.proxy.DownloadBatchFile(dbSyncContext.BatchFileName);
          using (FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write))
            fileStream.Write(buffer, 0, buffer.Length);
        }
        dbSyncContext.BatchFileName = str;
      }
      return changes.ChangeBatch;
    }

    public override FullEnumerationChangeBatch GetFullEnumerationChangeBatch(
      uint batchSize,
      SyncId lowerEnumerationBound,
      SyncKnowledge knowledgeForDataRetrieval,
      out object changeDataRetriever)
    {
      throw new NotImplementedException();
    }

    public override void GetSyncBatchParameters(out uint batchSize, out SyncKnowledge knowledge)
    {
      SyncBatchParameters knowledge1 = this.proxy.GetKnowledge();
      batchSize = knowledge1.BatchSize;
      knowledge = knowledge1.DestinationKnowledge;
    }

    public override SyncIdFormatGroup IdFormats
    {
      get
      {
        if (this.idFormatGroup == (SyncIdFormatGroup) null)
        {
          this.idFormatGroup = new SyncIdFormatGroup();
          this.idFormatGroup.ChangeUnitIdFormat.IsVariableLength = false;
          this.idFormatGroup.ChangeUnitIdFormat.Length = (ushort) 1;
          this.idFormatGroup.ReplicaIdFormat.IsVariableLength = false;
          this.idFormatGroup.ReplicaIdFormat.Length = (ushort) 16;
          this.idFormatGroup.ItemIdFormat.IsVariableLength = true;
          this.idFormatGroup.ItemIdFormat.Length = (ushort) 10240;
        }
        return this.idFormatGroup;
      }
    }

    public override void ProcessChangeBatch(
      ConflictResolutionPolicy resolutionPolicy,
      ChangeBatch sourceChanges,
      object changeDataRetriever,
      SyncCallbacks syncCallbacks,
      SyncSessionStatistics sessionStatistics)
    {
      if (changeDataRetriever is DbSyncContext dbSyncContext && dbSyncContext.IsDataBatched)
      {
        string name = new FileInfo(dbSyncContext.BatchFileName).Name;
        string remotePeerId = ((object) dbSyncContext.MadeWithKnowledge.ReplicaId).ToString();
        if (!this.proxy.HasUploadedBatchFile(name, remotePeerId))
        {
          FileStream fileStream = new FileStream(dbSyncContext.BatchFileName, FileMode.Open, FileAccess.Read);
          byte[] numArray = new byte[fileStream.Length];
          using (fileStream)
            fileStream.Read(numArray, 0, numArray.Length);
          this.proxy.UploadBatchFile(name, numArray, remotePeerId);
        }
        dbSyncContext.BatchFileName = name;
      }
      SyncSessionStatistics sessionStatistics1 = this.proxy.ApplyChanges(resolutionPolicy, sourceChanges, changeDataRetriever);
      sessionStatistics.ChangesApplied += sessionStatistics1.ChangesApplied;
      sessionStatistics.ChangesFailed += sessionStatistics1.ChangesFailed;
    }

    public override void ProcessFullEnumerationChangeBatch(
      ConflictResolutionPolicy resolutionPolicy,
      FullEnumerationChangeBatch sourceChanges,
      object changeDataRetriever,
      SyncCallbacks syncCallbacks,
      SyncSessionStatistics sessionStatistics)
    {
    }

    protected string GetEndpointAdress(string ip)
    {
      return string.Format("net.tcp://{0}:13759/MSSSynchronizationService", (object) ip);
    }

    protected void CreateProxy()
    {
      this.proxy = new ChannelFactory<ISyncService>((Binding) MSSHelper.GetNetTcpBinding(), new EndpointAddress(this.GetEndpointAdress(this.serviceIP))).CreateChannel();
      ((IContextChannel) this.proxy).OperationTimeout = new TimeSpan(0, 10, 0);
    }

    public DbSyncScopeDescription GetScopeDescription(string databaseEngine)
    {
      return this.proxy.GetScopeDescription(databaseEngine);
    }

    ~SynchronizationServiceClient()
    {
      try
      {
        this.Dispose(false);
      }
      finally
      {
        // ISSUE: explicit finalizer call
        // ISSUE: explicit non-virtual call
        __nonvirtual (((object) this).Finalize());
      }
    }

    public virtual void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing && this.proxy != null)
        this.CloseProxy();
      this.disposed = true;
    }

    public virtual void CloseProxy()
    {
      if (this.proxy == null)
        return;
      this.proxy.Cleanup();
      if (this.proxy is ICommunicationObject proxy)
      {
        try
        {
          proxy.Close();
        }
        catch (TimeoutException ex)
        {
          proxy.Abort();
        }
        catch (CommunicationException ex)
        {
          proxy.Abort();
        }
      }
      this.proxy = (ISyncService) null;
    }
  }
}
