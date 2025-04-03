// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.WCFRelated.SettingsConnectionManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Utils;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

#nullable disable
namespace MSS.Business.Modules.WCFRelated
{
  public class SettingsConnectionManager
  {
    public static async Task<string> ServerMethodCall(string ip)
    {
      string str;
      using (ServiceClient proxy = new ServiceClient(ip))
      {
        string result = await proxy.GetClientStatusAsync(AppContext.Current.MSSClientId);
        MessageHandler.LogDebug("--------------------------ServerMethodCall----------------------------------" + result);
        str = result;
      }
      return str;
    }

    public bool TestConnectiontoServer(string ip)
    {
      try
      {
        using (ServiceClient serviceClient = new ServiceClient(ip))
          serviceClient.TestConnection();
      }
      catch (BaseApplicationException ex)
      {
        return false;
      }
      return true;
    }

    public bool PingHost(string nameOrAddress)
    {
      PingReply pingReply;
      using (Ping ping = new Ping())
        pingReply = ping.Send(nameOrAddress);
      bool flag = false;
      if (pingReply != null)
        flag = pingReply.Status == IPStatus.Success;
      return flag;
    }

    public bool IsServerAvailableAndStatusAccepted(string ip)
    {
      try
      {
        if (string.IsNullOrEmpty(ip))
          return false;
        bool flag;
        try
        {
          flag = this.PingHost(ip);
        }
        catch (PingException ex)
        {
          return false;
        }
        if (!flag)
          return false;
        using (ServiceClient serviceClient = new ServiceClient(ip))
          return serviceClient.GetClientStatus(AppContext.Current.MSSClientId) == "accepted";
      }
      catch (BaseApplicationException ex)
      {
        return false;
      }
    }
  }
}
