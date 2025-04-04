// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.FetchRequestBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.EagerFetching
{
  public abstract class FetchRequestBase : SequenceTypePreservingResultOperatorBase
  {
    private readonly FetchRequestCollection _innerFetchRequestCollection = new FetchRequestCollection();
    private MemberInfo _relationMember;

    protected FetchRequestBase(MemberInfo relationMember)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (relationMember), relationMember);
      this._relationMember = relationMember;
    }

    public MemberInfo RelationMember
    {
      get => this._relationMember;
      set => this._relationMember = ArgumentUtility.CheckNotNull<MemberInfo>(nameof (value), value);
    }

    public IEnumerable<FetchRequestBase> InnerFetchRequests
    {
      get => this._innerFetchRequestCollection.FetchRequests;
    }

    public virtual QueryModel CreateFetchQueryModel(QueryModel sourceItemQueryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (sourceItemQueryModel), sourceItemQueryModel);
      string newName = sourceItemQueryModel.GetNewName("#fetch");
      QueryModel subQuery;
      try
      {
        subQuery = sourceItemQueryModel.ConvertToSubQuery(newName);
      }
      catch (InvalidOperationException ex)
      {
        throw new ArgumentException(string.Format("The given source query model cannot be used to fetch the relation member '{0}': {1}", (object) this.RelationMember.Name, (object) ex.Message), nameof (sourceItemQueryModel), (Exception) ex);
      }
      if (!this.RelationMember.DeclaringType.IsAssignableFrom(subQuery.MainFromClause.ItemType))
        throw new ArgumentException(string.Format("The given source query model selects items that do not match the fetch request. In order to fetch the relation member '{0}', the query must yield objects of type '{1}', but it yields '{2}'.", (object) this.RelationMember.Name, (object) this.RelationMember.DeclaringType, (object) subQuery.MainFromClause.ItemType), nameof (sourceItemQueryModel));
      this.ModifyFetchQueryModel(subQuery);
      return subQuery;
    }

    protected abstract void ModifyFetchQueryModel(QueryModel fetchQueryModel);

    public FetchRequestBase GetOrAddInnerFetchRequest(FetchRequestBase fetchRequest)
    {
      ArgumentUtility.CheckNotNull<FetchRequestBase>(nameof (fetchRequest), fetchRequest);
      return this._innerFetchRequestCollection.GetOrAddFetchRequest(fetchRequest);
    }

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return input;
    }

    public override string ToString()
    {
      return SeparatedStringBuilder.Build<string>(".Then", ((IEnumerable<string>) new string[1]
      {
        string.Format("Fetch ({0}.{1})", (object) this._relationMember.DeclaringType.Name, (object) this._relationMember.Name)
      }).Concat<string>(this.InnerFetchRequests.Select<FetchRequestBase, string>((Func<FetchRequestBase, string>) (request => request.ToString()))));
    }
  }
}
