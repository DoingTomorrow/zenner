// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.IBluetoothRadio
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public interface IBluetoothRadio
  {
    ClassOfDevice ClassOfDevice { get; }

    IntPtr Handle { get; }

    HardwareStatus HardwareStatus { get; }

    BluetoothAddress LocalAddress { get; }

    RadioMode Mode { get; set; }

    string Name { get; set; }

    Manufacturer SoftwareManufacturer { get; }

    string Remote { get; }

    HciVersion HciVersion { get; }

    int HciRevision { get; }

    LmpVersion LmpVersion { get; }

    int LmpSubversion { get; }

    Manufacturer Manufacturer { get; }
  }
}
