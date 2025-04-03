// Decompiled with JetBrains decompiler
// Type: AutoMapper.ResolutionContext
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper
{
  public class ResolutionContext : IEquatable<ResolutionContext>
  {
    public MappingOperationOptions Options { get; private set; }

    public TypeMap TypeMap { get; private set; }

    public PropertyMap PropertyMap { get; private set; }

    public Type SourceType { get; private set; }

    public Type DestinationType { get; private set; }

    public int? ArrayIndex { get; private set; }

    public object SourceValue { get; private set; }

    public object DestinationValue { get; private set; }

    public ResolutionContext Parent { get; private set; }

    public Dictionary<ResolutionContext, object> InstanceCache { get; private set; }

    public ResolutionContext(
      TypeMap typeMap,
      object source,
      Type sourceType,
      Type destinationType,
      MappingOperationOptions options)
      : this(typeMap, source, (object) null, sourceType, destinationType, options)
    {
    }

    public ResolutionContext(
      TypeMap typeMap,
      object source,
      object destination,
      Type sourceType,
      Type destinationType,
      MappingOperationOptions options)
    {
      this.TypeMap = typeMap;
      this.SourceValue = source;
      this.DestinationValue = destination;
      this.AssignTypes(typeMap, sourceType, destinationType);
      this.InstanceCache = new Dictionary<ResolutionContext, object>();
      this.Options = options;
    }

    private void AssignTypes(TypeMap typeMap, Type sourceType, Type destinationType)
    {
      if (typeMap != null)
      {
        this.SourceType = typeMap.SourceType;
        this.DestinationType = typeMap.DestinationType;
      }
      else
      {
        this.SourceType = sourceType;
        this.DestinationType = destinationType;
      }
    }

    private ResolutionContext(ResolutionContext context, object sourceValue)
    {
      this.ArrayIndex = context.ArrayIndex;
      this.TypeMap = context.TypeMap;
      this.PropertyMap = context.PropertyMap;
      this.SourceType = context.SourceType;
      this.SourceValue = sourceValue;
      this.DestinationValue = context.DestinationValue;
      this.Parent = context;
      this.DestinationType = context.DestinationType;
      this.InstanceCache = context.InstanceCache;
      this.Options = context.Options;
    }

    private ResolutionContext(ResolutionContext context, object sourceValue, Type sourceType)
    {
      this.ArrayIndex = context.ArrayIndex;
      this.TypeMap = context.TypeMap;
      this.PropertyMap = context.PropertyMap;
      this.SourceType = sourceType;
      this.SourceValue = sourceValue;
      this.DestinationValue = context.DestinationValue;
      this.Parent = context;
      this.DestinationType = context.DestinationType;
      this.InstanceCache = context.InstanceCache;
      this.Options = context.Options;
    }

    private ResolutionContext(
      ResolutionContext context,
      TypeMap memberTypeMap,
      object sourceValue,
      object destinationValue,
      Type sourceType,
      Type destinationType)
    {
      this.TypeMap = memberTypeMap;
      this.SourceValue = sourceValue;
      this.DestinationValue = destinationValue;
      this.Parent = context;
      this.AssignTypes(memberTypeMap, sourceType, destinationType);
      this.InstanceCache = context.InstanceCache;
      this.Options = context.Options;
    }

    private ResolutionContext(
      ResolutionContext context,
      object sourceValue,
      object destinationValue,
      TypeMap memberTypeMap,
      PropertyMap propertyMap)
    {
      this.TypeMap = memberTypeMap;
      this.PropertyMap = propertyMap;
      this.SourceValue = sourceValue;
      this.DestinationValue = destinationValue;
      this.Parent = context;
      this.InstanceCache = context.InstanceCache;
      this.SourceType = memberTypeMap.SourceType;
      this.DestinationType = memberTypeMap.DestinationType;
      this.Options = context.Options;
    }

    private ResolutionContext(
      ResolutionContext context,
      object sourceValue,
      object destinationValue,
      Type sourceType,
      PropertyMap propertyMap)
    {
      this.PropertyMap = propertyMap;
      this.SourceType = sourceType;
      this.SourceValue = sourceValue;
      this.DestinationValue = destinationValue;
      this.Parent = context;
      this.DestinationType = propertyMap.DestinationProperty.MemberType;
      this.InstanceCache = context.InstanceCache;
      this.Options = context.Options;
    }

    private ResolutionContext(
      ResolutionContext context,
      object sourceValue,
      TypeMap typeMap,
      Type sourceType,
      Type destinationType,
      int arrayIndex)
    {
      this.ArrayIndex = new int?(arrayIndex);
      this.TypeMap = typeMap;
      this.PropertyMap = context.PropertyMap;
      this.SourceValue = sourceValue;
      this.Parent = context;
      this.InstanceCache = context.InstanceCache;
      this.AssignTypes(typeMap, sourceType, destinationType);
      this.Options = context.Options;
    }

    public string MemberName
    {
      get
      {
        if (this.PropertyMap == null)
          return string.Empty;
        return this.ArrayIndex.HasValue ? this.PropertyMap.DestinationProperty.Name + (object) this.ArrayIndex.Value : this.PropertyMap.DestinationProperty.Name;
      }
    }

    public bool IsSourceValueNull => this.SourceValue is null;

    public ResolutionContext CreateValueContext(object sourceValue)
    {
      return new ResolutionContext(this, sourceValue);
    }

    public ResolutionContext CreateValueContext(object sourceValue, Type sourceType)
    {
      return new ResolutionContext(this, sourceValue, sourceType);
    }

    public ResolutionContext CreateTypeContext(
      TypeMap memberTypeMap,
      object sourceValue,
      object destinationValue,
      Type sourceType,
      Type destinationType)
    {
      return new ResolutionContext(this, memberTypeMap, sourceValue, destinationValue, sourceType, destinationType);
    }

    public ResolutionContext CreatePropertyMapContext(PropertyMap propertyMap)
    {
      return new ResolutionContext(this, this.SourceValue, this.DestinationValue, this.SourceType, propertyMap);
    }

    public ResolutionContext CreateMemberContext(
      TypeMap memberTypeMap,
      object memberValue,
      object destinationValue,
      Type sourceMemberType,
      PropertyMap propertyMap)
    {
      return memberTypeMap == null ? new ResolutionContext(this, memberValue, destinationValue, sourceMemberType, propertyMap) : new ResolutionContext(this, memberValue, destinationValue, memberTypeMap, propertyMap);
    }

    public ResolutionContext CreateElementContext(
      TypeMap elementTypeMap,
      object item,
      Type sourceElementType,
      Type destinationElementType,
      int arrayIndex)
    {
      return new ResolutionContext(this, item, elementTypeMap, sourceElementType, destinationElementType, arrayIndex);
    }

    public override string ToString()
    {
      return string.Format("Trying to map {0} to {1}.", new object[2]
      {
        (object) this.SourceType.Name,
        (object) this.DestinationType.Name
      });
    }

    public TypeMap GetContextTypeMap()
    {
      TypeMap typeMap = this.TypeMap;
      for (ResolutionContext parent = this.Parent; typeMap == null && parent != null; parent = parent.Parent)
        typeMap = parent.TypeMap;
      return typeMap;
    }

    public PropertyMap GetContextPropertyMap()
    {
      PropertyMap propertyMap = this.PropertyMap;
      for (ResolutionContext parent = this.Parent; propertyMap == null && parent != null; parent = parent.Parent)
        propertyMap = parent.PropertyMap;
      return propertyMap;
    }

    public bool Equals(ResolutionContext other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.TypeMap, (object) this.TypeMap) && object.Equals((object) other.SourceType, (object) this.SourceType) && object.Equals((object) other.DestinationType, (object) this.DestinationType) && object.Equals(other.SourceValue, this.SourceValue);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (ResolutionContext) && this.Equals((ResolutionContext) obj);
    }

    public override int GetHashCode()
    {
      return (((this.TypeMap != null ? this.TypeMap.GetHashCode() : 0) * 397 ^ ((object) this.SourceType != null ? this.SourceType.GetHashCode() : 0)) * 397 ^ ((object) this.DestinationType != null ? this.DestinationType.GetHashCode() : 0)) * 397 ^ (this.SourceValue != null ? this.SourceValue.GetHashCode() : 0);
    }

    public static ResolutionContext New<TSource>(TSource sourceValue)
    {
      return new ResolutionContext((TypeMap) null, (object) sourceValue, typeof (TSource), (Type) null, new MappingOperationOptions());
    }
  }
}
