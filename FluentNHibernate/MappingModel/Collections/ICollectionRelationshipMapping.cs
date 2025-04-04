// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.ICollectionRelationshipMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  public interface ICollectionRelationshipMapping : IMapping
  {
    Type ChildType { get; }

    TypeReference Class { get; }

    string NotFound { get; }

    string EntityName { get; }
  }
}
