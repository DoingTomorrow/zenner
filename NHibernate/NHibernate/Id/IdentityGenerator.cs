// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.IdentityGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.SqlCommand;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Id
{
  public class IdentityGenerator : AbstractPostInsertGenerator
  {
    public override IInsertGeneratedIdentifierDelegate GetInsertGeneratedIdentifierDelegate(
      IPostInsertIdentityPersister persister,
      ISessionFactoryImplementor factory,
      bool isGetGeneratedKeysEnabled)
    {
      if (isGetGeneratedKeysEnabled)
        throw new NotSupportedException();
      return factory.Dialect.SupportsInsertSelectIdentity ? (IInsertGeneratedIdentifierDelegate) new IdentityGenerator.InsertSelectDelegate(persister, factory) : (IInsertGeneratedIdentifierDelegate) new IdentityGenerator.BasicDelegate(persister, factory);
    }

    public class InsertSelectDelegate : AbstractReturningDelegate, IInsertGeneratedIdentifierDelegate
    {
      private readonly IPostInsertIdentityPersister persister;
      private readonly ISessionFactoryImplementor factory;

      public InsertSelectDelegate(
        IPostInsertIdentityPersister persister,
        ISessionFactoryImplementor factory)
        : base(persister)
      {
        this.persister = persister;
        this.factory = factory;
      }

      public override IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert()
      {
        InsertSelectIdentityInsert selectIdentityInsert = new InsertSelectIdentityInsert(this.factory);
        selectIdentityInsert.AddIdentityColumn(this.persister.RootTableKeyColumnNames[0]);
        return (IdentifierGeneratingInsert) selectIdentityInsert;
      }

      protected internal override IDbCommand Prepare(
        SqlCommandInfo insertSQL,
        ISessionImplementor session)
      {
        return session.Batcher.PrepareCommand(CommandType.Text, insertSQL.Text, insertSQL.ParameterTypes);
      }

      public override object ExecuteAndExtract(IDbCommand insert, ISessionImplementor session)
      {
        IDataReader dataReader = session.Batcher.ExecuteReader(insert);
        try
        {
          return IdentifierGeneratorFactory.GetGeneratedIdentity(dataReader, this.persister.IdentifierType, session);
        }
        finally
        {
          session.Batcher.CloseReader(dataReader);
        }
      }

      public object DetermineGeneratedIdentifier(ISessionImplementor session, object entity)
      {
        throw new AssertionFailure("insert statement returns generated value");
      }
    }

    public class BasicDelegate : AbstractSelectingDelegate, IInsertGeneratedIdentifierDelegate
    {
      private readonly IPostInsertIdentityPersister persister;
      private readonly ISessionFactoryImplementor factory;

      public BasicDelegate(
        IPostInsertIdentityPersister persister,
        ISessionFactoryImplementor factory)
        : base(persister)
      {
        this.persister = persister;
        this.factory = factory;
      }

      protected internal override SqlString SelectSQL
      {
        get => new SqlString(this.persister.IdentitySelectString);
      }

      public override IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert()
      {
        IdentifierGeneratingInsert generatingInsert = new IdentifierGeneratingInsert(this.factory);
        generatingInsert.AddIdentityColumn(this.persister.RootTableKeyColumnNames[0]);
        return generatingInsert;
      }

      protected internal override object GetResult(
        ISessionImplementor session,
        IDataReader rs,
        object obj)
      {
        return IdentifierGeneratorFactory.GetGeneratedIdentity(rs, this.persister.IdentifierType, session);
      }
    }
  }
}
