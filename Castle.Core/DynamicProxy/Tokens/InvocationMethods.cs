// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.InvocationMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class InvocationMethods
  {
    public static readonly FieldInfo Target = typeof (CompositionInvocation).GetField("target", BindingFlags.Instance | BindingFlags.NonPublic);
    public static readonly FieldInfo ProxyObject = typeof (AbstractInvocation).GetField("proxyObject", BindingFlags.Instance | BindingFlags.NonPublic);
    public static readonly MethodInfo GetArguments = typeof (AbstractInvocation).GetMethod("get_Arguments");
    public static readonly MethodInfo GetArgumentValue = typeof (AbstractInvocation).GetMethod(nameof (GetArgumentValue));
    public static readonly MethodInfo GetReturnValue = typeof (AbstractInvocation).GetMethod("get_ReturnValue");
    public static readonly MethodInfo ThrowOnNoTarget = typeof (AbstractInvocation).GetMethod(nameof (ThrowOnNoTarget), BindingFlags.Instance | BindingFlags.NonPublic);
    public static readonly MethodInfo SetArgumentValue = typeof (AbstractInvocation).GetMethod(nameof (SetArgumentValue));
    public static readonly MethodInfo SetGenericMethodArguments = typeof (AbstractInvocation).GetMethod(nameof (SetGenericMethodArguments), new Type[1]
    {
      typeof (Type[])
    });
    public static readonly MethodInfo SetReturnValue = typeof (AbstractInvocation).GetMethod("set_ReturnValue");
    public static readonly ConstructorInfo InheritanceInvocationConstructorNoSelector = typeof (InheritanceInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[5]
    {
      typeof (Type),
      typeof (object),
      typeof (IInterceptor[]),
      typeof (MethodInfo),
      typeof (object[])
    }, (ParameterModifier[]) null);
    public static readonly ConstructorInfo InheritanceInvocationConstructorWithSelector = typeof (InheritanceInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[7]
    {
      typeof (Type),
      typeof (object),
      typeof (IInterceptor[]),
      typeof (MethodInfo),
      typeof (object[]),
      typeof (IInterceptorSelector),
      typeof (IInterceptor[]).MakeByRefType()
    }, (ParameterModifier[]) null);
    public static readonly ConstructorInfo CompositionInvocationConstructorNoSelector = typeof (CompositionInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[5]
    {
      typeof (object),
      typeof (object),
      typeof (IInterceptor[]),
      typeof (MethodInfo),
      typeof (object[])
    }, (ParameterModifier[]) null);
    public static readonly ConstructorInfo CompositionInvocationConstructorWithSelector = typeof (CompositionInvocation).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[7]
    {
      typeof (object),
      typeof (object),
      typeof (IInterceptor[]),
      typeof (MethodInfo),
      typeof (object[]),
      typeof (IInterceptorSelector),
      typeof (IInterceptor[]).MakeByRefType()
    }, (ParameterModifier[]) null);
    public static readonly MethodInfo Proceed = typeof (AbstractInvocation).GetMethod(nameof (Proceed), BindingFlags.Instance | BindingFlags.Public);
    public static readonly MethodInfo EnsureValidTarget = typeof (CompositionInvocation).GetMethod(nameof (EnsureValidTarget), BindingFlags.Instance | BindingFlags.NonPublic);
  }
}
