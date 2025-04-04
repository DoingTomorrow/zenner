// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.CommonBluetoothListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  internal abstract class CommonBluetoothListener : IBluetoothListener
  {
    private const int DefaultBacklog = 1;
    private const int RestrictPortCount = 1;
    private readonly BluetoothFactory m_factory;
    private volatile Exception _brokenE;
    private BluetoothEndPoint m_requestedLocalEP;
    private BluetoothEndPoint m_liveLocalEP;
    private ServiceRecord m_serviceRecord;
    private bool m_manualServiceRecord;
    private bool m_ServiceRecordAdded;
    private string m_serviceName;
    protected bool m_authenticate;
    protected bool m_encrypt;
    private Queue<AsyncResult<CommonRfcommStream>> m_callers = new Queue<AsyncResult<CommonRfcommStream>>();
    private Queue<CommonBluetoothListener.AcceptedPort> m_accepted = new Queue<CommonBluetoothListener.AcceptedPort>();
    private Dictionary<CommonRfcommStream, CommonRfcommStream> m_listening = new Dictionary<CommonRfcommStream, CommonRfcommStream>();
    private object m_key = new object();
    private int m_backlog = -1;

    protected CommonBluetoothListener(BluetoothFactory factory)
    {
      this.m_factory = factory;
      GC.SuppressFinalize((object) this);
    }

    public void Construct(BluetoothEndPoint localEP)
    {
      if (localEP == null)
        throw new ArgumentNullException(nameof (localEP));
      if (localEP.Address != BluetoothAddress.None)
        throw new NotSupportedException("Don't support binding to a particular local address/port.");
      this.m_requestedLocalEP = (BluetoothEndPoint) localEP.Clone();
    }

    public void Construct(BluetoothAddress localAddress, Guid service)
    {
      this.Construct(new BluetoothEndPoint(localAddress, service));
    }

    public void Construct(Guid service)
    {
      this.Construct(new BluetoothEndPoint(BluetoothAddress.None, service));
    }

    public void Construct(BluetoothEndPoint localEP, ServiceRecord sdpRecord)
    {
      this.Construct(localEP);
      this.m_serviceRecord = sdpRecord;
      this.m_manualServiceRecord = true;
    }

    public void Construct(BluetoothAddress localAddress, Guid service, ServiceRecord sdpRecord)
    {
      this.Construct(new BluetoothEndPoint(localAddress, service), sdpRecord);
    }

    public void Construct(Guid service, ServiceRecord sdpRecord)
    {
      this.Construct(new BluetoothEndPoint(BluetoothAddress.None, service), sdpRecord);
    }

    private static ServiceRecord ParseRaw(byte[] sdpRecord, int channelOffset)
    {
      return new ServiceRecordParser().Parse(sdpRecord);
    }

    public void Construct(BluetoothEndPoint localEP, byte[] sdpRecord, int channelOffset)
    {
      this.Construct(localEP, CommonBluetoothListener.ParseRaw(sdpRecord, channelOffset));
    }

    public void Construct(
      BluetoothAddress localAddress,
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
    {
      this.Construct(localAddress, service, CommonBluetoothListener.ParseRaw(sdpRecord, channelOffset));
    }

    public void Construct(Guid service, byte[] sdpRecord, int channelOffset)
    {
      this.Construct(service, CommonBluetoothListener.ParseRaw(sdpRecord, channelOffset));
    }

    public void Start() => this.Start(4);

    public void Start(int backlog)
    {
      BluetoothEndPoint liveLocalEP;
      this.SetupListener(this.m_requestedLocalEP, out liveLocalEP);
      this.m_liveLocalEP = liveLocalEP;
      this.m_backlog = Math.Max(1, Math.Min(backlog, 10));
      lock (this.m_key)
        this.StartEnoughNewListenerPort_inLock();
      if (this._brokenE != null)
        throw this._brokenE;
      this.AddServiceRecord(ref this.m_serviceRecord, this.m_liveLocalEP.Port, this.m_requestedLocalEP.Service, this.m_serviceName);
      this.m_ServiceRecordAdded = true;
    }

    protected virtual void VerifyPortIsInRange(BluetoothEndPoint bep)
    {
      if (bep.Port < 1 || bep.Port > 30)
        throw new ArgumentOutOfRangeException(nameof (bep), "Channel Number must be in the range 1 to 30.");
    }

    private void SetupListener(BluetoothEndPoint bep, out BluetoothEndPoint liveLocalEP)
    {
      if (bep == null)
        throw new ArgumentNullException(nameof (bep));
      int scn;
      if (bep.Port == 0 || bep.Port == -1)
      {
        scn = 0;
      }
      else
      {
        this.VerifyPortIsInRange(bep);
        scn = bep.Port;
      }
      this.SetupListener(bep, scn, out liveLocalEP);
    }

    private void AddServiceRecord(
      ref ServiceRecord fullServiceRecord,
      int livePort,
      Guid serviceClass,
      string serviceName)
    {
      if (fullServiceRecord != null)
        this.AddCustomServiceRecord(ref fullServiceRecord, livePort);
      else
        this.AddSimpleServiceRecord(out fullServiceRecord, livePort, serviceClass, serviceName);
    }

    protected abstract void SetupListener(
      BluetoothEndPoint bep,
      int scn,
      out BluetoothEndPoint liveLocalEP);

    protected abstract void AddCustomServiceRecord(
      ref ServiceRecord fullServiceRecord,
      int livePort);

    protected abstract void AddSimpleServiceRecord(
      out ServiceRecord fullServiceRecord,
      int livePort,
      Guid serviceClass,
      string serviceName);

    protected abstract bool IsDisposed { get; }

    private bool IsListening => this.m_liveLocalEP != null && !this.IsDisposed;

    public void Stop()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~CommonBluetoothListener() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      try
      {
        this.OtherDispose(disposing);
      }
      finally
      {
        AsyncResult<CommonRfcommStream>[] all1;
        CommonRfcommStream[] all2;
        CommonBluetoothListener.AcceptedPort[] all3;
        lock (this.m_key)
        {
          this.ClearAllCallers_inLock(out all1);
          this.ClearAllListeners_inLock(out all2);
          this.ClearAllAccepted_inLock(out all3);
        }
        this.Abort((IList<AsyncResult<CommonRfcommStream>>) all1);
        if (disposing)
        {
          this.OtherDisposeMore();
          this.Abort((IList<CommonRfcommStream>) all2);
          this.Abort((IList<CommonBluetoothListener.AcceptedPort>) all3);
        }
      }
    }

    protected abstract void OtherDispose(bool disposing);

    protected abstract void OtherDisposeMore();

    private void ClearAllCallers_inLock(out AsyncResult<CommonRfcommStream>[] all)
    {
      all = this.m_callers.ToArray();
      this.m_callers.Clear();
    }

    private void ClearAllListeners_inLock(out CommonRfcommStream[] all)
    {
      ICollection values = (ICollection) this.m_listening.Values;
      all = new CommonRfcommStream[values.Count];
      values.CopyTo((Array) all, 0);
      this.m_listening.Clear();
    }

    private void ClearAllAccepted_inLock(out CommonBluetoothListener.AcceptedPort[] all)
    {
      all = this.m_accepted.ToArray();
      this.m_accepted.Clear();
    }

    private void Abort(IList<AsyncResult<CommonRfcommStream>> all)
    {
      foreach (AsyncResultNoResult asyncResultNoResult in (IEnumerable<AsyncResult<CommonRfcommStream>>) all)
        asyncResultNoResult.SetAsCompleted((Exception) new ObjectDisposedException("BluetoothListener"), false);
    }

    private void Abort(IList<CommonRfcommStream> all)
    {
      foreach (Stream stream in (IEnumerable<CommonRfcommStream>) all)
        stream.Close();
    }

    private void Abort(IList<CommonBluetoothListener.AcceptedPort> all)
    {
      foreach (CommonBluetoothListener.AcceptedPort acceptedPort in (IEnumerable<CommonBluetoothListener.AcceptedPort>) all)
      {
        if (acceptedPort._port != null)
          acceptedPort._port.Close();
      }
    }

    public bool Pending()
    {
      lock (this.m_key)
        return this.m_accepted.Count > 0;
    }

    public Socket Server => throw new NotSupportedException("This stack does not use sockets.");

    private void StartEnoughNewListenerPort_inLock()
    {
      if (this.IsDisposed)
        return;
      int num = 0;
      while (this.m_listening.Count + this.m_accepted.Count < this.m_backlog && this.m_listening.Count < 1)
      {
        int count = this.m_listening.Count;
        try
        {
          this._StartOneNewListenerPort_inLock();
        }
        catch (Exception ex)
        {
          this._brokenE = ex;
          MiscUtils.Trace_WriteLine("Error calling StartEnoughNewListenerPort_inLock ex: " + (object) ex);
          break;
        }
        if (this.m_listening.Count == count + 1)
          ++num;
        else
          break;
      }
      if (this._brokenE == null)
        return;
      AsyncResult<CommonRfcommStream>[] all;
      this.ClearAllCallers_inLock(out all);
      foreach (AsyncResultNoResult asyncResultNoResult in all)
        asyncResultNoResult.SetAsCompleted((Exception) new TargetInvocationException(this._brokenE), AsyncResultCompletion.MakeAsync);
    }

    private void _StartOneNewListenerPort_inLock()
    {
      CommonRfcommStream newPort = this.GetNewPort();
      this.m_listening.Add(newPort, newPort);
      newPort.BeginAccept(this.m_liveLocalEP, (string) null, new AsyncCallback(this.PortAcceptCallback), (object) newPort);
    }

    protected abstract CommonRfcommStream GetNewPort();

    private void PortAcceptCallback(IAsyncResult ar)
    {
      AsyncResultNoResult ar1 = (AsyncResultNoResult) ar;
      CommonRfcommStream asyncState = (CommonRfcommStream) ar1.AsyncState;
      Exception error;
      AsyncResult<CommonRfcommStream> asyncResult;
      lock (this.m_key)
      {
        try
        {
          asyncState.EndAccept((IAsyncResult) ar1);
          error = (Exception) null;
        }
        catch (Exception ex)
        {
          int num = this.IsDisposed ? 1 : 0;
          if (this.IsDisposed)
            return;
          error = ex;
        }
        this.m_listening.Remove(asyncState);
        if (this.m_callers.Count > 0)
        {
          asyncResult = this.m_callers.Dequeue();
        }
        else
        {
          this.m_accepted.Enqueue(new CommonBluetoothListener.AcceptedPort(asyncState, error));
          asyncResult = (AsyncResult<CommonRfcommStream>) null;
        }
        this.StartEnoughNewListenerPort_inLock();
      }
      ThreadPool.QueueUserWorkItem(new WaitCallback(CommonBluetoothListener.RaiseAccept), (object) new CommonBluetoothListener.RaiseAcceptParams()
      {
        arSac = asyncResult,
        exSac = error,
        port = asyncState
      });
    }

    private static void RaiseAccept(object state)
    {
      CommonBluetoothListener.RaiseAcceptParams raiseAcceptParams = (CommonBluetoothListener.RaiseAcceptParams) state;
      if (raiseAcceptParams.arSac == null)
        return;
      if (raiseAcceptParams.exSac != null)
        raiseAcceptParams.arSac.SetAsCompleted(raiseAcceptParams.exSac, false);
      else
        raiseAcceptParams.arSac.SetAsCompleted(raiseAcceptParams.port, false);
    }

    public IBluetoothClient AcceptBluetoothClient()
    {
      return this.EndAcceptBluetoothClient(this.BeginAcceptBluetoothClient((AsyncCallback) null, (object) null));
    }

    public IAsyncResult BeginAcceptBluetoothClient(AsyncCallback callback, object state)
    {
      AsyncResult<CommonRfcommStream> asyncResult1 = new AsyncResult<CommonRfcommStream>(callback, state);
      CommonBluetoothListener.AcceptedPort acceptedPort;
      AsyncResult<CommonRfcommStream> asyncResult2;
      Exception exception;
      lock (this.m_key)
      {
        if (this.m_accepted.Count > 0)
        {
          acceptedPort = this.m_accepted.Dequeue();
          asyncResult2 = asyncResult1;
          exception = (Exception) null;
          this.StartEnoughNewListenerPort_inLock();
        }
        else
        {
          acceptedPort = (CommonBluetoothListener.AcceptedPort) null;
          if (!this.IsListening)
          {
            asyncResult2 = asyncResult1;
            exception = (Exception) new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
          }
          else if (this._brokenE != null)
          {
            asyncResult2 = asyncResult1;
            exception = this._brokenE;
          }
          else
          {
            this.m_callers.Enqueue(asyncResult1);
            asyncResult2 = (AsyncResult<CommonRfcommStream>) null;
            exception = (Exception) null;
          }
        }
      }
      if (asyncResult2 != null)
      {
        if (exception != null)
          asyncResult2.SetAsCompleted(exception, true);
        else if (acceptedPort._error != null)
          asyncResult2.SetAsCompleted(acceptedPort._port, true);
        else
          asyncResult2.SetAsCompleted(acceptedPort._port, true);
      }
      return (IAsyncResult) asyncResult1;
    }

    public IBluetoothClient EndAcceptBluetoothClient(IAsyncResult asyncResult)
    {
      return this.GetBluetoothClientForListener(((AsyncResult<CommonRfcommStream>) asyncResult).EndInvoke());
    }

    protected virtual IBluetoothClient GetBluetoothClientForListener(CommonRfcommStream strm)
    {
      return this.m_factory.DoGetBluetoothClientForListener(strm);
    }

    public Socket AcceptSocket()
    {
      throw new NotSupportedException("This stack does not use Sockets.");
    }

    public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
    {
      throw new NotSupportedException("This stack does not use Sockets.");
    }

    public Socket EndAcceptSocket(IAsyncResult asyncResult)
    {
      throw new NotSupportedException("This stack does not use Sockets.");
    }

    public BluetoothEndPoint LocalEndPoint => this.m_liveLocalEP;

    public ServiceClass ServiceClass
    {
      get => throw new NotImplementedException("The method or operation is not implemented.");
      set
      {
      }
    }

    public string ServiceName
    {
      get => this.m_serviceName;
      set
      {
        if (this.m_ServiceRecordAdded)
          throw new InvalidOperationException("Can not change ServiceName when started.");
        if (this.m_manualServiceRecord)
          throw new InvalidOperationException("ServiceName may not be specified when a custom Service Record is being used.");
        this.m_serviceName = value;
      }
    }

    public ServiceRecord ServiceRecord => this.m_serviceRecord;

    public bool Authenticate
    {
      get => this.m_authenticate;
      set => this.m_authenticate = value;
    }

    public bool Encrypt
    {
      get => this.m_encrypt;
      set => this.m_encrypt = value;
    }

    public void SetPin(string pin)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    public void SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    private class AcceptedPort
    {
      public readonly CommonRfcommStream _port;
      public readonly Exception _error;

      public AcceptedPort(CommonRfcommStream port, Exception error)
      {
        this._port = port != null ? port : throw new ArgumentNullException(nameof (port));
        this._error = error;
        Exception error1 = this._error;
      }
    }

    private class RaiseAcceptParams
    {
      internal AsyncResult<CommonRfcommStream> arSac { get; set; }

      internal Exception exSac { get; set; }

      internal CommonRfcommStream port { get; set; }
    }
  }
}
