// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommDecoratorNetworkStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.IO;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommDecoratorNetworkStream : DecoratorNetworkStream
  {
    private readonly CommonRfcommStream m_childWrs;

    internal WidcommDecoratorNetworkStream(CommonRfcommStream childWrs)
      : base((Stream) childWrs)
    {
      this.m_childWrs = childWrs.Connected ? childWrs : throw new ArgumentException("Child stream must be connected.");
    }

    public override bool DataAvailable => this.m_childWrs.DataAvailable;
  }
}
