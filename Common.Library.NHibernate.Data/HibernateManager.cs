// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.HibernateManager
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using Common.Library.Core.Business.Database;
using Common.Library.NHibernate.Data.Configuration;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Common.Library.NHibernate.Data
{
  public class HibernateManager
  {
    private static ISessionFactory sessionFactory = (ISessionFactory) null;
    private static bool useFluent = false;
    private static bool isInitialized = false;

    public static FluentConfiguration FluentConfiguration { get; set; }

    public static NHibernate.Cfg.Configuration Configuration { get; set; }

    public static bool UsingFluent => HibernateManager.useFluent;

    public static void Initialize()
    {
      if (HibernateManager.isInitialized)
        return;
      HibernateMappingsAssemblySection assemblySection = ConfigurationManager.GetSection("hibernateMappingAssemblies") as HibernateMappingsAssemblySection;
      HibernateManager.Configuration = new NHibernate.Cfg.Configuration().Configure();
      string str = string.Empty;
      IInterceptor interceptor = (IInterceptor) null;
      foreach (HibernateMappingsAssembly mappingsAssembly in assemblySection.Assemblies.OfType<HibernateMappingsAssembly>())
      {
        Type type = Enumerable.SingleOrDefault<Type>((IEnumerable<Type>) Assembly.Load(mappingsAssembly.FullyQualifiedName).GetTypes(), (Func<Type, bool>) (t => typeof (IInterceptor).IsAssignableFrom(t)));
        if (Type.op_Inequality(type, (Type) null))
          interceptor = Activator.CreateInstance(type, (object[]) null) as IInterceptor;
      }
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach (Assembly assembly in assemblies)
      {
        if (Assembly.op_Inequality(assembly, Assembly.GetExecutingAssembly()))
        {
          Type type = Enumerable.SingleOrDefault<Type>((IEnumerable<Type>) assembly.GetTypes(), (Func<Type, bool>) (t => typeof (IConnectionStringProvider).IsAssignableFrom(t) && !t.IsInterface));
          if (Type.op_Inequality(type, (Type) null))
          {
            str = HibernateManager.ConfigureConnectionStringFromProvider(Activator.CreateInstance(type, (object[]) null) as IConnectionStringProvider);
            break;
          }
        }
      }
      if (str == string.Empty)
        HibernateManager.ConfigureConnectionStringFromProvider((IConnectionStringProvider) new DefaultConnectionStringProvider());
      HibernateManager.useFluent = assemblySection.UseFluent;
      if (HibernateManager.useFluent)
      {
        HibernateManager.FluentConfiguration = Fluently.Configure(HibernateManager.Configuration).ExposeConfiguration((Action<NHibernate.Cfg.Configuration>) (config =>
        {
          if (interceptor == null)
            return;
          config.SetInterceptor(interceptor);
          HibernateManager.Interceptor = interceptor;
        })).Mappings((Action<MappingConfiguration>) (m =>
        {
          foreach (HibernateMappingsAssembly assembly1 in (ConfigurationElementCollection) assemblySection.Assemblies)
          {
            HibernateMappingsAssembly mappingsAssembly = assembly1;
            Assembly assembly2 = Enumerable.FirstOrDefault<Assembly>((IEnumerable<Assembly>) assemblies, (Func<Assembly, bool>) (a => a.GetName().Name.Equals(mappingsAssembly.FullyQualifiedName)));
            if (Assembly.op_Inequality(assembly2, (Assembly) null))
            {
              m.FluentMappings.AddFromAssembly(assembly2);
              m.HbmMappings.AddFromAssembly(assembly2);
            }
          }
        }));
        HibernateManager.sessionFactory = HibernateManager.FluentConfiguration.BuildSessionFactory();
      }
      else
      {
        foreach (HibernateMappingsAssembly assembly3 in (ConfigurationElementCollection) assemblySection.Assemblies)
        {
          HibernateMappingsAssembly mappingsAssembly = assembly3;
          Assembly assembly4 = Enumerable.FirstOrDefault<Assembly>((IEnumerable<Assembly>) assemblies, (Func<Assembly, bool>) (a => a.GetName().Name.Equals(mappingsAssembly.FullyQualifiedName)));
          if (Assembly.op_Inequality(assembly4, (Assembly) null))
            HibernateManager.Configuration.AddAssembly(assembly4);
        }
        HibernateManager.sessionFactory = HibernateManager.Configuration.BuildSessionFactory();
      }
      HibernateManager.isInitialized = true;
    }

    private static string ConfigureConnectionStringFromProvider(
      IConnectionStringProvider providerInstance)
    {
      HibernateManager.Configuration.SetProperty("connection.connection_string", providerInstance.GetConnectionString());
      return providerInstance.GetConnectionString();
    }

    public static ISessionFactory DataSessionFactory
    {
      get
      {
        if (HibernateManager.sessionFactory == null)
          HibernateManager.Initialize();
        return HibernateManager.sessionFactory;
      }
    }

    public static void Delete(object target)
    {
      HibernateManager.Delete(target, HibernateManager.DataSessionFactory.GetCurrentSession());
    }

    public static void Delete(object target, ISession session)
    {
      ITransaction transaction = session.BeginTransaction();
      try
      {
        session.Delete(target);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        transaction.Rollback();
        throw ex;
      }
    }

    public static void Update(object target)
    {
      HibernateManager.Update(target, HibernateManager.DataSessionFactory.GetCurrentSession());
    }

    public static void Update(object target, ISession session)
    {
      ITransaction transaction = session.BeginTransaction();
      try
      {
        session.SaveOrUpdate(target);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        transaction.Rollback();
        throw ex;
      }
    }

    public static void Insert(object target)
    {
      HibernateManager.Insert(target, HibernateManager.DataSessionFactory.GetCurrentSession());
    }

    public static void Insert(object target, ISession session)
    {
      ITransaction transaction = session.BeginTransaction();
      try
      {
        session.SaveOrUpdate(target);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        transaction.Rollback();
        throw ex;
      }
    }

    public static T Load<T>(object id) where T : class
    {
      return HibernateManager.Load<T>(id, HibernateManager.DataSessionFactory.GetCurrentSession());
    }

    public static T Load<T>(object id, ISession session) where T : class
    {
      if (!HibernateManager.IsCompositeIdentifier(id))
        return session.Get<T>(id);
      ICriteria criteria = session.CreateCriteria<T>();
      foreach (PropertyInfo referenceProperty in HibernateManager.GetCompositeReferenceProperties(typeof (T)))
      {
        CompositeKeyPropertyReferenceAttribute customAttribute = referenceProperty.GetCustomAttributes(typeof (CompositeKeyPropertyReferenceAttribute), false)[0] as CompositeKeyPropertyReferenceAttribute;
        string alias = "_" + referenceProperty.Name;
        criteria.CreateAlias(referenceProperty.Name, alias);
        List<string> list1 = ((IEnumerable<string>) customAttribute.ReferencedPrimaryKey.Split(',')).ToList<string>();
        List<string> list2 = ((IEnumerable<string>) customAttribute.CompositeIdProperties.Split(',')).ToList<string>();
        foreach (string str in list1)
        {
          string name = list2[list1.IndexOf(str)];
          object obj = id.GetType().GetProperty(name).GetValue(id, (object[]) null);
          criteria.Add((ICriterion) Restrictions.Eq(string.Format("{0}.{1}", (object) alias, (object) str), obj));
        }
      }
      return criteria.UniqueResult<T>();
    }

    public static void TransactionalInsert(object target, ISession session) => session.Save(target);

    public static void TransactionalUpdate(object target, ISession session)
    {
      session.SaveOrUpdate(target);
    }

    public static void TransactionalDelete(object target, ISession session)
    {
      session.Delete(target);
    }

    private static IEnumerable<PropertyInfo> GetCompositeReferenceProperties(Type type)
    {
      return Enumerable.Where<PropertyInfo>(((IEnumerable<PropertyInfo>) type.GetProperties()).AsEnumerable<PropertyInfo>(), (Func<PropertyInfo, bool>) (p => ((IEnumerable<object>) p.GetCustomAttributes(typeof (CompositeKeyPropertyReferenceAttribute), false)).Count<object>() == 1));
    }

    private static bool IsCompositeIdentifier(object target)
    {
      return ((IEnumerable<object>) target.GetType().GetCustomAttributes(typeof (CompositeIdentifierAttribute), false)).Count<object>() > 0;
    }

    public static IInterceptor Interceptor { get; set; }
  }
}
