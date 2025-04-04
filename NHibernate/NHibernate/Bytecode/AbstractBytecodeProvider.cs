// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.AbstractBytecodeProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Bytecode
{
  public abstract class AbstractBytecodeProvider : 
    IBytecodeProvider,
    IInjectableProxyFactoryFactory,
    IInjectableCollectionTypeFactoryClass
  {
    private readonly IObjectsFactory objectsFactory = (IObjectsFactory) new ActivatorObjectsFactory();
    protected System.Type proxyFactoryFactory;
    private ICollectionTypeFactory collectionTypeFactory;
    private System.Type collectionTypeFactoryClass = typeof (DefaultCollectionTypeFactory);

    public virtual IProxyFactoryFactory ProxyFactoryFactory
    {
      get
      {
        if (this.proxyFactoryFactory == null)
          return (IProxyFactoryFactory) new DefaultProxyFactoryFactory();
        try
        {
          return (IProxyFactoryFactory) this.ObjectsFactory.CreateInstance(this.proxyFactoryFactory);
        }
        catch (Exception ex)
        {
          throw new HibernateByteCodeException("Failed to create an instance of '" + this.proxyFactoryFactory.FullName + "'!", ex);
        }
      }
    }

    public abstract IReflectionOptimizer GetReflectionOptimizer(
      System.Type clazz,
      IGetter[] getters,
      ISetter[] setters);

    public virtual IObjectsFactory ObjectsFactory => this.objectsFactory;

    public virtual ICollectionTypeFactory CollectionTypeFactory
    {
      get
      {
        if (this.collectionTypeFactory == null)
        {
          try
          {
            this.collectionTypeFactory = (ICollectionTypeFactory) this.ObjectsFactory.CreateInstance(this.collectionTypeFactoryClass);
          }
          catch (Exception ex)
          {
            throw new HibernateByteCodeException("Failed to create an instance of CollectionTypeFactory!", ex);
          }
        }
        return this.collectionTypeFactory;
      }
    }

    public virtual void SetProxyFactoryFactory(string typeName)
    {
      System.Type c;
      try
      {
        c = ReflectHelper.ClassForName(typeName);
      }
      catch (Exception ex)
      {
        throw new UnableToLoadProxyFactoryFactoryException(typeName, ex);
      }
      this.proxyFactoryFactory = typeof (IProxyFactoryFactory).IsAssignableFrom(c) ? c : throw new HibernateByteCodeException(c.FullName + " does not implement " + typeof (IProxyFactoryFactory).FullName);
    }

    public void SetCollectionTypeFactoryClass(string typeAssemblyQualifiedName)
    {
      if (string.IsNullOrEmpty(typeAssemblyQualifiedName))
        throw new ArgumentNullException(nameof (typeAssemblyQualifiedName));
      this.SetCollectionTypeFactoryClass(ReflectHelper.ClassForName(typeAssemblyQualifiedName));
    }

    public void SetCollectionTypeFactoryClass(System.Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (!typeof (ICollectionTypeFactory).IsAssignableFrom(type))
        throw new HibernateByteCodeException(type.FullName + " does not implement " + typeof (ICollectionTypeFactory).FullName);
      if (this.collectionTypeFactory != null && !this.collectionTypeFactoryClass.Equals(type))
        throw new InvalidOperationException("CollectionTypeFactory in use, can't change it.");
      this.collectionTypeFactoryClass = type;
    }
  }
}
