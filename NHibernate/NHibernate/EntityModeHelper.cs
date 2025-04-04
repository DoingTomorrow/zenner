// Decompiled with JetBrains decompiler
// Type: NHibernate.EntityModeHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate
{
  public static class EntityModeHelper
  {
    public static string ToString(EntityMode entityMode)
    {
      switch (entityMode)
      {
        case EntityMode.Poco:
          return "poco";
        case EntityMode.Map:
          return "dynamic-map";
        case EntityMode.Xml:
          return "xml";
        default:
          return (string) null;
      }
    }

    public static EntityMode Parse(string name)
    {
      switch (name.ToLowerInvariant())
      {
        case "poco":
          return EntityMode.Poco;
        case "dynamic-map":
          return EntityMode.Map;
        case "xml":
          return EntityMode.Xml;
        default:
          return EntityMode.Poco;
      }
    }
  }
}
