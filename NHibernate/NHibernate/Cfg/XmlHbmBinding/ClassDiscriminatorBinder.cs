// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ClassDiscriminatorBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using System;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class ClassDiscriminatorBinder : Binder
  {
    private readonly PersistentClass rootClass;

    public ClassDiscriminatorBinder(PersistentClass rootClass, Mappings mappings)
      : base(mappings)
    {
      this.rootClass = rootClass;
    }

    public void BindDiscriminator(HbmDiscriminator discriminatorSchema, Table table)
    {
      if (discriminatorSchema == null)
        return;
      SimpleValue discriminator = new SimpleValue(table);
      this.rootClass.Discriminator = (IValue) discriminator;
      this.BindSimpleValue(discriminatorSchema, discriminator);
      if (discriminator.Type == null)
        discriminator.TypeName = NHibernateUtil.String.Name;
      this.rootClass.IsPolymorphic = true;
      this.rootClass.IsForceDiscriminator = discriminatorSchema.force;
      this.rootClass.IsDiscriminatorInsertable = discriminatorSchema.insert;
    }

    private void BindSimpleValue(HbmDiscriminator discriminatorSchema, SimpleValue discriminator)
    {
      if (discriminatorSchema.type != null)
        discriminator.TypeName = discriminatorSchema.type;
      if (discriminatorSchema.formula != null)
      {
        Formula formula = new Formula()
        {
          FormulaString = discriminatorSchema.formula
        };
        discriminator.AddFormula(formula);
      }
      else
        new ColumnsBinder(discriminator, this.Mappings).Bind(discriminatorSchema.Columns, false, (Func<HbmColumn>) (() => new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName("class"),
          length = discriminatorSchema.length,
          notnull = discriminatorSchema.notnull,
          notnullSpecified = true
        }));
    }
  }
}
