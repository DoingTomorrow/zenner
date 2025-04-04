// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.AbstractBasePropertyContainerMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public abstract class AbstractBasePropertyContainerMapper
  {
    protected Type container;
    protected HbmMapping mapDoc;

    protected AbstractBasePropertyContainerMapper(Type container, HbmMapping mapDoc)
    {
      if (container == null)
        throw new ArgumentNullException(nameof (container));
      if (mapDoc == null)
        throw new ArgumentNullException(nameof (mapDoc));
      this.container = container;
      this.mapDoc = mapDoc;
    }

    protected HbmMapping MapDoc => this.mapDoc;

    protected Type Container => this.container;

    protected abstract void AddProperty(object property);

    public virtual void Property(MemberInfo property, Action<IPropertyMapper> mapping)
    {
      HbmProperty hbmProperty = this.IsMemberSupportedByMappedContainer(property) ? new HbmProperty()
      {
        name = property.Name
      } : throw new ArgumentOutOfRangeException(nameof (property), "Can't add a property of another graph");
      mapping((IPropertyMapper) new PropertyMapper(property, hbmProperty));
      this.AddProperty((object) hbmProperty);
    }

    protected virtual bool IsMemberSupportedByMappedContainer(MemberInfo property)
    {
      return property.DeclaringType.IsAssignableFrom(this.container);
    }

    public virtual void Component(MemberInfo property, Action<IComponentMapper> mapping)
    {
      HbmComponent hbmComponent = this.IsMemberSupportedByMappedContainer(property) ? new HbmComponent()
      {
        name = property.Name
      } : throw new ArgumentOutOfRangeException(nameof (property), "Can't add a property of another graph");
      mapping((IComponentMapper) new ComponentMapper(hbmComponent, property.GetPropertyOrFieldType(), property, this.MapDoc));
      this.AddProperty((object) hbmComponent);
    }

    public virtual void Component(MemberInfo property, Action<IDynamicComponentMapper> mapping)
    {
      HbmDynamicComponent dynamicComponent = this.IsMemberSupportedByMappedContainer(property) ? new HbmDynamicComponent()
      {
        name = property.Name
      } : throw new ArgumentOutOfRangeException(nameof (property), "Can't add a property of another graph");
      mapping((IDynamicComponentMapper) new DynamicComponentMapper(dynamicComponent, property, this.MapDoc));
      this.AddProperty((object) dynamicComponent);
    }

    public virtual void ManyToOne(MemberInfo property, Action<IManyToOneMapper> mapping)
    {
      HbmManyToOne hbmManyToOne = this.IsMemberSupportedByMappedContainer(property) ? new HbmManyToOne()
      {
        name = property.Name
      } : throw new ArgumentOutOfRangeException(nameof (property), "Can't add a property of another graph");
      mapping((IManyToOneMapper) new ManyToOneMapper(property, hbmManyToOne, this.MapDoc));
      this.AddProperty((object) hbmManyToOne);
    }

    public virtual void Any(MemberInfo property, Type idTypeOfMetaType, Action<IAnyMapper> mapping)
    {
      HbmAny hbmAny = this.IsMemberSupportedByMappedContainer(property) ? new HbmAny()
      {
        name = property.Name
      } : throw new ArgumentOutOfRangeException(nameof (property), "Can't add a property of another graph");
      mapping((IAnyMapper) new AnyMapper(property, idTypeOfMetaType, hbmAny, this.MapDoc));
      this.AddProperty((object) hbmAny);
    }
  }
}
