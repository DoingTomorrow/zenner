// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.AssignStatement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class AssignStatement : Statement
  {
    private readonly Reference target;
    private readonly Expression expression;

    public AssignStatement(Reference target, Expression expression)
    {
      this.target = target;
      this.expression = expression;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference(this.target.OwnerReference, gen);
      this.expression.Emit(member, gen);
      this.target.StoreReference(gen);
    }
  }
}
