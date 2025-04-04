// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.BTH_DEVICE_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal struct BTH_DEVICE_INFO
  {
    internal BluetoothDeviceInfoProperties flags;
    internal long address;
    internal uint classOfDevice;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 248)]
    internal byte[] name;
  }
}
