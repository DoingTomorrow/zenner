// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Lock.ILockingStrategy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;

#nullable disable
namespace NHibernate.Dialect.Lock
{
  public interface ILockingStrategy
  {
    void Lock(object id, object version, object obj, ISessionImplementor session);
  }
}
