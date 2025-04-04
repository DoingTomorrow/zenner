// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.MDMService.IService_MDMS_DataChannel
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace MSS.MDMCommunication.Business.MDMService
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public interface IService_MDMS_DataChannel : 
    IService_MDMS_Data,
    IClientChannel,
    IContextChannel,
    IChannel,
    ICommunicationObject,
    IExtensibleObject<IContextChannel>,
    IDisposable
  {
  }
}
