// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ICascadeInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface ICascadeInstance
  {
    void All();

    void None();

    void SaveUpdate();

    void Delete();

    void Merge();
  }
}
