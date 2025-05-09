﻿// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.BaseXmlCollectionWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public abstract class BaseXmlCollectionWriter : NullMappingModelVisitor
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    protected XmlDocument document;

    protected BaseXmlCollectionWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public override void Visit(KeyMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<KeyMapping>().Write(mapping));
    }

    public override void Visit(CacheMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<CacheMapping>().Write(mapping));
    }

    public override void Visit(ICollectionRelationshipMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ICollectionRelationshipMapping>().Write(mapping));
    }

    public override void Visit(CompositeElementMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<CompositeElementMapping>().Write(mapping));
    }

    public override void Visit(ElementMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ElementMapping>().Write(mapping));
    }

    public override void Visit(FilterMapping filterMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<FilterMapping>().Write(filterMapping));
    }

    protected void WriteBaseCollectionAttributes(XmlElement element, CollectionMapping mapping)
    {
      if (mapping.IsSpecified("Access"))
        element.WithAtt("access", mapping.Access);
      if (mapping.IsSpecified("BatchSize"))
        element.WithAtt("batch-size", mapping.BatchSize);
      if (mapping.IsSpecified("Cascade"))
        element.WithAtt("cascade", mapping.Cascade);
      if (mapping.IsSpecified("Check"))
        element.WithAtt("check", mapping.Check);
      if (mapping.IsSpecified("CollectionType") && mapping.CollectionType != TypeReference.Empty)
        element.WithAtt("collection-type", mapping.CollectionType);
      if (mapping.IsSpecified("Fetch"))
        element.WithAtt("fetch", mapping.Fetch);
      if (mapping.IsSpecified("Generic"))
        element.WithAtt("generic", mapping.Generic);
      if (mapping.IsSpecified("Inverse"))
        element.WithAtt("inverse", mapping.Inverse);
      if (mapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", mapping.Lazy.ToString().ToLowerInvariant());
      if (mapping.IsSpecified("Name"))
        element.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("OptimisticLock"))
        element.WithAtt("optimistic-lock", mapping.OptimisticLock);
      if (mapping.IsSpecified("Persister"))
        element.WithAtt("persister", mapping.Persister);
      if (mapping.IsSpecified("Schema"))
        element.WithAtt("schema", mapping.Schema);
      if (mapping.IsSpecified("TableName"))
        element.WithAtt("table", mapping.TableName);
      if (mapping.IsSpecified("Where"))
        element.WithAtt("where", mapping.Where);
      if (mapping.IsSpecified("Subselect"))
        element.WithAtt("subselect", mapping.Subselect);
      if (!mapping.IsSpecified("Mutable"))
        return;
      element.WithAtt("mutable", mapping.Mutable);
    }
  }
}
