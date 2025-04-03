// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.EnumerableToDictionaryMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Mappers
{
  public class EnumerableToDictionaryMapper : IObjectMapper
  {
    private static readonly Type KvpType = typeof (KeyValuePair<,>);

    public bool IsMatch(ResolutionContext context)
    {
      return context.DestinationType.IsDictionaryType() && context.SourceType.IsEnumerableType() && !context.SourceType.IsDictionaryType();
    }

    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      IEnumerable enumerable = (IEnumerable) context.SourceValue ?? (IEnumerable) new object[0];
      IEnumerable<object> objects = enumerable.Cast<object>();
      Type elementType = TypeHelper.GetElementType(context.SourceType, enumerable);
      Type dictionaryType = context.DestinationType.GetDictionaryType();
      Type genericArgument1 = dictionaryType.GetGenericArguments()[0];
      Type genericArgument2 = dictionaryType.GetGenericArguments()[1];
      Type destinationType = EnumerableToDictionaryMapper.KvpType.MakeGenericType(genericArgument1, genericArgument2);
      object dictionary = ObjectCreator.CreateDictionary(context.DestinationType, genericArgument1, genericArgument2);
      int arrayIndex = 0;
      foreach (object source in objects)
      {
        TypeMap typeMapFor = mapper.ConfigurationProvider.FindTypeMapFor(source, (object) null, elementType, destinationType);
        Type sourceElementType = typeMapFor != null ? typeMapFor.SourceType : elementType;
        Type destinationElementType = typeMapFor != null ? typeMapFor.DestinationType : destinationType;
        ResolutionContext elementContext = context.CreateElementContext(typeMapFor, source, sourceElementType, destinationElementType, arrayIndex);
        object obj1 = mapper.Map(elementContext);
        object obj2 = obj1.GetType().GetProperty("Key").GetValue(obj1, (object[]) null);
        object obj3 = obj1.GetType().GetProperty("Value").GetValue(obj1, (object[]) null);
        dictionaryType.GetMethod("Add").Invoke(dictionary, new object[2]
        {
          obj2,
          obj3
        });
        ++arrayIndex;
      }
      return dictionary;
    }
  }
}
