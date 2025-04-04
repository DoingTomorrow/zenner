// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.BluetoothConnector
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Widcomm;
using System;
using System.Collections.Generic;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  internal abstract class BluetoothConnector
  {
    protected bool m_disposed;
    private IUsesBluetoothConnectorImplementsServiceLookup _parent;
    private AsyncResultNoResult m_arConnect;
    protected int? _remotePort;
    protected BluetoothEndPoint m_remoteEndPoint;

    internal BluetoothConnector(
      IUsesBluetoothConnectorImplementsServiceLookup parent)
    {
      this._parent = parent;
    }

    protected void Connect_SetAsCompleted_CompletedSyncFalse(
      AsyncResultNoResult arConnect_Debug,
      Exception ex)
    {
      AsyncResultNoResult asyncResultNoResult = Interlocked.Exchange<AsyncResultNoResult>(ref this.m_arConnect, (AsyncResultNoResult) null);
      if (asyncResultNoResult == null)
        return;
      ThreadPool.QueueUserWorkItem(new WaitCallback(BluetoothConnector.RaiseConnect), (object) new BluetoothConnector.RaiseConnectParams()
      {
        arOrig = asyncResultNoResult,
        ex = ex
      });
    }

    private static void RaiseConnect(object state)
    {
      BluetoothConnector.RaiseConnectParams raiseConnectParams = (BluetoothConnector.RaiseConnectParams) state;
      raiseConnectParams.arOrig.SetAsCompleted(raiseConnectParams.ex, false);
    }

    public void Connect(BluetoothEndPoint remoteEP)
    {
      this.EndConnect(this.BeginConnect(remoteEP, (AsyncCallback) null, (object) null));
    }

    public IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      BluetoothEndPoint bluetoothEndPoint = (BluetoothEndPoint) remoteEP.Clone();
      AsyncResultNoResult arCliConnect = new AsyncResultNoResult(requestCallback, state);
      if (Interlocked.CompareExchange<AsyncResultNoResult>(ref this.m_arConnect, arCliConnect, (AsyncResultNoResult) null) != null)
        throw new InvalidOperationException("Another Connect operation is already in progress.");
      this.BeginFillInPort(bluetoothEndPoint, new AsyncCallback(this.Connect_FillInPortCallback), (object) new BluetoothConnector.BeginConnectState(bluetoothEndPoint, arCliConnect));
      return (IAsyncResult) arCliConnect;
    }

    public void EndConnect(IAsyncResult asyncResult)
    {
      ((AsyncResultNoResult) asyncResult).EndInvoke();
    }

    private void Connect_FillInPortCallback(IAsyncResult ar)
    {
      BluetoothConnector.BeginConnectState asyncState = (BluetoothConnector.BeginConnectState) ar.AsyncState;
      AsyncResultNoResult arCliConnect = asyncState.arCliConnect;
      try
      {
        BluetoothEndPoint remoteEP = this.EndFillInPort(ar);
        if (arCliConnect.IsCompleted)
          return;
        this._remotePort = new int?(remoteEP.Port);
        this.ConnBeginConnect(remoteEP, new AsyncCallback(this.Connect_ConnCallback), (object) asyncState);
      }
      catch (Exception ex)
      {
        this.Connect_SetAsCompleted_CompletedSyncFalse(arCliConnect, ex);
      }
    }

    protected abstract IAsyncResult ConnBeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state);

    protected abstract void ConnEndConnect(IAsyncResult asyncResult);

    private void Connect_ConnCallback(IAsyncResult ar)
    {
      AsyncResultNoResult arCliConnect = ((BluetoothConnector.BeginConnectState) ar.AsyncState).arCliConnect;
      try
      {
        this.ConnEndConnect(ar);
        this.Connect_SetAsCompleted_CompletedSyncFalse(arCliConnect, (Exception) null);
      }
      catch (Exception ex)
      {
        this.Connect_SetAsCompleted_CompletedSyncFalse(arCliConnect, ex);
      }
    }

    private IAsyncResult BeginFillInPort(
      BluetoothEndPoint bep,
      AsyncCallback asyncCallback,
      object state)
    {
      MiscUtils.Trace_WriteLine("BeginFillInPortState");
      AsyncResult<BluetoothEndPoint> arFillInPort = new AsyncResult<BluetoothEndPoint>(asyncCallback, state);
      if (bep.Port != 0 && bep.Port != -1)
      {
        MiscUtils.Trace_WriteLine("BeginFillInPort, has port -> Completed Syncronously");
        arFillInPort.SetAsCompleted(bep, true);
        return (IAsyncResult) arFillInPort;
      }
      this._parent.BeginServiceDiscovery(bep.Address, bep.Service, new AsyncCallback(this.FillInPort_ServiceDiscoveryCallback), (object) new BluetoothConnector.BeginFillInPortState(bep, arFillInPort));
      return (IAsyncResult) arFillInPort;
    }

    private BluetoothEndPoint EndFillInPort(IAsyncResult ar)
    {
      return ((AsyncResult<BluetoothEndPoint>) ar).EndInvoke();
    }

    private void FillInPort_ServiceDiscoveryCallback(IAsyncResult ar)
    {
      BluetoothConnector.BeginFillInPortState asyncState = (BluetoothConnector.BeginFillInPortState) ar.AsyncState;
      AsyncResult<BluetoothEndPoint> arFillInPort = asyncState.arFillInPort;
      try
      {
        this.DoEndServiceDiscovery(ar, asyncState.inputEP, arFillInPort);
      }
      catch (Exception ex)
      {
        arFillInPort.SetAsCompleted(ex, false);
      }
    }

    internal static List<int> ListAllRfcommPortsInRecords(List<ServiceRecord> list)
    {
      List<int> intList = new List<int>();
      foreach (ServiceRecord record in list)
      {
        int rfcommChannelNumber = ServiceRecordHelper.GetRfcommChannelNumber(record);
        intList.Add(rfcommChannelNumber);
      }
      return intList;
    }

    internal static List<int> ListAllL2CapPortsInRecords(List<ServiceRecord> list)
    {
      List<int> intList = new List<int>();
      foreach (ServiceRecord record in list)
      {
        int capChannelNumber = ServiceRecordHelper.GetL2CapChannelNumber(record);
        intList.Add(capChannelNumber);
      }
      return intList;
    }

    private void DoEndServiceDiscovery(
      IAsyncResult ar,
      BluetoothEndPoint inputEP,
      AsyncResult<BluetoothEndPoint> arFipToBeSACd)
    {
      try
      {
        List<int> intList = this._parent.EndServiceDiscovery(ar);
        if (intList.Count == 0)
        {
          MiscUtils.Trace_WriteLine("DoEndServiceDiscovery, zero records!");
          arFipToBeSACd.SetAsCompleted((Exception) WidcommSocketExceptions.Create_NoResultCode(10049, "PortLookup_Zero"), false);
        }
        else
        {
          MiscUtils.Trace_WriteLine("DoEndServiceDiscovery, got {0} records.", (object) intList.Count);
          foreach (int port in intList)
          {
            if (port != -1)
            {
              MiscUtils.Trace_WriteLine("FillInPort_ServiceDiscoveryCallback, got port: {0}", (object) port);
              BluetoothEndPoint result = new BluetoothEndPoint(inputEP.Address, inputEP.Service, port);
              arFipToBeSACd.SetAsCompleted(result, false);
              return;
            }
          }
          MiscUtils.Trace_WriteLine("FillInPort_ServiceDiscoveryCallback, no scn found");
          arFipToBeSACd.SetAsCompleted((Exception) WidcommSocketExceptions.Create_NoResultCode(10064, "PortLookup_NoneRfcomm"), false);
        }
      }
      catch (Exception ex)
      {
        arFipToBeSACd.SetAsCompleted(ex, false);
      }
    }

    private class RaiseConnectParams
    {
      internal Exception ex { get; set; }

      internal AsyncResultNoResult arOrig { get; set; }
    }

    private sealed class BeginConnectState
    {
      internal AsyncResultNoResult arCliConnect;

      public BeginConnectState(BluetoothEndPoint inputEP, AsyncResultNoResult arCliConnect)
      {
        this.arCliConnect = arCliConnect;
      }
    }

    private sealed class BeginFillInPortState
    {
      internal BluetoothEndPoint inputEP;
      internal AsyncResult<BluetoothEndPoint> arFillInPort;

      public BeginFillInPortState(
        BluetoothEndPoint inputEP,
        AsyncResult<BluetoothEndPoint> arFillInPort)
      {
        this.inputEP = inputEP;
        this.arFillInPort = arFillInPort;
      }
    }
  }
}
