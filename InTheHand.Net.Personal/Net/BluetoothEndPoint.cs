// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.BluetoothEndPoint
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace InTheHand.Net
{
  [Serializable]
  public class BluetoothEndPoint : EndPoint
  {
    private const int defaultPort = -1;
    public const int MinPort = 1;
    public const int MaxPort = 65535;
    public const int MinScn = 1;
    public const int MaxScn = 30;
    private BluetoothAddress m_id;
    private Guid m_service;
    private int m_port;
    private static readonly AddressFamily _addrFamily = (AddressFamily) 32;
    private static readonly bool _isBlueZ;

    static BluetoothEndPoint()
    {
      if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != (PlatformID) 128)
        return;
      BluetoothEndPoint._isBlueZ = true;
      BluetoothEndPoint._addrFamily = AddressFamily.HyperChannel | AddressFamily.AppleTalk;
    }

    private BluetoothEndPoint()
    {
    }

    public BluetoothEndPoint(BluetoothAddress address, Guid service)
      : this(address, service, -1)
    {
    }

    public BluetoothEndPoint(BluetoothAddress address, Guid service, int port)
    {
      this.m_id = address;
      this.m_service = service;
      this.m_port = port;
    }

    public override SocketAddress Serialize()
    {
      int size = 30;
      if (BluetoothEndPoint._isBlueZ)
        size = 10;
      SocketAddress socketAddress = new SocketAddress(BluetoothEndPoint._addrFamily, size);
      socketAddress[0] = checked ((byte) (uint) BluetoothEndPoint._addrFamily);
      if (this.m_id != (BluetoothAddress) null)
      {
        byte[] byteArray = this.m_id.ToByteArray();
        for (int index = 0; index < 6; ++index)
          socketAddress[index + 2] = byteArray[index];
      }
      if (this.m_service != Guid.Empty)
      {
        byte[] byteArray = this.m_service.ToByteArray();
        for (int index = 0; index < 16 && !BluetoothEndPoint._isBlueZ; ++index)
          socketAddress[index + 10] = byteArray[index];
      }
      byte[] bytes = BitConverter.GetBytes(this.m_port);
      for (int index = 0; index < 4; ++index)
      {
        if (BluetoothEndPoint._isBlueZ)
        {
          socketAddress[index + 8] = bytes[index];
          break;
        }
        socketAddress[index + 26] = bytes[index];
      }
      return socketAddress;
    }

    public override EndPoint Create(SocketAddress socketAddress)
    {
      if (socketAddress == null)
        throw new ArgumentNullException(nameof (socketAddress));
      if ((AddressFamily) socketAddress[0] != BluetoothEndPoint._addrFamily)
        return base.Create(socketAddress);
      byte[] address = new byte[6];
      for (int index = 0; index < 6; ++index)
        address[index] = socketAddress[2 + index];
      byte[] b = new byte[16];
      for (int index = 0; index < 16 && !BluetoothEndPoint._isBlueZ; ++index)
        b[index] = socketAddress[10 + index];
      byte[] numArray = new byte[4];
      for (int index = 0; index < 4; ++index)
      {
        if (BluetoothEndPoint._isBlueZ)
        {
          numArray[index] = socketAddress[8 + index];
          break;
        }
        numArray[index] = socketAddress[26 + index];
      }
      return (EndPoint) new BluetoothEndPoint(new BluetoothAddress(address), new Guid(b), BitConverter.ToInt32(numArray, 0));
    }

    public override bool Equals(object obj)
    {
      if (!(obj is BluetoothEndPoint bluetoothEndPoint))
        return base.Equals(obj);
      return this.Address.Equals((object) bluetoothEndPoint.Address) && this.Service.Equals(bluetoothEndPoint.Service);
    }

    public override int GetHashCode() => this.Address.GetHashCode();

    public override string ToString()
    {
      return this.m_port != -1 ? this.Address.ToString() + ":" + this.Port.ToString() : this.Address.ToString() + ":" + this.Service.ToString("N");
    }

    public override AddressFamily AddressFamily
    {
      [DebuggerStepThrough] get => (AddressFamily) 32;
    }

    public BluetoothAddress Address
    {
      [DebuggerStepThrough] get => this.m_id;
      [DebuggerStepThrough] set => this.m_id = value;
    }

    public Guid Service
    {
      [DebuggerStepThrough] get => this.m_service;
      [DebuggerStepThrough] set => this.m_service = value;
    }

    public int Port
    {
      [DebuggerStepThrough] get => this.m_port;
      [DebuggerStepThrough] set => this.m_port = value;
    }

    public bool HasPort => this.m_port != 0 && this.m_port != -1;

    public object Clone()
    {
      return (object) new BluetoothEndPoint((BluetoothAddress) this.Address.Clone(), this.Service, this.Port);
    }

    [Conditional("DEBUG")]
    internal static void Dump(string name, SocketAddress btsa)
    {
      StringBuilder stringBuilder = new StringBuilder(3 * btsa.Size);
      for (int offset = 0; offset < btsa.Size; ++offset)
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0:X2} ", (object) btsa[offset]);
      if (stringBuilder.Length > 0)
        --stringBuilder.Length;
      string.Format((IFormatProvider) CultureInfo.InvariantCulture, "SA @ {0,9}: family: {1} 0x{1:X}, size: {2}, < {3} >", (object) name, (object) btsa.Family, (object) btsa.Size, (object) stringBuilder.ToString());
    }
  }
}
