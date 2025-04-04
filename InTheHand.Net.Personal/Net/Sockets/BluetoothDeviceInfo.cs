// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.BluetoothDeviceInfo
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class BluetoothDeviceInfo : IComparable
  {
    private readonly IBluetoothDeviceInfo m_impl;

    internal BluetoothDeviceInfo(IBluetoothDeviceInfo impl) => this.m_impl = impl;

    public BluetoothDeviceInfo(BluetoothAddress address)
      : this(BluetoothFactory.Factory.DoGetBluetoothDeviceInfo(address))
    {
    }

    internal void Merge(IBluetoothDeviceInfo other) => this.m_impl.Merge(other);

    public void Refresh() => this.m_impl.Refresh();

    public void Update() => this.m_impl.Update();

    public BluetoothAddress DeviceAddress
    {
      [DebuggerStepThrough] get => this.m_impl.DeviceAddress;
    }

    public string DeviceName
    {
      [DebuggerStepThrough] get => this.m_impl.DeviceName;
      set => this.m_impl.DeviceName = value;
    }

    public ClassOfDevice ClassOfDevice => this.m_impl.ClassOfDevice;

    public int Rssi => this.m_impl.Rssi;

    public Guid[] InstalledServices => this.m_impl.InstalledServices;

    public void SetServiceState(Guid service, bool state)
    {
      this.m_impl.SetServiceState(service, state);
    }

    public void SetServiceState(Guid service, bool state, bool throwOnError)
    {
      this.m_impl.SetServiceState(service, state, throwOnError);
    }

    public ServiceRecord[] GetServiceRecords(Guid service)
    {
      return this.m_impl.GetServiceRecords(service);
    }

    public IAsyncResult BeginGetServiceRecords(Guid service, AsyncCallback callback, object state)
    {
      return this.m_impl.BeginGetServiceRecords(service, callback, state);
    }

    public ServiceRecord[] EndGetServiceRecords(IAsyncResult asyncResult)
    {
      return this.m_impl.EndGetServiceRecords(asyncResult);
    }

    public byte[][] GetServiceRecordsUnparsed(Guid service)
    {
      return this.m_impl.GetServiceRecordsUnparsed(service);
    }

    public RadioVersions GetVersions() => this.m_impl.GetVersions();

    public bool Connected => this.m_impl.Connected;

    public bool Remembered => this.m_impl.Remembered;

    public bool Authenticated => this.m_impl.Authenticated;

    public DateTime LastSeen
    {
      get
      {
        this.AssertUtc(this.m_impl.LastSeen);
        return this.m_impl.LastSeen;
      }
    }

    public DateTime LastUsed
    {
      get
      {
        this.AssertUtc(this.m_impl.LastUsed);
        return this.m_impl.LastUsed;
      }
    }

    private void AssertUtc(DateTime dateTime)
    {
      int num = dateTime == DateTime.MinValue ? 1 : 0;
    }

    public void ShowDialog() => this.m_impl.ShowDialog();

    public override bool Equals(object obj)
    {
      if (obj is BluetoothDeviceInfo bluetoothDeviceInfo)
        return this.DeviceAddress.Equals((object) bluetoothDeviceInfo.DeviceAddress);
      BluetoothDeviceInfo.IsAMsftInternalType(obj);
      return base.Equals(obj);
    }

    internal static bool IsAMsftInternalType(object obj)
    {
      return obj != null && obj.GetType().FullName.Equals("MS.Internal.NamedObject", StringComparison.Ordinal);
    }

    internal static bool EqualsIBDI(IBluetoothDeviceInfo x, IBluetoothDeviceInfo y)
    {
      return x.DeviceAddress.Equals((object) y.DeviceAddress);
    }

    internal static bool EqualsIBDI(IBluetoothDeviceInfo x, object obj)
    {
      return obj is IBluetoothDeviceInfo y ? BluetoothDeviceInfo.EqualsIBDI(x, y) : object.Equals((object) x, obj);
    }

    internal static int ListIndexOf(List<IBluetoothDeviceInfo> list, IBluetoothDeviceInfo item)
    {
      return list.FindIndex((Predicate<IBluetoothDeviceInfo>) (cur => item.DeviceAddress.Equals((object) cur.DeviceAddress)));
    }

    internal static bool AddUniqueDevice(List<IBluetoothDeviceInfo> list, IBluetoothDeviceInfo bdi)
    {
      int index = BluetoothDeviceInfo.ListIndexOf(list, bdi);
      if (index == -1)
      {
        list.Add(bdi);
        return true;
      }
      IBluetoothDeviceInfo bluetoothDeviceInfo = list[index];
      list[index] = bdi;
      return false;
    }

    [Conditional("DEBUG")]
    private static void AssertManualExistsIf(
      int index,
      List<IBluetoothDeviceInfo> list,
      IBluetoothDeviceInfo item)
    {
      foreach (IBluetoothDeviceInfo bluetoothDeviceInfo in list)
      {
        int num = bluetoothDeviceInfo.DeviceAddress == item.DeviceAddress ? 1 : 0;
      }
    }

    internal static IBluetoothDeviceInfo[] Intersect(
      IBluetoothDeviceInfo[] list,
      List<IBluetoothDeviceInfo> seenDevices)
    {
      List<IBluetoothDeviceInfo> bluetoothDeviceInfoList = new List<IBluetoothDeviceInfo>(list.Length);
      foreach (IBluetoothDeviceInfo bluetoothDeviceInfo in list)
      {
        if (-1 != BluetoothDeviceInfo.ListIndexOf(seenDevices, bluetoothDeviceInfo))
          bluetoothDeviceInfoList.Add(bluetoothDeviceInfo);
      }
      return bluetoothDeviceInfoList.ToArray();
    }

    public override int GetHashCode() => this.DeviceAddress.GetHashCode();

    int IComparable.CompareTo(object obj)
    {
      return obj is BluetoothDeviceInfo bluetoothDeviceInfo ? ((IComparable) this.DeviceAddress).CompareTo((object) bluetoothDeviceInfo) : -1;
    }

    internal static BluetoothDeviceInfo[] Wrap(IBluetoothDeviceInfo[] orig)
    {
      BluetoothDeviceInfo[] bluetoothDeviceInfoArray = new BluetoothDeviceInfo[orig.Length];
      for (int index = 0; index < orig.Length; ++index)
        bluetoothDeviceInfoArray[index] = new BluetoothDeviceInfo(orig[index]);
      return bluetoothDeviceInfoArray;
    }
  }
}
