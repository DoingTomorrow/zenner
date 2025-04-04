// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothDeviceInfo
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBluetoothDeviceInfo : IBluetoothDeviceInfo
  {
    private readonly WidcommBluetoothFactoryBase m_factory;
    private REM_DEV_INFO m_remDevInfo;
    private bool m_remembered;
    private string m_cachedName;
    private DateTime m_lastSeen;

    private WidcommBluetoothDeviceInfo(REM_DEV_INFO remDevInfo, WidcommBluetoothFactoryBase factory)
    {
      this.m_factory = factory;
      this.m_remDevInfo = remDevInfo;
    }

    internal static WidcommBluetoothDeviceInfo CreateFromGivenAddressNoLookup(
      BluetoothAddress address,
      WidcommBluetoothFactoryBase factory)
    {
      return new WidcommBluetoothDeviceInfo(new REM_DEV_INFO()
      {
        bda = WidcommUtils.FromBluetoothAddress(address)
      }, factory);
    }

    internal static WidcommBluetoothDeviceInfo CreateFromGivenAddress(
      BluetoothAddress address,
      WidcommBluetoothFactoryBase factory)
    {
      new REM_DEV_INFO().bda = WidcommUtils.FromBluetoothAddress(address);
      return factory.GetWidcommBtInterface().ReadDeviceFromRegistryAndCheckAndSetIfPaired(address, factory) ?? WidcommBluetoothDeviceInfo.CreateFromGivenAddressNoLookup(address, factory);
    }

    internal static WidcommBluetoothDeviceInfo CreateFromStoredRemoteDeviceInfo(
      REM_DEV_INFO rdi,
      WidcommBluetoothFactoryBase factory)
    {
      return new WidcommBluetoothDeviceInfo(rdi, factory)
      {
        m_remembered = true
      };
    }

    internal static WidcommBluetoothDeviceInfo CreateFromHandleDeviceResponded(
      byte[] bdAddr,
      byte[] deviceName,
      byte[] devClass,
      bool connected,
      WidcommBluetoothFactoryBase factory)
    {
      return new WidcommBluetoothDeviceInfo(new REM_DEV_INFO()
      {
        bda = bdAddr,
        bd_name = deviceName,
        b_connected = connected,
        dev_class = devClass
      }, factory);
    }

    internal static void CheckAndSetIfPaired(
      WidcommBluetoothDeviceInfo bdi,
      WidcommBluetoothFactoryBase factory)
    {
      if (bdi.m_remDevInfo.b_paired)
        return;
      bool flag = factory.GetWidcommBtInterface().BondQuery(bdi.m_remDevInfo.bda);
      bdi.m_remDevInfo.b_paired = flag;
    }

    public void Merge(IBluetoothDeviceInfo other)
    {
      this.m_remembered = other.Remembered;
      this.m_remDevInfo.b_paired = other.Authenticated;
    }

    public void SetDiscoveryTime(DateTime dt)
    {
      this.m_lastSeen = !(this.m_lastSeen != DateTime.MinValue) ? dt : throw new InvalidOperationException("LastSeen is already set.");
    }

    public BluetoothAddress DeviceAddress => WidcommUtils.ToBluetoothAddress(this.m_remDevInfo.bda);

    public string DeviceName
    {
      get
      {
        if (this.m_cachedName == null)
        {
          if (this.m_remDevInfo.bd_name != null)
            this.m_cachedName = WidcommUtils.BdNameToString(this.m_remDevInfo.bd_name);
          if (this.m_cachedName == null)
            this.m_cachedName = this.DeviceAddress.ToString("C", (IFormatProvider) CultureInfo.InvariantCulture);
        }
        return this.m_cachedName;
      }
      set
      {
        this.m_cachedName = value;
        this.m_remDevInfo.bd_name = Encoding.UTF8.GetBytes(this.m_cachedName + "\0");
      }
    }

    public bool Remembered => this.m_remembered;

    public bool Authenticated => this.m_remDevInfo.b_paired;

    public ClassOfDevice ClassOfDevice
    {
      get
      {
        if (this.m_remDevInfo.dev_class != null)
          return WidcommUtils.ToClassOfDevice(this.m_remDevInfo.dev_class);
        MiscUtils.Trace_WriteLine("NOT m_remDevInfo.dev_class");
        return new ClassOfDevice(0U);
      }
    }

    public bool Connected => this.m_remDevInfo.b_connected;

    public DateTime LastSeen => this.m_lastSeen;

    public DateTime LastUsed => DateTime.MinValue;

    public int Rssi => this.m_factory.GetWidcommBtInterface().GetRssi(this.m_remDevInfo.bda);

    public void Refresh() => this.m_cachedName = (string) null;

    public ServiceRecord[] GetServiceRecords(Guid service)
    {
      return this.EndGetServiceRecords(this.BeginGetServiceRecords(service, (AsyncCallback) null, (object) null));
    }

    public IAsyncResult BeginGetServiceRecords(Guid service, AsyncCallback callback, object state)
    {
      return this.BeginGetServiceRecordsUnparsedWidcomm(service, callback, state);
    }

    public ServiceRecord[] EndGetServiceRecords(IAsyncResult asyncResult)
    {
      using (ISdpDiscoveryRecordsBuffer recordsUnparsedWidcomm = this.EndGetServiceRecordsUnparsedWidcomm(asyncResult))
        return recordsUnparsedWidcomm.GetServiceRecords();
    }

    private IAsyncResult BeginGetServiceRecordsUnparsedWidcomm(
      Guid service,
      AsyncCallback callback,
      object state)
    {
      return this.m_factory.GetWidcommBtInterface().BeginServiceDiscovery(this.DeviceAddress, service, SdpSearchScope.Anywhere, callback, state);
    }

    private ISdpDiscoveryRecordsBuffer EndGetServiceRecordsUnparsedWidcomm(IAsyncResult ar)
    {
      return this.m_factory.GetWidcommBtInterface().EndServiceDiscovery(ar);
    }

    public byte[][] GetServiceRecordsUnparsed(Guid service)
    {
      throw new NotSupportedException("Can't get the raw record from the Widcomm stack.");
    }

    public Guid[] InstalledServices
    {
      get => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public void SetServiceState(Guid service, bool state, bool throwOnError)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    public void SetServiceState(Guid service, bool state)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    RadioVersions IBluetoothDeviceInfo.GetVersions()
    {
      throw new NotImplementedException("GetVersions not currently supported on this stack.");
    }

    public void ShowDialog()
    {
      int num = (int) MessageBox.Show("Name: " + this.DeviceName + "\r\nAddress: " + this.DeviceAddress.ToString("C", (IFormatProvider) CultureInfo.InvariantCulture), this.DeviceName + " Properties", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions) 0);
    }

    public void Update()
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
  }
}
