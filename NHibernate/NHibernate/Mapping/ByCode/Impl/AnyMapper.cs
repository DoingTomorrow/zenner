// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.AnyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class AnyMapper : IAnyMapper, IEntityPropertyMapper, IAccessorPropertyMapper
  {
    private const string DefaultIdColumnNameWhenNoProperty = "ReferencedId";
    private const string DefaultMetaColumnNameWhenNoProperty = "ReferencedClass";
    private readonly HbmAny any;
    private readonly ColumnMapper classColumnMapper;
    private readonly IAccessorPropertyMapper entityPropertyMapper;
    private readonly System.Type foreignIdType;
    private readonly ColumnMapper idColumnMapper;
    private readonly HbmMapping mapDoc;
    private readonly MemberInfo member;

    public AnyMapper(MemberInfo member, System.Type foreignIdType, HbmAny any, HbmMapping mapDoc)
      : this(member, foreignIdType, member == null ? (IAccessorPropertyMapper) new NoMemberPropertyMapper() : (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => any.access = x)), any, mapDoc)
    {
    }

    public AnyMapper(
      MemberInfo member,
      System.Type foreignIdType,
      IAccessorPropertyMapper accessorMapper,
      HbmAny any,
      HbmMapping mapDoc)
    {
      this.member = member;
      this.foreignIdType = foreignIdType;
      this.any = any;
      this.mapDoc = mapDoc;
      if (member == null)
        this.any.access = "none";
      this.entityPropertyMapper = member == null ? (IAccessorPropertyMapper) new NoMemberPropertyMapper() : accessorMapper;
      if (foreignIdType == null)
        throw new ArgumentNullException(nameof (foreignIdType));
      if (any == null)
        throw new ArgumentNullException(nameof (any));
      this.any.idtype = this.foreignIdType.GetNhTypeName();
      HbmColumn mapping1 = new HbmColumn();
      string memberName1 = member == null ? "ReferencedId" : member.Name + "Id";
      this.idColumnMapper = new ColumnMapper(mapping1, memberName1);
      HbmColumn mapping2 = new HbmColumn();
      string memberName2 = member == null ? "ReferencedClass" : member.Name + "Class";
      this.classColumnMapper = new ColumnMapper(mapping2, memberName2);
      any.column = new HbmColumn[2]{ mapping2, mapping1 };
    }

    public void Access(Accessor accessor) => this.entityPropertyMapper.Access(accessor);

    public void Access(System.Type accessorType) => this.entityPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.any.optimisticlock = takeInConsiderationForOptimisticLock;
    }

    public void MetaType(IType metaType)
    {
      if (metaType == null)
        return;
      this.CheckMetaTypeImmutability(metaType.Name);
      this.any.metatype = metaType.Name;
    }

    public void MetaType<TMetaType>() => this.MetaType(typeof (TMetaType));

    public void MetaType(System.Type metaType)
    {
      if (metaType == null)
        return;
      string nhTypeName = metaType.GetNhTypeName();
      this.CheckMetaTypeImmutability(nhTypeName);
      this.any.metatype = nhTypeName;
    }

    public void IdType(IType idType)
    {
      if (idType == null)
        return;
      this.CheckIdTypeImmutability(idType.Name);
      this.any.idtype = idType.Name;
    }

    public void IdType<TIdType>() => this.IdType(typeof (TIdType));

    public void IdType(System.Type idType)
    {
      if (idType == null)
        return;
      string nhTypeName = idType.GetNhTypeName();
      this.CheckIdTypeImmutability(nhTypeName);
      this.any.idtype = nhTypeName;
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
      System.Type metaType = !(value is System.Type) ? value.GetType() : throw new ArgumentOutOfRangeException(nameof (value), "System.Type is invalid meta-type (you don't need to set meta-values).");
      if (this.any.metavalue == null)
        this.any.metavalue = new HbmMetaValue[0];
      Dictionary<string, string> dictionary = ((IEnumerable<HbmMetaValue>) this.any.metavalue).ToDictionary<HbmMetaValue, string, string>((Func<HbmMetaValue, string>) (mv => mv.value), (Func<HbmMetaValue, string>) (mv => mv.@class));
      this.MetaType(metaType);
      string shortClassName = entityType.GetShortClassName(this.mapDoc);
      string key = value.ToString();
      string str;
      if (dictionary.TryGetValue(key, out str) && str != shortClassName)
        throw new ArgumentException(string.Format("Can't set two different classes for same meta-value (meta-value='{0}' old-class:'{1}' new-class='{2}')", (object) key, (object) str, (object) shortClassName));
      dictionary[key] = shortClassName;
      this.any.metavalue = dictionary.Select<KeyValuePair<string, string>, HbmMetaValue>((Func<KeyValuePair<string, string>, HbmMetaValue>) (vd => new HbmMetaValue()
      {
        value = vd.Key,
        @class = vd.Value
      })).ToArray<HbmMetaValue>();
    }

    public void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle)
    {
      this.any.cascade = cascadeStyle.Exclude(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans).ToCascadeString();
    }

    public void Index(string indexName) => this.any.index = indexName;

    public void Lazy(bool isLazy) => this.any.lazy = isLazy;

    public void Update(bool consideredInUpdateQuery) => this.any.update = consideredInUpdateQuery;

    public void Insert(bool consideredInInsertQuery) => this.any.insert = consideredInInsertQuery;

    private void CheckMetaTypeImmutability(string nhTypeName)
    {
      if (this.any.metavalue != null && this.any.metavalue.Length > 0 && this.any.metatype != nhTypeName)
        throw new ArgumentException(string.Format("Can't change the meta-type (was '{0}' trying to change to '{1}')", (object) this.any.metatype, (object) nhTypeName));
    }

    private void CheckIdTypeImmutability(string nhTypeName)
    {
      if (this.any.metavalue != null && this.any.metavalue.Length > 0 && this.any.idtype != nhTypeName)
        throw new ArgumentException(string.Format("Can't change the id-type after add meta-values (was '{0}' trying to change to '{1}')", (object) this.any.idtype, (object) nhTypeName));
    }
  }
}
