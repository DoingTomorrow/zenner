﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.ReflectionUtility
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using Microsoft.Synchronization.ClientServices.SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal class ReflectionUtility
  {
    private static object _lockObject = new object();
    private static Dictionary<string, IEnumerable<PropertyInfo>> _stringToPropInfoMapping = new Dictionary<string, IEnumerable<PropertyInfo>>();
    private static Dictionary<string, IEnumerable<PropertyInfo>> _stringToPKPropInfoMapping = new Dictionary<string, IEnumerable<PropertyInfo>>();
    private static Dictionary<string, ConstructorInfo> _stringToCtorInfoMapping = new Dictionary<string, ConstructorInfo>();

    public static IEnumerable<PropertyInfo> GetPropertyInfoMapping(Type type)
    {
      IEnumerable<PropertyInfo> source;
      if (!ReflectionUtility._stringToPropInfoMapping.TryGetValue(type.FullName, out source))
      {
        lock (ReflectionUtility._lockObject)
        {
          if (!ReflectionUtility._stringToPropInfoMapping.TryGetValue(type.FullName, out source))
          {
            source = (IEnumerable<PropertyInfo>) type.GetProperties();
            source = (IEnumerable<PropertyInfo>) source.Where<PropertyInfo>((Func<PropertyInfo, bool>) (e => !e.Name.Equals("ServiceMetadata", StringComparison.Ordinal) && e.GetMethod != (MethodInfo) null && e.SetMethod != (MethodInfo) null && e.DeclaringType == type)).ToArray<PropertyInfo>();
            ReflectionUtility._stringToPropInfoMapping[type.FullName] = source;
            PropertyInfo[] array = source.Where<PropertyInfo>((Func<PropertyInfo, bool>) (e => ((IEnumerable<object>) e.GetCustomAttributes(typeof (PrimaryKeyAttribute), true)).Any<object>())).ToArray<PropertyInfo>();
            ReflectionUtility._stringToPKPropInfoMapping[type.FullName] = array.Length != 0 ? (IEnumerable<PropertyInfo>) array : throw new InvalidOperationException(string.Format("Entity {0} does not have the any property marked with the [DataAnnotations.KeyAttribute]. or [SQLite.PrimaryKeyAttribute]", (object) type.Name));
            ConstructorInfo constructorInfo = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (e => !((IEnumerable<ParameterInfo>) e.GetParameters()).Any<ParameterInfo>()));
            ReflectionUtility._stringToCtorInfoMapping[type.FullName] = !(constructorInfo == (ConstructorInfo) null) ? constructorInfo : throw new InvalidOperationException(string.Format("Type {0} does not have a public parameterless constructor.", (object) type.FullName));
          }
        }
      }
      return source;
    }

    public static IEnumerable<PropertyInfo> GetPrimaryKeysPropertyInfoMapping(Type type)
    {
      IEnumerable<PropertyInfo> propertyInfoMapping;
      if (!ReflectionUtility._stringToPKPropInfoMapping.TryGetValue(type.FullName, out propertyInfoMapping))
      {
        ReflectionUtility.GetPropertyInfoMapping(type);
        ReflectionUtility._stringToPKPropInfoMapping.TryGetValue(type.FullName, out propertyInfoMapping);
      }
      return propertyInfoMapping;
    }

    public static string GetPrimaryKeyString(IOfflineEntity live)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      foreach (PropertyInfo propertyInfo in ReflectionUtility.GetPrimaryKeysPropertyInfoMapping(live.GetType()))
      {
        if (propertyInfo.PropertyType == FormatterConstants.GuidType)
          stringBuilder.AppendFormat("{0}{1}=guid'{2}'", (object) str, (object) propertyInfo.Name, propertyInfo.GetValue((object) live, (object[]) null));
        else if (propertyInfo.PropertyType == FormatterConstants.StringType)
          stringBuilder.AppendFormat("{0}{1}='{2}'", (object) str, (object) propertyInfo.Name, propertyInfo.GetValue((object) live, (object[]) null));
        else
          stringBuilder.AppendFormat("{0}{1}={2}", (object) str, (object) propertyInfo.Name, propertyInfo.GetValue((object) live, (object[]) null));
        if (string.IsNullOrEmpty(str))
          str = ", ";
      }
      return stringBuilder.ToString();
    }

    public static IOfflineEntity GetObjectForType(EntryInfoWrapper wrapper, Type[] knownTypes)
    {
      ConstructorInfo constructorInfo;
      Type type;
      if (!ReflectionUtility._stringToCtorInfoMapping.TryGetValue(wrapper.TypeName, out constructorInfo))
      {
        if (knownTypes == null)
          throw new InvalidOperationException(string.Format("Unable to find a matching type for entry '{0}' in the loaded assemblies. Specify the type name in the KnownTypes argument to the SyncReader instance.", (object) wrapper.TypeName));
        type = ((IEnumerable<Type>) knownTypes).FirstOrDefault<Type>((Func<Type, bool>) (e => e.FullName.Equals(wrapper.TypeName, StringComparison.CurrentCultureIgnoreCase)));
        if (type == (Type) null)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unable to find a matching type for entry '{0}' in list of KnownTypes.", (object) wrapper.TypeName));
        ReflectionUtility.GetPropertyInfoMapping(type);
        constructorInfo = ReflectionUtility._stringToCtorInfoMapping[wrapper.TypeName];
      }
      else
        type = constructorInfo.DeclaringType;
      object obj = constructorInfo.Invoke((object[]) null);
      if (!wrapper.IsTombstone)
      {
        foreach (PropertyInfo propertyInfo in ReflectionUtility.GetPropertyInfoMapping(type))
        {
          string str;
          if (wrapper.PropertyBag.TryGetValue(propertyInfo.Name, out str))
            propertyInfo.SetValue(obj, ReflectionUtility.GetValueFromType(propertyInfo.PropertyType, str), (object[]) null);
        }
      }
      IOfflineEntity objectForType = (IOfflineEntity) obj;
      objectForType.SetServiceMetadata(new OfflineEntityMetadata(wrapper.IsTombstone, wrapper.Id, wrapper.ETag, wrapper.EditUri));
      return objectForType;
    }

    private static object GetValueFromType(Type type, string value)
    {
      if (value == null)
      {
        if (type.IsGenericType() || !type.IsPrimitive())
          return (object) null;
        throw new InvalidOperationException("Error in deserializing type " + type.FullName);
      }
      if (type.IsGenericType() && type.GetGenericTypeDefinition() == FormatterConstants.NullableType)
        type = type.GetGenericArguments()[0];
      if (FormatterConstants.StringType.IsAssignableFrom(type))
        return (object) value;
      if (FormatterConstants.ByteArrayType.IsAssignableFrom(type))
        return (object) Convert.FromBase64String(value);
      if (FormatterConstants.GuidType.IsAssignableFrom(type))
        return (object) new Guid(value);
      if (FormatterConstants.DateTimeType.IsAssignableFrom(type) || FormatterConstants.DateTimeOffsetType.IsAssignableFrom(type) || FormatterConstants.TimeSpanType.IsAssignableFrom(type))
        return FormatterUtilities.ParseDateTimeFromString(value, type);
      return type.IsPrimitive() || FormatterConstants.DecimalType.IsAssignableFrom(type) || FormatterConstants.FloatType.IsAssignableFrom(type) ? Convert.ChangeType((object) value, type, (IFormatProvider) CultureInfo.InvariantCulture) : (object) value;
    }
  }
}
