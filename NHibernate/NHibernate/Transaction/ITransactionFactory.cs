// Decompiled with JetBrains decompiler
// Type: NHibernate.Transaction.ITransactionFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using System.Collections;

#nullable disable
namespace NHibernate.Transaction
{
  public interface ITransactionFactory
  {
    void Configure(IDictionary props);

    ITransaction CreateTransaction(ISessionImplementor session);

    void EnlistInDistributedTransactionIfNeeded(ISessionImplementor session);

    bool IsInDistributedActiveTransaction(ISessionImplementor session);

    void ExecuteWorkInIsolation(ISessionImplementor session, IIsolatedWork work, bool transacted);
  }
}
