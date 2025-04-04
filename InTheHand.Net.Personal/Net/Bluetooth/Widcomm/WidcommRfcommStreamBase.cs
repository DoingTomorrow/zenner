// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommRfcommStreamBase
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.IO;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal abstract class WidcommRfcommStreamBase : CommonRfcommStream
  {
    private WidcommBluetoothFactoryBase m_factory;
    private IRfcommPort m_port;
    private IRfcommPort m_origPort;
    private string m_passcodeToTry;
    private WidcommRfcommInterface m_RfCommIf;
    private WidcommPortSingleThreader _singleThreader;
    private string m_debugId;
    private string m_origPortId;
    private string m_curPortId;
    private PORT_EV PORT_EV_ModemSignal = PORT_EV.CTS | PORT_EV.DSR | PORT_EV.RLSD | PORT_EV.BREAK | PORT_EV.ERR | PORT_EV.RING | PORT_EV.CTSS | PORT_EV.DSRS | PORT_EV.RLSDS | PORT_EV.OVERRUN;

    protected WidcommRfcommStreamBase(
      IRfcommPort port,
      IRfCommIf rfCommIf,
      WidcommBluetoothFactoryBase factory)
    {
      this.m_factory = factory;
      this._singleThreader = factory.GetSingleThreader();
      bool flag = false;
      try
      {
        this.SetPort(port);
        if (rfCommIf != null)
        {
          this.m_RfCommIf = new WidcommRfcommInterface(rfCommIf);
          rfCommIf.Create();
        }
        flag = true;
      }
      finally
      {
        if (!flag)
          GC.SuppressFinalize((object) this);
      }
    }

    private void SetPort(IRfcommPort port)
    {
      if (this.m_port != null)
        this.m_origPort = this.m_port;
      this.m_port = port;
      port.SetParentStream(this);
      this.DoPortCreate(port);
      this.ResetDebugId();
      if (this.m_origPortId == null)
        this.m_origPortId = port.DebugId;
      else
        this.m_curPortId = port.DebugId;
    }

    private void DoPortCreate(IRfcommPort port)
    {
      if (this._singleThreader != null)
        this.AddCommand<WidcommPortSingleThreader.PortCreateCommand>(new WidcommPortSingleThreader.PortCreateCommand(port)).WaitCompletion();
      else
        port.Create();
    }

    protected override void ResetDebugId() => this.m_debugId = (string) null;

    protected internal override string DebugId
    {
      get
      {
        if (this.m_debugId == null)
        {
          string str = this.m_origPortId;
          if (this.m_curPortId != null)
            str = str + "->" + this.m_curPortId;
          this.m_debugId = str;
        }
        return this.m_debugId;
      }
    }

    protected override void RemovePortRecords() => this.m_factory.RemovePort(this);

    protected override void DoPortClose(bool disposing)
    {
      PORT_RETURN_CODE portReturnCode = this._singleThreader == null ? this.m_port.Close() : this.AddCommand<WidcommPortSingleThreader.PortCloseCommand>(new WidcommPortSingleThreader.PortCloseCommand(this.m_port)).WaitCompletion(disposing);
      if (!disposing)
        ;
    }

    protected override void DoOtherPreDestroy(bool disposing)
    {
      if (this.m_RfCommIf == null)
        return;
      this.m_RfCommIf.Dispose(disposing);
    }

    protected override void DoPortDestroy(bool disposing)
    {
      if (this._singleThreader != null)
        this.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => this.m_port.Destroy()))).WaitCompletion(disposing);
      else
        this.m_port.Destroy();
    }

    internal IAsyncResult BeginConnect(
      BluetoothEndPoint bep,
      string pin,
      AsyncCallback asyncCallback,
      object state)
    {
      this.m_passcodeToTry = pin;
      return this.BeginConnect(bep, asyncCallback, state);
    }

    protected override void AddPortRecords() => this.m_factory.AddPort(this);

    protected override void DoOtherSetup(BluetoothEndPoint bep, int scn)
    {
      this.m_RfCommIf.SetScnForPeerServer(bep.Service, scn);
      this.m_RfCommIf.SetSecurityLevelClient(BTM_SEC.NONE);
    }

    protected override void DoOpenClient(int scn, BluetoothAddress addressToConnect)
    {
      byte[] address = WidcommUtils.FromBluetoothAddress(addressToConnect);
      PORT_RETURN_CODE result = this._singleThreader == null ? this.m_port.OpenClient(scn, address) : this.AddCommand<WidcommPortSingleThreader.OpenClientCommand>(new WidcommPortSingleThreader.OpenClientCommand(scn, address, this.m_port)).WaitCompletion();
      MiscUtils.Trace_WriteLine("OpenClient ret: {0}=0x{0:X}", (object) result);
      if (result != PORT_RETURN_CODE.SUCCESS)
        throw WidcommSocketExceptions.Create(result, "OpenClient");
    }

    protected override void DoOpenServer(int scn)
    {
      PORT_RETURN_CODE result = this._singleThreader == null ? this.m_port.OpenServer(scn) : this.AddCommand<WidcommPortSingleThreader.OpenServerCommand>(new WidcommPortSingleThreader.OpenServerCommand(scn, this.m_port)).WaitCompletion();
      MiscUtils.Trace_WriteLine("OpenServer ret: {0}=0x{0:X}", (object) result);
      if (result != PORT_RETURN_CODE.SUCCESS)
        throw WidcommSocketExceptions.Create(result, "OpenServer");
    }

    internal void HandlePortEvent(PORT_EV eventId, IRfcommPort port)
    {
      lock (this._lockKey)
      {
        if (port != this.m_port)
          return;
      }
      int num = 0;
      if ((eventId & this.PORT_EV_ModemSignal) != (PORT_EV) 0)
        ++num;
      if ((eventId & PORT_EV.CONNECTED) != (PORT_EV) 0)
      {
        ++num;
        this.HandleCONNECTED(eventId);
      }
      if ((eventId & PORT_EV.CONNECT_ERR) != (PORT_EV) 0)
      {
        ++num;
        this.HandleCONNECT_ERR(eventId);
      }
      if ((eventId & PORT_EV.TXCHAR) != (PORT_EV) 0)
        ++num;
      if ((eventId & PORT_EV.TXEMPTY) != (PORT_EV) 0)
      {
        ++num;
        this.FreePendingWrites();
      }
      if ((eventId & PORT_EV.FCS) != (PORT_EV) 0)
        ++num;
      if ((eventId & PORT_EV.FC) != (PORT_EV) 0 && (eventId & PORT_EV.FCS) == (PORT_EV) 0)
        ++num;
      if (num != 0)
        return;
      MiscUtils.Trace_WriteLine(this.DebugId + ": Unknown event: '{0}'=0x{0:X}", (object) eventId);
    }

    private void HandleCONNECTED(PORT_EV eventId) => this.HandleCONNECTED(eventId.ToString());

    protected override bool DoIsConnected(out BluetoothAddress p_remote_bdaddr)
    {
      bool flag;
      if (this._singleThreader != null)
      {
        WidcommRfcommStreamBase.IsConnectedResult isConnectedResult1 = this.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<WidcommRfcommStreamBase.IsConnectedResult>>(new WidcommPortSingleThreader.MiscReturnCommand<WidcommRfcommStreamBase.IsConnectedResult>((Func<WidcommRfcommStreamBase.IsConnectedResult>) (() =>
        {
          WidcommRfcommStreamBase.IsConnectedResult isConnectedResult2 = new WidcommRfcommStreamBase.IsConnectedResult();
          isConnectedResult2._connected = this.m_port.IsConnected(out isConnectedResult2._p_remote_bdaddr);
          return isConnectedResult2;
        }))).WaitCompletion();
        flag = isConnectedResult1._connected;
        p_remote_bdaddr = isConnectedResult1._p_remote_bdaddr;
      }
      else
        flag = this.m_port.IsConnected(out p_remote_bdaddr);
      return flag;
    }

    internal void HandlePortReceive(byte[] buffer, IRfcommPort port)
    {
      lock (this._lockKey)
      {
        if (port != this.m_port)
          return;
      }
      this.HandlePortReceive(buffer);
    }

    protected override void DoWrite(byte[] p_data, ushort len_to_write, out ushort p_len_written)
    {
      PORT_RETURN_CODE result = this._singleThreader == null ? this.m_port.Write(p_data, len_to_write, out p_len_written) : this.AddCommand<WidcommPortSingleThreader.PortWriteCommand>(new WidcommPortSingleThreader.PortWriteCommand(p_data, len_to_write, this.m_port)).WaitCompletion(out p_len_written);
      if (result != PORT_RETURN_CODE.SUCCESS)
        throw new IOException("IOError on socket.", (Exception) WidcommSocketExceptions.Create(result, "Write"));
    }

    private PORT_RETURN_CODE BackgroundWrite(
      byte[] p_data,
      ushort len_to_write,
      out ushort p_len_written,
      Thread callerThread)
    {
      return this.m_port.Write(p_data, len_to_write, out p_len_written);
    }

    private void HandleCONNECT_ERR(PORT_EV eventId)
    {
      this.HandleCONNECT_ERR(eventId.ToString(), new int?());
    }

    protected override bool TryBondingIf_inLock(
      BluetoothAddress addressToConnect,
      int ocScn,
      out Exception error)
    {
      try
      {
        if (this.m_passcodeToTry != null)
        {
          if (addressToConnect == (BluetoothAddress) null)
          {
            error = (Exception) null;
            return false;
          }
          string passcodeToTry = this.m_passcodeToTry;
          this.m_passcodeToTry = (string) null;
          if (this.Bond(addressToConnect, passcodeToTry))
          {
            this.SetPort(this.m_factory.GetWidcommRfcommPort());
            PORT_RETURN_CODE result = this.m_port.OpenClient(ocScn, WidcommUtils.FromBluetoothAddress(addressToConnect));
            MiscUtils.Trace_WriteLine("OpenClient/AB ret: {0}=0x{0:X}", (object) result);
            if (result == PORT_RETURN_CODE.SUCCESS)
            {
              error = (Exception) null;
              return true;
            }
            error = (Exception) WidcommSocketExceptions.Create(result, "OpenClient/AB");
            return false;
          }
        }
        error = (Exception) null;
      }
      catch (Exception ex)
      {
        error = ex;
      }
      return false;
    }

    private bool Bond(BluetoothAddress device, string passcode)
    {
      switch (((WidcommBluetoothSecurity) this.m_factory.DoGetBluetoothSecurity()).Bond_(device, passcode))
      {
        case BOND_RETURN_CODE.SUCCESS:
          return true;
        case BOND_RETURN_CODE.BAD_PARAMETER:
          return false;
        case BOND_RETURN_CODE.ALREADY_BONDED:
          return false;
        case BOND_RETURN_CODE.FAIL:
          return false;
        case BOND_RETURN_CODE.REPEATED_ATTEMPTS:
          return false;
        default:
          return false;
      }
    }

    private T AddCommand<T>(T cmd) where T : WidcommPortSingleThreader.StCommand
    {
      return this._singleThreader.AddCommand<T>(cmd);
    }

    private delegate PORT_RETURN_CODE FuncPortWrite(
      byte[] p_data,
      ushort len_to_write,
      out ushort p_len_written,
      Thread callerThread);

    private class IsConnectedResult
    {
      public bool _connected;
      public BluetoothAddress _p_remote_bdaddr;
    }
  }
}
