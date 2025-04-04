// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.PersisterFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using System;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Persister
{
  public sealed class PersisterFactory
  {
    private static readonly Type[] PersisterConstructorArgs = new Type[4]
    {
      typeof (PersistentClass),
      typeof (ICacheConcurrencyStrategy),
      typeof (ISessionFactoryImplementor),
      typeof (IMapping)
    };
    private static readonly Type[] CollectionPersisterConstructorArgs = new Type[3]
    {
      typeof (NHibernate.Mapping.Collection),
      typeof (ICacheConcurrencyStrategy),
      typeof (ISessionFactoryImplementor)
    };
    private static readonly Type[] CollectionPersisterConstructor2Args = new Type[4]
    {
      typeof (NHibernate.Mapping.Collection),
      typeof (ICacheConcurrencyStrategy),
      typeof (Configuration),
      typeof (ISessionFactoryImplementor)
    };

    private PersisterFactory()
    {
    }

    public static IEntityPersister CreateClassPersister(
      PersistentClass model,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory,
      IMapping cfg)
    {
      Type entityPersisterClass = model.EntityPersisterClass;
      if (entityPersisterClass == null || entityPersisterClass == typeof (SingleTableEntityPersister))
        return (IEntityPersister) new SingleTableEntityPersister(model, cache, factory, cfg);
      if (entityPersisterClass == typeof (JoinedSubclassEntityPersister))
        return (IEntityPersister) new JoinedSubclassEntityPersister(model, cache, factory, cfg);
      return entityPersisterClass == typeof (UnionSubclassEntityPersister) ? (IEntityPersister) new UnionSubclassEntityPersister(model, cache, factory, cfg) : PersisterFactory.Create(entityPersisterClass, model, cache, factory, cfg);
    }

    public static ICollectionPersister CreateCollectionPersister(
      Configuration cfg,
      NHibernate.Mapping.Collection model,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory)
    {
      Type collectionPersisterClass = model.CollectionPersisterClass;
      if (collectionPersisterClass != null)
        return PersisterFactory.Create(collectionPersisterClass, model, cache, factory, cfg);
      return !model.IsOneToMany ? (ICollectionPersister) new BasicCollectionPersister(model, cache, cfg, factory) : (ICollectionPersister) new OneToManyPersister(model, cache, cfg, factory);
    }

    public static IEntityPersister Create(
      Type persisterClass,
      PersistentClass model,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory,
      IMapping cfg)
    {
      ConstructorInfo constructor;
      try
      {
        constructor = persisterClass.GetConstructor(PersisterFactory.PersisterConstructorArgs);
      }
      catch (Exception ex)
      {
        throw new MappingException("Could not get constructor for " + persisterClass.Name, ex);
      }
      try
      {
        return (IEntityPersister) constructor.Invoke(new object[4]
        {
          (object) model,
          (object) cache,
          (object) factory,
          (object) cfg
        });
      }
      catch (TargetInvocationException ex)
      {
        Exception innerException = ex.InnerException;
        if (innerException is HibernateException)
          throw innerException;
        throw new MappingException("Could not instantiate persister " + persisterClass.Name, innerException);
      }
      catch (Exception ex)
      {
        throw new MappingException("Could not instantiate persister " + persisterClass.Name, ex);
      }
    }

    public static ICollectionPersister Create(
      Type persisterClass,
      NHibernate.Mapping.Collection model,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory,
      Configuration cfg)
    {
      bool flag = false;
      ConstructorInfo constructor;
      try
      {
        constructor = persisterClass.GetConstructor(PersisterFactory.CollectionPersisterConstructorArgs);
        if (constructor == null)
        {
          flag = true;
          constructor = persisterClass.GetConstructor(PersisterFactory.CollectionPersisterConstructor2Args);
        }
      }
      catch (Exception ex)
      {
        throw new MappingException("Could not get constructor for " + persisterClass.Name, ex);
      }
      if (constructor == null)
      {
        StringBuilder messageBuilder = new StringBuilder();
        messageBuilder.AppendLine("Could not find a public constructor for " + persisterClass.Name + ";");
        messageBuilder.AppendLine("- The ctor may have " + (object) PersisterFactory.CollectionPersisterConstructorArgs.Length + " parameters of types (in order):");
        System.Array.ForEach<Type>(PersisterFactory.CollectionPersisterConstructorArgs, (Action<Type>) (t => messageBuilder.AppendLine(t.FullName)));
        messageBuilder.AppendLine();
        messageBuilder.AppendLine("- The ctor may have " + (object) PersisterFactory.CollectionPersisterConstructor2Args.Length + " parameters of types (in order):");
        System.Array.ForEach<Type>(PersisterFactory.CollectionPersisterConstructor2Args, (Action<Type>) (t => messageBuilder.AppendLine(t.FullName)));
        throw new MappingException(messageBuilder.ToString());
      }
      try
      {
        return !flag ? (ICollectionPersister) constructor.Invoke(new object[3]
        {
          (object) model,
          (object) cache,
          (object) factory
        }) : (ICollectionPersister) constructor.Invoke(new object[4]
        {
          (object) model,
          (object) cache,
          (object) cfg,
          (object) factory
        });
      }
      catch (TargetInvocationException ex)
      {
        Exception innerException = ex.InnerException;
        if (innerException is HibernateException)
          throw innerException;
        throw new MappingException("Could not instantiate collection persister " + persisterClass.Name, innerException);
      }
      catch (Exception ex)
      {
        throw new MappingException("Could not instantiate collection persister " + persisterClass.Name, ex);
      }
    }
  }
}
