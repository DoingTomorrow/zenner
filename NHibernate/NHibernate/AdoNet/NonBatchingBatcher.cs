// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.NonBatchingBatcher
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.AdoNet
{
  public class NonBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor) : 
    AbstractBatcher(connectionManager, interceptor)
  {
    public override void AddToBatch(IExpectation expectation)
    {
      IDbCommand currentCommand = this.CurrentCommand;
      this.Driver.AdjustCommand(currentCommand);
      int rowCount = this.ExecuteNonQuery(currentCommand);
      expectation.VerifyOutcomeNonBatched(rowCount, currentCommand);
    }

    protected override void DoExecuteBatch(IDbCommand ps)
    {
    }

    protected override int CountOfStatementsInCurrentBatch => 1;

    public override int BatchSize
    {
      get => 1;
      set
      {
        throw new NotSupportedException("No batch size was defined for the session factory, batching is disabled. Set adonet.batch_size = 1 to enable batching.");
      }
    }
  }
}
