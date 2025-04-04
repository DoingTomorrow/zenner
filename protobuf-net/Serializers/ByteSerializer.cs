// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.ByteSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class ByteSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (byte);

    public Type ExpectedType => ByteSerializer.expectedType;

    public ByteSerializer(TypeModel model)
    {
    }

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public void Write(object value, ProtoWriter dest) => ProtoWriter.WriteByte((byte) value, dest);

    public object Read(object value, ProtoReader source) => (object) source.ReadByte();

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteByte", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadByte", this.ExpectedType);
    }
  }
}
