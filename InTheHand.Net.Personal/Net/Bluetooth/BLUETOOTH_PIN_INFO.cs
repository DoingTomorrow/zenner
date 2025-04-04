// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BLUETOOTH_PIN_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [StructLayout(LayoutKind.Sequential, Size = 20)]
  internal struct BLUETOOTH_PIN_INFO
  {
    internal const int BTH_MAX_PIN_SIZE = 16;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    internal byte[] pin;
    internal int pinLength;
  }
}
