// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.IBluesoleilApi
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal interface IBluesoleilApi
  {
    BtSdkError Btsdk_Init();

    BtSdkError Btsdk_Done();

    bool Btsdk_IsSDKInitialized();

    bool Btsdk_IsServerConnected();

    BtSdkError Btsdk_RegisterGetStatusInfoCB4ThirdParty(
      ref NativeMethods.Func_ReceiveBluetoothStatusInfo statusCBK);

    BtSdkError Btsdk_RegisterCallback4ThirdParty(ref Structs.BtSdkCallbackStru call_back);

    BtSdkError Btsdk_SetStatusInfoFlag(ushort usMsgType);

    void Btsdk_FreeMemory(IntPtr memblock);

    BtSdkError Btsdk_StartBluetooth();

    BtSdkError Btsdk_StopBluetooth();

    bool Btsdk_IsBluetoothReady();

    bool Btsdk_IsBluetoothHardwareExisted();

    BtSdkError Btsdk_SetDiscoveryMode(StackConsts.DiscoveryMode mode);

    BtSdkError Btsdk_GetDiscoveryMode(out StackConsts.DiscoveryMode mode);

    BtSdkError Btsdk_GetLocalDeviceAddress(byte[] bd_addr);

    BtSdkError Btsdk_GetLocalName(byte[] name, ref ushort len);

    BtSdkError Btsdk_GetLocalDeviceClass(out uint device_class);

    BtSdkError Btsdk_GetLocalLMPInfo(ref Structs.BtSdkLocalLMPInfoStru lmp_info);

    BtSdkError Btsdk_StartDeviceDiscovery(uint device_class, ushort max_num, ushort max_seconds);

    BtSdkError Btsdk_StopDeviceDiscovery();

    BtSdkError Btsdk_UpdateRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen);

    BtSdkError Btsdk_IsDevicePaired(uint dev_hdl, out bool pis_paired);

    BtSdkError Btsdk_PairDevice(uint dev_hdl);

    BtSdkError Btsdk_PinCodeReply(uint dev_hdl, byte[] pin_code, ushort size);

    bool Btsdk_IsDeviceConnected(uint dev_hdl);

    BtSdkError Btsdk_GetRemoteRSSI(uint device_handle, out sbyte prssi);

    BtSdkError Btsdk_GetRemoteLinkQuality(uint device_handle, out ushort plink_quality);

    uint Btsdk_GetRemoteDeviceHandle(byte[] bd_addr);

    uint Btsdk_AddRemoteDevice(byte[] bd_addr);

    BtSdkError Btsdk_DeleteRemoteDeviceByHandle(uint dev_hdl);

    int Btsdk_GetStoredDevicesByClass(uint dev_class, uint[] pdev_hdl, int max_dev_num);

    BtSdkError Btsdk_GetRemoteDeviceAddress(uint dev_hdl, byte[] bd_addr);

    BtSdkError Btsdk_GetRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen);

    BtSdkError Btsdk_GetRemoteDeviceClass(uint dev_hdl, out uint pdevice_class);

    BtSdkError Btsdk_GetRemoteDeviceProperty(
      uint dev_hdl,
      out Structs.BtSdkRemoteDevicePropertyStru rmt_dev_prop);

    BtSdkError Btsdk_BrowseRemoteServicesEx(
      uint dev_hdl,
      Structs.BtSdkSDPSearchPatternStru[] psch_ptn,
      int ptn_num,
      uint[] svc_hdl,
      ref int svc_count);

    BtSdkError Btsdk_GetRemoteServiceAttributes(
      uint svc_hdl,
      ref Structs.BtSdkRemoteServiceAttrStru attribute);

    BtSdkError Btsdk_ConnectEx(uint dev_hdl, ushort service_class, uint lParam, out uint conn_hdl);

    BtSdkError Btsdk_ConnectEx(
      uint dev_hdl,
      ushort service_class,
      ref Structs.BtSdkSPPConnParamStru lParam,
      out uint conn_hdl);

    uint Btsdk_StartEnumConnection();

    uint Btsdk_EnumConnection(uint enum_handle, ref Structs.BtSdkConnectionPropertyStru conn_prop);

    BtSdkError Btsdk_EndEnumConnection(uint enum_handle);

    BtSdkError Btsdk_Disconnect(uint handle);

    BtSdkError Btsdk_InitCommObj(byte com_idx, ushort svc_class);

    BtSdkError Btsdk_DeinitCommObj(byte com_idx);

    short Btsdk_GetClientPort(uint conn_hdl);

    BtSdkError Btsdk_SearchAppExtSPPService(uint dev_hdl, ref Structs.BtSdkAppExtSPPAttrStru psvc);

    BtSdkError Btsdk_ConnectAppExtSPPService(
      uint dev_hdl,
      ref Structs.BtSdkAppExtSPPAttrStru psvc,
      out uint conn_hdl);

    uint Btsdk_CommNumToSerialNum(int comportNum);

    void Btsdk_PlugOutVComm(uint serialNum, StackConsts.COMM_SET flag);

    bool Btsdk_PlugInVComm(
      uint serialNum,
      out uint comportNumber,
      uint usageType,
      StackConsts.COMM_SET flag,
      uint dwTimeout);

    uint Btsdk_GetASerialNum();
  }
}
