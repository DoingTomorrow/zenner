// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.NullDecorator
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class NullDecorator : ProtoDecoratorBase
  {
    private readonly Type expectedType;
    public const int Tag = 1;

    public NullDecorator(TypeModel model, IProtoSerializer tail)
      : base(tail)
    {
      Type type = tail.ReturnsValue ? tail.ExpectedType : throw new NotSupportedException("NullDecorator only supports implementations that return values");
      if (Helpers.IsValueType(type))
        this.expectedType = model.MapType(typeof (Nullable<>)).MakeGenericType(type);
      else
        this.expectedType = type;
    }

    public override Type ExpectedType => this.expectedType;

    public override bool ReturnsValue => true;

    public override bool RequiresOldValue => true;

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      using (Local localWithValue = ctx.GetLocalWithValue(this.expectedType, valueFrom))
      {
        using (Local local1 = new Local(ctx, ctx.MapType(typeof (SubItemToken))))
        {
          using (Local local2 = new Local(ctx, ctx.MapType(typeof (int))))
          {
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("StartSubItem"));
            ctx.StoreValue(local1);
            CodeLabel label1 = ctx.DefineLabel();
            CodeLabel label2 = ctx.DefineLabel();
            CodeLabel label3 = ctx.DefineLabel();
            ctx.MarkLabel(label1);
            ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof (int)));
            ctx.CopyValue();
            ctx.StoreValue(local2);
            ctx.LoadValue(1);
            ctx.BranchIfEqual(label2, true);
            ctx.LoadValue(local2);
            ctx.LoadValue(1);
            ctx.BranchIfLess(label3, false);
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("SkipField"));
            ctx.Branch(label1, true);
            ctx.MarkLabel(label2);
            if (this.Tail.RequiresOldValue)
            {
              if (Helpers.IsValueType(this.expectedType))
              {
                ctx.LoadAddress(localWithValue, this.expectedType);
                ctx.EmitCall(this.expectedType.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
              }
              else
                ctx.LoadValue(localWithValue);
            }
            this.Tail.EmitRead(ctx, (Local) null);
            if (Helpers.IsValueType(this.expectedType))
              ctx.EmitCtor(this.expectedType, this.Tail.ExpectedType);
            ctx.StoreValue(localWithValue);
            ctx.Branch(label1, false);
            ctx.MarkLabel(label3);
            ctx.LoadValue(local1);
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("EndSubItem"));
            ctx.LoadValue(localWithValue);
          }
        }
      }
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      using (Local localWithValue = ctx.GetLocalWithValue(this.expectedType, valueFrom))
      {
        using (Local local = new Local(ctx, ctx.MapType(typeof (SubItemToken))))
        {
          ctx.LoadNullRef();
          ctx.LoadReaderWriter();
          ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("StartSubItem"));
          ctx.StoreValue(local);
          if (Helpers.IsValueType(this.expectedType))
          {
            ctx.LoadAddress(localWithValue, this.expectedType);
            ctx.LoadValue(this.expectedType.GetProperty("HasValue"));
          }
          else
            ctx.LoadValue(localWithValue);
          CodeLabel label = ctx.DefineLabel();
          ctx.BranchIfFalse(label, false);
          if (Helpers.IsValueType(this.expectedType))
          {
            ctx.LoadAddress(localWithValue, this.expectedType);
            ctx.EmitCall(this.expectedType.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
          }
          else
            ctx.LoadValue(localWithValue);
          this.Tail.EmitWrite(ctx, (Local) null);
          ctx.MarkLabel(label);
          ctx.LoadValue(local);
          ctx.LoadReaderWriter();
          ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("EndSubItem"));
        }
      }
    }

    public override object Read(object value, ProtoReader source)
    {
      SubItemToken token = ProtoReader.StartSubItem(source);
      int num;
      while ((num = source.ReadFieldHeader()) > 0)
      {
        if (num == 1)
          value = this.Tail.Read(value, source);
        else
          source.SkipField();
      }
      ProtoReader.EndSubItem(token, source);
      return value;
    }

    public override void Write(object value, ProtoWriter dest)
    {
      SubItemToken token = ProtoWriter.StartSubItem((object) null, dest);
      if (value != null)
        this.Tail.Write(value, dest);
      ProtoWriter writer = dest;
      ProtoWriter.EndSubItem(token, writer);
    }
  }
}
