// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.IBluetoothListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public interface IBluetoothListener
  {
    void Construct(Guid service);

    void Construct(BluetoothAddress localAddress, Guid service);

    void Construct(BluetoothEndPoint localEP);

    void Construct(Guid service, byte[] sdpRecord, int channelOffset);

    void Construct(
      BluetoothAddress localAddress,
      Guid service,
      byte[] sdpRecord,
      int channelOffset);

    void Construct(BluetoothEndPoint localEP, byte[] sdpRecord, int channelOffset);

    void Construct(Guid service, ServiceRecord sdpRecord);

    void Construct(BluetoothAddress localAddress, Guid service, ServiceRecord sdpRecord);

    void Construct(BluetoothEndPoint localEP, ServiceRecord sdpRecord);

    IBluetoothClient AcceptBluetoothClient();

    Socket AcceptSocket();

    bool Authenticate { get; set; }

    IAsyncResult BeginAcceptBluetoothClient(AsyncCallback callback, object state);

    IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state);

    bool Encrypt { get; set; }

    IBluetoothClient EndAcceptBluetoothClient(IAsyncResult asyncResult);

    Socket EndAcceptSocket(IAsyncResult asyncResult);

    BluetoothEndPoint LocalEndPoint { get; }

    bool Pending();

    Socket Server { get; }

    ServiceClass ServiceClass { get; set; }

    string ServiceName { get; set; }

    ServiceRecord ServiceRecord { get; }

    void SetPin(string pin);

    void SetPin(BluetoothAddress device, string pin);

    void Start(int backlog);

    void Start();

    void Stop();
  }
}
