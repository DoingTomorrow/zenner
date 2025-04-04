// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.RealBluesoleilApi
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class RealBluesoleilApi : IBluesoleilApi
  {
    BtSdkError IBluesoleilApi.Btsdk_Init() => NativeMethods.Btsdk_Init();

    bool IBluesoleilApi.Btsdk_IsSDKInitialized() => NativeMethods.Btsdk_IsSDKInitialized();

    bool IBluesoleilApi.Btsdk_IsServerConnected() => NativeMethods.Btsdk_IsServerConnected();

    BtSdkError IBluesoleilApi.Btsdk_Done() => NativeMethods.Btsdk_Done();

    BtSdkError IBluesoleilApi.Btsdk_RegisterGetStatusInfoCB4ThirdParty(
      ref NativeMethods.Func_ReceiveBluetoothStatusInfo statusCBK)
    {
      return NativeMethods.Btsdk_RegisterGetStatusInfoCB4ThirdParty(statusCBK);
    }

    BtSdkError IBluesoleilApi.Btsdk_RegisterCallback4ThirdParty(
      ref Structs.BtSdkCallbackStru call_back)
    {
      return NativeMethods.Btsdk_RegisterCallback4ThirdParty(ref call_back);
    }

    BtSdkError IBluesoleilApi.Btsdk_SetStatusInfoFlag(ushort usMsgType)
    {
      return NativeMethods.Btsdk_SetStatusInfoFlag(usMsgType);
    }

    void IBluesoleilApi.Btsdk_FreeMemory(IntPtr memblock)
    {
      NativeMethods.Btsdk_FreeMemory(memblock);
    }

    BtSdkError IBluesoleilApi.Btsdk_StartBluetooth() => NativeMethods.Btsdk_StartBluetooth();

    BtSdkError IBluesoleilApi.Btsdk_StopBluetooth() => NativeMethods.Btsdk_StopBluetooth();

    bool IBluesoleilApi.Btsdk_IsBluetoothReady() => NativeMethods.Btsdk_IsBluetoothReady();

    bool IBluesoleilApi.Btsdk_IsBluetoothHardwareExisted()
    {
      return NativeMethods.Btsdk_IsBluetoothHardwareExisted();
    }

    BtSdkError IBluesoleilApi.Btsdk_SetDiscoveryMode(StackConsts.DiscoveryMode mode)
    {
      return NativeMethods.Btsdk_SetDiscoveryMode(mode);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetDiscoveryMode(out StackConsts.DiscoveryMode mode)
    {
      return NativeMethods.Btsdk_GetDiscoveryMode(out mode);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetLocalDeviceAddress(byte[] bd_addr)
    {
      return NativeMethods.Btsdk_GetLocalDeviceAddress(bd_addr);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetLocalName(byte[] name, ref ushort len)
    {
      return NativeMethods.Btsdk_GetLocalName(name, ref len);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetLocalDeviceClass(out uint device_class)
    {
      return NativeMethods.Btsdk_GetLocalDeviceClass(out device_class);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetLocalLMPInfo(ref Structs.BtSdkLocalLMPInfoStru lmp_info)
    {
      return NativeMethods.Btsdk_GetLocalLMPInfo(ref lmp_info);
    }

    BtSdkError IBluesoleilApi.Btsdk_StartDeviceDiscovery(
      uint device_class,
      ushort max_num,
      ushort max_seconds)
    {
      return NativeMethods.Btsdk_StartDeviceDiscovery(device_class, max_num, max_seconds);
    }

    BtSdkError IBluesoleilApi.Btsdk_StopDeviceDiscovery()
    {
      return NativeMethods.Btsdk_StopDeviceDiscovery();
    }

    BtSdkError IBluesoleilApi.Btsdk_UpdateRemoteDeviceName(
      uint dev_hdl,
      byte[] name,
      ref ushort plen)
    {
      return NativeMethods.Btsdk_UpdateRemoteDeviceName(dev_hdl, name, ref plen);
    }

    BtSdkError IBluesoleilApi.Btsdk_IsDevicePaired(uint dev_hdl, out bool pis_paired)
    {
      return NativeMethods.Btsdk_IsDevicePaired(dev_hdl, out pis_paired);
    }

    BtSdkError IBluesoleilApi.Btsdk_PairDevice(uint dev_hdl)
    {
      return NativeMethods.Btsdk_PairDevice(dev_hdl);
    }

    BtSdkError IBluesoleilApi.Btsdk_PinCodeReply(uint dev_hdl, byte[] pin_code, ushort size)
    {
      return NativeMethods.Btsdk_PinCodeReply(dev_hdl, pin_code, size);
    }

    bool IBluesoleilApi.Btsdk_IsDeviceConnected(uint dev_hdl)
    {
      return NativeMethods.Btsdk_IsDeviceConnected(dev_hdl);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteRSSI(uint device_handle, out sbyte prssi)
    {
      return NativeMethods.Btsdk_GetRemoteRSSI(device_handle, out prssi);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteLinkQuality(
      uint device_handle,
      out ushort plink_quality)
    {
      return NativeMethods.Btsdk_GetRemoteLinkQuality(device_handle, out plink_quality);
    }

    uint IBluesoleilApi.Btsdk_GetRemoteDeviceHandle(byte[] bd_addr)
    {
      return NativeMethods.Btsdk_GetRemoteDeviceHandle(bd_addr);
    }

    uint IBluesoleilApi.Btsdk_AddRemoteDevice(byte[] bd_addr)
    {
      return NativeMethods.Btsdk_AddRemoteDevice(bd_addr);
    }

    BtSdkError IBluesoleilApi.Btsdk_DeleteRemoteDeviceByHandle(uint dev_hdl)
    {
      return NativeMethods.Btsdk_DeleteRemoteDeviceByHandle(dev_hdl);
    }

    int IBluesoleilApi.Btsdk_GetStoredDevicesByClass(
      uint dev_class,
      uint[] pdev_hdl,
      int max_dev_num)
    {
      return NativeMethods.Btsdk_GetStoredDevicesByClass(dev_class, pdev_hdl, max_dev_num);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceAddress(uint dev_hdl, byte[] bd_addr)
    {
      return NativeMethods.Btsdk_GetRemoteDeviceAddress(dev_hdl, bd_addr);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen)
    {
      return NativeMethods.Btsdk_GetRemoteDeviceName(dev_hdl, name, ref plen);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceClass(uint dev_hdl, out uint pdevice_class)
    {
      return NativeMethods.Btsdk_GetRemoteDeviceClass(dev_hdl, out pdevice_class);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceProperty(
      uint dev_hdl,
      out Structs.BtSdkRemoteDevicePropertyStru rmt_dev_prop)
    {
      return NativeMethods.Btsdk_GetRemoteDeviceProperty(dev_hdl, out rmt_dev_prop);
    }

    BtSdkError IBluesoleilApi.Btsdk_BrowseRemoteServicesEx(
      uint dev_hdl,
      Structs.BtSdkSDPSearchPatternStru[] psch_ptn,
      int ptn_num,
      uint[] svc_hdl,
      ref int svc_count)
    {
      return NativeMethods.Btsdk_BrowseRemoteServicesEx(dev_hdl, psch_ptn, ptn_num, svc_hdl, ref svc_count);
    }

    BtSdkError IBluesoleilApi.Btsdk_GetRemoteServiceAttributes(
      uint svc_hdl,
      ref Structs.BtSdkRemoteServiceAttrStru attribute)
    {
      return NativeMethods.Btsdk_GetRemoteServiceAttributes(svc_hdl, ref attribute);
    }

    BtSdkError IBluesoleilApi.Btsdk_ConnectEx(
      uint dev_hdl,
      ushort service_class,
      uint lParam,
      out uint conn_hdl)
    {
      return NativeMethods.Btsdk_ConnectEx(dev_hdl, service_class, lParam, out conn_hdl);
    }

    BtSdkError IBluesoleilApi.Btsdk_ConnectEx(
      uint dev_hdl,
      ushort service_class,
      ref Structs.BtSdkSPPConnParamStru lParam,
      out uint conn_hdl)
    {
      return NativeMethods.Btsdk_ConnectEx(dev_hdl, service_class, ref lParam, out conn_hdl);
    }

    uint IBluesoleilApi.Btsdk_StartEnumConnection() => NativeMethods.Btsdk_StartEnumConnection();

    uint IBluesoleilApi.Btsdk_EnumConnection(
      uint enum_handle,
      ref Structs.BtSdkConnectionPropertyStru conn_prop)
    {
      return NativeMethods.Btsdk_EnumConnection(enum_handle, ref conn_prop);
    }

    BtSdkError IBluesoleilApi.Btsdk_EndEnumConnection(uint enum_handle)
    {
      return NativeMethods.Btsdk_EndEnumConnection(enum_handle);
    }

    BtSdkError IBluesoleilApi.Btsdk_Disconnect(uint handle)
    {
      return NativeMethods.Btsdk_Disconnect(handle);
    }

    BtSdkError IBluesoleilApi.Btsdk_InitCommObj(byte com_idx, ushort svc_class)
    {
      return NativeMethods.Btsdk_InitCommObj(com_idx, svc_class);
    }

    BtSdkError IBluesoleilApi.Btsdk_DeinitCommObj(byte com_idx)
    {
      return NativeMethods.Btsdk_DeinitCommObj(com_idx);
    }

    short IBluesoleilApi.Btsdk_GetClientPort(uint conn_hdl)
    {
      return NativeMethods.Btsdk_GetClientPort(conn_hdl);
    }

    BtSdkError IBluesoleilApi.Btsdk_SearchAppExtSPPService(
      uint dev_hdl,
      ref Structs.BtSdkAppExtSPPAttrStru psvc)
    {
      return NativeMethods.Btsdk_SearchAppExtSPPService(dev_hdl, ref psvc);
    }

    BtSdkError IBluesoleilApi.Btsdk_ConnectAppExtSPPService(
      uint dev_hdl,
      ref Structs.BtSdkAppExtSPPAttrStru psvc,
      out uint conn_hdl)
    {
      return NativeMethods.Btsdk_ConnectAppExtSPPService(dev_hdl, ref psvc, out conn_hdl);
    }

    uint IBluesoleilApi.Btsdk_CommNumToSerialNum(int comportNum)
    {
      return NativeMethods.Btsdk_CommNumToSerialNum(comportNum);
    }

    void IBluesoleilApi.Btsdk_PlugOutVComm(uint serialNum, StackConsts.COMM_SET flag)
    {
      NativeMethods.Btsdk_PlugOutVComm(serialNum, flag);
    }

    bool IBluesoleilApi.Btsdk_PlugInVComm(
      uint serialNum,
      out uint comportNumber,
      uint usageType,
      StackConsts.COMM_SET flag,
      uint dwTimeout)
    {
      return NativeMethods.Btsdk_PlugInVComm(serialNum, out comportNumber, usageType, flag, dwTimeout);
    }

    uint IBluesoleilApi.Btsdk_GetASerialNum() => NativeMethods.Btsdk_GetASerialNum();
  }
}
