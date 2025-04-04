// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.SaveAddressRecordResponse
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
  [MessageContract(WrapperName = "SaveAddressRecordResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class SaveAddressRecordResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool SaveAddressRecordResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string msg;

    public SaveAddressRecordResponse()
    {
    }

    public SaveAddressRecordResponse(bool SaveAddressRecordResult, string msg)
    {
      this.SaveAddressRecordResult = SaveAddressRecordResult;
      this.msg = msg;
    }
  }
}
