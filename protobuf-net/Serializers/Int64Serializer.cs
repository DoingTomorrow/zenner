// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.Int64Serializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class Int64Serializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (long);

    public Int64Serializer(TypeModel model)
    {
    }

    public Type ExpectedType => Int64Serializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public object Read(object value, ProtoReader source) => (object) source.ReadInt64();

    public void Write(object value, ProtoWriter dest) => ProtoWriter.WriteInt64((long) value, dest);

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteInt64", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadInt64", this.ExpectedType);
    }
  }
}
