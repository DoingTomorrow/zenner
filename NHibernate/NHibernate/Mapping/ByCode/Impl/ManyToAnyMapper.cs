// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ManyToAnyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ManyToAnyMapper : IManyToAnyMapper
  {
    private const string DefaultIdColumnNameWhenNoProperty = "ReferencedId";
    private const string DefaultMetaColumnNameWhenNoProperty = "ReferencedClass";
    private readonly System.Type elementType;
    private readonly System.Type foreignIdType;
    private readonly ColumnMapper classColumnMapper;
    private readonly ColumnMapper idColumnMapper;
    private readonly HbmManyToAny manyToAny;
    private readonly HbmMapping mapDoc;

    public ManyToAnyMapper(
      System.Type elementType,
      System.Type foreignIdType,
      HbmManyToAny manyToAny,
      HbmMapping mapDoc)
    {
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      if (foreignIdType == null)
        throw new ArgumentNullException(nameof (foreignIdType));
      if (manyToAny == null)
        throw new ArgumentNullException(nameof (manyToAny));
      this.elementType = elementType;
      this.foreignIdType = foreignIdType;
      this.manyToAny = manyToAny;
      this.mapDoc = mapDoc;
      this.manyToAny.idtype = this.foreignIdType.GetNhTypeName();
      HbmColumn mapping1 = new HbmColumn();
      this.idColumnMapper = new ColumnMapper(mapping1, "ReferencedId");
      HbmColumn mapping2 = new HbmColumn();
      this.classColumnMapper = new ColumnMapper(mapping2, "ReferencedClass");
      manyToAny.column = new HbmColumn[2]
      {
        mapping2,
        mapping1
      };
    }

    public void MetaType(IType metaType)
    {
      if (metaType == null)
        return;
      this.CheckMetaTypeImmutability(metaType.Name);
      this.manyToAny.metatype = metaType.Name;
    }

    public void MetaType<TMetaType>() => this.MetaType(typeof (TMetaType));

    public void MetaType(System.Type metaType)
    {
      if (metaType == null)
        return;
      string nhTypeName = metaType.GetNhTypeName();
      this.CheckMetaTypeImmutability(nhTypeName);
      this.manyToAny.metatype = nhTypeName;
    }

    public void IdType(IType idType)
    {
      if (idType == null)
        return;
      this.CheckIdTypeImmutability(idType.Name);
      this.manyToAny.idtype = idType.Name;
    }

    public void IdType<TIdType>() => this.IdType(typeof (TIdType));

    public void IdType(System.Type idType)
    {
      if (idType == null)
        return;
      string nhTypeName = idType.GetNhTypeName();
      this.CheckIdTypeImmutability(nhTypeName);
      this.manyToAny.idtype = nhTypeName;
    }

    public void Columns(
      Action<IColumnMapper> idColumnMapping,
      Action<IColumnMapper> classColumnMapping)
    {
      idColumnMapping((IColumnMapper) this.idColumnMapper);
      classColumnMapping((IColumnMapper) this.classColumnMapper);
    }

    public void MetaValue(object value, System.Type entityType)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (entityType == null)
        throw new ArgumentNullException(nameof (entityType));
      if (value is System.Type)
        throw new ArgumentOutOfRangeException(nameof (value), "System.Type is invalid meta-type (you don't need to set meta-values).");
      if (!this.elementType.IsAssignableFrom(entityType))
        throw new ArgumentOutOfRangeException(nameof (entityType), string.Format("A {0} is not assignable to the collection's elements which type is {1}", (object) entityType, (object) this.elementType));
      System.Type type = value.GetType();
      if (this.manyToAny.metavalue == null)
        this.manyToAny.metavalue = new HbmMetaValue[0];
      Dictionary<string, string> dictionary = ((IEnumerable<HbmMetaValue>) this.manyToAny.metavalue).ToDictionary<HbmMetaValue, string, string>((Func<HbmMetaValue, string>) (mv => mv.value), (Func<HbmMetaValue, string>) (mv => mv.@class));
      this.MetaType(type);
      string shortClassName = entityType.GetShortClassName(this.mapDoc);
      string key = value.ToString();
      string str;
      if (dictionary.TryGetValue(key, out str) && str != shortClassName)
        throw new ArgumentException(string.Format("Can't set two different classes for same meta-value (meta-value='{0}' old-class:'{1}' new-class='{2}')", (object) key, (object) str, (object) shortClassName));
      dictionary[key] = shortClassName;
      this.manyToAny.metavalue = dictionary.Select<KeyValuePair<string, string>, HbmMetaValue>((Func<KeyValuePair<string, string>, HbmMetaValue>) (vd => new HbmMetaValue()
      {
        value = vd.Key,
        @class = vd.Value
      })).ToArray<HbmMetaValue>();
    }

    private void CheckMetaTypeImmutability(string nhTypeName)
    {
      if (this.manyToAny.metavalue != null && this.manyToAny.metavalue.Length > 0 && this.manyToAny.metatype != nhTypeName)
        throw new ArgumentException(string.Format("Can't change the meta-type (was '{0}' trying to change to '{1}')", (object) this.manyToAny.metatype, (object) nhTypeName));
    }

    private void CheckIdTypeImmutability(string nhTypeName)
    {
      if (this.manyToAny.metavalue != null && this.manyToAny.metavalue.Length > 0 && this.manyToAny.idtype != nhTypeName)
        throw new ArgumentException(string.Format("Can't change the id-type after add meta-values (was '{0}' trying to change to '{1}')", (object) this.manyToAny.idtype, (object) nhTypeName));
    }
  }
}
