// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.WindowsBluetoothSecurity
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal sealed class WindowsBluetoothSecurity : IBluetoothSecurity
  {
    private Hashtable authenticators = new Hashtable();

    internal WindowsBluetoothSecurity()
    {
    }

    public bool SetPin(BluetoothAddress device, string pin)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      if (pin == null)
        throw new ArgumentNullException(nameof (pin));
      if (this.authenticators.ContainsKey((object) device))
      {
        BluetoothWin32Authentication authenticator = (BluetoothWin32Authentication) this.authenticators[(object) device];
        this.authenticators.Remove((object) device);
        authenticator.Dispose();
      }
      this.authenticators.Add((object) device, (object) new BluetoothWin32Authentication(device, pin));
      return true;
    }

    public bool RevokePin(BluetoothAddress device)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      if (!this.authenticators.ContainsKey((object) device))
        return false;
      BluetoothWin32Authentication authenticator = (BluetoothWin32Authentication) this.authenticators[(object) device];
      this.authenticators.Remove((object) device);
      authenticator.Dispose();
      return true;
    }

    public bool PairRequest(BluetoothAddress device, string pin)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      BLUETOOTH_DEVICE_INFO pbtdi = device.ToInt64() != 0L ? new BLUETOOTH_DEVICE_INFO(device.ToInt64()) : throw new ArgumentNullException(nameof (device), "A non-blank address must be specified.");
      int ulPasskeyLength = 0;
      if (pin != null)
        ulPasskeyLength = pin.Length;
      return NativeMethods.BluetoothAuthenticateDevice(IntPtr.Zero, IntPtr.Zero, ref pbtdi, pin, ulPasskeyLength) == 0;
    }

    private bool PairRequest(
      BluetoothAddress device,
      BluetoothAuthenticationRequirements authenticationRequirement)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      BLUETOOTH_DEVICE_INFO pbtdiInout = device.ToInt64() != 0L ? new BLUETOOTH_DEVICE_INFO(device.ToInt64()) : throw new ArgumentNullException(nameof (device), "A non-blank address must be specified.");
      byte[] pbtOobData = (byte[]) null;
      return NativeMethods.BluetoothAuthenticateDeviceEx(IntPtr.Zero, IntPtr.Zero, ref pbtdiInout, pbtOobData, authenticationRequirement) == 0;
    }

    public bool RemoveDevice(BluetoothAddress device)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      return NativeMethods.BluetoothRemoveDevice(device.ToByteArray()) == 0;
    }

    public bool SetLinkKey(BluetoothAddress device, Guid linkkey)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      throw new PlatformNotSupportedException("Not supported on Windows XP");
    }

    public BluetoothAddress GetPinRequest()
    {
      throw new PlatformNotSupportedException("Not supported on Windows XP");
    }

    public bool RefusePinRequest(BluetoothAddress device)
    {
      if (device == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (device));
      throw new PlatformNotSupportedException("Not supported on Windows XP");
    }
  }
}
