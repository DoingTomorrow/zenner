// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Transaction.Isolater
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Engine.Transaction
{
  public class Isolater
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Isolater));

    public static void DoIsolatedWork(IIsolatedWork work, ISessionImplementor session)
    {
      session.Factory.TransactionFactory.ExecuteWorkInIsolation(session, work, true);
    }

    public static void DoNonTransactedWork(IIsolatedWork work, ISessionImplementor session)
    {
      session.Factory.TransactionFactory.ExecuteWorkInIsolation(session, work, false);
    }
  }
}
