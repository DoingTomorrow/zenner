// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.EnumerableMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections;

#nullable disable
namespace AutoMapper.Mappers
{
  public class EnumerableMapper : EnumerableMapperBase<IList>
  {
    public override bool IsMatch(ResolutionContext context)
    {
      return context.DestinationType.IsEnumerableType() && context.SourceType.IsEnumerableType();
    }

    protected override void SetElementValue(IList destination, object mappedValue, int index)
    {
      destination.Add(mappedValue);
    }

    protected override void ClearEnumerable(IList enumerable) => enumerable.Clear();

    protected override IList CreateDestinationObjectBase(Type destElementType, int sourceLength)
    {
      return ObjectCreator.CreateList(destElementType);
    }
  }
}
