// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ColumnInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class ColumnInstance : ColumnInspector, IColumnInstance, IColumnInspector, IInspector
  {
    private readonly ColumnMapping mapping;

    public ColumnInstance(Type parentType, ColumnMapping mapping)
      : base(parentType, mapping)
    {
      this.mapping = mapping;
    }

    public void Length(int length)
    {
      this.mapping.Set<int>((Expression<Func<ColumnMapping, int>>) (x => x.Length), 1, length);
    }

    public void Index(string indexname)
    {
      this.mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Index), 1, indexname);
    }

    public void Default(string defaultvalue)
    {
      this.mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Default), 1, defaultvalue);
    }
  }
}
