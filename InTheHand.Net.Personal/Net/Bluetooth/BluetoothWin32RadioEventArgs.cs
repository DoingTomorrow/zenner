// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothWin32RadioEventArgs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Msft;
using InTheHand.Net.Sockets;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public abstract class BluetoothWin32RadioEventArgs : EventArgs
  {
    private readonly WindowsBluetoothDeviceInfo _bdiWin;
    private readonly BluetoothDeviceInfo _bdi;

    internal BluetoothWin32RadioEventArgs(BLUETOOTH_DEVICE_INFO bdi0)
    {
      this._bdiWin = new WindowsBluetoothDeviceInfo(bdi0);
      this._bdi = new BluetoothDeviceInfo((IBluetoothDeviceInfo) this._bdiWin);
    }

    public BluetoothDeviceInfo Device => this._bdi;

    internal WindowsBluetoothDeviceInfo DeviceWindows => this._bdiWin;
  }
}
