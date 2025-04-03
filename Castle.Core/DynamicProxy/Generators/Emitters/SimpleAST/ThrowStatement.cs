// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ThrowStatement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ThrowStatement : Statement
  {
    private readonly Type exceptionType;
    private readonly string errorMessage;

    public ThrowStatement(Type exceptionType, string errorMessage)
    {
      this.exceptionType = exceptionType;
      this.errorMessage = errorMessage;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      new NewInstanceExpression(this.exceptionType.GetConstructor(new Type[1]
      {
        typeof (string)
      }), new Expression[1]
      {
        new ConstReference((object) this.errorMessage).ToExpression()
      }).Emit(member, gen);
      gen.Emit(OpCodes.Throw);
    }
  }
}
