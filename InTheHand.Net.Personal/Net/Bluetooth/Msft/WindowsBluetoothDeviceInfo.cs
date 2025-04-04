// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.WindowsBluetoothDeviceInfo
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Runtime.InteropServices;
using InTheHand.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal class WindowsBluetoothDeviceInfo : IBluetoothDeviceInfo
  {
    private BLUETOOTH_DEVICE_INFO deviceInfo;
    private bool valid;
    private RadioVersions _remoteVersionsInfo;

    public WindowsBluetoothDeviceInfo(IntPtr pDevice)
    {
      this.deviceInfo = new BLUETOOTH_DEVICE_INFO(0L);
      Marshal.PtrToStructure(pDevice, (object) this.deviceInfo);
      this.valid = true;
    }

    internal WindowsBluetoothDeviceInfo(BLUETOOTH_DEVICE_INFO device)
    {
      this.deviceInfo = device;
      this.valid = true;
    }

    public WindowsBluetoothDeviceInfo(BluetoothAddress address)
    {
      this.deviceInfo = !(address == (BluetoothAddress) null) ? new BLUETOOTH_DEVICE_INFO(address.ToInt64()) : throw new ArgumentNullException(nameof (address));
      this.GetDeviceInfo();
      this.valid = true;
    }

    private void GetDeviceInfo()
    {
      if (this.valid)
        return;
      int deviceInfo = NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref this.deviceInfo);
      if (deviceInfo != 0)
        Trace.WriteLine("BluetoothGetDeviceInfo returned: 0x" + deviceInfo.ToString("X"));
      this.valid = true;
    }

    public void Refresh()
    {
      this.valid = false;
      this.deviceInfo.ulClassofDevice = 0U;
      this.deviceInfo.szName = "";
      this._remoteVersionsInfo = (RadioVersions) null;
    }

    public void Update() => NativeMethods.BluetoothUpdateDeviceRecord(ref this.deviceInfo);

    public BluetoothAddress DeviceAddress
    {
      [DebuggerStepThrough] get => new BluetoothAddress(this.deviceInfo.Address);
    }

    public string DeviceName
    {
      get
      {
        this.GetDeviceInfo();
        return string.IsNullOrEmpty(this.deviceInfo.szName) ? this.DeviceAddress.ToString("C") : this.deviceInfo.szName;
      }
      set => this.deviceInfo.szName = value;
    }

    public bool HasDeviceName => !string.IsNullOrEmpty(this.deviceInfo.szName);

    public ClassOfDevice ClassOfDevice
    {
      get
      {
        this.GetDeviceInfo();
        return new ClassOfDevice(this.deviceInfo.ulClassofDevice);
      }
    }

    public int Rssi => int.MinValue;

    public Guid[] InstalledServices
    {
      get
      {
        this.GetDeviceInfo();
        int pcServices = 0;
        NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref this.deviceInfo, ref pcServices, (byte[]) null);
        byte[] numArray1 = new byte[pcServices * 16];
        if (NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref this.deviceInfo, ref pcServices, numArray1) < 0)
          return new Guid[0];
        Guid[] installedServices = new Guid[pcServices];
        for (int index = 0; index < pcServices; ++index)
        {
          byte[] numArray2 = new byte[16];
          Buffer.BlockCopy((Array) numArray1, index * 16, (Array) numArray2, 0, 16);
          installedServices[index] = new Guid(numArray2);
        }
        return installedServices;
      }
    }

    public void SetServiceState(Guid service, bool state)
    {
      this.SetServiceState(service, state, false);
    }

    public void SetServiceState(Guid service, bool state, bool throwOnError)
    {
      this.GetDeviceInfo();
      int error = NativeMethods.BluetoothSetServiceState(IntPtr.Zero, ref this.deviceInfo, ref service, state ? 1 : 0);
      if (error == 0)
        return;
      Win32Exception win32Exception = new Win32Exception(error);
      if (throwOnError)
        throw win32Exception;
    }

    IAsyncResult IBluetoothDeviceInfo.BeginGetServiceRecords(
      Guid service,
      AsyncCallback callback,
      object state)
    {
      AsyncResult<ServiceRecord[], Guid> state1 = new AsyncResult<ServiceRecord[], Guid>(callback, state, service);
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.BeginGetServiceRecords_Runner), (object) state1);
      return (IAsyncResult) state1;
    }

    private void BeginGetServiceRecords_Runner(object state)
    {
      AsyncResult<ServiceRecord[], Guid> ar = (AsyncResult<ServiceRecord[], Guid>) state;
      ar.SetAsCompletedWithResultOf((Func<ServiceRecord[]>) (() => this.GetServiceRecords(ar.BeginParameters)), false);
    }

    ServiceRecord[] IBluetoothDeviceInfo.EndGetServiceRecords(IAsyncResult asyncResult)
    {
      return ((AsyncResult<ServiceRecord[]>) asyncResult).EndInvoke();
    }

    public ServiceRecord[] GetServiceRecords(Guid service)
    {
      byte[][] serviceRecordsUnparsed = this.GetServiceRecordsUnparsed(service);
      ServiceRecord[] serviceRecords = new ServiceRecord[serviceRecordsUnparsed.Length];
      ServiceRecordParser serviceRecordParser = new ServiceRecordParser();
      int index = 0;
      foreach (byte[] numArray in serviceRecordsUnparsed)
      {
        ServiceRecord serviceRecord = serviceRecordParser.Parse(numArray);
        serviceRecord.SetSourceBytes(numArray);
        serviceRecords[index] = serviceRecord;
        ++index;
      }
      return serviceRecords;
    }

    public byte[][] GetServiceRecordsUnparsed(Guid service)
    {
      return this.GetServiceRecordsUnparsedWindowsRaw(service);
    }

    public byte[][] GetServiceRecordsUnparsedWindowsRaw(Guid service)
    {
      Socket socket = new Socket((AddressFamily) 32, SocketType.Stream, ProtocolType.Ggp);
      ArrayList arrayList = new ArrayList();
      WSAQUERYSET pQuerySet = new WSAQUERYSET();
      pQuerySet.dwSize = WqsOffset.StructLength_60;
      pQuerySet.dwNameSpace = 16;
      GCHandle gcHandle = GCHandle.Alloc((object) service.ToByteArray(), GCHandleType.Pinned);
      pQuerySet.lpServiceClassId = gcHandle.AddrOfPinnedObject();
      pQuerySet.lpszContext = "(" + this.DeviceAddress.ToString("C") + ")";
      IntPtr lphLookup = IntPtr.Zero;
      int errorCode = NativeMethods.WSALookupServiceBegin(ref pQuerySet, LookupFlags.ReturnName | LookupFlags.ReturnBlob | LookupFlags.FlushCache, out lphLookup);
      SocketBluetoothClient.ThrowSocketExceptionForHR(errorCode);
      gcHandle.Free();
      while (errorCode == 0)
      {
        byte[] numArray = new byte[6000];
        BitConverter.GetBytes(WqsOffset.StructLength_60).CopyTo((Array) numArray, WqsOffset.dwSize_0);
        BitConverter.GetBytes(16).CopyTo((Array) numArray, WqsOffset.dwNameSpace_20);
        int length1 = numArray.Length;
        errorCode = NativeMethods.WSALookupServiceNext(lphLookup, LookupFlags.ReturnBlob | LookupFlags.FlushCache, ref length1, numArray);
        if (errorCode == -1)
        {
          SocketBluetoothClient.ThrowSocketExceptionForHrExceptFor(errorCode, 10110);
        }
        else
        {
          IntPtr ptr = Marshal32.ReadIntPtr(numArray, WqsOffset.lpBlob_56);
          if (ptr != IntPtr.Zero)
          {
            IntPtr source = Marshal32.ReadIntPtr(ptr, BlobOffsets.Offset_pBlobData_4);
            int length2 = Marshal.ReadInt32(ptr);
            if (length2 > 2)
            {
              byte[] destination = new byte[length2];
              Marshal.Copy(source, destination, 0, length2);
              arrayList.Add((object) destination);
            }
          }
        }
      }
      SocketBluetoothClient.ThrowSocketExceptionForHR(NativeMethods.WSALookupServiceEnd(lphLookup));
      return (byte[][]) arrayList.ToArray(typeof (byte[]));
    }

    public bool Connected
    {
      get
      {
        this.GetDeviceInfo();
        return this.deviceInfo.fConnected;
      }
    }

    public bool Remembered
    {
      get
      {
        this.GetDeviceInfo();
        return this.deviceInfo.fRemembered;
      }
    }

    public bool Authenticated
    {
      get
      {
        this.GetDeviceInfo();
        return this.deviceInfo.fAuthenticated;
      }
    }

    public void Merge(IBluetoothDeviceInfo other)
    {
    }

    public DateTime LastSeen
    {
      get
      {
        this.GetDeviceInfo();
        return this.deviceInfo.LastSeen;
      }
    }

    public void SetDiscoveryTime(DateTime dt)
    {
      if (this.LastSeen != DateTime.MinValue)
        throw new InvalidOperationException("LastSeen is already set.");
      this.deviceInfo.stLastSeen = SYSTEMTIME.FromDateTime(dt);
    }

    public DateTime LastUsed
    {
      get
      {
        this.GetDeviceInfo();
        return this.deviceInfo.LastUsed;
      }
    }

    public RadioVersions GetVersions()
    {
      this.ReadVersionsInfo();
      return this._remoteVersionsInfo;
    }

    private void ReadVersionsInfo()
    {
      if (this._remoteVersionsInfo != null)
        return;
      IntPtr handle = BluetoothRadio.PrimaryRadio.Handle;
      long int64 = this.DeviceAddress.ToInt64();
      BTH_RADIO_INFO OutBuffer = new BTH_RADIO_INFO();
      int nOutBufferSize = Marshal.SizeOf((object) OutBuffer);
      if (!NativeMethods.DeviceIoControl(handle, 4259844U, ref int64, Marshal.SizeOf((object) int64), ref OutBuffer, nOutBufferSize, out int _, IntPtr.Zero))
      {
        Marshal.GetLastWin32Error();
        throw new Win32Exception();
      }
      this._remoteVersionsInfo = OutBuffer.ConvertToRadioVersions();
    }

    public void ShowDialog()
    {
      this.GetDeviceInfo();
      NativeMethods.BluetoothDisplayDeviceProperties(IntPtr.Zero, ref this.deviceInfo);
    }

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
  }
}
