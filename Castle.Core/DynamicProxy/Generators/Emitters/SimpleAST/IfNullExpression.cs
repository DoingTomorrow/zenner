// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.IfNullExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class IfNullExpression : Expression
  {
    private readonly Expression ifNotNull;
    private readonly Expression ifNull;
    private readonly Reference reference;

    public IfNullExpression(Reference reference, Expression ifNull, Expression ifNotNull)
    {
      this.reference = reference;
      this.ifNull = ifNull;
      this.ifNotNull = ifNotNull;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference(this.reference, gen);
      Label label = gen.DefineLabel();
      gen.Emit(OpCodes.Brtrue_S, label);
      this.ifNull.Emit(member, gen);
      gen.MarkLabel(label);
      this.ifNotNull.Emit(member, gen);
    }
  }
}
