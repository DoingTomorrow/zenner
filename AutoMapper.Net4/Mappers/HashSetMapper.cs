// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.HashSetMapper
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Mappers
{
  public class HashSetMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      return ((IObjectMapper) Activator.CreateInstance(typeof (HashSetMapper.EnumerableMapper<,>).MakeGenericType(context.DestinationType, TypeHelper.GetElementType(context.DestinationType)))).Map(context, mapper);
    }

    public bool IsMatch(ResolutionContext context)
    {
      return context.SourceType.IsEnumerableType() && HashSetMapper.IsSetType(context.DestinationType);
    }

    private static bool IsSetType(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (ISet<>) || ((IEnumerable<Type>) type.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).Select<Type, Type>((Func<Type, Type>) (t => t.GetGenericTypeDefinition())).Any<Type>((Func<Type, bool>) (t => t == typeof (ISet<>)));
    }

    private class EnumerableMapper<TCollection, TElement> : EnumerableMapperBase<TCollection> where TCollection : ISet<TElement>
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
        return (TCollection) (!typeof (TCollection).IsInterface ? ObjectCreator.CreateDefaultValue(typeof (TCollection)) : (object) new HashSet<TElement>());
      }
    }
  }
}
