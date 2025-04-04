// Decompiled with JetBrains decompiler
// Type: NHibernate.IInterceptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections;

#nullable disable
namespace NHibernate
{
  public interface IInterceptor
  {
    bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types);

    bool OnFlushDirty(
      object entity,
      object id,
      object[] currentState,
      object[] previousState,
      string[] propertyNames,
      IType[] types);

    bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types);

    void OnDelete(
      object entity,
      object id,
      object[] state,
      string[] propertyNames,
      IType[] types);

    void OnCollectionRecreate(object collection, object key);

    void OnCollectionRemove(object collection, object key);

    void OnCollectionUpdate(object collection, object key);

    void PreFlush(ICollection entities);

    void PostFlush(ICollection entities);

    bool? IsTransient(object entity);

    int[] FindDirty(
      object entity,
      object id,
      object[] currentState,
      object[] previousState,
      string[] propertyNames,
      IType[] types);

    object Instantiate(string entityName, EntityMode entityMode, object id);

    string GetEntityName(object entity);

    object GetEntity(string entityName, object id);

    void AfterTransactionBegin(ITransaction tx);

    void BeforeTransactionCompletion(ITransaction tx);

    void AfterTransactionCompletion(ITransaction tx);

    SqlString OnPrepareStatement(SqlString sql);

    void SetSession(ISession session);
  }
}
