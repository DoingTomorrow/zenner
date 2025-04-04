// Decompiled with JetBrains decompiler
// Type: NHibernate.ByteCode.Castle.ProxyFactoryFactory
// Assembly: NHibernate.ByteCode.Castle, Version=3.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: 1144BD3F-E8FD-45B0-9AA0-77466B846AAB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.ByteCode.Castle.dll

using NHibernate.Bytecode;
using NHibernate.Proxy;
using System;

#nullable disable
namespace NHibernate.ByteCode.Castle
{
  public class ProxyFactoryFactory : IProxyFactoryFactory
  {
    public IProxyFactory BuildProxyFactory() => (IProxyFactory) new ProxyFactory();

    public IProxyValidator ProxyValidator => (IProxyValidator) new DynProxyTypeValidator();

    public bool IsInstrumented(Type entityClass) => true;

    public bool IsProxy(object entity) => entity is INHibernateProxy;
  }
}
