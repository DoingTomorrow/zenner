// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.TimeSpanSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class TimeSpanSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (TimeSpan);

    public TimeSpanSerializer(TypeModel model)
    {
    }

    public Type ExpectedType => TimeSpanSerializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public object Read(object value, ProtoReader source)
    {
      return (object) BclHelpers.ReadTimeSpan(source);
    }

    public void Write(object value, ProtoWriter dest)
    {
      BclHelpers.WriteTimeSpan((TimeSpan) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), "WriteTimeSpan", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), "ReadTimeSpan", this.ExpectedType);
    }
  }
}
