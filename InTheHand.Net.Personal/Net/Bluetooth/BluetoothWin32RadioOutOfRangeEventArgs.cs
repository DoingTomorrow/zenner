// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothWin32RadioOutOfRangeEventArgs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Msft;
using System;
using System.Globalization;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class BluetoothWin32RadioOutOfRangeEventArgs : BluetoothWin32RadioEventArgs
  {
    private BluetoothWin32RadioOutOfRangeEventArgs(BLUETOOTH_DEVICE_INFO bdi0)
      : base(bdi0)
    {
    }

    internal static BluetoothWin32RadioOutOfRangeEventArgs Create(long addrLong)
    {
      return new BluetoothWin32RadioOutOfRangeEventArgs(new BLUETOOTH_DEVICE_INFO(addrLong));
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Device: {0} '{1}'", (object) this.Device.DeviceAddress, (object) this.Device.DeviceName);
    }
  }
}
