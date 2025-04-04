// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Msft;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal struct BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS
  {
    internal BLUETOOTH_DEVICE_INFO deviceInfo;
    internal BluetoothAuthenticationMethod authenticationMethod;
    internal BluetoothIoCapability ioCapability;
    internal BluetoothAuthenticationRequirements authenticationRequirements;
    internal uint Numeric_Value_Passkey;

    private void ShutupCompiler()
    {
      this.deviceInfo = new BLUETOOTH_DEVICE_INFO();
      this.authenticationMethod = BluetoothAuthenticationMethod.Legacy;
      this.ioCapability = BluetoothIoCapability.Undefined;
      this.authenticationRequirements = BluetoothAuthenticationRequirements.MITMProtectionNotDefined;
      this.Numeric_Value_Passkey = 0U;
    }
  }
}
