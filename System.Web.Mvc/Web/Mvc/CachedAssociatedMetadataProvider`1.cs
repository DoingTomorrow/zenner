// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CachedAssociatedMetadataProvider`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Caching;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class CachedAssociatedMetadataProvider<TModelMetadata> : AssociatedMetadataProvider
    where TModelMetadata : ModelMetadata
  {
    private static ConcurrentDictionary<Type, string> _typeIds = new ConcurrentDictionary<Type, string>();
    private string _cacheKeyPrefix;
    private CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy()
    {
      SlidingExpiration = TimeSpan.FromMinutes(20.0)
    };
    private ObjectCache _prototypeCache;

    protected internal CacheItemPolicy CacheItemPolicy
    {
      get => this._cacheItemPolicy;
      set => this._cacheItemPolicy = value;
    }

    protected string CacheKeyPrefix
    {
      get
      {
        if (this._cacheKeyPrefix == null)
          this._cacheKeyPrefix = "MetadataPrototypes::" + this.GetType().GUID.ToString("B");
        return this._cacheKeyPrefix;
      }
    }

    protected internal ObjectCache PrototypeCache
    {
      get => this._prototypeCache ?? (ObjectCache) MemoryCache.Default;
      set => this._prototypeCache = value;
    }

    protected override sealed ModelMetadata CreateMetadata(
      IEnumerable<Attribute> attributes,
      Type containerType,
      Func<object> modelAccessor,
      Type modelType,
      string propertyName)
    {
      Type type = containerType;
      if ((object) type == null)
        type = modelType;
      string cacheKey = this.GetCacheKey(type, propertyName);
      if (!(this.PrototypeCache.Get(cacheKey) is TModelMetadata metadataPrototype))
      {
        metadataPrototype = this.CreateMetadataPrototype(attributes, containerType, modelType, propertyName);
        this.PrototypeCache.Add(cacheKey, (object) metadataPrototype, this.CacheItemPolicy);
      }
      return (ModelMetadata) this.CreateMetadataFromPrototype(metadataPrototype, modelAccessor);
    }

    protected abstract TModelMetadata CreateMetadataPrototype(
      IEnumerable<Attribute> attributes,
      Type containerType,
      Type modelType,
      string propertyName);

    protected abstract TModelMetadata CreateMetadataFromPrototype(
      TModelMetadata prototype,
      Func<object> modelAccessor);

    internal string GetCacheKey(Type type, string propertyName = null)
    {
      propertyName = propertyName ?? string.Empty;
      return this.CacheKeyPrefix + CachedAssociatedMetadataProvider<TModelMetadata>.GetTypeId(type) + propertyName;
    }

    public override sealed ModelMetadata GetMetadataForProperty(
      Func<object> modelAccessor,
      Type containerType,
      string propertyName)
    {
      return base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
    }

    protected override sealed ModelMetadata GetMetadataForProperty(
      Func<object> modelAccessor,
      Type containerType,
      PropertyDescriptor propertyDescriptor)
    {
      return base.GetMetadataForProperty(modelAccessor, containerType, propertyDescriptor);
    }

    public override sealed IEnumerable<ModelMetadata> GetMetadataForProperties(
      object container,
      Type containerType)
    {
      return base.GetMetadataForProperties(container, containerType);
    }

    public override sealed ModelMetadata GetMetadataForType(
      Func<object> modelAccessor,
      Type modelType)
    {
      return base.GetMetadataForType(modelAccessor, modelType);
    }

    private static string GetTypeId(Type type)
    {
      return CachedAssociatedMetadataProvider<TModelMetadata>._typeIds.GetOrAdd(type, (Func<Type, string>) (_ => Guid.NewGuid().ToString("B")));
    }
  }
}
