// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.TagDecorator
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class TagDecorator : ProtoDecoratorBase, IProtoTypeSerializer, IProtoSerializer
  {
    private readonly bool strict;
    private readonly int fieldNumber;
    private readonly WireType wireType;

    public bool HasCallbacks(TypeModel.CallbackType callbackType)
    {
      return this.Tail is IProtoTypeSerializer tail && tail.HasCallbacks(callbackType);
    }

    public bool CanCreateInstance()
    {
      return this.Tail is IProtoTypeSerializer tail && tail.CanCreateInstance();
    }

    public object CreateInstance(ProtoReader source)
    {
      return ((IProtoTypeSerializer) this.Tail).CreateInstance(source);
    }

    public void Callback(
      object value,
      TypeModel.CallbackType callbackType,
      SerializationContext context)
    {
      if (!(this.Tail is IProtoTypeSerializer tail))
        return;
      tail.Callback(value, callbackType, context);
    }

    public void EmitCallback(
      CompilerContext ctx,
      Local valueFrom,
      TypeModel.CallbackType callbackType)
    {
      ((IProtoTypeSerializer) this.Tail).EmitCallback(ctx, valueFrom, callbackType);
    }

    public void EmitCreateInstance(CompilerContext ctx)
    {
      ((IProtoTypeSerializer) this.Tail).EmitCreateInstance(ctx);
    }

    public override Type ExpectedType => this.Tail.ExpectedType;

    public TagDecorator(int fieldNumber, WireType wireType, bool strict, IProtoSerializer tail)
      : base(tail)
    {
      this.fieldNumber = fieldNumber;
      this.wireType = wireType;
      this.strict = strict;
    }

    public override bool RequiresOldValue => this.Tail.RequiresOldValue;

    public override bool ReturnsValue => this.Tail.ReturnsValue;

    private bool NeedsHint => (this.wireType & ~(WireType.StartGroup | WireType.EndGroup)) != 0;

    public override object Read(object value, ProtoReader source)
    {
      if (this.strict)
        source.Assert(this.wireType);
      else if (this.NeedsHint)
        source.Hint(this.wireType);
      return this.Tail.Read(value, source);
    }

    public override void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteFieldHeader(this.fieldNumber, this.wireType, dest);
      this.Tail.Write(value, dest);
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadValue(this.fieldNumber);
      ctx.LoadValue((int) this.wireType);
      ctx.LoadReaderWriter();
      ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("WriteFieldHeader"));
      this.Tail.EmitWrite(ctx, valueFrom);
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.strict || this.NeedsHint)
      {
        ctx.LoadReaderWriter();
        ctx.LoadValue((int) this.wireType);
        ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod(this.strict ? "Assert" : "Hint"));
      }
      this.Tail.EmitRead(ctx, valueFrom);
    }
  }
}
