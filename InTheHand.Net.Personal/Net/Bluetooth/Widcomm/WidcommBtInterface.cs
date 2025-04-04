// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBtInterface
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBtInterface : IDisposable
  {
    private const int MaxNumberSdpRecords = 10;
    private const string DevicesRegPath = "Software\\WIDCOMM\\BTConfig\\Devices\\";
    private volatile bool _disposed;
    private readonly WidcommBluetoothFactoryBase m_factory;
    private readonly IBtIf m_btIf;
    private readonly WidcommBtInterface.WidcommInquiry _inquiryHandler;
    private object lockServiceDiscovery = new object();
    private AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams> m_arServiceDiscovery;

    internal WidcommBtInterface(IBtIf btIf, WidcommBluetoothFactoryBase factory)
    {
      this.m_factory = factory;
      this._inquiryHandler = new WidcommBtInterface.WidcommInquiry(this.m_factory, new ThreadStart(this.StopInquiry));
      bool flag = false;
      try
      {
        this.m_btIf = btIf;
        this.m_btIf.SetParent(this);
        this.m_btIf.Create();
        flag = true;
      }
      finally
      {
        if (!flag)
          GC.SuppressFinalize((object) this);
      }
    }

    private void EnsureLoaded()
    {
      if (!this._disposed)
        return;
      this.m_factory.EnsureLoaded();
    }

    void IDisposable.Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~WidcommBtInterface() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      this._disposed = true;
      this.m_btIf.Destroy(disposing);
    }

    internal IAsyncResult BeginInquiry(
      int maxDevices,
      TimeSpan inquiryLength,
      AsyncCallback asyncCallback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args)
    {
      return this._inquiryHandler.BeginInquiry(maxDevices, inquiryLength, asyncCallback, state, liveDiscoHandler, liveDiscoState, new ThreadStart(this.StartInquiry), args);
    }

    private void StartInquiry()
    {
      if (!this.m_btIf.StartInquiry())
        throw WidcommSocketExceptions.Create_StartInquiry(nameof (StartInquiry));
    }

    private void StopInquiry()
    {
      MiscUtils.Trace_WriteLine(nameof (StopInquiry));
      this.m_btIf.StopInquiry();
    }

    internal List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      return ((AsyncResult<List<IBluetoothDeviceInfo>>) ar).EndInvoke();
    }

    internal void HandleDeviceResponded(
      byte[] bdAddr,
      byte[] devClass,
      byte[] deviceName,
      bool connected)
    {
      this._inquiryHandler.HandleDeviceResponded(bdAddr, devClass, deviceName, connected);
    }

    internal void HandleInquiryComplete(bool success, ushort numResponses)
    {
      this._inquiryHandler.HandleInquiryComplete(success, numResponses);
    }

    public IAsyncResult BeginServiceDiscovery(
      BluetoothAddress address,
      Guid serviceGuid,
      SdpSearchScope searchScope,
      AsyncCallback asyncCallback,
      object state)
    {
      this.BeginServiceDiscoveryKillInquiry();
      BluetoothAddress address1 = (BluetoothAddress) address.Clone();
      AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams> asyncResult = new AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams>(asyncCallback, state, new ServiceDiscoveryParams(address1, serviceGuid, searchScope));
      lock (this.lockServiceDiscovery)
      {
        if (this.m_arServiceDiscovery != null)
          throw new NotSupportedException("Currently support only one concurrent Service Lookup operation.");
        bool flag = false;
        try
        {
          this.m_arServiceDiscovery = asyncResult;
          if (!this.m_btIf.StartDiscovery(address1, serviceGuid))
            throw WidcommSocketExceptions.Create_StartDiscovery(this.GetExtendedError());
          flag = true;
        }
        finally
        {
          if (!flag)
            this.m_arServiceDiscovery = (AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams>) null;
        }
      }
      return (IAsyncResult) asyncResult;
    }

    private void BeginServiceDiscoveryKillInquiry()
    {
      MiscUtils.Trace_WriteLine("BeginServiceDiscovery gonna call StopInquiry.");
      this.StopInquiry();
    }

    public ISdpDiscoveryRecordsBuffer EndServiceDiscovery(IAsyncResult asyncResult)
    {
      return ((AsyncResult<ISdpDiscoveryRecordsBuffer>) asyncResult).EndInvoke();
    }

    internal void HandleDiscoveryComplete()
    {
      MiscUtils.Trace_WriteLine(nameof (HandleDiscoveryComplete));
      AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams> sacAr = (AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams>) null;
      ISdpDiscoveryRecordsBuffer recBuf = (ISdpDiscoveryRecordsBuffer) null;
      Exception sacEx = (Exception) null;
      try
      {
        lock (this.lockServiceDiscovery)
        {
          if (this.m_arServiceDiscovery == null)
            return;
          sacAr = this.m_arServiceDiscovery;
          this.m_arServiceDiscovery = (AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams>) null;
          BluetoothAddress address;
          DISCOVERY_RESULT lastDiscoveryResult = this.m_btIf.GetLastDiscoveryResult(out address, out ushort _);
          if (lastDiscoveryResult != DISCOVERY_RESULT.SUCCESS)
            sacEx = (Exception) WidcommSocketExceptions.Create(lastDiscoveryResult, "ServiceRecordsGetResult");
          else if (!address.Equals((object) sacAr.BeginParameters.address))
            sacEx = (Exception) new InvalidOperationException("Internal error -- different DiscoveryComplete address.");
          else
            recBuf = this.m_btIf.ReadDiscoveryRecords(address, 10, sacAr.BeginParameters);
        }
      }
      catch (Exception ex)
      {
        sacEx = ex;
      }
      finally
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          WidcommBtInterface.RaiseDiscoveryComplete(sacAr, recBuf, sacEx);
        });
      }
    }

    private static void RaiseDiscoveryComplete(
      AsyncResult<ISdpDiscoveryRecordsBuffer, ServiceDiscoveryParams> sacAr,
      ISdpDiscoveryRecordsBuffer recBuf,
      Exception sacEx)
    {
      if (sacAr == null)
        return;
      if (sacEx != null)
        sacAr.SetAsCompleted(sacEx, false);
      else
        sacAr.SetAsCompleted(recBuf, false);
    }

    public List<IBluetoothDeviceInfo> GetKnownRemoteDeviceEntries()
    {
      List<REM_DEV_INFO> remDevInfoList = new List<REM_DEV_INFO>();
      REM_DEV_INFO remDevInfo = new REM_DEV_INFO();
      int cb = Marshal.SizeOf(typeof (REM_DEV_INFO));
      IntPtr num = Marshal.AllocHGlobal(cb);
      try
      {
        REM_DEV_INFO_RETURN_CODE remoteDeviceInfo1 = this.m_btIf.GetRemoteDeviceInfo(ref remDevInfo, num, cb);
        MiscUtils.Trace_WriteLine("GRDI: ret: {0}=0x{0:X}", (object) remoteDeviceInfo1);
        while (remoteDeviceInfo1 == REM_DEV_INFO_RETURN_CODE.SUCCESS)
        {
          remDevInfoList.Add(remDevInfo);
          remoteDeviceInfo1 = this.m_btIf.GetNextRemoteDeviceInfo(ref remDevInfo, num, cb);
          MiscUtils.Trace_WriteLine("GnRDI: ret: {0}=0x{0:X}", (object) remoteDeviceInfo1);
        }
        if (remoteDeviceInfo1 != REM_DEV_INFO_RETURN_CODE.EOF)
          throw WidcommSocketExceptions.Create(remoteDeviceInfo1, "Get[Next]RemoteDeviceInfo");
        List<IBluetoothDeviceInfo> remoteDeviceEntries = new List<IBluetoothDeviceInfo>(remDevInfoList.Count);
        foreach (REM_DEV_INFO rdi in remDevInfoList)
        {
          IBluetoothDeviceInfo remoteDeviceInfo2 = (IBluetoothDeviceInfo) WidcommBluetoothDeviceInfo.CreateFromStoredRemoteDeviceInfo(rdi, this.m_factory);
          remoteDeviceEntries.Add(remoteDeviceInfo2);
        }
        return remoteDeviceEntries;
      }
      finally
      {
        Marshal.FreeHGlobal(num);
      }
    }

    private static string GetWidcommDeviceKeyName(BluetoothAddress device)
    {
      return device.ToString("C").ToLower(CultureInfo.InvariantCulture);
    }

    public List<IBluetoothDeviceInfo> ReadKnownDevicesFromRegistry()
    {
      List<IBluetoothDeviceInfo> bluetoothDeviceInfoList = new List<IBluetoothDeviceInfo>();
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\WIDCOMM\\BTConfig\\Devices\\"))
      {
        if (registryKey == null)
          return bluetoothDeviceInfoList;
        foreach (string subKeyName in registryKey.GetSubKeyNames())
        {
          using (RegistryKey rkItem = registryKey.OpenSubKey(subKeyName))
          {
            WidcommBluetoothDeviceInfo bluetoothDeviceInfo = this.ReadDeviceFromRegistryAndCheckAndSetIfPaired_(subKeyName, rkItem, this.m_factory);
            bluetoothDeviceInfoList.Add((IBluetoothDeviceInfo) bluetoothDeviceInfo);
          }
        }
      }
      return bluetoothDeviceInfoList;
    }

    private WidcommBluetoothDeviceInfo ReadDeviceFromRegistryAndCheckAndSetIfPaired_(
      string itemName,
      RegistryKey rkItem,
      WidcommBluetoothFactoryBase factory)
    {
      BluetoothAddress devAddress = BluetoothAddress.Parse(itemName);
      byte[] devName;
      byte[] devClass;
      try
      {
        devName = WidcommBtInterface.Registry_ReadBinaryValue(rkItem, "Name");
        devClass = WidcommBtInterface.Registry_ReadBinaryValue(rkItem, "DevClass");
      }
      catch (IOException ex)
      {
        return (WidcommBluetoothDeviceInfo) null;
      }
      WidcommBtInterface.Registry_ReadDwordValue_Optional(rkItem, "TrustedMask");
      WidcommBluetoothDeviceInfo remoteDeviceInfo = WidcommBtInterface.CreateFromStoredRemoteDeviceInfo(devAddress, devName, devClass, factory);
      WidcommBluetoothDeviceInfo.CheckAndSetIfPaired(remoteDeviceInfo, factory);
      return remoteDeviceInfo;
    }

    internal WidcommBluetoothDeviceInfo ReadDeviceFromRegistryAndCheckAndSetIfPaired(
      BluetoothAddress address,
      WidcommBluetoothFactoryBase factory)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\WIDCOMM\\BTConfig\\Devices\\"))
      {
        if (registryKey == null)
          return (WidcommBluetoothDeviceInfo) null;
        string str = address.ToString("C");
        using (RegistryKey rkItem = registryKey.OpenSubKey(str))
          return rkItem == null ? (WidcommBluetoothDeviceInfo) null : this.ReadDeviceFromRegistryAndCheckAndSetIfPaired_(str, rkItem, factory);
      }
    }

    private static WidcommBluetoothDeviceInfo CreateFromStoredRemoteDeviceInfo(
      BluetoothAddress devAddress,
      byte[] devName,
      byte[] devClass,
      WidcommBluetoothFactoryBase factory)
    {
      WidcommBluetoothDeviceInfo remoteDeviceInfo = WidcommBluetoothDeviceInfo.CreateFromStoredRemoteDeviceInfo(new REM_DEV_INFO()
      {
        bda = WidcommUtils.FromBluetoothAddress(devAddress),
        bd_name = devName,
        dev_class = devClass
      }, factory);
      string deviceName = remoteDeviceInfo.DeviceName;
      return remoteDeviceInfo;
    }

    private static byte[] Registry_ReadBinaryValue(RegistryKey rkItem, string name)
    {
      WidcommBtInterface.Registry_CheckIsKind(rkItem, name, RegistryValueKind.Binary);
      return (byte[]) rkItem.GetValue(name);
    }

    private static int? Registry_ReadDwordValue_Optional(RegistryKey rkItem, string name)
    {
      object obj = rkItem.GetValue(name);
      if (obj == null)
        return new int?();
      WidcommBtInterface.Registry_CheckIsKind(rkItem, name, RegistryValueKind.DWord);
      return new int?((int) obj);
    }

    private static void Registry_CheckIsKind(
      RegistryKey rkItem,
      string name,
      RegistryValueKind expectedKind)
    {
      if (PlatformVerification.IsMonoRuntime)
      {
        MiscUtils.Trace_WriteLine("Skipping Registry_CheckIsKind check on Mono as it's not supported.");
      }
      else
      {
        RegistryValueKind valueKind = rkItem.GetValueKind(name);
        if (valueKind != expectedKind)
          throw new FormatException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Expected '{0}':'{1}', to be '{2}' but was '{3}'.", (object) rkItem.Name, (object) name, (object) expectedKind, (object) valueKind));
      }
    }

    internal static bool DeleteKnownDevice(BluetoothAddress device)
    {
      string widcommDeviceKeyName = WidcommBtInterface.GetWidcommDeviceKeyName(device);
      using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("Software\\WIDCOMM\\BTConfig\\Devices\\", true))
      {
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey(widcommDeviceKeyName, false))
        {
          if (registryKey2 == null)
            return true;
        }
        try
        {
          registryKey1.DeleteSubKeyTree(widcommDeviceKeyName);
          return true;
        }
        catch (SecurityException ex)
        {
          MiscUtils.Trace_WriteLine("DeleteKnownDevice DeleteSubKeyTree(" + widcommDeviceKeyName + "): " + ExceptionExtension.ToStringNoStackTrace((Exception) ex));
        }
        catch (UnauthorizedAccessException ex)
        {
          MiscUtils.Trace_WriteLine("DeleteKnownDevice DeleteSubKeyTree(" + widcommDeviceKeyName + "): " + ExceptionExtension.ToStringNoStackTrace((Exception) ex));
        }
        return false;
      }
    }

    internal bool GetLocalDeviceVersionInfo(ref DEV_VER_INFO m_dvi)
    {
      return this.m_btIf.GetLocalDeviceVersionInfo(ref m_dvi);
    }

    internal bool GetLocalDeviceInfoBdAddr(byte[] bdAddr)
    {
      return this.m_btIf.GetLocalDeviceInfoBdAddr(bdAddr);
    }

    internal bool GetLocalDeviceName(byte[] bdName) => this.m_btIf.GetLocalDeviceName(bdName);

    internal void IsStackUpAndRadioReady(out bool stackServerUp, out bool deviceReady)
    {
      this.m_btIf.IsStackUpAndRadioReady(out stackServerUp, out deviceReady);
    }

    internal void IsDeviceConnectableDiscoverable(out bool conno, out bool disco)
    {
      this.m_btIf.IsDeviceConnectableDiscoverable(out conno, out disco);
    }

    internal void SetDeviceConnectableDiscoverable(
      bool connectable,
      bool forPairedOnly,
      bool discoverable)
    {
      if (connectable || discoverable)
        this.EnsureLoaded();
      this.m_btIf.SetDeviceConnectableDiscoverable(connectable, forPairedOnly, discoverable);
    }

    internal int GetRssi(byte[] bd_addr) => this.m_btIf.GetRssi(bd_addr);

    internal bool BondQuery(byte[] bd_addr) => this.m_btIf.BondQuery(bd_addr);

    internal BOND_RETURN_CODE Bond(BluetoothAddress address, string passphrase)
    {
      return this.m_btIf.Bond(address, passphrase);
    }

    internal bool UnBond(BluetoothAddress address) => this.m_btIf.UnBond(address);

    private WBtRc GetExtendedError() => this.m_btIf.GetExtendedError();

    internal SDK_RETURN_CODE IsRemoteDevicePresent(byte[] bd_addr)
    {
      return this.m_btIf.IsRemoteDevicePresent(bd_addr);
    }

    internal bool IsRemoteDeviceConnected(byte[] bd_addr)
    {
      return this.m_btIf.IsRemoteDeviceConnected(bd_addr);
    }

    [DebuggerStepThrough]
    public static bool IsWidcommSingleThread(WidcommPortSingleThreader st)
    {
      return WidcommPortSingleThreader.IsWidcommSingleThread(st);
    }

    private static T GetStaticData<T>(LocalDataStoreSlot slot) where T : struct
    {
      object data = Thread.GetData(slot);
      return data == null ? default (T) : (T) data;
    }

    private class WidcommInquiry : CommonBluetoothInquiry<IBluetoothDeviceInfo>
    {
      private readonly WidcommBluetoothFactoryBase m_factory;
      private ThreadStart _stopInquiry;

      internal WidcommInquiry(WidcommBluetoothFactoryBase factory, ThreadStart stopInquiry)
      {
        this.m_factory = factory;
        this._stopInquiry = stopInquiry;
      }

      protected override IBluetoothDeviceInfo CreateDeviceInfo(IBluetoothDeviceInfo item) => item;

      internal void HandleDeviceResponded(
        byte[] bdAddr,
        byte[] devClass,
        byte[] deviceName,
        bool connected)
      {
        MiscUtils.Trace_WriteLine(nameof (HandleDeviceResponded));
        WidcommBluetoothDeviceInfo handleDeviceResponded = WidcommBluetoothDeviceInfo.CreateFromHandleDeviceResponded(bdAddr, deviceName, devClass, connected, this.m_factory);
        MiscUtils.Trace_WriteLine("HDR: {0} {1} {2} {3}", (object) WidcommBtInterface.WidcommInquiry.ToStringQuotedOrNull(bdAddr), (object) WidcommBtInterface.WidcommInquiry.ToStringQuotedOrNull(devClass), (object) WidcommBtInterface.WidcommInquiry.ToStringQuotedOrNull(deviceName), (object) connected);
        this.HandleInquiryResultInd((IBluetoothDeviceInfo) handleDeviceResponded);
        MiscUtils.Trace_WriteLine("exit HDR");
      }

      private static string ToStringQuotedOrNull(byte[] array)
      {
        return array == null ? "(null)" : "\"" + BitConverter.ToString(array) + "\"";
      }

      internal void HandleInquiryComplete(bool success, ushort numResponses)
      {
        MiscUtils.Trace_WriteLine(nameof (HandleInquiryComplete));
        this.HandleInquiryComplete(new int?((int) numResponses));
        MiscUtils.Trace_WriteLine("exit HandleInquiryComplete");
      }

      protected override void StopInquiry() => this._stopInquiry();
    }
  }
}
