// Decompiled with JetBrains decompiler
// Type: AutoMapper.TypeMap
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class TypeMap
  {
    private readonly IList<Action<object, object>> _afterMapActions = (IList<Action<object, object>>) new List<Action<object, object>>();
    private readonly IList<Action<object, object>> _beforeMapActions = (IList<Action<object, object>>) new List<Action<object, object>>();
    private readonly TypeInfo _destinationType;
    private readonly ISet<TypePair> _includedDerivedTypes = (ISet<TypePair>) new HashSet<TypePair>();
    private readonly ThreadSafeList<PropertyMap> _propertyMaps = new ThreadSafeList<PropertyMap>();
    private readonly ThreadSafeList<SourceMemberConfig> _sourceMemberConfigs = new ThreadSafeList<SourceMemberConfig>();
    private readonly IList<PropertyMap> _inheritedMaps = (IList<PropertyMap>) new List<PropertyMap>();
    private PropertyMap[] _orderedPropertyMaps;
    private readonly TypeInfo _sourceType;
    private bool _sealed;
    private Func<ResolutionContext, bool> _condition;
    private ConstructorMap _constructorMap;

    public TypeMap(TypeInfo sourceType, TypeInfo destinationType, MemberList memberList)
    {
      this._sourceType = sourceType;
      this._destinationType = destinationType;
      this.Profile = "";
      this.ConfiguredMemberList = memberList;
    }

    public ConstructorMap ConstructorMap => this._constructorMap;

    public Type SourceType => this._sourceType.Type;

    public Type DestinationType => this._destinationType.Type;

    public string Profile { get; set; }

    public Func<ResolutionContext, object> CustomMapper { get; private set; }

    public Action<object, object> BeforeMap
    {
      get
      {
        return (Action<object, object>) ((src, dest) =>
        {
          foreach (Action<object, object> beforeMapAction in (IEnumerable<Action<object, object>>) this._beforeMapActions)
            beforeMapAction(src, dest);
        });
      }
    }

    public Action<object, object> AfterMap
    {
      get
      {
        return (Action<object, object>) ((src, dest) =>
        {
          foreach (Action<object, object> afterMapAction in (IEnumerable<Action<object, object>>) this._afterMapActions)
            afterMapAction(src, dest);
        });
      }
    }

    public Func<ResolutionContext, object> DestinationCtor { get; set; }

    public List<string> IgnorePropertiesStartingWith { get; set; }

    public Type DestinationTypeOverride { get; set; }

    public bool ConstructDestinationUsingServiceLocator { get; set; }

    public MemberList ConfiguredMemberList { get; private set; }

    public IEnumerable<PropertyMap> GetPropertyMaps()
    {
      return this._sealed ? (IEnumerable<PropertyMap>) this._orderedPropertyMaps : this._propertyMaps.Concat<PropertyMap>((IEnumerable<PropertyMap>) this._inheritedMaps);
    }

    public IEnumerable<PropertyMap> GetCustomPropertyMaps()
    {
      return (IEnumerable<PropertyMap>) this._propertyMaps;
    }

    public void AddPropertyMap(PropertyMap propertyMap) => this._propertyMaps.Add(propertyMap);

    protected void AddInheritedMap(PropertyMap propertyMap) => this._inheritedMaps.Add(propertyMap);

    public void AddPropertyMap(IMemberAccessor destProperty, IEnumerable<IValueResolver> resolvers)
    {
      PropertyMap propertyMap = new PropertyMap(destProperty);
      resolvers.Each<IValueResolver>(new Action<IValueResolver>(propertyMap.ChainResolver));
      this.AddPropertyMap(propertyMap);
    }

    public string[] GetUnmappedPropertyNames()
    {
      IEnumerable<string> second1 = this._propertyMaps.Where<PropertyMap>((Func<PropertyMap, bool>) (pm => pm.IsMapped())).Select<PropertyMap, string>((Func<PropertyMap, string>) (pm => pm.DestinationProperty.Name));
      IEnumerable<string> second2 = this._inheritedMaps.Where<PropertyMap>((Func<PropertyMap, bool>) (pm => pm.IsMapped())).Select<PropertyMap, string>((Func<PropertyMap, string>) (pm => pm.DestinationProperty.Name));
      IEnumerable<string> source;
      if (this.ConfiguredMemberList == MemberList.Destination)
      {
        source = this._destinationType.GetPublicWriteAccessors().Select<MemberInfo, string>((Func<MemberInfo, string>) (p => p.Name)).Except<string>(second1).Except<string>(second2);
      }
      else
      {
        IEnumerable<string> second3 = this._propertyMaps.Where<PropertyMap>((Func<PropertyMap, bool>) (pm => pm.IsMapped())).Where<PropertyMap>((Func<PropertyMap, bool>) (pm => pm.CustomExpression != null)).Where<PropertyMap>((Func<PropertyMap, bool>) (pm => (object) pm.SourceMember != null)).Select<PropertyMap, string>((Func<PropertyMap, string>) (pm => pm.SourceMember.Name));
        IEnumerable<string> second4 = this._sourceMemberConfigs.Where<SourceMemberConfig>((Func<SourceMemberConfig, bool>) (smc => smc.IsIgnored())).Select<SourceMemberConfig, string>((Func<SourceMemberConfig, string>) (pm => pm.SourceMember.Name));
        source = this._sourceType.GetPublicReadAccessors().Select<MemberInfo, string>((Func<MemberInfo, string>) (p => p.Name)).Except<string>(second1).Except<string>(second2).Except<string>(second3).Except<string>(second4);
      }
      return source.Where<string>((Func<string, bool>) (memberName => !this.IgnorePropertiesStartingWith.Any<string>(new Func<string, bool>(memberName.StartsWith)))).ToArray<string>();
    }

    public PropertyMap FindOrCreatePropertyMapFor(IMemberAccessor destinationProperty)
    {
      PropertyMap propertyMap = this.GetExistingPropertyMapFor(destinationProperty);
      if (propertyMap == null)
      {
        propertyMap = new PropertyMap(destinationProperty);
        this.AddPropertyMap(propertyMap);
      }
      return propertyMap;
    }

    public void IncludeDerivedTypes(Type derivedSourceType, Type derivedDestinationType)
    {
      this._includedDerivedTypes.Add(new TypePair(derivedSourceType, derivedDestinationType));
    }

    public Type GetDerivedTypeFor(Type derivedSourceType)
    {
      TypePair typePair = this._includedDerivedTypes.FirstOrDefault<TypePair>((Func<TypePair, bool>) (tp => (object) tp.SourceType == (object) derivedSourceType));
      return !typePair.Equals(new TypePair()) ? typePair.DestinationType : this.DestinationType;
    }

    public bool TypeHasBeenIncluded(Type derivedSourceType, Type derivedDestinationType)
    {
      return this._includedDerivedTypes.Contains(new TypePair(derivedSourceType, derivedDestinationType));
    }

    public bool HasDerivedTypesToInclude() => this._includedDerivedTypes.Any<TypePair>();

    public void UseCustomMapper(Func<ResolutionContext, object> customMapper)
    {
      this.CustomMapper = customMapper;
      this._propertyMaps.Clear();
    }

    public void AddBeforeMapAction(Action<object, object> beforeMap)
    {
      this._beforeMapActions.Add(beforeMap);
    }

    public void AddAfterMapAction(Action<object, object> afterMap)
    {
      this._afterMapActions.Add(afterMap);
    }

    public void Seal()
    {
      if (this._sealed)
        return;
      this._orderedPropertyMaps = this._propertyMaps.Union<PropertyMap>((IEnumerable<PropertyMap>) this._inheritedMaps).OrderBy<PropertyMap, int>((Func<PropertyMap, int>) (map => map.GetMappingOrder())).ToArray<PropertyMap>();
      ((IEnumerable<PropertyMap>) this._orderedPropertyMaps).Each<PropertyMap>((Action<PropertyMap>) (pm => pm.Seal()));
      this._sealed = true;
    }

    public bool Equals(TypeMap other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other._sourceType, (object) this._sourceType) && object.Equals((object) other._destinationType, (object) this._destinationType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (TypeMap) && this.Equals((TypeMap) obj);
    }

    public override int GetHashCode()
    {
      return this._sourceType.GetHashCode() * 397 ^ this._destinationType.GetHashCode();
    }

    public PropertyMap GetExistingPropertyMapFor(IMemberAccessor destinationProperty)
    {
      return this._propertyMaps.FirstOrDefault<PropertyMap>((Func<PropertyMap, bool>) (pm => pm.DestinationProperty.Name.Equals(destinationProperty.Name))) ?? this._inheritedMaps.FirstOrDefault<PropertyMap>((Func<PropertyMap, bool>) (pm => pm.DestinationProperty.Name.Equals(destinationProperty.Name)));
    }

    public void AddInheritedPropertyMap(PropertyMap mappedProperty)
    {
      this._inheritedMaps.Add(mappedProperty);
    }

    public void InheritTypes(TypeMap inheritedTypeMap)
    {
      foreach (TypePair typePair in inheritedTypeMap._includedDerivedTypes.Where<TypePair>((Func<TypePair, bool>) (includedDerivedType => !this._includedDerivedTypes.Contains(includedDerivedType))))
        this._includedDerivedTypes.Add(typePair);
    }

    public void SetCondition(Func<ResolutionContext, bool> condition)
    {
      this._condition = condition;
    }

    public bool ShouldAssignValue(ResolutionContext resolutionContext)
    {
      return this._condition == null || this._condition(resolutionContext);
    }

    public void AddConstructorMap(
      ConstructorInfo constructorInfo,
      IEnumerable<ConstructorParameterMap> parameters)
    {
      this._constructorMap = new ConstructorMap(constructorInfo, parameters);
    }

    public SourceMemberConfig FindOrCreateSourceMemberConfigFor(MemberInfo sourceMember)
    {
      SourceMemberConfig propertyMap = this._sourceMemberConfigs.FirstOrDefault<SourceMemberConfig>((Func<SourceMemberConfig, bool>) (smc => (object) smc.SourceMember == (object) sourceMember));
      if (propertyMap == null)
      {
        propertyMap = new SourceMemberConfig(sourceMember);
        this._sourceMemberConfigs.Add(propertyMap);
      }
      return propertyMap;
    }
  }
}
