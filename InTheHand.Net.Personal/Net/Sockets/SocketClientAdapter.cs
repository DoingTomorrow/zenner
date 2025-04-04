// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.SocketClientAdapter
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  internal sealed class SocketClientAdapter : SocketStreamIOAdapter
  {
    private readonly BluetoothClient cli1B;
    private readonly Socket sIT;

    public SocketClientAdapter(BluetoothClient cli)
      : base((Stream) cli.GetStream())
    {
      this.cli1B = cli;
    }

    public SocketClientAdapter(IrDAClient cli)
      : base((Stream) cli.GetStream())
    {
      this.sIT = cli.Client;
    }

    public SocketClientAdapter(TcpClient cli)
      : base((Stream) cli.GetStream())
    {
      this.sIT = cli.Client;
    }

    public override EndPoint LocalEndPoint
    {
      get
      {
        return this.cli1B != null ? (EndPoint) new BluetoothEndPoint(BluetoothAddress.None, Guid.Empty) : this.sIT.LocalEndPoint;
      }
    }

    public override EndPoint RemoteEndPoint
    {
      get => this.cli1B != null ? (EndPoint) this.cli1B.RemoteEndPoint : this.sIT.RemoteEndPoint;
    }

    public override int Available => this.cli1B != null ? this.cli1B.Available : this.sIT.Available;
  }
}
