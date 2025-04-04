// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.AccessStrategyBuilder`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Properties;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class AccessStrategyBuilder<T> : AccessStrategyBuilder
  {
    private readonly T parent;

    public AccessStrategyBuilder(T parent, Action<string> setter)
      : base(setter)
    {
      this.parent = parent;
    }

    public T Property()
    {
      base.Property();
      return this.parent;
    }

    public T Field()
    {
      base.Field();
      return this.parent;
    }

    public T BackingField()
    {
      base.BackingField();
      return this.parent;
    }

    public T ReadOnly()
    {
      base.ReadOnly();
      return this.parent;
    }

    public T CamelCaseField()
    {
      base.CamelCaseField();
      return this.parent;
    }

    public T CamelCaseField(Prefix prefix)
    {
      base.CamelCaseField(prefix);
      return this.parent;
    }

    public T LowerCaseField()
    {
      base.LowerCaseField();
      return this.parent;
    }

    public T LowerCaseField(Prefix prefix)
    {
      base.LowerCaseField(prefix);
      return this.parent;
    }

    public T PascalCaseField(Prefix prefix)
    {
      base.PascalCaseField(prefix);
      return this.parent;
    }

    public T ReadOnlyPropertyThroughCamelCaseField()
    {
      base.ReadOnlyPropertyThroughCamelCaseField();
      return this.parent;
    }

    public T ReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
    {
      base.ReadOnlyPropertyThroughCamelCaseField(prefix);
      return this.parent;
    }

    public T ReadOnlyPropertyThroughLowerCaseField()
    {
      base.ReadOnlyPropertyThroughLowerCaseField();
      return this.parent;
    }

    public T ReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
    {
      base.ReadOnlyPropertyThroughLowerCaseField(prefix);
      return this.parent;
    }

    public T ReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
    {
      base.ReadOnlyPropertyThroughPascalCaseField(prefix);
      return this.parent;
    }

    public T Using(string propertyAccessorAssemblyQualifiedClassName)
    {
      base.Using(propertyAccessorAssemblyQualifiedClassName);
      return this.parent;
    }

    public T Using(Type propertyAccessorClassType)
    {
      base.Using(propertyAccessorClassType);
      return this.parent;
    }

    public T Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
    {
      base.Using<TPropertyAccessorClass>();
      return this.parent;
    }

    public T NoOp()
    {
      this.setValue("noop");
      return this.parent;
    }

    public T None()
    {
      this.setValue("none");
      return this.parent;
    }
  }
}
