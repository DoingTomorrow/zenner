// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.NewInstanceExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class NewInstanceExpression : Expression
  {
    private readonly Type type;
    private readonly Type[] constructorArgs;
    private readonly Expression[] arguments;
    private ConstructorInfo constructor;

    public NewInstanceExpression(ConstructorInfo constructor, params Expression[] args)
    {
      this.constructor = constructor;
      this.arguments = args;
    }

    public NewInstanceExpression(Type target, Type[] constructor_args, params Expression[] args)
    {
      this.type = target;
      this.constructorArgs = constructor_args;
      this.arguments = args;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      foreach (Expression expression in this.arguments)
        expression.Emit(member, gen);
      if (this.constructor == null)
        this.constructor = this.type.GetConstructor(this.constructorArgs);
      if (this.constructor == null)
        throw new ProxyGenerationException("Could not find constructor matching specified arguments");
      gen.Emit(OpCodes.Newobj, this.constructor);
    }
  }
}
