// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.TypeMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public static class TypeMapping
  {
    public static string GetTypeString(Type type)
    {
      if (type.Assembly != typeof (string).Assembly)
        return type.AssemblyQualifiedName;
      return !type.IsGenericType ? type.Name : type.FullName;
    }
  }
}
