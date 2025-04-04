// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.IProxyFactoryFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Proxy;
using System;

#nullable disable
namespace NHibernate.Bytecode
{
  public interface IProxyFactoryFactory
  {
    IProxyFactory BuildProxyFactory();

    IProxyValidator ProxyValidator { get; }

    bool IsInstrumented(Type entityClass);

    bool IsProxy(object entity);
  }
}
