// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMCommunication
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using MSS.MDMCommunication.Business.MDMService;

#nullable disable
namespace MSS.MDMCommunication.Business
{
  public class MDMCommunication
  {
    private static void Main() => new Service_MDMS_DataClient().Close();
  }
}
