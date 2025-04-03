// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.ListSourceMapper
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.Collections;
using System.ComponentModel;

#nullable disable
namespace AutoMapper.Mappers
{
  public class ListSourceMapper : EnumerableMapperBase<IList>
  {
    public override bool IsMatch(ResolutionContext context)
    {
      return typeof (IListSource).IsAssignableFrom(context.DestinationType);
    }

    protected override void SetElementValue(IList destination, object mappedValue, int index)
    {
      destination.Add(mappedValue);
    }

    protected override IList CreateDestinationObjectBase(Type destElementType, int sourceLength)
    {
      throw new NotImplementedException();
    }

    protected override IList GetEnumerableFor(object destination)
    {
      return ((IListSource) destination).GetList();
    }

    protected override void ClearEnumerable(IList enumerable) => enumerable.Clear();
  }
}
