// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.AccessInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using NHibernate.Properties;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class AccessInstance : IAccessInstance
  {
    private readonly Action<string> setter;

    public AccessInstance(Action<string> setter) => this.setter = setter;

    public void Property() => this.setter("property");

    public void Field() => this.setter("field");

    public void BackField() => this.setter("backfield");

    public void CamelCaseField() => this.CamelCaseField(CamelCasePrefix.None);

    public void CamelCaseField(CamelCasePrefix prefix)
    {
      this.setter("field.camelcase" + (object) prefix);
    }

    public void LowerCaseField() => this.LowerCaseField(LowerCasePrefix.None);

    public void LowerCaseField(LowerCasePrefix prefix)
    {
      this.setter("field.lowercase" + (object) prefix);
    }

    public void PascalCaseField(PascalCasePrefix prefix)
    {
      this.setter("field.pascalcase" + (object) prefix);
    }

    public void ReadOnlyProperty() => this.setter("nosetter");

    public void ReadOnlyPropertyThroughCamelCaseField()
    {
      this.ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.None);
    }

    public void ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix)
    {
      this.setter("nosetter.camelcase" + (object) prefix);
    }

    public void ReadOnlyPropertyThroughLowerCaseField()
    {
      this.ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix.None);
    }

    public void ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix)
    {
      this.setter("nosetter.lowercase" + (object) prefix);
    }

    public void ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix)
    {
      this.setter("nosetter.pascalcase" + (object) prefix);
    }

    public void Using(string propertyAccessorAssemblyQualifiedClassName)
    {
      this.setter(propertyAccessorAssemblyQualifiedClassName);
    }

    public void Using(Type propertyAccessorClassType)
    {
      this.Using(propertyAccessorClassType.AssemblyQualifiedName);
    }

    public void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
    {
      this.Using(typeof (TPropertyAccessorClass));
    }

    public void NoOp() => this.setter("noop");

    public void None() => this.setter("none");
  }
}
