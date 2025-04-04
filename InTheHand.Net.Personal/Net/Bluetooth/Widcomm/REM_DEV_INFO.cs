// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.REM_DEV_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct REM_DEV_INFO
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    internal byte[] bda;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    internal byte[] dev_class;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 248)]
    internal byte[] bd_name;
    internal bool b_paired;
    internal bool b_connected;
  }
}
