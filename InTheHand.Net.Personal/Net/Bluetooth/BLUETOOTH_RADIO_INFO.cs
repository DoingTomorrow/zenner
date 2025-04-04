// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BLUETOOTH_RADIO_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct BLUETOOTH_RADIO_INFO
  {
    private const int BLUETOOTH_MAX_NAME_SIZE = 248;
    internal int dwSize;
    internal long address;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
    internal string szName;
    internal uint ulClassofDevice;
    internal ushort lmpSubversion;
    [MarshalAs(UnmanagedType.U2)]
    internal Manufacturer manufacturer;
  }
}
