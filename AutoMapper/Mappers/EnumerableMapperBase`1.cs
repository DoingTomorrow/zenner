// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.EnumerableMapperBase`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Mappers
{
  public abstract class EnumerableMapperBase<TEnumerable> : IObjectMapper where TEnumerable : IEnumerable
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      if (context.IsSourceValueNull && mapper.ShouldMapSourceCollectionAsNull(context))
        return (object) null;
      ICollection<object> list = (ICollection<object>) ((IEnumerable) context.SourceValue ?? (IEnumerable) new object[0]).Cast<object>().ToList<object>();
      Type elementType1 = TypeHelper.GetElementType(context.SourceType, (IEnumerable) list);
      Type elementType2 = TypeHelper.GetElementType(context.DestinationType);
      int count = list.Count;
      object destinationObject = this.GetOrCreateDestinationObject(context, mapper, elementType2, count);
      TEnumerable enumerableFor = this.GetEnumerableFor(destinationObject);
      this.ClearEnumerable(enumerableFor);
      int num = 0;
      foreach (object obj in (IEnumerable<object>) list)
      {
        ResolutionResult resolutionResult = new ResolutionResult(context.CreateElementContext((TypeMap) null, obj, elementType1, elementType2, num));
        TypeMap typeMapFor = mapper.ConfigurationProvider.FindTypeMapFor(resolutionResult, elementType2);
        Type sourceElementType = typeMapFor != null ? typeMapFor.SourceType : elementType1;
        Type destinationElementType = typeMapFor != null ? typeMapFor.DestinationType : elementType2;
        ResolutionContext elementContext = context.CreateElementContext(typeMapFor, obj, sourceElementType, destinationElementType, num);
        object mappedValue = mapper.Map(elementContext);
        this.SetElementValue(enumerableFor, mappedValue, num);
        ++num;
      }
      return destinationObject;
    }

    protected virtual object GetOrCreateDestinationObject(
      ResolutionContext context,
      IMappingEngineRunner mapper,
      Type destElementType,
      int sourceLength)
    {
      return context.DestinationValue ?? this.CreateDestinationObject(context, destElementType, sourceLength, mapper);
    }

    protected virtual TEnumerable GetEnumerableFor(object destination) => (TEnumerable) destination;

    protected virtual void ClearEnumerable(TEnumerable enumerable)
    {
    }

    protected virtual object CreateDestinationObject(
      ResolutionContext context,
      Type destinationElementType,
      int count,
      IMappingEngineRunner mapper)
    {
      Type destinationType = context.DestinationType;
      return !destinationType.IsInterface && !destinationType.IsArray ? mapper.CreateObject(context) : (object) this.CreateDestinationObjectBase(destinationElementType, count);
    }

    public abstract bool IsMatch(ResolutionContext context);

    protected abstract void SetElementValue(TEnumerable destination, object mappedValue, int index);

    protected abstract TEnumerable CreateDestinationObjectBase(
      Type destElementType,
      int sourceLength);
  }
}
