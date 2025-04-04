// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.WindowsBluetoothRadio
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Win32;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal sealed class WindowsBluetoothRadio : IBluetoothRadio
  {
    private BLUETOOTH_RADIO_INFO radio;
    private IntPtr handle;
    private readonly LmpVersion _lmpV = LmpVersion.Unknown;
    private readonly int _lmpSubv;
    private readonly HciVersion _hciV = HciVersion.Unknown;
    private readonly int _hciRev;

    public string Remote => (string) null;

    internal static bool IsPlatformSupported => WindowsBluetoothRadio.AllRadios.Length > 0;

    internal WindowsBluetoothRadio(IntPtr handle)
    {
      this.radio = new BLUETOOTH_RADIO_INFO();
      this.radio.dwSize = 520;
      int radioInfo = NativeMethods.BluetoothGetRadioInfo(handle, ref this.radio);
      if (radioInfo != 0)
        throw new Win32Exception(radioInfo, "Error retrieving Radio information.");
      this.handle = handle;
      this.ReadVersionInfo(handle, ref this._lmpV, ref this._lmpSubv, ref this._hciV, ref this._hciRev);
    }

    private void ReadVersionInfo(
      IntPtr handle,
      ref LmpVersion lmpV,
      ref int lmpSubv,
      ref HciVersion hciV,
      ref int hciRev)
    {
      byte[] OutBuffer = new byte[300];
      if (!NativeMethods.DeviceIoControl(handle, 4259840U, IntPtr.Zero, 0, OutBuffer, OutBuffer.Length, out int _, IntPtr.Zero))
      {
        Marshal.GetLastWin32Error();
      }
      else
      {
        hciRev = (int) BitConverter.ToUInt16(OutBuffer, 276);
        hciV = (HciVersion) OutBuffer[278];
        lmpSubv = (int) BitConverter.ToUInt16(OutBuffer, 289);
        lmpV = (LmpVersion) OutBuffer[291];
        int uint16 = (int) BitConverter.ToUInt16(OutBuffer, 287);
        int uint32 = (int) BitConverter.ToUInt32(OutBuffer, 272);
        BitConverter.ToInt64(OutBuffer, 279);
      }
    }

    internal static IBluetoothRadio GetPrimaryRadio()
    {
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      BLUETOOTH_FIND_RADIO_PARAMS pbtfrp;
      pbtfrp.dwSize = 4;
      IntPtr firstRadio = NativeMethods.BluetoothFindFirstRadio(ref pbtfrp, ref zero1);
      if (firstRadio != IntPtr.Zero)
        NativeMethods.BluetoothFindRadioClose(firstRadio);
      return zero1 != IntPtr.Zero ? (IBluetoothRadio) new WindowsBluetoothRadio(zero1) : throw new PlatformNotSupportedException("No Radio.");
    }

    internal static IBluetoothRadio[] AllRadios
    {
      get
      {
        IntPtr zero1 = IntPtr.Zero;
        IntPtr zero2 = IntPtr.Zero;
        BLUETOOTH_FIND_RADIO_PARAMS pbtfrp;
        pbtfrp.dwSize = 4;
        List<IntPtr> numList = new List<IntPtr>();
        IntPtr firstRadio = NativeMethods.BluetoothFindFirstRadio(ref pbtfrp, ref zero1);
        if (firstRadio != IntPtr.Zero)
        {
          numList.Add(zero1);
          while (NativeMethods.BluetoothFindNextRadio(firstRadio, ref zero1))
            numList.Add(zero1);
          NativeMethods.BluetoothFindRadioClose(firstRadio);
        }
        IBluetoothRadio[] allRadios = new IBluetoothRadio[numList.Count];
        for (int index = 0; index < allRadios.Length; ++index)
          allRadios[index] = (IBluetoothRadio) new WindowsBluetoothRadio(numList[index]);
        return allRadios;
      }
    }

    public IntPtr Handle => this.handle;

    public HardwareStatus HardwareStatus
    {
      get
      {
        return NativeMethods.BluetoothGetRadioInfo(this.handle, ref new BLUETOOTH_RADIO_INFO()
        {
          dwSize = 520
        }) != 0 ? HardwareStatus.NotPresent : HardwareStatus.Running;
      }
    }

    public RadioMode Mode
    {
      get
      {
        if (NativeMethods.BluetoothIsDiscoverable(this.handle))
          return RadioMode.Discoverable;
        return NativeMethods.BluetoothIsConnectable(this.handle) ? RadioMode.Connectable : RadioMode.PowerOff;
      }
      set
      {
        switch (value)
        {
          case RadioMode.PowerOff:
            if (this.Mode == RadioMode.Discoverable)
            {
              bool? nullable1 = new bool?(NativeMethods.BluetoothEnableDiscovery(this.handle, false));
            }
            bool? nullable2 = new bool?(NativeMethods.BluetoothEnableIncomingConnections(this.handle, false));
            break;
          case RadioMode.Connectable:
            if (this.Mode == RadioMode.Discoverable)
            {
              bool? nullable3 = new bool?(NativeMethods.BluetoothEnableDiscovery(this.handle, false));
              break;
            }
            bool? nullable4 = new bool?(NativeMethods.BluetoothEnableIncomingConnections(this.handle, true));
            break;
          case RadioMode.Discoverable:
            if (this.Mode == RadioMode.PowerOff)
            {
              bool? nullable5 = new bool?(NativeMethods.BluetoothEnableIncomingConnections(this.handle, true));
            }
            bool? nullable6 = new bool?(NativeMethods.BluetoothEnableDiscovery(this.handle, true));
            break;
        }
      }
    }

    public BluetoothAddress LocalAddress => new BluetoothAddress(this.radio.address);

    public string Name
    {
      get => this.radio.szName;
      set
      {
        string radioRegKey = WindowsBluetoothRadio.GetRadioRegKey(this.LocalAddress);
        if (radioRegKey == null)
          return;
        string name = string.Format("SYSTEM\\CurrentControlSet\\Enum\\{0}\\Device Parameters", (object) radioRegKey);
        new RegistryPermission(PermissionState.Unrestricted).Demand();
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, true);
        if (registryKey == null)
          return;
        if (value == null)
          value = Environment.MachineName;
        registryKey.SetValue("Local Name", (object) new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(value))
        {
          (byte) 0
        }.ToArray(), RegistryValueKind.Binary);
        registryKey.Close();
        uint dwIoControlCode = 6 > Environment.OSVersion.Version.Major ? 2232276U : 4263944U;
        long InBuffer = 4;
        int pBytesReturned = 0;
        if (!NativeMethods.DeviceIoControl(this.Handle, dwIoControlCode, ref InBuffer, 4, IntPtr.Zero, 0, out pBytesReturned, IntPtr.Zero))
          throw new Win32Exception();
      }
    }

    private static string GetRadioRegKey(BluetoothAddress radioAddress)
    {
      Guid devclassBluetooth = NativeMethods.GUID_DEVCLASS_BLUETOOTH;
      IntPtr classDevs = NativeMethods.SetupDiGetClassDevs(ref devclassBluetooth, (string) null, IntPtr.Zero, NativeMethods.DIGCF.PRESENT | NativeMethods.DIGCF.PROFILE);
      if (IntPtr.Zero.Equals((object) classDevs))
        throw new Win32Exception();
      try
      {
        NativeMethods.SP_DEVINFO_DATA deviceInfoData = new NativeMethods.SP_DEVINFO_DATA();
        deviceInfoData.cbSize = Marshal.SizeOf((object) deviceInfoData);
        uint num1 = 0;
        while (NativeMethods.SetupDiEnumDeviceInfo(classDevs, num1++, ref deviceInfoData))
        {
          string radioRegKey = WindowsBluetoothRadio.DevInstanceId(classDevs, deviceInfoData);
          int num2 = radioRegKey.LastIndexOf("\\");
          BluetoothAddress result;
          BluetoothAddress.TryParse(radioRegKey.Substring(num2 + 1, radioRegKey.Length - num2 - 1), out result);
          if (result != (BluetoothAddress) null && radioAddress == result)
            return radioRegKey;
        }
      }
      finally
      {
        NativeMethods.SetupDiDestroyDeviceInfoList(classDevs);
      }
      return (string) null;
    }

    private static string DevInstanceId(IntPtr hDevInfo, NativeMethods.SP_DEVINFO_DATA data)
    {
      Win32Error error = Win32Error.ERROR_SUCCESS;
      int requiredSize = 0;
      StringBuilder deviceInstanceId = (StringBuilder) null;
      for (int index = 0; index < 2; ++index)
      {
        error = Win32Error.ERROR_SUCCESS;
        if (!NativeMethods.SetupDiGetDeviceInstanceId(hDevInfo, ref data, deviceInstanceId, requiredSize, out requiredSize))
        {
          error = (Win32Error) Marshal.GetLastWin32Error();
          if (error == Win32Error.ERROR_INSUFFICIENT_BUFFER)
            deviceInstanceId = new StringBuilder(requiredSize + 2);
          else
            break;
        }
      }
      if (error != Win32Error.ERROR_SUCCESS)
        throw new Win32Exception((int) error);
      return deviceInstanceId.ToString();
    }

    public ClassOfDevice ClassOfDevice => new ClassOfDevice(this.radio.ulClassofDevice);

    public Manufacturer Manufacturer => this.radio.manufacturer;

    LmpVersion IBluetoothRadio.LmpVersion => this._lmpV;

    public int LmpSubversion => (int) this.radio.lmpSubversion;

    HciVersion IBluetoothRadio.HciVersion => this._hciV;

    int IBluetoothRadio.HciRevision => this._hciRev;

    public Manufacturer SoftwareManufacturer => Manufacturer.Microsoft;

    [Flags]
    private enum LOCAL_FLAGS
    {
      NonDiscoverableNonConnectable = 0,
      Discoverable = 1,
      Connectable = 2,
    }
  }
}
