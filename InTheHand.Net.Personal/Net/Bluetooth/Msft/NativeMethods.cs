// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.NativeMethods
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal static class NativeMethods
  {
    internal const int MAX_PATH = 260;
    private const string wsDll = "ws2_32.dll";
    private const string irpropsDll = "Irprops.cpl";
    private const string bthpropsDll = "bthprops.cpl";
    internal const int BTH_MAX_NAME_SIZE = 248;
    internal const int BLUETOOTH_MAX_PASSKEY_SIZE = 16;
    internal static readonly Guid GUID_DEVCLASS_BLUETOOTH = new Guid("{E0CBF06C-CD8B-4647-BB8A-263B43F0F974}");

    [DllImport("ws2_32.dll", SetLastError = true)]
    internal static extern int WSASetService(
      ref WSAQUERYSET lpqsRegInfo,
      WSAESETSERVICEOP essoperation,
      int dwControlFlags);

    [DllImport("ws2_32.dll", SetLastError = true)]
    internal static extern int WSALookupServiceBegin(
      byte[] pQuerySet,
      LookupFlags dwFlags,
      out IntPtr lphLookup);

    [DllImport("ws2_32.dll", SetLastError = true)]
    internal static extern int WSALookupServiceBegin(
      ref WSAQUERYSET pQuerySet,
      LookupFlags dwFlags,
      out IntPtr lphLookup);

    [DllImport("ws2_32.dll", SetLastError = true)]
    internal static extern int WSALookupServiceNext(
      IntPtr hLookup,
      LookupFlags dwFlags,
      ref int lpdwBufferLength,
      byte[] pResults);

    [DllImport("ws2_32.dll", SetLastError = true)]
    internal static extern int WSALookupServiceEnd(IntPtr hLookup);

    [DllImport("Irprops.cpl", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int BluetoothAuthenticateDevice(
      IntPtr hwndParent,
      IntPtr hRadio,
      ref BLUETOOTH_DEVICE_INFO pbtdi,
      string pszPasskey,
      int ulPasskeyLength);

    [DllImport("bthprops.cpl", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int BluetoothAuthenticateDeviceEx(
      IntPtr hwndParentIn,
      IntPtr hRadioIn,
      ref BLUETOOTH_DEVICE_INFO pbtdiInout,
      byte[] pbtOobData,
      BluetoothAuthenticationRequirements authenticationRequirement);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothDisplayDeviceProperties(
      IntPtr hwndParent,
      ref BLUETOOTH_DEVICE_INFO pbtdi);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothEnableDiscovery(IntPtr hRadio, bool fEnabled);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio, bool fEnabled);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothEnumerateInstalledServices(
      IntPtr hRadio,
      ref BLUETOOTH_DEVICE_INFO pbtdi,
      ref int pcServices,
      byte[] pGuidServices);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothSetServiceState(
      IntPtr hRadio,
      ref BLUETOOTH_DEVICE_INFO pbtdi,
      ref Guid pGuidService,
      int dwServiceFlags);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern IntPtr BluetoothFindFirstRadio(
      ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp,
      ref IntPtr phRadio);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothFindNextRadio(IntPtr hFind, ref IntPtr phRadio);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothFindRadioClose(IntPtr hFind);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothGetDeviceInfo(
      IntPtr hRadio,
      ref BLUETOOTH_DEVICE_INFO pbtdi);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothGetRadioInfo(
      IntPtr hRadio,
      ref BLUETOOTH_RADIO_INFO pRadioInfo);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothIsConnectable(IntPtr hRadio);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothIsDiscoverable(IntPtr hRadio);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothUpdateDeviceRecord(ref BLUETOOTH_DEVICE_INFO pbtdi);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothSdpGetAttributeValue(
      byte[] pRecordStream,
      int cbRecordLength,
      ushort usAttributeId,
      IntPtr pAttributeData);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothSdpGetContainerElementData(
      byte[] pContainerStream,
      uint cbContainerLength,
      ref IntPtr pElement,
      byte[] pData);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothSdpGetElementData(
      byte[] pSdpStream,
      uint cbSpdStreamLength,
      byte[] pData);

    [DllImport("Irprops.cpl", SetLastError = true)]
    internal static extern int BluetoothSdpGetString(
      byte[] pRecordStream,
      uint cbRecordLength,
      IntPtr pStringData,
      ushort usStringOffset,
      byte[] pszString,
      ref uint pcchStringLength);

    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothSdpEnumAttributes(
      IntPtr pSDPStream,
      int cbStreamSize,
      NativeMethods.BluetoothEnumAttributesCallback pfnCallback,
      IntPtr pvParam);

    [DllImport("Irprops.cpl", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint BluetoothRegisterForAuthentication(
      ref BLUETOOTH_DEVICE_INFO pbtdi,
      out BluetoothAuthenticationRegistrationHandle phRegHandle,
      NativeMethods.BluetoothAuthenticationCallback pfnCallback,
      IntPtr pvParam);

    [DllImport("bthprops.cpl", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint BluetoothRegisterForAuthenticationEx(
      ref BLUETOOTH_DEVICE_INFO pbtdi,
      out BluetoothAuthenticationRegistrationHandle phRegHandle,
      NativeMethods.BluetoothAuthenticationCallbackEx pfnCallback,
      IntPtr pvParam);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("Irprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothUnregisterAuthentication(IntPtr hRegHandle);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("bthprops.cpl", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothUnregisterAuthenticationEx(IntPtr hRegHandle);

    [DllImport("Irprops.cpl", CharSet = CharSet.Unicode)]
    internal static extern int BluetoothSendAuthenticationResponse(
      IntPtr hRadio,
      ref BLUETOOTH_DEVICE_INFO pbtdi,
      string pszPasskey);

    [DllImport("bthprops.cpl", CharSet = CharSet.Unicode)]
    internal static extern int BluetoothSendAuthenticationResponseEx(
      IntPtr hRadio,
      ref BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO pauthResponse);

    [DllImport("bthprops.cpl", CharSet = CharSet.Unicode)]
    internal static extern int BluetoothSendAuthenticationResponseEx(
      IntPtr hRadio,
      ref BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO pauthResponse);

    [DllImport("bthprops.cpl", CharSet = CharSet.Unicode)]
    internal static extern int BluetoothSendAuthenticationResponseEx(
      IntPtr hRadio,
      ref BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO pauthResponse);

    [DllImport("Irprops.cpl", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int BluetoothRemoveDevice(byte[] pAddress);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool DeviceIoControl(
      IntPtr hDevice,
      uint dwIoControlCode,
      ref long InBuffer,
      int nInBufferSize,
      IntPtr OutBuffer,
      int nOutBufferSize,
      out int pBytesReturned,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool DeviceIoControl(
      IntPtr hDevice,
      uint dwIoControlCode,
      IntPtr InBuffer,
      int nInBufferSize,
      byte[] OutBuffer,
      int nOutBufferSize,
      out int pBytesReturned,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool DeviceIoControl(
      IntPtr hDevice,
      uint dwIoControlCode,
      byte[] InBuffer,
      int nInBufferSize,
      byte[] OutBuffer,
      int nOutBufferSize,
      out int pBytesReturned,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool DeviceIoControl(
      IntPtr hDevice,
      uint dwIoControlCode,
      ref long InBuffer,
      int nInBufferSize,
      ref BTH_RADIO_INFO OutBuffer,
      int nOutBufferSize,
      out int pBytesReturned,
      IntPtr lpOverlapped);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr SetupDiGetClassDevs(
      ref Guid classGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string enumerator,
      IntPtr hwndParent,
      NativeMethods.DIGCF flags);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiEnumDeviceInfo(
      IntPtr deviceInfoSet,
      uint memberIndex,
      ref NativeMethods.SP_DEVINFO_DATA deviceInfoData);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiGetDeviceInstanceId(
      IntPtr deviceInfoSet,
      ref NativeMethods.SP_DEVINFO_DATA deviceInfoData,
      StringBuilder deviceInstanceId,
      int deviceInstanceIdSize,
      out int requiredSize);

    [DllImport("bthprops.cpl", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool BluetoothIsVersionAvailable(byte MajorVersion, byte MinorVersion);

    internal enum BTH_ERROR
    {
      SUCCESS = 0,
      UNKNOWN_HCI_COMMAND = 1,
      NO_CONNECTION = 2,
      HARDWARE_FAILURE = 3,
      PAGE_TIMEOUT = 4,
      AUTHENTICATION_FAILURE = 5,
      KEY_MISSING = 6,
      MEMORY_FULL = 7,
      CONNECTION_TIMEOUT = 8,
      MAX_NUMBER_OF_CONNECTIONS = 9,
      MAX_NUMBER_OF_SCO_CONNECTIONS = 10, // 0x0000000A
      ACL_CONNECTION_ALREADY_EXISTS = 11, // 0x0000000B
      COMMAND_DISALLOWED = 12, // 0x0000000C
      HOST_REJECTED_LIMITED_RESOURCES = 13, // 0x0000000D
      HOST_REJECTED_SECURITY_REASONS = 14, // 0x0000000E
      HOST_REJECTED_PERSONAL_DEVICE = 15, // 0x0000000F
      HOST_TIMEOUT = 16, // 0x00000010
      UNSUPPORTED_FEATURE_OR_PARAMETER = 17, // 0x00000011
      INVALID_HCI_PARAMETER = 18, // 0x00000012
      REMOTE_USER_ENDED_CONNECTION = 19, // 0x00000013
      REMOTE_LOW_RESOURCES = 20, // 0x00000014
      REMOTE_POWERING_OFF = 21, // 0x00000015
      LOCAL_HOST_TERMINATED_CONNECTION = 22, // 0x00000016
      REPEATED_ATTEMPTS = 23, // 0x00000017
      PAIRING_NOT_ALLOWED = 24, // 0x00000018
      UKNOWN_LMP_PDU = 25, // 0x00000019
      UNSUPPORTED_REMOTE_FEATURE = 26, // 0x0000001A
      SCO_OFFSET_REJECTED = 27, // 0x0000001B
      SCO_INTERVAL_REJECTED = 28, // 0x0000001C
      SCO_AIRMODE_REJECTED = 29, // 0x0000001D
      INVALID_LMP_PARAMETERS = 30, // 0x0000001E
      UNSPECIFIED_ERROR = 31, // 0x0000001F
      UNSUPPORTED_LMP_PARM_VALUE = 32, // 0x00000020
      ROLE_CHANGE_NOT_ALLOWED = 33, // 0x00000021
      LMP_RESPONSE_TIMEOUT = 34, // 0x00000022
      LMP_TRANSACTION_COLLISION = 35, // 0x00000023
      LMP_PDU_NOT_ALLOWED = 36, // 0x00000024
      ENCRYPTION_MODE_NOT_ACCEPTABLE = 37, // 0x00000025
      UNIT_KEY_NOT_USED = 38, // 0x00000026
      QOS_IS_NOT_SUPPORTED = 39, // 0x00000027
      INSTANT_PASSED = 40, // 0x00000028
      PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED = 41, // 0x00000029
      DIFFERENT_TRANSACTION_COLLISION = 42, // 0x0000002A
      QOS_UNACCEPTABLE_PARAMETER = 44, // 0x0000002C
      QOS_REJECTED = 45, // 0x0000002D
      CHANNEL_CLASSIFICATION_NOT_SUPPORTED = 46, // 0x0000002E
      INSUFFICIENT_SECURITY = 47, // 0x0000002F
      PARAMETER_OUT_OF_MANDATORY_RANGE = 48, // 0x00000030
      ROLE_SWITCH_PENDING = 50, // 0x00000032
      RESERVED_SLOT_VIOLATION = 52, // 0x00000034
      ROLE_SWITCH_FAILED = 53, // 0x00000035
      EXTENDED_INQUIRY_RESPONSE_TOO_LARGE = 54, // 0x00000036
      SECURE_SIMPLE_PAIRING_NOT_SUPPORTED_BY_HOST = 55, // 0x00000037
      HOST_BUSY_PAIRING = 56, // 0x00000038
      CONNECTION_REJECTED_DUE_TO_NO_SUITABLE_CHANNEL_FOUND = 57, // 0x00000039
      CONTROLLER_BUSY = 58, // 0x0000003A
      UNACCEPTABLE_CONNECTION_INTERVAL = 59, // 0x0000003B
      DIRECTED_ADVERTISING_TIMEOUT = 60, // 0x0000003C
      CONNECTION_TERMINATED_DUE_TO_MIC_FAILURE = 61, // 0x0000003D
      CONNECTION_FAILED_TO_BE_ESTABLISHED = 62, // 0x0000003E
      MAC_CONNECTION_FAILED = 63, // 0x0000003F
      UNSPECIFIED = 255, // 0x000000FF
    }

    internal struct SDP_STRING_TYPE_DATA
    {
      internal ushort encoding;
      internal ushort mibeNum;
      internal ushort attributeID;

      private void ShutUpCompiler()
      {
        this.encoding = (ushort) 0;
        this.mibeNum = (ushort) 0;
        this.attributeID = (ushort) 0;
      }
    }

    [return: MarshalAs(UnmanagedType.Bool)]
    internal delegate bool BluetoothEnumAttributesCallback(
      uint uAttribId,
      IntPtr pValueStream,
      int cbStreamSize,
      IntPtr pvParam);

    [return: MarshalAs(UnmanagedType.Bool)]
    internal delegate bool BluetoothAuthenticationCallback(
      IntPtr pvParam,
      ref BLUETOOTH_DEVICE_INFO bdi);

    [return: MarshalAs(UnmanagedType.Bool)]
    internal delegate bool BluetoothAuthenticationCallbackEx(
      IntPtr pvParam,
      ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams);

    [Flags]
    internal enum DIGCF
    {
      PRESENT = 2,
      ALLCLASSES = 4,
      PROFILE = 8,
    }

    internal enum SPDRP
    {
      HARDWAREID = 1,
      DRIVER = 9,
    }

    internal struct SP_DEVINFO_DATA
    {
      internal int cbSize;
      internal Guid ClassGuid;
      internal uint DevInst;
      internal IntPtr Reserved;
    }

    internal static class MsftWin32BthIOCTL
    {
      private const uint IOCTL_INTERNAL_BTH_SUBMIT_BRB = 4259843;
      private const uint IOCTL_INTERNAL_BTHENUM_GET_ENUMINFO = 4259847;
      private const uint IOCTL_INTERNAL_BTHENUM_GET_DEVINFO = 4259851;
      internal const uint IOCTL_BTH_GET_LOCAL_INFO = 4259840;
      internal const uint IOCTL_BTH_GET_RADIO_INFO = 4259844;
      internal const uint IOCTL_BTH_GET_DEVICE_INFO = 4259848;
      internal const uint IOCTL_BTH_DISCONNECT_DEVICE = 4259852;
      internal const uint IOCTL_BTH_GET_DEVICE_RSSI = 4259860;
      internal const uint IOCTL_BTH_EIR_GET_RECORDS = 4259904;
      internal const uint IOCTL_BTH_EIR_SUBMIT_RECORD = 4259908;
      internal const uint IOCTL_BTH_EIR_UPDATE_RECORD = 4259912;
      internal const uint IOCTL_BTH_EIR_REMOVE_RECORD = 4259916;
      internal const uint IOCTL_BTH_HCI_VENDOR_COMMAND = 4259920;
      internal const uint IOCTL_BTH_SDP_CONNECT = 4260352;
      internal const uint IOCTL_BTH_SDP_DISCONNECT = 4260356;
      internal const uint IOCTL_BTH_SDP_SERVICE_SEARCH = 4260360;
      internal const uint IOCTL_BTH_SDP_ATTRIBUTE_SEARCH = 4260364;
      internal const uint IOCTL_BTH_SDP_SERVICE_ATTRIBUTE_SEARCH = 4260368;
      internal const uint IOCTL_BTH_SDP_SUBMIT_RECORD = 4260372;
      internal const uint IOCTL_BTH_SDP_REMOVE_RECORD = 4260376;
      internal const uint IOCTL_BTH_SDP_SUBMIT_RECORD_WITH_INFO = 4260380;
    }
  }
}
