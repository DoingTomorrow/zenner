// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.ReadOnlyCollectionMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace AutoMapper.Mappers
{
  public class ReadOnlyCollectionMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      return ((IObjectMapper) Activator.CreateInstance(typeof (ReadOnlyCollectionMapper.EnumerableMapper<>).MakeGenericType(TypeHelper.GetElementType(context.DestinationType)))).Map(context.CreateMemberContext(context.TypeMap, context.SourceValue, (object) null, context.SourceType, context.PropertyMap), mapper);
    }

    public bool IsMatch(ResolutionContext context)
    {
      return context.SourceType.IsEnumerableType() && context.DestinationType.IsGenericType && (object) context.DestinationType.GetGenericTypeDefinition() == (object) typeof (ReadOnlyCollection<>);
    }

    private class EnumerableMapper<TElement> : EnumerableMapperBase<IList<TElement>>
    {
      private readonly IList<TElement> inner = (IList<TElement>) new List<TElement>();

      public override bool IsMatch(ResolutionContext context)
      {
        throw new NotImplementedException();
      }

      protected override void SetElementValue(
        IList<TElement> elements,
        object mappedValue,
        int index)
      {
        this.inner.Add((TElement) mappedValue);
      }

      protected override IList<TElement> GetEnumerableFor(object destination) => this.inner;

      protected override IList<TElement> CreateDestinationObjectBase(
        Type destElementType,
        int sourceLength)
      {
        throw new NotImplementedException();
      }

      protected override object CreateDestinationObject(
        ResolutionContext context,
        Type destinationElementType,
        int count,
        IMappingEngineRunner mapper)
      {
        return (object) new ReadOnlyCollection<TElement>(this.inner);
      }
    }
  }
}
