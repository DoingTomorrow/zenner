// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ComponentColumnPrefixVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class ComponentColumnPrefixVisitor : DefaultMappingModelVisitor
  {
    private Stack<string> prefixes = new Stack<string>();

    public override void Visit(IComponentMapping mapping)
    {
      this.StorePrefix(mapping);
      base.Visit(mapping);
      this.ResetPrefix();
    }

    public override void ProcessColumn(ColumnMapping columnMapping)
    {
      if (!this.prefixes.Any<string>())
        return;
      columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 2, this.GetPrefix() + columnMapping.Name);
    }

    private string GetPrefix()
    {
      return string.Join("", this.prefixes.Reverse<string>().ToArray<string>());
    }

    private void StorePrefix(IComponentMapping mapping)
    {
      if (!mapping.HasColumnPrefix)
        return;
      this.prefixes.Push(mapping.ColumnPrefix.Replace("{property}", mapping.Member.Name));
    }

    private void ResetPrefix()
    {
      if (this.prefixes.Count <= 0)
        return;
      this.prefixes.Pop();
    }
  }
}
