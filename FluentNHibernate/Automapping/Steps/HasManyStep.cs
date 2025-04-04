// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.HasManyStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class HasManyStep : IAutomappingStep
  {
    private readonly SimpleTypeCollectionStep simpleTypeCollectionStepStep;
    private readonly CollectionStep collectionStep;

    public HasManyStep(IAutomappingConfiguration cfg)
    {
      this.simpleTypeCollectionStepStep = new SimpleTypeCollectionStep(cfg);
      this.collectionStep = new CollectionStep(cfg);
    }

    public bool ShouldMap(Member member)
    {
      return this.simpleTypeCollectionStepStep.ShouldMap(member) || this.collectionStep.ShouldMap(member);
    }

    public void Map(ClassMappingBase classMap, Member member)
    {
      if (member.DeclaringType != classMap.Type)
        return;
      if (this.simpleTypeCollectionStepStep.ShouldMap(member))
      {
        this.simpleTypeCollectionStepStep.Map(classMap, member);
      }
      else
      {
        if (!this.collectionStep.ShouldMap(member))
          return;
        this.collectionStep.Map(classMap, member);
      }
    }
  }
}
