// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothSecurity
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public static class BluetoothSecurity
  {
    private static IBluetoothSecurity _impl;

    private static IBluetoothSecurity GetImpl()
    {
      if (BluetoothSecurity._impl == null)
      {
        BluetoothSecurity._impl = BluetoothFactory.Factory.DoGetBluetoothSecurity();
        if (BluetoothSecurity._impl == null)
          throw new InvalidOperationException("Null IBluetoothSecurity returned.");
      }
      return BluetoothSecurity._impl;
    }

    public static bool PairRequest(BluetoothAddress device, string pin)
    {
      return BluetoothSecurity.GetImpl().PairRequest(device, pin);
    }

    public static bool RemoveDevice(BluetoothAddress device)
    {
      return !(device == (BluetoothAddress) null) ? BluetoothSecurity.GetImpl().RemoveDevice(device) : throw new ArgumentNullException(nameof (device));
    }

    public static bool SetPin(BluetoothAddress device, string pin)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      return pin != null ? BluetoothSecurity.GetImpl().SetPin(device, pin) : throw new ArgumentNullException(nameof (pin));
    }

    public static bool RevokePin(BluetoothAddress device)
    {
      return !(device == (BluetoothAddress) null) ? BluetoothSecurity.GetImpl().RevokePin(device) : throw new ArgumentNullException(nameof (device));
    }

    public static bool SetLinkKey(BluetoothAddress device, Guid linkKey)
    {
      return !(device == (BluetoothAddress) null) ? BluetoothSecurity.GetImpl().SetLinkKey(device, linkKey) : throw new ArgumentNullException(nameof (device));
    }

    public static BluetoothAddress GetPinRequest() => BluetoothSecurity.GetImpl().GetPinRequest();

    public static bool RefusePinRequest(BluetoothAddress device)
    {
      return !(device == (BluetoothAddress) null) ? BluetoothSecurity.GetImpl().RefusePinRequest(device) : throw new ArgumentNullException(nameof (device));
    }
  }
}
