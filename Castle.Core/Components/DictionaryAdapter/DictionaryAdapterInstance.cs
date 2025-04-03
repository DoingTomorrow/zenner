// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryAdapterInstance
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class DictionaryAdapterInstance
  {
    private IDictionary extendedProperties;

    public DictionaryAdapterInstance(
      IDictionary dictionary,
      DictionaryAdapterMeta meta,
      PropertyDescriptor descriptor,
      IDictionaryAdapterFactory factory)
    {
      this.Dictionary = dictionary;
      this.Descriptor = descriptor;
      this.Factory = factory;
      this.Properties = meta.Properties;
      this.Initializers = meta.Initializers;
      this.MergeBehaviorOverrides(meta);
    }

    internal int? OldHashCode { get; set; }

    public IDictionary Dictionary { get; private set; }

    public PropertyDescriptor Descriptor { get; private set; }

    public IDictionaryAdapterFactory Factory { get; private set; }

    public IDictionaryInitializer[] Initializers { get; private set; }

    public IDictionary<string, PropertyDescriptor> Properties { get; private set; }

    public IDictionaryEqualityHashCodeStrategy EqualityHashCodeStrategy { get; set; }

    public IDictionaryCreateStrategy CreateStrategy { get; set; }

    public IDictionary ExtendedProperties
    {
      get
      {
        if (this.extendedProperties == null)
          this.extendedProperties = (IDictionary) new System.Collections.Generic.Dictionary<object, object>();
        return this.extendedProperties;
      }
    }

    private void MergeBehaviorOverrides(DictionaryAdapterMeta meta)
    {
      if (this.Descriptor == null)
        return;
      if (this.Descriptor is DictionaryDescriptor descriptor)
        this.Initializers = ((IEnumerable<IDictionaryInitializer>) this.Initializers).Prioritize<IDictionaryInitializer>((IEnumerable<IDictionaryInitializer>) descriptor.Initializers).ToArray<IDictionaryInitializer>();
      this.Properties = (IDictionary<string, PropertyDescriptor>) new System.Collections.Generic.Dictionary<string, PropertyDescriptor>();
      foreach (KeyValuePair<string, PropertyDescriptor> property in (IEnumerable<KeyValuePair<string, PropertyDescriptor>>) meta.Properties)
      {
        PropertyDescriptor source = property.Value;
        PropertyDescriptor propertyDescriptor = new PropertyDescriptor(source, false).AddKeyBuilders(source.KeyBuilders.Prioritize<IDictionaryKeyBuilder>((IEnumerable<IDictionaryKeyBuilder>) this.Descriptor.KeyBuilders)).AddGetters(source.Getters.Prioritize<IDictionaryPropertyGetter>((IEnumerable<IDictionaryPropertyGetter>) this.Descriptor.Getters)).AddSetters(source.Setters.Prioritize<IDictionaryPropertySetter>((IEnumerable<IDictionaryPropertySetter>) this.Descriptor.Setters));
        this.Properties.Add(property.Key, propertyDescriptor);
      }
    }
  }
}
