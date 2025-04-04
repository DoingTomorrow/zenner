// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.FetchRequestCollection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.EagerFetching
{
  public class FetchRequestCollection
  {
    private readonly Dictionary<MemberInfo, FetchRequestBase> _fetchRequests = new Dictionary<MemberInfo, FetchRequestBase>();

    public IEnumerable<FetchRequestBase> FetchRequests
    {
      get => (IEnumerable<FetchRequestBase>) this._fetchRequests.Values;
    }

    public FetchRequestBase GetOrAddFetchRequest(FetchRequestBase fetchRequest)
    {
      ArgumentUtility.CheckNotNull<FetchRequestBase>(nameof (fetchRequest), fetchRequest);
      FetchRequestBase orAddFetchRequest;
      if (this._fetchRequests.TryGetValue(fetchRequest.RelationMember, out orAddFetchRequest))
        return orAddFetchRequest;
      this._fetchRequests.Add(fetchRequest.RelationMember, fetchRequest);
      return fetchRequest;
    }
  }
}
