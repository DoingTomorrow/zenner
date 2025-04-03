// Decompiled with JetBrains decompiler
// Type: AsyncCom.MeterVPNServer.MeterVPNService
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using AsyncCom.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

#nullable disable
namespace AsyncCom.MeterVPNServer
{
  [GeneratedCode("System.Web.Services", "4.8.4084.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "MeterVPNBinding", Namespace = "urn:MeterVPN")]
  [SoapInclude(typeof (COMserver))]
  public class MeterVPNService : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetCOMserversOperationCompleted;
    private SendOrPostCallback AddCOMserverToCustomerOperationCompleted;
    private SendOrPostCallback DelCOMserverFromCustomerOperationCompleted;
    private SendOrPostCallback ModCOMserverOperationCompleted;
    private bool useDefaultCredentialsSetExplicitly;

    public MeterVPNService()
    {
      this.Url = Settings.Default.AsyncCom_MeterVPN_MeterVPNService;
      if (this.IsLocalFileSystemWebService(this.Url))
      {
        this.UseDefaultCredentials = true;
        this.useDefaultCredentialsSetExplicitly = false;
      }
      else
        this.useDefaultCredentialsSetExplicitly = true;
    }

    public new string Url
    {
      get => base.Url;
      set
      {
        if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
          base.UseDefaultCredentials = false;
        base.Url = value;
      }
    }

    public new bool UseDefaultCredentials
    {
      get => base.UseDefaultCredentials;
      set
      {
        base.UseDefaultCredentials = value;
        this.useDefaultCredentialsSetExplicitly = true;
      }
    }

    public event GetCOMserversCompletedEventHandler GetCOMserversCompleted;

    public event AddCOMserverToCustomerCompletedEventHandler AddCOMserverToCustomerCompleted;

    public event DelCOMserverFromCustomerCompletedEventHandler DelCOMserverFromCustomerCompleted;

    public event ModCOMserverCompletedEventHandler ModCOMserverCompleted;

    [SoapRpcMethod("urn:MeterVPNAction", RequestNamespace = "urn:MeterVPN", ResponseNamespace = "urn:MeterVPN")]
    [return: SoapElement("COMservers")]
    public COMserver[] GetCOMservers(string Login)
    {
      return (COMserver[]) this.Invoke(nameof (GetCOMservers), new object[1]
      {
        (object) Login
      })[0];
    }

    public void GetCOMserversAsync(string Login) => this.GetCOMserversAsync(Login, (object) null);

    public void GetCOMserversAsync(string Login, object userState)
    {
      if (this.GetCOMserversOperationCompleted == null)
        this.GetCOMserversOperationCompleted = new SendOrPostCallback(this.OnGetCOMserversOperationCompleted);
      this.InvokeAsync("GetCOMservers", new object[1]
      {
        (object) Login
      }, this.GetCOMserversOperationCompleted, userState);
    }

    private void OnGetCOMserversOperationCompleted(object arg)
    {
      if (this.GetCOMserversCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetCOMserversCompleted((object) this, new GetCOMserversCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapRpcMethod("urn:MeterVPNAction", RequestNamespace = "urn:MeterVPN", ResponseNamespace = "urn:MeterVPN")]
    [return: SoapElement("Result")]
    public string AddCOMserverToCustomer(string Cert, string Name, string Password)
    {
      return (string) this.Invoke(nameof (AddCOMserverToCustomer), new object[3]
      {
        (object) Cert,
        (object) Name,
        (object) Password
      })[0];
    }

    public void AddCOMserverToCustomerAsync(string Cert, string Name, string Password)
    {
      this.AddCOMserverToCustomerAsync(Cert, Name, Password, (object) null);
    }

    public void AddCOMserverToCustomerAsync(
      string Cert,
      string Name,
      string Password,
      object userState)
    {
      if (this.AddCOMserverToCustomerOperationCompleted == null)
        this.AddCOMserverToCustomerOperationCompleted = new SendOrPostCallback(this.OnAddCOMserverToCustomerOperationCompleted);
      this.InvokeAsync("AddCOMserverToCustomer", new object[3]
      {
        (object) Cert,
        (object) Name,
        (object) Password
      }, this.AddCOMserverToCustomerOperationCompleted, userState);
    }

    private void OnAddCOMserverToCustomerOperationCompleted(object arg)
    {
      if (this.AddCOMserverToCustomerCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AddCOMserverToCustomerCompleted((object) this, new AddCOMserverToCustomerCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapRpcMethod("urn:MeterVPNAction", RequestNamespace = "urn:MeterVPN", ResponseNamespace = "urn:MeterVPN")]
    [return: SoapElement("Result")]
    public string DelCOMserverFromCustomer(string Cert)
    {
      return (string) this.Invoke(nameof (DelCOMserverFromCustomer), new object[1]
      {
        (object) Cert
      })[0];
    }

    public void DelCOMserverFromCustomerAsync(string Cert)
    {
      this.DelCOMserverFromCustomerAsync(Cert, (object) null);
    }

    public void DelCOMserverFromCustomerAsync(string Cert, object userState)
    {
      if (this.DelCOMserverFromCustomerOperationCompleted == null)
        this.DelCOMserverFromCustomerOperationCompleted = new SendOrPostCallback(this.OnDelCOMserverFromCustomerOperationCompleted);
      this.InvokeAsync("DelCOMserverFromCustomer", new object[1]
      {
        (object) Cert
      }, this.DelCOMserverFromCustomerOperationCompleted, userState);
    }

    private void OnDelCOMserverFromCustomerOperationCompleted(object arg)
    {
      if (this.DelCOMserverFromCustomerCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.DelCOMserverFromCustomerCompleted((object) this, new DelCOMserverFromCustomerCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapRpcMethod("urn:MeterVPNAction", RequestNamespace = "urn:MeterVPN", ResponseNamespace = "urn:MeterVPN")]
    [return: SoapElement("Result")]
    public string ModCOMserver(string Cert, string Name)
    {
      return (string) this.Invoke(nameof (ModCOMserver), new object[2]
      {
        (object) Cert,
        (object) Name
      })[0];
    }

    public void ModCOMserverAsync(string Cert, string Name)
    {
      this.ModCOMserverAsync(Cert, Name, (object) null);
    }

    public void ModCOMserverAsync(string Cert, string Name, object userState)
    {
      if (this.ModCOMserverOperationCompleted == null)
        this.ModCOMserverOperationCompleted = new SendOrPostCallback(this.OnModCOMserverOperationCompleted);
      this.InvokeAsync("ModCOMserver", new object[2]
      {
        (object) Cert,
        (object) Name
      }, this.ModCOMserverOperationCompleted, userState);
    }

    private void OnModCOMserverOperationCompleted(object arg)
    {
      if (this.ModCOMserverCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ModCOMserverCompleted((object) this, new ModCOMserverCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);

    private bool IsLocalFileSystemWebService(string url)
    {
      if (url == null || url == string.Empty)
        return false;
      Uri uri = new Uri(url);
      return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}
