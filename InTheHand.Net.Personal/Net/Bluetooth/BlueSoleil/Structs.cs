// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.Structs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Widcomm;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal static class Structs
  {
    internal struct BtSdkLocalLMPInfoStru
    {
      internal readonly byte lmp_feature_0;
      internal readonly byte lmp_feature_1;
      internal readonly byte lmp_feature_2;
      internal readonly byte lmp_feature_3;
      internal readonly byte lmp_feature_4;
      internal readonly byte lmp_feature_5;
      internal readonly byte lmp_feature_6;
      internal readonly byte lmp_feature_7;
      internal readonly ushort manuf_name;
      internal readonly ushort lmp_subversion;
      internal readonly byte lmp_version;
      internal readonly byte hci_version;
      internal readonly ushort hci_revision;
      internal readonly byte country_code;

      internal BtSdkLocalLMPInfoStru(HciVersion fake)
        : this()
      {
        DEV_VER_INFO.SetManufacturerAndVersionsToUnknown(out this.manuf_name, out this.hci_version, out this.lmp_version);
      }
    }

    internal struct BtSdkCallbackStru
    {
      internal readonly StackConsts.CallbackType _type;
      private readonly Delegate _func;

      internal BtSdkCallbackStru(
        NativeMethods.Btsdk_Inquiry_Result_Ind_Func inquiryResultIndFunc)
      {
        this._type = StackConsts.CallbackType.INQUIRY_RESULT_IND;
        this._func = (Delegate) inquiryResultIndFunc;
      }

      internal BtSdkCallbackStru(
        NativeMethods.Btsdk_Inquiry_Complete_Ind_Func inquiryCompleteIndFunc)
      {
        this._type = StackConsts.CallbackType.INQUIRY_COMPLETE_IND;
        this._func = (Delegate) inquiryCompleteIndFunc;
      }

      internal BtSdkCallbackStru(
        NativeMethods.Btsdk_Connection_Event_Ind_Func connectionEventIndFunc)
      {
        this._type = StackConsts.CallbackType.CONNECTION_EVENT_IND;
        this._func = (Delegate) connectionEventIndFunc;
      }

      internal BtSdkCallbackStru(
        NativeMethods.Btsdk_UserHandle_Pin_Req_Ind_Func pinReqIndFunc)
      {
        this._type = StackConsts.CallbackType.PIN_CODE_IND;
        this._func = (Delegate) pinReqIndFunc;
      }
    }

    internal struct BtSdkAppExtSPPAttrStru
    {
      internal readonly int size;
      internal readonly uint sdp_record_handle;
      internal readonly Guid service_class_128;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
      internal readonly byte[] svc_name;
      internal readonly byte rf_svr_chnl;
      internal readonly byte com_index;

      internal BtSdkAppExtSPPAttrStru(BluetoothEndPoint remoteEP)
      {
        this.size = Marshal.SizeOf(typeof (Structs.BtSdkAppExtSPPAttrStru));
        this.sdp_record_handle = 0U;
        if (remoteEP.Port != 0 && remoteEP.Port != -1)
        {
          this.service_class_128 = Guid.Empty;
          this.rf_svr_chnl = checked ((byte) remoteEP.Port);
        }
        else
        {
          this.service_class_128 = remoteEP.Service;
          this.rf_svr_chnl = (byte) 0;
        }
        this.svc_name = new byte[80];
        this.com_index = (byte) 0;
      }

      internal BtSdkAppExtSPPAttrStru(Guid serviceClass)
      {
        this.size = Marshal.SizeOf(typeof (Structs.BtSdkAppExtSPPAttrStru));
        this.sdp_record_handle = 0U;
        this.service_class_128 = serviceClass;
        this.rf_svr_chnl = (byte) 0;
        this.svc_name = new byte[80];
        this.com_index = (byte) 0;
      }

      public override string ToString()
      {
        CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        StringBuilder stringBuilder = new StringBuilder(this.GetType().Name);
        stringBuilder.AppendFormat((IFormatProvider) invariantCulture, " sdp_record_handle: 0x{0:X}", (object) this.sdp_record_handle);
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat((IFormatProvider) invariantCulture, "   service_class_128: {0}", (object) this.service_class_128);
        stringBuilder.AppendFormat((IFormatProvider) invariantCulture, " svc_name: \"{0}...\"", (object) Structs.BtSdkAppExtSPPAttrStru.ToPrintable(this.svc_name[0]));
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat((IFormatProvider) invariantCulture, "   rf_svr_chnl: {0}", (object) this.rf_svr_chnl);
        stringBuilder.AppendFormat((IFormatProvider) invariantCulture, " com_index: {0}", (object) this.com_index);
        stringBuilder.Append(".");
        return stringBuilder.ToString();
      }

      private static string ToPrintable(byte b)
      {
        return !char.IsControl((char) b) ? ((char) b).ToString() : "\\x" + b.ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    internal struct BtSdkSPPConnParamStru
    {
      private int size;
      private ushort mask;
      private byte com_index;

      internal BtSdkSPPConnParamStru(uint osComPort)
      {
        this.size = Marshal.SizeOf(typeof (Structs.BtSdkSPPConnParamStru));
        this.com_index = checked ((byte) osComPort);
        this.mask = (ushort) 0;
      }
    }

    internal struct BtSdkSDPSearchPatternStru
    {
      private readonly Structs.BtSdkSDPSearchPatternStru.UuidTypeMask mask;
      private readonly Guid uuid;

      internal BtSdkSDPSearchPatternStru(Guid serviceClass)
      {
        this.uuid = serviceClass;
        this.mask = Structs.BtSdkSDPSearchPatternStru.UuidTypeMask.BTSDK_SSPM_UUID128;
      }

      private enum UuidTypeMask : uint
      {
        BTSDK_SSPM_UUID16 = 1,
        BTSDK_SSPM_UUID32 = 2,
        BTSDK_SSPM_UUID128 = 4,
      }
    }

    internal struct BtSdkRemoteServiceAttrStru
    {
      internal readonly StackConsts.AttributeLookup mask;
      internal readonly ushort svc_class;
      internal readonly uint dev_hdl;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
      internal readonly byte[] svc_name;
      internal readonly IntPtr ext_attributes;
      internal readonly ushort status;

      public BtSdkRemoteServiceAttrStru(StackConsts.AttributeLookup mask)
      {
        this.mask = mask;
        this.svc_class = (ushort) 0;
        this.dev_hdl = 0U;
        this.svc_name = new byte[80];
        this.ext_attributes = IntPtr.Zero;
        this.status = (ushort) 0;
      }

      public BtSdkRemoteServiceAttrStru(
        StackConsts.AttributeLookup mask,
        ushort svc_class,
        byte[] svcName,
        IntPtr pExtAttr)
      {
        this.mask = mask;
        this.svc_class = svc_class;
        this.svc_name = new byte[80];
        svcName.CopyTo((Array) this.svc_name, 0);
        this.status = (ushort) 0;
        this.ext_attributes = pExtAttr;
        this.dev_hdl = 0U;
      }
    }

    internal struct BtSdkRmtSPPSvcExtAttrStru
    {
      internal readonly int size;
      internal readonly byte server_channel;

      internal BtSdkRmtSPPSvcExtAttrStru(byte scn)
      {
        this.size = Marshal.SizeOf(typeof (Structs.BtSdkRmtSPPSvcExtAttrStru));
        this.server_channel = scn;
      }
    }

    internal struct BtSdkConnectionPropertyStru
    {
      internal readonly uint role_AND_result;
      internal readonly uint device_handle;
      internal readonly uint service_handle;
      internal readonly ushort service_class;
      internal readonly uint duration;
      internal readonly uint received_bytes;
      internal readonly uint sent_bytes;

      private BtSdkConnectionPropertyStru(Version shutUpCompiler)
      {
        this.role_AND_result = 0U;
        this.device_handle = 0U;
        this.service_handle = 0U;
        this.service_class = (ushort) 0;
        this.duration = 0U;
        this.received_bytes = 0U;
        this.sent_bytes = 0U;
      }

      internal StackConsts.BTSDK_CONNROLE Role
      {
        get => (StackConsts.BTSDK_CONNROLE) ((int) this.role_AND_result & 3);
      }
    }

    internal struct BtSdkRmtDISvcExtAttrStru
    {
      internal const int StackMiscountsPaddingSize = -1;
      internal readonly int size;
      internal readonly ushort mask;
      internal readonly ushort spec_id;
      internal readonly ushort vendor_id;
      internal readonly ushort product_id;
      internal readonly ushort version;
      [MarshalAs(UnmanagedType.I1)]
      internal readonly bool primary_record;
      internal readonly ushort vendor_id_source;
      internal readonly ushort list_size;
      internal readonly byte str_url_list__ARRAY;

      internal BtSdkRmtDISvcExtAttrStru(
        ushort spec_id,
        ushort vendor_id,
        ushort product_id,
        ushort version,
        bool primary_record,
        ushort vendor_id_source,
        ushort mask)
      {
        this.mask = mask;
        this.spec_id = spec_id;
        this.vendor_id = vendor_id;
        this.product_id = product_id;
        this.version = version;
        this.primary_record = primary_record;
        this.vendor_id_source = vendor_id_source;
        this.size = Marshal.SizeOf(typeof (Structs.BtSdkRmtDISvcExtAttrStru)) - 1;
        this.list_size = (ushort) 0;
        this.str_url_list__ARRAY = (byte) 0;
      }
    }

    internal struct BtSdkRemoteDevicePropertyStru
    {
      internal readonly Structs.BtSdkRemoteDevicePropertyStru.Mask mask;
      internal readonly uint dev_hdl;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
      internal readonly byte[] bd_addr;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
      internal readonly byte[] name;
      internal readonly uint dev_class;
      internal readonly Structs.BtSdkRemoteLMPInfoStru lmp_info;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
      internal readonly byte[] link_key;

      internal enum Mask : uint
      {
        Handle = 1,
        Address = 2,
        Name = 4,
        Class = 8,
        LmpInfo = 16, // 0x00000010
        LinkKey = 32, // 0x00000020
      }
    }

    internal struct BtSdkRemoteLMPInfoStru
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
      internal readonly byte[] lmp_feature;
      internal readonly Manufacturer manuf_name;
      internal readonly ushort lmp_subversion;
      internal readonly LmpVersion lmp_version;
    }

    internal struct BtSdkRmtHidSvcExtAttrStru_HACK
    {
      internal const int StackMiscountsPaddingSize = -1;
      internal readonly int size;
      internal readonly ushort mask;
      internal readonly ushort deviceReleaseNumber;
      internal readonly ushort unknown0a;
      internal readonly byte deviceSubclass;
      internal readonly byte countryCode;
      internal readonly uint unknownA;
      internal readonly uint unknownB;
      internal readonly uint unknownC;
      internal readonly ushort unknownD;
      internal readonly byte unknownE;

      private BtSdkRmtHidSvcExtAttrStru_HACK(Version shutUpCompiler)
      {
        this.size = 0;
        this.mask = (ushort) 0;
        this.deviceReleaseNumber = (ushort) 0;
        this.unknown0a = (ushort) 0;
        this.deviceSubclass = (byte) 0;
        this.countryCode = (byte) 0;
        this.unknownA = 0U;
        this.unknownB = 0U;
        this.unknownC = 0U;
        this.unknownD = (ushort) 0;
        this.unknownE = (byte) 0;
      }
    }
  }
}
