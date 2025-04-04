// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.BluetoothClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Msft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  [DebuggerDisplay("impl={m_impl}")]
  public class BluetoothClient : IDisposable
  {
    private readonly IBluetoothClient m_impl;

    internal BluetoothClient(IBluetoothClient impl) => this.m_impl = impl;

    public BluetoothClient()
      : this(BluetoothFactory.Factory)
    {
    }

    internal BluetoothClient(BluetoothFactory factory)
      : this(factory.DoGetBluetoothClient())
    {
    }

    public BluetoothClient(BluetoothEndPoint localEP)
      : this(BluetoothFactory.Factory, localEP)
    {
    }

    internal BluetoothClient(BluetoothFactory factory, BluetoothEndPoint localEP)
      : this(factory.DoGetBluetoothClient(localEP))
    {
    }

    public int InquiryAccessCode
    {
      [DebuggerStepThrough] get => this.m_impl.InquiryAccessCode;
      [DebuggerStepThrough] set => this.m_impl.InquiryAccessCode = value;
    }

    public TimeSpan InquiryLength
    {
      [DebuggerStepThrough] get => this.m_impl.InquiryLength;
      [DebuggerStepThrough] set => this.m_impl.InquiryLength = value;
    }

    public BluetoothDeviceInfo[] DiscoverDevices()
    {
      return this.DiscoverDevices((int) byte.MaxValue, true, true, true);
    }

    public BluetoothDeviceInfo[] DiscoverDevices(int maxDevices)
    {
      return this.DiscoverDevices(maxDevices, true, true, true);
    }

    public BluetoothDeviceInfo[] DiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown)
    {
      return BluetoothDeviceInfo.Wrap(this.m_impl.DiscoverDevices(maxDevices, authenticated, remembered, unknown, false));
    }

    [DebuggerStepThrough]
    public BluetoothDeviceInfo[] DiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly)
    {
      return BluetoothDeviceInfo.Wrap(this.m_impl.DiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly));
    }

    public BluetoothDeviceInfo[] DiscoverDevicesInRange()
    {
      return this.DiscoverDevices((int) byte.MaxValue, false, false, false, true);
    }

    [DebuggerStepThrough]
    public IAsyncResult BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state)
    {
      return this.m_impl.BeginDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, callback, state);
    }

    public BluetoothDeviceInfo[] EndDiscoverDevices(IAsyncResult asyncResult)
    {
      return BluetoothDeviceInfo.Wrap(this.m_impl.EndDiscoverDevices(asyncResult));
    }

    internal IAsyncResult BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback handler,
      object liveDiscoState)
    {
      return this.m_impl.BeginDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, callback, state, handler, liveDiscoState);
    }

    public int Available
    {
      [DebuggerStepThrough] get => this.m_impl.Available;
    }

    public Socket Client
    {
      [DebuggerStepThrough] get => this.m_impl.Client;
      [DebuggerStepThrough] set => this.m_impl.Client = value;
    }

    public void Connect(BluetoothEndPoint remoteEP)
    {
      if (remoteEP == null)
        throw new ArgumentNullException(nameof (remoteEP));
      this.m_impl.Connect(remoteEP);
    }

    public void Connect(BluetoothAddress address, Guid service)
    {
      if (address == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (address));
      if (service == Guid.Empty)
        throw new ArgumentNullException(nameof (service));
      this.Connect(new BluetoothEndPoint(address, service));
    }

    public IAsyncResult BeginConnect(
      BluetoothAddress address,
      Guid service,
      AsyncCallback requestCallback,
      object state)
    {
      if (address == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (address));
      if (service == Guid.Empty)
        throw new ArgumentNullException(nameof (service));
      return this.BeginConnect(new BluetoothEndPoint(address, service), requestCallback, state);
    }

    public IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      if (remoteEP == null)
        throw new ArgumentNullException(nameof (remoteEP));
      return this.m_impl.BeginConnect(remoteEP, requestCallback, state);
    }

    [DebuggerStepThrough]
    public void EndConnect(IAsyncResult asyncResult) => this.m_impl.EndConnect(asyncResult);

    public bool Connected
    {
      [DebuggerStepThrough] get => this.m_impl.Connected;
    }

    [DebuggerStepThrough]
    public void Close() => this.Dispose();

    [DebuggerStepThrough]
    public NetworkStream GetStream() => this.m_impl.GetStream();

    public LingerOption LingerState
    {
      [DebuggerStepThrough] get => this.m_impl.LingerState;
      [DebuggerStepThrough] set => this.m_impl.LingerState = value;
    }

    public bool Authenticate
    {
      get => this.m_impl.Authenticate;
      set => this.m_impl.Authenticate = value;
    }

    public bool Encrypt
    {
      [DebuggerStepThrough] get => this.m_impl.Encrypt;
      [DebuggerStepThrough] set => this.m_impl.Encrypt = value;
    }

    public Guid LinkKey
    {
      [DebuggerStepThrough] get => this.m_impl.LinkKey;
    }

    public LinkPolicy LinkPolicy => this.m_impl.LinkPolicy;

    [DebuggerStepThrough]
    public void SetPin(string pin) => this.m_impl.SetPin(pin);

    [DebuggerStepThrough]
    public void SetPin(BluetoothAddress device, string pin) => this.m_impl.SetPin(device, pin);

    public BluetoothEndPoint RemoteEndPoint
    {
      [DebuggerStepThrough] get => this.m_impl.RemoteEndPoint;
    }

    public string RemoteMachineName
    {
      [DebuggerStepThrough] get => this.m_impl.RemoteMachineName;
    }

    [DebuggerStepThrough]
    public string GetRemoteMachineName(BluetoothAddress a) => this.m_impl.GetRemoteMachineName(a);

    public static string GetRemoteMachineName(Socket s)
    {
      return SocketBluetoothClient.GetRemoteMachineName(s);
    }

    [DebuggerStepThrough]
    public void Dispose() => this.m_impl.Dispose();

    [Obsolete("Use the four Boolean method.")]
    internal static List<IBluetoothDeviceInfo> DiscoverDevicesMerge(
      bool authenticated,
      bool remembered,
      bool unknown,
      List<IBluetoothDeviceInfo> knownDevices,
      List<IBluetoothDeviceInfo> discoverableDevices,
      DateTime discoTime)
    {
      return BluetoothClient.DiscoverDevicesMerge(authenticated, remembered, unknown, knownDevices, discoverableDevices, false, discoTime);
    }

    internal static List<IBluetoothDeviceInfo> DiscoverDevicesMerge(
      bool authenticated,
      bool remembered,
      bool unknown,
      List<IBluetoothDeviceInfo> knownDevices,
      List<IBluetoothDeviceInfo> discoverableDevices,
      bool discoverableOnly,
      DateTime discoTime)
    {
      if (unknown || discoverableOnly)
      {
        if (discoverableDevices == null)
          throw new ArgumentNullException(nameof (discoverableDevices));
      }
      else
        TestUtilities.IsUnderTestHarness();
      if (knownDevices == null)
        throw new ArgumentNullException(nameof (knownDevices));
      bool flag1;
      if (discoverableOnly)
      {
        flag1 = false;
      }
      else
      {
        flag1 = authenticated || remembered;
        if (!flag1 && !unknown)
          return new List<IBluetoothDeviceInfo>();
      }
      List<IBluetoothDeviceInfo> list = unknown || discoverableOnly ? new List<IBluetoothDeviceInfo>((IEnumerable<IBluetoothDeviceInfo>) discoverableDevices) : new List<IBluetoothDeviceInfo>();
      foreach (IBluetoothDeviceInfo knownDevice in knownDevices)
      {
        foreach (IBluetoothDeviceInfo x in list)
        {
          if (BluetoothDeviceInfo.EqualsIBDI(x, knownDevice))
            break;
        }
        int index = BluetoothDeviceInfo.ListIndexOf(list, knownDevice);
        if (index != -1)
        {
          if (flag1 || discoverableOnly)
            list[index].Merge(knownDevice);
          else
            list.RemoveAt(index);
        }
        else
        {
          bool flag2 = remembered && knownDevice.Remembered || authenticated && knownDevice.Authenticated;
          if (flag1 && flag2)
            list.Add(knownDevice);
        }
      }
      return list;
    }

    [Conditional("DEBUG")]
    private static void AssertNoDuplicates(List<IBluetoothDeviceInfo> deviceList, string location)
    {
      if (deviceList == null)
        return;
      for (int index1 = 0; index1 < deviceList.Count; ++index1)
      {
        IBluetoothDeviceInfo device = deviceList[index1];
        BluetoothAddress deviceAddress = device.DeviceAddress;
        for (int index2 = index1 + 1; index2 < deviceList.Count; ++index2)
        {
          if (deviceList[index2].DeviceAddress == deviceAddress)
            string.Format((IFormatProvider) CultureInfo.InvariantCulture, "'{0}' duplicate #{1}==#{2}: '{3}' '{4}'", (object) location, (object) index1, (object) index2, (object) device, (object) deviceList[index2]);
        }
      }
    }

    public delegate void LiveDiscoveryCallback(IBluetoothDeviceInfo p1, object p2);
  }
}
