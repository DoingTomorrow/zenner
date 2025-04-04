// Decompiled with JetBrains decompiler
// Type: NHibernate.IQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate
{
  public interface IQuery
  {
    string QueryString { get; }

    IType[] ReturnTypes { get; }

    string[] ReturnAliases { get; }

    string[] NamedParameters { get; }

    bool IsReadOnly { get; }

    IEnumerable Enumerable();

    IEnumerable<T> Enumerable<T>();

    IList List();

    void List(IList results);

    IList<T> List<T>();

    object UniqueResult();

    T UniqueResult<T>();

    int ExecuteUpdate();

    IQuery SetMaxResults(int maxResults);

    IQuery SetFirstResult(int firstResult);

    IQuery SetReadOnly(bool readOnly);

    IQuery SetCacheable(bool cacheable);

    IQuery SetCacheRegion(string cacheRegion);

    IQuery SetTimeout(int timeout);

    IQuery SetFetchSize(int fetchSize);

    IQuery SetLockMode(string alias, LockMode lockMode);

    IQuery SetComment(string comment);

    IQuery SetFlushMode(FlushMode flushMode);

    IQuery SetCacheMode(CacheMode cacheMode);

    IQuery SetParameter(int position, object val, IType type);

    IQuery SetParameter(string name, object val, IType type);

    IQuery SetParameter<T>(int position, T val);

    IQuery SetParameter<T>(string name, T val);

    IQuery SetParameter(int position, object val);

    IQuery SetParameter(string name, object val);

    IQuery SetParameterList(string name, IEnumerable vals, IType type);

    IQuery SetParameterList(string name, IEnumerable vals);

    IQuery SetProperties(object obj);

    IQuery SetAnsiString(int position, string val);

    IQuery SetAnsiString(string name, string val);

    IQuery SetBinary(int position, byte[] val);

    IQuery SetBinary(string name, byte[] val);

    IQuery SetBoolean(int position, bool val);

    IQuery SetBoolean(string name, bool val);

    IQuery SetByte(int position, byte val);

    IQuery SetByte(string name, byte val);

    IQuery SetCharacter(int position, char val);

    IQuery SetCharacter(string name, char val);

    IQuery SetDateTime(int position, DateTime val);

    IQuery SetDateTime(string name, DateTime val);

    IQuery SetDateTime2(int position, DateTime val);

    IQuery SetDateTime2(string name, DateTime val);

    IQuery SetTimeSpan(int position, TimeSpan val);

    IQuery SetTimeSpan(string name, TimeSpan val);

    IQuery SetTimeAsTimeSpan(int position, TimeSpan val);

    IQuery SetTimeAsTimeSpan(string name, TimeSpan val);

    IQuery SetDateTimeOffset(int position, DateTimeOffset val);

    IQuery SetDateTimeOffset(string name, DateTimeOffset val);

    IQuery SetDecimal(int position, Decimal val);

    IQuery SetDecimal(string name, Decimal val);

    IQuery SetDouble(int position, double val);

    IQuery SetDouble(string name, double val);

    IQuery SetEnum(int position, Enum val);

    IQuery SetEnum(string name, Enum val);

    IQuery SetInt16(int position, short val);

    IQuery SetInt16(string name, short val);

    IQuery SetInt32(int position, int val);

    IQuery SetInt32(string name, int val);

    IQuery SetInt64(int position, long val);

    IQuery SetInt64(string name, long val);

    IQuery SetSingle(int position, float val);

    IQuery SetSingle(string name, float val);

    IQuery SetString(int position, string val);

    IQuery SetString(string name, string val);

    IQuery SetTime(int position, DateTime val);

    IQuery SetTime(string name, DateTime val);

    IQuery SetTimestamp(int position, DateTime val);

    IQuery SetTimestamp(string name, DateTime val);

    IQuery SetGuid(int position, Guid val);

    IQuery SetGuid(string name, Guid val);

    IQuery SetEntity(int position, object val);

    IQuery SetEntity(string name, object val);

    IQuery SetResultTransformer(IResultTransformer resultTransformer);

    IEnumerable<T> Future<T>();

    IFutureValue<T> FutureValue<T>();
  }
}
