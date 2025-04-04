// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IAccessInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using NHibernate.Properties;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IAccessInstance
  {
    void Property();

    void Field();

    void BackField();

    void CamelCaseField();

    void CamelCaseField(CamelCasePrefix prefix);

    void LowerCaseField();

    void LowerCaseField(LowerCasePrefix prefix);

    void PascalCaseField(PascalCasePrefix prefix);

    void ReadOnlyProperty();

    void ReadOnlyPropertyThroughCamelCaseField();

    void ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix);

    void ReadOnlyPropertyThroughLowerCaseField();

    void ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix);

    void ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix);

    void Using(string propertyAccessorAssemblyQualifiedClassName);

    void Using(Type propertyAccessorClassType);

    void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor;

    void NoOp();

    void None();
  }
}
