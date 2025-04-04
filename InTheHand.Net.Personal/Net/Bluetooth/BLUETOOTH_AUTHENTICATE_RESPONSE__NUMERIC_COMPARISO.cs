// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal struct BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO
  {
    internal long bthAddressRemote;
    internal BluetoothAuthenticationMethod authMethod;
    internal uint numericComp_passkey;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
    private byte[] _padding;
    internal byte negativeResponse;
  }
}
