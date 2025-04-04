// Decompiled with JetBrains decompiler
// Type: NHibernate.Intercept.AbstractFieldInterceptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Proxy;
using System;

#nullable disable
namespace NHibernate.Intercept
{
  [Serializable]
  public abstract class AbstractFieldInterceptor : IFieldInterceptor
  {
    public static readonly object InvokeImplementation = new object();
    [NonSerialized]
    private ISessionImplementor session;
    private ISet<string> uninitializedFields;
    private readonly ISet<string> unwrapProxyFieldNames;
    private readonly ISet<string> loadedUnwrapProxyFieldNames = (ISet<string>) new HashedSet<string>();
    private readonly string entityName;
    private readonly Type mappedClass;
    [NonSerialized]
    private bool initializing;
    private bool isDirty;

    protected internal AbstractFieldInterceptor(
      ISessionImplementor session,
      ISet<string> uninitializedFields,
      ISet<string> unwrapProxyFieldNames,
      string entityName,
      Type mappedClass)
    {
      this.session = session;
      this.uninitializedFields = uninitializedFields;
      this.unwrapProxyFieldNames = unwrapProxyFieldNames ?? (ISet<string>) new HashedSet<string>();
      this.entityName = entityName;
      this.mappedClass = mappedClass;
    }

    public bool IsDirty => this.isDirty;

    public ISessionImplementor Session
    {
      get => this.session;
      set => this.session = value;
    }

    public bool IsInitialized
    {
      get => this.uninitializedFields == null || this.uninitializedFields.Count == 0;
    }

    public bool IsInitializedField(string field)
    {
      return !this.IsUninitializedProperty(field) && !this.IsUninitializedAssociation(field);
    }

    public void MarkDirty() => this.isDirty = true;

    public void ClearDirty() => this.isDirty = false;

    public string EntityName => this.entityName;

    public Type MappedClass => this.mappedClass;

    public ISet<string> UninitializedFields => this.uninitializedFields;

    public bool Initializing => this.initializing;

    public object Intercept(object target, string fieldName, object value)
    {
      if (this.initializing)
        return AbstractFieldInterceptor.InvokeImplementation;
      if (this.IsInitializedField(fieldName))
        return value;
      if (this.session == null)
        throw new LazyInitializationException(this.EntityName, (object) null, string.Format("entity with lazy properties is not associated with a session. entity-name:'{0}' property:'{1}'", (object) this.EntityName, (object) fieldName));
      if (!this.session.IsOpen || !this.session.IsConnected)
        throw new LazyInitializationException(this.EntityName, (object) null, string.Format("session is not connected. entity-name:'{0}' property:'{1}'", (object) this.EntityName, (object) fieldName));
      if (this.IsUninitializedProperty(fieldName))
        return this.InitializeField(fieldName, target);
      return value.IsProxy() && this.IsUninitializedAssociation(fieldName) ? this.InitializeOrGetAssociation(value as INHibernateProxy, fieldName) : AbstractFieldInterceptor.InvokeImplementation;
    }

    private bool IsUninitializedAssociation(string fieldName)
    {
      return this.unwrapProxyFieldNames.Contains(fieldName) && !this.loadedUnwrapProxyFieldNames.Contains(fieldName);
    }

    private bool IsUninitializedProperty(string fieldName)
    {
      return this.uninitializedFields != null && this.uninitializedFields.Contains(fieldName);
    }

    private object InitializeOrGetAssociation(INHibernateProxy value, string fieldName)
    {
      if (value.HibernateLazyInitializer.IsUninitialized)
      {
        value.HibernateLazyInitializer.Initialize();
        value.HibernateLazyInitializer.Unwrap = true;
        this.loadedUnwrapProxyFieldNames.Add(fieldName);
      }
      return value.HibernateLazyInitializer.GetImplementation(this.session);
    }

    private object InitializeField(string fieldName, object target)
    {
      this.initializing = true;
      object obj;
      try
      {
        obj = ((ILazyPropertyInitializer) this.session.Factory.GetEntityPersister(this.entityName)).InitializeLazyProperty(fieldName, target, this.session);
      }
      finally
      {
        this.initializing = false;
      }
      this.uninitializedFields = (ISet<string>) null;
      return obj;
    }
  }
}
