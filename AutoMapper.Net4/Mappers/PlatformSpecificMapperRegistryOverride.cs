// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.PlatformSpecificMapperRegistryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.Linq;

#nullable disable
namespace AutoMapper.Mappers
{
  public class PlatformSpecificMapperRegistryOverride : IPlatformSpecificMapperRegistry
  {
    public void Initialize()
    {
      this.InsertBefore<TypeMapMapper>((IObjectMapper) new DataReaderMapper());
      this.InsertBefore<DictionaryMapper>((IObjectMapper) new NameValueCollectionMapper());
      this.InsertBefore<ReadOnlyCollectionMapper>((IObjectMapper) new ListSourceMapper());
      this.InsertBefore<CollectionMapper>((IObjectMapper) new HashSetMapper());
      this.InsertBefore<NullableSourceMapper>((IObjectMapper) new TypeConverterMapper());
    }

    private void InsertBefore<TObjectMapper>(IObjectMapper mapper) where TObjectMapper : IObjectMapper
    {
      IObjectMapper objectMapper = MapperRegistry.Mappers.FirstOrDefault<IObjectMapper>((Func<IObjectMapper, bool>) (om => om is TObjectMapper));
      MapperRegistry.Mappers.Insert(objectMapper == null ? 0 : MapperRegistry.Mappers.IndexOf(objectMapper), mapper);
    }
  }
}
