// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothWin32Events
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Msft;
using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class BluetoothWin32Events : IDisposable
  {
    private static readonly int _OffsetOfData = Marshal.OffsetOf(typeof (BluetoothWin32Events.DEV_BROADCAST_HANDLE__withData), "dbch_data__0").ToInt32();
    private static WeakReference _instance;
    private BluetoothWin32Events.BtEventsForm _form;
    private Thread _formThread;
    private ManualResetEvent _inited;
    private Exception _formThreadException;
    private readonly IntPtr _bluetoothRadioHandle;

    private static T Marshal_PtrToStructure<T>(IntPtr ptr) where T : struct
    {
      return (T) Marshal.PtrToStructure(ptr, typeof (T));
    }

    public BluetoothWin32Events()
    {
      BluetoothRadio bluetoothRadio = (BluetoothRadio) null;
      foreach (BluetoothRadio allRadio in BluetoothRadio.AllRadios)
      {
        if (allRadio.SoftwareManufacturer == Manufacturer.Microsoft)
        {
          bluetoothRadio = allRadio;
          break;
        }
      }
      IntPtr num = bluetoothRadio != null ? bluetoothRadio.Handle : throw new InvalidOperationException("There is no Radio using the Microsoft Bluetooth stack.");
      this._bluetoothRadioHandle = !(num == IntPtr.Zero) ? num : throw new ArgumentException("The bluetoothRadioHandle may not be NULL.");
      this.Start();
    }

    public BluetoothWin32Events(BluetoothRadio microsoftWin32BluetoothRadio)
    {
      if (microsoftWin32BluetoothRadio == null)
        throw new ArgumentNullException(nameof (microsoftWin32BluetoothRadio));
      IntPtr num = microsoftWin32BluetoothRadio.SoftwareManufacturer == Manufacturer.Microsoft ? microsoftWin32BluetoothRadio.Handle : throw new ArgumentException("The specified Radio does not use the Microsoft Bluetooth stack.");
      this._bluetoothRadioHandle = !(num == IntPtr.Zero) ? num : throw new ArgumentException("The bluetoothRadioHandle may not be NULL.");
      this.Start();
    }

    public static BluetoothWin32Events GetInstance()
    {
      WeakReference instance = BluetoothWin32Events._instance;
      object target;
      if (instance == null || (target = instance.Target) == null)
      {
        target = (object) new BluetoothWin32Events();
        BluetoothWin32Events._instance = new WeakReference(target);
      }
      else
        target.ToString();
      return (BluetoothWin32Events) target;
    }

    private void RaiseInRange(BluetoothWin32RadioInRangeEventArgs e) => this.OnInRange(e);

    protected void OnInRange(BluetoothWin32RadioInRangeEventArgs e)
    {
      this.InRange((object) this, e);
    }

    public event EventHandler<BluetoothWin32RadioInRangeEventArgs> InRange = delegate { };

    private void RaiseOutOfRange(BluetoothWin32RadioOutOfRangeEventArgs e) => this.OnOutOfRange(e);

    protected void OnOutOfRange(BluetoothWin32RadioOutOfRangeEventArgs e)
    {
      this.OutOfRange((object) this, e);
    }

    public event EventHandler<BluetoothWin32RadioOutOfRangeEventArgs> OutOfRange = delegate { };

    private void Start()
    {
      if (Application.MessageLoop)
        this.StartUseExistingAppLoop();
      else
        this.StartOwnAppLoop();
    }

    private void StartUseExistingAppLoop() => this.CreateForm();

    private void StartOwnAppLoop()
    {
      bool flag = false;
      try
      {
        using (this._inited = new ManualResetEvent(false))
        {
          this._formThread = new Thread(new ParameterizedThreadStart(this.MessageLoop_Runner));
          this._formThread.IsBackground = true;
          this._formThread.SetApartmentState(ApartmentState.STA);
          this._formThread.Start((object) this);
          this._inited.WaitOne();
        }
        this._inited = (ManualResetEvent) null;
        Thread.MemoryBarrier();
        this._formThreadException = this._formThreadException == null ? (Exception) null : throw this._formThreadException;
        flag = true;
      }
      finally
      {
        if (!flag)
        {
          BluetoothWin32Events.BtEventsForm form = this._form;
        }
      }
    }

    private void MessageLoop_Runner(object state)
    {
      try
      {
        int apartmentState = (int) this._formThread.GetApartmentState();
        this.CreateForm();
        this._inited.Set();
        Application.Run();
      }
      catch (Exception ex)
      {
        this._formThreadException = ex;
        Thread.MemoryBarrier();
      }
      finally
      {
        if (this._form != null)
          this._form.Dispose();
        if (this._inited != null)
          this._inited.Set();
      }
    }

    private void CreateForm()
    {
      this._form = new BluetoothWin32Events.BtEventsForm(this);
      this._form.btRegister(this._bluetoothRadioHandle);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._form == null)
        return;
      this._form.Invoke((Delegate) new MethodInvoker(this.DisposeForm));
      this._form = (BluetoothWin32Events.BtEventsForm) null;
      if (this._formThread == null)
        return;
      this._formThread.Join(60000);
      if (this._formThreadException != null)
        throw this._formThreadException;
      this._formThread = (Thread) null;
    }

    private void DisposeForm()
    {
      this._form.Dispose();
      if (this._formThread == null)
        return;
      Application.ExitThread();
    }

    private void HandleMessage(ref Message m)
    {
      if ((BluetoothWin32Events.WindowMessageId) m.Msg == BluetoothWin32Events.WindowMessageId.DeviceChange)
      {
        switch ((BluetoothWin32Events.Dbt) (int) m.WParam)
        {
          case BluetoothWin32Events.Dbt.DeviceArrival:
          case BluetoothWin32Events.Dbt.DeviceQueryRemove:
          case BluetoothWin32Events.Dbt.DeviceQueryRemoveFailed:
          case BluetoothWin32Events.Dbt.DeviceRemovePending:
          case BluetoothWin32Events.Dbt.DeviceRemoveComplete:
          case BluetoothWin32Events.Dbt.DeviceTypeSpecific:
          case BluetoothWin32Events.Dbt.CustomEvent:
            this.DoBroadcastHdr(m);
            break;
        }
      }
    }

    private void DoBroadcastHdr(Message m)
    {
      string empty = string.Empty;
      BluetoothWin32Events.DEV_BROADCAST_HDR lparam = (BluetoothWin32Events.DEV_BROADCAST_HDR) m.GetLParam(typeof (BluetoothWin32Events.DEV_BROADCAST_HDR));
      if (lparam.dbch_devicetype == BluetoothWin32Events.DbtDevTyp.Port)
      {
        BluetoothWin32Events.DoDevTypPort(ref m, ref empty, ref lparam);
      }
      else
      {
        if (lparam.dbch_devicetype != BluetoothWin32Events.DbtDevTyp.Handle)
          return;
        this.DoDevTypHandle(ref m, ref empty);
      }
    }

    private void DoDevTypHandle(ref Message m, ref string text)
    {
      BluetoothWin32Events.DEV_BROADCAST_HANDLE lparam = (BluetoothWin32Events.DEV_BROADCAST_HANDLE) m.GetLParam(typeof (BluetoothWin32Events.DEV_BROADCAST_HANDLE));
      IntPtr ptr = Pointers.Add(m.LParam, BluetoothWin32Events._OffsetOfData);
      if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.BthPortDeviceInterface == lparam.dbch_eventguid)
        text += "GUID_BTHPORT_DEVICE_INTERFACE";
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.RadioInRange == lparam.dbch_eventguid)
      {
        text += "GUID_BLUETOOTH_RADIO_IN_RANGE";
        BluetoothWin32Events.BTH_RADIO_IN_RANGE structure = (BluetoothWin32Events.BTH_RADIO_IN_RANGE) Marshal.PtrToStructure(ptr, typeof (BluetoothWin32Events.BTH_RADIO_IN_RANGE));
        text += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " 0x{0:X12}", (object) structure.deviceInfo.address);
        text += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " is ({0}) 0x{0:X}", (object) structure.deviceInfo.flags);
        text += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " was ({0}) 0x{0:X}", (object) structure.previousDeviceFlags);
        BLUETOOTH_DEVICE_INFO deviceInfo = BLUETOOTH_DEVICE_INFO.Create(structure.deviceInfo);
        this.RaiseInRange(BluetoothWin32RadioInRangeEventArgs.Create(structure.previousDeviceFlags, structure.deviceInfo.flags, deviceInfo));
      }
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.RadioOutOfRange == lparam.dbch_eventguid)
      {
        BluetoothWin32Events.BTH_RADIO_OUT_OF_RANGE structure = (BluetoothWin32Events.BTH_RADIO_OUT_OF_RANGE) Marshal.PtrToStructure(ptr, typeof (BluetoothWin32Events.BTH_RADIO_OUT_OF_RANGE));
        text += "GUID_BLUETOOTH_RADIO_OUT_OF_RANGE";
        text += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " 0x{0:X12}", (object) structure.deviceAddress);
        this.RaiseOutOfRange(BluetoothWin32RadioOutOfRangeEventArgs.Create(structure.deviceAddress));
      }
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.PinRequest == lparam.dbch_eventguid)
        text += "GUID_BLUETOOTH_PIN_REQUEST";
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.L2capEvent == lparam.dbch_eventguid)
        text += "GUID_BLUETOOTH_L2CAP_EVENT";
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.HciEvent == lparam.dbch_eventguid)
        text += "GUID_BLUETOOTH_HCI_EVENT";
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.AuthenticationRequestEvent == lparam.dbch_eventguid)
        text += "GUID_BLUETOOTH_AUTHENTICATION_REQUEST";
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.KeyPressEvent == lparam.dbch_eventguid)
        text += "GUID_BLUETOOTH_KEYPRESS_EVENT";
      else if (BluetoothWin32Events.BluetoothDeviceNotificationEvent.HciVendorEvent == lparam.dbch_eventguid)
      {
        text += "GUID_BLUETOOTH_HCI_VENDOR_EVENT";
      }
      else
      {
        ref string local = ref text;
        local = local + "Unknown event: " + (object) lparam.dbch_eventguid;
      }
    }

    private static void DoDevTypPort(
      ref Message m,
      ref string text,
      ref BluetoothWin32Events.DEV_BROADCAST_HDR hdr)
    {
      text += "Port: ";
      string stringUni = hdr.dbch_size - 12 <= 0 ? (string) null : Marshal.PtrToStringUni((IntPtr) (12L + m.LParam.ToInt64()));
      text += stringUni;
    }

    private sealed class RegisterDeviceNotificationSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      public RegisterDeviceNotificationSafeHandle()
        : base(true)
      {
      }

      protected override bool ReleaseHandle()
      {
        return BluetoothWin32Events.RegisterDeviceNotificationSafeHandle.btUnregister(this.handle);
      }

      internal static bool btUnregister(IntPtr hDevNotification)
      {
        bool flag = BluetoothWin32Events.UnsafeNativeMethods.UnregisterDeviceNotification(hDevNotification);
        hDevNotification = IntPtr.Zero;
        return flag;
      }
    }

    private sealed class BtEventsForm : Form
    {
      private BluetoothWin32Events _parent;
      private BluetoothWin32Events.RegisterDeviceNotificationSafeHandle _hDevNotification;

      internal BtEventsForm(BluetoothWin32Events parent)
      {
        this._parent = parent;
        this.Text = "32feet.NET WM_DEVICECHANGE Window";
      }

      protected override void Dispose(bool disposing)
      {
        try
        {
          if (this._hDevNotification == null || this._hDevNotification.IsClosed)
            return;
          this._hDevNotification.Close();
        }
        finally
        {
          base.Dispose(disposing);
        }
      }

      [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
      internal void btRegister(IntPtr bluetoothRadioHandle)
      {
        IntPtr handle = this.Handle;
        BluetoothWin32Events.DEV_BROADCAST_HANDLE notificationFilter = new BluetoothWin32Events.DEV_BROADCAST_HANDLE(bluetoothRadioHandle);
        BluetoothWin32Events.RegisterDeviceNotificationSafeHandle notificationSafeHandle = BluetoothWin32Events.UnsafeNativeMethods.RegisterDeviceNotification_SafeHandle(handle, ref notificationFilter, BluetoothWin32Events.RegisterDeviceNotificationFlags.DEVICE_NOTIFY_WINDOW_HANDLE);
        this._hDevNotification = !notificationSafeHandle.IsInvalid ? notificationSafeHandle : throw new Win32Exception();
      }

      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
      protected override void WndProc(ref Message m)
      {
        this._parent.HandleMessage(ref m);
        base.WndProc(ref m);
      }
    }

    private static class BluetoothDeviceNotificationEvent
    {
      public static readonly Guid BthPortDeviceInterface = new Guid("{0850302A-B344-4fda-9BE9-90576B8D46F0}");
      public static readonly Guid RadioInRange = new Guid("{EA3B5B82-26EE-450E-B0D8-D26FE30A3869}");
      public static readonly Guid RadioOutOfRange = new Guid("{E28867C9-C2AA-4CED-B969-4570866037C4}");
      public static readonly Guid PinRequest = new Guid("{BD198B7C-24AB-4B9A-8C0D-A8EA8349AA16}");
      public static readonly Guid L2capEvent = new Guid("{7EAE4030-B709-4AA8-AC55-E953829C9DAA}");
      public static readonly Guid HciEvent = new Guid("{FC240062-1541-49BE-B463-84C4DCD7BF7F}");
      public static readonly Guid AuthenticationRequestEvent = new Guid("{5DC9136D-996C-46DB-84F5-32C0A3F47352}");
      public static readonly Guid KeyPressEvent = new Guid("{D668DFCD-0F4E-4EFC-BFE0-392EEEC5109C}");
      public static readonly Guid HciVendorEvent = new Guid("{547247e6-45bb-4c33-af8c-c00efe15a71d}");
    }

    private struct BTH_RADIO_IN_RANGE
    {
      internal BTH_DEVICE_INFO deviceInfo;
      internal BluetoothDeviceInfoProperties previousDeviceFlags;
    }

    private struct BTH_RADIO_OUT_OF_RANGE
    {
      internal long deviceAddress;
    }

    private struct BTH_L2CAP_EVENT_INFO
    {
      internal readonly long bthAddress;
      internal readonly ushort psm;
      [MarshalAs(UnmanagedType.U1)]
      internal readonly bool connected;
      [MarshalAs(UnmanagedType.U1)]
      internal readonly bool initiated;

      internal BTH_L2CAP_EVENT_INFO(Version shutUpCompiler)
      {
        this.bthAddress = 0L;
        this.psm = (ushort) 0;
        this.connected = false;
        this.initiated = false;
      }
    }

    private enum HCI_CONNNECTION_TYPE : byte
    {
      ACL = 1,
      SCO = 2,
      LE = 3,
    }

    private struct BTH_HCI_EVENT_INFO
    {
      internal readonly long bthAddress;
      internal readonly BluetoothWin32Events.HCI_CONNNECTION_TYPE connectionType;
      [MarshalAs(UnmanagedType.U1)]
      internal readonly bool connected;

      internal BTH_HCI_EVENT_INFO(Version shutUpCompiler)
      {
        this.bthAddress = 0L;
        this.connectionType = (BluetoothWin32Events.HCI_CONNNECTION_TYPE) 0;
        this.connected = false;
      }
    }

    private enum RegisterDeviceNotificationFlags
    {
      DEVICE_NOTIFY_WINDOW_HANDLE = 0,
      DEVICE_NOTIFY_SERVICE_HANDLE = 1,
      DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4,
    }

    private static class UnsafeNativeMethods
    {
      [DllImport("User32.dll", EntryPoint = "RegisterDeviceNotification", CharSet = CharSet.Unicode, SetLastError = true)]
      internal static extern BluetoothWin32Events.RegisterDeviceNotificationSafeHandle RegisterDeviceNotification_SafeHandle(
        IntPtr hRecipient,
        ref BluetoothWin32Events.DEV_BROADCAST_HANDLE notificationFilter,
        BluetoothWin32Events.RegisterDeviceNotificationFlags flags);

      [DllImport("User32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UnregisterDeviceNotification(IntPtr handle);
    }

    private enum WindowMessageId
    {
      ActivateApp = 28, // 0x0000001C
      DeviceChange = 537, // 0x00000219
    }

    private enum Dbt
    {
      DevNodesChanged = 7,
      QueryChangeConfig = 23, // 0x00000017
      ConfigChanged = 24, // 0x00000018
      ConfigChangeCanceled = 25, // 0x00000019
      DeviceArrival = 32768, // 0x00008000
      DeviceQueryRemove = 32769, // 0x00008001
      DeviceQueryRemoveFailed = 32770, // 0x00008002
      DeviceRemovePending = 32771, // 0x00008003
      DeviceRemoveComplete = 32772, // 0x00008004
      DeviceTypeSpecific = 32773, // 0x00008005
      CustomEvent = 32774, // 0x00008006
      UserDefined = 65535, // 0x0000FFFF
    }

    private enum DbtDevTyp : uint
    {
      Oem,
      DevNode,
      Volume,
      Port,
      Network,
      DeviceInterface,
      Handle,
    }

    private struct DEV_BROADCAST_HDR
    {
      internal int dbch_size;
      internal BluetoothWin32Events.DbtDevTyp dbch_devicetype;
      internal int dbch_reserved;
    }

    private struct DEV_BROADCAST_PORT
    {
      internal BluetoothWin32Events.DEV_BROADCAST_HDR header;
      internal byte[] ____dbcp_name;
    }

    private struct DEV_BROADCAST_HANDLE
    {
      private const int SizeWithoutFakeDataArray = 40;
      private const int SizeOfOneByteWithPadding = 4;
      private const int SizeWithFakeDataArray = 44;
      private static int ActualSizeWithFakeDataArray;
      public BluetoothWin32Events.DEV_BROADCAST_HDR header;
      internal readonly IntPtr dbch_handle;
      internal readonly IntPtr dbch_hdevnotify;
      internal readonly Guid dbch_eventguid;
      internal readonly int dbch_nameoffset;

      public DEV_BROADCAST_HANDLE(IntPtr deviceHandle)
      {
        this.header.dbch_reserved = 0;
        this.dbch_hdevnotify = IntPtr.Zero;
        this.dbch_eventguid = Guid.Empty;
        this.dbch_nameoffset = 0;
        this.header.dbch_devicetype = BluetoothWin32Events.DbtDevTyp.Handle;
        this.dbch_handle = deviceHandle;
        if (BluetoothWin32Events.DEV_BROADCAST_HANDLE.ActualSizeWithFakeDataArray == 0)
          BluetoothWin32Events.DEV_BROADCAST_HANDLE.ActualSizeWithFakeDataArray = BluetoothWin32Events.DEV_BROADCAST_HANDLE.Pad(1 + Marshal.SizeOf(typeof (BluetoothWin32Events.DEV_BROADCAST_HANDLE)), IntPtr.Size);
        this.header.dbch_size = BluetoothWin32Events.DEV_BROADCAST_HANDLE.ActualSizeWithFakeDataArray;
      }

      private static int Pad(int size, int alignment)
      {
        return (size + alignment - 1) / alignment * alignment;
      }
    }

    private struct DEV_BROADCAST_HANDLE__withData
    {
      public BluetoothWin32Events.DEV_BROADCAST_HDR header;
      private readonly IntPtr dbch_handle;
      private readonly IntPtr dbch_hdevnotify;
      private readonly Guid dbch_eventguid;
      private readonly int dbch_nameoffset;
      private readonly byte dbch_data__0;

      private DEV_BROADCAST_HANDLE__withData(int weAreForPinvoke)
      {
        this.header = new BluetoothWin32Events.DEV_BROADCAST_HDR();
        this.dbch_handle = IntPtr.Zero;
        this.dbch_hdevnotify = IntPtr.Zero;
        this.dbch_eventguid = Guid.Empty;
        this.dbch_nameoffset = this.dbch_nameoffset = 0;
        this.dbch_data__0 = this.dbch_data__0 = (byte) 0;
      }
    }

    private static class ShutUpCompiler
    {
      internal static void ShutUpCompilerTheStructFieldAsUsed()
      {
        BluetoothWin32Events.DEV_BROADCAST_HDR devBroadcastHdr;
        devBroadcastHdr.dbch_size = 0;
        devBroadcastHdr.dbch_devicetype = BluetoothWin32Events.DbtDevTyp.DevNode;
        devBroadcastHdr.dbch_reserved = 0;
      }
    }
  }
}
