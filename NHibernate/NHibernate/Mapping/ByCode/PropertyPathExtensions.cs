// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.PropertyPathExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class PropertyPathExtensions
  {
    public static Type GetContainerEntity(
      this PropertyPath propertyPath,
      IModelInspector domainInspector)
    {
      PropertyPath propertyPath1 = propertyPath;
      while (propertyPath1.PreviousPath != null && !domainInspector.IsEntity(propertyPath1.LocalMember.ReflectedType))
        propertyPath1 = propertyPath1.PreviousPath;
      return propertyPath1.LocalMember.ReflectedType;
    }

    public static string ToColumnName(this PropertyPath propertyPath, string pathSeparator)
    {
      return propertyPath.ToString().Replace(".", pathSeparator);
    }

    public static IEnumerable<PropertyPath> InverseProgressivePath(this PropertyPath source)
    {
      PropertyPath analizing = source != null ? source : throw new ArgumentNullException(nameof (source));
      List<MemberInfo> returnLocalMembers = new List<MemberInfo>(10);
      do
      {
        returnLocalMembers.Add(analizing.LocalMember);
        PropertyPath progressivePath = (PropertyPath) null;
        for (int index = returnLocalMembers.Count - 1; index >= 0; --index)
          progressivePath = new PropertyPath(progressivePath, returnLocalMembers[index]);
        yield return progressivePath;
        analizing = analizing.PreviousPath;
      }
      while (analizing != null);
    }

    public static PropertyPath DepureFirstLevelIfCollection(this PropertyPath source)
    {
      if (!source.GetRootMember().GetPropertyOrFieldType().IsGenericCollection())
        return source;
      PropertyPath[] array = source.InverseProgressivePath().ToArray<PropertyPath>();
      return array[array.Length - 2];
    }
  }
}
