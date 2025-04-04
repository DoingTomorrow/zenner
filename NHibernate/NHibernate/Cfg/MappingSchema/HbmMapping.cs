// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [DesignerCategory("code")]
  [XmlRoot("hibernate-mapping", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmMapping : AbstractDecoratable
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("typedef")]
    public HbmTypedef[] typedef;
    [XmlElement("import")]
    public HbmImport[] import;
    [XmlElement("class", typeof (HbmClass))]
    [XmlElement("subclass", typeof (HbmSubclass))]
    [XmlElement("union-subclass", typeof (HbmUnionSubclass))]
    [XmlElement("joined-subclass", typeof (HbmJoinedSubclass))]
    public object[] Items;
    [XmlElement("resultset")]
    public HbmResultSet[] resultset;
    [XmlElement("query", typeof (HbmQuery))]
    [XmlElement("sql-query", typeof (HbmSqlQuery))]
    public object[] Items1;
    [XmlElement("filter-def")]
    public HbmFilterDef[] filterdef;
    [XmlElement("database-object")]
    public HbmDatabaseObject[] databaseobject;
    [XmlAttribute]
    public string schema;
    [XmlAttribute]
    public string catalog;
    [XmlAttribute("default-cascade")]
    [DefaultValue("none")]
    public string defaultcascade;
    [DefaultValue("property")]
    [XmlAttribute("default-access")]
    public string defaultaccess;
    [DefaultValue(true)]
    [XmlAttribute("default-lazy")]
    public bool defaultlazy;
    [XmlAttribute("auto-import")]
    [DefaultValue(true)]
    public bool autoimport;
    [XmlAttribute]
    public string @namespace;
    [XmlAttribute]
    public string assembly;

    public HbmMapping()
    {
      this.defaultcascade = "none";
      this.defaultaccess = "property";
      this.defaultlazy = true;
      this.autoimport = true;
    }

    public HbmDatabaseObject[] DatabaseObjects => this.databaseobject ?? new HbmDatabaseObject[0];

    public HbmFilterDef[] FilterDefinitions => this.filterdef ?? new HbmFilterDef[0];

    public HbmResultSet[] ResultSets => this.resultset ?? new HbmResultSet[0];

    public HbmTypedef[] TypeDefinitions => this.typedef ?? new HbmTypedef[0];

    public HbmImport[] Imports => this.import ?? new HbmImport[0];

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    public HbmClass[] RootClasses
    {
      get
      {
        return this.Items == null ? new HbmClass[0] : this.Items.OfType<HbmClass>().ToArray<HbmClass>();
      }
    }

    public HbmSubclass[] SubClasses
    {
      get
      {
        return this.Items == null ? new HbmSubclass[0] : this.Items.OfType<HbmSubclass>().ToArray<HbmSubclass>();
      }
    }

    public HbmJoinedSubclass[] JoinedSubclasses
    {
      get
      {
        return this.Items == null ? new HbmJoinedSubclass[0] : this.Items.OfType<HbmJoinedSubclass>().ToArray<HbmJoinedSubclass>();
      }
    }

    public HbmUnionSubclass[] UnionSubclasses
    {
      get
      {
        return this.Items == null ? new HbmUnionSubclass[0] : this.Items.OfType<HbmUnionSubclass>().ToArray<HbmUnionSubclass>();
      }
    }

    public HbmQuery[] HqlQueries
    {
      get
      {
        return this.Items1 == null ? new HbmQuery[0] : this.Items1.OfType<HbmQuery>().ToArray<HbmQuery>();
      }
    }

    public HbmSqlQuery[] SqlQueries
    {
      get
      {
        return this.Items1 == null ? new HbmSqlQuery[0] : this.Items1.OfType<HbmSqlQuery>().ToArray<HbmSqlQuery>();
      }
    }
  }
}
