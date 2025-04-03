// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ConvertExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ConvertExpression : Expression
  {
    private readonly Expression right;
    private Type fromType;
    private Type target;

    public ConvertExpression(Type targetType, Expression right)
      : this(targetType, typeof (object), right)
    {
    }

    public ConvertExpression(Type targetType, Type fromType, Expression right)
    {
      this.target = targetType;
      this.fromType = fromType;
      this.right = right;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      this.right.Emit(member, gen);
      if (this.fromType == this.target)
        return;
      if (this.fromType.IsByRef)
        this.fromType = this.fromType.GetElementType();
      if (this.target.IsByRef)
        this.target = this.target.GetElementType();
      if (this.target.IsValueType)
      {
        if (this.fromType.IsValueType)
          throw new NotImplementedException("Cannot convert between distinct value types");
        if (LdindOpCodesDictionary.Instance[this.target] != LdindOpCodesDictionary.EmptyOpCode)
        {
          gen.Emit(OpCodes.Unbox, this.target);
          OpCodeUtil.EmitLoadIndirectOpCodeForType(gen, this.target);
        }
        else
          gen.Emit(OpCodes.Unbox_Any, this.target);
      }
      else if (this.fromType.IsValueType)
      {
        gen.Emit(OpCodes.Box, this.fromType);
        ConvertExpression.EmitCastIfNeeded(typeof (object), this.target, gen);
      }
      else
        ConvertExpression.EmitCastIfNeeded(this.fromType, this.target, gen);
    }

    private static void EmitCastIfNeeded(Type from, Type target, ILGenerator gen)
    {
      if (target.IsGenericParameter)
        gen.Emit(OpCodes.Unbox_Any, target);
      else if (from.IsGenericParameter)
        gen.Emit(OpCodes.Box, from);
      else if (target.IsGenericType && target != from)
      {
        gen.Emit(OpCodes.Castclass, target);
      }
      else
      {
        if (!target.IsSubclassOf(from))
          return;
        gen.Emit(OpCodes.Castclass, target);
      }
    }
  }
}
