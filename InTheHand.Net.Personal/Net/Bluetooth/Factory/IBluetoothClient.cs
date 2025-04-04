// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.IBluetoothClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public interface IBluetoothClient : IDisposable
  {
    void Connect(BluetoothEndPoint remoteEP);

    IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state);

    void EndConnect(IAsyncResult asyncResult);

    bool Connected { get; }

    Socket Client { get; set; }

    int Available { get; }

    bool Authenticate { get; set; }

    IBluetoothDeviceInfo[] DiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly);

    IAsyncResult BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state);

    IBluetoothDeviceInfo[] EndDiscoverDevices(IAsyncResult asyncResult);

    IAsyncResult BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback handler,
      object liveDiscoState);

    bool Encrypt { get; set; }

    string GetRemoteMachineName(BluetoothAddress a);

    NetworkStream GetStream();

    LingerOption LingerState { get; set; }

    TimeSpan InquiryLength { get; set; }

    int InquiryAccessCode { get; set; }

    Guid LinkKey { get; }

    LinkPolicy LinkPolicy { get; }

    BluetoothEndPoint RemoteEndPoint { get; }

    string RemoteMachineName { get; }

    void SetPin(string pin);

    void SetPin(BluetoothAddress device, string pin);
  }
}
