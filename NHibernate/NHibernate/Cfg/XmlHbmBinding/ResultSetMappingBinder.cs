// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ResultSetMappingBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Engine.Query.Sql;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class ResultSetMappingBinder(Mappings mappings) : Binder(mappings)
  {
    public ResultSetMappingDefinition Create(HbmResultSet resultSetSchema)
    {
      return this.Create(resultSetSchema.name, resultSetSchema.Items);
    }

    public ResultSetMappingDefinition Create(HbmSqlQuery sqlQuerySchema)
    {
      return this.Create(sqlQuerySchema.name, sqlQuerySchema.Items);
    }

    private ResultSetMappingDefinition Create(string name, object[] items)
    {
      ResultSetMappingDefinition mappingDefinition = new ResultSetMappingDefinition(name);
      int count = 0;
      foreach (object obj in items ?? new object[0])
      {
        ++count;
        INativeSQLQueryReturn queryReturn = this.CreateQueryReturn(obj, count);
        if (queryReturn != null)
          mappingDefinition.AddQueryReturn(queryReturn);
      }
      return mappingDefinition;
    }

    private INativeSQLQueryReturn CreateQueryReturn(object item, int count)
    {
      HbmLoadCollection loadCollectionSchema = item as HbmLoadCollection;
      HbmReturn returnSchema = item as HbmReturn;
      HbmReturnJoin returnJoinSchema = item as HbmReturnJoin;
      if (item is HbmReturnScalar returnScalarSchema)
        return this.CreateScalarReturn(returnScalarSchema);
      if (returnSchema != null)
        return (INativeSQLQueryReturn) this.CreateReturn(returnSchema, count);
      if (returnJoinSchema != null)
        return (INativeSQLQueryReturn) this.CreateJoinReturn(returnJoinSchema);
      return loadCollectionSchema != null ? (INativeSQLQueryReturn) this.CreateLoadCollectionReturn(loadCollectionSchema) : (INativeSQLQueryReturn) null;
    }

    private INativeSQLQueryReturn CreateScalarReturn(HbmReturnScalar returnScalarSchema)
    {
      IDictionary<string, string> parameters = (IDictionary<string, string>) null;
      TypeDef typeDef = this.mappings.GetTypeDef(returnScalarSchema.type);
      string typeName;
      if (typeDef != null)
      {
        typeName = typeDef.TypeClass;
        parameters = typeDef.Parameters;
      }
      else
        typeName = returnScalarSchema.type;
      return (INativeSQLQueryReturn) new NativeSQLQueryScalarReturn(returnScalarSchema.column, TypeFactory.HeuristicType(typeName, parameters) ?? throw new MappingException("could not interpret type: " + returnScalarSchema.type));
    }

    private NativeSQLQueryRootReturn CreateReturn(HbmReturn returnSchema, int count)
    {
      string str1 = returnSchema.alias;
      if (StringHelper.IsEmpty(str1))
        str1 = "alias_" + (object) count;
      if (string.IsNullOrEmpty(returnSchema.@class) && string.IsNullOrEmpty(returnSchema.entityname))
        throw new MappingException("<return alias='" + str1 + "'> must specify either a class or entity-name");
      string str2 = returnSchema.entityname ?? Binder.GetClassName(returnSchema.@class, this.mappings);
      LockMode lockMode = ResultSetMappingBinder.GetLockMode(returnSchema.lockmode);
      PersistentClass pc = this.mappings.GetClass(str2);
      IDictionary<string, string[]> propertyResults = this.BindPropertyResults(str1, returnSchema.returndiscriminator, returnSchema.returnproperty, pc);
      return new NativeSQLQueryRootReturn(str1, str2, propertyResults, lockMode);
    }

    private NativeSQLQueryJoinReturn CreateJoinReturn(HbmReturnJoin returnJoinSchema)
    {
      int length = returnJoinSchema.property.LastIndexOf('.');
      if (length == -1)
        throw new MappingException("Role attribute for sql query return [alias=" + returnJoinSchema.alias + "] not formatted correctly {owningAlias.propertyName}");
      string ownerAlias = returnJoinSchema.property.Substring(0, length);
      string ownerProperty = returnJoinSchema.property.Substring(length + 1);
      IDictionary<string, string[]> propertyResults = this.BindPropertyResults(returnJoinSchema.alias, (HbmReturnDiscriminator) null, returnJoinSchema.returnproperty, (PersistentClass) null);
      return new NativeSQLQueryJoinReturn(returnJoinSchema.alias, ownerAlias, ownerProperty, propertyResults, ResultSetMappingBinder.GetLockMode(returnJoinSchema.lockmode));
    }

    private NativeSQLQueryCollectionReturn CreateLoadCollectionReturn(
      HbmLoadCollection loadCollectionSchema)
    {
      int length = loadCollectionSchema.role.LastIndexOf('.');
      if (length == -1)
        throw new MappingException("Collection attribute for sql query return [alias=" + loadCollectionSchema.alias + "] not formatted correctly {OwnerClassName.propertyName}");
      string nameWithoutAssembly = this.GetClassNameWithoutAssembly(loadCollectionSchema.role.Substring(0, length));
      string ownerProperty = loadCollectionSchema.role.Substring(length + 1);
      IDictionary<string, string[]> propertyResults = this.BindPropertyResults(loadCollectionSchema.alias, (HbmReturnDiscriminator) null, loadCollectionSchema.returnproperty, (PersistentClass) null);
      return new NativeSQLQueryCollectionReturn(loadCollectionSchema.alias, nameWithoutAssembly, ownerProperty, propertyResults, ResultSetMappingBinder.GetLockMode(loadCollectionSchema.lockmode));
    }

    private IDictionary<string, string[]> BindPropertyResults(
      string alias,
      HbmReturnDiscriminator discriminatorSchema,
      HbmReturnProperty[] returnProperties,
      PersistentClass pc)
    {
      Dictionary<string, string[]> dictionary1 = new Dictionary<string, string[]>();
      if (discriminatorSchema != null)
        dictionary1["class"] = ResultSetMappingBinder.GetResultColumns(discriminatorSchema).ToArray();
      List<HbmReturnProperty> hbmReturnPropertyList = new List<HbmReturnProperty>();
      List<string> propertyNames = new List<string>();
      foreach (HbmReturnProperty hbmReturnProperty in returnProperties ?? new HbmReturnProperty[0])
      {
        string name1 = hbmReturnProperty.name;
        if (pc == null || name1.IndexOf('.') == -1)
        {
          hbmReturnPropertyList.Add(hbmReturnProperty);
          propertyNames.Add(name1);
        }
        else
        {
          int length = name1.LastIndexOf('.');
          string propertyPath = name1.Substring(0, length);
          IValue obj = pc.GetRecursiveProperty(propertyPath).Value;
          IEnumerable<Property> propertyIterator;
          switch (obj)
          {
            case Component _:
              propertyIterator = ((Component) obj).PropertyIterator;
              break;
            case ToOne _:
              ToOne toOne = (ToOne) obj;
              PersistentClass persistentClass = this.mappings.GetClass(toOne.ReferencedEntityName);
              if (toOne.ReferencedPropertyName != null)
              {
                try
                {
                  propertyIterator = ((Component) persistentClass.GetRecursiveProperty(toOne.ReferencedPropertyName).Value).PropertyIterator;
                  break;
                }
                catch (InvalidCastException ex)
                {
                  throw new MappingException("dotted notation reference neither a component nor a many/one to one", (Exception) ex);
                }
              }
              else
              {
                try
                {
                  propertyIterator = ((Component) persistentClass.IdentifierProperty.Value).PropertyIterator;
                  break;
                }
                catch (InvalidCastException ex)
                {
                  throw new MappingException("dotted notation reference neither a component nor a many/one to one", (Exception) ex);
                }
              }
            default:
              throw new MappingException("dotted notation reference neither a component nor a many/one to one");
          }
          bool flag = false;
          List<string> stringList = new List<string>();
          foreach (Property property in propertyIterator)
          {
            string name2 = property.Name;
            string str = propertyPath + (object) '.' + name2;
            if (flag)
              stringList.Add(str);
            if (name1.Equals(str))
              flag = true;
          }
          int index1 = propertyNames.Count;
          int count = stringList.Count;
          for (int index2 = 0; index2 < count; ++index2)
          {
            string follower = stringList[index2];
            int matchingProperty = ResultSetMappingBinder.GetIndexOfFirstMatchingProperty((IList) propertyNames, follower);
            index1 = matchingProperty == -1 || matchingProperty >= index1 ? index1 : matchingProperty;
          }
          propertyNames.Insert(index1, name1);
          hbmReturnPropertyList.Insert(index1, hbmReturnProperty);
        }
      }
      ISet<string> set = (ISet<string>) new HashedSet<string>();
      foreach (HbmReturnProperty returnPropertySchema in hbmReturnPropertyList)
      {
        string name = returnPropertySchema.name;
        if ("class".Equals(name))
          throw new MappingException("class is not a valid property name to use in a <return-property>, use <return-discriminator> instead");
        List<string> resultColumns = ResultSetMappingBinder.GetResultColumns(returnPropertySchema);
        if (resultColumns.Count == 0)
          throw new MappingException("return-property for alias " + alias + " must specify at least one column or return-column name");
        if (set.Contains(name))
          throw new MappingException("duplicate return-property for property " + name + " on alias " + alias);
        set.Add(name);
        string key = name;
        string[] to;
        if (!dictionary1.TryGetValue(key, out to))
          dictionary1[key] = resultColumns.ToArray();
        else
          ArrayHelper.AddAll((IList) to, (IList) resultColumns);
      }
      Dictionary<string, string[]> dictionary2 = new Dictionary<string, string[]>();
      foreach (KeyValuePair<string, string[]> keyValuePair in dictionary1)
        dictionary2[keyValuePair.Key] = keyValuePair.Value;
      return dictionary2.Count != 0 ? (IDictionary<string, string[]>) dictionary2 : (IDictionary<string, string[]>) new CollectionHelper.EmptyMapClass<string, string[]>();
    }

    private static List<string> GetResultColumns(HbmReturnProperty returnPropertySchema)
    {
      List<string> resultColumns = new List<string>();
      string str = ResultSetMappingBinder.Unquote(returnPropertySchema.column);
      if (str != null)
        resultColumns.Add(str);
      foreach (HbmReturnColumn hbmReturnColumn in returnPropertySchema.returncolumn ?? new HbmReturnColumn[0])
        resultColumns.Add(ResultSetMappingBinder.Unquote(hbmReturnColumn.name));
      return resultColumns;
    }

    private static List<string> GetResultColumns(HbmReturnDiscriminator discriminatorSchema)
    {
      string str = ResultSetMappingBinder.Unquote(discriminatorSchema.column);
      List<string> resultColumns = new List<string>();
      if (str != null)
        resultColumns.Add(str);
      return resultColumns;
    }

    private static LockMode GetLockMode(HbmLockMode lockMode)
    {
      switch (lockMode)
      {
        case HbmLockMode.None:
          return LockMode.None;
        case HbmLockMode.Read:
          return LockMode.Read;
        case HbmLockMode.Upgrade:
          return LockMode.Upgrade;
        case HbmLockMode.UpgradeNowait:
          return LockMode.UpgradeNoWait;
        case HbmLockMode.Write:
          return LockMode.Write;
        default:
          throw new MappingException("unknown lockMode " + (object) lockMode);
      }
    }

    private static int GetIndexOfFirstMatchingProperty(IList propertyNames, string follower)
    {
      int count = propertyNames.Count;
      for (int index = 0; index < count; ++index)
      {
        if (((string) propertyNames[index]).StartsWith(follower))
          return index;
      }
      return -1;
    }

    private static string Unquote(string name)
    {
      if (name != null && name[0] == '`')
        name = name.Substring(1, name.Length - 2);
      return name;
    }

    private string GetClassNameWithoutAssembly(string unqualifiedName)
    {
      return TypeNameParser.Parse(unqualifiedName, this.mappings.DefaultNamespace, this.mappings.DefaultAssembly).Type;
    }
  }
}
