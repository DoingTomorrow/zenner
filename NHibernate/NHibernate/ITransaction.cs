// Decompiled with JetBrains decompiler
// Type: NHibernate.ITransaction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Transaction;
using System;
using System.Data;

#nullable disable
namespace NHibernate
{
  public interface ITransaction : IDisposable
  {
    void Begin();

    void Begin(IsolationLevel isolationLevel);

    void Commit();

    void Rollback();

    bool IsActive { get; }

    bool WasRolledBack { get; }

    bool WasCommitted { get; }

    void Enlist(IDbCommand command);

    void RegisterSynchronization(ISynchronization synchronization);
  }
}
