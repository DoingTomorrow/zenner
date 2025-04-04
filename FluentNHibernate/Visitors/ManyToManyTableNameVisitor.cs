// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ManyToManyTableNameVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class ManyToManyTableNameVisitor : DefaultMappingModelVisitor
  {
    public override void ProcessCollection(CollectionMapping mapping)
    {
      if (!(mapping.Relationship is ManyToManyMapping))
        return;
      if (mapping.OtherSide == null)
      {
        mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.TableName), 0, mapping.ChildType.Name + "To" + mapping.ContainingEntityType.Name);
      }
      else
      {
        CollectionMapping otherSide = (CollectionMapping) mapping.OtherSide;
        string str = mapping.TableName ?? otherSide.TableName ?? otherSide.Member.Name + "To" + mapping.Member.Name;
        mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.TableName), 0, str);
        otherSide.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.TableName), 0, str);
      }
    }
  }
}
