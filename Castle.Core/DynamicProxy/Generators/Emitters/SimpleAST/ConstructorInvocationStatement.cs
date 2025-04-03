// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ConstructorInvocationStatement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ConstructorInvocationStatement : Statement
  {
    private readonly ConstructorInfo cmethod;
    private readonly Expression[] args;

    public ConstructorInvocationStatement(ConstructorInfo method, params Expression[] args)
    {
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      if (args == null)
        throw new ArgumentNullException(nameof (args));
      this.cmethod = method;
      this.args = args;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      gen.Emit(OpCodes.Ldarg_0);
      foreach (Expression expression in this.args)
        expression.Emit(member, gen);
      gen.Emit(OpCodes.Call, this.cmethod);
    }
  }
}
