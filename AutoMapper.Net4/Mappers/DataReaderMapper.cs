// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.DataReaderMapper
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using AutoMapper.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace AutoMapper.Mappers
{
  public class DataReaderMapper : IObjectMapper
  {
    private static ConcurrentDictionary<DataReaderMapper.BuilderKey, DataReaderMapper.Build> _builderCache = new ConcurrentDictionary<DataReaderMapper.BuilderKey, DataReaderMapper.Build>();
    private static ConcurrentDictionary<Type, DataReaderMapper.CreateEnumerableAdapter> _enumerableAdapterCache = new ConcurrentDictionary<Type, DataReaderMapper.CreateEnumerableAdapter>();
    private static readonly MethodInfo getValueMethod = typeof (IDataRecord).GetMethod("get_Item", new Type[1]
    {
      typeof (int)
    });
    private static readonly MethodInfo isDBNullMethod = typeof (IDataRecord).GetMethod("IsDBNull", new Type[1]
    {
      typeof (int)
    });

    static DataReaderMapper()
    {
      FeatureDetector.IsIDataRecordType = (System.Func<Type, bool>) (t => typeof (IDataRecord).IsAssignableFrom(t));
    }

    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      if (DataReaderMapper.IsDataReader(context))
      {
        bool yieldReturnEnabled = ((IMappingOptions) mapper.ConfigurationProvider).DataReaderMapperYieldReturnEnabled;
        Type elementType = TypeHelper.GetElementType(context.DestinationType);
        IEnumerable enumerable = DataReaderMapper.MapDataReaderToEnumerable(context, mapper, elementType, yieldReturnEnabled);
        return yieldReturnEnabled ? DataReaderMapper.GetDelegateToCreateEnumerableAdapter(elementType)(enumerable) : (object) enumerable;
      }
      if (!DataReaderMapper.IsDataRecord(context))
        return (object) null;
      IDataRecord sourceValue = context.SourceValue as IDataRecord;
      object result = DataReaderMapper.CreateBuilder(context.DestinationType, sourceValue)(sourceValue);
      DataReaderMapper.MapPropertyValues(context, mapper, result);
      return result;
    }

    private static IEnumerable MapDataReaderToEnumerable(
      ResolutionContext context,
      IMappingEngineRunner mapper,
      Type destinationElementType,
      bool useYieldReturn)
    {
      IDataReader sourceValue = (IDataReader) context.SourceValue;
      ResolutionContext resolveUsingContext = context;
      if (context.TypeMap == null)
        resolveUsingContext = new ResolutionContext(mapper.ConfigurationProvider.FindTypeMapFor(context.SourceValue, context.DestinationValue, context.SourceType, destinationElementType), context.SourceValue, context.SourceType, destinationElementType, new MappingOperationOptions());
      DataReaderMapper.Build builder = DataReaderMapper.CreateBuilder(destinationElementType, (IDataRecord) sourceValue);
      return useYieldReturn ? DataReaderMapper.LoadDataReaderViaYieldReturn(sourceValue, mapper, builder, resolveUsingContext) : DataReaderMapper.LoadDataReaderViaList(sourceValue, mapper, builder, resolveUsingContext, destinationElementType);
    }

    private static IEnumerable LoadDataReaderViaList(
      IDataReader dataReader,
      IMappingEngineRunner mapper,
      DataReaderMapper.Build buildFrom,
      ResolutionContext resolveUsingContext,
      Type elementType)
    {
      IList list = ObjectCreator.CreateList(elementType);
      while (dataReader.Read())
      {
        object result = buildFrom((IDataRecord) dataReader);
        DataReaderMapper.MapPropertyValues(resolveUsingContext, mapper, result);
        list.Add(result);
      }
      return (IEnumerable) list;
    }

    private static IEnumerable LoadDataReaderViaYieldReturn(
      IDataReader dataReader,
      IMappingEngineRunner mapper,
      DataReaderMapper.Build buildFrom,
      ResolutionContext resolveUsingContext)
    {
      while (dataReader.Read())
      {
        object result = buildFrom((IDataRecord) dataReader);
        DataReaderMapper.MapPropertyValues(resolveUsingContext, mapper, result);
        yield return result;
      }
    }

    public bool IsMatch(ResolutionContext context)
    {
      return DataReaderMapper.IsDataReader(context) || DataReaderMapper.IsDataRecord(context);
    }

    private static bool IsDataReader(ResolutionContext context)
    {
      return typeof (IDataReader).IsAssignableFrom(context.SourceType) && context.DestinationType.IsEnumerableType();
    }

    private static bool IsDataRecord(ResolutionContext context)
    {
      return typeof (IDataRecord).IsAssignableFrom(context.SourceType);
    }

    private static DataReaderMapper.Build CreateBuilder(
      Type destinationType,
      IDataRecord dataRecord)
    {
      DataReaderMapper.BuilderKey key = new DataReaderMapper.BuilderKey(destinationType, dataRecord);
      DataReaderMapper.Build builder1;
      if (DataReaderMapper._builderCache.TryGetValue(key, out builder1))
        return builder1;
      DynamicMethod dynamicMethod = new DynamicMethod("DynamicCreate", destinationType, new Type[1]
      {
        typeof (IDataRecord)
      }, destinationType, true);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      LocalBuilder local = ilGenerator.DeclareLocal(destinationType);
      ilGenerator.Emit(OpCodes.Newobj, destinationType.GetConstructor(Type.EmptyTypes));
      ilGenerator.Emit(OpCodes.Stloc, local);
      for (int i = 0; i < dataRecord.FieldCount; ++i)
      {
        PropertyInfo property = destinationType.GetProperty(dataRecord.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        Label label = ilGenerator.DefineLabel();
        if (property != (PropertyInfo) null && property.GetSetMethod(true) != (MethodInfo) null)
        {
          ilGenerator.Emit(OpCodes.Ldarg_0);
          ilGenerator.Emit(OpCodes.Ldc_I4, i);
          ilGenerator.Emit(OpCodes.Callvirt, DataReaderMapper.isDBNullMethod);
          ilGenerator.Emit(OpCodes.Brtrue, label);
          ilGenerator.Emit(OpCodes.Ldloc, local);
          ilGenerator.Emit(OpCodes.Ldarg_0);
          ilGenerator.Emit(OpCodes.Ldc_I4, i);
          ilGenerator.Emit(OpCodes.Callvirt, DataReaderMapper.getValueMethod);
          if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof (Nullable<>)))
          {
            Type genericArgument = property.PropertyType.GetGenericTypeDefinition().GetGenericArguments()[0];
            if (!genericArgument.IsEnum)
            {
              ilGenerator.Emit(OpCodes.Unbox_Any, property.PropertyType);
            }
            else
            {
              ilGenerator.Emit(OpCodes.Unbox_Any, genericArgument);
              ilGenerator.Emit(OpCodes.Newobj, property.PropertyType);
            }
          }
          else
            ilGenerator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
          ilGenerator.Emit(OpCodes.Callvirt, property.GetSetMethod(true));
          ilGenerator.MarkLabel(label);
        }
      }
      ilGenerator.Emit(OpCodes.Ldloc, local);
      ilGenerator.Emit(OpCodes.Ret);
      DataReaderMapper.Build builder2 = (DataReaderMapper.Build) dynamicMethod.CreateDelegate(typeof (DataReaderMapper.Build));
      DataReaderMapper._builderCache[key] = builder2;
      return builder2;
    }

    private static void MapPropertyValues(
      ResolutionContext context,
      IMappingEngineRunner mapper,
      object result)
    {
      if (context.TypeMap == null)
        throw new AutoMapperMappingException(context, "Missing type map configuration or unsupported mapping.");
      foreach (PropertyMap propertyMap in context.TypeMap.GetPropertyMaps())
        DataReaderMapper.MapPropertyValue(context, mapper, result, propertyMap);
    }

    private static void MapPropertyValue(
      ResolutionContext context,
      IMappingEngineRunner mapper,
      object mappedObject,
      PropertyMap propertyMap)
    {
      if (!propertyMap.CanResolveValue())
        return;
      ResolutionResult resolutionResult = propertyMap.ResolveValue(context);
      ResolutionContext memberContext = context.CreateMemberContext((TypeMap) null, resolutionResult.Value, (object) null, resolutionResult.Type, propertyMap);
      if (!propertyMap.ShouldAssignValue(memberContext))
        return;
      try
      {
        object obj = mapper.Map(memberContext);
        if (!propertyMap.CanBeSet)
          return;
        propertyMap.DestinationProperty.SetValue(mappedObject, obj);
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

    private static DataReaderMapper.CreateEnumerableAdapter GetDelegateToCreateEnumerableAdapter(
      Type elementType)
    {
      DataReaderMapper.CreateEnumerableAdapter enumerableAdapter1;
      if (DataReaderMapper._enumerableAdapterCache.TryGetValue(elementType, out enumerableAdapter1))
        return enumerableAdapter1;
      ConstructorInfo constructor = typeof (DataReaderMapper.EnumerableAdapter<>).MakeGenericType(elementType).GetConstructor(new Type[1]
      {
        typeof (IEnumerable)
      });
      ParameterExpression parameterExpression = Expression.Parameter(typeof (IEnumerable), "items");
      DataReaderMapper.CreateEnumerableAdapter enumerableAdapter2 = (DataReaderMapper.CreateEnumerableAdapter) Expression.Lambda(typeof (DataReaderMapper.CreateEnumerableAdapter), (Expression) Expression.New(constructor, (Expression) parameterExpression), parameterExpression).Compile();
      DataReaderMapper._enumerableAdapterCache[elementType] = enumerableAdapter2;
      return enumerableAdapter2;
    }

    private delegate object Build(IDataRecord dataRecord);

    private delegate object CreateEnumerableAdapter(IEnumerable items);

    private class BuilderKey
    {
      private readonly List<string> _dataRecordNames;
      private readonly Type _destinationType;

      public BuilderKey(Type destinationType, IDataRecord record)
      {
        this._destinationType = destinationType;
        this._dataRecordNames = new List<string>(record.FieldCount);
        for (int i = 0; i < record.FieldCount; ++i)
          this._dataRecordNames.Add(record.GetName(i));
      }

      public override int GetHashCode()
      {
        int hashCode = this._destinationType.GetHashCode();
        foreach (string dataRecordName in this._dataRecordNames)
          hashCode = hashCode * 37 + dataRecordName.GetHashCode();
        return hashCode;
      }

      public override bool Equals(object obj)
      {
        if (!(obj is DataReaderMapper.BuilderKey builderKey) || this._dataRecordNames.Count != builderKey._dataRecordNames.Count)
          return false;
        for (int index = 0; index < this._dataRecordNames.Count; ++index)
        {
          if (this._dataRecordNames[index] != builderKey._dataRecordNames[index])
            return false;
        }
        return true;
      }
    }

    private class EnumerableAdapter<Item> : IEnumerable<Item>, IEnumerable
    {
      private IEnumerable<Item> _items;

      public EnumerableAdapter(IEnumerable items) => this._items = items.Cast<Item>();

      public IEnumerator<Item> GetEnumerator() => this._items.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
