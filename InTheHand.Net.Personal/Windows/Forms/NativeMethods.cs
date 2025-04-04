// Decompiled with JetBrains decompiler
// Type: InTheHand.Windows.Forms.NativeMethods
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Windows.Forms
{
  internal static class NativeMethods
  {
    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothSelectDevices(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothSelectDevicesFree(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);
  }
}
