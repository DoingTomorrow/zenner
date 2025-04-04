// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.IBtIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal interface IBtIf
  {
    void SetParent(WidcommBtInterface parent);

    void Create();

    void Destroy(bool disposing);

    bool StartInquiry();

    void StopInquiry();

    bool StartDiscovery(BluetoothAddress address, Guid serviceGuid);

    DISCOVERY_RESULT GetLastDiscoveryResult(out BluetoothAddress address, out ushort p_num_recs);

    ISdpDiscoveryRecordsBuffer ReadDiscoveryRecords(
      BluetoothAddress address,
      int maxRecords,
      ServiceDiscoveryParams args);

    REM_DEV_INFO_RETURN_CODE GetRemoteDeviceInfo(
      ref REM_DEV_INFO remDevInfo,
      IntPtr p_rem_dev_info,
      int cb);

    REM_DEV_INFO_RETURN_CODE GetNextRemoteDeviceInfo(
      ref REM_DEV_INFO remDevInfo,
      IntPtr p_rem_dev_info,
      int cb);

    bool GetLocalDeviceVersionInfo(ref DEV_VER_INFO devVerInfo);

    bool GetLocalDeviceInfoBdAddr(byte[] bdAddr);

    bool GetLocalDeviceName(byte[] bdName);

    void IsStackUpAndRadioReady(out bool stackServerUp, out bool deviceReady);

    void IsDeviceConnectableDiscoverable(out bool conno, out bool disco);

    void SetDeviceConnectableDiscoverable(bool connectable, bool pairedOnly, bool discoverable);

    int GetRssi(byte[] bd_addr);

    bool BondQuery(byte[] bd_addr);

    BOND_RETURN_CODE Bond(BluetoothAddress address, string passphrase);

    bool UnBond(BluetoothAddress address);

    WBtRc GetExtendedError();

    SDK_RETURN_CODE IsRemoteDevicePresent(byte[] bd_addr);

    bool IsRemoteDeviceConnected(byte[] bd_addr);
  }
}
