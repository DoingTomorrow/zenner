// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.ProtoDecoratorBase
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal abstract class ProtoDecoratorBase : IProtoSerializer
  {
    protected readonly IProtoSerializer Tail;

    public abstract Type ExpectedType { get; }

    protected ProtoDecoratorBase(IProtoSerializer tail) => this.Tail = tail;

    public abstract bool ReturnsValue { get; }

    public abstract bool RequiresOldValue { get; }

    public abstract void Write(object value, ProtoWriter dest);

    public abstract object Read(object value, ProtoReader source);

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      this.EmitWrite(ctx, valueFrom);
    }

    protected abstract void EmitWrite(CompilerContext ctx, Local valueFrom);

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      this.EmitRead(ctx, valueFrom);
    }

    protected abstract void EmitRead(CompilerContext ctx, Local valueFrom);
  }
}
