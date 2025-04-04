// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.SchemaActionConverter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class SchemaActionConverter
  {
    public static string ToSchemaActionString(this SchemaAction source)
    {
      return source != SchemaAction.All ? string.Join(",", source.SchemaActionDefinitions().ToArray<string>()) : (string) null;
    }

    public static bool Has(this SchemaAction source, SchemaAction value)
    {
      return (source & value) == value;
    }

    private static IEnumerable<string> SchemaActionDefinitions(this SchemaAction source)
    {
      if (SchemaAction.None.Equals((object) source))
      {
        yield return "none";
      }
      else
      {
        if (source.Has(SchemaAction.Drop))
          yield return "drop";
        if (source.Has(SchemaAction.Update))
          yield return "update";
        if (source.Has(SchemaAction.Export))
          yield return "export";
        if (source.Has(SchemaAction.Validate))
          yield return "validate";
      }
    }
  }
}
