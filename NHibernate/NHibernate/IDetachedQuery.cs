// Decompiled with JetBrains decompiler
// Type: NHibernate.IDetachedQuery
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
  public interface IDetachedQuery
  {
    IQuery GetExecutableQuery(ISession session);

    IDetachedQuery SetMaxResults(int maxResults);

    IDetachedQuery SetFirstResult(int firstResult);

    IDetachedQuery SetCacheable(bool cacheable);

    IDetachedQuery SetCacheRegion(string cacheRegion);

    IDetachedQuery SetReadOnly(bool readOnly);

    IDetachedQuery SetTimeout(int timeout);

    IDetachedQuery SetFetchSize(int fetchSize);

    void SetLockMode(string alias, LockMode lockMode);

    IDetachedQuery SetComment(string comment);

    IDetachedQuery SetParameter(int position, object val, IType type);

    IDetachedQuery SetParameter(string name, object val, IType type);

    IDetachedQuery SetParameter(int position, object val);

    IDetachedQuery SetParameter(string name, object val);

    IDetachedQuery SetParameterList(string name, ICollection vals, IType type);

    IDetachedQuery SetParameterList(string name, ICollection vals);

    IDetachedQuery SetProperties(object obj);

    IDetachedQuery SetAnsiString(int position, string val);

    IDetachedQuery SetAnsiString(string name, string val);

    IDetachedQuery SetBinary(int position, byte[] val);

    IDetachedQuery SetBinary(string name, byte[] val);

    IDetachedQuery SetBoolean(int position, bool val);

    IDetachedQuery SetBoolean(string name, bool val);

    IDetachedQuery SetByte(int position, byte val);

    IDetachedQuery SetByte(string name, byte val);

    IDetachedQuery SetCharacter(int position, char val);

    IDetachedQuery SetCharacter(string name, char val);

    IDetachedQuery SetDateTime(int position, DateTime val);

    IDetachedQuery SetDateTime(string name, DateTime val);

    IDetachedQuery SetDecimal(int position, Decimal val);

    IDetachedQuery SetDecimal(string name, Decimal val);

    IDetachedQuery SetDouble(int position, double val);

    IDetachedQuery SetDouble(string name, double val);

    IDetachedQuery SetEntity(int position, object val);

    IDetachedQuery SetEntity(string name, object val);

    IDetachedQuery SetEnum(int position, Enum val);

    IDetachedQuery SetEnum(string name, Enum val);

    IDetachedQuery SetInt16(int position, short val);

    IDetachedQuery SetInt16(string name, short val);

    IDetachedQuery SetInt32(int position, int val);

    IDetachedQuery SetInt32(string name, int val);

    IDetachedQuery SetInt64(int position, long val);

    IDetachedQuery SetInt64(string name, long val);

    IDetachedQuery SetSingle(int position, float val);

    IDetachedQuery SetSingle(string name, float val);

    IDetachedQuery SetString(int position, string val);

    IDetachedQuery SetString(string name, string val);

    IDetachedQuery SetTime(int position, DateTime val);

    IDetachedQuery SetTime(string name, DateTime val);

    IDetachedQuery SetTimestamp(int position, DateTime val);

    IDetachedQuery SetTimestamp(string name, DateTime val);

    IDetachedQuery SetGuid(int position, Guid val);

    IDetachedQuery SetGuid(string name, Guid val);

    IDetachedQuery SetFlushMode(FlushMode flushMode);

    IDetachedQuery SetResultTransformer(IResultTransformer resultTransformer);

    IDetachedQuery SetIgnoreUknownNamedParameters(bool ignoredUnknownNamedParameters);

    IDetachedQuery SetCacheMode(CacheMode cacheMode);
  }
}
