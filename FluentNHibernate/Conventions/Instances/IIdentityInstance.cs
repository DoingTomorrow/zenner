// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IIdentityInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IIdentityInstance : 
    IIdentityInspector,
    IExposedThroughPropertyInspector,
    IIdentityInspectorBase,
    IInspector
  {
    void Column(string column);

    void UnsavedValue(string unsavedValue);

    void Length(int length);

    void CustomType(string type);

    void CustomType(Type type);

    void CustomType<T>();

    IAccessInstance Access { get; }

    IGeneratorInstance GeneratedBy { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IIdentityInstance Not { get; }

    void Precision(int precision);

    void Scale(int scale);

    void Nullable();

    void Unique();

    void UniqueKey(string columns);

    void CustomSqlType(string sqlType);

    void Index(string index);

    void Check(string constraint);

    void Default(object value);
  }
}
