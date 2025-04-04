// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ElementPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ElementPart : IElementMappingProvider
  {
    private readonly System.Type entity;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly AttributeStore columnAttributes = new AttributeStore();
    private readonly ColumnMappingCollection<ElementPart> columns;

    public ElementPart(System.Type entity)
    {
      this.entity = entity;
      this.columns = new ColumnMappingCollection<ElementPart>(this);
    }

    public ElementPart Column(string elementColumnName)
    {
      this.Columns.Add(elementColumnName);
      return this;
    }

    public ColumnMappingCollection<ElementPart> Columns => this.columns;

    public ElementPart Type<TElement>()
    {
      this.attributes.Set(nameof (Type), 2, (object) new TypeReference(typeof (TElement)));
      return this;
    }

    public ElementPart Length(int length)
    {
      this.columnAttributes.Set(nameof (Length), 2, (object) length);
      return this;
    }

    public ElementPart Formula(string formula)
    {
      this.attributes.Set(nameof (Formula), 2, (object) formula);
      return this;
    }

    ElementMapping IElementMappingProvider.GetElementMapping()
    {
      ElementMapping elementMapping = new ElementMapping(this.attributes.Clone());
      elementMapping.ContainingEntityType = this.entity;
      foreach (ColumnMapping column in this.Columns)
      {
        column.MergeAttributes(this.columnAttributes);
        elementMapping.AddColumn(0, column);
      }
      return elementMapping;
    }
  }
}
