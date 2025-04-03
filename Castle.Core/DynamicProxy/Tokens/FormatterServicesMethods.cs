// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.FormatterServicesMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class FormatterServicesMethods
  {
    public static readonly MethodInfo GetObjectData = typeof (FormatterServices).GetMethod(nameof (GetObjectData), new Type[2]
    {
      typeof (object),
      typeof (MemberInfo[])
    });
    public static readonly MethodInfo GetSerializableMembers = typeof (FormatterServices).GetMethod(nameof (GetSerializableMembers), new Type[1]
    {
      typeof (Type)
    });
  }
}
