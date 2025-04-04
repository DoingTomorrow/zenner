// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.Extensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace FluentNHibernate.Utils
{
  public static class Extensions
  {
    public static bool In<T>(this T instance, params T[] expected)
    {
      return !object.ReferenceEquals((object) instance, (object) null) && ((IEnumerable<T>) expected).Any<T>((Func<T, bool>) (x => instance.Equals((object) x)));
    }

    public static string ToLowerInvariantString(this object value)
    {
      return value.ToString().ToLowerInvariant();
    }

    public static bool Closes(this Type type, Type openGenericType)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
    }

    public static bool ClosesInterface(this Type type, Type openGenericInterface)
    {
      return ((IEnumerable<Type>) type.GetInterfaces()).Any<Type>((Func<Type, bool>) (x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericInterface));
    }

    public static bool IsEnum(this Type type)
    {
      return type.IsNullable() ? type.GetGenericArguments()[0].IsEnum : type.IsEnum;
    }

    public static bool IsNullable(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static T InstantiateUsingParameterlessConstructor<T>(this Type type)
    {
      return (T) type.InstantiateUsingParameterlessConstructor();
    }

    public static object InstantiateUsingParameterlessConstructor(this Type type)
    {
      return (ReflectHelper.GetDefaultConstructor(type) ?? throw new MissingConstructorException(type)).Invoke((object[]) null);
    }

    public static bool HasInterface(this Type type, Type interfaceType)
    {
      return ((IEnumerable<Type>) type.GetInterfaces()).Contains<Type>(interfaceType);
    }

    public static T DeepClone<T>(this T obj)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) serializationStream, (object) obj);
        serializationStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }
  }
}
