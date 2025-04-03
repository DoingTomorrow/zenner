// Decompiled with JetBrains decompiler
// Type: CommunicationPort.ReceivedCountEventArgs
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using System;

#nullable disable
namespace CommunicationPort
{
  public class ReceivedCountEventArgs : EventArgs
  {
    public int Count { get; set; }

    public ReceivedCountEventArgs(int count) => this.Count = count;
  }
}
