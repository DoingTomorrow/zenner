// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.ILogReceiverTwoWayClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.ServiceModel;

#nullable disable
namespace NLog.LogReceiverService
{
  [ServiceContract(Namespace = "http://nlog-project.org/ws/", ConfigurationName = "NLog.LogReceiverService.ILogReceiverClient")]
  public interface ILogReceiverTwoWayClient
  {
    [OperationContract(AsyncPattern = true, Action = "http://nlog-project.org/ws/ILogReceiverServer/ProcessLogMessages", ReplyAction = "http://nlog-project.org/ws/ILogReceiverServer/ProcessLogMessagesResponse")]
    IAsyncResult BeginProcessLogMessages(
      NLogEvents events,
      AsyncCallback callback,
      object asyncState);

    void EndProcessLogMessages(IAsyncResult result);
  }
}
