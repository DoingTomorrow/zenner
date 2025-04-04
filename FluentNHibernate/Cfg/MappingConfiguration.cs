// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.MappingConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Diagnostics;
using NHibernate.Cfg;

#nullable disable
namespace FluentNHibernate.Cfg
{
  public class MappingConfiguration
  {
    private bool mergeMappings;
    private readonly IDiagnosticLogger logger;
    private PersistenceModel model;

    public MappingConfiguration()
      : this((IDiagnosticLogger) new NullDiagnosticsLogger())
    {
    }

    public MappingConfiguration(IDiagnosticLogger logger)
    {
      this.logger = logger;
      this.FluentMappings = new FluentMappingsContainer();
      this.AutoMappings = new AutoMappingsContainer();
      this.HbmMappings = new HbmMappingsContainer();
      this.UsePersistenceModel(new PersistenceModel());
      this.model.SetLogger(logger);
    }

    public MappingConfiguration UsePersistenceModel(PersistenceModel persistenceModel)
    {
      this.model = persistenceModel;
      return this;
    }

    public FluentMappingsContainer FluentMappings { get; private set; }

    public AutoMappingsContainer AutoMappings { get; private set; }

    public HbmMappingsContainer HbmMappings { get; private set; }

    public bool WasUsed
    {
      get => this.FluentMappings.WasUsed || this.AutoMappings.WasUsed || this.HbmMappings.WasUsed;
    }

    public void Apply(Configuration cfg)
    {
      foreach (PersistenceModel autoMapping in this.AutoMappings)
        autoMapping.SetLogger(this.logger);
      if (this.mergeMappings)
      {
        foreach (PersistenceModel autoMapping in this.AutoMappings)
          autoMapping.MergeMappings = true;
        this.model.MergeMappings = true;
      }
      this.HbmMappings.Apply(cfg);
      this.FluentMappings.Apply(this.model);
      this.AutoMappings.Apply(cfg, this.model);
      this.model.Configure(cfg);
    }

    public MappingConfiguration MergeMappings()
    {
      this.mergeMappings = true;
      return this;
    }
  }
}
