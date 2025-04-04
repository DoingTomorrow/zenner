// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal struct BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO
  {
    internal long bthAddressRemote;
    internal BluetoothAuthenticationMethod authMethod;
    internal BLUETOOTH_OOB_DATA_INFO oobInfo;
    internal byte negativeResponse;
  }
}
