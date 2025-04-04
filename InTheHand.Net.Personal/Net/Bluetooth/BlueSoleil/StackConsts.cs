// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.StackConsts
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal static class StackConsts
  {
    internal const int BTSDK_SERVICENAME_MAXLENGTH = 80;
    internal const int BTSDK_MAX_SUPPORT_FORMAT = 6;
    internal const int BTSDK_PATH_MAXLENGTH = 256;
    internal const int BTSDK_CARDNAME_MAXLENGTH = 256;
    internal const int BTSDK_PACKETTYPE_MAXNUM = 10;
    internal const int BTSDK_DEVNAME_LEN = 64;
    internal const int BTSDK_SHORTCUT_NAME_LEN = 100;
    internal const int BTSDK_BDADDR_LEN = 6;
    internal const int BTSDK_LINKKEY_LEN = 16;
    internal const int BTSDK_PINCODE_LEN = 16;
    internal const uint BTSDK_INVALID_HANDLE = 0;
    internal const ushort BTSDK_BLUETOOTH_STATUS_FLAG = 2;
    internal const uint BTSDK_CONNROLE_Mask = 3;
    internal const StackConsts.DiscoveryMode BTSDK_DISCOVERY_DEFAULT_MODE = StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE | StackConsts.DiscoveryMode.BTSDK_CONNECTABLE | StackConsts.DiscoveryMode.BTSDK_PAIRABLE;
    private const uint BTSDK_CLIENTCBK_PRIORITY_HIGH = 3;
    private const uint BTSDK_CLIENTCBK_PRIORITY_MEDIUM = 2;

    internal enum BTSDK_BTSTATUS : uint
    {
      TurnOn = 1,
      TurnOff = 2,
      HwPlugged = 3,
      HwPulled = 4,
    }

    internal enum BTSDK_CONNROLE
    {
      Acceptor = 1,
      Initiator = 2,
    }

    internal enum CallbackType : ushort
    {
      PIN_CODE_IND = 0,
      LINK_KEY_NOTIF_IND = 2,
      AUTHENTICATION_FAIL_IND = 3,
      INQUIRY_RESULT_IND = 4,
      INQUIRY_COMPLETE_IND = 5,
      AUTHORIZATION_IND = 6,
      CONNECTION_EVENT_IND = 9,
    }

    [Flags]
    internal enum DiscoveryMode : ushort
    {
      BTSDK_GENERAL_DISCOVERABLE = 1,
      BTSDK_LIMITED_DISCOVERABLE = 2,
      BTSDK_DISCOVERABLE = BTSDK_GENERAL_DISCOVERABLE, // 0x0001
      BTSDK_CONNECTABLE = 4,
      BTSDK_PAIRABLE = 8,
    }

    internal enum ConnectionEventType : ushort
    {
      CONN_IND = 1,
      DISC_IND = 2,
      CONN_CFM = 7,
      DISC_CFM = 8,
    }

    internal enum CallbackResult : byte
    {
      NotHandled,
      Handled,
    }

    [Flags]
    internal enum AttributeLookup : ushort
    {
      ServiceName = 1,
      ExtAttributes = 2,
    }

    internal enum COMM_SET : uint
    {
      UsageType = 1,
      Record = 16, // 0x00000010
    }
  }
}
