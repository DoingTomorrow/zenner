// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IVersionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IVersionInstance : IVersionInspector, IInspector
  {
    IAccessInstance Access { get; }

    IGeneratedInstance Generated { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IVersionInstance Not { get; }

    void Column(string columnName);

    void UnsavedValue(string unsavedValue);

    void Length(int length);

    void Precision(int precision);

    void Scale(int scale);

    void Nullable();

    void Unique();

    void UniqueKey(string keyColumns);

    void CustomSqlType(string sqlType);

    void Index(string index);

    void Check(string constraint);

    void Default(object value);

    void CustomType<T>();

    void CustomType(Type type);

    void CustomType(string type);
  }
}
