// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.AssignArgumentStatement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class AssignArgumentStatement : Statement
  {
    private readonly ArgumentReference argument;
    private readonly Expression expression;

    public AssignArgumentStatement(ArgumentReference argument, Expression expression)
    {
      this.argument = argument;
      this.expression = expression;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference((Reference) this.argument, gen);
      this.expression.Emit(member, gen);
    }
  }
}
