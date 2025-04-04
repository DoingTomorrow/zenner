// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.DefaultValueDecorator
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class DefaultValueDecorator : ProtoDecoratorBase
  {
    private readonly object defaultValue;

    public override Type ExpectedType => this.Tail.ExpectedType;

    public override bool RequiresOldValue => this.Tail.RequiresOldValue;

    public override bool ReturnsValue => this.Tail.ReturnsValue;

    public DefaultValueDecorator(TypeModel model, object defaultValue, IProtoSerializer tail)
      : base(tail)
    {
      if (defaultValue == null)
        throw new ArgumentNullException(nameof (defaultValue));
      if (model.MapType(defaultValue.GetType()) != tail.ExpectedType)
        throw new ArgumentException("Default value is of incorrect type", nameof (defaultValue));
      this.defaultValue = defaultValue;
    }

    public override void Write(object value, ProtoWriter dest)
    {
      if (object.Equals(value, this.defaultValue))
        return;
      this.Tail.Write(value, dest);
    }

    public override object Read(object value, ProtoReader source) => this.Tail.Read(value, source);

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      CodeLabel label1 = ctx.DefineLabel();
      if (valueFrom == null)
      {
        ctx.CopyValue();
        CodeLabel label2 = ctx.DefineLabel();
        this.EmitBranchIfDefaultValue(ctx, label2);
        this.Tail.EmitWrite(ctx, (Local) null);
        ctx.Branch(label1, true);
        ctx.MarkLabel(label2);
        ctx.DiscardValue();
      }
      else
      {
        ctx.LoadValue(valueFrom);
        this.EmitBranchIfDefaultValue(ctx, label1);
        this.Tail.EmitWrite(ctx, valueFrom);
      }
      ctx.MarkLabel(label1);
    }

    private void EmitBeq(CompilerContext ctx, CodeLabel label, Type type)
    {
      switch (Helpers.GetTypeCode(type))
      {
        case ProtoTypeCode.Boolean:
        case ProtoTypeCode.Char:
        case ProtoTypeCode.SByte:
        case ProtoTypeCode.Byte:
        case ProtoTypeCode.Int16:
        case ProtoTypeCode.UInt16:
        case ProtoTypeCode.Int32:
        case ProtoTypeCode.UInt32:
        case ProtoTypeCode.Int64:
        case ProtoTypeCode.UInt64:
        case ProtoTypeCode.Single:
        case ProtoTypeCode.Double:
          ctx.BranchIfEqual(label, false);
          break;
        default:
          MethodInfo method = type.GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[2]
          {
            type,
            type
          }, (ParameterModifier[]) null);
          if (method == (MethodInfo) null || method.ReturnType != ctx.MapType(typeof (bool)))
            throw new InvalidOperationException("No suitable equality operator found for default-values of type: " + type.FullName);
          ctx.EmitCall(method);
          ctx.BranchIfTrue(label, false);
          break;
      }
    }

    private void EmitBranchIfDefaultValue(CompilerContext ctx, CodeLabel label)
    {
      Type expectedType = this.ExpectedType;
      switch (Helpers.GetTypeCode(expectedType))
      {
        case ProtoTypeCode.Boolean:
          if ((bool) this.defaultValue)
          {
            ctx.BranchIfTrue(label, false);
            break;
          }
          ctx.BranchIfFalse(label, false);
          break;
        case ProtoTypeCode.Char:
          if ((char) this.defaultValue == char.MinValue)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) (char) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.SByte:
          if ((sbyte) this.defaultValue == (sbyte) 0)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) (sbyte) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Byte:
          if ((byte) this.defaultValue == (byte) 0)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) (byte) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Int16:
          if ((short) this.defaultValue == (short) 0)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) (short) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.UInt16:
          if ((ushort) this.defaultValue == (ushort) 0)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) (ushort) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Int32:
          if ((int) this.defaultValue == 0)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.UInt32:
          if ((uint) this.defaultValue == 0U)
          {
            ctx.BranchIfFalse(label, false);
            break;
          }
          ctx.LoadValue((int) (uint) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Int64:
          ctx.LoadValue((long) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.UInt64:
          ctx.LoadValue((long) (ulong) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Single:
          ctx.LoadValue((float) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Double:
          ctx.LoadValue((double) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Decimal:
          Decimal defaultValue1 = (Decimal) this.defaultValue;
          ctx.LoadValue(defaultValue1);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.DateTime:
          ctx.LoadValue(((DateTime) this.defaultValue).ToBinary());
          ctx.EmitCall(ctx.MapType(typeof (DateTime)).GetMethod("FromBinary"));
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.String:
          ctx.LoadValue((string) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.TimeSpan:
          TimeSpan defaultValue2 = (TimeSpan) this.defaultValue;
          if (defaultValue2 == TimeSpan.Zero)
          {
            ctx.LoadValue(typeof (TimeSpan).GetField("Zero"));
          }
          else
          {
            ctx.LoadValue(defaultValue2.Ticks);
            ctx.EmitCall(ctx.MapType(typeof (TimeSpan)).GetMethod("FromTicks"));
          }
          this.EmitBeq(ctx, label, expectedType);
          break;
        case ProtoTypeCode.Guid:
          ctx.LoadValue((Guid) this.defaultValue);
          this.EmitBeq(ctx, label, expectedType);
          break;
        default:
          throw new NotSupportedException("Type cannot be represented as a default value: " + expectedType.FullName);
      }
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      this.Tail.EmitRead(ctx, valueFrom);
    }
  }
}
