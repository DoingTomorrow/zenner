// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.CompiledSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class CompiledSerializer : IProtoTypeSerializer, IProtoSerializer
  {
    private readonly IProtoTypeSerializer head;
    private readonly ProtoSerializer serializer;
    private readonly ProtoDeserializer deserializer;

    bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
    {
      return this.head.HasCallbacks(callbackType);
    }

    bool IProtoTypeSerializer.CanCreateInstance() => this.head.CanCreateInstance();

    object IProtoTypeSerializer.CreateInstance(ProtoReader source)
    {
      return this.head.CreateInstance(source);
    }

    public void Callback(
      object value,
      TypeModel.CallbackType callbackType,
      SerializationContext context)
    {
      this.head.Callback(value, callbackType, context);
    }

    public static CompiledSerializer Wrap(IProtoTypeSerializer head, TypeModel model)
    {
      if (!(head is CompiledSerializer compiledSerializer))
        compiledSerializer = new CompiledSerializer(head, model);
      return compiledSerializer;
    }

    private CompiledSerializer(IProtoTypeSerializer head, TypeModel model)
    {
      this.head = head;
      this.serializer = CompilerContext.BuildSerializer((IProtoSerializer) head, model);
      this.deserializer = CompilerContext.BuildDeserializer((IProtoSerializer) head, model);
    }

    bool IProtoSerializer.RequiresOldValue => this.head.RequiresOldValue;

    bool IProtoSerializer.ReturnsValue => this.head.ReturnsValue;

    Type IProtoSerializer.ExpectedType => this.head.ExpectedType;

    void IProtoSerializer.Write(object value, ProtoWriter dest) => this.serializer(value, dest);

    object IProtoSerializer.Read(object value, ProtoReader source)
    {
      return this.deserializer(value, source);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      this.head.EmitWrite(ctx, valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      this.head.EmitRead(ctx, valueFrom);
    }

    void IProtoTypeSerializer.EmitCallback(
      CompilerContext ctx,
      Local valueFrom,
      TypeModel.CallbackType callbackType)
    {
      this.head.EmitCallback(ctx, valueFrom, callbackType);
    }

    void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
    {
      this.head.EmitCreateInstance(ctx);
    }
  }
}
