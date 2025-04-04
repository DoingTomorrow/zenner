// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.IComponentMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  public interface IComponentMapping : IPropertiesContainerMapping
  {
    string Class { get; }

    HbmParent Parent { get; }

    string EmbeddedNode { get; }

    string Name { get; }
  }
}
