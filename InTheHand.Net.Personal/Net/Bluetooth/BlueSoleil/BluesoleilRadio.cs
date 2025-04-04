// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BluesoleilRadio
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class BluesoleilRadio : IBluetoothRadio
  {
    private readonly BluesoleilFactory _fcty;
    private readonly BluetoothAddress _addr;
    private readonly string _name;
    private readonly ClassOfDevice _cod;
    private readonly Structs.BtSdkLocalLMPInfoStru _lmp;

    internal BluesoleilRadio(BluesoleilFactory fcty)
    {
      this._fcty = fcty;
      this._fcty.SdkInit();
      byte[] numArray1 = new byte[6];
      BtSdkError localDeviceAddress;
      BtSdkError btSdkError = localDeviceAddress = this._fcty.Api.Btsdk_GetLocalDeviceAddress(numArray1);
      BluesoleilUtils.Assert(localDeviceAddress, "Btsdk_GetLocalDeviceAddress");
      this._addr = btSdkError != BtSdkError.OK ? BluetoothAddress.None : BluesoleilUtils.ToBluetoothAddress(numArray1);
      byte[] numArray2 = new byte[500];
      ushort length = checked ((ushort) numArray2.Length);
      BtSdkError localName = this._fcty.Api.Btsdk_GetLocalName(numArray2, ref length);
      if (localDeviceAddress == BtSdkError.OK)
        BluesoleilUtils.Assert(localName, "Btsdk_GetLocalName");
      this._name = localName != BtSdkError.OK ? string.Empty : BluesoleilUtils.FromNameString(numArray2, new ushort?(length));
      uint device_class;
      this._fcty.Api.Btsdk_GetLocalDeviceClass(out device_class);
      this._cod = new ClassOfDevice(device_class);
      this._lmp = new Structs.BtSdkLocalLMPInfoStru(HciVersion.Unknown);
      BtSdkError localLmpInfo = this._fcty.Api.Btsdk_GetLocalLMPInfo(ref this._lmp);
      if (localDeviceAddress != BtSdkError.OK)
        return;
      BluesoleilUtils.Assert(localLmpInfo, "Btsdk_GetLocalLMPInfo");
    }

    public BluetoothAddress LocalAddress => this._addr;

    public string Name
    {
      get => this._name;
      set => throw new NotImplementedException();
    }

    public RadioMode Mode
    {
      get
      {
        StackConsts.DiscoveryMode mode;
        if (this.HardwareStatus != HardwareStatus.Running || this._fcty.Api.Btsdk_GetDiscoveryMode(out mode) != BtSdkError.OK || (mode & StackConsts.DiscoveryMode.BTSDK_CONNECTABLE) != StackConsts.DiscoveryMode.BTSDK_CONNECTABLE)
          return RadioMode.PowerOff;
        return (mode & StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE) == StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE ? RadioMode.Discoverable : RadioMode.Connectable;
      }
      set
      {
        if (value == RadioMode.PowerOff)
        {
          BluesoleilUtils.Assert(this._fcty.Api.Btsdk_StopBluetooth(), "Radio.set_Mode Stop");
        }
        else
        {
          BluesoleilUtils.Assert(this._fcty.Api.Btsdk_StartBluetooth(), "Radio.set_Mode Start");
          StackConsts.DiscoveryMode mode;
          BtSdkError discoveryMode1 = this._fcty.Api.Btsdk_GetDiscoveryMode(out mode);
          BluesoleilUtils.Assert(discoveryMode1, "Radio.set_Mode Get");
          if (discoveryMode1 != BtSdkError.OK)
            mode = StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE | StackConsts.DiscoveryMode.BTSDK_CONNECTABLE | StackConsts.DiscoveryMode.BTSDK_PAIRABLE;
          StackConsts.DiscoveryMode discoveryMode2 = mode | StackConsts.DiscoveryMode.BTSDK_CONNECTABLE;
          BluesoleilUtils.Assert(this._fcty.Api.Btsdk_SetDiscoveryMode((value & RadioMode.Discoverable) != RadioMode.Discoverable ? discoveryMode2 & ~StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE : discoveryMode2 | StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE), "Radio.set_Mode Set");
        }
      }
    }

    public ClassOfDevice ClassOfDevice => this._cod;

    public Manufacturer SoftwareManufacturer => Manufacturer.IvtBlueSoleilXxxx;

    public IntPtr Handle
    {
      [DebuggerNonUserCode] get => throw new NotSupportedException();
    }

    public string Remote => (string) null;

    public HardwareStatus HardwareStatus
    {
      get
      {
        if (!this._fcty.Api.Btsdk_IsBluetoothHardwareExisted())
          return HardwareStatus.NotPresent;
        return !this._fcty.Api.Btsdk_IsBluetoothReady() ? HardwareStatus.Shutdown : HardwareStatus.Running;
      }
    }

    public LmpVersion LmpVersion => (LmpVersion) this._lmp.lmp_version;

    public int LmpSubversion => (int) this._lmp.lmp_subversion;

    public HciVersion HciVersion => (HciVersion) this._lmp.hci_version;

    public int HciRevision => (int) this._lmp.hci_revision;

    public Manufacturer Manufacturer => BluesoleilUtils.FromManufName(this._lmp.manuf_name);
  }
}
