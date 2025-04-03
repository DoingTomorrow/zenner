// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.HibernateMultipleDatabasesManager
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Xml;

#nullable disable
namespace Common.Library.NHibernate.Data
{
  public class HibernateMultipleDatabasesManager
  {
    private static readonly Dictionary<string, ISessionFactory> sessionFactoryDictionary = new Dictionary<string, ISessionFactory>();
    private static bool useFluent = false;
    private static readonly Dictionary<string, bool> isInitializedDictionary = new Dictionary<string, bool>();

    public static FluentConfiguration FluentConfiguration { get; set; }

    public static NHibernate.Cfg.Configuration Configuration { get; set; }

    public static bool UsingFluent => HibernateMultipleDatabasesManager.useFluent;

    public static void Initialize(string connectionIdentifier)
    {
      string appSetting = ConfigurationManager.AppSettings["NHibernateDatabaseConfig"];
      if (HibernateMultipleDatabasesManager.isInitializedDictionary.ContainsKey(connectionIdentifier))
        return;
      HibernateMultipleDatabasesManager.Configuration = new NHibernate.Cfg.Configuration().Configure(appSetting, connectionIdentifier);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(appSetting);
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
      XmlNode xmlNode1 = xmlDocument.SelectSingleNode("descendant::hibernateMappingAssemblies[@databaseName='" + connectionIdentifier + "']", nsmgr);
      XmlNodeList assemblyNodeList = xmlNode1.SelectNodes("descendant::assembly", nsmgr);
      IInterceptor interceptor = (IInterceptor) null;
      foreach (XmlNode xmlNode2 in assemblyNodeList)
      {
        Type type = Enumerable.SingleOrDefault<Type>((IEnumerable<Type>) Assembly.Load(xmlNode2.Attributes["fullyQualifiedName"].Value).GetTypes(), (Func<Type, bool>) (t => typeof (IInterceptor).IsAssignableFrom(t)));
        if (Type.op_Inequality(type, (Type) null))
          interceptor = Activator.CreateInstance(type, (object[]) null) as IInterceptor;
      }
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      HibernateMultipleDatabasesManager.useFluent = Convert.ToBoolean(xmlNode1.Attributes["useFluent"].Value);
      if (HibernateMultipleDatabasesManager.useFluent)
      {
        HibernateMultipleDatabasesManager.FluentConfiguration = Fluently.Configure(HibernateMultipleDatabasesManager.Configuration).ExposeConfiguration((Action<NHibernate.Cfg.Configuration>) (config =>
        {
          if (interceptor == null)
            return;
          config.SetInterceptor(interceptor);
          HibernateMultipleDatabasesManager.Interceptor = interceptor;
        })).Mappings((Action<MappingConfiguration>) (m =>
        {
          foreach (XmlNode xmlNode3 in assemblyNodeList)
          {
            XmlNode mappingsAssembly = xmlNode3;
            Assembly assembly = Enumerable.FirstOrDefault<Assembly>((IEnumerable<Assembly>) assemblies, (Func<Assembly, bool>) (a => a.GetName().Name.Equals(mappingsAssembly.Attributes["fullyQualifiedName"].Value)));
            if (Assembly.op_Inequality(assembly, (Assembly) null))
            {
              m.FluentMappings.AddFromAssembly(assembly);
              m.HbmMappings.AddFromAssembly(assembly);
            }
          }
        }));
        ISessionFactory sessionFactory = HibernateMultipleDatabasesManager.FluentConfiguration.BuildSessionFactory();
        HibernateMultipleDatabasesManager.sessionFactoryDictionary.Add(connectionIdentifier, sessionFactory);
      }
      else
      {
        foreach (XmlNode xmlNode4 in assemblyNodeList)
        {
          XmlNode mappingsAssembly = xmlNode4;
          Assembly assembly = Enumerable.FirstOrDefault<Assembly>((IEnumerable<Assembly>) assemblies, (Func<Assembly, bool>) (a => a.GetName().Name.Equals(mappingsAssembly.Attributes["fullyQualifiedName"].Value)));
          if (Assembly.op_Inequality(assembly, (Assembly) null))
            HibernateMultipleDatabasesManager.Configuration.AddAssembly(assembly);
        }
        HibernateMultipleDatabasesManager.sessionFactoryDictionary.Add(connectionIdentifier, HibernateMultipleDatabasesManager.FluentConfiguration.BuildSessionFactory());
      }
      HibernateMultipleDatabasesManager.isInitializedDictionary.Add(connectionIdentifier, true);
    }

    public static ISessionFactory DataSessionFactory(string connectionIdentifier)
    {
      if (!HibernateMultipleDatabasesManager.sessionFactoryDictionary.ContainsKey(connectionIdentifier))
        HibernateMultipleDatabasesManager.Initialize(connectionIdentifier);
      return HibernateMultipleDatabasesManager.sessionFactoryDictionary[connectionIdentifier];
    }

    public static void Delete(object target, string connectionIdentifier)
    {
      HibernateMultipleDatabasesManager.Delete(target, HibernateMultipleDatabasesManager.DataSessionFactory(connectionIdentifier).GetCurrentSession());
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

    public static void Update(object target, string connectionIdentifier)
    {
      HibernateMultipleDatabasesManager.Update(target, HibernateMultipleDatabasesManager.DataSessionFactory(connectionIdentifier).GetCurrentSession());
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

    public static void Insert(object target, string connectionIdentifier)
    {
      HibernateMultipleDatabasesManager.Insert(target, HibernateMultipleDatabasesManager.DataSessionFactory(connectionIdentifier).GetCurrentSession());
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

    public static T Load<T>(object id, string connectionIdentifier) where T : class
    {
      return HibernateMultipleDatabasesManager.Load<T>(id, HibernateMultipleDatabasesManager.DataSessionFactory(connectionIdentifier).GetCurrentSession());
    }

    public static T Load<T>(object id, ISession session) where T : class
    {
      if (!HibernateMultipleDatabasesManager.IsCompositeIdentifier(id))
        return session.Get<T>(id);
      ICriteria criteria = session.CreateCriteria<T>();
      foreach (PropertyInfo referenceProperty in HibernateMultipleDatabasesManager.GetCompositeReferenceProperties(typeof (T)))
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
