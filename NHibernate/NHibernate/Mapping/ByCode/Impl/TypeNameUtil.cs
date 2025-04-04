// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.TypeNameUtil
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public static class TypeNameUtil
  {
    public static string GetNhTypeName(this System.Type type)
    {
      IType type1 = TypeFactory.HeuristicType(type.AssemblyQualifiedName);
      return type1 == null ? type.FullName + ", " + type.Assembly.GetName().Name : type1.Name;
    }

    public static string GetShortClassName(this System.Type type, HbmMapping mapDoc)
    {
      if (type == null)
        return (string) null;
      if (mapDoc == null)
        return type.AssemblyQualifiedName;
      string name = type.Assembly.GetName().Name;
      string fullName = type.Assembly.FullName;
      string str1 = type.Namespace;
      string str2 = (string) null;
      if (!name.Equals(mapDoc.assembly) && !fullName.Equals(mapDoc.assembly))
        str2 = name;
      string str3 = (string) null;
      if (!str1.Equals(mapDoc.@namespace))
        str3 = str1;
      if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3))
        return type.AssemblyQualifiedName;
      if (!string.IsNullOrEmpty(str2) && string.IsNullOrEmpty(str3))
        return TypeNameUtil.GetTypeNameForMapping(type) + ", " + str2;
      return string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3) ? type.FullName : TypeNameUtil.GetTypeNameForMapping(type);
    }

    private static string GetTypeNameForMapping(System.Type type)
    {
      return type.IsGenericType || type.IsNested ? type.FullName : type.Name;
    }
  }
}
