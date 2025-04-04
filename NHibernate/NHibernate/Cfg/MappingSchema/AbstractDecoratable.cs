// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.AbstractDecoratable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [Serializable]
  public abstract class AbstractDecoratable : IDecoratable
  {
    private static readonly IDictionary<string, MetaAttribute> EmptyMetaData = (IDictionary<string, MetaAttribute>) new CollectionHelper.EmptyMapClass<string, MetaAttribute>();
    [XmlIgnore]
    [NonSerialized]
    private IDictionary<string, MetaAttribute> mappedMetaData;
    [XmlIgnore]
    [NonSerialized]
    private IDictionary<string, MetaAttribute> inheritableMetaData;

    [XmlIgnore]
    public virtual IDictionary<string, MetaAttribute> MappedMetaData
    {
      get
      {
        if (this.mappedMetaData == null)
          this.CreateMappedMetadata(this.Metadatas);
        return this.mappedMetaData;
      }
    }

    [XmlIgnore]
    public IDictionary<string, MetaAttribute> InheritableMetaData
    {
      get
      {
        if (this.mappedMetaData == null)
          this.CreateMappedMetadata(this.Metadatas);
        return this.inheritableMetaData;
      }
    }

    protected void CreateMappedMetadata(HbmMeta[] metadatas)
    {
      if (metadatas == null)
      {
        this.mappedMetaData = AbstractDecoratable.EmptyMetaData;
        this.inheritableMetaData = AbstractDecoratable.EmptyMetaData;
      }
      else
      {
        this.mappedMetaData = (IDictionary<string, MetaAttribute>) new Dictionary<string, MetaAttribute>(10);
        this.inheritableMetaData = (IDictionary<string, MetaAttribute>) new Dictionary<string, MetaAttribute>(10);
        foreach (HbmMeta metadata in metadatas)
        {
          MetaAttribute metaAttribute;
          if (!this.mappedMetaData.TryGetValue(metadata.attribute, out metaAttribute))
          {
            metaAttribute = new MetaAttribute(metadata.attribute);
            this.mappedMetaData[metadata.attribute] = metaAttribute;
            if (metadata.inherit)
              this.inheritableMetaData[metadata.attribute] = metaAttribute;
          }
          if (metadata.Text != null)
            metaAttribute.AddValue(string.Concat(metadata.Text));
        }
      }
    }

    protected abstract HbmMeta[] Metadatas { get; }
  }
}
