// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.AutoMappingsContainer
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace FluentNHibernate.Cfg
{
  public class AutoMappingsContainer : IEnumerable<AutoPersistenceModel>, IEnumerable
  {
    private readonly IList<AutoPersistenceModel> mappings = (IList<AutoPersistenceModel>) new List<AutoPersistenceModel>();
    private string exportPath;
    private TextWriter exportTextWriter;

    internal AutoMappingsContainer()
    {
    }

    public AutoMappingsContainer Add(Func<AutoPersistenceModel> model) => this.Add(model());

    public AutoMappingsContainer Add(AutoPersistenceModel model)
    {
      this.mappings.Add(model);
      this.WasUsed = true;
      return this;
    }

    public AutoMappingsContainer ExportTo(string path)
    {
      this.exportPath = path;
      return this;
    }

    public AutoMappingsContainer ExportTo(TextWriter textWriter)
    {
      this.exportTextWriter = textWriter;
      return this;
    }

    internal bool WasUsed { get; set; }

    internal void Apply(Configuration cfg, PersistenceModel model)
    {
      foreach (AutoPersistenceModel mapping in (IEnumerable<AutoPersistenceModel>) this.mappings)
      {
        if (!string.IsNullOrEmpty(this.exportPath))
          mapping.WriteMappingsTo(this.exportPath);
        if (this.exportTextWriter != null)
          mapping.WriteMappingsTo(this.exportTextWriter);
        mapping.ImportProviders(model);
        mapping.Configure(cfg);
      }
    }

    public IEnumerator<AutoPersistenceModel> GetEnumerator() => this.mappings.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
