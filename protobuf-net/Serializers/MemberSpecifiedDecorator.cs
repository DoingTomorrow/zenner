// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.MemberSpecifiedDecorator
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class MemberSpecifiedDecorator : ProtoDecoratorBase
  {
    private readonly MethodInfo getSpecified;
    private readonly MethodInfo setSpecified;

    public override Type ExpectedType => this.Tail.ExpectedType;

    public override bool RequiresOldValue => this.Tail.RequiresOldValue;

    public override bool ReturnsValue => this.Tail.ReturnsValue;

    public MemberSpecifiedDecorator(
      MethodInfo getSpecified,
      MethodInfo setSpecified,
      IProtoSerializer tail)
      : base(tail)
    {
      this.getSpecified = !(getSpecified == (MethodInfo) null) || !(setSpecified == (MethodInfo) null) ? getSpecified : throw new InvalidOperationException();
      this.setSpecified = setSpecified;
    }

    public override void Write(object value, ProtoWriter dest)
    {
      if (!(this.getSpecified == (MethodInfo) null) && !(bool) this.getSpecified.Invoke(value, (object[]) null))
        return;
      this.Tail.Write(value, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      object obj = this.Tail.Read(value, source);
      if (this.setSpecified != (MethodInfo) null)
        this.setSpecified.Invoke(value, new object[1]
        {
          (object) true
        });
      return obj;
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      if (this.getSpecified == (MethodInfo) null)
      {
        this.Tail.EmitWrite(ctx, valueFrom);
      }
      else
      {
        using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
        {
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          ctx.EmitCall(this.getSpecified);
          CodeLabel label = ctx.DefineLabel();
          ctx.BranchIfFalse(label, false);
          this.Tail.EmitWrite(ctx, localWithValue);
          ctx.MarkLabel(label);
        }
      }
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.setSpecified == (MethodInfo) null)
      {
        this.Tail.EmitRead(ctx, valueFrom);
      }
      else
      {
        using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
        {
          this.Tail.EmitRead(ctx, localWithValue);
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          ctx.LoadValue(1);
          ctx.EmitCall(this.setSpecified);
        }
      }
    }
  }
}
