// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.BooleanSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class BooleanSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (bool);

    public BooleanSerializer(TypeModel model)
    {
    }

    public Type ExpectedType => BooleanSerializer.expectedType;

    public void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteBoolean((bool) value, dest);
    }

    public object Read(object value, ProtoReader source) => (object) source.ReadBoolean();

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteBoolean", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadBoolean", this.ExpectedType);
    }
  }
}
