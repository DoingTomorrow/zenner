// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.TypeMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class TypeMethods
  {
    public static readonly MethodInfo StaticGetType = typeof (Type).GetMethod("GetType", new Type[3]
    {
      typeof (string),
      typeof (bool),
      typeof (bool)
    });
    public static readonly MethodInfo GetTypeFromHandle = typeof (Type).GetMethod(nameof (GetTypeFromHandle));
  }
}
