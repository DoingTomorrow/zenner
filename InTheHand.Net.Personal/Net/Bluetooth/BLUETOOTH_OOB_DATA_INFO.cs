// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BLUETOOTH_OOB_DATA_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [StructLayout(LayoutKind.Sequential, Size = 32)]
  internal struct BLUETOOTH_OOB_DATA_INFO
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    internal byte[] C;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    internal byte[] R;

    private void ShutupCompiler() => this.C = this.R = (byte[]) null;
  }
}
