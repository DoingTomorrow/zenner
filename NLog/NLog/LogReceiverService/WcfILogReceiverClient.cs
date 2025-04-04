// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.WcfILogReceiverClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace NLog.LogReceiverService
{
  [Obsolete("Use WcfLogReceiverOneWayClient class instead. Marked obsolete before v4.3.11 and it may be removed in a future release.")]
  public sealed class WcfILogReceiverClient : 
    WcfLogReceiverClientBase<ILogReceiverClient>,
    ILogReceiverClient
  {
    public WcfILogReceiverClient()
    {
    }

    public WcfILogReceiverClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public WcfILogReceiverClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public WcfILogReceiverClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public WcfILogReceiverClient(Binding binding, EndpointAddress remoteAddress)
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
