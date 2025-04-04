// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.ComponentType
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public class ComponentType
  {
    public static readonly ComponentType Component = new ComponentType("component");
    public static readonly ComponentType DynamicComponent = new ComponentType("dynamic-component");
    private readonly string elementName;

    private ComponentType(string elementName) => this.elementName = elementName;

    public string GetElementName() => this.elementName;

    public override bool Equals(object obj)
    {
      return obj is ComponentType && this.Equals(obj as ComponentType);
    }

    public bool Equals(ComponentType other)
    {
      return object.Equals((object) other.elementName, (object) this.elementName);
    }

    public override int GetHashCode()
    {
      return this.elementName == null ? 0 : this.elementName.GetHashCode();
    }
  }
}
