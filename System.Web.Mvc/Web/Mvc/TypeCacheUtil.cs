// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TypeCacheUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  internal static class TypeCacheUtil
  {
    private static IEnumerable<Type> FilterTypesInAssemblies(
      IBuildManager buildManager,
      Predicate<Type> predicate)
    {
      IEnumerable<Type> types1 = (IEnumerable<Type>) Type.EmptyTypes;
      foreach (Assembly referencedAssembly in (IEnumerable) buildManager.GetReferencedAssemblies())
      {
        Type[] types2;
        try
        {
          types2 = referencedAssembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
          types2 = ex.Types;
        }
        types1 = types1.Concat<Type>((IEnumerable<Type>) types2);
      }
      return types1.Where<Type>((Func<Type, bool>) (type => TypeCacheUtil.TypeIsPublicClass(type) && predicate(type)));
    }

    public static List<Type> GetFilteredTypesFromAssemblies(
      string cacheName,
      Predicate<Type> predicate,
      IBuildManager buildManager)
    {
      TypeCacheSerializer serializer = new TypeCacheSerializer();
      List<Type> typesFromAssemblies = TypeCacheUtil.ReadTypesFromCache(cacheName, predicate, buildManager, serializer);
      if (typesFromAssemblies != null)
        return typesFromAssemblies;
      List<Type> list = TypeCacheUtil.FilterTypesInAssemblies(buildManager, predicate).ToList<Type>();
      TypeCacheUtil.SaveTypesToCache(cacheName, (IList<Type>) list, buildManager, serializer);
      return list;
    }

    internal static List<Type> ReadTypesFromCache(
      string cacheName,
      Predicate<Type> predicate,
      IBuildManager buildManager,
      TypeCacheSerializer serializer)
    {
      try
      {
        Stream stream = buildManager.ReadCachedFile(cacheName);
        if (stream != null)
        {
          using (StreamReader input = new StreamReader(stream))
          {
            List<Type> source = serializer.DeserializeTypes((TextReader) input);
            if (source != null)
            {
              if (source.All<Type>((Func<Type, bool>) (type => TypeCacheUtil.TypeIsPublicClass(type) && predicate(type))))
                return source;
            }
          }
        }
      }
      catch
      {
      }
      return (List<Type>) null;
    }

    internal static void SaveTypesToCache(
      string cacheName,
      IList<Type> matchingTypes,
      IBuildManager buildManager,
      TypeCacheSerializer serializer)
    {
      try
      {
        Stream cachedFile = buildManager.CreateCachedFile(cacheName);
        if (cachedFile == null)
          return;
        using (StreamWriter output = new StreamWriter(cachedFile))
          serializer.SerializeTypes((IEnumerable<Type>) matchingTypes, (TextWriter) output);
      }
      catch
      {
      }
    }

    private static bool TypeIsPublicClass(Type type)
    {
      return type != (Type) null && type.IsPublic && type.IsClass && !type.IsAbstract;
    }
  }
}
