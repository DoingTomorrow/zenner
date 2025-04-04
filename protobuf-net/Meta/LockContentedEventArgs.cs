// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.LockContentedEventArgs
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;

#nullable disable
namespace ProtoBuf.Meta
{
  public sealed class LockContentedEventArgs : EventArgs
  {
    private readonly string ownerStackTrace;

    internal LockContentedEventArgs(string ownerStackTrace)
    {
      this.ownerStackTrace = ownerStackTrace;
    }

    public string OwnerStackTrace => this.ownerStackTrace;
  }
}
