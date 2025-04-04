// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.NetworkSenderFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Net.Sockets;

#nullable disable
namespace NLog.Internal.NetworkSenders
{
  internal class NetworkSenderFactory : INetworkSenderFactory
  {
    public static readonly INetworkSenderFactory Default = (INetworkSenderFactory) new NetworkSenderFactory();

    public NetworkSender Create(string url, int maxQueueSize)
    {
      if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new HttpNetworkSender(url);
      if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new HttpNetworkSender(url);
      if (url.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new TcpNetworkSender(url, AddressFamily.Unspecified)
        {
          MaxQueueSize = maxQueueSize
        };
      if (url.StartsWith("tcp4://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new TcpNetworkSender(url, AddressFamily.InterNetwork)
        {
          MaxQueueSize = maxQueueSize
        };
      if (url.StartsWith("tcp6://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new TcpNetworkSender(url, AddressFamily.InterNetworkV6)
        {
          MaxQueueSize = maxQueueSize
        };
      if (url.StartsWith("udp://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new UdpNetworkSender(url, AddressFamily.Unspecified);
      if (url.StartsWith("udp4://", StringComparison.OrdinalIgnoreCase))
        return (NetworkSender) new UdpNetworkSender(url, AddressFamily.InterNetwork);
      return url.StartsWith("udp6://", StringComparison.OrdinalIgnoreCase) ? (NetworkSender) new UdpNetworkSender(url, AddressFamily.InterNetworkV6) : throw new ArgumentException("Unrecognized network address", nameof (url));
    }
  }
}
