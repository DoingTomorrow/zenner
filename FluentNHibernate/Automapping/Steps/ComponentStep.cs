// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.ComponentStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class ComponentStep : IAutomappingStep
  {
    private readonly IAutomappingConfiguration cfg;

    public ComponentStep(IAutomappingConfiguration cfg) => this.cfg = cfg;

    public bool ShouldMap(Member member) => this.cfg.IsComponent(member.PropertyType);

    public void Map(ClassMappingBase classMap, Member member)
    {
      ReferenceComponentMapping componentMapping = new ReferenceComponentMapping(ComponentType.Component, member, member.PropertyType, classMap.Type, this.cfg.GetComponentColumnPrefix(member));
      classMap.AddComponent((IComponentMapping) componentMapping);
    }
  }
}
