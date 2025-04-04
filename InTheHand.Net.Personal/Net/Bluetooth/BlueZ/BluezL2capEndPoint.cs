// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.BluezL2capEndPoint
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal class BluezL2capEndPoint(BluetoothAddress address, int psm) : BluetoothEndPoint(address, BluetoothService.Empty, psm)
  {
    private const int PsmOffset = 2;
    private const int PsmLength = 2;
    private const int AddrOffset = 4;
    private const int CidOffset = 10;
    private const int SaLen = 12;

    internal static BluezL2capEndPoint CreateConnectEndPoint(BluetoothEndPoint localEP)
    {
      return BluezL2capEndPoint.CreateBindEndPoint(localEP);
    }

    internal static BluezL2capEndPoint CreateBindEndPoint(BluetoothEndPoint serverEP)
    {
      int port = serverEP.Port != -1 ? serverEP.Port : 0;
      return new BluezL2capEndPoint(serverEP.Address, port);
    }

    public override AddressFamily AddressFamily
    {
      get => AddressFamily.HyperChannel | AddressFamily.AppleTalk;
    }

    public override EndPoint Create(SocketAddress socketAddress)
    {
      if (socketAddress.Family != this.AddressFamily)
        throw new ArgumentException("Wrong AddressFamily.");
      ushort psm = socketAddress.Size >= 12 ? (ushort) BitConverter.ToInt16(BluezL2capEndPoint.CopyFromSa(socketAddress, 2, 2), 0) : throw new ArgumentException("Too short sockaddr_l2 expected at least " + (object) 12 + ", but was: " + (object) socketAddress.Size + ".");
      return (EndPoint) new BluezL2capEndPoint(BluetoothAddress.CreateFromLittleEndian(BluezL2capEndPoint.CopyFromSa(socketAddress, 4, 6)), (int) psm);
    }

    public override SocketAddress Serialize()
    {
      SocketAddress sa = new SocketAddress(this.AddressFamily, 12);
      BluezL2capEndPoint.CopyToSa(BitConverter.GetBytes((short) checked ((ushort) this.Port)), sa, 2);
      BluezL2capEndPoint.CopyToSa(this.Address.ToByteArrayLittleEndian(), sa, 4);
      return sa;
    }

    internal static byte[] CopyFromSa(SocketAddress sa, int saOffset, int len)
    {
      byte[] numArray = new byte[len];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = sa[index + saOffset];
      return numArray;
    }

    internal static void CopyToSa(byte[] arr, SocketAddress sa, int saOffset)
    {
      for (int index = 0; index < arr.Length; ++index)
        sa[index + saOffset] = arr[index];
    }
  }
}
