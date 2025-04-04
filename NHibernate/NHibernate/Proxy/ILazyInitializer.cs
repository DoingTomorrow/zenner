// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.ILazyInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Proxy
{
  public interface ILazyInitializer
  {
    void Initialize();

    object Identifier { get; set; }

    string EntityName { get; }

    Type PersistentClass { get; }

    bool IsUninitialized { get; }

    bool Unwrap { get; set; }

    ISessionImplementor Session { get; set; }

    bool IsReadOnlySettingAvailable { get; }

    bool ReadOnly { get; set; }

    object GetImplementation();

    object GetImplementation(ISessionImplementor s);

    void SetImplementation(object target);

    void SetSession(ISessionImplementor s);

    void UnsetSession();
  }
}
