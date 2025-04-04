// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.JoinedSubclassInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Persister.Entity;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class JoinedSubclassInstance : 
    JoinedSubclassInspector,
    IJoinedSubclassInstance,
    IJoinedSubclassInspector,
    ISubclassInspectorBase,
    IInspector
  {
    private readonly SubclassMapping mapping;
    private bool nextBool = true;

    public JoinedSubclassInstance(SubclassMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IKeyInstance Key
    {
      get
      {
        if (this.mapping.Key == null)
          this.mapping.Set<KeyMapping>((Expression<Func<SubclassMapping, KeyMapping>>) (x => x.Key), 1, new KeyMapping());
        return (IKeyInstance) new KeyInstance(this.mapping.Key);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IJoinedSubclassInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IJoinedSubclassInstance) this;
      }
    }

    public void Abstract()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.Abstract), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Check(string constraint)
    {
      this.mapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Check), 1, constraint);
    }

    public void DynamicInsert()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.DynamicInsert), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void DynamicUpdate()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.DynamicUpdate), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void LazyLoad()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.Lazy), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Proxy(Type type)
    {
      this.mapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Proxy), 1, type.AssemblyQualifiedName);
    }

    public void Proxy<T>() => this.Proxy(typeof (T));

    public void Schema(string schema)
    {
      this.mapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Schema), 1, schema);
    }

    public void SelectBeforeUpdate()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.SelectBeforeUpdate), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Table(string tableName)
    {
      this.mapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.TableName), 1, tableName);
    }

    public void Subselect(string subselect)
    {
      this.mapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Subselect), 1, subselect);
    }

    public void Persister<T>() where T : IEntityPersister => this.Persister(typeof (T));

    public void Persister(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<SubclassMapping, TypeReference>>) (x => x.Persister), 1, new TypeReference(type));
    }

    public void Persister(string type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<SubclassMapping, TypeReference>>) (x => x.Persister), 1, new TypeReference(type));
    }

    public void BatchSize(int batchSize)
    {
      this.mapping.Set<int>((Expression<Func<SubclassMapping, int>>) (x => x.BatchSize), 1, batchSize);
    }
  }
}
