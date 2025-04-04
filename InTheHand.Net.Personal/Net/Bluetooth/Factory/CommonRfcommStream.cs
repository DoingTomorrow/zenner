// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.CommonRfcommStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  [CLSCompliant(false)]
  public abstract class CommonRfcommStream : Stream
  {
    private const string ObjectDisposedException_ObjectName = "Network";
    internal const string WrappingIOExceptionMessage = "IOError on socket.";
    protected CommonRfcommStream.State m_state_;
    private bool m_connected;
    protected object _lockKey = new object();
    private BluetoothAddress m_remoteAddress;
    private BluetoothAddress m_addressToConnect;
    private int m_ocScn;
    private AsyncResultNoResult m_arConnect;
    private AsyncResultNoResult m_arConnectCompleted;
    private int m_receivedDataFirstBlockOffset;
    private Queue<byte[]> m_receivedData = new Queue<byte[]>();
    private int m_amountInReadBuffers;
    private Queue<AsyncResult<int, CommonRfcommStream.BeginReadParameters>> m_arReceiveList = new Queue<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>();
    private Queue<AsyncNoResult<CommonRfcommStream.BeginReadParameters>> m_arWriteQueue = new Queue<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>();
    private ManualResetEvent m_writeEmptied;
    private LingerOption m_lingerState = new LingerOption(true, 0);
    private int _readTimeout = -1;
    private int _writeTimeout = -1;

    [Conditional("DEBUG")]
    protected void Log(string message) => MiscUtils.Trace_WriteLine(message);

    [Conditional("DEBUG")]
    protected void LogFormat(string format, params object[] args)
    {
    }

    protected virtual void ResetDebugId()
    {
    }

    protected internal virtual string DebugId => "NULL_DEBUGID";

    internal LingerOption LingerState
    {
      get => this.m_lingerState;
      set
      {
        this.m_lingerState = value.Enabled ? value : throw new ArgumentException("No support for non-Linger mode in Widcomm wrapper.");
      }
    }

    private void EnsureOpenForWrite()
    {
      this.EnsureOpenForRead();
      if (this.m_state_ == CommonRfcommStream.State.Connected)
        return;
      if (this.m_state_ == CommonRfcommStream.State.PeerDidClose)
      {
        this.SetAsDisconnected();
        throw new IOException("IOError on socket.", FooSocketExceptions.ConnectionIsPeerClosed());
      }
      throw new InvalidOperationException("Not connected.");
    }

    private void EnsureOpenForRead()
    {
      if (this.m_state_ == CommonRfcommStream.State.Closed)
        throw new IOException("IOError on socket.", (Exception) new ObjectDisposedException("Network"));
      if (this.m_state_ == CommonRfcommStream.State.New)
        throw new InvalidOperationException("Not connected.");
      if (!this.m_connected)
        throw new IOException("IOError on socket.", FooSocketExceptions.ConnectionIsPeerClosed());
      if (this.m_state_ != CommonRfcommStream.State.Connected && this.m_state_ != CommonRfcommStream.State.PeerDidClose)
        throw new InvalidOperationException("Not connected.");
    }

    internal bool Connected => this.m_connected;

    internal bool LiveConnected => this.m_state_ == CommonRfcommStream.State.Connected;

    private void SetAsDisconnected() => this.SetAsDisconnectedFromDisposal();

    private void SetAsDisconnectedFromDisposal() => this.m_connected = false;

    private void ImplicitPeerClose() => this.m_state_ = CommonRfcommStream.State.PeerDidClose;

    protected override void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
      CommonRfcommStream.State state = this.m_state_;
      AsyncResultNoResult asyncResultNoResult = (AsyncResultNoResult) null;
      Exception exToThrow = (Exception) null;
      bool dbgExpectEmptyWriteQueue = false;
      try
      {
        if (this.m_state_ != CommonRfcommStream.State.Closed)
        {
          try
          {
            this.m_state_ = CommonRfcommStream.State.Closed;
            this.SetAsDisconnectedFromDisposal();
            this.DisposeLinger(disposing, out exToThrow, out dbgExpectEmptyWriteQueue);
            AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] all1;
            AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] all2;
            lock (this._lockKey)
            {
              this.RemovePortRecords();
              this.DoPortClose(disposing);
              this.ClearAllReadRequests_inLock(out all1);
              this.ClearAllWriteRequests_inLock(out all2);
              if (this.m_arConnect != null)
              {
                asyncResultNoResult = this.m_arConnect;
                this.m_arConnectCompleted = this.m_arConnect;
                this.m_arConnect = (AsyncResultNoResult) null;
              }
              ManualResetEvent writeEmptied = this.m_writeEmptied;
              if (disposing)
              {
                if (writeEmptied != null)
                {
                  this.m_writeEmptied = (ManualResetEvent) null;
                  writeEmptied.Close();
                }
              }
            }
            this.AbortReads_WithEof((IList<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>) all1);
            this.AbortWrites_AsPeerClose((IList<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>) all2);
            asyncResultNoResult?.SetAsCompleted((Exception) new ObjectDisposedException("Network"), false);
          }
          finally
          {
            this.DoOtherPreDestroy(disposing);
            this.DoPortDestroy(disposing);
          }
        }
      }
      finally
      {
        base.Dispose(disposing);
      }
      if (exToThrow != null)
        throw exToThrow;
    }

    protected abstract void RemovePortRecords();

    protected abstract void DoOtherPreDestroy(bool disposing);

    protected abstract void DoPortClose(bool disposing);

    protected abstract void DoPortDestroy(bool disposing);

    private void DisposeLinger(
      bool disposing,
      out Exception exToThrow,
      out bool dbgExpectEmptyWriteQueue)
    {
      if (disposing)
      {
        if (!this.LingerState.Enabled)
          throw new NotSupportedException("Background non-linger Close not supported.");
        if (this.LingerState.LingerTime == 0)
        {
          dbgExpectEmptyWriteQueue = true;
          exToThrow = (Exception) null;
        }
        else
        {
          lock (this._lockKey)
          {
            if (this.m_arWriteQueue.Count != 0)
              this.m_writeEmptied = new ManualResetEvent(false);
          }
          if (this.m_writeEmptied == null)
          {
            exToThrow = (Exception) null;
            dbgExpectEmptyWriteQueue = true;
          }
          else if (this.m_writeEmptied.WaitOne(this.LingerState.LingerTime * 1000, false))
          {
            exToThrow = (Exception) null;
            dbgExpectEmptyWriteQueue = true;
          }
          else
          {
            exToThrow = new Exception("Linger time-out FIXME");
            dbgExpectEmptyWriteQueue = false;
          }
        }
      }
      else
      {
        exToThrow = (Exception) null;
        dbgExpectEmptyWriteQueue = false;
      }
    }

    ~CommonRfcommStream() => this.Dispose(false);

    private static void MemoryBarrier() => Thread.MemoryBarrier();

    protected virtual void VerifyPortIsInRange(BluetoothEndPoint bep)
    {
      CommonRfcommStream.VerifyRfcommScnIsInRange(bep);
    }

    protected static void VerifyRfcommScnIsInRange(BluetoothEndPoint bep)
    {
      if (bep.Port < 1 || bep.Port > 30)
        throw new ArgumentOutOfRangeException(nameof (bep), "Channel Number must be in the range 1 to 30.");
    }

    protected static void VerifyL2CapPsmIsInRange(BluetoothEndPoint bep)
    {
      int port = (int) checked ((ushort) bep.Port);
    }

    protected internal IAsyncResult BeginConnect(
      BluetoothEndPoint bep,
      AsyncCallback asyncCallback,
      object state)
    {
      if (bep.Port == 0 || bep.Port == -1)
        throw new ArgumentException("Channel Number must be set in the BluetoothEndPoint, i.e. SDP lookup done.", nameof (bep));
      this.VerifyPortIsInRange(bep);
      int port = bep.Port;
      lock (this._lockKey)
      {
        if (this.m_state_ == CommonRfcommStream.State.Closed)
          throw new ObjectDisposedException("Network");
        if (this.m_state_ != CommonRfcommStream.State.New)
          throw new SocketException(10056);
        if (this.m_arConnect != null)
          throw new InvalidOperationException("Another Connect operation is already in progress.");
        AsyncResultNoResult asyncResultNoResult = new AsyncResultNoResult(asyncCallback, state);
        this.DoOtherSetup(bep, port);
        this.m_ocScn = port;
        this.m_addressToConnect = bep.Address;
        this.DoOpenClient(this.m_ocScn, this.m_addressToConnect);
        this.AddPortRecords();
        this.m_arConnect = asyncResultNoResult;
        return (IAsyncResult) asyncResultNoResult;
      }
    }

    protected abstract void DoOtherSetup(BluetoothEndPoint bep, int scn);

    protected abstract void AddPortRecords();

    protected abstract void DoOpenClient(int scn, BluetoothAddress addressToConnect);

    internal void EndConnect(IAsyncResult ar)
    {
      if (ar != this.m_arConnect)
      {
        if (ar != this.m_arConnectCompleted)
          throw new InvalidOperationException("Unknown IAsyncResult.");
      }
      try
      {
        ((AsyncResultNoResult) ar).EndInvoke();
      }
      finally
      {
        CommonRfcommStream.MemoryBarrier();
        this.m_arConnectCompleted = (AsyncResultNoResult) null;
      }
    }

    internal IAsyncResult BeginAccept(
      BluetoothEndPoint bep,
      string serviceName,
      AsyncCallback asyncCallback,
      object state)
    {
      if (bep == null)
        throw new ArgumentNullException(nameof (bep));
      if (bep.Port == 0 || bep.Port == -1)
        throw new ArgumentException("Channel Number must be set in the BluetoothEndPoint, i.e. SDP lookup done.", nameof (bep));
      this.VerifyPortIsInRange(bep);
      int port = bep.Port;
      lock (this._lockKey)
      {
        if (this.m_state_ == CommonRfcommStream.State.Closed)
          throw new ObjectDisposedException("Network");
        if (this.m_state_ != CommonRfcommStream.State.New)
          throw new SocketException(10056);
        if (this.m_arConnect != null)
          throw new InvalidOperationException("Another Connect operation is already in progress.");
        AsyncResultNoResult asyncResultNoResult = new AsyncResultNoResult(asyncCallback, state);
        this.DoOpenServer(port);
        this.AddPortRecords();
        this.m_arConnect = asyncResultNoResult;
        return (IAsyncResult) asyncResultNoResult;
      }
    }

    protected abstract void DoOpenServer(int scn);

    internal void EndAccept(IAsyncResult ar)
    {
      if (ar != this.m_arConnect)
      {
        if (ar != this.m_arConnectCompleted)
          throw new InvalidOperationException("Unknown IAsyncResult.");
      }
      try
      {
        ((AsyncResultNoResult) ar).EndInvoke();
      }
      finally
      {
        CommonRfcommStream.MemoryBarrier();
        this.m_arConnectCompleted = (AsyncResultNoResult) null;
      }
    }

    internal BluetoothAddress RemoteAddress => this.m_remoteAddress;

    public override bool CanRead => this.Connected;

    public override bool CanWrite => this.Connected;

    public override bool CanSeek => false;

    protected void HandleCONNECTED(string eventIdToString)
    {
      AsyncResultNoResult asyncResultNoResult;
      lock (this._lockKey)
      {
        if (this.m_arConnect != null && !this.m_arConnect.IsCompleted)
        {
          this.m_state_ = CommonRfcommStream.State.Connected;
          this.m_connected = true;
          asyncResultNoResult = this.m_arConnect;
          this.m_arConnectCompleted = this.m_arConnect;
          this.m_arConnect = (AsyncResultNoResult) null;
          try
          {
            this.DoIsConnected(out this.m_remoteAddress);
          }
          catch (Exception ex)
          {
          }
        }
        else
          asyncResultNoResult = (AsyncResultNoResult) null;
      }
      asyncResultNoResult?.SetAsCompleted((Exception) null, AsyncResultCompletion.MakeAsync);
    }

    protected virtual bool DoIsConnected(out BluetoothAddress p_remote_bdaddr)
    {
      p_remote_bdaddr = !(this.m_addressToConnect != (BluetoothAddress) null) ? BluetoothAddress.None : this.m_addressToConnect;
      return true;
    }

    protected void HandleCONNECT_ERR(string eventIdToString, int? socketErrorCode)
    {
      AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] allRead = (AsyncResult<int, CommonRfcommStream.BeginReadParameters>[]) null;
      AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] allWrite = (AsyncNoResult<CommonRfcommStream.BeginReadParameters>[]) null;
      AsyncResultNoResult asyncResultNoResult;
      Exception exception;
      lock (this._lockKey)
      {
        if (this.m_arConnect != null)
        {
          MiscUtils.Trace_WriteLine("HandlePortEvent: connect failed.");
          Exception err;
          if (this.TryBondingIf_inLock(this.m_addressToConnect, this.m_ocScn, out err))
          {
            asyncResultNoResult = (AsyncResultNoResult) null;
            exception = (Exception) null;
          }
          else if (err != null)
          {
            asyncResultNoResult = this.m_arConnect;
            exception = err;
            this.m_arConnectCompleted = this.m_arConnect;
            this.m_arConnect = (AsyncResultNoResult) null;
          }
          else
          {
            asyncResultNoResult = this.m_arConnect;
            exception = FooSocketExceptions.CreateConnectFailed("PortCONNECT_ERR=" + eventIdToString, socketErrorCode);
            this.m_arConnectCompleted = this.m_arConnect;
            this.m_arConnect = (AsyncResultNoResult) null;
          }
        }
        else if (this.m_state_ == CommonRfcommStream.State.Closed)
        {
          asyncResultNoResult = (AsyncResultNoResult) null;
          exception = (Exception) null;
        }
        else
        {
          MiscUtils.Trace_WriteLine("HandlePortEvent: closed when open.");
          this.CloseInternal(out allRead, out allWrite);
          asyncResultNoResult = (AsyncResultNoResult) null;
          exception = (Exception) null;
        }
      }
      asyncResultNoResult?.SetAsCompleted(exception, AsyncResultCompletion.MakeAsync);
      this.AbortIf_withPeerCloseAndEof((IList<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>) allRead, (IList<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>) allWrite);
    }

    private void CloseInternal(
      out AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] allRead,
      out AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] allWrite)
    {
      this.RemovePortRecords();
      if (this.m_state_ != CommonRfcommStream.State.PeerDidClose)
        this.DoPortClose(false);
      this.m_state_ = CommonRfcommStream.State.PeerDidClose;
      if (this.GetPendingReceiveDataLength() == 0)
        this.ClearAllReadRequests_inLock(out allRead);
      else
        allRead = (AsyncResult<int, CommonRfcommStream.BeginReadParameters>[]) null;
      this.ClearAllWriteRequests_inLock(out allWrite);
    }

    internal void CloseInternalAndAbort_willLock()
    {
      AsyncResultNoResult asyncResultNoResult = (AsyncResultNoResult) null;
      AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] allRead;
      AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] allWrite;
      lock (this._lockKey)
      {
        this.CloseInternal(out allRead, out allWrite);
        if (this.m_arConnect != null)
        {
          asyncResultNoResult = this.m_arConnect;
          this.m_arConnectCompleted = this.m_arConnect;
          this.m_arConnect = (AsyncResultNoResult) null;
        }
      }
      this.AbortIf_withPeerCloseAndEof((IList<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>) allRead, (IList<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>) allWrite);
      if (asyncResultNoResult == null)
        return;
      try
      {
        asyncResultNoResult.SetAsCompleted((Exception) new SocketException(10050), AsyncResultCompletion.MakeAsync);
      }
      catch (Exception ex)
      {
        MiscUtils.Trace_WriteLine("Port CloseInternal, SaC SocketError_NetworkDown ex: " + (object) ex);
      }
    }

    protected abstract bool TryBondingIf_inLock(
      BluetoothAddress addressToConnect,
      int ocScn,
      out Exception err);

    private void ClearAllReadRequests_inLock(
      out AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] all)
    {
      all = this.m_arReceiveList.ToArray();
      this.m_arReceiveList.Clear();
    }

    private void AbortReads_WithEof(
      IList<AsyncResult<int, CommonRfcommStream.BeginReadParameters>> all)
    {
      foreach (AsyncResult<int, CommonRfcommStream.BeginReadParameters> asyncResult in (IEnumerable<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>) all)
      {
        try
        {
          if (!asyncResult.IsCompleted)
            asyncResult.SetAsCompleted(0, AsyncResultCompletion.MakeAsync);
        }
        catch
        {
        }
      }
    }

    private void ClearAllRequests_inLock(
      out AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] allRead,
      out AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] allWrite)
    {
      this.ClearAllReadRequests_inLock(out allRead);
      this.ClearAllWriteRequests_inLock(out allWrite);
    }

    private void AbortIf_withPeerCloseAndEof(
      IList<AsyncResult<int, CommonRfcommStream.BeginReadParameters>> allRead,
      IList<AsyncNoResult<CommonRfcommStream.BeginReadParameters>> allWrite)
    {
      if (allRead != null)
        this.AbortReads_WithEof(allRead);
      if (allWrite == null)
        return;
      this.AbortWrites_AsPeerClose(allWrite);
    }

    protected void HandlePortReceive(byte[] buffer)
    {
      lock (this._lockKey)
      {
        this.m_receivedData.Enqueue(buffer);
        this.m_amountInReadBuffers += buffer.Length;
      }
      while (true)
      {
        AsyncResult<int, CommonRfcommStream.BeginReadParameters> asyncResult;
        int result;
        lock (this._lockKey)
        {
          if (this.m_arReceiveList.Count == 0 || this.GetPendingReceiveDataLength() == 0)
            break;
          asyncResult = this.m_arReceiveList.Dequeue();
          CommonRfcommStream.BeginReadParameters beginParameters = asyncResult.BeginParameters;
          result = this.ReturnSomeReceivedData_MustBeInLock(beginParameters.buffer, beginParameters.offset, beginParameters.count);
        }
        asyncResult.SetAsCompleted(result, AsyncResultCompletion.MakeAsync);
      }
    }

    private int GetPendingReceiveDataLength()
    {
      if (this.m_receivedData.Count == 0)
        return 0;
      int length;
      this.FirstReceivedBlockInfo_(out length, out int _);
      return length;
    }

    private int ReturnSomeReceivedData_MustBeInLock(byte[] buffer, int offset, int count)
    {
      int length1;
      int offset1;
      this.FirstReceivedBlockInfo_(out length1, out offset1);
      int num;
      if (length1 <= count)
      {
        Array.Copy((Array) this.m_receivedData.Dequeue(), this.m_receivedDataFirstBlockOffset, (Array) buffer, offset, length1);
        this.m_receivedDataFirstBlockOffset = 0;
        num = length1;
      }
      else
      {
        byte[] sourceArray = this.m_receivedData.Peek();
        int length2 = Math.Min(count, length1);
        Array.Copy((Array) sourceArray, offset1, (Array) buffer, offset, length2);
        this.m_receivedDataFirstBlockOffset += length2;
        num = length2;
      }
      this.m_amountInReadBuffers -= num;
      return num;
    }

    private void FirstReceivedBlockInfo_(out int length, out int offset)
    {
      byte[] numArray = this.m_receivedData.Peek();
      offset = this.m_receivedDataFirstBlockOffset;
      length = numArray.Length - offset;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      this.EnsureOpenForRead();
      IAsyncResult asyncResult;
      lock (this._lockKey)
      {
        if (this.m_receivedData.Count != 0)
          return this.ReturnSomeReceivedData_MustBeInLock(buffer, offset, count);
        if (this.m_state_ == CommonRfcommStream.State.PeerDidClose)
          return 0;
        asyncResult = this.BeginRead(buffer, offset, count, (AsyncCallback) null, (object) null);
      }
      if (!CommonRfcommStream.IsInfiniteTimeout(this._readTimeout))
        this.ApplyTimeout<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>(asyncResult, this._readTimeout, this.m_arReceiveList);
      return this.EndRead(asyncResult);
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      this.EnsureOpenForRead();
      AsyncResult<int, CommonRfcommStream.BeginReadParameters> asyncResult1;
      int result;
      lock (this._lockKey)
      {
        CommonRfcommStream.BeginReadParameters args = new CommonRfcommStream.BeginReadParameters(buffer, offset, count);
        AsyncResult<int, CommonRfcommStream.BeginReadParameters> asyncResult2 = new AsyncResult<int, CommonRfcommStream.BeginReadParameters>(callback, state, args);
        if (this.m_receivedData.Count == 0)
        {
          if (this.m_state_ == CommonRfcommStream.State.Connected)
          {
            this.m_arReceiveList.Enqueue(asyncResult2);
            return (IAsyncResult) asyncResult2;
          }
          asyncResult1 = asyncResult2;
          result = 0;
        }
        else
        {
          result = this.ReturnSomeReceivedData_MustBeInLock(args.buffer, args.offset, args.count);
          asyncResult1 = asyncResult2;
        }
      }
      asyncResult1.SetAsCompleted(result, true);
      return (IAsyncResult) asyncResult1;
    }

    public override int EndRead(IAsyncResult asyncResult)
    {
      return ((AsyncResult<int>) asyncResult).EndInvoke();
    }

    public virtual bool DataAvailable => this.AmountInReadBuffers > 0;

    internal int AmountInReadBuffers
    {
      get
      {
        lock (this._lockKey)
          return this.m_amountInReadBuffers;
      }
    }

    public override bool CanTimeout => true;

    public override int ReadTimeout
    {
      get => this._readTimeout;
      set => this._readTimeout = value;
    }

    public override int WriteTimeout
    {
      get => this._writeTimeout;
      set => this._writeTimeout = value;
    }

    internal static bool IsInfiniteTimeout(int timeout) => timeout == -1 || timeout == 0;

    private void ApplyTimeout<TAsyncResult>(
      IAsyncResult ar,
      int timeout,
      Queue<TAsyncResult> queue)
      where TAsyncResult : AsyncResultNoResult
    {
      TAsyncResult asyncResult = default (TAsyncResult);
      AsyncResult<int, CommonRfcommStream.BeginReadParameters>[] allRead = (AsyncResult<int, CommonRfcommStream.BeginReadParameters>[]) null;
      AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] allWrite = (AsyncNoResult<CommonRfcommStream.BeginReadParameters>[]) null;
      int num = 0;
      while (!ar.AsyncWaitHandle.WaitOne(timeout, false))
      {
        lock (this._lockKey)
        {
          if (queue.Count == 0)
          {
            asyncResult = default (TAsyncResult);
            break;
          }
          if ((object) queue.Peek() == ar)
          {
            asyncResult = queue.Dequeue();
            this.CloseInternal(out allRead, out allWrite);
            break;
          }
          asyncResult = default (TAsyncResult);
        }
        ++num;
      }
      if ((object) asyncResult == null)
        return;
      Exception innerException = (Exception) new SocketException(10060);
      asyncResult.SetAsCompleted((Exception) new IOException(innerException.Message, innerException), true);
      this.AbortIf_withPeerCloseAndEof((IList<AsyncResult<int, CommonRfcommStream.BeginReadParameters>>) allRead, (IList<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>) allWrite);
    }

    protected void FreePendingWrites()
    {
      Queue<AsyncNoResult<CommonRfcommStream.BeginReadParameters>> asyncNoResultQueue = new Queue<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>();
      AsyncNoResult<CommonRfcommStream.BeginReadParameters> asyncNoResult1 = (AsyncNoResult<CommonRfcommStream.BeginReadParameters>) null;
      Exception exception = (Exception) null;
      lock (this._lockKey)
      {
        int num1 = 0;
        while (this.m_arWriteQueue.Count != 0)
        {
          ++num1;
          CommonRfcommStream.BeginReadParameters beginParameters = this.m_arWriteQueue.Peek().BeginParameters;
          int num2;
          try
          {
            num2 = this.PortWrite(beginParameters.buffer, beginParameters.offset, beginParameters.count);
          }
          catch (Exception ex)
          {
            TestUtilities.IsUnderTestHarness();
            this.ImplicitPeerClose();
            this.SetAsDisconnected();
            asyncNoResult1 = this.m_arWriteQueue.Dequeue();
            exception = ex;
            break;
          }
          if (num2 == beginParameters.count)
          {
            AsyncNoResult<CommonRfcommStream.BeginReadParameters> asyncNoResult2 = this.m_arWriteQueue.Dequeue();
            asyncNoResultQueue.Enqueue(asyncNoResult2);
          }
          else
          {
            beginParameters.count -= num2;
            beginParameters.offset += num2;
            break;
          }
        }
        if (this.m_arWriteQueue.Count == 0 && this.m_writeEmptied != null)
          this.m_writeEmptied.Set();
        if (num1 != 0)
          ;
      }
      while (asyncNoResultQueue.Count != 0)
        asyncNoResultQueue.Dequeue().SetAsCompleted((Exception) null, AsyncResultCompletion.MakeAsync);
      asyncNoResult1?.SetAsCompleted(exception, AsyncResultCompletion.MakeAsync);
    }

    private void ClearAllWriteRequests_inLock(
      out AsyncNoResult<CommonRfcommStream.BeginReadParameters>[] all)
    {
      all = this.m_arWriteQueue.ToArray();
      this.m_arWriteQueue.Clear();
    }

    private void AbortWrites_AsPeerClose(
      IList<AsyncNoResult<CommonRfcommStream.BeginReadParameters>> all)
    {
      foreach (AsyncResultNoResult asyncResultNoResult in (IEnumerable<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>) all)
        asyncResultNoResult.SetAsCompleted((Exception) new IOException("IOError on socket.", FooSocketExceptions.ConnectionIsPeerClosed()), AsyncResultCompletion.MakeAsync);
    }

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      this.EnsureOpenForWrite();
      AsyncNoResult<CommonRfcommStream.BeginReadParameters> asyncNoResult1;
      lock (this._lockKey)
      {
        if (this.m_arWriteQueue.Count == 0)
        {
          bool flag = false;
          int num;
          try
          {
            num = this.PortWrite(buffer, offset, count);
            flag = true;
          }
          finally
          {
            if (!flag)
            {
              TestUtilities.IsUnderTestHarness();
              this.ImplicitPeerClose();
              this.SetAsDisconnected();
            }
          }
          if (num == count)
          {
            asyncNoResult1 = new AsyncNoResult<CommonRfcommStream.BeginReadParameters>(callback, state, (CommonRfcommStream.BeginReadParameters) null);
          }
          else
          {
            CommonRfcommStream.BeginReadParameters args = new CommonRfcommStream.BeginReadParameters(buffer, offset + num, count - num);
            AsyncNoResult<CommonRfcommStream.BeginReadParameters> asyncNoResult2 = new AsyncNoResult<CommonRfcommStream.BeginReadParameters>(callback, state, args);
            this.m_arWriteQueue.Enqueue(asyncNoResult2);
            return (IAsyncResult) asyncNoResult2;
          }
        }
        else
        {
          AsyncNoResult<CommonRfcommStream.BeginReadParameters> asyncNoResult3 = new AsyncNoResult<CommonRfcommStream.BeginReadParameters>(callback, state, new CommonRfcommStream.BeginReadParameters(buffer, offset, count));
          this.m_arWriteQueue.Enqueue(asyncNoResult3);
          return (IAsyncResult) asyncNoResult3;
        }
      }
      asyncNoResult1.SetAsCompleted((Exception) null, true);
      return (IAsyncResult) asyncNoResult1;
    }

    public override void EndWrite(IAsyncResult asyncResult)
    {
      bool flag = false;
      try
      {
        ((AsyncResultNoResult) asyncResult).EndInvoke();
        flag = true;
      }
      finally
      {
        int num = flag ? 1 : 0;
      }
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      IAsyncResult asyncResult = this.BeginWrite(buffer, offset, count, (AsyncCallback) null, (object) null);
      if (!CommonRfcommStream.IsInfiniteTimeout(this._writeTimeout))
        this.ApplyTimeout<AsyncNoResult<CommonRfcommStream.BeginReadParameters>>(asyncResult, this._writeTimeout, this.m_arWriteQueue);
      this.EndWrite(asyncResult);
    }

    private int PortWrite(byte[] buffer, int offset, int count)
    {
      int num = 0;
      while (count > 0)
      {
        ushort lenAttemptToWrite;
        ushort lenWritten;
        this.PortWriteMax16kb(buffer, offset, count, out lenAttemptToWrite, out lenWritten);
        count -= (int) lenWritten;
        offset += (int) lenWritten;
        num += (int) lenWritten;
        if ((int) lenAttemptToWrite != (int) lenWritten)
          break;
      }
      return num;
    }

    private void PortWriteMax16kb(
      byte[] buffer,
      int offset,
      int count,
      out ushort lenAttemptToWrite,
      out ushort lenWritten)
    {
      lenAttemptToWrite = (ushort) Math.Min(count, (int) ushort.MaxValue);
      byte[] numArray;
      if (offset == 0 && (int) lenAttemptToWrite == count && buffer.Length == (int) lenAttemptToWrite)
      {
        numArray = buffer;
      }
      else
      {
        numArray = new byte[(int) lenAttemptToWrite];
        Array.Copy((Array) buffer, offset, (Array) numArray, 0, (int) lenAttemptToWrite);
      }
      this.DoWrite(numArray, lenAttemptToWrite, out lenWritten);
    }

    protected abstract void DoWrite(byte[] p_data, ushort len_to_write, out ushort p_len_written);

    public override void Flush()
    {
    }

    public override long Length => throw CommonRfcommStream.NewNotSupportedException();

    public override long Position
    {
      get => throw CommonRfcommStream.NewNotSupportedException();
      set => throw CommonRfcommStream.NewNotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw CommonRfcommStream.NewNotSupportedException();
    }

    public override void SetLength(long value)
    {
      throw CommonRfcommStream.NewNotSupportedException();
    }

    private static Exception NewNotSupportedException() => throw new NotSupportedException();

    protected enum State
    {
      New,
      Connected,
      PeerDidClose,
      Closed,
    }

    internal sealed class BeginReadParameters
    {
      public byte[] buffer;
      public int offset;
      public int count;

      public BeginReadParameters(byte[] buffer, int offset, int count)
      {
        this.buffer = buffer;
        this.offset = offset;
        this.count = count;
      }
    }
  }
}
