// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.DiscoDevsParams
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public class DiscoDevsParams
  {
    public readonly DateTime discoTime;
    public readonly int maxDevices;
    public readonly bool authenticated;
    public readonly bool remembered;
    public readonly bool unknown;
    public readonly bool discoverableOnly;

    public DiscoDevsParams(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      DateTime discoTime)
    {
      this.maxDevices = maxDevices;
      this.authenticated = authenticated;
      this.remembered = remembered;
      this.unknown = unknown;
      this.discoverableOnly = discoverableOnly;
      this.discoTime = discoTime;
    }

    public static List<IBluetoothDeviceInfo> DiscoverDevicesMerge(
      bool authenticated,
      bool remembered,
      bool unknown,
      List<IBluetoothDeviceInfo> knownDevices,
      List<IBluetoothDeviceInfo> discoverableDevices,
      bool discoverableOnly,
      DateTime discoTime)
    {
      return BluetoothClient.DiscoverDevicesMerge(authenticated, remembered, unknown, knownDevices, discoverableDevices, discoverableOnly, discoTime);
    }
  }
}
