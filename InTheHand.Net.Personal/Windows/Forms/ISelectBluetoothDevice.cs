// Decompiled with JetBrains decompiler
// Type: InTheHand.Windows.Forms.ISelectBluetoothDevice
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;

#nullable disable
namespace InTheHand.Windows.Forms
{
  internal interface ISelectBluetoothDevice
  {
    void Reset();

    bool ShowAuthenticated { get; set; }

    bool ShowRemembered { get; set; }

    bool ShowUnknown { get; set; }

    bool ShowDiscoverableOnly { get; set; }

    bool ForceAuthentication { get; set; }

    string Info { get; set; }

    bool AddNewDeviceWizard { get; set; }

    bool SkipServicesPage { get; set; }

    void SetClassOfDevices(ClassOfDevice[] classOfDevices);

    void SetFilter(
      Predicate<BluetoothDeviceInfo> filterFn,
      SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK msftFilterFn);
  }
}
