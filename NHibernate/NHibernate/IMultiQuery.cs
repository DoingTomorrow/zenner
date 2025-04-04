// Decompiled with JetBrains decompiler
// Type: NHibernate.IMultiQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate
{
  public interface IMultiQuery
  {
    IList List();

    IMultiQuery Add(System.Type resultGenericListType, IQuery query);

    IMultiQuery Add<T>(IQuery query);

    IMultiQuery Add<T>(string key, IQuery query);

    IMultiQuery Add<T>(string key, string hql);

    IMultiQuery Add<T>(string hql);

    IMultiQuery AddNamedQuery<T>(string queryName);

    IMultiQuery AddNamedQuery<T>(string key, string queryName);

    IMultiQuery Add(string key, IQuery query);

    IMultiQuery Add(IQuery query);

    IMultiQuery Add(string key, string hql);

    IMultiQuery Add(string hql);

    IMultiQuery AddNamedQuery(string queryName);

    IMultiQuery AddNamedQuery(string key, string queryName);

    IMultiQuery SetCacheable(bool cacheable);

    IMultiQuery SetCacheRegion(string region);

    IMultiQuery SetForceCacheRefresh(bool forceCacheRefresh);

    IMultiQuery SetTimeout(int timeout);

    IMultiQuery SetParameter(string name, object val, IType type);

    IMultiQuery SetParameter(string name, object val);

    IMultiQuery SetParameterList(string name, ICollection vals, IType type);

    IMultiQuery SetParameterList(string name, ICollection vals);

    IMultiQuery SetAnsiString(string name, string val);

    IMultiQuery SetBinary(string name, byte[] val);

    IMultiQuery SetBoolean(string name, bool val);

    IMultiQuery SetByte(string name, byte val);

    IMultiQuery SetCharacter(string name, char val);

    IMultiQuery SetDateTime(string name, DateTime val);

    IMultiQuery SetDateTime2(string name, DateTime val);

    IMultiQuery SetTimeSpan(string name, TimeSpan val);

    IMultiQuery SetTimeAsTimeSpan(string name, TimeSpan val);

    IMultiQuery SetDateTimeOffset(string name, DateTimeOffset val);

    IMultiQuery SetDecimal(string name, Decimal val);

    IMultiQuery SetDouble(string name, double val);

    IMultiQuery SetEntity(string name, object val);

    IMultiQuery SetEnum(string name, Enum val);

    IMultiQuery SetInt16(string name, short val);

    IMultiQuery SetInt32(string name, int val);

    IMultiQuery SetInt64(string name, long val);

    IMultiQuery SetSingle(string name, float val);

    IMultiQuery SetString(string name, string val);

    IMultiQuery SetGuid(string name, Guid val);

    IMultiQuery SetTime(string name, DateTime val);

    IMultiQuery SetTimestamp(string name, DateTime val);

    IMultiQuery SetFlushMode(FlushMode mode);

    IMultiQuery SetResultTransformer(IResultTransformer transformer);

    object GetResult(string key);
  }
}
