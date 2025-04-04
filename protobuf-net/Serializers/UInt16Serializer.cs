// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.UInt16Serializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal class UInt16Serializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (ushort);

    public UInt16Serializer(TypeModel model)
    {
    }

    public virtual Type ExpectedType => UInt16Serializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public virtual object Read(object value, ProtoReader source) => (object) source.ReadUInt16();

    public virtual void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteUInt16((ushort) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteUInt16", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadUInt16", ctx.MapType(typeof (ushort)));
    }
  }
}
