// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.MapKeyRelation
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class MapKeyRelation : IMapKeyRelation
  {
    private readonly Type dictionaryKeyType;
    private readonly HbmMapping mapDoc;
    private readonly HbmMap mapMapping;
    private ComponentMapKeyMapper componentMapKeyMapper;
    private MapKeyManyToManyMapper mapKeyManyToManyMapper;
    private MapKeyMapper mapKeyMapper;

    public MapKeyRelation(Type dictionaryKeyType, HbmMap mapMapping, HbmMapping mapDoc)
    {
      if (dictionaryKeyType == null)
        throw new ArgumentNullException(nameof (dictionaryKeyType));
      if (mapMapping == null)
        throw new ArgumentNullException(nameof (mapMapping));
      if (mapDoc == null)
        throw new ArgumentNullException(nameof (mapDoc));
      this.dictionaryKeyType = dictionaryKeyType;
      this.mapMapping = mapMapping;
      this.mapDoc = mapDoc;
    }

    public void Element(Action<IMapKeyMapper> mapping)
    {
      if (this.mapKeyMapper == null)
        this.mapKeyMapper = new MapKeyMapper(new HbmMapKey()
        {
          type = this.dictionaryKeyType.GetNhTypeName()
        });
      mapping((IMapKeyMapper) this.mapKeyMapper);
      this.mapMapping.Item = (object) this.mapKeyMapper.MapKeyMapping;
    }

    public void ManyToMany(Action<IMapKeyManyToManyMapper> mapping)
    {
      if (this.mapKeyManyToManyMapper == null)
        this.mapKeyManyToManyMapper = new MapKeyManyToManyMapper(new HbmMapKeyManyToMany()
        {
          @class = this.dictionaryKeyType.GetShortClassName(this.mapDoc)
        });
      mapping((IMapKeyManyToManyMapper) this.mapKeyManyToManyMapper);
      this.mapMapping.Item = (object) this.mapKeyManyToManyMapper.MapKeyManyToManyMapping;
    }

    public void Component(Action<IComponentMapKeyMapper> mapping)
    {
      if (this.componentMapKeyMapper == null)
        this.componentMapKeyMapper = new ComponentMapKeyMapper(this.dictionaryKeyType, new HbmCompositeMapKey(), this.mapDoc);
      mapping((IComponentMapKeyMapper) this.componentMapKeyMapper);
      this.mapMapping.Item = (object) this.componentMapKeyMapper.CompositeMapKeyMapping;
    }
  }
}
