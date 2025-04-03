// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.NullCoalescingOperatorExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class NullCoalescingOperatorExpression : Expression
  {
    private readonly Expression expression;
    private readonly Expression @default;

    public NullCoalescingOperatorExpression(Expression expression, Expression @default)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      if (@default == null)
        throw new ArgumentNullException(nameof (@default));
      this.expression = expression;
      this.@default = @default;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      this.expression.Emit(member, gen);
      gen.Emit(OpCodes.Dup);
      Label label = gen.DefineLabel();
      gen.Emit(OpCodes.Brtrue_S, label);
      gen.Emit(OpCodes.Pop);
      this.@default.Emit(member, gen);
      gen.MarkLabel(label);
    }
  }
}
