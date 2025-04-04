// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothRadio
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Diagnostics;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBluetoothRadio : IBluetoothRadio
  {
    private readonly WidcommBluetoothFactoryBase _factory;
    private readonly DEV_VER_INFO m_dvi;
    private readonly string m_name;

    internal WidcommBluetoothRadio(WidcommBluetoothFactoryBase factory)
    {
      ThreadStart threadStart1 = (ThreadStart) (() => { });
      ThreadStart threadStart2 = (ThreadStart) (() =>
      {
        throw new PlatformNotSupportedException("Widcomm Bluetooth stack not supported (Radio).");
      });
      this._factory = factory;
      WidcommBtInterface btIf = this.BtIf;
      if (!this.BtIf.GetLocalDeviceVersionInfo(ref this.m_dvi))
      {
        threadStart2();
        this.m_dvi = new DEV_VER_INFO(HciVersion.Unknown);
      }
      byte[] numArray = new byte[248];
      if (this.BtIf.GetLocalDeviceName(numArray))
        this.m_name = WidcommUtils.BdNameToString(numArray);
      else
        threadStart2();
      if (!(this.LocalAddress == (BluetoothAddress) null) && this.LocalAddress.ToInt64() != 0L)
        return;
      MiscUtils.Trace_WriteLine("GetLocalDeviceVersionInfo's bd_addr is empty, trying GetLocalDeviceInfoBdAddr...");
      if (this.m_dvi.bd_addr == null)
        this.m_dvi.bd_addr = new byte[6];
      if (this.BtIf.GetLocalDeviceInfoBdAddr(this.m_dvi.bd_addr))
        return;
      this.m_dvi.bd_addr = new byte[6];
      threadStart2();
    }

    private WidcommBtInterface BtIf
    {
      [DebuggerStepThrough] get => this._factory.GetWidcommBtInterface();
    }

    public string Remote => (string) null;

    public ClassOfDevice ClassOfDevice => new ClassOfDevice(0U);

    public IntPtr Handle => throw new NotSupportedException("WidcommBluetoothRadio.Handle");

    public HardwareStatus HardwareStatus
    {
      get
      {
        bool stackServerUp;
        bool deviceReady;
        this.BtIf.IsStackUpAndRadioReady(out stackServerUp, out deviceReady);
        return !stackServerUp || !deviceReady ? HardwareStatus.Shutdown : HardwareStatus.Running;
      }
    }

    LmpVersion IBluetoothRadio.LmpVersion => (LmpVersion) this.m_dvi.lmp_version;

    public int LmpSubversion => (int) this.m_dvi.lmp_sub_version;

    HciVersion IBluetoothRadio.HciVersion => (HciVersion) this.m_dvi.hci_version;

    public int HciRevision => (int) this.m_dvi.hci_revision;

    public BluetoothAddress LocalAddress
    {
      get
      {
        return this.m_dvi.bd_addr == null ? (BluetoothAddress) null : WidcommUtils.ToBluetoothAddress(this.m_dvi.bd_addr);
      }
    }

    public Manufacturer Manufacturer => (Manufacturer) this.m_dvi.manufacturer;

    public RadioMode Mode
    {
      get
      {
        if (this.HardwareStatus != HardwareStatus.Running)
          return RadioMode.PowerOff;
        bool conno;
        bool disco;
        this.BtIf.IsDeviceConnectableDiscoverable(out conno, out disco);
        if (disco)
          return RadioMode.Discoverable;
        return conno ? RadioMode.Connectable : RadioMode.PowerOff;
      }
      set
      {
        bool forPairedOnly = false;
        bool connectable;
        bool discoverable;
        switch (value)
        {
          case RadioMode.PowerOff:
            connectable = false;
            discoverable = false;
            break;
          case RadioMode.Connectable:
            connectable = true;
            discoverable = false;
            break;
          case RadioMode.Discoverable:
            connectable = true;
            discoverable = true;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (value));
        }
        this.BtIf.SetDeviceConnectableDiscoverable(connectable, forPairedOnly, discoverable);
      }
    }

    public string Name
    {
      get => this.m_name;
      set => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public Manufacturer SoftwareManufacturer => Manufacturer.Broadcom;
  }
}
