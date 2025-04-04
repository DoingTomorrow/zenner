// Decompiled with JetBrains decompiler
// Type: InTheHand.Windows.Forms.SelectBluetoothDeviceDialog
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Msft;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace InTheHand.Windows.Forms
{
  public class SelectBluetoothDeviceDialog : CommonDialog
  {
    private BLUETOOTH_SELECT_DEVICE_PARAMS dialogWin32;
    private readonly SelectBluetoothDeviceForm dialogCustom;
    private readonly ISelectBluetoothDevice dialog;
    private BluetoothDeviceInfo device;
    private bool _ShowDiscoverableOnly;
    private Predicate<BluetoothDeviceInfo> _deviceFilter;
    private readonly SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK _msftFilterProxy;

    public SelectBluetoothDeviceDialog()
      : this(false)
    {
    }

    internal SelectBluetoothDeviceDialog(bool forceCustomDialog)
    {
      BluetoothRadio primaryRadio;
      if (forceCustomDialog || (primaryRadio = BluetoothRadio.PrimaryRadio) != null && primaryRadio.SoftwareManufacturer != Manufacturer.Microsoft)
      {
        this.dialogCustom = new SelectBluetoothDeviceForm();
        this.dialog = (ISelectBluetoothDevice) this.dialogCustom;
      }
      else
      {
        this._msftFilterProxy = new SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK(this.MsftFilterProxy);
        this.dialogWin32 = new BLUETOOTH_SELECT_DEVICE_PARAMS();
        this.dialogWin32.Reset();
        this.dialog = (ISelectBluetoothDevice) this.dialogWin32;
      }
      this.ClassOfDevices = new List<ClassOfDevice>();
    }

    public override void Reset()
    {
      this.DeviceFilter = (Predicate<BluetoothDeviceInfo>) null;
      this.dialog.Reset();
    }

    private DialogResult ShowCustomDialog()
    {
      this.dialogCustom.SetClassOfDevices(this.ClassOfDevices.ToArray());
      DialogResult dialogResult = this.dialogCustom.ShowDialog();
      this.device = this.dialogCustom.selectedDevice;
      return dialogResult;
    }

    protected override bool RunDialog(IntPtr hwndOwner)
    {
      return this.dialogCustom == null ? this.RunDialogMsft(hwndOwner) : this.ShowCustomDialog() == DialogResult.OK;
    }

    private bool RunDialogMsft(IntPtr hwndOwner)
    {
      this.AssertWindowsFormsThread();
      if (this._ShowDiscoverableOnly)
        return false;
      if ((ValueType) this.dialogWin32 != this.dialog)
        this.dialogWin32 = (BLUETOOTH_SELECT_DEVICE_PARAMS) this.dialog;
      this.dialogWin32.hwndParent = hwndOwner;
      this.dialogWin32.SetClassOfDevices(this.ClassOfDevices.ToArray());
      bool flag = NativeMethods.BluetoothSelectDevices(ref this.dialogWin32);
      if (flag)
      {
        if (this.dialogWin32.cNumDevices > 0)
          this.device = new BluetoothDeviceInfo((IBluetoothDeviceInfo) new WindowsBluetoothDeviceInfo(this.dialogWin32.Device));
        NativeMethods.BluetoothSelectDevicesFree(ref this.dialogWin32);
      }
      return flag;
    }

    private void AssertWindowsFormsThread()
    {
      if (Application.MessageLoop)
        return;
      Trace.WriteLine("WARNING: The SelectBluetoothDeviceDialog needs to be called on the UI thread.");
    }

    public bool AddNewDeviceWizard
    {
      get => this.dialog.AddNewDeviceWizard;
      set => this.dialog.AddNewDeviceWizard = value;
    }

    public bool SkipServicesPage
    {
      get => this.dialog.SkipServicesPage;
      set => this.dialog.SkipServicesPage = value;
    }

    public string Info
    {
      get => this.dialog.Info;
      set => this.dialog.Info = value;
    }

    public List<ClassOfDevice> ClassOfDevices { get; private set; }

    public BluetoothDeviceInfo SelectedDevice => this.device;

    public bool ShowAuthenticated
    {
      get => this.dialog.ShowAuthenticated;
      set => this.dialog.ShowAuthenticated = value;
    }

    public bool ShowRemembered
    {
      get => this.dialog.ShowRemembered;
      set => this.dialog.ShowRemembered = value;
    }

    public bool ShowUnknown
    {
      get => this.dialog.ShowUnknown;
      set => this.dialog.ShowUnknown = value;
    }

    public bool ForceAuthentication
    {
      get => this.dialog.ForceAuthentication;
      set => this.dialog.ForceAuthentication = value;
    }

    public bool ShowDiscoverableOnly
    {
      get => this.dialog.ShowDiscoverableOnly;
      set
      {
        this.dialog.ShowDiscoverableOnly = value;
        this._ShowDiscoverableOnly = value;
      }
    }

    [Obsolete("Please use the ShowDiscoverableOnly property.")]
    public bool DiscoverableOnly
    {
      get => this.ShowDiscoverableOnly;
      set => this.ShowDiscoverableOnly = value;
    }

    public Predicate<BluetoothDeviceInfo> DeviceFilter
    {
      get => this._deviceFilter;
      set
      {
        this._deviceFilter = value;
        this.dialog.SetFilter(value, this._msftFilterProxy);
      }
    }

    private bool MsftFilterProxy(IntPtr state, ref BLUETOOTH_DEVICE_INFO dev)
    {
      return this._deviceFilter == null || this._deviceFilter(new BluetoothDeviceInfo((IBluetoothDeviceInfo) new WindowsBluetoothDeviceInfo(dev)));
    }

    protected override void Dispose(bool disposing)
    {
      if (this.dialogCustom == null)
        this.dialogWin32.SetClassOfDevices(new ClassOfDevice[0]);
      base.Dispose(disposing);
    }

    internal delegate bool PFN_DEVICE_CALLBACK(IntPtr pvParam, ref BLUETOOTH_DEVICE_INFO pDevice);
  }
}
