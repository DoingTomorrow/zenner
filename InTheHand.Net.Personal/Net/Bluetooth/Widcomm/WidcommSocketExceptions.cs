// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommSocketExceptions
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal static class WidcommSocketExceptions
  {
    internal const int SocketError_StartInquiry_Failed = 10091;
    internal const int SocketError_SetSecurityLevel_Client_Fail = -1;
    internal const int SocketError_StartDiscovery_Failed = 10092;
    internal const int SocketError_NoSuchService = 10049;
    internal const int SocketError_ServiceNoneRfcommScn = 10064;
    internal const int SocketError_ConnectionClosed = 10057;
    private const int SocketError_Listener_SdpError = 10014;

    internal static SocketException Create(REM_DEV_INFO_RETURN_CODE err, string location)
    {
      return (SocketException) new REM_DEV_INFO_RETURN_CODE_WidcommSocketException(10000, err, location);
    }

    internal static SocketException Create(DISCOVERY_RESULT result, string location)
    {
      return (SocketException) new DISCOVERY_RESULT_WidcommSocketException(10092, result, location);
    }

    internal static SocketException Create(PORT_RETURN_CODE result, string location)
    {
      return (SocketException) new PORT_RETURN_CODE_WidcommSocketException(10092, result, location);
    }

    internal static SocketException Create_SDP_RETURN_CODE(
      SdpService.SDP_RETURN_CODE ret,
      string location)
    {
      return (SocketException) new SDP_RETURN_CODE_WidcommSocketException(10014, ret, location);
    }

    internal static SocketException Create_NoResultCode(int errorCode, string location)
    {
      return (SocketException) new NoResultCodeWidcommSocketException(errorCode, location);
    }

    internal static SocketException Create_StartInquiry(string location)
    {
      return WidcommSocketExceptions.Create_NoResultCode(10091, location);
    }

    internal static SocketException CreateConnectFailed(string location, int? socketErrorCode)
    {
      return WidcommSocketExceptions.Create_NoResultCode(socketErrorCode ?? 10061, location);
    }

    internal static SocketException ConnectionIsPeerClosed()
    {
      return WidcommSocketExceptions.Create_NoResultCode(10057, "RfcommStream_Closed");
    }

    internal static SocketException Create_StartDiscovery(WBtRc ee)
    {
      string str;
      if (ee != ~WBtRc.WBT_SUCCESS)
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, ", {0} = 0x{1:X}", (object) ee, (object) (uint) ee);
      else
        str = string.Empty;
      return WidcommSocketExceptions.Create_NoResultCode(10092, "StartDiscoverySDP" + str);
    }
  }
}
