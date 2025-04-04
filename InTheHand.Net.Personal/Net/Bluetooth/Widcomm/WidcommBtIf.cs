// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBtIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBtIf : IBtIf
  {
    private const string ModuleName32feet = "32feetWidcomm";
    private const string ModuleNameWidcomm1 = "btwapi";
    private const string ModuleNameWidcomm0 = "wbtapi";
    private const string ErrorMsgNeedNativeDllUpgrade = "Need to upgrade your 32feetWidcomm.dll!";
    private static volatile bool s_alreadyExists;
    private IntPtr m_pBtIf;
    private WidcommBtInterface m_parent;
    private readonly WidcommBluetoothFactory _factory;
    private WidcommBtIf.NativeMethods.OnDeviceResponded m_handleDeviceResponded;
    private WidcommBtIf.NativeMethods.OnInquiryComplete m_handleInquiryComplete;
    private WidcommBtIf.NativeMethods.OnDiscoveryComplete m_handleDiscoveryComplete;
    private WidcommBtIf.NativeMethods.OnStackStatusChange m_handleStackStatusChange;

    internal WidcommBtIf(WidcommBluetoothFactory fcty)
    {
      WidcommBtIf.s_alreadyExists = !WidcommBtIf.s_alreadyExists ? true : throw new InvalidOperationException("May only be one instance of WidcommBtIf.");
      this._factory = fcty;
    }

    public void SetParent(WidcommBtInterface parent)
    {
      this.m_parent = this.m_parent == null ? parent : throw new InvalidOperationException("Can only have one parent.");
    }

    public void Create()
    {
      if (this.m_pBtIf != IntPtr.Zero)
        throw new InvalidOperationException("Create already called.");
      this.m_handleDeviceResponded = new WidcommBtIf.NativeMethods.OnDeviceResponded(this.HandleDeviceResponded);
      this.m_handleInquiryComplete = new WidcommBtIf.NativeMethods.OnInquiryComplete(this.HandleInquiryComplete);
      this.m_handleDiscoveryComplete = new WidcommBtIf.NativeMethods.OnDiscoveryComplete(this.HandleDiscoveryComplete);
      this.m_handleStackStatusChange = new WidcommBtIf.NativeMethods.OnStackStatusChange(this.HandleStackStatusChange);
      bool flag = false;
      try
      {
        WidcommBtIf.NativeMethods.BtIf_Create(out this.m_pBtIf, this.m_handleDeviceResponded, this.m_handleInquiryComplete, this.m_handleDiscoveryComplete);
        if (this.m_pBtIf == IntPtr.Zero)
          throw new InvalidOperationException("Failed to initialise CBtIf.");
        flag = true;
      }
      catch (Exception ex)
      {
        int num = (int) WidcommBtIf.CheckDependencies(ex);
      }
      finally
      {
        if (!flag)
          WidcommBtIf.s_alreadyExists = false;
      }
      this.SetCallback2();
    }

    private void SetCallback2()
    {
      try
      {
        WidcommBtIf.NativeMethods.OnStackStatusChange stackStatusChange1 = this.m_handleStackStatusChange;
        WidcommBtIf.NativeMethods.OnStackStatusChange stackStatusChange2 = (WidcommBtIf.NativeMethods.OnStackStatusChange) null;
        if (stackStatusChange2 == null || WidcommBtIf.NativeMethods.BtIf_SetCallback2(this.m_pBtIf, 1, stackStatusChange2) >= 1)
          return;
        WidcommBtIf.ReportNeedNeedNativeDllUpgrade((Exception) new ArgumentException("BtIf_SetCallback2 returned " + (object) 1 + ", but wanted: " + (object) 1 + "."), true);
      }
      catch (EntryPointNotFoundException ex)
      {
        WidcommBtIf.ReportNeedNeedNativeDllUpgrade((Exception) ex, false);
      }
      catch (MissingMethodException ex)
      {
        WidcommBtIf.ReportNeedNeedNativeDllUpgrade((Exception) ex, false);
      }
    }

    public void Destroy(bool disposing)
    {
      MiscUtils.Trace_WriteLine("WidcommBtIf.Destroy");
      if (this.m_pBtIf != IntPtr.Zero)
      {
        WidcommBtIf.NativeMethods.BtIf_Destroy(this.m_pBtIf);
        this.m_pBtIf = IntPtr.Zero;
      }
      WidcommBtIf.s_alreadyExists = false;
    }

    private static string GetWidcommInstalledModuleName() => "wbtapi";

    private static WidcommBtIf.LibraryStatus CheckDependencies(Exception wrapException)
    {
      string[] strArray = new string[3]
      {
        "32feetWidcomm",
        "wbtapi",
        "btwapi"
      };
      foreach (string moduleName in strArray)
      {
        WidcommBtIf.LibraryStatus status;
        try
        {
          status = WidcommBtIf.CheckLibraryDependency(moduleName);
        }
        catch (Exception ex)
        {
          string message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "'{0}' status exception: {1}!", (object) moduleName, (object) ex);
          MiscUtils.Trace_WriteLine(message);
          if (wrapException != null)
            throw new PlatformNotSupportedException(message, ex);
          status = WidcommBtIf.LibraryStatus.Exception;
        }
        string message1 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Dependency DLL '{0}' status: {1}.", (object) moduleName, (object) status);
        MiscUtils.Trace_WriteLine(message1);
        if (wrapException != null)
        {
          if (!WidcommBtIf.IsFound(status))
            throw new PlatformNotSupportedException(message1, wrapException);
        }
        else if (status != WidcommBtIf.LibraryStatus.ModuleLoaded && status != WidcommBtIf.LibraryStatus.LoadLibraryAccessible)
          return status;
      }
      if (wrapException != null)
        throw new PlatformNotSupportedException(wrapException.Message, wrapException);
      return WidcommBtIf.LibraryStatus.AggregateOk;
    }

    public static Exception IsWidcommStackPresentButNotInterfaceDll()
    {
      if (WidcommBtIf.CheckLibraryDependency(WidcommBtIf.GetWidcommInstalledModuleName()) == WidcommBtIf.LibraryStatus.NotFound)
        return (Exception) null;
      if (WidcommBtIf.CheckLibraryDependency("32feetWidcomm") != WidcommBtIf.LibraryStatus.NotFound)
        return (Exception) null;
      OperatingSystem osVersion = Environment.OSVersion;
      return osVersion.Version.Major >= 6 && osVersion.Platform == PlatformID.Win32NT && !BluetoothFactoryConfig.WidcommICheckIgnorePlatform ? (Exception) null : (Exception) new PlatformNotSupportedException("Widcomm stack seems to be present, need to install 32feetWidcomm.dll alongside.");
    }

    private static bool IsFound(WidcommBtIf.LibraryStatus status)
    {
      return status == WidcommBtIf.LibraryStatus.ModuleLoaded || status == WidcommBtIf.LibraryStatus.LoadLibraryAccessible;
    }

    private static WidcommBtIf.LibraryStatus CheckLibraryDependency(string moduleName)
    {
      if (WidcommBtIf.NativeMethods.GetModuleHandle(moduleName) != IntPtr.Zero)
        return WidcommBtIf.LibraryStatus.ModuleLoaded;
      WidcommBtIf.NativeMethods.LoadLibraryExFlags dwFlags = WidcommBtIf.NativeMethods.LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE;
      IntPtr hModule = WidcommBtIf.NativeMethods.LoadLibraryEx(moduleName, IntPtr.Zero, dwFlags);
      if (!(hModule != IntPtr.Zero))
        return WidcommBtIf.LibraryStatus.NotFound;
      WidcommBtIf.NativeMethods.FreeLibrary(hModule);
      return WidcommBtIf.LibraryStatus.LoadLibraryAccessible;
    }

    private void HandleStackStatusChange(int new_status)
    {
      STACK_STATUS stackStatus = (STACK_STATUS) new_status;
      MiscUtils.Trace_WriteLine(WidcommUtils.GetTime4Log() + ": StackStatusChange: {0}", (object) stackStatus);
      switch (stackStatus)
      {
        case STACK_STATUS.Down:
        case STACK_STATUS.Error:
        case STACK_STATUS.Unloaded:
          this._factory._seenStackDownEvent = true;
          ThreadPool.QueueUserWorkItem(new WaitCallback(this.UnloadedKill_Runner));
          break;
      }
    }

    private void UnloadedKill_Runner(object state)
    {
      MiscUtils.Trace_WriteLine(WidcommUtils.GetTime4Log() + ": PortKill_Runner");
      WidcommRfcommStreamBase[] portList = this._factory.GetPortList();
      foreach (CommonRfcommStream commonRfcommStream in portList)
        commonRfcommStream.CloseInternalAndAbort_willLock();
      MiscUtils.Trace_WriteLine("PortKill_Runner done ({0}).", (object) portList.Length);
    }

    public bool StartInquiry() => WidcommBtIf.NativeMethods.BtIf_StartInquiry(this.m_pBtIf);

    public void StopInquiry() => WidcommBtIf.NativeMethods.BtIf_StopInquiry(this.m_pBtIf);

    private void HandleDeviceResponded(
      IntPtr bdAddr,
      IntPtr devClass,
      IntPtr deviceName,
      bool connected)
    {
      byte[] bdAddrArr;
      byte[] devClassArr;
      byte[] deviceNameArr;
      WidcommUtils.GetBluetoothCallbackValues(bdAddr, devClass, deviceName, out bdAddrArr, out devClassArr, out deviceNameArr);
      this.m_parent.HandleDeviceResponded(bdAddrArr, devClassArr, deviceNameArr, connected);
    }

    private void HandleInquiryComplete(bool success, ushort numResponses)
    {
      this.m_parent.HandleInquiryComplete(success, numResponses);
    }

    public bool StartDiscovery(BluetoothAddress address, Guid serviceGuid)
    {
      MiscUtils.Trace_WriteLine("WidcommBtIf.StartDiscovery");
      return WidcommBtIf.NativeMethods.BtIf_StartDiscovery(this.m_pBtIf, WidcommUtils.FromBluetoothAddress(address), ref serviceGuid, out int _);
    }

    public DISCOVERY_RESULT GetLastDiscoveryResult(
      out BluetoothAddress address,
      out ushort p_num_recs)
    {
      byte[] numArray = WidcommUtils.FromBluetoothAddress(BluetoothAddress.None);
      DISCOVERY_RESULT lastDiscoveryResult = WidcommBtIf.NativeMethods.BtIf_GetLastDiscoveryResult(this.m_pBtIf, numArray, out p_num_recs);
      address = WidcommUtils.ToBluetoothAddress(numArray);
      return lastDiscoveryResult;
    }

    public ISdpDiscoveryRecordsBuffer ReadDiscoveryRecords(
      BluetoothAddress address,
      int maxRecords,
      ServiceDiscoveryParams args)
    {
      byte[] p_bda = WidcommUtils.FromBluetoothAddress(address);
      Guid serviceGuid = args.serviceGuid;
      IntPtr pSdpDiscoveryRecArray;
      int recordCount;
      if (args.searchScope == SdpSearchScope.Anywhere)
      {
        recordCount = WidcommBtIf.NativeMethods.BtIf_ReadDiscoveryRecords(this.m_pBtIf, p_bda, maxRecords, out pSdpDiscoveryRecArray);
      }
      else
      {
        try
        {
          recordCount = WidcommBtIf.NativeMethods.BtIf_ReadDiscoveryRecordsServiceClassOnly(this.m_pBtIf, p_bda, maxRecords, out pSdpDiscoveryRecArray, ref serviceGuid);
        }
        catch (EntryPointNotFoundException ex)
        {
          WidcommBtIf.ReportNeedNeedNativeDllUpgrade((Exception) ex, true);
          recordCount = WidcommBtIf.NativeMethods.BtIf_ReadDiscoveryRecords(this.m_pBtIf, p_bda, maxRecords, out pSdpDiscoveryRecArray);
        }
        catch (MissingMethodException ex)
        {
          WidcommBtIf.ReportNeedNeedNativeDllUpgrade((Exception) ex, true);
          recordCount = WidcommBtIf.NativeMethods.BtIf_ReadDiscoveryRecords(this.m_pBtIf, p_bda, maxRecords, out pSdpDiscoveryRecArray);
        }
      }
      return (ISdpDiscoveryRecordsBuffer) new SdpDiscoveryRecordsBuffer((WidcommBluetoothFactoryBase) this._factory, pSdpDiscoveryRecArray, recordCount, args);
    }

    internal static void ReportNeedNeedNativeDllUpgrade(Exception ex, bool mayAssert)
    {
      int num = mayAssert ? 1 : 0;
      MiscUtils.Trace_WriteLine("Need to upgrade your 32feetWidcomm.dll!\n" + (object) ex);
    }

    private void HandleDiscoveryComplete() => this.m_parent.HandleDiscoveryComplete();

    public REM_DEV_INFO_RETURN_CODE GetRemoteDeviceInfo(
      ref REM_DEV_INFO remDevInfo,
      IntPtr p_rem_dev_info,
      int cb)
    {
      MiscUtils.Trace_WriteLine("enter GetRemoteDeviceInfo, pBuf=0x{0:X}", (object) p_rem_dev_info.ToInt64());
      REM_DEV_INFO_RETURN_CODE remoteDeviceInfo = WidcommBtIf.NativeMethods.BtIf_GetRemoteDeviceInfo(this.m_pBtIf, p_rem_dev_info, cb);
      MiscUtils.Trace_WriteLine("exit GetRemoteDeviceInfo");
      remDevInfo = (REM_DEV_INFO) Marshal.PtrToStructure(p_rem_dev_info, typeof (REM_DEV_INFO));
      return remoteDeviceInfo;
    }

    public REM_DEV_INFO_RETURN_CODE GetNextRemoteDeviceInfo(
      ref REM_DEV_INFO remDevInfo,
      IntPtr p_rem_dev_info,
      int cb)
    {
      MiscUtils.Trace_WriteLine("enter GetNextRemoteDeviceInfo, pBuf=0x{0:X}", (object) p_rem_dev_info.ToInt64());
      REM_DEV_INFO_RETURN_CODE remoteDeviceInfo = WidcommBtIf.NativeMethods.BtIf_GetNextRemoteDeviceInfo(this.m_pBtIf, p_rem_dev_info, cb);
      MiscUtils.Trace_WriteLine("exit GetNextRemoteDeviceInfo");
      remDevInfo = (REM_DEV_INFO) Marshal.PtrToStructure(p_rem_dev_info, typeof (REM_DEV_INFO));
      return remoteDeviceInfo;
    }

    public bool GetLocalDeviceVersionInfo(ref DEV_VER_INFO devVerInfo)
    {
      int cb = Marshal.SizeOf((object) devVerInfo);
      return WidcommBtIf.NativeMethods.BtIf_GetLocalDeviceVersionInfo(this.m_pBtIf, ref devVerInfo, cb);
    }

    public bool GetLocalDeviceName(byte[] bdName)
    {
      return WidcommBtIf.NativeMethods.BtIf_GetLocalDeviceName(this.m_pBtIf, bdName, bdName.Length);
    }

    public bool GetLocalDeviceInfoBdAddr(byte[] bdAddr)
    {
      return WidcommBtIf.NativeMethods.BtIf_GetLocalDeviceInfoBdAddr(this.m_pBtIf, bdAddr, bdAddr.Length);
    }

    public void IsStackUpAndRadioReady(out bool stackServerUp, out bool deviceReady)
    {
      try
      {
        WidcommBtIf.NativeMethods.BtIf_IsStackUpAndRadioReady(this.m_pBtIf, out stackServerUp, out deviceReady);
      }
      catch (EntryPointNotFoundException ex)
      {
        MiscUtils.Trace_WriteLine("Need to upgrade your 32feetWidcomm.dll!" + "\n" + (object) ex);
        stackServerUp = deviceReady = true;
      }
      catch (MissingMethodException ex)
      {
        MiscUtils.Trace_WriteLine("Need to upgrade your 32feetWidcomm.dll!" + "\n" + (object) ex);
        stackServerUp = deviceReady = true;
      }
    }

    public void IsDeviceConnectableDiscoverable(out bool conno, out bool disco)
    {
      WidcommBtIf.NativeMethods.BtIf_IsDeviceConnectableDiscoverable(this.m_pBtIf, out conno, out disco);
    }

    public void SetDeviceConnectableDiscoverable(
      bool connectable,
      bool forPairedOnly,
      bool discoverable)
    {
      throw new NotSupportedException("No Widcomm API support.");
    }

    public int GetRssi(byte[] bd_addr)
    {
      tBT_CONN_STATS pStats = new tBT_CONN_STATS();
      return !WidcommBtIf.NativeMethods.BtIf_GetConnectionStats(this.m_pBtIf, bd_addr, out pStats, Marshal.SizeOf((object) pStats)) ? int.MinValue : pStats.rssi;
    }

    public bool BondQuery(byte[] bd_addr)
    {
      return WidcommBtIf.NativeMethods.BtIf_BondQuery(this.m_pBtIf, bd_addr);
    }

    public BOND_RETURN_CODE Bond(BluetoothAddress address, string passphrase)
    {
      byte[] bdAddr = WidcommUtils.FromBluetoothAddress(address);
      byte[] pin_code;
      if (passphrase != null)
      {
        pin_code = Encoding.UTF8.GetBytes(passphrase + "\0");
        Encoding.Unicode.GetBytes(passphrase + "\0");
      }
      else
        pin_code = (byte[]) null;
      return (BOND_RETURN_CODE) WidcommBtIf.NativeMethods.BtIf_Bond(this.m_pBtIf, bdAddr, pin_code);
    }

    private static BOND_RETURN_CODE Convert(BOND_RETURN_CODE__WCE wce)
    {
      BOND_RETURN_CODE bondReturnCode;
      switch (wce)
      {
        case BOND_RETURN_CODE__WCE.SUCCESS:
          bondReturnCode = BOND_RETURN_CODE.SUCCESS;
          break;
        case BOND_RETURN_CODE__WCE.ALREADY_BONDED:
          bondReturnCode = BOND_RETURN_CODE.ALREADY_BONDED;
          break;
        case BOND_RETURN_CODE__WCE.BAD_PARAMETER:
          bondReturnCode = BOND_RETURN_CODE.BAD_PARAMETER;
          break;
        case BOND_RETURN_CODE__WCE.FAIL:
          bondReturnCode = BOND_RETURN_CODE.FAIL;
          break;
        default:
          bondReturnCode = (BOND_RETURN_CODE) 99;
          break;
      }
      return bondReturnCode;
    }

    public bool UnBond(BluetoothAddress address)
    {
      return WidcommBtIf.NativeMethods.BtIf_UnBond(this.m_pBtIf, WidcommUtils.FromBluetoothAddress(address));
    }

    public WBtRc GetExtendedError()
    {
      return (WBtRc) WidcommBtIf.NativeMethods.BtIf_GetExtendedError(this.m_pBtIf);
    }

    public SDK_RETURN_CODE IsRemoteDevicePresent(byte[] bd_addr)
    {
      return (SDK_RETURN_CODE) WidcommBtIf.NativeMethods.BtIf_IsRemoteDevicePresent(this.m_pBtIf, bd_addr);
    }

    public bool IsRemoteDeviceConnected(byte[] bd_addr)
    {
      return WidcommBtIf.NativeMethods.BtIf_IsRemoteDeviceConnected(this.m_pBtIf, bd_addr);
    }

    private static class NativeMethods
    {
      private const string KernelCoreLibrary = "kernel32.dll";

      [DllImport("32feetWidcomm")]
      internal static extern void BtIf_Create(
        out IntPtr ppBtIf,
        WidcommBtIf.NativeMethods.OnDeviceResponded deviceResponded,
        WidcommBtIf.NativeMethods.OnInquiryComplete handleInquiryCompleted,
        WidcommBtIf.NativeMethods.OnDiscoveryComplete handleDiscoveryComplete);

      [DllImport("32feetWidcomm")]
      internal static extern WidcommBtIf.NativeMethods.MY32FEET_AutoReconnectErrors BtIf_SetAutoReconnect(
        IntPtr pObj,
        [MarshalAs(UnmanagedType.Bool)] bool autoReconnect);

      [DllImport("32feetWidcomm")]
      internal static extern int BtIf_SetCallback2(
        IntPtr pBtIf,
        int num,
        WidcommBtIf.NativeMethods.OnStackStatusChange stackStatusChange);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_StartInquiry(IntPtr pBtIf);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern void BtIf_StopInquiry(IntPtr pBtIf);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_StartDiscovery(
        IntPtr pObj,
        byte[] p_bda,
        ref Guid p_service_guid,
        out int sizeofSdpDiscoveryRec);

      [DllImport("32feetWidcomm")]
      internal static extern DISCOVERY_RESULT BtIf_GetLastDiscoveryResult(
        IntPtr pObj,
        [Out] byte[] p_bda,
        out ushort p_num_recs);

      [DllImport("32feetWidcomm")]
      internal static extern int BtIf_ReadDiscoveryRecords(
        IntPtr pObj,
        byte[] p_bda,
        int max_size,
        out IntPtr pSdpDiscoveryRecArray);

      [DllImport("32feetWidcomm")]
      internal static extern int BtIf_ReadDiscoveryRecordsServiceClassOnly(
        IntPtr pObj,
        byte[] p_bda,
        int max_size,
        out IntPtr pSdpDiscoveryRecArray,
        ref Guid p_guid_filter);

      [DllImport("32feetWidcomm")]
      internal static extern REM_DEV_INFO_RETURN_CODE BtIf_GetRemoteDeviceInfo(
        IntPtr pObj,
        IntPtr p_rem_dev_info,
        int cb);

      [DllImport("32feetWidcomm")]
      internal static extern REM_DEV_INFO_RETURN_CODE BtIf_GetNextRemoteDeviceInfo(
        IntPtr pObj,
        IntPtr p_rem_dev_info,
        int cb);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_GetLocalDeviceVersionInfo(
        IntPtr pObj,
        ref DEV_VER_INFO pBuf,
        int cb);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_GetLocalDeviceInfoBdAddr(
        IntPtr m_pBtIf,
        [Out] byte[] bdAddr,
        int cb);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_GetLocalDeviceName(IntPtr pObj, [Out] byte[] pBdName, int cb);

      [DllImport("32feetWidcomm")]
      internal static extern void BtIf_IsStackUpAndRadioReady(
        IntPtr pObj,
        [MarshalAs(UnmanagedType.Bool)] out bool stackServerUp,
        [MarshalAs(UnmanagedType.Bool)] out bool deviceReady);

      [DllImport("32feetWidcomm")]
      internal static extern void BtIf_IsDeviceConnectableDiscoverable(
        IntPtr pObj,
        [MarshalAs(UnmanagedType.Bool)] out bool conno,
        [MarshalAs(UnmanagedType.Bool)] out bool disco);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_GetConnectionStats(
        IntPtr pObj,
        byte[] bdAddr,
        out tBT_CONN_STATS pStats,
        int cb);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_BondQuery(IntPtr pObj, byte[] bdAddr);

      [DllImport("32feetWidcomm")]
      internal static extern int BtIf_Bond(IntPtr pObj, byte[] bdAddr, byte[] pin_code);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_UnBond(IntPtr pObj, byte[] bdAddr);

      [DllImport("32feetWidcomm")]
      internal static extern uint BtIf_GetExtendedError(IntPtr pBtIf);

      [DllImport("32feetWidcomm")]
      internal static extern int BtIf_IsRemoteDevicePresent(IntPtr pObj, byte[] bdAddr);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BtIf_IsRemoteDeviceConnected(IntPtr pObj, byte[] bdAddr);

      [DllImport("32feetWidcomm")]
      internal static extern void BtIf_IsRemoteDevicePresentConnected(
        IntPtr pObj,
        byte[] bdAddr,
        out int pPresent,
        [MarshalAs(UnmanagedType.Bool)] out bool pConnected);

      [DllImport("32feetWidcomm")]
      internal static extern void BtIf_Destroy(IntPtr pBtIf);

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      internal static extern IntPtr LoadLibraryEx(
        string fileName,
        IntPtr hFile,
        WidcommBtIf.NativeMethods.LoadLibraryExFlags dwFlags);

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FreeLibrary(IntPtr hModule);

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
      internal static extern IntPtr GetModuleHandle(string moduleName);

      internal delegate void OnDeviceResponded(
        IntPtr bdAddr,
        IntPtr devClass,
        IntPtr deviceName,
        bool connected);

      internal delegate void OnInquiryComplete(bool success, ushort numResponses);

      internal delegate void OnDiscoveryComplete();

      internal delegate void OnStackStatusChange(int new_status);

      internal enum MY32FEET_AutoReconnectErrors
      {
        SUCCESS,
        NO_FUNCTION,
        CALL_FAILED,
      }

      [Flags]
      internal enum LoadLibraryExFlags
      {
        DONT_RESOLVE_DLL_REFERENCES = 1,
        LOAD_LIBRARY_AS_DATAFILE = 2,
        LOAD_WITH_ALTERED_SEARCH_PATH = 8,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 16, // 0x00000010
        VISTA_AND_LATER__LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 64, // 0x00000040
      }
    }

    private enum LibraryStatus
    {
      ModuleLoaded,
      LoadLibraryAccessible,
      AggregateOk,
      NotFound,
      Exception,
    }
  }
}
