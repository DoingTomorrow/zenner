// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.Service_MDMS_DataClient
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

#nullable disable
namespace MSS.MDMCommunication.Business.MDMService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class Service_MDMS_DataClient : ClientBase<IService_MDMS_Data>, IService_MDMS_Data
  {
    public Service_MDMS_DataClient()
    {
    }

    public Service_MDMS_DataClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public Service_MDMS_DataClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public Service_MDMS_DataClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public Service_MDMS_DataClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SavePortfolioRecordResponse IService_MDMS_Data.SavePortfolioRecord(
      SavePortfolioRecordRequest request)
    {
      return this.Channel.SavePortfolioRecord(request);
    }

    public bool SavePortfolioRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      DataTable dataTb,
      out string msg)
    {
      SavePortfolioRecordResponse portfolioRecordResponse = ((IService_MDMS_Data) this).SavePortfolioRecord(new SavePortfolioRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        dataTb = dataTb
      });
      msg = portfolioRecordResponse.msg;
      return portfolioRecordResponse.SavePortfolioRecordResult;
    }

    public Task<SavePortfolioRecordResponse> SavePortfolioRecordAsync(
      SavePortfolioRecordRequest request)
    {
      return this.Channel.SavePortfolioRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveBuildingRecordResponse IService_MDMS_Data.SaveBuildingRecord(
      SaveBuildingRecordRequest request)
    {
      return this.Channel.SaveBuildingRecord(request);
    }

    public bool SaveBuildingRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveBuildingRecordResponse buildingRecordResponse = ((IService_MDMS_Data) this).SaveBuildingRecord(new SaveBuildingRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = buildingRecordResponse.msg;
      return buildingRecordResponse.SaveBuildingRecordResult;
    }

    public Task<SaveBuildingRecordResponse> SaveBuildingRecordAsync(
      SaveBuildingRecordRequest request)
    {
      return this.Channel.SaveBuildingRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveTenantInfoRecordResponse IService_MDMS_Data.SaveTenantInfoRecord(
      SaveTenantInfoRecordRequest request)
    {
      return this.Channel.SaveTenantInfoRecord(request);
    }

    public bool SaveTenantInfoRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveTenantInfoRecordResponse infoRecordResponse = ((IService_MDMS_Data) this).SaveTenantInfoRecord(new SaveTenantInfoRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = infoRecordResponse.msg;
      return infoRecordResponse.SaveTenantInfoRecordResult;
    }

    public Task<SaveTenantInfoRecordResponse> SaveTenantInfoRecordAsync(
      SaveTenantInfoRecordRequest request)
    {
      return this.Channel.SaveTenantInfoRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveTenantFlatRecordResponse IService_MDMS_Data.SaveTenantFlatRecord(
      SaveTenantFlatRecordRequest request)
    {
      return this.Channel.SaveTenantFlatRecord(request);
    }

    public bool SaveTenantFlatRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveTenantFlatRecordResponse flatRecordResponse = ((IService_MDMS_Data) this).SaveTenantFlatRecord(new SaveTenantFlatRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = flatRecordResponse.msg;
      return flatRecordResponse.SaveTenantFlatRecordResult;
    }

    public Task<SaveTenantFlatRecordResponse> SaveTenantFlatRecordAsync(
      SaveTenantFlatRecordRequest request)
    {
      return this.Channel.SaveTenantFlatRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveDeviceInfoRecordResponse IService_MDMS_Data.SaveDeviceInfoRecord(
      SaveDeviceInfoRecordRequest request)
    {
      return this.Channel.SaveDeviceInfoRecord(request);
    }

    public bool SaveDeviceInfoRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveDeviceInfoRecordResponse infoRecordResponse = ((IService_MDMS_Data) this).SaveDeviceInfoRecord(new SaveDeviceInfoRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = infoRecordResponse.msg;
      return infoRecordResponse.SaveDeviceInfoRecordResult;
    }

    public Task<SaveDeviceInfoRecordResponse> SaveDeviceInfoRecordAsync(
      SaveDeviceInfoRecordRequest request)
    {
      return this.Channel.SaveDeviceInfoRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveReadDataRecordResponse IService_MDMS_Data.SaveReadDataRecord(
      SaveReadDataRecordRequest request)
    {
      return this.Channel.SaveReadDataRecord(request);
    }

    public bool SaveReadDataRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveReadDataRecordResponse dataRecordResponse = ((IService_MDMS_Data) this).SaveReadDataRecord(new SaveReadDataRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = dataRecordResponse.msg;
      return dataRecordResponse.SaveReadDataRecordResult;
    }

    public Task<SaveReadDataRecordResponse> SaveReadDataRecordAsync(
      SaveReadDataRecordRequest request)
    {
      return this.Channel.SaveReadDataRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveDCUInfoRecordResponse IService_MDMS_Data.SaveDCUInfoRecord(SaveDCUInfoRecordRequest request)
    {
      return this.Channel.SaveDCUInfoRecord(request);
    }

    public bool SaveDCUInfoRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveDCUInfoRecordResponse infoRecordResponse = ((IService_MDMS_Data) this).SaveDCUInfoRecord(new SaveDCUInfoRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = infoRecordResponse.msg;
      return infoRecordResponse.SaveDCUInfoRecordResult;
    }

    public Task<SaveDCUInfoRecordResponse> SaveDCUInfoRecordAsync(SaveDCUInfoRecordRequest request)
    {
      return this.Channel.SaveDCUInfoRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveAddressRecordResponse IService_MDMS_Data.SaveAddressRecord(SaveAddressRecordRequest request)
    {
      return this.Channel.SaveAddressRecord(request);
    }

    public bool SaveAddressRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveAddressRecordResponse addressRecordResponse = ((IService_MDMS_Data) this).SaveAddressRecord(new SaveAddressRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = addressRecordResponse.msg;
      return addressRecordResponse.SaveAddressRecordResult;
    }

    public Task<SaveAddressRecordResponse> SaveAddressRecordAsync(SaveAddressRecordRequest request)
    {
      return this.Channel.SaveAddressRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveDCUConnectionRecordResponse IService_MDMS_Data.SaveDCUConnectionRecord(
      SaveDCUConnectionRecordRequest request)
    {
      return this.Channel.SaveDCUConnectionRecord(request);
    }

    public bool SaveDCUConnectionRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveDCUConnectionRecordResponse connectionRecordResponse = ((IService_MDMS_Data) this).SaveDCUConnectionRecord(new SaveDCUConnectionRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = connectionRecordResponse.msg;
      return connectionRecordResponse.SaveDCUConnectionRecordResult;
    }

    public Task<SaveDCUConnectionRecordResponse> SaveDCUConnectionRecordAsync(
      SaveDCUConnectionRecordRequest request)
    {
      return this.Channel.SaveDCUConnectionRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveAMRRouteRecordResponse IService_MDMS_Data.SaveAMRRouteRecord(
      SaveAMRRouteRecordRequest request)
    {
      return this.Channel.SaveAMRRouteRecord(request);
    }

    public bool SaveAMRRouteRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveAMRRouteRecordResponse routeRecordResponse = ((IService_MDMS_Data) this).SaveAMRRouteRecord(new SaveAMRRouteRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = routeRecordResponse.msg;
      return routeRecordResponse.SaveAMRRouteRecordResult;
    }

    public Task<SaveAMRRouteRecordResponse> SaveAMRRouteRecordAsync(
      SaveAMRRouteRecordRequest request)
    {
      return this.Channel.SaveAMRRouteRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveTestconfigRunRecordResponse IService_MDMS_Data.SaveTestconfigRunRecord(
      SaveTestconfigRunRecordRequest request)
    {
      return this.Channel.SaveTestconfigRunRecord(request);
    }

    public bool SaveTestconfigRunRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveTestconfigRunRecordResponse runRecordResponse = ((IService_MDMS_Data) this).SaveTestconfigRunRecord(new SaveTestconfigRunRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = runRecordResponse.msg;
      return runRecordResponse.SaveTestconfigRunRecordResult;
    }

    public Task<SaveTestconfigRunRecordResponse> SaveTestconfigRunRecordAsync(
      SaveTestconfigRunRecordRequest request)
    {
      return this.Channel.SaveTestconfigRunRecordAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    SaveTestconfigDeviceRecordResponse IService_MDMS_Data.SaveTestconfigDeviceRecord(
      SaveTestconfigDeviceRecordRequest request)
    {
      return this.Channel.SaveTestconfigDeviceRecord(request);
    }

    public bool SaveTestconfigDeviceRecord(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb,
      out string msg)
    {
      SaveTestconfigDeviceRecordResponse deviceRecordResponse = ((IService_MDMS_Data) this).SaveTestconfigDeviceRecord(new SaveTestconfigDeviceRecordRequest()
      {
        mdms_user = mdms_user,
        password = password,
        enterprise_id = enterprise_id,
        portforlio_id = portforlio_id,
        dataTb = dataTb
      });
      msg = deviceRecordResponse.msg;
      return deviceRecordResponse.SaveTestconfigDeviceRecordResult;
    }

    public Task<SaveTestconfigDeviceRecordResponse> SaveTestconfigDeviceRecordAsync(
      SaveTestconfigDeviceRecordRequest request)
    {
      return this.Channel.SaveTestconfigDeviceRecordAsync(request);
    }
  }
}
