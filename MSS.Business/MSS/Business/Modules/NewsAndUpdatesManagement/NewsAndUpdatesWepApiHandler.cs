// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.NewsAndUpdatesManagement.NewsAndUpdatesWepApiHandler
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Utils;
using MSSWeb.Common.WebApiUtils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

#nullable disable
namespace MSS.Business.Modules.NewsAndUpdatesManagement
{
  public class NewsAndUpdatesWepApiHandler
  {
    public static List<NewsAndUpdatesSerializable> GetNewsAndUpdates(string customerNumber)
    {
      List<NewsAndUpdatesSerializable> updatesSerializableList1 = new List<NewsAndUpdatesSerializable>();
      List<NewsAndUpdatesSerializable> updatesSerializableList2 = new List<NewsAndUpdatesSerializable>();
      List<NewsAndUpdatesSerializable> newsAndUpdates;
      try
      {
        IRestResponse restResponse = new RestClient().Execute((IRestRequest) new RestRequest(CustomerConfiguration.GetPropertyValue("LicenseWebApi") + "/NewsAndUpdates/GetNewsAndUpdates?customerNumber=" + customerNumber, Method.GET)
        {
          RequestFormat = DataFormat.Json,
          Timeout = MSS.Business.Utils.AppContext.Current.TechnicalParameters.LicenseWebApiTimeout
        });
        if (restResponse.ErrorException != null)
          throw new BaseApplicationException("MSSError_6", restResponse.ErrorException);
        newsAndUpdates = restResponse.StatusCode == HttpStatusCode.OK ? new JavaScriptSerializer().Deserialize<List<NewsAndUpdatesSerializable>>(restResponse.Content) : throw new BaseApplicationException("MSSError_7", new Exception(restResponse.Content));
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        throw new BaseApplicationException("MSSError_8", ex);
      }
      return newsAndUpdates;
    }
  }
}
