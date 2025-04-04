// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.IBluetoothDeviceInfo
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public interface IBluetoothDeviceInfo
  {
    void Merge(IBluetoothDeviceInfo other);

    void SetDiscoveryTime(DateTime dt);

    bool Authenticated { get; }

    ClassOfDevice ClassOfDevice { get; }

    bool Connected { get; }

    BluetoothAddress DeviceAddress { get; }

    string DeviceName { get; set; }

    byte[][] GetServiceRecordsUnparsed(Guid service);

    ServiceRecord[] GetServiceRecords(Guid service);

    IAsyncResult BeginGetServiceRecords(Guid service, AsyncCallback callback, object state);

    ServiceRecord[] EndGetServiceRecords(IAsyncResult asyncResult);

    Guid[] InstalledServices { get; }

    DateTime LastSeen { get; }

    DateTime LastUsed { get; }

    void Refresh();

    bool Remembered { get; }

    int Rssi { get; }

    void SetServiceState(Guid service, bool state, bool throwOnError);

    void SetServiceState(Guid service, bool state);

    void ShowDialog();

    void Update();

    RadioVersions GetVersions();
  }
}
