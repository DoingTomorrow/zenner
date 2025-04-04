// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommStBtIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal class WidcommStBtIf : IBtIf
  {
    private readonly IBtIf _child;
    private readonly WidcommPortSingleThreader _st;

    public WidcommStBtIf(WidcommBluetoothFactoryBase factory, IBtIf child)
    {
      this._st = factory.GetSingleThreader();
      this._child = child;
    }

    public void SetParent(WidcommBtInterface parent) => this._child.SetParent(parent);

    public void Create()
    {
      this._st.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => this._child.Create()))).WaitCompletion();
    }

    public void Destroy(bool disposing)
    {
      if (!disposing)
        return;
      if (!WidcommBtInterface.IsWidcommSingleThread(this._st))
        this._st.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => this._child.Destroy(disposing)))).WaitCompletion();
      else
        this._child.Destroy(disposing);
    }

    public bool StartInquiry()
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.StartInquiry()))).WaitCompletion();
    }

    public void StopInquiry()
    {
      this._st.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => this._child.StopInquiry()))).WaitCompletion();
    }

    public bool StartDiscovery(BluetoothAddress address, Guid serviceGuid)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.StartDiscovery(address, serviceGuid)))).WaitCompletion();
    }

    public DISCOVERY_RESULT GetLastDiscoveryResult(
      out BluetoothAddress address,
      out ushort p_num_recs)
    {
      return this._child.GetLastDiscoveryResult(out address, out p_num_recs);
    }

    public ISdpDiscoveryRecordsBuffer ReadDiscoveryRecords(
      BluetoothAddress address,
      int maxRecords,
      ServiceDiscoveryParams args)
    {
      return this._child.ReadDiscoveryRecords(address, maxRecords, args);
    }

    public REM_DEV_INFO_RETURN_CODE GetRemoteDeviceInfo(
      ref REM_DEV_INFO remDevInfo,
      IntPtr p_rem_dev_info,
      int cb)
    {
      return this._child.GetRemoteDeviceInfo(ref remDevInfo, p_rem_dev_info, cb);
    }

    public REM_DEV_INFO_RETURN_CODE GetNextRemoteDeviceInfo(
      ref REM_DEV_INFO remDevInfo,
      IntPtr p_rem_dev_info,
      int cb)
    {
      return this._child.GetNextRemoteDeviceInfo(ref remDevInfo, p_rem_dev_info, cb);
    }

    public bool GetLocalDeviceVersionInfo(ref DEV_VER_INFO devVerInfo)
    {
      return this._child.GetLocalDeviceVersionInfo(ref devVerInfo);
    }

    public bool GetLocalDeviceInfoBdAddr(byte[] bdAddr)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.GetLocalDeviceInfoBdAddr(bdAddr)))).WaitCompletion();
    }

    public bool GetLocalDeviceName(byte[] bdName)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.GetLocalDeviceName(bdName)))).WaitCompletion();
    }

    public void IsStackUpAndRadioReady(out bool stackServerUp, out bool deviceReady)
    {
      WidcommStBtIf.HoldStackDevice holdStackDevice1 = this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<WidcommStBtIf.HoldStackDevice>>(new WidcommPortSingleThreader.MiscReturnCommand<WidcommStBtIf.HoldStackDevice>((Func<WidcommStBtIf.HoldStackDevice>) (() =>
      {
        WidcommStBtIf.HoldStackDevice holdStackDevice2 = new WidcommStBtIf.HoldStackDevice();
        this._child.IsStackUpAndRadioReady(out holdStackDevice2.stackServerUp, out holdStackDevice2.deviceReady);
        return holdStackDevice2;
      }))).WaitCompletion();
      stackServerUp = holdStackDevice1.stackServerUp;
      deviceReady = holdStackDevice1.deviceReady;
    }

    public void IsDeviceConnectableDiscoverable(out bool conno, out bool disco)
    {
      WidcommStBtIf.HoldConnoDisco holdConnoDisco1 = this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<WidcommStBtIf.HoldConnoDisco>>(new WidcommPortSingleThreader.MiscReturnCommand<WidcommStBtIf.HoldConnoDisco>((Func<WidcommStBtIf.HoldConnoDisco>) (() =>
      {
        WidcommStBtIf.HoldConnoDisco holdConnoDisco2 = new WidcommStBtIf.HoldConnoDisco();
        this._child.IsDeviceConnectableDiscoverable(out holdConnoDisco2.conno, out holdConnoDisco2.disco);
        return holdConnoDisco2;
      }))).WaitCompletion();
      conno = holdConnoDisco1.conno;
      disco = holdConnoDisco1.disco;
    }

    public void SetDeviceConnectableDiscoverable(
      bool connectable,
      bool forPairedOnly,
      bool discoverable)
    {
      this._child.SetDeviceConnectableDiscoverable(connectable, forPairedOnly, discoverable);
    }

    public int GetRssi(byte[] bd_addr)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<int>>(new WidcommPortSingleThreader.MiscReturnCommand<int>((Func<int>) (() => this._child.GetRssi(bd_addr)))).WaitCompletion();
    }

    public bool BondQuery(byte[] bd_addr)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.BondQuery(bd_addr)))).WaitCompletion();
    }

    public BOND_RETURN_CODE Bond(BluetoothAddress address, string passphrase)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<BOND_RETURN_CODE>>(new WidcommPortSingleThreader.MiscReturnCommand<BOND_RETURN_CODE>((Func<BOND_RETURN_CODE>) (() => this._child.Bond(address, passphrase)))).WaitCompletion();
    }

    public bool UnBond(BluetoothAddress address)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.UnBond(address)))).WaitCompletion();
    }

    public WBtRc GetExtendedError()
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<WBtRc>>(new WidcommPortSingleThreader.MiscReturnCommand<WBtRc>((Func<WBtRc>) (() => this._child.GetExtendedError()))).WaitCompletion();
    }

    public SDK_RETURN_CODE IsRemoteDevicePresent(byte[] bd_addr)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<SDK_RETURN_CODE>>(new WidcommPortSingleThreader.MiscReturnCommand<SDK_RETURN_CODE>((Func<SDK_RETURN_CODE>) (() => this._child.IsRemoteDevicePresent(bd_addr)))).WaitCompletion();
    }

    public bool IsRemoteDeviceConnected(byte[] bd_addr)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.IsRemoteDeviceConnected(bd_addr)))).WaitCompletion();
    }

    private class HoldStackDevice
    {
      public bool stackServerUp;
      public bool deviceReady;
    }

    private class HoldConnoDisco
    {
      public bool conno;
      public bool disco;
    }
  }
}
