// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.IWcfLogReceiverClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.ComponentModel;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;

#nullable disable
namespace NLog.LogReceiverService
{
  public interface IWcfLogReceiverClient : ICommunicationObject
  {
    event EventHandler<AsyncCompletedEventArgs> ProcessLogMessagesCompleted;

    event EventHandler<AsyncCompletedEventArgs> OpenCompleted;

    event EventHandler<AsyncCompletedEventArgs> CloseCompleted;

    ClientCredentials ClientCredentials { get; }

    IClientChannel InnerChannel { get; }

    ServiceEndpoint Endpoint { get; }

    void OpenAsync();

    void OpenAsync(object userState);

    void CloseAsync();

    void CloseAsync(object userState);

    void ProcessLogMessagesAsync(NLogEvents events);

    void ProcessLogMessagesAsync(NLogEvents events, object userState);

    IAsyncResult BeginProcessLogMessages(
      NLogEvents events,
      AsyncCallback callback,
      object asyncState);

    void EndProcessLogMessages(IAsyncResult result);

    void DisplayInitializationUI();

    CookieContainer CookieContainer { get; set; }
  }
}
