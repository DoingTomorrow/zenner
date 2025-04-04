// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.SessionIdLoggingContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Impl
{
  public class SessionIdLoggingContext : IDisposable
  {
    [ThreadStatic]
    private static Guid? CurrentSessionId;
    private readonly Guid? oldSessonId;

    public SessionIdLoggingContext(Guid id)
    {
      this.oldSessonId = SessionIdLoggingContext.SessionId;
      SessionIdLoggingContext.SessionId = new Guid?(id);
    }

    public static Guid? SessionId
    {
      get => SessionIdLoggingContext.CurrentSessionId;
      set => SessionIdLoggingContext.CurrentSessionId = value;
    }

    public void Dispose() => SessionIdLoggingContext.SessionId = this.oldSessonId;
  }
}
