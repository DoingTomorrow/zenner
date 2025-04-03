// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.ObjectCreator
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper.Mappers
{
  public static class ObjectCreator
  {
    private static readonly IDelegateFactory DelegateFactory = PlatformAdapter.Resolve<IDelegateFactory>();

    public static Array CreateArray(Type elementType, int length)
    {
      return Array.CreateInstance(elementType, length);
    }

    public static IList CreateList(Type elementType)
    {
      return (IList) ObjectCreator.CreateObject(typeof (List<>).MakeGenericType(elementType));
    }

    public static object CreateDictionary(Type dictionaryType, Type keyType, Type valueType)
    {
      Type type;
      if (!dictionaryType.IsInterface)
        type = dictionaryType;
      else
        type = typeof (Dictionary<,>).MakeGenericType(keyType, valueType);
      return ObjectCreator.CreateObject(type);
    }

    public static object CreateDefaultValue(Type type)
    {
      return !type.IsValueType ? (object) null : ObjectCreator.CreateObject(type);
    }

    public static object CreateNonNullValue(Type type)
    {
      if (type.IsValueType)
        return ObjectCreator.CreateObject(type);
      return (object) type == (object) typeof (string) ? (object) string.Empty : Activator.CreateInstance(type);
    }

    public static object CreateObject(Type type)
    {
      return !type.IsArray ? ObjectCreator.DelegateFactory.CreateCtor(type)() : (object) ObjectCreator.CreateArray(type.GetElementType(), 0);
    }
  }
}
