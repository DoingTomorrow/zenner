// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.SaveReadDataRecordResponse
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
  [MessageContract(WrapperName = "SaveReadDataRecordResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class SaveReadDataRecordResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool SaveReadDataRecordResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string msg;

    public SaveReadDataRecordResponse()
    {
    }

    public SaveReadDataRecordResponse(bool SaveReadDataRecordResult, string msg)
    {
      this.SaveReadDataRecordResult = SaveReadDataRecordResult;
      this.msg = msg;
    }
  }
}
