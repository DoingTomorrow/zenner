// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.SequenceIdentityGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id.Insert;

#nullable disable
namespace NHibernate.Id
{
  public class SequenceIdentityGenerator : 
    SequenceGenerator,
    IPostInsertIdentifierGenerator,
    IIdentifierGenerator
  {
    public override object Generate(ISessionImplementor session, object obj)
    {
      return IdentifierGeneratorFactory.PostInsertIndicator;
    }

    public IInsertGeneratedIdentifierDelegate GetInsertGeneratedIdentifierDelegate(
      IPostInsertIdentityPersister persister,
      ISessionFactoryImplementor factory,
      bool isGetGeneratedKeysEnabled)
    {
      return (IInsertGeneratedIdentifierDelegate) new SequenceIdentityGenerator.SequenceIdentityDelegate(persister, factory, this.SequenceName);
    }

    public class SequenceIdentityDelegate : OutputParamReturningDelegate
    {
      private readonly string sequenceNextValFragment;

      public SequenceIdentityDelegate(
        IPostInsertIdentityPersister persister,
        ISessionFactoryImplementor factory,
        string sequenceName)
        : base(persister, factory)
      {
        this.sequenceNextValFragment = factory.Dialect.GetSelectSequenceNextValString(sequenceName);
      }

      public override IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert()
      {
        IdentifierGeneratingInsert generatingInsert = base.PrepareIdentifierGeneratingInsert();
        generatingInsert.AddColumn(this.Persister.RootTableKeyColumnNames[0], this.sequenceNextValFragment);
        return generatingInsert;
      }
    }
  }
}
