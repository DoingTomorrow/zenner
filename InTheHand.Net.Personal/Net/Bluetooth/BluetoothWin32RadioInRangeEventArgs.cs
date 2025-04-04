// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothWin32RadioInRangeEventArgs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Msft;
using System;
using System.Globalization;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class BluetoothWin32RadioInRangeEventArgs : BluetoothWin32RadioEventArgs
  {
    private readonly BluetoothDeviceInfoProperties _currentFlags;
    private readonly BluetoothDeviceInfoProperties _previousFlags;

    private BluetoothWin32RadioInRangeEventArgs(
      BLUETOOTH_DEVICE_INFO bdi0,
      BluetoothDeviceInfoProperties currentFlags,
      BluetoothDeviceInfoProperties previousFlags)
      : base(bdi0)
    {
      this._currentFlags = currentFlags;
      this._previousFlags = previousFlags;
    }

    internal static BluetoothWin32RadioInRangeEventArgs Create(
      BluetoothDeviceInfoProperties previousFlags,
      BluetoothDeviceInfoProperties currentFlags,
      BLUETOOTH_DEVICE_INFO deviceInfo)
    {
      return new BluetoothWin32RadioInRangeEventArgs(deviceInfo, currentFlags, previousFlags);
    }

    public BluetoothDeviceInfoProperties CurrentState => this._currentFlags;

    public BluetoothDeviceInfoProperties PreviousState => this._previousFlags;

    private BluetoothDeviceInfoProperties DifferentStates
    {
      get => this._currentFlags ^ this._previousFlags;
    }

    public BluetoothDeviceInfoProperties GainedStates => this.DifferentStates & this._currentFlags;

    public BluetoothDeviceInfoProperties LostStates => this.DifferentStates & this._previousFlags;

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Device: {0} '{1}', cur flags: '{2}', old: '{3}'", (object) this.Device.DeviceAddress, (object) this.Device.DeviceName, (object) this._currentFlags, (object) this._previousFlags);
    }
  }
}
