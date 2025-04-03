// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.MeterVPNServer.MeterVPNPort
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

#nullable disable
namespace ZENNER.CommonLibrary.MeterVPNServer
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(Namespace = "urn:MeterVPN", ConfigurationName = "MeterVPNServer.MeterVPNPort")]
  public interface MeterVPNPort
  {
    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, SupportFaults = true, Use = OperationFormatUse.Encoded)]
    [ServiceKnownType(typeof (COMserver))]
    [return: MessageParameter(Name = "COMservers")]
    COMserver[] GetCOMservers(string Login);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [return: MessageParameter(Name = "COMservers")]
    Task<COMserver[]> GetCOMserversAsync(string Login);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, SupportFaults = true, Use = OperationFormatUse.Encoded)]
    [ServiceKnownType(typeof (COMserver))]
    [return: MessageParameter(Name = "Result")]
    string AddCOMserverToCustomer(string Cert, string Name, string Password);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [return: MessageParameter(Name = "Result")]
    Task<string> AddCOMserverToCustomerAsync(string Cert, string Name, string Password);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, SupportFaults = true, Use = OperationFormatUse.Encoded)]
    [ServiceKnownType(typeof (COMserver))]
    [return: MessageParameter(Name = "Result")]
    string DelCOMserverFromCustomer(string Cert);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [return: MessageParameter(Name = "Result")]
    Task<string> DelCOMserverFromCustomerAsync(string Cert);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, SupportFaults = true, Use = OperationFormatUse.Encoded)]
    [ServiceKnownType(typeof (COMserver))]
    [return: MessageParameter(Name = "Result")]
    string ModCOMserver(string Cert, string Name);

    [OperationContract(Action = "urn:MeterVPNAction", ReplyAction = "*")]
    [return: MessageParameter(Name = "Result")]
    Task<string> ModCOMserverAsync(string Cert, string Name);
  }
}
