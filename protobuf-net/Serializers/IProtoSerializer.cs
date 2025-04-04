// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.IProtoSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal interface IProtoSerializer
  {
    Type ExpectedType { get; }

    void Write(object value, ProtoWriter dest);

    object Read(object value, ProtoReader source);

    bool RequiresOldValue { get; }

    bool ReturnsValue { get; }

    void EmitWrite(CompilerContext ctx, Local valueFrom);

    void EmitRead(CompilerContext ctx, Local entity);
  }
}
