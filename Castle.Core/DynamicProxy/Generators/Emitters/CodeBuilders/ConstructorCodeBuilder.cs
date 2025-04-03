// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.CodeBuilders.ConstructorCodeBuilder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.CodeBuilders
{
  public class ConstructorCodeBuilder : AbstractCodeBuilder
  {
    private readonly Type baseType;

    public ConstructorCodeBuilder(Type baseType, ILGenerator generator)
      : base(generator)
    {
      this.baseType = baseType;
    }

    public void InvokeBaseConstructor()
    {
      Type type = this.baseType;
      if (type.ContainsGenericParameters)
        type = type.GetGenericTypeDefinition();
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      this.InvokeBaseConstructor(type.GetConstructor(bindingAttr, (Binder) null, new Type[0], (ParameterModifier[]) null));
    }

    public void InvokeBaseConstructor(ConstructorInfo constructor)
    {
      this.AddStatement((Statement) new ConstructorInvocationStatement(constructor, new Expression[0]));
    }

    public void InvokeBaseConstructor(
      ConstructorInfo constructor,
      params ArgumentReference[] arguments)
    {
      this.AddStatement((Statement) new ConstructorInvocationStatement(constructor, ArgumentsUtil.ConvertArgumentReferenceToExpression(arguments)));
    }
  }
}
