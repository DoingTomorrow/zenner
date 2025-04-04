// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PropertyHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Internal
{
  internal static class PropertyHelper
  {
    private static Dictionary<Type, Dictionary<string, PropertyInfo>> parameterInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

    internal static void SetPropertyFromString(
      object obj,
      string propertyName,
      string value,
      ConfigurationItemFactory configurationItemFactory)
    {
      InternalLogger.Debug<string, string, string>("Setting '{0}.{1}' to '{2}'", obj.GetType().Name, propertyName, value);
      PropertyInfo result;
      if (!PropertyHelper.TryGetPropertyInfo(obj, propertyName, out result))
        throw new NotSupportedException(string.Format("Parameter {0} not supported on {1}", (object) propertyName, (object) obj.GetType().Name));
      try
      {
        Type nullableType = !result.IsDefined(typeof (ArrayParameterAttribute), false) ? result.PropertyType : throw new NotSupportedException(string.Format("Parameter {0} of {1} is an array and cannot be assigned a scalar value.", (object) propertyName, (object) obj.GetType().Name));
        Type type1 = Nullable.GetUnderlyingType(nullableType);
        if ((object) type1 == null)
          type1 = nullableType;
        Type type2 = type1;
        object obj1;
        if (!PropertyHelper.TryNLogSpecificConversion(type2, value, out obj1, configurationItemFactory) && !PropertyHelper.TryGetEnumValue(type2, value, out obj1, true) && !PropertyHelper.TrySpecialConversion(type2, value, out obj1) && !PropertyHelper.TryImplicitConversion(type2, value, out obj1) && !PropertyHelper.TryFlatListConversion(type2, value, out obj1) && !PropertyHelper.TryTypeConverterConversion(type2, value, out obj1))
          obj1 = Convert.ChangeType((object) value, type2, (IFormatProvider) CultureInfo.InvariantCulture);
        result.SetValue(obj, obj1, (object[]) null);
      }
      catch (TargetInvocationException ex)
      {
        throw new NLogConfigurationException(string.Format("Error when setting property '{0}' on {1}", (object) result.Name, obj), ex.InnerException);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Error when setting property '{0}' on '{1}'", (object) result.Name, obj);
        if (!ex.MustBeRethrownImmediately())
          throw new NLogConfigurationException(string.Format("Error when setting property '{0}' on {1}", (object) result.Name, obj), ex);
        throw;
      }
    }

    internal static bool IsArrayProperty(Type t, string propertyName)
    {
      PropertyInfo result;
      if (!PropertyHelper.TryGetPropertyInfo(t, propertyName, out result))
        throw new NotSupportedException(string.Format("Parameter {0} not supported on {1}", (object) propertyName, (object) t.Name));
      return result.IsDefined(typeof (ArrayParameterAttribute), false);
    }

    internal static bool TryGetPropertyInfo(
      object obj,
      string propertyName,
      out PropertyInfo result)
    {
      PropertyInfo property = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
      if (property != (PropertyInfo) null)
      {
        result = property;
        return true;
      }
      lock (PropertyHelper.parameterInfoCache)
      {
        Type type = obj.GetType();
        Dictionary<string, PropertyInfo> dictionary;
        if (!PropertyHelper.parameterInfoCache.TryGetValue(type, out dictionary))
        {
          dictionary = PropertyHelper.BuildPropertyInfoDictionary(type);
          PropertyHelper.parameterInfoCache[type] = dictionary;
        }
        return dictionary.TryGetValue(propertyName, out result);
      }
    }

    internal static Type GetArrayItemType(PropertyInfo propInfo)
    {
      return propInfo.GetCustomAttribute<ArrayParameterAttribute>()?.ItemType;
    }

    internal static PropertyInfo[] GetAllReadableProperties(Type type)
    {
      return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    }

    internal static void CheckRequiredParameters(object o)
    {
      foreach (PropertyInfo readableProperty in PropertyHelper.GetAllReadableProperties(o.GetType()))
      {
        if (readableProperty.IsDefined(typeof (RequiredParameterAttribute), false) && readableProperty.GetValue(o, (object[]) null) == null)
          throw new NLogConfigurationException(string.Format("Required parameter '{0}' on '{1}' was not specified.", (object) readableProperty.Name, o));
      }
    }

    private static bool TryImplicitConversion(Type resultType, string value, out object result)
    {
      try
      {
        if (Type.GetTypeCode(resultType) != TypeCode.Object)
        {
          result = (object) null;
          return false;
        }
        MethodInfo method = resultType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[1]
        {
          value.GetType()
        }, (ParameterModifier[]) null);
        if (method == (MethodInfo) null || !resultType.IsAssignableFrom(method.ReturnType))
        {
          result = (object) null;
          return false;
        }
        result = method.Invoke((object) null, new object[1]
        {
          (object) value
        });
        return true;
      }
      catch (Exception ex)
      {
        object[] objArray = new object[2]
        {
          (object) value,
          (object) resultType
        };
        InternalLogger.Warn(ex, "Implicit Conversion Failed of {0} to {1}", objArray);
      }
      result = (object) null;
      return false;
    }

    private static bool TryNLogSpecificConversion(
      Type propertyType,
      string value,
      out object newValue,
      ConfigurationItemFactory configurationItemFactory)
    {
      if (propertyType == typeof (Layout) || propertyType == typeof (SimpleLayout))
      {
        newValue = (object) new SimpleLayout(value, configurationItemFactory);
        return true;
      }
      if (propertyType == typeof (ConditionExpression))
      {
        newValue = (object) ConditionParser.ParseExpression(value, configurationItemFactory);
        return true;
      }
      newValue = (object) null;
      return false;
    }

    private static bool TryGetEnumValue(
      Type resultType,
      string value,
      out object result,
      bool flagsEnumAllowed)
    {
      if (!resultType.IsEnum())
      {
        result = (object) null;
        return false;
      }
      if (flagsEnumAllowed && resultType.IsDefined(typeof (FlagsAttribute), false))
      {
        ulong num = 0;
        string str1 = value;
        char[] chArray = new char[1]{ ',' };
        foreach (string str2 in str1.Split(chArray))
        {
          FieldInfo field = resultType.GetField(str2.Trim(), BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
          if (field == (FieldInfo) null)
            throw new NLogConfigurationException(string.Format("Invalid enumeration value '{0}'.", (object) value));
          num |= Convert.ToUInt64(field.GetValue((object) null), (IFormatProvider) CultureInfo.InvariantCulture);
        }
        result = Convert.ChangeType((object) num, Enum.GetUnderlyingType(resultType), (IFormatProvider) CultureInfo.InvariantCulture);
        result = Enum.ToObject(resultType, result);
        return true;
      }
      FieldInfo field1 = resultType.GetField(value, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      result = !(field1 == (FieldInfo) null) ? field1.GetValue((object) null) : throw new NLogConfigurationException(string.Format("Invalid enumeration value '{0}'.", (object) value));
      return true;
    }

    private static bool TrySpecialConversion(Type type, string value, out object newValue)
    {
      if (type == typeof (Encoding))
      {
        value = value.Trim();
        newValue = (object) Encoding.GetEncoding(value);
        return true;
      }
      if (type == typeof (CultureInfo))
      {
        value = value.Trim();
        newValue = (object) new CultureInfo(value);
        return true;
      }
      if (type == typeof (Type))
      {
        value = value.Trim();
        newValue = (object) Type.GetType(value, true);
        return true;
      }
      newValue = (object) null;
      return false;
    }

    private static bool TryFlatListConversion(Type type, string valueRaw, out object newValue)
    {
      if (type.IsGenericType())
      {
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        bool flag = genericTypeDefinition == typeof (ISet<>) || genericTypeDefinition == typeof (HashSet<>);
        if (flag || genericTypeDefinition == typeof (List<>) || genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (IEnumerable<>))
        {
          Type genericArgument = type.GetGenericArguments()[0];
          Type type1 = (flag ? typeof (HashSet<>) : typeof (List<>)).MakeGenericType(genericArgument);
          object instance = Activator.CreateInstance(type1);
          if (instance == null)
            throw new NLogConfigurationException("Cannot create instance of {0} for value {1}", new object[2]
            {
              (object) type.ToString(),
              (object) valueRaw
            });
          IEnumerable<string> strings = valueRaw.SplitQuoted(',', '\'', '\\');
          MethodInfo method = type1.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
          if (method == (MethodInfo) null)
            throw new NLogConfigurationException("Add method on type {0} for value {1} not found", new object[2]
            {
              (object) type.ToString(),
              (object) valueRaw
            });
          foreach (string str in strings)
          {
            if (!PropertyHelper.TryGetEnumValue(genericArgument, str, out newValue, false) && !PropertyHelper.TrySpecialConversion(genericArgument, str, out newValue) && !PropertyHelper.TryImplicitConversion(genericArgument, str, out newValue) && !PropertyHelper.TryTypeConverterConversion(genericArgument, str, out newValue))
              newValue = Convert.ChangeType((object) str, genericArgument, (IFormatProvider) CultureInfo.InvariantCulture);
            method.Invoke(instance, new object[1]
            {
              newValue
            });
          }
          newValue = instance;
          return true;
        }
      }
      newValue = (object) null;
      return false;
    }

    private static bool TryTypeConverterConversion(Type type, string value, out object newValue)
    {
      TypeConverter converter = TypeDescriptor.GetConverter(type);
      if (converter.CanConvertFrom(typeof (string)))
      {
        newValue = converter.ConvertFromInvariantString(value);
        return true;
      }
      newValue = (object) null;
      return false;
    }

    private static bool TryGetPropertyInfo(
      Type targetType,
      string propertyName,
      out PropertyInfo result)
    {
      if (!string.IsNullOrEmpty(propertyName))
      {
        PropertyInfo property = targetType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        if (property != (PropertyInfo) null)
        {
          result = property;
          return true;
        }
      }
      lock (PropertyHelper.parameterInfoCache)
      {
        Dictionary<string, PropertyInfo> dictionary;
        if (!PropertyHelper.parameterInfoCache.TryGetValue(targetType, out dictionary))
        {
          dictionary = PropertyHelper.BuildPropertyInfoDictionary(targetType);
          PropertyHelper.parameterInfoCache[targetType] = dictionary;
        }
        return dictionary.TryGetValue(propertyName, out result);
      }
    }

    private static Dictionary<string, PropertyInfo> BuildPropertyInfoDictionary(Type t)
    {
      Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (PropertyInfo readableProperty in PropertyHelper.GetAllReadableProperties(t))
      {
        ArrayParameterAttribute customAttribute = readableProperty.GetCustomAttribute<ArrayParameterAttribute>();
        if (customAttribute != null)
          dictionary[customAttribute.ElementName] = readableProperty;
        else
          dictionary[readableProperty.Name] = readableProperty;
        if (readableProperty.IsDefined(typeof (DefaultParameterAttribute), false))
          dictionary[string.Empty] = readableProperty;
      }
      return dictionary;
    }
  }
}
