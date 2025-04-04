// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ExtendsQueueEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Xml;

#nullable disable
namespace NHibernate.Cfg
{
  public class ExtendsQueueEntry
  {
    private readonly string explicitName;
    private readonly string mappingPackage;
    private readonly XmlDocument document;

    public ExtendsQueueEntry(string explicitName, string mappingPackage, XmlDocument document)
    {
      this.explicitName = explicitName;
      this.mappingPackage = mappingPackage;
      this.document = document;
    }

    public string ExplicitName => this.explicitName;

    public string MappingPackage => this.mappingPackage;

    public XmlDocument Document => this.document;
  }
}
