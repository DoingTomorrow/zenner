// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.IndexNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class IndexNode(IToken token) : FromReferenceNode(token)
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (IndexNode));

    public override void SetScalarColumnText(int i)
    {
      throw new InvalidOperationException("An IndexNode cannot generate column text!");
    }

    public override void ResolveIndex(IASTNode parent) => throw new InvalidOperationException();

    public override void Resolve(
      bool generateJoin,
      bool implicitJoin,
      string classAlias,
      IASTNode parent)
    {
      if (this.IsResolved)
        return;
      FromReferenceNode child1 = (FromReferenceNode) this.GetChild(0);
      SessionFactoryHelperExtensions sessionFactoryHelper = this.SessionFactoryHelper;
      child1.ResolveIndex((IASTNode) this);
      IType dataType = child1.DataType;
      string role = dataType.IsCollectionType ? ((CollectionType) dataType).Role : throw new SemanticException("The [] operator cannot be applied to type " + (object) dataType);
      IQueryableCollection queryableCollection = sessionFactoryHelper.RequireQueryableCollection(role);
      if (!queryableCollection.HasIndex)
        throw new QueryException("unindexed fromElement before []: " + child1.Path);
      FromElement fromElement1 = child1.FromElement;
      string tableAlias = fromElement1.TableAlias;
      FromClause fromClause = fromElement1.FromClause;
      string path = child1.Path;
      FromElement fromElement2 = fromClause.FindCollectionJoin(path);
      if (fromElement2 == null)
      {
        fromElement2 = new FromElementFactory(fromClause, fromElement1, path).CreateCollectionElementsJoin(queryableCollection, tableAlias);
        if (IndexNode.Log.IsDebugEnabled)
          IndexNode.Log.Debug((object) ("No FROM element found for the elements of collection join path " + path + ", created " + (object) fromElement2));
      }
      else if (IndexNode.Log.IsDebugEnabled)
        IndexNode.Log.Debug((object) ("FROM element found for collection join path " + path));
      this.FromElement = fromElement1;
      IASTNode child2 = this.GetChild(1);
      if (child2 == null)
        throw new QueryException("No index value!");
      string str1 = tableAlias;
      if (fromElement2.CollectionTableAlias != null)
        str1 = fromElement2.CollectionTableAlias;
      JoinSequence joinSequence = fromElement1.JoinSequence;
      string[] indexColumnNames = queryableCollection.IndexColumnNames;
      if (indexColumnNames.Length != 1)
        throw new QueryException("composite-index appears in []: " + child1.Path);
      SqlGenerator sqlGenerator = new SqlGenerator(this.SessionFactoryHelper.Factory, (ITreeNodeStream) new CommonTreeNodeStream((object) child2));
      try
      {
        sqlGenerator.simpleExpr();
      }
      catch (RecognitionException ex)
      {
        throw new QueryException(ex.Message, (Exception) ex);
      }
      string str2 = sqlGenerator.GetSQL().ToString();
      joinSequence.AddCondition(new SqlString(str1 + (object) '.' + indexColumnNames[0] + " = " + str2));
      IList<IParameterSpecification> collectedParameters = sqlGenerator.GetCollectedParameters();
      if (collectedParameters != null)
      {
        switch (collectedParameters.Count)
        {
          case 0:
            break;
          case 1:
            IParameterSpecification indexCollectionSelectorParamSpec = collectedParameters[0];
            indexCollectionSelectorParamSpec.ExpectedType = queryableCollection.IndexType;
            fromElement1.SetIndexCollectionSelectorParamSpec(indexCollectionSelectorParamSpec);
            break;
          default:
            fromElement1.SetIndexCollectionSelectorParamSpec((IParameterSpecification) new AggregatedIndexCollectionSelectorParameterSpecifications(collectedParameters));
            break;
        }
      }
      this.Text = queryableCollection.GetElementColumnNames(tableAlias)[0];
      this.IsResolved = true;
    }

    public override void PrepareForDot(string propertyName)
    {
      FromElement fromElement = this.FromElement;
      IQueryableCollection queryableCollection = fromElement != null ? fromElement.QueryableCollection : throw new InvalidOperationException("No FROM element for index operator!");
      if (queryableCollection == null || queryableCollection.IsOneToMany)
        return;
      string path = ((FromReferenceNode) this.GetChild(0)).Path + "[]." + propertyName;
      if (IndexNode.Log.IsDebugEnabled)
        IndexNode.Log.Debug((object) ("Creating join for many-to-many elements for " + path));
      this.FromElement = new FromElementFactory(fromElement.FromClause, fromElement, path).CreateElementJoin(queryableCollection);
    }
  }
}
