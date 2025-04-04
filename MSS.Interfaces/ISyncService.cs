// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.ISyncService
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using MSS.DTO.Sync;
using System.ServiceModel;

#nullable disable
namespace MSS.Interfaces
{
  [ServiceContract(SessionMode = SessionMode.Required)]
  [ServiceKnownType(typeof (SimpleSyncObject))]
  [ServiceKnownType(typeof (ApplicationSync))]
  [ServiceKnownType(typeof (SyncIdFormatGroup))]
  [ServiceKnownType(typeof (DbSyncContext))]
  [ServiceKnownType(typeof (SyncSchema))]
  [ServiceKnownType(typeof (WebSyncFaultException))]
  [ServiceKnownType(typeof (SyncBatchParameters))]
  [ServiceKnownType(typeof (GetChangesParameters))]
  public interface ISyncService
  {
    [OperationContract(IsInitiating = true)]
    void Initialize(string scopeName);

    [OperationContract]
    DbSyncScopeDescription GetScopeDescription(string databaseEngine);

    [OperationContract]
    void BeginSession(SyncProviderPosition position);

    [OperationContract]
    SyncBatchParameters GetKnowledge();

    [OperationContract]
    GetChangesParameters GetChanges(uint batchSize, SyncKnowledge destinationKnowledge);

    [OperationContract]
    SyncSessionStatistics ApplyChanges(
      ConflictResolutionPolicy resolutionPolicy,
      ChangeBatch sourceChanges,
      object changeData);

    [OperationContract]
    bool HasUploadedBatchFile(string batchFileid, string remotePeerId);

    [OperationContract]
    void UploadBatchFile(string batchFileid, byte[] batchFile, string remotePeerId);

    [OperationContract]
    byte[] DownloadBatchFile(string batchFileId);

    [OperationContract]
    void EndSession();

    [OperationContract(IsTerminating = true)]
    void Cleanup();
  }
}
