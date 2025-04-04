// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Property
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Properties;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Property : IMetaAttributable
  {
    private string name;
    private IValue propertyValue;
    private string cascade;
    private bool updateable = true;
    private bool insertable = true;
    private bool selectable = true;
    private string propertyAccessorName;
    private bool optional;
    private string nodeName;
    private IDictionary<string, MetaAttribute> metaAttributes;
    private PersistentClass persistentClass;
    private bool isOptimisticLocked;
    private PropertyGeneration generation;
    private bool isLazy;
    private bool isNaturalIdentifier;

    public Property()
    {
    }

    public Property(IValue propertyValue) => this.propertyValue = propertyValue;

    public IType Type => this.propertyValue.Type;

    public int ColumnSpan => this.propertyValue.ColumnSpan;

    public IEnumerable<ISelectable> ColumnIterator => this.propertyValue.ColumnIterator;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public bool IsComposite => this.propertyValue is Component;

    public IValue Value
    {
      get => this.propertyValue;
      set => this.propertyValue = value;
    }

    public CascadeStyle CascadeStyle
    {
      get
      {
        IType type1 = this.propertyValue.Type;
        if (type1.IsComponentType && !type1.IsAnyType)
        {
          IAbstractComponentType type2 = (IAbstractComponentType) this.propertyValue.Type;
          int length = type2.Subtypes.Length;
          for (int i = 0; i < length; ++i)
          {
            if (type2.GetCascadeStyle(i) != CascadeStyle.None)
              return CascadeStyle.All;
          }
          return CascadeStyle.None;
        }
        if (string.IsNullOrEmpty(this.cascade) || this.cascade.Equals("none"))
          return CascadeStyle.None;
        string[] strArray = this.cascade.Split(',');
        if (strArray.Length == 0)
          return CascadeStyle.None;
        CascadeStyle[] styles = new CascadeStyle[strArray.Length];
        int num = 0;
        foreach (string str in strArray)
          styles[num++] = CascadeStyle.GetCascadeStyle(str.ToLowerInvariant().Trim());
        return strArray.Length == 1 ? styles[0] : (CascadeStyle) new CascadeStyle.MultipleCascadeStyle(styles);
      }
    }

    public string Cascade
    {
      get => this.cascade;
      set => this.cascade = value;
    }

    public bool IsUpdateable
    {
      get
      {
        bool[] columnUpdateability = this.propertyValue.ColumnUpdateability;
        return this.updateable && !ArrayHelper.IsAllFalse(columnUpdateability);
      }
      set => this.updateable = value;
    }

    public bool IsInsertable
    {
      get
      {
        bool[] columnInsertability = this.propertyValue.ColumnInsertability;
        if (!this.insertable)
          return false;
        return columnInsertability.Length == 0 || !ArrayHelper.IsAllFalse(columnInsertability);
      }
      set => this.insertable = value;
    }

    public bool IsNullable => this.propertyValue == null || this.propertyValue.IsNullable;

    public bool IsOptional
    {
      get => this.optional || this.IsNullable;
      set => this.optional = value;
    }

    public string PropertyAccessorName
    {
      get => this.propertyAccessorName;
      set => this.propertyAccessorName = value;
    }

    public IGetter GetGetter(System.Type clazz)
    {
      return this.PropertyAccessor.GetGetter(clazz, this.name);
    }

    public ISetter GetSetter(System.Type clazz)
    {
      return this.PropertyAccessor.GetSetter(clazz, this.name);
    }

    protected virtual IPropertyAccessor PropertyAccessor
    {
      get => PropertyAccessorFactory.GetPropertyAccessor(this.PropertyAccessorName);
    }

    public virtual string GetAccessorPropertyName(EntityMode mode)
    {
      return mode != EntityMode.Xml ? this.Name : this.nodeName;
    }

    public virtual bool IsBasicPropertyAccessor
    {
      get => this.PropertyAccessor.CanAccessThroughReflectionOptimizer;
    }

    public IDictionary<string, MetaAttribute> MetaAttributes
    {
      get => this.metaAttributes;
      set => this.metaAttributes = value;
    }

    public MetaAttribute GetMetaAttribute(string attributeName)
    {
      if (this.metaAttributes == null)
        return (MetaAttribute) null;
      MetaAttribute metaAttribute;
      this.metaAttributes.TryGetValue(attributeName, out metaAttribute);
      return metaAttribute;
    }

    public bool IsValid(IMapping mapping) => this.Value.IsValid(mapping);

    public string NullValue
    {
      get
      {
        return this.propertyValue is SimpleValue ? ((SimpleValue) this.propertyValue).NullValue : (string) null;
      }
    }

    public PersistentClass PersistentClass
    {
      get => this.persistentClass;
      set => this.persistentClass = value;
    }

    public bool IsSelectable
    {
      get => this.selectable;
      set => this.selectable = value;
    }

    public bool IsOptimisticLocked
    {
      get => this.isOptimisticLocked;
      set => this.isOptimisticLocked = value;
    }

    public PropertyGeneration Generation
    {
      get => this.generation;
      set => this.generation = value;
    }

    public bool IsLazy
    {
      get => this.isLazy;
      set => this.isLazy = value;
    }

    public virtual bool BackRef => false;

    public bool IsNaturalIdentifier
    {
      get => this.isNaturalIdentifier;
      set => this.isNaturalIdentifier = value;
    }

    public string NodeName
    {
      get => this.nodeName;
      set => this.nodeName = value;
    }

    public bool UnwrapProxy { get; set; }

    public bool IsEntityRelation => this.Value is ToOne;
  }
}
