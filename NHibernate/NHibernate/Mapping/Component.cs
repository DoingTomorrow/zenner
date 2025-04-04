// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Component
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Tuple.Component;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Component : SimpleValue, IMetaAttributable
  {
    private readonly List<Property> properties = new List<Property>();
    private System.Type componentClass;
    private bool embedded;
    private Property parentProperty;
    private PersistentClass owner;
    private bool dynamic;
    private bool isKey;
    private string nodeName;
    private string roleName;
    private Dictionary<EntityMode, string> tuplizerImpls;
    private string componentClassName;
    private IType type;

    public int PropertySpan => this.properties.Count;

    public IEnumerable<Property> PropertyIterator => (IEnumerable<Property>) this.properties;

    public void AddProperty(Property p) => this.properties.Add(p);

    public override void AddColumn(Column column)
    {
      throw new NotSupportedException("Cant add a column to a component");
    }

    public override int ColumnSpan
    {
      get
      {
        int columnSpan = 0;
        foreach (Property property in this.PropertyIterator)
          columnSpan += property.ColumnSpan;
        return columnSpan;
      }
    }

    public override IEnumerable<ISelectable> ColumnIterator
    {
      get
      {
        List<IEnumerable<ISelectable>> enumerables = new List<IEnumerable<ISelectable>>();
        foreach (Property property in this.PropertyIterator)
          enumerables.Add(property.ColumnIterator);
        return (IEnumerable<ISelectable>) new JoinedEnumerable<ISelectable>(enumerables);
      }
    }

    public Component(PersistentClass owner)
      : base(owner.Table)
    {
      this.owner = owner;
    }

    public Component(Table table, PersistentClass owner)
      : base(table)
    {
      this.owner = owner;
    }

    public Component(Collection collection)
      : base(collection.CollectionTable)
    {
      this.owner = collection.Owner;
    }

    public Component(NHibernate.Mapping.Component component)
      : base(component.Table)
    {
      this.owner = component.Owner;
    }

    public override void SetTypeUsingReflection(
      string className,
      string propertyName,
      string accesorName)
    {
    }

    public bool IsEmbedded
    {
      get => this.embedded;
      set => this.embedded = value;
    }

    public bool IsDynamic
    {
      get => this.dynamic;
      set => this.dynamic = value;
    }

    public System.Type ComponentClass
    {
      get
      {
        if (this.componentClass == null)
        {
          try
          {
            this.componentClass = ReflectHelper.ClassForName(this.componentClassName);
          }
          catch (Exception ex)
          {
            if (!this.IsDynamic)
              throw new MappingException("component class not found: " + this.componentClassName, ex);
            return (System.Type) null;
          }
        }
        return this.componentClass;
      }
      set
      {
        this.componentClass = value;
        if (this.componentClass == null)
          return;
        this.componentClassName = this.componentClass.AssemblyQualifiedName;
      }
    }

    public PersistentClass Owner
    {
      get => this.owner;
      set => this.owner = value;
    }

    public Property ParentProperty
    {
      get => this.parentProperty;
      set => this.parentProperty = value;
    }

    public override bool[] ColumnInsertability
    {
      get
      {
        bool[] destinationArray = new bool[this.ColumnSpan];
        int destinationIndex = 0;
        foreach (Property property in this.PropertyIterator)
        {
          bool[] columnInsertability = property.Value.ColumnInsertability;
          if (property.IsInsertable)
            System.Array.Copy((System.Array) columnInsertability, 0, (System.Array) destinationArray, destinationIndex, columnInsertability.Length);
          destinationIndex += columnInsertability.Length;
        }
        return destinationArray;
      }
    }

    public override bool[] ColumnUpdateability
    {
      get
      {
        bool[] destinationArray = new bool[this.ColumnSpan];
        int destinationIndex = 0;
        foreach (Property property in this.PropertyIterator)
        {
          bool[] columnUpdateability = property.Value.ColumnUpdateability;
          if (property.IsUpdateable)
            System.Array.Copy((System.Array) columnUpdateability, 0, (System.Array) destinationArray, destinationIndex, columnUpdateability.Length);
          destinationIndex += columnUpdateability.Length;
        }
        return destinationArray;
      }
    }

    public string ComponentClassName
    {
      get => this.componentClassName;
      set
      {
        if ((this.componentClassName != null || value == null) && (this.componentClassName == null || this.componentClassName.Equals(value)))
          return;
        this.componentClass = (System.Type) null;
        this.componentClassName = value;
      }
    }

    public bool IsKey
    {
      get => this.isKey;
      set => this.isKey = value;
    }

    public string NodeName
    {
      get => this.nodeName;
      set => this.nodeName = value;
    }

    public string RoleName
    {
      get => this.roleName;
      set => this.roleName = value;
    }

    public Property GetProperty(string propertyName)
    {
      foreach (Property property in this.PropertyIterator)
      {
        if (property.Name.Equals(propertyName))
          return property;
      }
      throw new MappingException("component property not found: " + propertyName);
    }

    public virtual void AddTuplizer(EntityMode entityMode, string implClassName)
    {
      if (this.tuplizerImpls == null)
        this.tuplizerImpls = new Dictionary<EntityMode, string>();
      this.tuplizerImpls[entityMode] = implClassName;
    }

    public virtual string GetTuplizerImplClassName(EntityMode mode)
    {
      return this.tuplizerImpls == null ? (string) null : this.tuplizerImpls[mode];
    }

    public virtual IDictionary<EntityMode, string> TuplizerMap
    {
      get
      {
        return this.tuplizerImpls == null ? (IDictionary<EntityMode, string>) null : (IDictionary<EntityMode, string>) this.tuplizerImpls;
      }
    }

    public bool HasPocoRepresentation => this.componentClassName != null;

    public override IType Type
    {
      get
      {
        if (this.type == null)
          this.type = this.BuildType();
        return this.type;
      }
    }

    private IType BuildType()
    {
      ComponentMetamodel metamodel = new ComponentMetamodel(this);
      return this.IsEmbedded ? (IType) new EmbeddedComponentType(metamodel) : (IType) new ComponentType(metamodel);
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + StringHelper.CollectionToString((ICollection) this.properties) + (object) ')';
    }

    public IDictionary<string, MetaAttribute> MetaAttributes { get; set; }

    public MetaAttribute GetMetaAttribute(string attributeName)
    {
      if (this.MetaAttributes == null)
        return (MetaAttribute) null;
      MetaAttribute metaAttribute;
      this.MetaAttributes.TryGetValue(attributeName, out metaAttribute);
      return metaAttribute;
    }
  }
}
