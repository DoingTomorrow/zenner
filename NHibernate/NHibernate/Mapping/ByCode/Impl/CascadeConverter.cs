// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CascadeConverter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public static class CascadeConverter
  {
    public static string ToCascadeString(this Cascade source)
    {
      return source != Cascade.None ? string.Join(",", source.CascadeDefinitions().ToArray<string>()) : (string) null;
    }

    private static IEnumerable<string> CascadeDefinitions(this Cascade source)
    {
      if (source.Has(Cascade.All))
        yield return "all";
      if (source.Has(Cascade.Persist))
        yield return "save-update, persist";
      if (source.Has(Cascade.Refresh))
        yield return "refresh";
      if (source.Has(Cascade.Merge))
        yield return "merge";
      if (source.Has(Cascade.Remove))
        yield return "delete";
      if (source.Has(Cascade.Detach))
        yield return "evict";
      if (source.Has(Cascade.ReAttach))
        yield return "lock";
      if (source.Has(Cascade.DeleteOrphans))
        yield return "delete-orphan";
    }
  }
}
