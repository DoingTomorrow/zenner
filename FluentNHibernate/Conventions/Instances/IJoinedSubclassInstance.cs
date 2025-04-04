// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IJoinedSubclassInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using NHibernate.Persister.Entity;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IJoinedSubclassInstance : 
    IJoinedSubclassInspector,
    ISubclassInspectorBase,
    IInspector
  {
    IKeyInstance Key { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IJoinedSubclassInstance Not { get; }

    void Abstract();

    void Check(string constraint);

    void DynamicInsert();

    void DynamicUpdate();

    void LazyLoad();

    void Proxy(Type type);

    void Proxy<T>();

    void Schema(string schema);

    void SelectBeforeUpdate();

    void Table(string tableName);

    void Subselect(string subselect);

    void Persister<T>() where T : IEntityPersister;

    void Persister(Type type);

    void Persister(string type);

    void BatchSize(int batchSize);
  }
}
