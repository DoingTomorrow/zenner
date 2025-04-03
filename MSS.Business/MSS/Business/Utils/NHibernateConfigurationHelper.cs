// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.NHibernateConfigurationHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace MSS.Business.Utils
{
  public static class NHibernateConfigurationHelper
  {
    public static string GetPropertyValue(string sessionFactoryName, string propertyName)
    {
      return XDocument.Load(ConfigurationManager.AppSettings["NHibernateDatabaseConfig"]).Descendants((XName) "{urn:nhibernate-configuration-2.2-x-factories}session-factory").Where<XElement>((Func<XElement, bool>) (node => node.Attribute((XName) "name").Value == sessionFactoryName)).Elements<XElement>((XName) "{urn:nhibernate-configuration-2.2-x-factories}property").First<XElement>((Func<XElement, bool>) (node => node.Attribute((XName) "name").Value == propertyName)).Value;
    }

    public static string GetDataSourceForConnString(string sessionFactoryName)
    {
      string str = XDocument.Load(ConfigurationManager.AppSettings["NHibernateDatabaseConfig"]).Descendants((XName) "{urn:nhibernate-configuration-2.2-x-factories}session-factory").Where<XElement>((Func<XElement, bool>) (node => node.Attribute((XName) "name").Value == sessionFactoryName)).Elements<XElement>((XName) "{urn:nhibernate-configuration-2.2-x-factories}property").First<XElement>((Func<XElement, bool>) (node => node.Attribute((XName) "name").Value == "connection.connection_string")).Value;
      int startIndex = str.IndexOf("data source=", StringComparison.Ordinal) + "data source=".Length;
      int length = str.IndexOf(";", startIndex, StringComparison.Ordinal) - startIndex;
      return str.Substring(startIndex, length);
    }

    public static Dictionary<string, string> GetPropertyAndColumnNames(
      ISessionFactory sessionFactory,
      Type entityType)
    {
      IClassMetadata classMetadata = sessionFactory.GetClassMetadata(entityType.ToString());
      AbstractEntityPersister abstractEntityPersister = (AbstractEntityPersister) classMetadata;
      Dictionary<string, string> propertyAndColumnNames = new Dictionary<string, string>();
      string identifierPropertyName = classMetadata.IdentifierPropertyName;
      string keyColumnName = abstractEntityPersister.KeyColumnNames[0];
      propertyAndColumnNames.Add(identifierPropertyName, keyColumnName);
      foreach (KeyValuePair<string, string[]> keyValuePair in (Dictionary<string, string[]>) typeof (AbstractEntityPersister).GetField("subclassPropertyColumnNames", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) abstractEntityPersister))
      {
        if (keyValuePair.Value.Length != 0)
        {
          if (!(keyValuePair.Value[0] == keyColumnName))
            propertyAndColumnNames.Add(keyValuePair.Key, keyValuePair.Value[0]);
          else
            break;
        }
      }
      return propertyAndColumnNames;
    }
  }
}
