// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IPropertyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IPropertyInstance : 
    IPropertyInspector,
    IReadOnlyInspector,
    IExposedThroughPropertyInspector,
    IInspector,
    IAccessInspector,
    IInsertInstance,
    IUpdateInstance,
    IReadOnlyInstance,
    INullableInstance
  {
    IAccessInstance Access { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IPropertyInstance Not { get; }

    void CustomType<T>();

    void CustomType<T>(string columnPrefix);

    void CustomType(TypeReference type);

    void CustomType(TypeReference type, string columnPrefix);

    void CustomType(Type type);

    void CustomType(Type type, string columnPrefix);

    void CustomType(string type);

    void CustomType(string type, string columnPrefix);

    void CustomSqlType(string sqlType);

    void Precision(int precision);

    void Scale(int scale);

    void Default(string value);

    void Unique();

    void UniqueKey(string keyName);

    void Column(string columnName);

    void Formula(string formula);

    IGeneratedInstance Generated { get; }

    void OptimisticLock();

    void Length(int length);

    void LazyLoad();

    void Index(string value);

    void Check(string constraint);
  }
}
