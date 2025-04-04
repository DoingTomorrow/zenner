// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoException
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf
{
  [Serializable]
  public class ProtoException : Exception
  {
    public ProtoException()
    {
    }

    public ProtoException(string message)
      : base(message)
    {
    }

    public ProtoException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ProtoException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
