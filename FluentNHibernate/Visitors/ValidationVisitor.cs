// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ValidationVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class ValidationVisitor : DefaultMappingModelVisitor
  {
    public ValidationVisitor() => this.Enabled = true;

    public override void ProcessClass(ClassMapping classMapping)
    {
      if (this.Enabled && classMapping.Id == null)
        throw new ValidationException(string.Format("The entity '{0}' doesn't have an Id mapped.", (object) classMapping.Type.Name), "Use the Id method to map your identity property. For example: Id(x => x.Id)", classMapping.Type);
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
      if (this.Enabled && mapping.OtherSide is CollectionMapping otherSide && mapping.Inverse && otherSide.Inverse)
        throw new ValidationException(string.Format("The relationship {0}.{1} to {2}.{3} has Inverse specified on both sides.", (object) mapping.ContainingEntityType.Name, (object) mapping.Name, (object) otherSide.ContainingEntityType.Name, (object) otherSide.Name), "Remove Inverse from one side of the relationship", mapping.ContainingEntityType);
    }

    public bool Enabled { get; set; }
  }
}
