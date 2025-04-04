// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.CompositeElementPropertyMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public class CompositeElementPropertyMapping : AbstractPropertyMapping
  {
    private readonly IAbstractComponentType compositeType;

    public CompositeElementPropertyMapping(
      string[] elementColumns,
      string[] elementFormulaTemplates,
      IAbstractComponentType compositeType,
      IMapping factory)
    {
      this.compositeType = compositeType;
      this.InitComponentPropertyPaths((string) null, compositeType, elementColumns, elementFormulaTemplates, factory);
    }

    public override IType Type => (IType) this.compositeType;

    protected override string EntityName => this.compositeType.Name;
  }
}
