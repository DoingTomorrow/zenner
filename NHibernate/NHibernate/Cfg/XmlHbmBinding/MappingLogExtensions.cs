// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.MappingLogExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping;
using System;
using System.Linq;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public static class MappingLogExtensions
  {
    public static void LogMapped(this Property property, IInternalLogger log)
    {
      if (!log.IsDebugEnabled)
        return;
      string message = "Mapped property: " + property.Name;
      string str = string.Join(",", property.Value.ColumnIterator.Select<ISelectable, string>((Func<ISelectable, string>) (c => c.Text)).ToArray<string>());
      if (str.Length > 0)
        message = message + " -> " + str;
      if (property.Type != null)
        message = message + ", type: " + property.Type.Name;
      log.Debug((object) message);
    }
  }
}
