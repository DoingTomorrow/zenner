// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.BLUETOOTH_DEVICE_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Win32;
using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct BLUETOOTH_DEVICE_INFO
  {
    internal int dwSize;
    internal long Address;
    internal uint ulClassofDevice;
    [MarshalAs(UnmanagedType.Bool)]
    internal bool fConnected;
    [MarshalAs(UnmanagedType.Bool)]
    internal bool fRemembered;
    [MarshalAs(UnmanagedType.Bool)]
    internal bool fAuthenticated;
    internal SYSTEMTIME stLastSeen;
    internal SYSTEMTIME stLastUsed;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
    internal string szName;

    public BLUETOOTH_DEVICE_INFO(long address)
    {
      this.dwSize = 560;
      this.Address = address;
      this.ulClassofDevice = 0U;
      this.fConnected = false;
      this.fRemembered = false;
      this.fAuthenticated = false;
      this.stLastSeen = new SYSTEMTIME();
      this.stLastUsed = new SYSTEMTIME();
      this.szName = "";
    }

    public BLUETOOTH_DEVICE_INFO(BluetoothAddress address)
    {
      if (address == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (address));
      this.dwSize = 560;
      this.Address = address.ToInt64();
      this.ulClassofDevice = 0U;
      this.fConnected = false;
      this.fRemembered = false;
      this.fAuthenticated = false;
      this.stLastSeen = new SYSTEMTIME();
      this.stLastUsed = new SYSTEMTIME();
      this.szName = "";
    }

    internal DateTime LastSeen => this.stLastSeen.ToDateTime(DateTimeKind.Utc);

    internal DateTime LastUsed => this.stLastUsed.ToDateTime(DateTimeKind.Utc);

    internal static BLUETOOTH_DEVICE_INFO Create(BTH_DEVICE_INFO deviceInfo)
    {
      BLUETOOTH_DEVICE_INFO bluetoothDeviceInfo = new BLUETOOTH_DEVICE_INFO(deviceInfo.address);
      if ((deviceInfo.flags & BluetoothDeviceInfoProperties.Cod) != (BluetoothDeviceInfoProperties) 0)
        bluetoothDeviceInfo.ulClassofDevice = deviceInfo.classOfDevice;
      byte[] name = deviceInfo.name;
      if ((deviceInfo.flags & BluetoothDeviceInfoProperties.Name) != (BluetoothDeviceInfoProperties) 0)
      {
        int count = Array.IndexOf<byte>(name, (byte) 0);
        if (count != -1)
        {
          string str = Encoding.UTF8.GetString(name, 0, count);
          bluetoothDeviceInfo.szName = str;
        }
      }
      bluetoothDeviceInfo.fAuthenticated = (BluetoothDeviceInfoProperties) 0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Paired);
      bluetoothDeviceInfo.fConnected = (BluetoothDeviceInfoProperties) 0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Connected);
      bluetoothDeviceInfo.fRemembered = (BluetoothDeviceInfoProperties) 0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Personal);
      return bluetoothDeviceInfo;
    }
  }
}
