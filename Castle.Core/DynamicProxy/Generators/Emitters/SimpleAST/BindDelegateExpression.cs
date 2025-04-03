// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.BindDelegateExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class BindDelegateExpression : Expression
  {
    private readonly ConstructorInfo delegateCtor;
    private readonly Expression owner;
    private readonly MethodInfo methodToBindTo;

    public BindDelegateExpression(
      Type @delegate,
      Expression owner,
      MethodInfo methodToBindTo,
      GenericTypeParameterBuilder[] genericTypeParams)
    {
      this.delegateCtor = @delegate.GetConstructors()[0];
      this.methodToBindTo = methodToBindTo;
      if (@delegate.IsGenericTypeDefinition)
      {
        this.delegateCtor = TypeBuilder.GetConstructor(@delegate.MakeGenericType((Type[]) genericTypeParams), this.delegateCtor);
        this.methodToBindTo = methodToBindTo.MakeGenericMethod((Type[]) genericTypeParams);
      }
      this.owner = owner;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      this.owner.Emit(member, gen);
      gen.Emit(OpCodes.Dup);
      if (this.methodToBindTo.IsFinal)
        gen.Emit(OpCodes.Ldftn, this.methodToBindTo);
      else
        gen.Emit(OpCodes.Ldvirtftn, this.methodToBindTo);
      gen.Emit(OpCodes.Newobj, this.delegateCtor);
    }
  }
}
