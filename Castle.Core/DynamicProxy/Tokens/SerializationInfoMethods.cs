// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Tokens.SerializationInfoMethods
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Tokens
{
  public static class SerializationInfoMethods
  {
    public static readonly MethodInfo AddValue_Bool = typeof (SerializationInfo).GetMethod("AddValue", new Type[2]
    {
      typeof (string),
      typeof (bool)
    });
    public static readonly MethodInfo AddValue_Int32 = typeof (SerializationInfo).GetMethod("AddValue", new Type[2]
    {
      typeof (string),
      typeof (int)
    });
    public static readonly MethodInfo AddValue_Object = typeof (SerializationInfo).GetMethod("AddValue", new Type[2]
    {
      typeof (string),
      typeof (object)
    });
    public static readonly MethodInfo GetValue = typeof (SerializationInfo).GetMethod(nameof (GetValue), new Type[2]
    {
      typeof (string),
      typeof (Type)
    });
    public static readonly MethodInfo SetType = typeof (SerializationInfo).GetMethod(nameof (SetType));
  }
}
