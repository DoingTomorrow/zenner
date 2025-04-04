// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothPublicFactory
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class BluetoothPublicFactory
  {
    private BluetoothFactory m_factory;

    internal BluetoothPublicFactory(BluetoothFactory factory)
    {
      this.m_factory = factory;
      this.m_factory.ToString();
    }

    public BluetoothClient CreateBluetoothClient() => new BluetoothClient(this.m_factory);

    public BluetoothClient CreateBluetoothClient(BluetoothEndPoint localEP)
    {
      return new BluetoothClient(this.m_factory, localEP);
    }

    public BluetoothListener CreateBluetoothListener(Guid service)
    {
      return new BluetoothListener(this.m_factory, service);
    }

    public BluetoothListener CreateBluetoothListener(BluetoothAddress localAddress, Guid service)
    {
      return new BluetoothListener(this.m_factory, localAddress, service);
    }

    public BluetoothListener CreateBluetoothListener(BluetoothEndPoint localEP)
    {
      return new BluetoothListener(this.m_factory, localEP);
    }

    public BluetoothListener CreateBluetoothListener(
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
    {
      return new BluetoothListener(this.m_factory, service, sdpRecord, channelOffset);
    }

    public BluetoothListener CreateBluetoothListener(
      BluetoothAddress localAddress,
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
    {
      return new BluetoothListener(this.m_factory, localAddress, service, sdpRecord, channelOffset);
    }

    public BluetoothListener CreateBluetoothListener(
      BluetoothEndPoint localEP,
      byte[] sdpRecord,
      int channelOffset)
    {
      return new BluetoothListener(this.m_factory, localEP, sdpRecord, channelOffset);
    }

    public BluetoothListener CreateBluetoothListener(Guid service, ServiceRecord sdpRecord)
    {
      return new BluetoothListener(this.m_factory, service, sdpRecord);
    }

    public BluetoothListener CreateBluetoothListener(
      BluetoothAddress localAddress,
      Guid service,
      ServiceRecord sdpRecord)
    {
      return new BluetoothListener(this.m_factory, localAddress, service, sdpRecord);
    }

    public BluetoothListener CreateBluetoothListener(
      BluetoothEndPoint localEP,
      ServiceRecord sdpRecord)
    {
      return new BluetoothListener(this.m_factory, localEP, sdpRecord);
    }

    public IBluetoothSecurity BluetoothSecurity => this.m_factory.DoGetBluetoothSecurity();

    public BluetoothDeviceInfo CreateBluetoothDeviceInfo(BluetoothAddress addr)
    {
      return new BluetoothDeviceInfo(this.m_factory.DoGetBluetoothDeviceInfo(addr));
    }

    public ObexWebRequest CreateObexWebRequest(Uri requestUri)
    {
      return new ObexWebRequest(requestUri, this);
    }

    public ObexWebRequest CreateObexWebRequest(string scheme, BluetoothAddress target, string path)
    {
      return new ObexWebRequest(scheme, target, path, this);
    }

    public ObexListener CreateObexListener() => new ObexListener(this);
  }
}
