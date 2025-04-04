// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Insert.OutputParamReturningDelegate
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Id.Insert
{
  public class OutputParamReturningDelegate : AbstractReturningDelegate
  {
    private const string ReturnParameterName = "nhIdOutParam";
    private readonly ISessionFactoryImplementor factory;
    private readonly string idColumnName;
    private readonly SqlType paramType;
    private string driveGeneratedParamName = "nhIdOutParam";

    public OutputParamReturningDelegate(
      IPostInsertIdentityPersister persister,
      ISessionFactoryImplementor factory)
      : base(persister)
    {
      if (this.Persister.RootTableKeyColumnNames.Length > 1)
        throw new HibernateException("identity-style generator cannot be used with multi-column keys");
      this.paramType = this.Persister.IdentifierType.SqlTypes((IMapping) factory)[0];
      this.idColumnName = this.Persister.RootTableKeyColumnNames[0];
      this.factory = factory;
    }

    public override IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert()
    {
      return (IdentifierGeneratingInsert) new ReturningIdentifierInsert(this.factory, this.idColumnName, "nhIdOutParam");
    }

    protected internal override IDbCommand Prepare(
      SqlCommandInfo insertSQL,
      ISessionImplementor session)
    {
      IDbCommand command = session.Batcher.PrepareCommand(CommandType.Text, insertSQL.Text, insertSQL.ParameterTypes);
      IDbDataParameter parameter = this.factory.ConnectionProvider.Driver.GenerateParameter(command, "nhIdOutParam", this.paramType);
      this.driveGeneratedParamName = parameter.ParameterName;
      if (this.factory.Dialect.InsertGeneratedIdentifierRetrievalMethod == InsertGeneratedIdentifierRetrievalMethod.OutputParameter)
      {
        parameter.Direction = ParameterDirection.Output;
      }
      else
      {
        if (this.factory.Dialect.InsertGeneratedIdentifierRetrievalMethod != InsertGeneratedIdentifierRetrievalMethod.ReturnValueParameter)
          throw new NotImplementedException("Unsupported InsertGeneratedIdentifierRetrievalMethod: " + (object) this.factory.Dialect.InsertGeneratedIdentifierRetrievalMethod);
        parameter.Direction = ParameterDirection.ReturnValue;
      }
      command.Parameters.Add((object) parameter);
      return command;
    }

    public override object ExecuteAndExtract(IDbCommand insert, ISessionImplementor session)
    {
      session.Batcher.ExecuteNonQuery(insert);
      return ((IDataParameter) insert.Parameters[this.driveGeneratedParamName]).Value;
    }
  }
}
