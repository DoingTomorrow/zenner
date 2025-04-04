// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BluesoleilDeviceInfo
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class BluesoleilDeviceInfo : IBluetoothDeviceInfo
  {
    private const int MaxServiceRecordsLookup = 100;
    private readonly BluesoleilFactory _factory;
    private readonly uint _hDev;
    private readonly BluetoothAddress _addr;
    private string _cachedName;
    private bool _paired;
    private bool _remembered;
    private bool _connected;
    private ClassOfDevice _cod;
    private DateTime _lastSeen;
    private Func<Guid, ServiceRecord[]> _dlgtGetServiceRecords;
    private RadioVersions _versions;

    private BluesoleilDeviceInfo(uint hDev, BluesoleilFactory factory)
    {
      this._factory = factory != null ? factory : throw new ArgumentNullException(nameof (factory));
      this._hDev = hDev;
      byte[] numArray = new byte[6];
      BtSdkError remoteDeviceAddress = factory.Api.Btsdk_GetRemoteDeviceAddress(this._hDev, numArray);
      BluesoleilUtils.CheckAndThrow(remoteDeviceAddress, "Btsdk_GetRemoteDeviceAddress");
      if (remoteDeviceAddress != BtSdkError.OK)
        return;
      this._addr = BluesoleilUtils.ToBluetoothAddress(numArray);
      uint pdevice_class;
      if (factory.Api.Btsdk_GetRemoteDeviceClass(this._hDev, out pdevice_class) != BtSdkError.OK)
        return;
      this._cod = new ClassOfDevice(pdevice_class);
      this._factory.Api.Btsdk_IsDevicePaired(this._hDev, out this._paired);
      this._connected = this._factory.Api.Btsdk_IsDeviceConnected(this._hDev);
      this.GetInfo(ref this._addr);
    }

    private void GetInfo(ref BluetoothAddress addr)
    {
      Structs.BtSdkRemoteDevicePropertyStru rmt_dev_prop = new Structs.BtSdkRemoteDevicePropertyStru();
      BluesoleilUtils.CheckAndThrow(this._factory.Api.Btsdk_GetRemoteDeviceProperty(this._hDev, out rmt_dev_prop), "Btsdk_GetRemoteDeviceProperty");
      int num = (int) (rmt_dev_prop.mask & Structs.BtSdkRemoteDevicePropertyStru.Mask.Handle);
      if ((rmt_dev_prop.mask & Structs.BtSdkRemoteDevicePropertyStru.Mask.Address) != (Structs.BtSdkRemoteDevicePropertyStru.Mask) 0)
        addr = BluesoleilUtils.ToBluetoothAddress(rmt_dev_prop.bd_addr);
      if ((rmt_dev_prop.mask & Structs.BtSdkRemoteDevicePropertyStru.Mask.Class) != (Structs.BtSdkRemoteDevicePropertyStru.Mask) 0)
        this._cod = new ClassOfDevice(rmt_dev_prop.dev_class);
      if ((rmt_dev_prop.mask & Structs.BtSdkRemoteDevicePropertyStru.Mask.Name) != (Structs.BtSdkRemoteDevicePropertyStru.Mask) 0)
        this._cachedName = BluesoleilUtils.FromNameString(rmt_dev_prop.name);
      if ((rmt_dev_prop.mask & Structs.BtSdkRemoteDevicePropertyStru.Mask.LmpInfo) == (Structs.BtSdkRemoteDevicePropertyStru.Mask) 0)
        return;
      LmpFeatures int64 = (LmpFeatures) BitConverter.ToInt64(rmt_dev_prop.lmp_info.lmp_feature, 0);
      this._versions = new RadioVersions(rmt_dev_prop.lmp_info.lmp_version, rmt_dev_prop.lmp_info.lmp_subversion, int64, rmt_dev_prop.lmp_info.manuf_name);
    }

    internal static BluesoleilDeviceInfo CreateFromGivenAddress(
      BluetoothAddress addr,
      BluesoleilFactory factory)
    {
      byte[] bd_addr = BluesoleilUtils.FromBluetoothAddress(addr);
      uint hDev = factory.Api.Btsdk_GetRemoteDeviceHandle(bd_addr);
      bool flag;
      if (hDev != 0U)
      {
        flag = true;
      }
      else
      {
        hDev = factory.Api.Btsdk_AddRemoteDevice(bd_addr);
        if (hDev == 0U)
          BluesoleilUtils.CheckAndThrow(BtSdkError.SDK_UNINIT, "Btsdk_Get/AddRemoteDevice");
        flag = false;
      }
      return new BluesoleilDeviceInfo(hDev, factory)
      {
        _remembered = flag
      };
    }

    internal static BluesoleilDeviceInfo CreateFromHandleFromStored(
      uint hDev,
      BluesoleilFactory factory)
    {
      return new BluesoleilDeviceInfo(hDev, factory)
      {
        _remembered = true
      };
    }

    internal static BluesoleilDeviceInfo CreateFromHandleFromConnection(
      uint hDev,
      BluesoleilFactory factory)
    {
      return new BluesoleilDeviceInfo(hDev, factory);
    }

    internal static BluesoleilDeviceInfo CreateFromHandleFromInquiry(
      uint hDev,
      BluesoleilFactory factory)
    {
      BluesoleilDeviceInfo handleFromInquiry = new BluesoleilDeviceInfo(hDev, factory);
      if (handleFromInquiry.Authenticated)
        handleFromInquiry._remembered = true;
      return handleFromInquiry;
    }

    internal static List<IBluetoothDeviceInfo> CreateFromInquiryHandles(
      List<uint> discoverableHandles,
      BluesoleilFactory factory)
    {
      List<IBluetoothDeviceInfo> fromInquiryHandles = new List<IBluetoothDeviceInfo>(discoverableHandles.Count);
      foreach (uint discoverableHandle in discoverableHandles)
      {
        IBluetoothDeviceInfo handleFromInquiry = (IBluetoothDeviceInfo) BluesoleilDeviceInfo.CreateFromHandleFromInquiry(discoverableHandle, factory);
        fromInquiryHandles.Add(handleFromInquiry);
      }
      return fromInquiryHandles;
    }

    public void Refresh() => this._cachedName = (string) null;

    public uint Handle => this._hDev;

    public BluetoothAddress DeviceAddress => this._addr;

    public string DeviceName
    {
      get
      {
        if (this._cachedName == null)
        {
          byte[] numArray = new byte[500];
          ushort length = checked ((ushort) numArray.Length);
          BtSdkError btSdkError = this._factory.Api.Btsdk_GetRemoteDeviceName(this._hDev, numArray, ref length);
          if (btSdkError == BtSdkError.OPERATION_FAILURE)
          {
            length = checked ((ushort) numArray.Length);
            btSdkError = this._factory.Api.Btsdk_UpdateRemoteDeviceName(this._hDev, numArray, ref length);
          }
          if (btSdkError == BtSdkError.OK)
            this._cachedName = BluesoleilUtils.FromNameString(numArray, new ushort?(length));
          if (this._cachedName == null)
            this._cachedName = this.DeviceAddress.ToString("C", (IFormatProvider) CultureInfo.InvariantCulture);
        }
        return this._cachedName;
      }
      set => this._cachedName = value;
    }

    public int Rssi
    {
      get
      {
        sbyte prssi;
        return this._factory.Api.Btsdk_GetRemoteRSSI(this._hDev, out prssi) == BtSdkError.OK ? (int) prssi : int.MinValue;
      }
    }

    public ClassOfDevice ClassOfDevice => this._cod == null ? new ClassOfDevice(0U) : this._cod;

    public bool Authenticated => this._paired;

    public bool Remembered => !this._remembered && this.Authenticated || this._remembered;

    public bool Connected => this._connected;

    public DateTime LastSeen => this._lastSeen;

    public DateTime LastUsed => DateTime.MinValue;

    public void Merge(IBluetoothDeviceInfo other)
    {
      this._paired = other.Authenticated;
      if (this._cachedName == null)
        this._cachedName = other.DeviceName;
      this._remembered = other.Remembered;
    }

    public void SetDiscoveryTime(DateTime dt)
    {
      this._lastSeen = !(this._lastSeen != DateTime.MinValue) ? dt : throw new InvalidOperationException("LastSeen is already set.");
    }

    RadioVersions IBluetoothDeviceInfo.GetVersions()
    {
      return this._versions != null ? this._versions : throw new InvalidOperationException("Unknown error.");
    }

    public byte[][] GetServiceRecordsUnparsed(Guid service)
    {
      throw new NotSupportedException("Can't get the raw record from the Widcomm stack.");
    }

    public ServiceRecord[] GetServiceRecords(Guid service)
    {
      Structs.BtSdkSDPSearchPatternStru[] psch_ptn = new Structs.BtSdkSDPSearchPatternStru[1]
      {
        new Structs.BtSdkSDPSearchPatternStru(service)
      };
      uint[] svc_hdl1 = new uint[100];
      int length1 = svc_hdl1.Length;
      int length2 = psch_ptn.Length;
      BtSdkError ret = this._factory.Api.Btsdk_BrowseRemoteServicesEx(this._hDev, psch_ptn, length2, svc_hdl1, ref length1);
      if (ret == BtSdkError.NO_SERVICE)
        return new ServiceRecord[0];
      BluesoleilUtils.CheckAndThrow(ret, "Btsdk_BrowseRemoteServicesEx");
      List<ServiceRecord> serviceRecordList = new List<ServiceRecord>();
      for (int index = 0; index < length1; ++index)
      {
        uint svc_hdl2 = svc_hdl1[index];
        Structs.BtSdkRemoteServiceAttrStru remoteServiceAttrStru = new Structs.BtSdkRemoteServiceAttrStru(StackConsts.AttributeLookup.ServiceName | StackConsts.AttributeLookup.ExtAttributes);
        BluesoleilUtils.CheckAndThrow(this._factory.Api.Btsdk_GetRemoteServiceAttributes(svc_hdl2, ref remoteServiceAttrStru), "Btsdk_RefreshRemoteServiceAttributes");
        IBluesoleilApi api = this._factory.Api;
        ServiceRecord serviceRecord = BluesoleilDeviceInfo.CreateServiceRecord(ref remoteServiceAttrStru, api);
        serviceRecordList.Add(serviceRecord);
      }
      return serviceRecordList.ToArray();
    }

    internal static ServiceRecord CreateServiceRecord(
      ref Structs.BtSdkRemoteServiceAttrStru attrs,
      IBluesoleilApi api)
    {
      ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
      Guid bluetoothUuid = BluetoothService.CreateBluetoothUuid(attrs.svc_class);
      serviceRecordBuilder.AddServiceClass(bluetoothUuid);
      string serviceName = BluesoleilDeviceInfo.ParseServiceName(ref attrs);
      if (serviceName.Length != 0)
        serviceRecordBuilder.ServiceName = serviceName;
      byte? nullable = new byte?();
      List<ServiceAttribute> serviceAttributeList = new List<ServiceAttribute>();
      if (attrs.ext_attributes != IntPtr.Zero)
      {
        if (bluetoothUuid == BluetoothService.HumanInterfaceDevice)
        {
          Structs.BtSdkRmtHidSvcExtAttrStru_HACK structure = (Structs.BtSdkRmtHidSvcExtAttrStru_HACK) Marshal.PtrToStructure(attrs.ext_attributes, typeof (Structs.BtSdkRmtHidSvcExtAttrStru_HACK));
          List<ServiceAttribute> collection = new List<ServiceAttribute>();
          if (structure.deviceReleaseNumber != (ushort) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 512, new ServiceElement(ElementType.UInt16, (object) structure.deviceReleaseNumber)));
          if (structure.deviceSubclass != (byte) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 514, new ServiceElement(ElementType.UInt8, (object) structure.deviceSubclass)));
          if (structure.countryCode != (byte) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 515, new ServiceElement(ElementType.UInt8, (object) structure.countryCode)));
          serviceAttributeList.AddRange((IEnumerable<ServiceAttribute>) collection);
        }
        else if (bluetoothUuid == BluetoothService.PnPInformation)
        {
          Structs.BtSdkRmtDISvcExtAttrStru structure = (Structs.BtSdkRmtDISvcExtAttrStru) Marshal.PtrToStructure(attrs.ext_attributes, typeof (Structs.BtSdkRmtDISvcExtAttrStru));
          List<ServiceAttribute> collection = new List<ServiceAttribute>();
          if (structure.spec_id != (ushort) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 512, new ServiceElement(ElementType.UInt16, (object) structure.spec_id)));
          if (structure.vendor_id != (ushort) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 513, new ServiceElement(ElementType.UInt16, (object) structure.vendor_id)));
          if (structure.product_id != (ushort) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 514, new ServiceElement(ElementType.UInt16, (object) structure.product_id)));
          if (structure.version != (ushort) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 515, new ServiceElement(ElementType.UInt16, (object) structure.version)));
          collection.Add(new ServiceAttribute((ServiceAttributeId) 516, new ServiceElement(ElementType.Boolean, (object) structure.primary_record)));
          if (structure.vendor_id_source != (ushort) 0)
            collection.Add(new ServiceAttribute((ServiceAttributeId) 517, new ServiceElement(ElementType.UInt16, (object) structure.vendor_id_source)));
          serviceAttributeList.AddRange((IEnumerable<ServiceAttribute>) collection);
        }
        else
          nullable = new byte?(((Structs.BtSdkRmtSPPSvcExtAttrStru) Marshal.PtrToStructure(attrs.ext_attributes, typeof (Structs.BtSdkRmtSPPSvcExtAttrStru))).server_channel);
        api.Btsdk_FreeMemory(attrs.ext_attributes);
      }
      Structs.BtSdkAppExtSPPAttrStru psvc = new Structs.BtSdkAppExtSPPAttrStru(bluetoothUuid);
      switch (api.Btsdk_SearchAppExtSPPService(attrs.dev_hdl, ref psvc))
      {
        case BtSdkError.OK:
          if (psvc.rf_svr_chnl != (byte) 0)
          {
            byte rfSvrChnl = psvc.rf_svr_chnl;
            if (!nullable.HasValue)
              nullable = new byte?(rfSvrChnl);
          }
          if (psvc.sdp_record_handle != 0U)
          {
            serviceRecordBuilder.AddCustomAttribute(new ServiceAttribute((ServiceAttributeId) 0, ServiceElement.CreateNumericalServiceElement(ElementType.UInt32, (object) psvc.sdp_record_handle)));
            break;
          }
          break;
      }
      if (!nullable.HasValue)
        serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.None;
      if (serviceAttributeList.Count != 0)
        serviceRecordBuilder.AddCustomAttributes((IEnumerable<ServiceAttribute>) serviceAttributeList);
      serviceRecordBuilder.AddCustomAttribute(new ServiceAttribute((ServiceAttributeId) -1, new ServiceElement(ElementType.TextString, (object) "<partial BlueSoleil decode>")));
      ServiceRecord serviceRecord = serviceRecordBuilder.ServiceRecord;
      if (nullable.HasValue)
        ServiceRecordHelper.SetRfcommChannelNumber(serviceRecord, nullable.Value);
      else
        serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.None;
      return serviceRecord;
    }

    private static string ParseServiceName(ref Structs.BtSdkRemoteServiceAttrStru attrs)
    {
      string stringUtf8 = BluesoleilUtils.FixedLengthArrayToStringUtf8(attrs.svc_name);
      string str = Encoding.Unicode.GetString(attrs.svc_name).Trim(new char[1]);
      string serviceName = stringUtf8;
      if (str.Length > serviceName.Length)
        serviceName = str;
      return serviceName;
    }

    public IAsyncResult BeginGetServiceRecords(Guid service, AsyncCallback callback, object state)
    {
      if (this._dlgtGetServiceRecords == null)
        this._dlgtGetServiceRecords = new Func<Guid, ServiceRecord[]>(this.GetServiceRecords);
      return this._dlgtGetServiceRecords.BeginInvoke(service, callback, state);
    }

    public ServiceRecord[] EndGetServiceRecords(IAsyncResult asyncResult)
    {
      return this._dlgtGetServiceRecords.EndInvoke(asyncResult);
    }

    public Guid[] InstalledServices => throw new NotImplementedException();

    public void SetServiceState(Guid service, bool state, bool throwOnError)
    {
      if (throwOnError)
      {
        this.SetServiceStateDoIt(service, state);
      }
      else
      {
        try
        {
          this.SetServiceStateDoIt(service, state);
        }
        catch (SocketException ex)
        {
        }
      }
    }

    public void SetServiceStateDoIt(Guid service, bool state)
    {
      ushort? asClassId16 = BluetoothService.GetAsClassId16(service);
      if (!asClassId16.HasValue)
        throw new ArgumentException("BlueSoleil only supports standard Bluetooth UUID16 services.");
      if (state)
      {
        if (this._factory.Api.Btsdk_ConnectEx(this._hDev, asClassId16.Value, 0U, out uint _) != BtSdkError.OK)
          throw new Win32Exception(1060, "Failed to enable the service.");
      }
      else
      {
        IList<uint> connection = this.FindConnection(this._hDev, asClassId16.Value);
        if (connection == null || connection.Count == 0)
          throw new Win32Exception(1168, "No matching enabled service found.");
        if (this._factory.Api.Btsdk_Disconnect(connection[0]) != BtSdkError.OK)
          throw new Win32Exception(1060, "Failed to disabled the service.");
      }
    }

    public void SetServiceState(Guid service, bool state)
    {
      this.SetServiceState(service, state, false);
    }

    private IList<uint> FindConnection(uint hDev, ushort classId)
    {
      List<uint> connection = new List<uint>();
      uint enum_handle = this._factory.Api.Btsdk_StartEnumConnection();
      if (enum_handle == 0U)
        return (IList<uint>) connection;
      while (true)
      {
        Structs.BtSdkConnectionPropertyStru conn_prop;
        uint num;
        do
        {
          conn_prop = new Structs.BtSdkConnectionPropertyStru();
          num = this._factory.Api.Btsdk_EnumConnection(enum_handle, ref conn_prop);
          if (num == 0U)
            goto label_5;
        }
        while ((int) conn_prop.device_handle != (int) hDev || (int) conn_prop.service_class != (int) classId || conn_prop.Role != StackConsts.BTSDK_CONNROLE.Initiator);
        connection.Add(num);
      }
label_5:
      BluesoleilUtils.Assert(this._factory.Api.Btsdk_EndEnumConnection(enum_handle), "Btsdk_EndEnumConnection");
      return (IList<uint>) connection;
    }

    public void ShowDialog() => throw new NotImplementedException();

    public void Update() => throw new NotImplementedException();
  }
}
