// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CachedAttributeGetter`1
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  internal static class CachedAttributeGetter<T> where T : Attribute
  {
    private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));

    public static T GetAttribute(object type)
    {
      return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
    }
  }
}
