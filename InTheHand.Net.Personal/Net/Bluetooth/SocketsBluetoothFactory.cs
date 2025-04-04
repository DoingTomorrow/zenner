// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.SocketsBluetoothFactory
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Msft;
using System;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal class SocketsBluetoothFactory : BluetoothFactory
  {
    private static WindowsBluetoothSecurity m_btSecurity;

    public SocketsBluetoothFactory()
    {
      if (!WindowsBluetoothRadio.IsPlatformSupported)
        throw new PlatformNotSupportedException("Microsoft Bluetooth stack not supported (radio).");
    }

    protected override void Dispose(bool disposing)
    {
    }

    protected override IBluetoothClient GetBluetoothClient()
    {
      return (IBluetoothClient) new SocketBluetoothClient((BluetoothFactory) this);
    }

    protected override IBluetoothClient GetBluetoothClient(Socket acceptedSocket)
    {
      return (IBluetoothClient) new SocketBluetoothClient((BluetoothFactory) this, acceptedSocket);
    }

    protected override IBluetoothClient GetBluetoothClientForListener(
      CommonRfcommStream acceptedStream)
    {
      throw new NotSupportedException();
    }

    protected override IBluetoothClient GetBluetoothClient(BluetoothEndPoint localEP)
    {
      return (IBluetoothClient) new SocketBluetoothClient((BluetoothFactory) this, localEP);
    }

    protected override IBluetoothListener GetBluetoothListener()
    {
      return (IBluetoothListener) new WindowsBluetoothListener((BluetoothFactory) this);
    }

    protected override IBluetoothDeviceInfo GetBluetoothDeviceInfo(BluetoothAddress address)
    {
      return (IBluetoothDeviceInfo) new WindowsBluetoothDeviceInfo(address);
    }

    protected override IBluetoothRadio GetPrimaryRadio() => WindowsBluetoothRadio.GetPrimaryRadio();

    protected override IBluetoothRadio[] GetAllRadios() => WindowsBluetoothRadio.AllRadios;

    protected override IBluetoothSecurity GetBluetoothSecurity()
    {
      if (SocketsBluetoothFactory.m_btSecurity == null)
        SocketsBluetoothFactory.m_btSecurity = new WindowsBluetoothSecurity();
      return (IBluetoothSecurity) SocketsBluetoothFactory.m_btSecurity;
    }
  }
}
