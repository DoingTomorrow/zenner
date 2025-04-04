// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.L2CapClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class L2CapClient
  {
    private readonly IL2CapClient m_impl;

    internal L2CapClient(IL2CapClient impl) => this.m_impl = impl;

    internal L2CapClient(IBluetoothClient impl) => this.m_impl = (IL2CapClient) impl;

    public L2CapClient()
    {
      foreach (BluetoothFactory factory in (IEnumerable<BluetoothFactory>) BluetoothFactory.Factories)
      {
        try
        {
          this.m_impl = factory.DoGetL2CapClient();
          return;
        }
        catch (NotImplementedException ex)
        {
        }
      }
      throw new NotSupportedException(nameof (L2CapClient));
    }

    [DebuggerStepThrough]
    public void Close() => this.Dispose();

    [DebuggerStepThrough]
    public void Dispose() => this.m_impl.Dispose();

    public void Connect(BluetoothEndPoint remoteEP)
    {
      if (remoteEP == null)
        throw new ArgumentNullException(nameof (remoteEP));
      this.m_impl.Connect(remoteEP);
    }

    [DebuggerStepThrough]
    public IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      if (remoteEP == null)
        throw new ArgumentNullException(nameof (remoteEP));
      return this.m_impl.BeginConnect(remoteEP, requestCallback, state);
    }

    [DebuggerStepThrough]
    public void EndConnect(IAsyncResult asyncResult) => this.m_impl.EndConnect(asyncResult);

    [DebuggerStepThrough]
    public Stream GetStream() => (Stream) this.m_impl.GetStream();

    public BluetoothEndPoint RemoteEndPoint
    {
      [DebuggerStepThrough] get => this.m_impl.RemoteEndPoint;
    }

    public int GetMtu() => this.m_impl.GetMtu();
  }
}
