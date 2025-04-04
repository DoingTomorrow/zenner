// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.NetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#nullable disable
namespace NLog.Internal.NetworkSenders
{
  internal abstract class NetworkSender : IDisposable
  {
    private static int currentSendTime;

    protected NetworkSender(string url)
    {
      this.Address = url;
      this.LastSendTime = Interlocked.Increment(ref NetworkSender.currentSendTime);
    }

    public string Address { get; private set; }

    public int LastSendTime { get; private set; }

    public void Initialize() => this.DoInitialize();

    public void Close(AsyncContinuation continuation) => this.DoClose(continuation);

    public void FlushAsync(AsyncContinuation continuation) => this.DoFlush(continuation);

    public void Send(byte[] bytes, int offset, int length, AsyncContinuation asyncContinuation)
    {
      this.LastSendTime = Interlocked.Increment(ref NetworkSender.currentSendTime);
      this.DoSend(bytes, offset, length, asyncContinuation);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void DoInitialize()
    {
    }

    protected virtual void DoClose(AsyncContinuation continuation)
    {
      continuation((Exception) null);
    }

    protected virtual void DoFlush(AsyncContinuation continuation)
    {
      continuation((Exception) null);
    }

    protected abstract void DoSend(
      byte[] bytes,
      int offset,
      int length,
      AsyncContinuation asyncContinuation);

    protected virtual EndPoint ParseEndpointAddress(Uri uri, AddressFamily addressFamily)
    {
      switch (uri.HostNameType)
      {
        case UriHostNameType.IPv4:
        case UriHostNameType.IPv6:
          return (EndPoint) new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
        default:
          foreach (IPAddress address in Dns.GetHostEntry(uri.Host).AddressList)
          {
            if (address.AddressFamily == addressFamily || addressFamily == AddressFamily.Unspecified)
              return (EndPoint) new IPEndPoint(address, uri.Port);
          }
          throw new IOException(string.Format("Cannot resolve '{0}' to an address in '{1}'", (object) uri.Host, (object) addressFamily));
      }
    }

    public virtual void CheckSocket()
    {
    }

    private void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.Close((AsyncContinuation) (ex => { }));
    }
  }
}
