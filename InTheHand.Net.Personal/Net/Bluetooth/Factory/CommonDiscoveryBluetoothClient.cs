// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.CommonDiscoveryBluetoothClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public abstract class CommonDiscoveryBluetoothClient : IBluetoothClient, IDisposable
  {
    private TimeSpan _inquiryLength = TimeSpan.FromSeconds(12.0);

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
    }

    public IBluetoothDeviceInfo[] DiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly)
    {
      return this.EndDiscoverDevices(this.BeginDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, (AsyncCallback) null, (object) null));
    }

    public IAsyncResult BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state)
    {
      return this.BeginDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, callback, state, (BluetoothClient.LiveDiscoveryCallback) null, (object) null);
    }

    public IAsyncResult BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState)
    {
      DateTime utcNow = DateTime.UtcNow;
      DiscoDevsParams args = new DiscoDevsParams(maxDevices, authenticated, remembered, unknown, discoverableOnly, utcNow);
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> state1 = new AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>(callback, state, args);
      if (unknown || discoverableOnly)
        this.BeginInquiry(maxDevices, new AsyncCallback(this.DiscoDevs_InquiryCallback), (object) state1, liveDiscoHandler, liveDiscoState, args);
      else
        state1.SetAsCompleted((List<IBluetoothDeviceInfo>) null, true);
      return (IAsyncResult) state1;
    }

    protected abstract void BeginInquiry(
      int maxDevices,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args);

    private void DiscoDevs_InquiryCallback(IAsyncResult ar)
    {
      ((AsyncResult<List<IBluetoothDeviceInfo>>) ar.AsyncState).SetAsCompletedWithResultOf((Func<List<IBluetoothDeviceInfo>>) (() => this.EndInquiry(ar)), false);
    }

    protected abstract List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar);

    public IBluetoothDeviceInfo[] EndDiscoverDevices(IAsyncResult asyncResult)
    {
      AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams> asyncResult1 = (AsyncResult<List<IBluetoothDeviceInfo>, DiscoDevsParams>) asyncResult;
      List<IBluetoothDeviceInfo> discoverableDevices = asyncResult1.EndInvoke();
      DiscoDevsParams beginParameters = asyncResult1.BeginParameters;
      if (!beginParameters.unknown)
      {
        int num = beginParameters.discoverableOnly ? 1 : 0;
      }
      List<IBluetoothDeviceInfo> remoteDeviceEntries = this.GetKnownRemoteDeviceEntries();
      return BluetoothClient.DiscoverDevicesMerge(beginParameters.authenticated, beginParameters.remembered, beginParameters.unknown, remoteDeviceEntries, discoverableDevices, beginParameters.discoverableOnly, beginParameters.discoTime).ToArray();
    }

    protected abstract List<IBluetoothDeviceInfo> GetKnownRemoteDeviceEntries();

    public TimeSpan InquiryLength
    {
      get => this._inquiryLength;
      set
      {
        this._inquiryLength = value.TotalSeconds > 0.0 && value.TotalSeconds <= 60.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), "QueryLength must be a positive timespan between 0 and 60 seconds.");
      }
    }

    public int InquiryAccessCode
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    internal static void ConvertBthInquiryParams(
      int maxDevices,
      TimeSpan inquiryLength,
      out byte hciMaxResponses,
      out byte hciInquiryLength)
    {
      double num = inquiryLength.TotalSeconds / 1.28;
      hciInquiryLength = checked ((byte) num);
      if (hciInquiryLength < (byte) 1 || hciInquiryLength > (byte) 48)
        hciInquiryLength = (byte) 10;
      if (maxDevices < 0 || maxDevices > (int) byte.MaxValue)
        hciMaxResponses = (byte) 0;
      else
        hciMaxResponses = checked ((byte) maxDevices);
    }

    public abstract void Connect(BluetoothEndPoint remoteEP);

    public abstract IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state);

    public abstract void EndConnect(IAsyncResult asyncResult);

    public abstract NetworkStream GetStream();

    public abstract LingerOption LingerState { get; set; }

    public abstract bool Connected { get; }

    public abstract int Available { get; }

    public abstract Socket Client { get; set; }

    public abstract bool Authenticate { get; set; }

    public abstract bool Encrypt { get; set; }

    public abstract BluetoothEndPoint RemoteEndPoint { get; }

    public abstract string GetRemoteMachineName(BluetoothAddress device);

    public abstract Guid LinkKey { get; }

    public abstract LinkPolicy LinkPolicy { get; }

    public abstract string RemoteMachineName { get; }

    public abstract void SetPin(string pin);

    public abstract void SetPin(BluetoothAddress device, string pin);
  }
}
