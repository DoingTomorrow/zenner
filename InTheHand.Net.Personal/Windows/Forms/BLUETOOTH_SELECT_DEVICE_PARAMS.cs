// Decompiled with JetBrains decompiler
// Type: InTheHand.Windows.Forms.BLUETOOTH_SELECT_DEVICE_PARAMS
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Msft;
using InTheHand.Net.Sockets;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Windows.Forms
{
  internal struct BLUETOOTH_SELECT_DEVICE_PARAMS : ISelectBluetoothDevice
  {
    private int dwSize;
    private int cNumOfClasses;
    private IntPtr prgClassOfDevices;
    [MarshalAs(UnmanagedType.LPWStr)]
    private string pszInfo;
    public IntPtr hwndParent;
    [MarshalAs(UnmanagedType.Bool)]
    private bool fForceAuthentication;
    [MarshalAs(UnmanagedType.Bool)]
    private bool fShowAuthenticated;
    [MarshalAs(UnmanagedType.Bool)]
    private bool fShowRemembered;
    [MarshalAs(UnmanagedType.Bool)]
    private bool fShowUnknown;
    [MarshalAs(UnmanagedType.Bool)]
    private bool fAddNewDeviceWizard;
    [MarshalAs(UnmanagedType.Bool)]
    private bool fSkipServicesPage;
    private SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK pfnDeviceCallback;
    private IntPtr pvParam;
    public int cNumDevices;
    public IntPtr pDevices;

    public void Reset()
    {
      this.dwSize = Marshal.SizeOf((object) this);
      this.cNumOfClasses = 0;
      this.prgClassOfDevices = IntPtr.Zero;
      this.pszInfo = (string) null;
      this.hwndParent = IntPtr.Zero;
      this.fForceAuthentication = false;
      this.fShowAuthenticated = true;
      this.fShowRemembered = true;
      this.fShowUnknown = true;
      this.fAddNewDeviceWizard = false;
      this.fSkipServicesPage = false;
      this.pfnDeviceCallback = (SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK) null;
      this.pvParam = IntPtr.Zero;
      this.cNumDevices = 0;
      this.pDevices = IntPtr.Zero;
    }

    public void SetClassOfDevices(ClassOfDevice[] classOfDevices)
    {
      if (this.prgClassOfDevices != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(this.prgClassOfDevices);
        this.prgClassOfDevices = IntPtr.Zero;
      }
      if (classOfDevices.Length == 0)
      {
        this.cNumOfClasses = 0;
        this.prgClassOfDevices = IntPtr.Zero;
      }
      else
      {
        this.cNumOfClasses = classOfDevices.Length;
        this.prgClassOfDevices = Marshal.AllocHGlobal(8 * classOfDevices.Length);
        for (int index = 0; index < this.cNumOfClasses; ++index)
          Marshal.WriteInt32(this.prgClassOfDevices, index * 8, (int) classOfDevices[index].Value);
      }
    }

    public void SetFilter(
      Predicate<BluetoothDeviceInfo> filterFn,
      SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK msftFilterFn)
    {
      this.pfnDeviceCallback = msftFilterFn;
    }

    public BLUETOOTH_DEVICE_INFO[] Devices
    {
      get
      {
        if (this.cNumDevices <= 0)
          return new BLUETOOTH_DEVICE_INFO[0];
        BLUETOOTH_DEVICE_INFO[] devices = new BLUETOOTH_DEVICE_INFO[this.cNumDevices];
        for (int index = 0; index < this.cNumDevices; ++index)
          devices[index] = (BLUETOOTH_DEVICE_INFO) Marshal.PtrToStructure(new IntPtr(this.pDevices.ToInt64() + (long) (index * Marshal.SizeOf(typeof (BLUETOOTH_DEVICE_INFO)))), typeof (BLUETOOTH_DEVICE_INFO));
        return devices;
      }
    }

    public BLUETOOTH_DEVICE_INFO Device
    {
      get
      {
        return this.cNumDevices > 0 ? (BLUETOOTH_DEVICE_INFO) Marshal.PtrToStructure(this.pDevices, typeof (BLUETOOTH_DEVICE_INFO)) : new BLUETOOTH_DEVICE_INFO();
      }
    }

    bool ISelectBluetoothDevice.ShowAuthenticated
    {
      get => this.fShowAuthenticated;
      set => this.fShowAuthenticated = value;
    }

    bool ISelectBluetoothDevice.ShowRemembered
    {
      get => this.fShowRemembered;
      set => this.fShowRemembered = value;
    }

    bool ISelectBluetoothDevice.ShowUnknown
    {
      get => this.fShowUnknown;
      set => this.fShowUnknown = value;
    }

    bool ISelectBluetoothDevice.ShowDiscoverableOnly
    {
      get => false;
      set
      {
        if (!value)
          return;
        Trace.WriteLine("ShowDiscoverableOnly is not supported by the Microsoft stack on desktop Windows.");
      }
    }

    bool ISelectBluetoothDevice.ForceAuthentication
    {
      get => this.fForceAuthentication;
      set => this.fForceAuthentication = value;
    }

    string ISelectBluetoothDevice.Info
    {
      get => this.pszInfo;
      set => this.pszInfo = value;
    }

    bool ISelectBluetoothDevice.AddNewDeviceWizard
    {
      get => this.fAddNewDeviceWizard;
      set => this.fAddNewDeviceWizard = value;
    }

    bool ISelectBluetoothDevice.SkipServicesPage
    {
      get => this.fSkipServicesPage;
      set => this.fSkipServicesPage = value;
    }
  }
}
