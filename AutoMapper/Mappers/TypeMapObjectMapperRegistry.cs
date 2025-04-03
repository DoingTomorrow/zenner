// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.TypeMapObjectMapperRegistry
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper.Mappers
{
  public static class TypeMapObjectMapperRegistry
  {
    private static readonly IList<ITypeMapObjectMapper> _mappers = (IList<ITypeMapObjectMapper>) new List<ITypeMapObjectMapper>()
    {
      (ITypeMapObjectMapper) new TypeMapObjectMapperRegistry.CustomMapperStrategy(),
      (ITypeMapObjectMapper) new TypeMapObjectMapperRegistry.NullMappingStrategy(),
      (ITypeMapObjectMapper) new TypeMapObjectMapperRegistry.CacheMappingStrategy(),
      (ITypeMapObjectMapper) new TypeMapObjectMapperRegistry.NewObjectPropertyMapMappingStrategy(),
      (ITypeMapObjectMapper) new TypeMapObjectMapperRegistry.ExistingObjectMappingStrategy()
    };

    public static IList<ITypeMapObjectMapper> Mappers => TypeMapObjectMapperRegistry._mappers;

    private class CustomMapperStrategy : ITypeMapObjectMapper
    {
      public object Map(ResolutionContext context, IMappingEngineRunner mapper)
      {
        return context.TypeMap.CustomMapper(context);
      }

      public bool IsMatch(ResolutionContext context, IMappingEngineRunner mapper)
      {
        return context.TypeMap.CustomMapper != null;
      }
    }

    private class NullMappingStrategy : ITypeMapObjectMapper
    {
      public object Map(ResolutionContext context, IMappingEngineRunner mapper) => (object) null;

      public bool IsMatch(ResolutionContext context, IMappingEngineRunner mapper)
      {
        IFormatterConfiguration profileConfiguration = mapper.ConfigurationProvider.GetProfileConfiguration(context.TypeMap.Profile);
        return context.SourceValue == null && profileConfiguration.MapNullSourceValuesAsNull;
      }
    }

    private class CacheMappingStrategy : ITypeMapObjectMapper
    {
      public object Map(ResolutionContext context, IMappingEngineRunner mapper)
      {
        return context.InstanceCache[context];
      }

      public bool IsMatch(ResolutionContext context, IMappingEngineRunner mapper)
      {
        return context.DestinationValue == null && context.InstanceCache.ContainsKey(context);
      }
    }

    private abstract class PropertyMapMappingStrategy : ITypeMapObjectMapper
    {
      public object Map(ResolutionContext context, IMappingEngineRunner mapper)
      {
        object mappedObject = this.GetMappedObject(context, mapper);
        if (context.SourceValue != null)
          context.InstanceCache[context] = mappedObject;
        context.TypeMap.BeforeMap(context.SourceValue, mappedObject);
        foreach (PropertyMap propertyMap in context.TypeMap.GetPropertyMaps())
          this.MapPropertyValue(context.CreatePropertyMapContext(propertyMap), mapper, mappedObject, propertyMap);
        object obj = this.ReassignValue(context, mappedObject);
        context.TypeMap.AfterMap(context.SourceValue, obj);
        return obj;
      }

      protected virtual object ReassignValue(ResolutionContext context, object o) => o;

      public abstract bool IsMatch(ResolutionContext context, IMappingEngineRunner mapper);

      protected abstract object GetMappedObject(
        ResolutionContext context,
        IMappingEngineRunner mapper);

      private void MapPropertyValue(
        ResolutionContext context,
        IMappingEngineRunner mapper,
        object mappedObject,
        PropertyMap propertyMap)
      {
        if (!propertyMap.CanResolveValue())
          return;
        Exception exception = (Exception) null;
        ResolutionResult resolutionResult;
        try
        {
          resolutionResult = propertyMap.ResolveValue(context);
        }
        catch (AutoMapperMappingException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          exception = (Exception) new AutoMapperMappingException(this.CreateErrorContext(context, propertyMap, (object) null), ex);
          resolutionResult = new ResolutionResult(context);
        }
        if (resolutionResult.ShouldIgnore)
          return;
        object destinationValue = propertyMap.GetDestinationValue(mappedObject);
        Type type = resolutionResult.Type;
        Type memberType = propertyMap.DestinationProperty.MemberType;
        TypeMap typeMapFor = mapper.ConfigurationProvider.FindTypeMapFor(resolutionResult, memberType);
        Type sourceMemberType = typeMapFor != null ? typeMapFor.SourceType : type;
        ResolutionContext memberContext = context.CreateMemberContext(typeMapFor, resolutionResult.Value, destinationValue, sourceMemberType, propertyMap);
        if (!propertyMap.ShouldAssignValue(memberContext))
          return;
        if (exception != null)
          throw exception;
        try
        {
          object propertyValueToAssign = mapper.Map(memberContext);
          this.AssignValue(propertyMap, mappedObject, propertyValueToAssign);
        }
        catch (AutoMapperMappingException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw new AutoMapperMappingException(memberContext, ex);
        }
      }

      protected virtual void AssignValue(
        PropertyMap propertyMap,
        object mappedObject,
        object propertyValueToAssign)
      {
        if (!propertyMap.CanBeSet)
          return;
        propertyMap.DestinationProperty.SetValue(mappedObject, propertyValueToAssign);
      }

      private ResolutionContext CreateErrorContext(
        ResolutionContext context,
        PropertyMap propertyMap,
        object destinationValue)
      {
        return context.CreateMemberContext((TypeMap) null, context.SourceValue, destinationValue, context.SourceValue == null ? typeof (object) : context.SourceValue.GetType(), propertyMap);
      }
    }

    private class NewObjectPropertyMapMappingStrategy : 
      TypeMapObjectMapperRegistry.PropertyMapMappingStrategy
    {
      public override bool IsMatch(ResolutionContext context, IMappingEngineRunner mapper)
      {
        return context.DestinationValue == null;
      }

      protected override object GetMappedObject(
        ResolutionContext context,
        IMappingEngineRunner mapper)
      {
        return mapper.CreateObject(context);
      }
    }

    private class ExistingObjectMappingStrategy : 
      TypeMapObjectMapperRegistry.PropertyMapMappingStrategy
    {
      public override bool IsMatch(ResolutionContext context, IMappingEngineRunner mapper) => true;

      protected override object GetMappedObject(
        ResolutionContext context,
        IMappingEngineRunner mapper)
      {
        return context.DestinationValue;
      }
    }
  }
}
