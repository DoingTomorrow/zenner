// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ReturnStatement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ReturnStatement : Statement
  {
    private readonly Reference reference;
    private readonly Expression expression;

    public ReturnStatement()
    {
    }

    public ReturnStatement(Reference reference) => this.reference = reference;

    public ReturnStatement(Expression expression) => this.expression = expression;

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      if (this.reference != null)
        ArgumentsUtil.EmitLoadOwnerAndReference(this.reference, gen);
      else if (this.expression != null)
        this.expression.Emit(member, gen);
      else if (member.ReturnType != typeof (void))
        OpCodeUtil.EmitLoadOpCodeForDefaultValueOfType(gen, member.ReturnType);
      gen.Emit(OpCodes.Ret);
    }
  }
}
