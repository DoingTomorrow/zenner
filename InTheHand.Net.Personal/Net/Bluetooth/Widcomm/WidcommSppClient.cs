// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommSppClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Ports;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommSppClient : WidcommSerialPort
  {
    private static int _sfInConnect;
    private WidcommBluetoothFactoryBase _fcty;
    private WidcommPortSingleThreader _singleThreader;
    private WidcommSppClient.NativeMethods.OnClientStateChange _handleClientStateChange;
    private IntPtr _pSppCli;
    private ManualResetEvent _waitConnect = new ManualResetEvent(false);
    private volatile WidcommSppClient.SPP_STATE_CODE _statusState;
    private byte[] _statusBda;
    private byte[] _statusDevClass;
    private byte[] _statusName;
    private short? _statusComPort;
    private volatile bool _disposed;
    private BluetoothAddress _addr;
    private short? _comNum;
    private string _comPortName;

    internal WidcommSppClient(WidcommBluetoothFactoryBase fcty)
    {
      this._fcty = fcty;
      this._singleThreader = fcty.GetSingleThreader();
      if (this._singleThreader == null)
        throw new InvalidOperationException("Internal Error: No GetSingleThreader");
      this._handleClientStateChange = new WidcommSppClient.NativeMethods.OnClientStateChange(this.HandleClientStateChange);
      bool flag = false;
      try
      {
        this._singleThreader.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => WidcommSppClient.NativeMethods.SppClient_Create(out this._pSppCli, this._handleClientStateChange)))).WaitCompletion();
        if (this._pSppCli == IntPtr.Zero)
          throw new InvalidOperationException("Failed to initialise CSppClient.");
        flag = true;
      }
      finally
      {
        int num = flag ? 1 : 0;
      }
    }

    public override BluetoothAddress Address => this._addr;

    public override Guid Service => BluetoothService.SerialPort;

    public override string PortName
    {
      get
      {
        return this._comPortName != null ? this._comPortName : throw new InvalidOperationException("Not connected");
      }
    }

    ~WidcommSppClient() => this.Dispose(false);

    protected override void Dispose(bool disposing)
    {
      if (this._pSppCli == IntPtr.Zero)
        return;
      this._disposed = true;
      IntPtr sppCli = this._pSppCli;
      try
      {
        this._singleThreader.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => WidcommSppClient.NativeMethods.SppClient_Destroy(sppCli)))).WaitCompletion(disposing);
      }
      finally
      {
        this._pSppCli = IntPtr.Zero;
        this._waitConnect.Close();
      }
    }

    private void HandleClientStateChange(
      IntPtr bdAddr,
      IntPtr devClass,
      IntPtr deviceName,
      short com_port,
      WidcommSppClient.SPP_STATE_CODE state)
    {
      try
      {
        WidcommUtils.GetBluetoothCallbackValues(bdAddr, devClass, deviceName, out this._statusBda, out this._statusDevClass, out this._statusName);
        this._statusComPort = new short?(com_port);
        this._statusState = state;
        this.MemoryBarrier();
        if (state == WidcommSppClient.SPP_STATE_CODE.CONNECTED && !WidcommSppClient.IsSet((WaitHandle) this._waitConnect))
        {
          string str = this.MakePortName(this._statusComPort.Value);
          this._comNum = new short?(com_port);
          this._comPortName = str;
          this._waitConnect.Set();
        }
        switch (state)
        {
          default:
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Event_Runner), (object) new PortStatusChangedEventArgs(state == WidcommSppClient.SPP_STATE_CODE.CONNECTED, this._comPortName, this._addr));
            break;
        }
      }
      catch (Exception ex)
      {
        MiscUtils.Trace_WriteLine("HandleClientStateChange ex: " + (object) ex);
      }
    }

    private void Event_Runner(object state)
    {
      this.OnPortStatusChanged((object) this, (PortStatusChangedEventArgs) state);
    }

    internal string CreatePort(BluetoothAddress addr)
    {
      if (WidcommSppClient.IsSet((WaitHandle) this._waitConnect))
        throw new InvalidOperationException("Already used.");
      this._addr = addr;
      byte[] bd_addr = WidcommUtils.FromBluetoothAddress(addr);
      byte[] tcharzServiceName = new byte[2];
      if (Interlocked.CompareExchange(ref WidcommSppClient._sfInConnect, 1, 0) != 0)
        throw new InvalidOperationException("Widcomm only allows one SPP Connect attempt at a time.");
      WidcommSppClient.SPP_CLIENT_RETURN_CODE ret = ~WidcommSppClient.SPP_CLIENT_RETURN_CODE.SUCCESS;
      this._singleThreader.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => ret = WidcommSppClient.NativeMethods.SppClient_CreateConnection(this._pSppCli, bd_addr, tcharzServiceName)))).WaitCompletion();
      bool flag = this._waitConnect.WaitOne(30000, false);
      Interlocked.Exchange(ref WidcommSppClient._sfInConnect, 0);
      if (!flag)
        throw WidcommSocketExceptions.Create_NoResultCode(10014, "CreatePort failed (time-out).");
      this.MemoryBarrier();
      if (this._statusState != WidcommSppClient.SPP_STATE_CODE.CONNECTED)
        throw WidcommSppSocketExceptions.Create(this._statusState, nameof (CreatePort));
      short? statusComPort = this._statusComPort;
      if (!(statusComPort.HasValue ? new int?((int) statusComPort.GetValueOrDefault()) : new int?()).HasValue)
        throw WidcommSocketExceptions.Create_NoResultCode(10014, "CreatePort did not complete (cpn).");
      return this._comPortName;
    }

    private string MakePortName(short comPortNumber) => "COM" + (object) comPortNumber;

    private tBT_CONN_STATS GetConnectionStats()
    {
      tBT_CONN_STATS stats = new tBT_CONN_STATS();
      WidcommSppClient.SPP_CLIENT_RETURN_CODE result = this._singleThreader.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<WidcommSppClient.SPP_CLIENT_RETURN_CODE>>(new WidcommPortSingleThreader.MiscReturnCommand<WidcommSppClient.SPP_CLIENT_RETURN_CODE>((Func<WidcommSppClient.SPP_CLIENT_RETURN_CODE>) (() => WidcommSppClient.NativeMethods.SppClient_GetConnectionStats(this._pSppCli, out stats, Marshal.SizeOf((object) stats))))).WaitCompletion();
      if (result != WidcommSppClient.SPP_CLIENT_RETURN_CODE.SUCCESS)
        throw WidcommSppSocketExceptions.Create(result, nameof (GetConnectionStats));
      return stats;
    }

    private static bool IsSet(WaitHandle waiter) => waiter.WaitOne(0, false);

    private void MemoryBarrier() => Thread.MemoryBarrier();

    private static class NativeMethods
    {
      [DllImport("32feetWidcomm")]
      internal static extern void SppClient_Create(
        out IntPtr ppSppClient,
        WidcommSppClient.NativeMethods.OnClientStateChange clientStateChange);

      [DllImport("32feetWidcomm")]
      internal static extern void SppClient_Destroy(IntPtr pSppClient);

      [DllImport("32feetWidcomm")]
      internal static extern WidcommSppClient.SPP_CLIENT_RETURN_CODE SppClient_RemoveConnection();

      [DllImport("32feetWidcomm")]
      internal static extern WidcommSppClient.SPP_CLIENT_RETURN_CODE SppClient_CreateConnection(
        IntPtr pSppClient,
        byte[] bda,
        IntPtr tcharzServiceName);

      [DllImport("32feetWidcomm")]
      internal static extern WidcommSppClient.SPP_CLIENT_RETURN_CODE SppClient_CreateConnection(
        IntPtr pSppClient,
        byte[] bda,
        byte[] tcharzServiceName);

      [DllImport("32feetWidcomm")]
      internal static extern WidcommSppClient.SPP_CLIENT_RETURN_CODE SppClient_GetConnectionStats(
        IntPtr pObj,
        out tBT_CONN_STATS pStats,
        int cb);

      internal delegate void OnClientStateChange(
        IntPtr bdAddr,
        IntPtr devClass,
        IntPtr deviceName,
        short com_port,
        WidcommSppClient.SPP_STATE_CODE state);
    }

    internal enum SPP_STATE_CODE
    {
      CONNECTED,
      DISCONNECTED,
      RFCOMM_CONNECTION_FAILED,
      PORT_IN_USE,
      PORT_NOT_CONFIGURED,
      SERVICE_NOT_FOUND,
      ALLOC_SCN_FAILED,
      SDP_FULL,
    }

    private enum SPP_STATE_CODE__WCE
    {
      CONNECTED,
      DISCONNECTED,
    }

    internal enum SPP_CLIENT_RETURN_CODE
    {
      SUCCESS,
      NO_BT_SERVER,
      ALREADY_CONNECTED,
      NOT_CONNECTED,
      NOT_ENOUGH_MEMORY,
      INVALID_PARAMETER__CE_UE,
      UNKNOWN_ERROR__CE_IP,
      NO_EMPTY_PORT,
      LICENSE_ERROR,
    }

    private enum SPP_CLIENT_RETURN_CODE__WCE
    {
      SUCCESS,
      NO_BT_SERVER,
      ALREADY_CONNECTED,
      NOT_CONNECTED,
      NOT_ENOUGH_MEMORY,
      UNKNOWN_ERROR,
      INVALID_PARAMETER,
    }
  }
}
