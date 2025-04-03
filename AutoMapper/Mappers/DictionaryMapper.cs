// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.DictionaryMapper
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
  public class DictionaryMapper : IObjectMapper
  {
    private static readonly Type KvpType = typeof (KeyValuePair<,>);

    public bool IsMatch(ResolutionContext context)
    {
      return context.SourceType.IsDictionaryType() && context.DestinationType.IsDictionaryType();
    }

    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      if (context.IsSourceValueNull && mapper.ShouldMapSourceCollectionAsNull(context))
        return (object) null;
      IEnumerable<object> source1 = ((IEnumerable) context.SourceValue ?? (IEnumerable) new object[0]).Cast<object>();
      Type dictionaryType1 = context.SourceType.GetDictionaryType();
      Type genericArgument1 = dictionaryType1.GetGenericArguments()[0];
      Type genericArgument2 = dictionaryType1.GetGenericArguments()[1];
      Type sourceKvpType = DictionaryMapper.KvpType.MakeGenericType(genericArgument1, genericArgument2);
      Type dictionaryType2 = context.DestinationType.GetDictionaryType();
      Type genericArgument3 = dictionaryType2.GetGenericArguments()[0];
      Type genericArgument4 = dictionaryType2.GetGenericArguments()[1];
      IEnumerable<DictionaryEntry> source2 = source1.OfType<DictionaryEntry>();
      if (source2.Any<DictionaryEntry>())
        source1 = source2.Select<DictionaryEntry, object>((Func<DictionaryEntry, object>) (e => Activator.CreateInstance(sourceKvpType, e.Key, e.Value)));
      object dictionary = ObjectCreator.CreateDictionary(context.DestinationType, genericArgument3, genericArgument4);
      int arrayIndex = 0;
      foreach (object obj1 in source1)
      {
        object source3 = sourceKvpType.GetProperty("Key").GetValue(obj1, new object[0]);
        object source4 = sourceKvpType.GetProperty("Value").GetValue(obj1, new object[0]);
        TypeMap typeMapFor1 = mapper.ConfigurationProvider.FindTypeMapFor(source3, (object) null, genericArgument1, genericArgument3);
        TypeMap typeMapFor2 = mapper.ConfigurationProvider.FindTypeMapFor(source4, (object) null, genericArgument2, genericArgument4);
        ResolutionContext elementContext1 = context.CreateElementContext(typeMapFor1, source3, genericArgument1, genericArgument3, arrayIndex);
        ResolutionContext elementContext2 = context.CreateElementContext(typeMapFor2, source4, genericArgument2, genericArgument4, arrayIndex);
        object obj2 = mapper.Map(elementContext1);
        object obj3 = mapper.Map(elementContext2);
        dictionaryType2.GetMethod("Add").Invoke(dictionary, new object[2]
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
