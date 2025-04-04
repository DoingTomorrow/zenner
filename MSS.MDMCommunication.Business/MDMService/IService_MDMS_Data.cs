// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.IService_MDMS_Data
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

#nullable disable
namespace MSS.MDMCommunication.Business.MDMService
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "MDMService.IService_MDMS_Data")]
  public interface IService_MDMS_Data
  {
    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SavePortfolioRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SavePortfolioRecordResponse")]
    SavePortfolioRecordResponse SavePortfolioRecord(SavePortfolioRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SavePortfolioRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SavePortfolioRecordResponse")]
    Task<SavePortfolioRecordResponse> SavePortfolioRecordAsync(SavePortfolioRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveBuildingRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveBuildingRecordResponse")]
    SaveBuildingRecordResponse SaveBuildingRecord(SaveBuildingRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveBuildingRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveBuildingRecordResponse")]
    Task<SaveBuildingRecordResponse> SaveBuildingRecordAsync(SaveBuildingRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTenantInfoRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTenantInfoRecordResponse")]
    SaveTenantInfoRecordResponse SaveTenantInfoRecord(SaveTenantInfoRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTenantInfoRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTenantInfoRecordResponse")]
    Task<SaveTenantInfoRecordResponse> SaveTenantInfoRecordAsync(SaveTenantInfoRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTenantFlatRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTenantFlatRecordResponse")]
    SaveTenantFlatRecordResponse SaveTenantFlatRecord(SaveTenantFlatRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTenantFlatRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTenantFlatRecordResponse")]
    Task<SaveTenantFlatRecordResponse> SaveTenantFlatRecordAsync(SaveTenantFlatRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveDeviceInfoRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveDeviceInfoRecordResponse")]
    SaveDeviceInfoRecordResponse SaveDeviceInfoRecord(SaveDeviceInfoRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveDeviceInfoRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveDeviceInfoRecordResponse")]
    Task<SaveDeviceInfoRecordResponse> SaveDeviceInfoRecordAsync(SaveDeviceInfoRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveReadDataRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveReadDataRecordResponse")]
    SaveReadDataRecordResponse SaveReadDataRecord(SaveReadDataRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveReadDataRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveReadDataRecordResponse")]
    Task<SaveReadDataRecordResponse> SaveReadDataRecordAsync(SaveReadDataRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveDCUInfoRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveDCUInfoRecordResponse")]
    SaveDCUInfoRecordResponse SaveDCUInfoRecord(SaveDCUInfoRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveDCUInfoRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveDCUInfoRecordResponse")]
    Task<SaveDCUInfoRecordResponse> SaveDCUInfoRecordAsync(SaveDCUInfoRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveAddressRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveAddressRecordResponse")]
    SaveAddressRecordResponse SaveAddressRecord(SaveAddressRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveAddressRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveAddressRecordResponse")]
    Task<SaveAddressRecordResponse> SaveAddressRecordAsync(SaveAddressRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveDCUConnectionRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveDCUConnectionRecordResponse")]
    SaveDCUConnectionRecordResponse SaveDCUConnectionRecord(SaveDCUConnectionRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveDCUConnectionRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveDCUConnectionRecordResponse")]
    Task<SaveDCUConnectionRecordResponse> SaveDCUConnectionRecordAsync(
      SaveDCUConnectionRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveAMRRouteRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveAMRRouteRecordResponse")]
    SaveAMRRouteRecordResponse SaveAMRRouteRecord(SaveAMRRouteRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveAMRRouteRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveAMRRouteRecordResponse")]
    Task<SaveAMRRouteRecordResponse> SaveAMRRouteRecordAsync(SaveAMRRouteRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigRunRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigRunRecordResponse")]
    SaveTestconfigRunRecordResponse SaveTestconfigRunRecord(SaveTestconfigRunRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigRunRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigRunRecordResponse")]
    Task<SaveTestconfigRunRecordResponse> SaveTestconfigRunRecordAsync(
      SaveTestconfigRunRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigDeviceRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigDeviceRecordResponse")]
    SaveTestconfigDeviceRecordResponse SaveTestconfigDeviceRecord(
      SaveTestconfigDeviceRecordRequest request);

    [OperationContract(Action = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigDeviceRecord", ReplyAction = "http://tempuri.org/IService_MDMS_Data/SaveTestconfigDeviceRecordResponse")]
    Task<SaveTestconfigDeviceRecordResponse> SaveTestconfigDeviceRecordAsync(
      SaveTestconfigDeviceRecordRequest request);
  }
}
