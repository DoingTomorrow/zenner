// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.NullBtCli
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal class NullBtCli : CommonBluetoothClient
  {
    internal NullBtCli(NullBluetoothFactory fcty, CommonRfcommStream conn)
      : base((BluetoothFactory) fcty, conn)
    {
    }

    public override IAsyncResult BeginServiceDiscovery(
      BluetoothAddress address,
      Guid serviceGuid,
      AsyncCallback asyncCallback,
      object state)
    {
      throw new NotImplementedException();
    }

    public override List<int> EndServiceDiscovery(IAsyncResult ar)
    {
      throw new NotImplementedException();
    }

    protected override void BeginInquiry(
      int maxDevices,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args)
    {
      throw new NotImplementedException();
    }

    protected override List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      throw new NotImplementedException();
    }

    protected override List<IBluetoothDeviceInfo> GetKnownRemoteDeviceEntries()
    {
      throw new NotImplementedException();
    }

    public override void SetPin(string pin) => throw new NotImplementedException();

    public override void SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException();
    }
  }
}
