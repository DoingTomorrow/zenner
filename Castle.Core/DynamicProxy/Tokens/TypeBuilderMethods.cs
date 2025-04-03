// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.TypeBuilderMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class TypeBuilderMethods
  {
    public static readonly MethodInfo DefineProperty = typeof (TypeBuilder).GetMethod(nameof (DefineProperty), new Type[9]
    {
      typeof (string),
      typeof (PropertyAttributes),
      typeof (CallingConventions),
      typeof (Type),
      typeof (Type[]),
      typeof (Type[]),
      typeof (Type[]),
      typeof (Type[][]),
      typeof (Type[][])
    });
  }
}
