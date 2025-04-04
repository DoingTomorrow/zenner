// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.WcfLogReceiverOneWayClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace NLog.LogReceiverService
{
  public sealed class WcfLogReceiverOneWayClient : 
    WcfLogReceiverClientBase<ILogReceiverOneWayClient>,
    ILogReceiverOneWayClient
  {
    public WcfLogReceiverOneWayClient()
    {
    }

    public WcfLogReceiverOneWayClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public WcfLogReceiverOneWayClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public WcfLogReceiverOneWayClient(
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public WcfLogReceiverOneWayClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public override IAsyncResult BeginProcessLogMessages(
      NLogEvents events,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Channel.BeginProcessLogMessages(events, callback, asyncState);
    }

    public override void EndProcessLogMessages(IAsyncResult result)
    {
      this.Channel.EndProcessLogMessages(result);
    }
  }
}
