// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryAdapterMeta
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [DebuggerDisplay("Type: {Type.FullName,nq}")]
  public class DictionaryAdapterMeta
  {
    private IDictionary extendedProperties;

    public DictionaryAdapterMeta(
      Type type,
      IDictionaryInitializer[] initializers,
      IDictionaryMetaInitializer[] metaInitializers,
      object[] behaviors,
      IDictionary<string, PropertyDescriptor> properties,
      DictionaryDescriptor descriptor,
      IDictionaryAdapterFactory factory)
    {
      this.Type = type;
      this.Initializers = initializers;
      this.MetaInitializers = metaInitializers;
      this.Behaviors = behaviors;
      this.Properties = properties;
      this.InitializeMeta(factory, descriptor);
    }

    public Type Type { get; private set; }

    public object[] Behaviors { get; private set; }

    public IDictionaryInitializer[] Initializers { get; private set; }

    public IDictionaryMetaInitializer[] MetaInitializers { get; private set; }

    public IDictionary<string, PropertyDescriptor> Properties { get; private set; }

    public IDictionary ExtendedProperties
    {
      get
      {
        if (this.extendedProperties == null)
          this.extendedProperties = (IDictionary) new Dictionary<object, object>();
        return this.extendedProperties;
      }
    }

    private void InitializeMeta(IDictionaryAdapterFactory factory, DictionaryDescriptor descriptor)
    {
      if (descriptor != null)
        this.MetaInitializers = ((IEnumerable<IDictionaryMetaInitializer>) this.MetaInitializers).Prioritize<IDictionaryMetaInitializer>((IEnumerable<IDictionaryMetaInitializer>) descriptor.MetaInitializers).ToArray<IDictionaryMetaInitializer>();
      foreach (IDictionaryMetaInitializer metaInitializer in this.MetaInitializers)
        metaInitializer.Initialize(factory, this);
    }
  }
}
