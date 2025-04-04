// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.FieldDecorator
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class FieldDecorator : ProtoDecoratorBase
  {
    private readonly FieldInfo field;
    private readonly Type forType;

    public override Type ExpectedType => this.forType;

    public override bool RequiresOldValue => true;

    public override bool ReturnsValue => false;

    public FieldDecorator(Type forType, FieldInfo field, IProtoSerializer tail)
      : base(tail)
    {
      this.forType = forType;
      this.field = field;
    }

    public override void Write(object value, ProtoWriter dest)
    {
      value = this.field.GetValue(value);
      if (value == null)
        return;
      this.Tail.Write(value, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      object obj = this.Tail.Read(this.Tail.RequiresOldValue ? this.field.GetValue(value) : (object) null, source);
      if (obj != null)
        this.field.SetValue(value, obj);
      return (object) null;
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadAddress(valueFrom, this.ExpectedType);
      ctx.LoadValue(this.field);
      ctx.WriteNullCheckedTail(this.field.FieldType, this.Tail, (Local) null);
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
      {
        if (this.Tail.RequiresOldValue)
        {
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          ctx.LoadValue(this.field);
        }
        ctx.ReadNullCheckedTail(this.field.FieldType, this.Tail, (Local) null);
        if (!this.Tail.ReturnsValue)
          return;
        using (Local local = new Local(ctx, this.field.FieldType))
        {
          ctx.StoreValue(local);
          if (Helpers.IsValueType(this.field.FieldType))
          {
            ctx.LoadAddress(localWithValue, this.ExpectedType);
            ctx.LoadValue(local);
            ctx.StoreValue(this.field);
          }
          else
          {
            CodeLabel label = ctx.DefineLabel();
            ctx.LoadValue(local);
            ctx.BranchIfFalse(label, true);
            ctx.LoadAddress(localWithValue, this.ExpectedType);
            ctx.LoadValue(local);
            ctx.StoreValue(this.field);
            ctx.MarkLabel(label);
          }
        }
      }
    }
  }
}
