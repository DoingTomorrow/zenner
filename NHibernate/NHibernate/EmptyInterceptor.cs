// Decompiled with JetBrains decompiler
// Type: NHibernate.EmptyInterceptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class EmptyInterceptor : IInterceptor
  {
    public virtual void OnDelete(
      object entity,
      object id,
      object[] state,
      string[] propertyNames,
      IType[] types)
    {
    }

    public virtual void OnCollectionRecreate(object collection, object key)
    {
    }

    public virtual void OnCollectionRemove(object collection, object key)
    {
    }

    public virtual void OnCollectionUpdate(object collection, object key)
    {
    }

    public virtual bool OnFlushDirty(
      object entity,
      object id,
      object[] currentState,
      object[] previousState,
      string[] propertyNames,
      IType[] types)
    {
      return false;
    }

    public virtual bool OnLoad(
      object entity,
      object id,
      object[] state,
      string[] propertyNames,
      IType[] types)
    {
      return false;
    }

    public virtual bool OnSave(
      object entity,
      object id,
      object[] state,
      string[] propertyNames,
      IType[] types)
    {
      return false;
    }

    public virtual void PostFlush(ICollection entities)
    {
    }

    public virtual void PreFlush(ICollection entitites)
    {
    }

    public virtual bool? IsTransient(object entity) => new bool?();

    public virtual object Instantiate(string clazz, EntityMode entityMode, object id)
    {
      return (object) null;
    }

    public virtual string GetEntityName(object entity) => (string) null;

    public virtual object GetEntity(string entityName, object id) => (object) null;

    public virtual int[] FindDirty(
      object entity,
      object id,
      object[] currentState,
      object[] previousState,
      string[] propertyNames,
      IType[] types)
    {
      return (int[]) null;
    }

    public virtual void AfterTransactionBegin(ITransaction tx)
    {
    }

    public virtual void BeforeTransactionCompletion(ITransaction tx)
    {
    }

    public virtual void AfterTransactionCompletion(ITransaction tx)
    {
    }

    public virtual void SetSession(ISession session)
    {
    }

    public virtual SqlString OnPrepareStatement(SqlString sql) => sql;
  }
}
