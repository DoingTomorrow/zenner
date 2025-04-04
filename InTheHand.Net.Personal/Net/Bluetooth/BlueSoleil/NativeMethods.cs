// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.NativeMethods
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal static class NativeMethods
  {
    private const string DllName = "BsSDK";

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_Init();

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_Done();

    [DllImport("BsSDK")]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static extern bool Btsdk_IsSDKInitialized();

    [DllImport("BsSDK")]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static extern bool Btsdk_IsServerConnected();

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_RegisterGetStatusInfoCB4ThirdParty(
      NativeMethods.Func_ReceiveBluetoothStatusInfo statusCBK);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_RegisterCallback4ThirdParty(
      ref Structs.BtSdkCallbackStru call_back);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_SetStatusInfoFlag(ushort usMsgType);

    [DllImport("BsSDK")]
    internal static extern void Btsdk_FreeMemory(IntPtr memblock);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_StartBluetooth();

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_StopBluetooth();

    [DllImport("BsSDK")]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static extern bool Btsdk_IsBluetoothReady();

    [DllImport("BsSDK")]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static extern bool Btsdk_IsBluetoothHardwareExisted();

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_SetDiscoveryMode(StackConsts.DiscoveryMode mode);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetDiscoveryMode(out StackConsts.DiscoveryMode mode);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetLocalDeviceAddress(byte[] bd_addr);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetLocalName(byte[] name, ref ushort len);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetLocalDeviceClass(out uint device_class);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetLocalLMPInfo(
      ref Structs.BtSdkLocalLMPInfoStru lmp_info);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_StartDeviceDiscovery(
      uint device_class,
      ushort max_num,
      ushort max_seconds);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_StopDeviceDiscovery();

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_UpdateRemoteDeviceName(
      uint dev_hdl,
      byte[] name,
      ref ushort plen);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_IsDevicePaired(uint dev_hdl, [MarshalAs(UnmanagedType.U1)] out bool pis_paired);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_PairDevice(uint dev_hdl);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_PinCodeReply(
      uint dev_hdl,
      byte[] pin_code,
      ushort size);

    [DllImport("BsSDK")]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static extern bool Btsdk_IsDeviceConnected(uint dev_hdl);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteRSSI(uint device_handle, out sbyte prssi);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteLinkQuality(
      uint device_handle,
      out ushort plink_quality);

    [DllImport("BsSDK")]
    internal static extern uint Btsdk_GetRemoteDeviceHandle(byte[] bd_addr);

    [DllImport("BsSDK")]
    internal static extern uint Btsdk_AddRemoteDevice(byte[] bd_addr);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_DeleteRemoteDeviceByHandle(uint dev_hdl);

    [DllImport("BsSDK")]
    internal static extern int Btsdk_GetStoredDevicesByClass(
      uint dev_class,
      uint[] pdev_hdl,
      int max_dev_num);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteDeviceAddress(uint dev_hdl, byte[] bd_addr);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteDeviceName(
      uint dev_hdl,
      byte[] name,
      ref ushort plen);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteDeviceClass(
      uint dev_hdl,
      out uint pdevice_class);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteDeviceProperty(
      uint dev_hdl,
      out Structs.BtSdkRemoteDevicePropertyStru rmt_dev_prop);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_BrowseRemoteServicesEx(
      uint dev_hdl,
      Structs.BtSdkSDPSearchPatternStru[] psch_ptn,
      int ptn_num,
      uint[] svc_hdl,
      ref int svc_count);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_GetRemoteServiceAttributes(
      uint svc_hdl,
      ref Structs.BtSdkRemoteServiceAttrStru attribute);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_ConnectEx(
      uint dev_hdl,
      ushort service_class,
      uint lParam,
      out uint conn_hdl);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_ConnectEx(
      uint dev_hdl,
      ushort service_class,
      ref Structs.BtSdkSPPConnParamStru lParam,
      out uint conn_hdl);

    [DllImport("BsSDK")]
    internal static extern uint Btsdk_StartEnumConnection();

    [DllImport("BsSDK")]
    internal static extern uint Btsdk_EnumConnection(
      uint enum_handle,
      ref Structs.BtSdkConnectionPropertyStru conn_prop);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_EndEnumConnection(uint enum_handle);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_Disconnect(uint handle);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_InitCommObj(byte com_idx, ushort svc_class);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_DeinitCommObj(byte com_idx);

    [DllImport("BsSDK")]
    internal static extern short Btsdk_GetClientPort(uint conn_hdl);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_SearchAppExtSPPService(
      uint dev_hdl,
      ref Structs.BtSdkAppExtSPPAttrStru psvc);

    [DllImport("BsSDK")]
    internal static extern BtSdkError Btsdk_ConnectAppExtSPPService(
      uint dev_hdl,
      ref Structs.BtSdkAppExtSPPAttrStru psvc,
      out uint conn_hdl);

    [DllImport("BsSDK")]
    internal static extern uint Btsdk_CommNumToSerialNum(int comportNum);

    [DllImport("BsSDK")]
    internal static extern void Btsdk_PlugOutVComm(uint serialNum, StackConsts.COMM_SET flag);

    [DllImport("BsSDK")]
    internal static extern bool Btsdk_PlugInVComm(
      uint serialNum,
      out uint comportNumber,
      uint usageType,
      StackConsts.COMM_SET flag,
      uint dwTimeout);

    [DllImport("BsSDK")]
    internal static extern uint Btsdk_GetASerialNum();

    internal delegate void Func_ReceiveBluetoothStatusInfo(
      uint usMsgType,
      uint pulData,
      uint param,
      IntPtr arg);

    internal delegate void Btsdk_Inquiry_Result_Ind_Func(uint dev_hdl);

    internal delegate void Btsdk_Inquiry_Complete_Ind_Func();

    internal delegate StackConsts.CallbackResult Btsdk_UserHandle_Pin_Req_Ind_Func(uint dev_hdl);

    internal delegate void Btsdk_Connection_Event_Ind_Func(
      uint conn_hdl,
      StackConsts.ConnectionEventType @event,
      IntPtr arg);
  }
}
