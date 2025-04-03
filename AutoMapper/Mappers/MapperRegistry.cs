// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.MapperRegistry
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Collections.Generic;

#nullable disable
namespace AutoMapper.Mappers
{
  public static class MapperRegistry
  {
    private static readonly IObjectMapper[] _initialMappers = new IObjectMapper[17]
    {
      (IObjectMapper) new TypeMapMapper((IEnumerable<ITypeMapObjectMapper>) TypeMapObjectMapperRegistry.Mappers),
      (IObjectMapper) new StringMapper(),
      (IObjectMapper) new AssignableArrayMapper(),
      (IObjectMapper) new FlagsEnumMapper(),
      (IObjectMapper) new EnumMapper(),
      (IObjectMapper) new PrimitiveArrayMapper(),
      (IObjectMapper) new ArrayMapper(),
      (IObjectMapper) new EnumerableToDictionaryMapper(),
      (IObjectMapper) new DictionaryMapper(),
      (IObjectMapper) new ReadOnlyCollectionMapper(),
      (IObjectMapper) new CollectionMapper(),
      (IObjectMapper) new EnumerableMapper(),
      (IObjectMapper) new AssignableMapper(),
      (IObjectMapper) new NullableSourceMapper(),
      (IObjectMapper) new NullableMapper(),
      (IObjectMapper) new ImplicitConversionOperatorMapper(),
      (IObjectMapper) new ExplicitConversionOperatorMapper()
    };
    private static readonly List<IObjectMapper> _mappers = new List<IObjectMapper>((IEnumerable<IObjectMapper>) MapperRegistry._initialMappers);

    public static IList<IObjectMapper> Mappers => (IList<IObjectMapper>) MapperRegistry._mappers;

    public static void Reset()
    {
      MapperRegistry._mappers.Clear();
      MapperRegistry._mappers.AddRange((IEnumerable<IObjectMapper>) MapperRegistry._initialMappers);
    }
  }
}
