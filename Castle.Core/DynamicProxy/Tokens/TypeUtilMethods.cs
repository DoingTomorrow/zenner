// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.TypeUtilMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class TypeUtilMethods
  {
    public static readonly MethodInfo Sort = typeof (TypeUtil).GetMethod(nameof (Sort), BindingFlags.Static | BindingFlags.Public);
  }
}
