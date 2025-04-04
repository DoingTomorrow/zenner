// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.RelationshipKeyPairingVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class RelationshipKeyPairingVisitor : DefaultMappingModelVisitor
  {
    public override void ProcessManyToOne(ManyToOneMapping thisSide)
    {
      if (thisSide.OtherSide == null)
        return;
      CollectionMapping otherSide = (CollectionMapping) thisSide.OtherSide;
      if (thisSide.ContainingEntityType != otherSide.ContainingEntityType)
        return;
      otherSide.Key.MakeColumnsEmpty(0);
      thisSide.Columns.Each<ColumnMapping>((Action<ColumnMapping>) (x => otherSide.Key.AddColumn(0, x.Clone())));
    }
  }
}
