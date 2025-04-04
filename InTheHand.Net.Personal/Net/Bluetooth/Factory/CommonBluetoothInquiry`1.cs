// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.CommonBluetoothInquiry`1
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public abstract class CommonBluetoothInquiry<TInquiryEventItemType>
  {
    private readonly object _lockInquiry = new object();
    private List<IBluetoothDeviceInfo> _inquiryDevices;
    private AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> _inquiryAr;
    private List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>> _arInquiryFollowers;
    private BluetoothClient.LiveDiscoveryCallback _liveDiscoHandler;
    private object _liveDiscoState;

    protected abstract IBluetoothDeviceInfo CreateDeviceInfo(TInquiryEventItemType item);

    protected virtual IBluetoothDeviceInfo CreateDeviceInfoFromManualNameLookup(
      IBluetoothDeviceInfo previous,
      string name)
    {
      throw new NotImplementedException();
    }

    public void HandleInquiryResultInd(TInquiryEventItemType item)
    {
      IBluetoothDeviceInfo bdi = this.CreateDeviceInfo(item);
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler;
      object liveDiscoState;
      lock (this._lockInquiry)
      {
        if (this._inquiryDevices == null)
          return;
        liveDiscoHandler = this._liveDiscoHandler;
        liveDiscoState = this._liveDiscoState;
        DateTime discoTime = this._inquiryAr.BeginParameters.discoTime;
        bdi.SetDiscoveryTime(discoTime);
        BluetoothDeviceInfo.AddUniqueDevice(this._inquiryDevices, bdi);
      }
      if (liveDiscoHandler == null)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.OnDeviceResponded(liveDiscoHandler, bdi, liveDiscoState);
      });
    }

    private void OnDeviceResponded(
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      IBluetoothDeviceInfo bdi,
      object liveDiscoState)
    {
      liveDiscoHandler(bdi, liveDiscoState);
    }

    public void HandleInquiryComplete(int? reportedNumResponses)
    {
      List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>> sacArFollowers = (List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>>) null;
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> ar;
      List<IBluetoothDeviceInfo> deviceList;
      lock (this._lockInquiry)
      {
        this.StopInquiry();
        ar = this._inquiryAr;
        deviceList = this._inquiryDevices;
        this._inquiryAr = (AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>) null;
        this._liveDiscoHandler = (BluetoothClient.LiveDiscoveryCallback) null;
        this._liveDiscoState = (object) null;
        if (this._arInquiryFollowers != null)
        {
          sacArFollowers = this._arInquiryFollowers;
          this._arInquiryFollowers = (List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>>) null;
        }
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        CommonBluetoothInquiry<TInquiryEventItemType>.OnInquiryComplete(ar, deviceList, sacArFollowers);
      });
    }

    protected virtual void StopInquiry()
    {
    }

    private static void OnInquiryComplete(
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> sacAr,
      List<IBluetoothDeviceInfo> sacResult,
      List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>> sacArFollowers)
    {
      if (sacAr == null)
        return;
      sacAr.SetAsCompleted(sacResult, false);
      if (sacArFollowers == null)
        return;
      foreach (AsyncResult<List<IBluetoothDeviceInfo>> sacArFollower in sacArFollowers)
        sacArFollower.SetAsCompleted(sacResult, false);
    }

    public IAsyncResult BeginInquiry(
      int maxDevices,
      TimeSpan inquiryLength,
      AsyncCallback asyncCallback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      ThreadStart startInquiry,
      DiscoDevsParams args)
    {
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> asyncResult = (AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>) null;
      List<IBluetoothDeviceInfo> result = (List<IBluetoothDeviceInfo>) null;
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> ar;
      lock (this._lockInquiry)
      {
        if (this._inquiryAr != null)
        {
          ar = new AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>(asyncCallback, state, args);
          if (this._inquiryAr.IsCompleted)
          {
            asyncResult = ar;
            result = this._inquiryDevices;
          }
          else
          {
            if (this._arInquiryFollowers == null)
              this._arInquiryFollowers = new List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>>();
            this._arInquiryFollowers.Add(ar);
          }
        }
        else
        {
          ar = new AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>(asyncCallback, state, args);
          this._inquiryAr = ar;
          this._arInquiryFollowers = (List<AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>>) null;
          this._inquiryDevices = new List<IBluetoothDeviceInfo>();
          this._liveDiscoHandler = liveDiscoHandler;
          this._liveDiscoState = liveDiscoState;
          bool flag = false;
          try
          {
            startInquiry();
            flag = true;
          }
          finally
          {
            if (!flag)
              this._inquiryAr = (AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>) null;
          }
          if (inquiryLength.CompareTo(TimeSpan.Zero) > 0)
          {
            TimeSpan inquiryLength1 = TimeSpan.FromMilliseconds(1.5 * inquiryLength.TotalMilliseconds);
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.InquiryTimeout_Runner), (object) new CommonBluetoothInquiry<TInquiryEventItemType>.InquiryTimeoutParams(ar, inquiryLength1));
          }
        }
      }
      asyncResult?.SetAsCompleted(result, true);
      return (IAsyncResult) ar;
    }

    public List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      return ((AsyncResult<List<IBluetoothDeviceInfo>>) ar).EndInvoke();
    }

    private void InquiryTimeout_Runner(object state)
    {
      CommonBluetoothInquiry<TInquiryEventItemType>.InquiryTimeoutParams inquiryTimeoutParams = (CommonBluetoothInquiry<TInquiryEventItemType>.InquiryTimeoutParams) state;
      if (inquiryTimeoutParams._ar.AsyncWaitHandle.WaitOne(inquiryTimeoutParams.InquiryLengthAsMiliseconds(), false))
        return;
      lock (this._lockInquiry)
      {
        if (inquiryTimeoutParams._ar.IsCompleted)
          return;
        object inquiryAr = (object) this._inquiryAr;
        if (inquiryAr == null || inquiryAr != inquiryTimeoutParams._ar)
          return;
        MiscUtils.Trace_WriteLine("Cancelling Inquiry due to timeout.");
        this.HandleInquiryComplete(new int?());
      }
    }

    private struct InquiryTimeoutParams(
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> ar,
      TimeSpan inquiryLength)
    {
      internal readonly IAsyncResult _ar = (IAsyncResult) ar;
      internal readonly TimeSpan _InquiryLength = inquiryLength;

      internal int InquiryLengthAsMiliseconds()
      {
        return checked ((int) this._InquiryLength.TotalMilliseconds);
      }
    }
  }
}
