// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BDIFMasks
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal static class BDIFMasks
  {
    public const BluetoothDeviceInfoProperties AllOrig = BluetoothDeviceInfoProperties.Address | BluetoothDeviceInfoProperties.Cod | BluetoothDeviceInfoProperties.Name | BluetoothDeviceInfoProperties.Paired | BluetoothDeviceInfoProperties.Personal | BluetoothDeviceInfoProperties.Connected;
    public const BluetoothDeviceInfoProperties AllKb942567 = BluetoothDeviceInfoProperties.Address | BluetoothDeviceInfoProperties.Cod | BluetoothDeviceInfoProperties.Name | BluetoothDeviceInfoProperties.Paired | BluetoothDeviceInfoProperties.Personal | BluetoothDeviceInfoProperties.Connected | BluetoothDeviceInfoProperties.ShortName | BluetoothDeviceInfoProperties.Visible | BluetoothDeviceInfoProperties.SspSupported | BluetoothDeviceInfoProperties.SspPaired | BluetoothDeviceInfoProperties.SspMitmProtected | BluetoothDeviceInfoProperties.Rssi | BluetoothDeviceInfoProperties.Eir;
    public const BluetoothDeviceInfoProperties AllWindows8 = BluetoothDeviceInfoProperties.Address | BluetoothDeviceInfoProperties.Cod | BluetoothDeviceInfoProperties.Name | BluetoothDeviceInfoProperties.Paired | BluetoothDeviceInfoProperties.Personal | BluetoothDeviceInfoProperties.Connected | BluetoothDeviceInfoProperties.ShortName | BluetoothDeviceInfoProperties.Visible | BluetoothDeviceInfoProperties.SspSupported | BluetoothDeviceInfoProperties.SspPaired | BluetoothDeviceInfoProperties.SspMitmProtected | BluetoothDeviceInfoProperties.Rssi | BluetoothDeviceInfoProperties.Eir | BluetoothDeviceInfoProperties.BR | BluetoothDeviceInfoProperties.LE | BluetoothDeviceInfoProperties.LEPaired | BluetoothDeviceInfoProperties.LEPersonal | BluetoothDeviceInfoProperties.LEMitmProtected;
  }
}
