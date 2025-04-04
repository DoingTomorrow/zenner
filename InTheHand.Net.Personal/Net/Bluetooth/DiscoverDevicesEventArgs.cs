// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.DiscoverDevicesEventArgs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.ComponentModel;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class DiscoverDevicesEventArgs : AsyncCompletedEventArgs
  {
    private BluetoothDeviceInfo[] m_devices;

    public DiscoverDevicesEventArgs(BluetoothDeviceInfo[] devices, object userState)
      : base((Exception) null, false, userState)
    {
      this.m_devices = devices != null ? devices : throw new ArgumentNullException(nameof (devices));
    }

    public DiscoverDevicesEventArgs(Exception exception, object userState)
      : base(exception, false, userState)
    {
    }

    public BluetoothDeviceInfo[] Devices
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this.m_devices;
      }
    }
  }
}
