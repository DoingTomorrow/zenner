// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.CollectionMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper.Mappers
{
  public class CollectionMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      return ((IObjectMapper) Activator.CreateInstance(typeof (CollectionMapper.EnumerableMapper<,>).MakeGenericType(context.DestinationType, TypeHelper.GetElementType(context.DestinationType)))).Map(context, mapper);
    }

    public bool IsMatch(ResolutionContext context)
    {
      return context.SourceType.IsEnumerableType() && context.DestinationType.IsCollectionType();
    }

    private class EnumerableMapper<TCollection, TElement> : EnumerableMapperBase<TCollection> where TCollection : ICollection<TElement>
    {
      public override bool IsMatch(ResolutionContext context)
      {
        throw new NotImplementedException();
      }

      protected override void SetElementValue(
        TCollection destination,
        object mappedValue,
        int index)
      {
        destination.Add((TElement) mappedValue);
      }

      protected override void ClearEnumerable(TCollection enumerable) => enumerable.Clear();

      protected override TCollection CreateDestinationObjectBase(
        Type destElementType,
        int sourceLength)
      {
        return (TCollection) (!typeof (TCollection).IsInterface ? ObjectCreator.CreateDefaultValue(typeof (TCollection)) : (object) new List<TElement>());
      }
    }
  }
}
