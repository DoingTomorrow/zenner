// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.MethodBaseMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class MethodBaseMethods
  {
    public static readonly MethodInfo GetMethodFromHandle1 = typeof (MethodBase).GetMethod("GetMethodFromHandle", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[1]
    {
      typeof (RuntimeMethodHandle)
    }, (ParameterModifier[]) null);
    public static readonly MethodInfo GetMethodFromHandle2 = typeof (MethodBase).GetMethod("GetMethodFromHandle", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[2]
    {
      typeof (RuntimeMethodHandle),
      typeof (RuntimeTypeHandle)
    }, (ParameterModifier[]) null);
  }
}
