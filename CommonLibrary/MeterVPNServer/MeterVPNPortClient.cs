// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.MeterVPNServer.MeterVPNPortClient
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

#nullable disable
namespace ZENNER.CommonLibrary.MeterVPNServer
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class MeterVPNPortClient : ClientBase<MeterVPNPort>, MeterVPNPort
  {
    public MeterVPNPortClient()
    {
    }

    public MeterVPNPortClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public MeterVPNPortClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public MeterVPNPortClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public MeterVPNPortClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public COMserver[] GetCOMservers(string Login) => this.Channel.GetCOMservers(Login);

    public Task<COMserver[]> GetCOMserversAsync(string Login)
    {
      return this.Channel.GetCOMserversAsync(Login);
    }

    public string AddCOMserverToCustomer(string Cert, string Name, string Password)
    {
      return this.Channel.AddCOMserverToCustomer(Cert, Name, Password);
    }

    public Task<string> AddCOMserverToCustomerAsync(string Cert, string Name, string Password)
    {
      return this.Channel.AddCOMserverToCustomerAsync(Cert, Name, Password);
    }

    public string DelCOMserverFromCustomer(string Cert)
    {
      return this.Channel.DelCOMserverFromCustomer(Cert);
    }

    public Task<string> DelCOMserverFromCustomerAsync(string Cert)
    {
      return this.Channel.DelCOMserverFromCustomerAsync(Cert);
    }

    public string ModCOMserver(string Cert, string Name) => this.Channel.ModCOMserver(Cert, Name);

    public Task<string> ModCOMserverAsync(string Cert, string Name)
    {
      return this.Channel.ModCOMserverAsync(Cert, Name);
    }
  }
}
