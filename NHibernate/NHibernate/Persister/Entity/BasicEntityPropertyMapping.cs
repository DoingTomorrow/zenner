// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.BasicEntityPropertyMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public class BasicEntityPropertyMapping : AbstractPropertyMapping
  {
    private readonly AbstractEntityPersister persister;

    public BasicEntityPropertyMapping(AbstractEntityPersister persister)
    {
      this.persister = persister;
    }

    public override string[] IdentifierColumnNames => this.persister.IdentifierColumnNames;

    protected override string EntityName => this.persister.EntityName;

    public override IType Type => this.persister.Type;

    public override string[] ToColumns(string alias, string propertyName)
    {
      return base.ToColumns(this.persister.GenerateTableAlias(alias, this.persister.GetSubclassPropertyTableNumber(propertyName)), propertyName);
    }
  }
}
