// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.BluezRfcommEndPoint
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal class BluezRfcommEndPoint(BluetoothAddress address, int scn) : BluetoothEndPoint(address, BluetoothService.Empty, scn)
  {
    private const int AddrOffset = 2;
    private const int ScnOffset = 8;
    private const int ScnLength = 1;
    private const int SaLen = 10;

    internal static BluezRfcommEndPoint CreateConnectEndPoint(BluetoothEndPoint localEP)
    {
      return BluezRfcommEndPoint.CreateBindEndPoint(localEP);
    }

    internal static BluezRfcommEndPoint CreateBindEndPoint(BluetoothEndPoint serverEP)
    {
      int port = serverEP.Port != -1 ? serverEP.Port : 0;
      return new BluezRfcommEndPoint(serverEP.Address, port);
    }

    public override AddressFamily AddressFamily
    {
      get => AddressFamily.HyperChannel | AddressFamily.AppleTalk;
    }

    public override EndPoint Create(SocketAddress socketAddress)
    {
      if (socketAddress.Family != this.AddressFamily)
        throw new ArgumentException("Wrong AddressFamily.");
      byte psm = BluezL2capEndPoint.CopyFromSa(socketAddress, 8, 1)[0];
      return (EndPoint) new BluezL2capEndPoint(BluetoothAddress.CreateFromLittleEndian(BluezL2capEndPoint.CopyFromSa(socketAddress, 2, 6)), (int) psm);
    }

    public override SocketAddress Serialize()
    {
      SocketAddress sa = new SocketAddress(this.AddressFamily, 10);
      BluezL2capEndPoint.CopyToSa(BitConverter.GetBytes((short) checked ((byte) this.Port)), sa, 8);
      BluezL2capEndPoint.CopyToSa(this.Address.ToByteArrayLittleEndian(), sa, 2);
      return sa;
    }
  }
}
