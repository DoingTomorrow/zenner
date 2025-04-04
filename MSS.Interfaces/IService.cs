// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IService
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Structures;
using MSS.DTO.Sync;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

#nullable disable
namespace MSS.Interfaces
{
  [ServiceContract]
  [ServiceKnownType(typeof (XElement))]
  public interface IService
  {
    [OperationContract]
    string InsertNewClient(string clientId, string userName, Guid userId);

    [OperationContract(Name = "GetClientStatus")]
    string GetClientStatus(string clientId);

    [OperationContract(Name = "GetClientStatusAsync")]
    Task<string> GetClientStatusAsync(string clientId);

    [OperationContract]
    void SaveReadingValues(List<MeterReadingValue> readingValues);

    [OperationContract]
    StructureNodeDTOListsSerializable GetStructures(
      string searchText,
      StructureTypeEnum structureType);

    [OperationContract]
    void TestConnection();

    [OperationContract]
    void SetClientState(Guid[] listOfObjects);

    [OperationContract]
    Dictionary<Guid, SimpleMetadata> GetTemporaryMetadataDictionary();

    [OperationContract]
    Dictionary<string, string> GetSynctonizationExtraInformation(
      List<string> guids,
      List<string> types);

    [OperationContract]
    byte[] GetOrders();

    [OperationContract]
    byte[] GetData(string fullName, object predicate);

    [OperationContract]
    void UpdateEntities(string t, object items, Guid idUser);

    [OperationContract]
    void ReceiveData(string fullName, byte[] items);

    [OperationContract]
    SerializedSyncResponse DownloadFromServer(
      Guid userId,
      DateTime lastSuccessfulDownload,
      List<Guid> existingRootNodes,
      List<Guid> existingOrders,
      bool userMasterPool);

    [OperationContract]
    bool UploadToServer(SerializedSyncResponse changeset);

    [OperationContract]
    bool LockDownloadedEntitiesFromServer(List<Guid> orderToUpdateIdList, Guid lockByUser);

    [OperationContract]
    string GetTimeFromServer();

    [OperationContract]
    bool SaveMinomatOnServer(MinomatSerializableDTO minomatDTO);
  }
}
