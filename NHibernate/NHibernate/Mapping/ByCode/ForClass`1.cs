// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ForClass`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class ForClass<T>
  {
    private const BindingFlags DefaultFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public static FieldInfo Field(string fieldName)
    {
      return fieldName == null ? (FieldInfo) null : ForClass<T>.GetField(typeof (T), fieldName);
    }

    private static FieldInfo GetField(Type type, string fieldName)
    {
      return type == typeof (object) || type == null ? (FieldInfo) null : type.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? ForClass<T>.GetField(type.BaseType, fieldName);
    }
  }
}
