// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.IrDAEndPoint
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace InTheHand.Net
{
  public class IrDAEndPoint : EndPoint
  {
    private IrDAAddress id;
    private string service;

    [Obsolete("Use the constructor which accepts an IrDAAddress.", false)]
    public IrDAEndPoint(byte[] irdaDeviceID, string serviceName)
    {
      if (irdaDeviceID == null)
        throw new ArgumentNullException(nameof (irdaDeviceID));
      if (serviceName == null)
        throw new ArgumentNullException(nameof (serviceName));
      this.id = new IrDAAddress(irdaDeviceID);
      this.service = serviceName;
    }

    public IrDAEndPoint(IrDAAddress irdaDeviceAddress, string serviceName)
    {
      if (irdaDeviceAddress == (IrDAAddress) null)
        throw new ArgumentNullException(nameof (irdaDeviceAddress));
      if (serviceName == null)
        throw new ArgumentNullException(nameof (serviceName));
      this.id = irdaDeviceAddress;
      this.service = serviceName;
    }

    public IrDAAddress Address
    {
      get => this.id;
      set
      {
        this.id = !(value == (IrDAAddress) null) ? value : throw new ArgumentNullException(nameof (value));
      }
    }

    [Obsolete("Use the Address property to access the device Address.", false)]
    public byte[] DeviceID
    {
      get => this.id.ToByteArray();
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.id = value.Length == 4 ? new IrDAAddress(value) : throw ExceptionFactory.ArgumentOutOfRangeException(nameof (value), "DeviceID must be 4 bytes");
      }
    }

    public string ServiceName
    {
      get => this.service;
      set => this.service = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public override AddressFamily AddressFamily => AddressFamily.Irda;

    public override SocketAddress Serialize()
    {
      SocketAddress socketAddress = new SocketAddress(AddressFamily.Irda, 32);
      byte[] byteArray = this.id.ToByteArray();
      for (int index = 0; index < 4; ++index)
        socketAddress[index + 2] = byteArray[index];
      byte[] bytes = Encoding.ASCII.GetBytes(this.service);
      if (bytes.Length > 24)
        throw new InvalidOperationException("ServiceName has a maximum length of 24 bytes.");
      for (int index = 0; index < bytes.Length; ++index)
        socketAddress[index + 6] = bytes[index];
      return socketAddress[30] == (byte) 0 && socketAddress[31] == (byte) 0 ? socketAddress : throw new InvalidOperationException("ServiceName too long for SocketAddress.");
    }

    public override EndPoint Create(SocketAddress socketAddress)
    {
      if (socketAddress == null)
        throw new ArgumentNullException(nameof (socketAddress));
      byte[] address = new byte[4];
      for (int index = 0; index < 4; ++index)
        address[index] = socketAddress[index + 2];
      byte[] bytes = new byte[24];
      for (int index = 0; index < bytes.Length; ++index)
        bytes[index] = socketAddress[index + 6];
      string serviceName = Encoding.ASCII.GetString(bytes, 0, 24);
      if (serviceName.IndexOf(char.MinValue) > -1)
        serviceName = serviceName.Substring(0, serviceName.IndexOf(char.MinValue));
      return (EndPoint) new IrDAEndPoint(new IrDAAddress(address), serviceName);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is IrDAEndPoint irDaEndPoint))
        return base.Equals(obj);
      return this.Address.Equals((object) irDaEndPoint.Address) && this.ServiceName.Equals(irDaEndPoint.ServiceName);
    }

    public override int GetHashCode() => this.Address.GetHashCode();

    public override string ToString() => this.Address.ToString() + ":" + this.ServiceName;
  }
}
