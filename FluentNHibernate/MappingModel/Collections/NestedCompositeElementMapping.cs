// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.NestedCompositeElementMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  [Serializable]
  public class NestedCompositeElementMapping : CompositeElementMapping
  {
    private readonly AttributeStore attributes;

    public NestedCompositeElementMapping()
      : this(new AttributeStore())
    {
    }

    public NestedCompositeElementMapping(AttributeStore attributes)
      : base(attributes)
    {
      this.attributes = attributes;
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public void Set<T>(
      Expression<Func<NestedCompositeElementMapping, T>> expression,
      int layer,
      T value)
    {
      this.Set(expression.ToMember<NestedCompositeElementMapping, T>().Name, layer, (object) value);
    }
  }
}
