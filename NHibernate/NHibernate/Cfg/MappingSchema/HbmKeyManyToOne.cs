// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmKeyManyToOne
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [XmlRoot("key-many-to-one", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmKeyManyToOne : 
    AbstractDecoratable,
    IColumnsMapping,
    IRelationship,
    IEntityPropertyMapping,
    IDecoratable
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("column")]
    public HbmColumn[] column;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    public string @class;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute("column")]
    public string column1;
    [XmlAttribute("foreign-key")]
    public string foreignkey;
    [XmlAttribute]
    public HbmRestrictedLaziness lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [XmlAttribute("not-found")]
    [DefaultValue(HbmNotFoundMode.Exception)]
    public HbmNotFoundMode notfound;

    public HbmKeyManyToOne() => this.notfound = HbmNotFoundMode.Exception;

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get => (IEnumerable<HbmColumn>) this.column ?? this.AsColumns();
    }

    private IEnumerable<HbmColumn> AsColumns()
    {
      if (!string.IsNullOrEmpty(this.column1))
        yield return new HbmColumn() { name = this.column1 };
    }

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    public HbmRestrictedLaziness? Lazy
    {
      get
      {
        return !this.lazySpecified ? new HbmRestrictedLaziness?() : new HbmRestrictedLaziness?(this.lazy);
      }
    }

    public string EntityName => this.entityname;

    public string Class => this.@class;

    public HbmNotFoundMode NotFoundMode => this.notfound;

    public string Name => this.name;

    public bool IsLazyProperty => false;

    public string Access => this.access;

    public bool OptimisticLock => false;
  }
}
