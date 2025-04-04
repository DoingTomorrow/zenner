// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BluesoleilFactory
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class BluesoleilFactory : BluetoothFactory
  {
    private readonly BluesoleilFactory.Records _records;
    private readonly IBluesoleilApi _api;
    private readonly BluesoleilSecurity _sec;
    private NativeMethods.Btsdk_Inquiry_Result_Ind_Func _inquiryResultIndFunc;
    private NativeMethods.Btsdk_Inquiry_Complete_Ind_Func _inquiryCompleteIndFunc;
    private NativeMethods.Btsdk_UserHandle_Pin_Req_Ind_Func _pinReqIndFunc;
    private NativeMethods.Btsdk_Connection_Event_Ind_Func _connectionEventIndFunc;
    private NativeMethods.Func_ReceiveBluetoothStatusInfo _statusCallback;
    private readonly BluesoleilFactory.BluesoleilInquiry _inquiryHandler;

    public BluesoleilFactory()
      : this((IBluesoleilApi) new RealBluesoleilApi())
    {
    }

    internal BluesoleilFactory(IBluesoleilApi api)
    {
      this._api = api != null ? api : throw new ArgumentNullException(nameof (api));
      this._records = new BluesoleilFactory.Records(api);
      this.SdkInit();
      if (!this.Api.Btsdk_IsBluetoothHardwareExisted())
        throw new PlatformNotSupportedException("BlueSoleil Bluetooth stack not supported (HardwareExisted).");
      this._inquiryHandler = new BluesoleilFactory.BluesoleilInquiry(this);
      this._sec = new BluesoleilSecurity(this);
      this.GetAllRadios();
    }

    [DebuggerStepThrough]
    internal void SdkInit() => this._records.SdkInit();

    protected override void Dispose(bool disposing)
    {
      if (this._records == null)
        return;
      this._records.Dispose(disposing);
    }

    internal IBluesoleilApi Api
    {
      [DebuggerStepThrough] get
      {
        this.SdkInit();
        return this._api;
      }
    }

    protected override IBluetoothDeviceInfo GetBluetoothDeviceInfo(BluetoothAddress address)
    {
      return (IBluetoothDeviceInfo) BluesoleilDeviceInfo.CreateFromGivenAddress(address, this);
    }

    protected override IBluetoothRadio GetPrimaryRadio()
    {
      this.SdkInit();
      return (IBluetoothRadio) new BluesoleilRadio(this);
    }

    protected override IBluetoothRadio[] GetAllRadios()
    {
      return new IBluetoothRadio[1]
      {
        this.GetPrimaryRadio()
      };
    }

    protected override IBluetoothClient GetBluetoothClient()
    {
      return (IBluetoothClient) new BluesoleilClient(this);
    }

    protected override IBluetoothClient GetBluetoothClient(BluetoothEndPoint localEP)
    {
      return (IBluetoothClient) new BluesoleilClient(this, localEP);
    }

    protected override IBluetoothClient GetBluetoothClient(Socket acceptedSocket)
    {
      throw new NotSupportedException("Cannot create a BluetoothClient from a Socket on the BlueSoleil stack.");
    }

    protected override IBluetoothClient GetBluetoothClientForListener(
      CommonRfcommStream acceptedStream)
    {
      throw new NotSupportedException();
    }

    protected override IBluetoothListener GetBluetoothListener()
    {
      throw new NotSupportedException("There seems to be no API in BlueSoleil for RFCOMM servers.");
    }

    protected override IBluetoothSecurity GetBluetoothSecurity() => (IBluetoothSecurity) this._sec;

    internal List<IBluetoothDeviceInfo> GetRememberedDevices(bool authenticated, bool remembered)
    {
      if (!remembered && !authenticated)
        return new List<IBluetoothDeviceInfo>();
      uint[] pdev_hdl = new uint[1000];
      int storedDevicesByClass = this.Api.Btsdk_GetStoredDevicesByClass(0U, pdev_hdl, pdev_hdl.Length);
      List<IBluetoothDeviceInfo> rememberedDevices = new List<IBluetoothDeviceInfo>();
      for (int index = 0; index < storedDevicesByClass; ++index)
      {
        BluesoleilDeviceInfo handleFromStored = BluesoleilDeviceInfo.CreateFromHandleFromStored(pdev_hdl[index], this);
        if (remembered || authenticated && handleFromStored.Authenticated)
          rememberedDevices.Add((IBluetoothDeviceInfo) handleFromStored);
      }
      return rememberedDevices;
    }

    internal void RegisterCallbacksOnce()
    {
      if (this._inquiryResultIndFunc != null)
        return;
      this._inquiryResultIndFunc = new NativeMethods.Btsdk_Inquiry_Result_Ind_Func(((CommonBluetoothInquiry<uint>) this._inquiryHandler).HandleInquiryResultInd);
      Structs.BtSdkCallbackStru call_back = new Structs.BtSdkCallbackStru(this._inquiryResultIndFunc);
      BluesoleilUtils.CheckAndThrow(this.Api.Btsdk_RegisterCallback4ThirdParty(ref call_back), "Btsdk_RegisterCallback4ThirdParty");
      this._inquiryCompleteIndFunc = new NativeMethods.Btsdk_Inquiry_Complete_Ind_Func(this.HandleInquiryComplete);
      call_back = new Structs.BtSdkCallbackStru(this._inquiryCompleteIndFunc);
      BluesoleilUtils.CheckAndThrow(this.Api.Btsdk_RegisterCallback4ThirdParty(ref call_back), "Btsdk_RegisterCallback4ThirdParty");
      this._pinReqIndFunc = new NativeMethods.Btsdk_UserHandle_Pin_Req_Ind_Func(this._sec.HandlePinReqInd);
      call_back = new Structs.BtSdkCallbackStru(this._pinReqIndFunc);
      BluesoleilUtils.CheckAndThrow(this.Api.Btsdk_RegisterCallback4ThirdParty(ref call_back), "Btsdk_RegisterCallback4ThirdParty");
      this._connectionEventIndFunc = new NativeMethods.Btsdk_Connection_Event_Ind_Func(this._records.HandleConnectionEventInd);
      call_back = new Structs.BtSdkCallbackStru(this._connectionEventIndFunc);
      BluesoleilUtils.CheckAndThrow(this.Api.Btsdk_RegisterCallback4ThirdParty(ref call_back), "Btsdk_RegisterCallback4ThirdParty");
      this._statusCallback = new NativeMethods.Func_ReceiveBluetoothStatusInfo(this.HandleReceiveBluetoothStatusInfo);
      BluesoleilUtils.CheckAndThrow(this.Api.Btsdk_RegisterGetStatusInfoCB4ThirdParty(ref this._statusCallback), "Btsdk_RegisterGetStatusInfoCB4ThirdParty");
      BluesoleilUtils.CheckAndThrow(this.Api.Btsdk_SetStatusInfoFlag((ushort) 2), "Btsdk_SetStatusInfoFlag");
    }

    private void HandleReceiveBluetoothStatusInfo(
      uint usMsgType,
      uint pulData,
      uint param,
      IntPtr arg)
    {
      if (pulData != 2U)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (_ => this._records.CloseAnyLiveConnections()), (object) null);
    }

    internal IAsyncResult BeginInquiry(
      int maxDevices,
      TimeSpan inquiryLength,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args)
    {
      return this._inquiryHandler.BeginInquiry(maxDevices, inquiryLength, callback, state, liveDiscoHandler, liveDiscoState, args);
    }

    internal List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      return this._inquiryHandler.EndInquiry(ar);
    }

    private void HandleInquiryComplete() => this._inquiryHandler.HandleInquiryComplete(new int?());

    internal int AddConnection(uint conn_hdl, IBluesoleilConnection newConn)
    {
      return this._records.AddConnection(conn_hdl, newConn);
    }

    private class BluesoleilInquiry : CommonBluetoothInquiry<uint>
    {
      private const uint AnyClass = 0;
      private readonly BluesoleilFactory _fcty;

      internal BluesoleilInquiry(BluesoleilFactory fcty) => this._fcty = fcty;

      internal IAsyncResult BeginInquiry(
        int maxDevices,
        TimeSpan inquiryLength,
        AsyncCallback callback,
        object state,
        BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
        object liveDiscoState,
        DiscoDevsParams args)
      {
        this._fcty.SdkInit();
        this._fcty.RegisterCallbacksOnce();
        byte maxDurations;
        byte maxNum;
        CommonDiscoveryBluetoothClient.ConvertBthInquiryParams(maxDevices, inquiryLength, out maxNum, out maxDurations);
        return this.BeginInquiry(maxDevices, inquiryLength, callback, state, liveDiscoHandler, liveDiscoState, (ThreadStart) (() => BluesoleilUtils.CheckAndThrow(this._fcty.Api.Btsdk_StartDeviceDiscovery(0U, (ushort) maxNum, (ushort) maxDurations), "Btsdk_StartDeviceDiscovery")), args);
      }

      protected override IBluetoothDeviceInfo CreateDeviceInfo(uint item)
      {
        return (IBluetoothDeviceInfo) BluesoleilDeviceInfo.CreateFromHandleFromInquiry(item, this._fcty);
      }
    }

    private class Records : CriticalFinalizerObject
    {
      private readonly IBluesoleilApi _api;
      private bool _needsDispose;
      private Dictionary<uint, IBluesoleilConnection> _liveConns = new Dictionary<uint, IBluesoleilConnection>();

      internal Records(IBluesoleilApi api) => this._api = api;

      ~Records() => this.Dispose(false);

      internal void Dispose(bool disposing)
      {
        if (!this._needsDispose)
          return;
        this.CloseAnyLiveConnections();
        this._needsDispose = false;
        BluesoleilUtils.Assert(this._api.Btsdk_Done(), "Btsdk_Done");
      }

      internal void SdkInit()
      {
        bool flag1 = this._api.Btsdk_IsSDKInitialized();
        bool flag2 = this._api.Btsdk_IsServerConnected();
        if (flag1 && flag2)
          return;
        BluesoleilUtils.CheckAndThrow(this._api.Btsdk_Init(), "Btsdk_Init");
        this._needsDispose = true;
      }

      internal void HandleConnectionEventInd(
        uint conn_hdl,
        StackConsts.ConnectionEventType eventType,
        IntPtr arg)
      {
        Structs.BtSdkConnectionPropertyStru structure = (Structs.BtSdkConnectionPropertyStru) Marshal.PtrToStructure(arg, typeof (Structs.BtSdkConnectionPropertyStru));
        string.Format((IFormatProvider) CultureInfo.InvariantCulture, "HandleConnectionEventInd event: {0}, conn_hdl: 0x{1:X}, arg.hDev: 0x{2:X} role_AND_result: 0x{3:X}.", (object) eventType, (object) conn_hdl, (object) structure.device_handle, (object) structure.role_AND_result);
        switch (eventType)
        {
          case StackConsts.ConnectionEventType.DISC_IND:
            this.UseNetworkDisconnectEvent(conn_hdl);
            goto case StackConsts.ConnectionEventType.DISC_CFM;
          case StackConsts.ConnectionEventType.DISC_CFM:
            this.RemoveConnection(conn_hdl);
            break;
        }
      }

      internal bool Contains(uint conn_hdl)
      {
        lock (this._liveConns)
          return this._liveConns.ContainsKey(conn_hdl);
      }

      internal int AddConnection(uint conn_hdl, IBluesoleilConnection newConn)
      {
        lock (this._liveConns)
        {
          if (!this._liveConns.ContainsKey(conn_hdl))
            this._liveConns.Add(conn_hdl, newConn);
          return this._liveConns.Count;
        }
      }

      private int RemoveConnection(uint conn_hdl)
      {
        lock (this._liveConns)
        {
          if (this._liveConns.ContainsKey(conn_hdl))
            this._liveConns.Remove(conn_hdl);
          return this._liveConns.Count;
        }
      }

      private int UseNetworkDisconnectEvent(uint conn_hdl)
      {
        lock (this._liveConns)
        {
          if (this._liveConns.ContainsKey(conn_hdl))
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.UseNetworkDisconnect_Runner), (object) this._liveConns[conn_hdl]);
          return this._liveConns.Count;
        }
      }

      private void UseNetworkDisconnect_Runner(object state)
      {
        ((IBluesoleilConnection) state).CloseNetworkOrInternal();
      }

      internal void CloseAnyLiveConnections()
      {
        ICollection<uint> keys;
        lock (this._liveConns)
          keys = (ICollection<uint>) this._liveConns.Keys;
        int num1 = 0;
        int num2 = 0;
        foreach (uint handle in (IEnumerable<uint>) keys)
        {
          BtSdkError btSdkError = this._api.Btsdk_Disconnect(handle);
          ++num1;
          if (btSdkError == BtSdkError.OK)
            ++num2;
        }
      }
    }
  }
}
