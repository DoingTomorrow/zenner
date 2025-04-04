// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.SaveDeviceInfoRecordResponse
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

#nullable disable
namespace MSS.MDMCommunication.Business.MDMService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "SaveDeviceInfoRecordResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class SaveDeviceInfoRecordResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool SaveDeviceInfoRecordResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string msg;

    public SaveDeviceInfoRecordResponse()
    {
    }

    public SaveDeviceInfoRecordResponse(bool SaveDeviceInfoRecordResult, string msg)
    {
      this.SaveDeviceInfoRecordResult = SaveDeviceInfoRecordResult;
      this.msg = msg;
    }
  }
}
