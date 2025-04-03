// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.ArrayMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;

#nullable disable
namespace AutoMapper.Mappers
{
  public class ArrayMapper : EnumerableMapperBase<Array>
  {
    public override bool IsMatch(ResolutionContext context)
    {
      return context.DestinationType.IsArray && context.SourceType.IsEnumerableType();
    }

    protected override void ClearEnumerable(Array enumerable)
    {
    }

    protected override void SetElementValue(Array destination, object mappedValue, int index)
    {
      destination.SetValue(mappedValue, index);
    }

    protected override Array CreateDestinationObjectBase(Type destElementType, int sourceLength)
    {
      throw new NotImplementedException();
    }

    protected override object GetOrCreateDestinationObject(
      ResolutionContext context,
      IMappingEngineRunner mapper,
      Type destElementType,
      int sourceLength)
    {
      return (object) ObjectCreator.CreateArray(destElementType, sourceLength);
    }
  }
}
