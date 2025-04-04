// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.UInt64Serializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class UInt64Serializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (ulong);

    public UInt64Serializer(TypeModel model)
    {
    }

    public Type ExpectedType => UInt64Serializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public object Read(object value, ProtoReader source) => (object) source.ReadUInt64();

    public void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteUInt64((ulong) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteUInt64", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadUInt64", this.ExpectedType);
    }
  }
}
