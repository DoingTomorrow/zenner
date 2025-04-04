// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlWriterFactory
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Infrastructure;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public static class XmlWriterFactory
  {
    private static readonly Container Container = (Container) new XmlWriterContainer();

    public static IXmlWriter<HibernateMapping> CreateHibernateMappingWriter()
    {
      return XmlWriterFactory.Container.Resolve<IXmlWriter<HibernateMapping>>();
    }
  }
}
