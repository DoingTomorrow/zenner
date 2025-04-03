// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.PropertyDescriptor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [DebuggerDisplay("{Property.DeclaringType.FullName,nq}.{PropertyName,nq}")]
  public class PropertyDescriptor : 
    IDictionaryKeyBuilder,
    IDictionaryPropertyGetter,
    IDictionaryPropertySetter,
    IDictionaryBehavior
  {
    private List<IDictionaryPropertyGetter> getters;
    private List<IDictionaryPropertySetter> setters;
    private List<IDictionaryKeyBuilder> keyBuilders;
    private IDictionary state;
    private static readonly object[] NoBehaviors = new object[0];
    private static readonly ICollection<IDictionaryKeyBuilder> NoKeysBuilders = (ICollection<IDictionaryKeyBuilder>) new IDictionaryKeyBuilder[0];
    private static readonly ICollection<IDictionaryPropertyGetter> NoHGetters = (ICollection<IDictionaryPropertyGetter>) new IDictionaryPropertyGetter[0];
    private static readonly ICollection<IDictionaryPropertySetter> NoSetters = (ICollection<IDictionaryPropertySetter>) new IDictionaryPropertySetter[0];

    public PropertyDescriptor() => this.Behaviors = PropertyDescriptor.NoBehaviors;

    public PropertyDescriptor(PropertyInfo property, object[] behaviors)
      : this()
    {
      this.Property = property;
      this.Behaviors = behaviors ?? PropertyDescriptor.NoBehaviors;
      this.IsDynamicProperty = typeof (IDynamicValue).IsAssignableFrom(property.PropertyType);
      this.ObtainTypeConverter();
    }

    public PropertyDescriptor(PropertyDescriptor source, bool copyBehaviors)
    {
      this.Property = source.Property;
      this.Behaviors = source.Behaviors;
      this.IsDynamicProperty = source.IsDynamicProperty;
      this.TypeConverter = source.TypeConverter;
      this.SuppressNotifications = source.SuppressNotifications;
      this.state = source.state;
      this.Fetch = source.Fetch;
      if (!copyBehaviors)
        return;
      this.keyBuilders = source.keyBuilders;
      this.getters = source.getters;
      this.setters = source.setters;
    }

    public int ExecutionOrder => 0;

    public string PropertyName => this.Property == null ? (string) null : this.Property.Name;

    public Type PropertyType => this.Property == null ? (Type) null : this.Property.PropertyType;

    public PropertyInfo Property { get; private set; }

    public bool IsDynamicProperty { get; private set; }

    public IDictionary State
    {
      get
      {
        if (this.state == null)
          this.state = (IDictionary) new Dictionary<object, object>();
        return this.state;
      }
    }

    public bool Fetch { get; set; }

    public bool SuppressNotifications { get; set; }

    public object[] Behaviors { get; private set; }

    public TypeConverter TypeConverter { get; private set; }

    public ICollection<IDictionaryKeyBuilder> KeyBuilders
    {
      get
      {
        return (ICollection<IDictionaryKeyBuilder>) this.keyBuilders ?? PropertyDescriptor.NoKeysBuilders;
      }
    }

    public ICollection<IDictionaryPropertySetter> Setters
    {
      get => (ICollection<IDictionaryPropertySetter>) this.setters ?? PropertyDescriptor.NoSetters;
    }

    public ICollection<IDictionaryPropertyGetter> Getters
    {
      get => (ICollection<IDictionaryPropertyGetter>) this.getters ?? PropertyDescriptor.NoHGetters;
    }

    public string GetKey(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      PropertyDescriptor descriptor)
    {
      if (this.keyBuilders != null)
      {
        foreach (IDictionaryKeyBuilder keyBuilder in this.keyBuilders)
          key = keyBuilder.GetKey(dictionaryAdapter, key, this);
      }
      return key;
    }

    public PropertyDescriptor AddKeyBuilder(params IDictionaryKeyBuilder[] builders)
    {
      return this.AddKeyBuilders((IEnumerable<IDictionaryKeyBuilder>) builders);
    }

    public PropertyDescriptor AddKeyBuilders(IEnumerable<IDictionaryKeyBuilder> builders)
    {
      if (builders != null)
      {
        if (this.keyBuilders == null)
          this.keyBuilders = new List<IDictionaryKeyBuilder>(builders);
        else
          this.keyBuilders.AddRange(builders);
      }
      return this;
    }

    public PropertyDescriptor CopyKeyBuilders(PropertyDescriptor other)
    {
      if (this.keyBuilders != null)
        other.AddKeyBuilders((IEnumerable<IDictionaryKeyBuilder>) this.keyBuilders);
      return this;
    }

    public PropertyDescriptor CopyKeyBuilders(
      PropertyDescriptor other,
      Func<IDictionaryKeyBuilder, bool> selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (this.keyBuilders != null)
        other.AddKeyBuilders(this.keyBuilders.Where<IDictionaryKeyBuilder>(selector));
      return this;
    }

    public object GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor descriptor,
      bool ifExists)
    {
      key = this.GetKey(dictionaryAdapter, key, descriptor);
      storedValue = storedValue ?? dictionaryAdapter.ReadProperty(key);
      if (this.getters != null)
      {
        foreach (IDictionaryPropertyGetter getter in this.getters)
          storedValue = getter.GetPropertyValue(dictionaryAdapter, key, storedValue, this, ifExists);
      }
      return storedValue;
    }

    public PropertyDescriptor AddGetter(params IDictionaryPropertyGetter[] getters)
    {
      return this.AddGetters((IEnumerable<IDictionaryPropertyGetter>) getters);
    }

    public PropertyDescriptor AddGetters(IEnumerable<IDictionaryPropertyGetter> gets)
    {
      if (gets != null)
      {
        if (this.getters == null)
          this.getters = new List<IDictionaryPropertyGetter>(gets);
        else
          this.getters.AddRange(gets);
      }
      return this;
    }

    public PropertyDescriptor CopyGetters(PropertyDescriptor other)
    {
      if (this.getters != null)
        other.AddGetters((IEnumerable<IDictionaryPropertyGetter>) this.getters);
      return this;
    }

    public PropertyDescriptor CopyGetters(
      PropertyDescriptor other,
      Func<IDictionaryPropertyGetter, bool> selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (this.getters != null)
        other.AddGetters(this.getters.Where<IDictionaryPropertyGetter>(selector));
      return this;
    }

    public bool SetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      ref object value,
      PropertyDescriptor descriptor)
    {
      bool flag = false;
      key = this.GetKey(dictionaryAdapter, key, descriptor);
      if (this.setters != null)
      {
        foreach (IDictionaryPropertySetter setter in this.setters)
        {
          if (!setter.SetPropertyValue(dictionaryAdapter, key, ref value, this))
            flag = true;
        }
      }
      if (!flag)
        dictionaryAdapter.StoreProperty(this, key, value);
      return !flag;
    }

    public PropertyDescriptor AddSetter(params IDictionaryPropertySetter[] setters)
    {
      return this.AddSetters((IEnumerable<IDictionaryPropertySetter>) setters);
    }

    public PropertyDescriptor AddSetters(IEnumerable<IDictionaryPropertySetter> sets)
    {
      if (sets != null)
      {
        if (this.setters == null)
          this.setters = new List<IDictionaryPropertySetter>(sets);
        else
          this.setters.AddRange(sets);
      }
      return this;
    }

    public PropertyDescriptor CopySetters(PropertyDescriptor other)
    {
      if (this.setters != null)
        other.AddSetters((IEnumerable<IDictionaryPropertySetter>) this.setters);
      return this;
    }

    public PropertyDescriptor CopySetters(
      PropertyDescriptor other,
      Func<IDictionaryPropertySetter, bool> selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (this.setters != null)
        other.AddSetters(this.setters.Where<IDictionaryPropertySetter>(selector));
      return this;
    }

    public PropertyDescriptor AddBehavior(params IDictionaryBehavior[] behaviors)
    {
      return this.AddBehaviors((IEnumerable<IDictionaryBehavior>) behaviors);
    }

    public PropertyDescriptor AddBehaviors(IEnumerable<IDictionaryBehavior> behaviors)
    {
      if (behaviors != null)
      {
        foreach (IDictionaryBehavior behavior in behaviors)
          this.InternalAddBehavior(behavior);
      }
      return this;
    }

    public PropertyDescriptor AddBehaviors(params IDictionaryBehaviorBuilder[] builders)
    {
      this.AddBehaviors(((IEnumerable<IDictionaryBehaviorBuilder>) builders).SelectMany<IDictionaryBehaviorBuilder, IDictionaryBehavior>((Func<IDictionaryBehaviorBuilder, IEnumerable<IDictionaryBehavior>>) (builder => builder.BuildBehaviors())));
      return this;
    }

    public virtual PropertyDescriptor CopyBehaviors(PropertyDescriptor other)
    {
      return this.CopyKeyBuilders(other).CopyGetters(other).CopySetters(other);
    }

    public PropertyDescriptor CopyBehaviors(
      PropertyDescriptor other,
      Func<IDictionaryBehavior, bool> selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return this.CopyKeyBuilders(other, (Func<IDictionaryKeyBuilder, bool>) (key => selector((IDictionaryBehavior) key))).CopyGetters(other, (Func<IDictionaryPropertyGetter, bool>) (getter => selector((IDictionaryBehavior) getter))).CopySetters(other, (Func<IDictionaryPropertySetter, bool>) (setter => selector((IDictionaryBehavior) setter)));
    }

    protected virtual void InternalAddBehavior(IDictionaryBehavior behavior)
    {
      if (behavior is IDictionaryKeyBuilder)
        this.AddKeyBuilder((IDictionaryKeyBuilder) behavior);
      if (behavior is IDictionaryPropertyGetter)
        this.AddGetter((IDictionaryPropertyGetter) behavior);
      if (!(behavior is IDictionaryPropertySetter))
        return;
      this.AddSetter((IDictionaryPropertySetter) behavior);
    }

    private void ObtainTypeConverter()
    {
      Type typeConverter = AttributesUtil.GetTypeConverter((MemberInfo) this.Property);
      if (typeConverter != null)
        this.TypeConverter = (TypeConverter) Activator.CreateInstance(typeConverter);
      else
        this.TypeConverter = TypeDescriptor.GetConverter(this.PropertyType);
    }
  }
}
