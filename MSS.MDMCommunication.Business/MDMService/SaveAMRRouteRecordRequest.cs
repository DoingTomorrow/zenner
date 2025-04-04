// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.SaveAMRRouteRecordRequest
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;

#nullable disable
namespace MSS.MDMCommunication.Business.MDMService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "SaveAMRRouteRecord", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class SaveAMRRouteRecordRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string mdms_user;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string password;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string enterprise_id;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string portforlio_id;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public DataTable dataTb;

    public SaveAMRRouteRecordRequest()
    {
    }

    public SaveAMRRouteRecordRequest(
      string mdms_user,
      string password,
      string enterprise_id,
      string portforlio_id,
      DataTable dataTb)
    {
      this.mdms_user = mdms_user;
      this.password = password;
      this.enterprise_id = enterprise_id;
      this.portforlio_id = portforlio_id;
      this.dataTb = dataTb;
    }
  }
}
