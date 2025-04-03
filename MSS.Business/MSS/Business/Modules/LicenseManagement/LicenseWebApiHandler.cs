// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.LicenseWebApiHandler
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Utils;
using MSSWeb.Common.WebApiUtils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Net.Security;

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public class LicenseWebApiHandler
  {
    public static DownloadLicenseResponse DownloadDocument(
      string customerNumber,
      string hardwareKey)
    {
      DownloadLicenseResponse downloadLicenseResponse = new DownloadLicenseResponse();
      try
      {
        ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
        RestClient restClient = new RestClient();
        restClient.Timeout = 5000;
        RestRequest request = new RestRequest(CustomerConfiguration.GetPropertyValue("LicenseWebApi") + "/License/DownloadDocument", Method.POST)
        {
          RequestFormat = DataFormat.Json,
          Timeout = MSS.Business.Utils.AppContext.Current.TechnicalParameters.LicenseWebApiTimeout
        };
        string str = JsonConvert.SerializeObject((object) new DownloadDocumentFullCriteria()
        {
          CustomerNumber = customerNumber,
          HardwareKey = hardwareKey
        });
        request.AddParameter("text/json", (object) str, ParameterType.RequestBody);
        IRestResponse restResponse = restClient.Execute((IRestRequest) request);
        if (restResponse.ErrorException != null)
          throw new BaseApplicationException("MSSError_6", restResponse.ErrorException);
        downloadLicenseResponse.FullLicenseBytes = restResponse.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<byte[]>(restResponse.Content, new JsonSerializerSettings()
        {
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        }) : throw new BaseApplicationException("MSSError_7", new Exception(restResponse.Content));
        downloadLicenseResponse.IsSuccessfullyDownloaded = true;
      }
      catch (Exception ex)
      {
        downloadLicenseResponse.IsSuccessfullyDownloaded = false;
        MessageHandler.LogException(ex);
        throw new BaseApplicationException("MSSError_8", ex);
      }
      return downloadLicenseResponse;
    }

    public static bool CheckIfCustomerExists(string customerNumber)
    {
      try
      {
        ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
        IRestResponse restResponse = new RestClient().Execute((IRestRequest) new RestRequest(CustomerConfiguration.GetPropertyValue("LicenseWebApi") + "/License/CheckIfCustomerExists?customerNumber=" + customerNumber, Method.GET)
        {
          RequestFormat = DataFormat.Json,
          Timeout = MSS.Business.Utils.AppContext.Current.TechnicalParameters.LicenseWebApiTimeout
        });
        if (restResponse.ErrorException != null)
          throw new BaseApplicationException("MSSError_6", restResponse.ErrorException);
        return restResponse.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<bool>(restResponse.Content, new JsonSerializerSettings()
        {
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        }) : throw new BaseApplicationException("MSSError_7", new Exception(restResponse.Content));
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        throw new BaseApplicationException("MSSError_8", ex);
      }
    }

    public static bool UpdateTheApplicationVersionInformation(
      string customerNumber,
      string hardwareKey,
      string applicationVersion)
    {
      try
      {
        ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
        RestClient restClient = new RestClient();
        RestRequest request = new RestRequest(CustomerConfiguration.GetPropertyValue("LicenseWebApi") + "/License/UpdateTheApplicationVersionInformation", Method.POST)
        {
          RequestFormat = DataFormat.Json,
          Timeout = MSS.Business.Utils.AppContext.Current.TechnicalParameters.LicenseWebApiTimeout
        };
        string str = JsonConvert.SerializeObject((object) new ApplicationVersionFullCriteria()
        {
          CustomerNumber = customerNumber,
          HardwareKey = hardwareKey,
          ApplicationVersion = applicationVersion
        });
        request.AddParameter("text/json", (object) str, ParameterType.RequestBody);
        IRestResponse restResponse = restClient.Execute((IRestRequest) request);
        if (restResponse.ErrorException != null)
          throw new BaseApplicationException("MSSError_6", restResponse.ErrorException);
        return restResponse.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<bool>(restResponse.Content, new JsonSerializerSettings()
        {
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        }) : throw new BaseApplicationException("MSSError_7", new Exception(restResponse.Content));
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        throw new BaseApplicationException("MSSError_8", ex);
      }
    }
  }
}
