// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.BindMappingEventArgs
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Cfg
{
  public class BindMappingEventArgs : EventArgs
  {
    public BindMappingEventArgs(NHibernate.Dialect.Dialect dialect, HbmMapping mapping, string fileName)
    {
      this.Dialect = dialect;
      this.Mapping = mapping;
      this.FileName = fileName;
    }

    public NHibernate.Dialect.Dialect Dialect { get; private set; }

    public HbmMapping Mapping { get; private set; }

    public string FileName { get; private set; }
  }
}
