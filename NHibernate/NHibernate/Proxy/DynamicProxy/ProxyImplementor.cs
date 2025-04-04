// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.ProxyImplementor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  internal class ProxyImplementor
  {
    private const MethodAttributes InterceptorMethodsAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName;
    private FieldBuilder field;

    public FieldBuilder InterceptorField => this.field;

    public void ImplementProxy(TypeBuilder typeBuilder)
    {
      typeBuilder.AddInterfaceImplementation(typeof (IProxy));
      this.field = typeBuilder.DefineField("__interceptor", typeof (IInterceptor), FieldAttributes.Private);
      MethodBuilder methodInfoBody1 = typeBuilder.DefineMethod("get_Interceptor", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, CallingConventions.HasThis, typeof (IInterceptor), new Type[0]);
      methodInfoBody1.SetImplementationFlags(MethodImplAttributes.IL);
      ILGenerator ilGenerator1 = methodInfoBody1.GetILGenerator();
      ilGenerator1.Emit(OpCodes.Ldarg_0);
      ilGenerator1.Emit(OpCodes.Ldfld, (FieldInfo) this.field);
      ilGenerator1.Emit(OpCodes.Ret);
      MethodBuilder methodInfoBody2 = typeBuilder.DefineMethod("set_Interceptor", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, CallingConventions.HasThis, typeof (void), new Type[1]
      {
        typeof (IInterceptor)
      });
      methodInfoBody2.SetImplementationFlags(MethodImplAttributes.IL);
      ILGenerator ilGenerator2 = methodInfoBody2.GetILGenerator();
      ilGenerator2.Emit(OpCodes.Ldarg_0);
      ilGenerator2.Emit(OpCodes.Ldarg_1);
      ilGenerator2.Emit(OpCodes.Stfld, (FieldInfo) this.field);
      ilGenerator2.Emit(OpCodes.Ret);
      MethodInfo method1 = typeof (IProxy).GetMethod("set_Interceptor");
      MethodInfo method2 = typeof (IProxy).GetMethod("get_Interceptor");
      typeBuilder.DefineMethodOverride((MethodInfo) methodInfoBody2, method1);
      typeBuilder.DefineMethodOverride((MethodInfo) methodInfoBody1, method2);
    }
  }
}
