// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.KeyPropertyPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class KeyPropertyPart
  {
    private readonly KeyPropertyMapping mapping;

    public KeyPropertyPart(KeyPropertyMapping mapping)
    {
      this.mapping = mapping;
      this.Access = new AccessStrategyBuilder<KeyPropertyPart>(this, (Action<string>) (value => mapping.Set<string>((Expression<Func<KeyPropertyMapping, string>>) (x => x.Access), 2, value)));
    }

    public KeyPropertyPart ColumnName(string columnName)
    {
      ColumnMapping mapping = new ColumnMapping();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 2, columnName);
      this.mapping.AddColumn(mapping);
      return this;
    }

    public KeyPropertyPart Type(System.Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<KeyPropertyMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(type));
      return this;
    }

    public KeyPropertyPart Type(string type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<KeyPropertyMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(type));
      return this;
    }

    public KeyPropertyPart Length(int length)
    {
      this.mapping.Set<int>((Expression<Func<KeyPropertyMapping, int>>) (x => x.Length), 2, length);
      return this;
    }

    public AccessStrategyBuilder<KeyPropertyPart> Access { get; private set; }
  }
}
