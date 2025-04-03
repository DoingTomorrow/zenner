// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.InheritanceInvocationTypeGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class InheritanceInvocationTypeGenerator(
    Type targetType,
    MetaMethod method,
    MethodInfo callback,
    IInvocationCreationContributor contributor) : InvocationTypeGenerator(targetType, method, callback, false, contributor)
  {
    public static readonly Type BaseType = typeof (InheritanceInvocation);

    protected override FieldReference GetTargetReference()
    {
      return new FieldReference(InvocationMethods.ProxyObject);
    }

    protected override Type GetBaseType() => InheritanceInvocationTypeGenerator.BaseType;

    protected override ArgumentReference[] GetBaseCtorArguments(
      Type targetFieldType,
      ProxyGenerationOptions proxyGenerationOptions,
      out ConstructorInfo baseConstructor)
    {
      if (proxyGenerationOptions.Selector == null)
      {
        baseConstructor = InvocationMethods.InheritanceInvocationConstructorNoSelector;
        return new ArgumentReference[5]
        {
          new ArgumentReference(typeof (Type)),
          new ArgumentReference(typeof (object)),
          new ArgumentReference(typeof (IInterceptor[])),
          new ArgumentReference(typeof (MethodInfo)),
          new ArgumentReference(typeof (object[]))
        };
      }
      baseConstructor = InvocationMethods.InheritanceInvocationConstructorWithSelector;
      return new ArgumentReference[7]
      {
        new ArgumentReference(typeof (Type)),
        new ArgumentReference(typeof (object)),
        new ArgumentReference(typeof (IInterceptor[])),
        new ArgumentReference(typeof (MethodInfo)),
        new ArgumentReference(typeof (object[])),
        new ArgumentReference(typeof (IInterceptorSelector)),
        new ArgumentReference(typeof (IInterceptor[]).MakeByRefType())
      };
    }
  }
}
