// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.SynchronizationHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using MSS.Business.Errors;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.DTO.Sync;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

#nullable disable
namespace MSS.Business.Modules.Synchronization
{
  public class SynchronizationHelper : SynchronizationHelperBase
  {
    public override SynchronizationResults SynchronizeScope(
      SyncScopesEnum syncScope,
      SyncDirectionOrder syncDirection)
    {
      RelationalSyncProvider localProvider = (RelationalSyncProvider) null;
      SynchronizationServiceClient remoteProvider = (SynchronizationServiceClient) null;
      switch (syncDirection)
      {
        case SyncDirectionOrder.Upload:
          localProvider = this.GetClientProvider(syncScope);
          remoteProvider = new SynchronizationServiceClient(syncScope.ToString(), MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
          break;
        case SyncDirectionOrder.Download:
          localProvider = this.GetClientProvider(syncScope);
          remoteProvider = new SynchronizationServiceClient(syncScope.ToString(), MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
          break;
      }
      SynchronizationResults synchronizationResults = new SynchronizationResults(syncScope.ToString(), this.SynchronizeProviders((KnowledgeSyncProvider) localProvider, (KnowledgeSyncProvider) remoteProvider, syncDirection));
      remoteProvider?.Dispose();
      localProvider?.Dispose();
      return synchronizationResults;
    }

    protected override bool IsVersionUpToDateDownload(SyncScopesEnum syncScope)
    {
      RelationalSyncProvider clientProvider = this.GetClientProvider(syncScope);
      SynchronizationServiceClient synchronizationServiceClient = new SynchronizationServiceClient(syncScope.ToString(), MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
      SyncBatchParameters syncBatchParameters = new SyncBatchParameters();
      SyncSessionContext syncSessionContext = new SyncSessionContext(new SyncIdFormatGroup(), (SyncCallbacks) null);
      clientProvider.BeginSession(SyncProviderPosition.Local, syncSessionContext);
      synchronizationServiceClient.BeginSession(SyncProviderPosition.Remote, syncSessionContext);
      clientProvider.GetSyncBatchParameters(out syncBatchParameters.BatchSize, out syncBatchParameters.DestinationKnowledge);
      bool dateDownload = !synchronizationServiceClient.IsDataChanged(syncBatchParameters.DestinationKnowledge);
      clientProvider.EndSession(syncSessionContext);
      synchronizationServiceClient.EndSession(syncSessionContext);
      synchronizationServiceClient.Dispose();
      return dateDownload;
    }

    protected override bool IsVersionUpToDateSend(SyncScopesEnum syncScope)
    {
      RelationalSyncProvider clientProvider = this.GetClientProvider(syncScope);
      SynchronizationServiceClient synchronizationServiceClient = new SynchronizationServiceClient(syncScope.ToString(), MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
      SyncBatchParameters syncBatchParameters = new SyncBatchParameters();
      SyncSessionContext syncSessionContext = new SyncSessionContext(new SyncIdFormatGroup(), (SyncCallbacks) null);
      clientProvider.BeginSession(SyncProviderPosition.Local, syncSessionContext);
      synchronizationServiceClient.BeginSession(SyncProviderPosition.Remote, syncSessionContext);
      synchronizationServiceClient.GetSyncBatchParameters(out syncBatchParameters.BatchSize, out syncBatchParameters.DestinationKnowledge);
      bool dateSend = !this.IsDataChanged(syncBatchParameters.DestinationKnowledge, (KnowledgeSyncProvider) clientProvider);
      clientProvider.EndSession(syncSessionContext);
      synchronizationServiceClient.EndSession(syncSessionContext);
      synchronizationServiceClient.Dispose();
      return dateSend;
    }

    private bool IsDataChanged(
      SyncKnowledge destinationKnowledge,
      KnowledgeSyncProvider localProvider)
    {
      SyncBatchParameters syncBatchParameters = new SyncBatchParameters();
      localProvider.GetSyncBatchParameters(out syncBatchParameters.BatchSize, out syncBatchParameters.DestinationKnowledge);
      object changeDataRetriever;
      localProvider.GetChangeBatch(syncBatchParameters.BatchSize, destinationKnowledge, out changeDataRetriever);
      if (changeDataRetriever is DbSyncContext dbSyncContext)
      {
        DataSet dataSet = dbSyncContext.DataSet;
        if (dataSet.Tables.Count > 0)
          return dataSet.Tables.Cast<DataTable>().Any<DataTable>((System.Func<DataTable, bool>) (table => table.Rows.Count > 0));
      }
      return false;
    }

    public string ListToXML(IEnumerable<SimpleSyncObject> filteredList)
    {
      StringWriter w = new StringWriter();
      XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter) w);
      xmlTextWriter.WriteStartElement("items");
      foreach (SimpleSyncObject filtered in filteredList)
      {
        xmlTextWriter.WriteStartElement("item");
        xmlTextWriter.WriteAttributeString("Id", filtered.Id.ToString());
        xmlTextWriter.WriteEndElement();
      }
      xmlTextWriter.WriteFullEndElement();
      xmlTextWriter.Flush();
      xmlTextWriter.Close();
      return w.ToString();
    }

    public RelationalSyncProvider GetClientProvider(SyncScopesEnum scopeName)
    {
      return this.GetProvider(scopeName);
    }

    public RelationalSyncProvider GetServerProvider(string scopeName)
    {
      return this.GetProvider((SyncScopesEnum) Enum.Parse(typeof (SyncScopesEnum), scopeName));
    }

    private RelationalSyncProvider GetProvider(SyncScopesEnum scopeName)
    {
      string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
      string propertyValue = NHibernateConfigurationHelper.GetPropertyValue(appSetting, "connection.connection_string");
      RelationalSyncProvider provider = (RelationalSyncProvider) null;
      if (appSetting == DatabaseEngineEnum.MSSQLDatabase.ToString())
      {
        SqlSyncProvider sqlSyncProvider = new SqlSyncProvider(scopeName.ToString(), new SqlConnection(propertyValue));
        sqlSyncProvider.ScopeName = scopeName.ToString();
        sqlSyncProvider.MemoryDataCacheSize = 10000U;
        sqlSyncProvider.BatchingDirectory = PathsHelper.GetTempFolderPath();
        sqlSyncProvider.ApplicationTransactionSize = 5000L;
        sqlSyncProvider.CommandTimeout = 120;
        provider = (RelationalSyncProvider) sqlSyncProvider;
      }
      if (!(appSetting == DatabaseEngineEnum.SQLiteDatabase.ToString()))
        ;
      provider.ApplyChangeFailed += new EventHandler<DbApplyChangeFailedEventArgs>(this.syncProvider_ApplyChangeFailed);
      return provider;
    }

    private void syncProvider_ApplyChangeFailed(object sender, DbApplyChangeFailedEventArgs e)
    {
      switch (e.Conflict.Type)
      {
        case DbConflictType.ErrorsOccurred:
          MessageHandler.LogException(e.Error);
          break;
        case DbConflictType.LocalUpdateRemoteUpdate:
          e.Action = ApplyAction.Continue;
          break;
        case DbConflictType.LocalUpdateRemoteDelete:
          e.Action = ApplyAction.Continue;
          break;
        case DbConflictType.LocalDeleteRemoteUpdate:
          e.Action = ApplyAction.Continue;
          break;
        case DbConflictType.LocalInsertRemoteInsert:
          int num = (int) MessageBox.Show("LocalInsertRemoteInsert");
          break;
        case DbConflictType.LocalDeleteRemoteDelete:
          e.Action = ApplyAction.Continue;
          break;
        case DbConflictType.LocalCleanedupDeleteRemoteUpdate:
          e.Action = ApplyAction.Continue;
          break;
      }
    }

    private SyncOperationStatistics SynchronizeProviders(
      KnowledgeSyncProvider localProvider,
      KnowledgeSyncProvider remoteProvider,
      SyncDirectionOrder direction)
    {
      return new SyncOrchestrator()
      {
        LocalProvider = ((SyncProvider) localProvider),
        RemoteProvider = ((SyncProvider) remoteProvider),
        Direction = direction
      }.Synchronize();
    }
  }
}
