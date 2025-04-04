// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ObjectGraphScanner
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NLog.Internal
{
  internal static class ObjectGraphScanner
  {
    public static List<T> FindReachableObjects<T>(
      bool aggressiveSearch,
      params object[] rootObjects)
      where T : class
    {
      if (InternalLogger.IsTraceEnabled)
        InternalLogger.Trace<Type>("FindReachableObject<{0}>:", typeof (T));
      List<T> objList = new List<T>();
      HashSet<object> visitedObjects = new HashSet<object>();
      foreach (object rootObject in rootObjects)
        ObjectGraphScanner.ScanProperties<T>(aggressiveSearch, objList, rootObject, 0, visitedObjects);
      return objList.ToList<T>();
    }

    private static void ScanProperties<T>(
      bool aggressiveSearch,
      List<T> result,
      object o,
      int level,
      HashSet<object> visitedObjects)
      where T : class
    {
      if (o == null)
        return;
      Type type = o.GetType();
      try
      {
        if (type == (Type) null)
          return;
        if (!type.IsDefined(typeof (NLogConfigurationItemAttribute), true))
          return;
      }
      catch (Exception ex)
      {
        object[] objArray = new object[2]
        {
          (object) new string(' ', level),
          (object) o.ToString()
        };
        InternalLogger.Info(ex, "{0}Type reflection not possible for: {1}. Maybe because of .NET Native.", objArray);
        return;
      }
      if (visitedObjects.Contains(o))
        return;
      visitedObjects.Add(o);
      if (InternalLogger.IsTraceEnabled)
        InternalLogger.Trace<string, string, object>("{0}Scanning {1} '{2}'", new string(' ', level), type.Name, o);
      if (o is T obj1)
      {
        result.Add(obj1);
        if (!aggressiveSearch)
          return;
      }
      foreach (PropertyInfo readableProperty in PropertyHelper.GetAllReadableProperties(type))
      {
        if (!(readableProperty == (PropertyInfo) null) && !(readableProperty.PropertyType == (Type) null) && !readableProperty.PropertyType.IsPrimitive() && !readableProperty.PropertyType.IsEnum())
        {
          if (!(readableProperty.PropertyType == typeof (string)))
          {
            try
            {
              if (readableProperty.IsDefined(typeof (NLogConfigurationIgnorePropertyAttribute), true))
                continue;
            }
            catch (Exception ex)
            {
              object[] objArray = new object[2]
              {
                (object) new string(' ', level + 1),
                (object) readableProperty.Name
              };
              InternalLogger.Info(ex, "{0}Type reflection not possible for property {1}. Maybe because of .NET Native.", objArray);
              continue;
            }
            object o1 = readableProperty.GetValue(o, (object[]) null);
            if (o1 != null)
            {
              if (InternalLogger.IsTraceEnabled)
                InternalLogger.Trace("{0}Scanning Property {1} '{2}' {3}", (object) new string(' ', level + 1), (object) readableProperty.Name, (object) o1.ToString(), (object) readableProperty.PropertyType.Namespace);
              switch (o1)
              {
                case IList list:
                  List<object> elements1;
                  lock (list.SyncRoot)
                  {
                    elements1 = new List<object>(list.Count);
                    for (int index = 0; index < list.Count; ++index)
                    {
                      object obj2 = list[index];
                      elements1.Add(obj2);
                    }
                  }
                  ObjectGraphScanner.ScanPropertiesList<T>(aggressiveSearch, result, (IEnumerable<object>) elements1, level + 1, visitedObjects);
                  continue;
                case IEnumerable source:
                  if (!(source is IList<object> objectList))
                    objectList = (IList<object>) source.Cast<object>().ToList<object>();
                  IList<object> elements2 = objectList;
                  ObjectGraphScanner.ScanPropertiesList<T>(aggressiveSearch, result, (IEnumerable<object>) elements2, level + 1, visitedObjects);
                  continue;
                default:
                  ObjectGraphScanner.ScanProperties<T>(aggressiveSearch, result, o1, level + 1, visitedObjects);
                  continue;
              }
            }
          }
        }
      }
    }

    private static void ScanPropertiesList<T>(
      bool aggressiveSearch,
      List<T> result,
      IEnumerable<object> elements,
      int level,
      HashSet<object> visitedObjects)
      where T : class
    {
      foreach (object element in elements)
        ObjectGraphScanner.ScanProperties<T>(aggressiveSearch, result, element, level, visitedObjects);
    }
  }
}
