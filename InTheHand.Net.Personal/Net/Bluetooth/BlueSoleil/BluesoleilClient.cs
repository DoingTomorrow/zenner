// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BluesoleilClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class BluesoleilClient : CommonDiscoveryBluetoothClient
  {
    private static readonly int? BufferSize = new int?();
    private readonly BluesoleilFactory _factory;
    private readonly Action<BluetoothEndPoint> _beginConnectDlgt;
    private uint? _hDev;
    private SerialPortNetworkStream _stream;
    private BluetoothEndPoint _remoteEp;

    internal BluesoleilClient(BluesoleilFactory fcty)
    {
      this._factory = fcty != null ? fcty : throw new ArgumentNullException(nameof (fcty));
      this._beginConnectDlgt = new Action<BluetoothEndPoint>(((CommonDiscoveryBluetoothClient) this).Connect);
    }

    internal BluesoleilClient(BluesoleilFactory fcty, BluetoothEndPoint localEP)
      : this(fcty)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (!disposing || this._stream == null)
        return;
      this._stream.Close();
    }

    protected override void BeginInquiry(
      int maxDevices,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args)
    {
      this._factory.BeginInquiry(maxDevices, this.InquiryLength, callback, state, liveDiscoHandler, liveDiscoState, args);
    }

    protected override List<IBluetoothDeviceInfo> GetKnownRemoteDeviceEntries()
    {
      return this._factory.GetRememberedDevices(true, true);
    }

    protected override List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      return this._factory.EndInquiry(ar);
    }

    public override Socket Client
    {
      get => throw new NotSupportedException("This stack does not use Sockets.");
      set => throw new NotSupportedException("This stack does not use Sockets.");
    }

    private void ConnectRfcommPreAllocateComPort(
      ushort svcClass16,
      uint hDev,
      out uint hConn,
      out byte channel,
      out int comPort)
    {
      uint aserialNum = this._factory.Api.Btsdk_GetASerialNum();
      uint comportNumber;
      if (!this._factory.Api.Btsdk_PlugInVComm(aserialNum, out comportNumber, 1U, StackConsts.COMM_SET.UsageType | StackConsts.COMM_SET.Record, 2200U))
        BluesoleilUtils.CheckAndThrow(BtSdkError.OPERATION_FAILURE, "Btsdk_PlugInVComm");
      comPort = checked ((int) comportNumber);
      BluesoleilUtils.CheckAndThrow(this._factory.Api.Btsdk_InitCommObj(checked ((byte) comportNumber), (ushort) 4353), "Btsdk_InitCommObj");
      bool flag = false;
      try
      {
        Structs.BtSdkSPPConnParamStru lParam = new Structs.BtSdkSPPConnParamStru(comportNumber);
        BluesoleilUtils.CheckAndThrow(this._factory.Api.Btsdk_ConnectEx(hDev, svcClass16, ref lParam, out hConn), "Btsdk_ConnectEx");
        this._hDev = new uint?(hDev);
        channel = (byte) 0;
        Console.WriteLine("Connect remote SPP Service with local COM{0}\n", (object) comportNumber);
        flag = true;
      }
      finally
      {
        if (!flag)
          BluesoleilClient.FreeComIndex(this._factory, comPort, aserialNum);
      }
    }

    internal static void FreeComIndex(BluesoleilFactory factory, int comNum, uint comSerialNum)
    {
      byte com_idx = checked ((byte) comNum);
      BluesoleilUtils.Assert(factory.Api.Btsdk_DeinitCommObj(com_idx), "Btsdk_DeinitCommObj");
      factory.Api.Btsdk_PlugOutVComm(comSerialNum, StackConsts.COMM_SET.Record);
    }

    private void PlayConnectSppSvc()
    {
      uint conn_hdl = 0;
      uint dev_hdl = 0;
      uint comportNumber;
      this._factory.Api.Btsdk_PlugInVComm(this._factory.Api.Btsdk_GetASerialNum(), out comportNumber, 1U, StackConsts.COMM_SET.UsageType | StackConsts.COMM_SET.Record, 2200U);
      if (this._factory.Api.Btsdk_InitCommObj(checked ((byte) comportNumber), (ushort) 4353) == BtSdkError.OK)
        return;
      Structs.BtSdkSPPConnParamStru lParam = new Structs.BtSdkSPPConnParamStru(comportNumber);
      if (this._factory.Api.Btsdk_ConnectEx(dev_hdl, (ushort) 4353, ref lParam, out conn_hdl) == BtSdkError.OK)
        return;
      Console.WriteLine("Connect remote SPP Service with local COM{0}\n", (object) comportNumber);
    }

    private void PlayDisconnect()
    {
      uint num1 = 0;
      byte serialNum = 0;
      byte clientPort = checked ((byte) this._factory.Api.Btsdk_GetClientPort(num1));
      int num2 = (int) this._factory.Api.Btsdk_Disconnect(num1);
      int num3 = (int) this._factory.Api.Btsdk_DeinitCommObj(clientPort);
      this._factory.Api.Btsdk_PlugOutVComm((uint) serialNum, StackConsts.COMM_SET.Record);
    }

    private void ConnectRfcomm(
      BluetoothEndPoint remoteEP,
      uint hDev,
      out uint hConn,
      out byte channel,
      out int comPort)
    {
      Structs.BtSdkAppExtSPPAttrStru psvc = new Structs.BtSdkAppExtSPPAttrStru(remoteEP);
      BtSdkError ret = this._factory.Api.Btsdk_ConnectAppExtSPPService(hDev, ref psvc, out hConn);
      this._hDev = new uint?(hDev);
      BluesoleilUtils.CheckAndThrow(ret, "Btsdk_ConnectAppExtSPPService");
      short clientPort = this._factory.Api.Btsdk_GetClientPort(hConn);
      if (psvc.rf_svr_chnl != (byte) 0)
      {
        channel = psvc.rf_svr_chnl;
        comPort = (int) psvc.com_index;
      }
      else if (clientPort != (short) 0)
      {
        comPort = (int) clientPort;
        channel = (byte) 0;
      }
      else
      {
        Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "BlueSoleil seems no RFCOMM connection made, closing. (channel: {0}, COM: {1})", (object) psvc.rf_svr_chnl, (object) psvc.com_index));
        this._factory.AddConnection(hConn, (IBluesoleilConnection) NullBluesoleilConnection.Instance);
        BluesoleilUtils.Assert(this._factory.Api.Btsdk_Disconnect(hConn), "Close non-RFCOMM connection");
        throw BluesoleilUtils.ErrorConnectIsNonRfcomm();
      }
    }

    public override void Connect(BluetoothEndPoint remoteEP)
    {
      if (remoteEP.Port != 0 && remoteEP.Port != -1)
        throw new NotSupportedException("Don't support connect to particular port.  Please can someone tell me how.");
      this._factory.SdkInit();
      this._factory.RegisterCallbacksOnce();
      uint handle = BluesoleilDeviceInfo.CreateFromGivenAddress(remoteEP.Address, this._factory).Handle;
      uint hConn;
      byte channel;
      int comPort;
      this.ConnectRfcomm(remoteEP, handle, out hConn, out channel, out comPort);
      this._remoteEp = new BluetoothEndPoint(remoteEP.Address, BluetoothService.Empty, (int) channel);
      ISerialPortWrapper serialPort = this.CreateSerialPort();
      if (BluesoleilClient.BufferSize.HasValue)
      {
        serialPort.ReadBufferSize = BluesoleilClient.BufferSize.Value;
        serialPort.WriteBufferSize = BluesoleilClient.BufferSize.Value;
      }
      serialPort.PortName = "COM" + (object) comPort;
      serialPort.Handshake = Handshake.RequestToSend;
      serialPort.Open();
      BlueSoleilSerialPortNetworkStream newConn = new BlueSoleilSerialPortNetworkStream(serialPort, hConn, this, this._factory);
      this._stream = (SerialPortNetworkStream) newConn;
      this._factory.AddConnection(hConn, (IBluesoleilConnection) newConn);
    }

    private ISerialPortWrapper CreateSerialPort()
    {
      return this.CreateSerialPortMethod == null ? (ISerialPortWrapper) new SerialPortWrapper(new SerialPort()) : this.CreateSerialPortMethod();
    }

    public Func<ISerialPortWrapper> CreateSerialPortMethod { get; set; }

    public override IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      return this._beginConnectDlgt.BeginInvoke(remoteEP, requestCallback, state);
    }

    public override void EndConnect(IAsyncResult asyncResult)
    {
      this._beginConnectDlgt.EndInvoke(asyncResult);
    }

    public override bool Connected
    {
      get
      {
        SerialPortNetworkStream stream = this._stream;
        if (stream == null)
          return false;
        int num = this._hDev.HasValue ? 1 : 0;
        return stream.Connected;
      }
    }

    public override int Available
    {
      get => (this._stream ?? throw new InvalidOperationException("Not connected.")).Available;
    }

    public override bool Authenticate
    {
      get => false;
      set
      {
        throw new NotSupportedException("BlueSoleil does not support setting authentication/encryption.");
      }
    }

    public override bool Encrypt
    {
      get => false;
      set
      {
        throw new NotSupportedException("BlueSoleil does not support setting authentication/encryption.");
      }
    }

    public override string GetRemoteMachineName(BluetoothAddress device)
    {
      return this._factory.DoGetBluetoothDeviceInfo(device).DeviceName;
    }

    public override NetworkStream GetStream()
    {
      return this._stream != null ? (NetworkStream) this._stream : throw new InvalidOperationException("Not connected.");
    }

    public override LingerOption LingerState
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public override Guid LinkKey => throw new NotImplementedException();

    public override LinkPolicy LinkPolicy => throw new NotImplementedException();

    public override BluetoothEndPoint RemoteEndPoint
    {
      get
      {
        return this.Connected && this._remoteEp != null ? this._remoteEp : throw new InvalidOperationException("Not connected.");
      }
    }

    public override string RemoteMachineName
    {
      get
      {
        if (!this.Connected || !this._hDev.HasValue)
          throw new InvalidOperationException("Not connected.");
        return BluesoleilDeviceInfo.CreateFromHandleFromConnection(this._hDev.Value, this._factory).DeviceName;
      }
    }

    public override void SetPin(string pin) => throw new NotImplementedException();

    public override void SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException();
    }
  }
}
