// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.CfgXmlHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Event;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public static class CfgXmlHelper
  {
    public const string CfgSectionName = "hibernate-configuration";
    public const string CfgSchemaXMLNS = "urn:nhibernate-configuration-2.2";
    public const string CfgNamespacePrefix = "cfg";
    private const string RootPrefixPath = "//cfg:";
    private const string ChildPrefixPath = "cfg:";
    private static readonly XmlNamespaceManager nsMgr = new XmlNamespaceManager((XmlNameTable) new NameTable());
    public static readonly XPathExpression ByteCodeProviderExpression;
    public static readonly XPathExpression ReflectionOptimizerExpression;
    public static readonly XPathExpression SessionFactoryExpression;
    public static readonly XPathExpression SessionFactoryPropertiesExpression;
    public static readonly XPathExpression SessionFactoryMappingsExpression;
    public static readonly XPathExpression SessionFactoryClassesCacheExpression;
    public static readonly XPathExpression SessionFactoryCollectionsCacheExpression;
    public static readonly XPathExpression SessionFactoryEventsExpression;
    public static readonly XPathExpression SessionFactoryListenersExpression;

    static CfgXmlHelper()
    {
      CfgXmlHelper.nsMgr.AddNamespace("cfg", "urn:nhibernate-configuration-2.2");
      CfgXmlHelper.ByteCodeProviderExpression = XPathExpression.Compile("//cfg:bytecode-provider", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.ReflectionOptimizerExpression = XPathExpression.Compile("//cfg:reflection-optimizer", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryExpression = XPathExpression.Compile("//cfg:session-factory", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryPropertiesExpression = XPathExpression.Compile("//cfg:session-factory/cfg:property", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryMappingsExpression = XPathExpression.Compile("//cfg:session-factory/cfg:mapping", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryClassesCacheExpression = XPathExpression.Compile("//cfg:session-factory/cfg:class-cache", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryCollectionsCacheExpression = XPathExpression.Compile("//cfg:session-factory/cfg:collection-cache", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryEventsExpression = XPathExpression.Compile("//cfg:session-factory/cfg:event", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
      CfgXmlHelper.SessionFactoryListenersExpression = XPathExpression.Compile("//cfg:session-factory/cfg:listener", (IXmlNamespaceResolver) CfgXmlHelper.nsMgr);
    }

    internal static string ToConfigurationString(this BytecodeProviderType source)
    {
      switch (source)
      {
        case BytecodeProviderType.Codedom:
          return "codedom";
        case BytecodeProviderType.Lcg:
          return "lcg";
        default:
          return "null";
      }
    }

    public static ClassCacheInclude ClassCacheIncludeConvertFrom(string include)
    {
      switch (include)
      {
        case "all":
          return ClassCacheInclude.All;
        case "non-lazy":
          return ClassCacheInclude.NonLazy;
        default:
          throw new HibernateConfigException(string.Format("Invalid ClassCacheInclude value:{0}", (object) include));
      }
    }

    internal static string ClassCacheIncludeConvertToString(ClassCacheInclude include)
    {
      switch (include)
      {
        case ClassCacheInclude.All:
          return "all";
        case ClassCacheInclude.NonLazy:
          return "non-lazy";
        default:
          return string.Empty;
      }
    }

    public static ListenerType ListenerTypeConvertFrom(string listenerType)
    {
      switch (listenerType)
      {
        case "auto-flush":
          return ListenerType.Autoflush;
        case "merge":
          return ListenerType.Merge;
        case "create":
          return ListenerType.Create;
        case "create-onflush":
          return ListenerType.CreateOnFlush;
        case "delete":
          return ListenerType.Delete;
        case "dirty-check":
          return ListenerType.DirtyCheck;
        case "evict":
          return ListenerType.Evict;
        case "flush":
          return ListenerType.Flush;
        case "flush-entity":
          return ListenerType.FlushEntity;
        case "load":
          return ListenerType.Load;
        case "load-collection":
          return ListenerType.LoadCollection;
        case "lock":
          return ListenerType.Lock;
        case "refresh":
          return ListenerType.Refresh;
        case "replicate":
          return ListenerType.Replicate;
        case "save-update":
          return ListenerType.SaveUpdate;
        case "save":
          return ListenerType.Save;
        case "pre-update":
          return ListenerType.PreUpdate;
        case "update":
          return ListenerType.Update;
        case "pre-load":
          return ListenerType.PreLoad;
        case "pre-delete":
          return ListenerType.PreDelete;
        case "pre-insert":
          return ListenerType.PreInsert;
        case "post-load":
          return ListenerType.PostLoad;
        case "post-insert":
          return ListenerType.PostInsert;
        case "post-update":
          return ListenerType.PostUpdate;
        case "post-delete":
          return ListenerType.PostDelete;
        case "post-commit-update":
          return ListenerType.PostCommitUpdate;
        case "post-commit-insert":
          return ListenerType.PostCommitInsert;
        case "post-commit-delete":
          return ListenerType.PostCommitDelete;
        case "pre-collection-recreate":
          return ListenerType.PreCollectionRecreate;
        case "pre-collection-remove":
          return ListenerType.PreCollectionRemove;
        case "pre-collection-update":
          return ListenerType.PreCollectionUpdate;
        case "post-collection-recreate":
          return ListenerType.PostCollectionRecreate;
        case "post-collection-remove":
          return ListenerType.PostCollectionRemove;
        case "post-collection-update":
          return ListenerType.PostCollectionUpdate;
        default:
          throw new HibernateConfigException(string.Format("Invalid ListenerType value:{0}", (object) listenerType));
      }
    }

    internal static string ListenerTypeConvertToString(ListenerType listenerType)
    {
      switch (listenerType)
      {
        case ListenerType.Autoflush:
          return "auto-flush";
        case ListenerType.Merge:
          return "merge";
        case ListenerType.Create:
          return "create";
        case ListenerType.CreateOnFlush:
          return "create-onflush";
        case ListenerType.Delete:
          return "delete";
        case ListenerType.DirtyCheck:
          return "dirty-check";
        case ListenerType.Evict:
          return "evict";
        case ListenerType.Flush:
          return "flush";
        case ListenerType.FlushEntity:
          return "flush-entity";
        case ListenerType.Load:
          return "load";
        case ListenerType.LoadCollection:
          return "load-collection";
        case ListenerType.Lock:
          return "lock";
        case ListenerType.Refresh:
          return "refresh";
        case ListenerType.Replicate:
          return "replicate";
        case ListenerType.SaveUpdate:
          return "save-update";
        case ListenerType.Save:
          return "save";
        case ListenerType.PreUpdate:
          return "pre-update";
        case ListenerType.Update:
          return "update";
        case ListenerType.PreLoad:
          return "pre-load";
        case ListenerType.PreDelete:
          return "pre-delete";
        case ListenerType.PreInsert:
          return "pre-insert";
        case ListenerType.PreCollectionRecreate:
          return "pre-collection-recreate";
        case ListenerType.PreCollectionRemove:
          return "pre-collection-remove";
        case ListenerType.PreCollectionUpdate:
          return "pre-collection-update";
        case ListenerType.PostLoad:
          return "post-load";
        case ListenerType.PostInsert:
          return "post-insert";
        case ListenerType.PostUpdate:
          return "post-update";
        case ListenerType.PostDelete:
          return "post-delete";
        case ListenerType.PostCommitUpdate:
          return "post-commit-update";
        case ListenerType.PostCommitInsert:
          return "post-commit-insert";
        case ListenerType.PostCommitDelete:
          return "post-commit-delete";
        case ListenerType.PostCollectionRecreate:
          return "post-collection-recreate";
        case ListenerType.PostCollectionRemove:
          return "post-collection-remove";
        case ListenerType.PostCollectionUpdate:
          return "post-collection-update";
        default:
          return string.Empty;
      }
    }
  }
}
