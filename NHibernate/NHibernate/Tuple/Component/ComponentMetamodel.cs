// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Component.ComponentMetamodel
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Tuple.Component
{
  public class ComponentMetamodel
  {
    private readonly string role;
    private readonly bool isKey;
    private readonly int propertySpan;
    private readonly StandardProperty[] properties;
    private readonly Dictionary<string, int> propertyIndexes;
    private readonly ComponentEntityModeToTuplizerMapping tuplizerMapping;

    public ComponentMetamodel(NHibernate.Mapping.Component component)
    {
      this.role = component.RoleName;
      this.isKey = component.IsKey;
      this.propertySpan = component.PropertySpan;
      this.properties = new StandardProperty[this.PropertySpan];
      this.propertyIndexes = new Dictionary<string, int>(this.propertySpan);
      int index = 0;
      foreach (NHibernate.Mapping.Property property in component.PropertyIterator)
      {
        this.properties[index] = PropertyFactory.BuildStandardProperty(property, false);
        this.propertyIndexes[property.Name] = index;
        ++index;
      }
      this.tuplizerMapping = new ComponentEntityModeToTuplizerMapping(component);
    }

    public string Role => this.role;

    public bool IsKey => this.isKey;

    public int PropertySpan => this.propertySpan;

    public StandardProperty[] Properties => this.properties;

    public ComponentEntityModeToTuplizerMapping TuplizerMapping => this.tuplizerMapping;

    public StandardProperty GetProperty(int index)
    {
      return index >= 0 && index < this.propertySpan ? this.properties[index] : throw new ArgumentOutOfRangeException(nameof (index), string.Format("illegal index value for component property access [request={0}, span={1}]", (object) index, (object) this.propertySpan));
    }

    public int GetPropertyIndex(string propertyName)
    {
      int propertyIndex;
      if (!this.propertyIndexes.TryGetValue(propertyName, out propertyIndex))
        throw new HibernateException("component does not contain such a property [" + propertyName + "]");
      return propertyIndex;
    }

    public StandardProperty GetProperty(string propertyName)
    {
      return this.properties[this.GetPropertyIndex(propertyName)];
    }
  }
}
