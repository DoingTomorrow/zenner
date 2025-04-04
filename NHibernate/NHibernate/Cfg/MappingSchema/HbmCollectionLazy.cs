// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmCollectionLazy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [XmlType(Namespace = "urn:nhibernate-mapping-2.2")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [Serializable]
  public enum HbmCollectionLazy
  {
    [XmlEnum("true")] True,
    [XmlEnum("false")] False,
    [XmlEnum("extra")] Extra,
  }
}
