// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.PersistenceSpecification`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;

#nullable disable
namespace FluentNHibernate.Testing
{
  public class PersistenceSpecification<T>
  {
    protected readonly List<Property<T>> allProperties = new List<Property<T>>();
    private readonly ISession currentSession;
    private readonly IEqualityComparer entityEqualityComparer;
    private readonly bool hasExistingTransaction;

    public PersistenceSpecification(ISessionSource source)
      : this(source.CreateSession())
    {
    }

    public PersistenceSpecification(ISessionSource source, IEqualityComparer entityEqualityComparer)
      : this(source.CreateSession(), entityEqualityComparer)
    {
    }

    public PersistenceSpecification(ISession session)
      : this(session, (IEqualityComparer) null)
    {
    }

    public PersistenceSpecification(ISession session, IEqualityComparer entityEqualityComparer)
    {
      this.currentSession = session;
      this.hasExistingTransaction = this.currentSession.Transaction != null && this.currentSession.Transaction.IsActive || Transaction.Current != (Transaction) null;
      this.entityEqualityComparer = entityEqualityComparer;
    }

    public T VerifyTheMappings()
    {
      return this.VerifyTheMappings(typeof (T).InstantiateUsingParameterlessConstructor<T>());
    }

    public T VerifyTheMappings(T first)
    {
      this.allProperties.ForEach((Action<Property<T>>) (p => p.SetValue(first)));
      this.TransactionalSave((object) first);
      object identifier = this.currentSession.GetIdentifier((object) first);
      this.currentSession.Flush();
      this.currentSession.Clear();
      T second = this.currentSession.Get<T>(identifier);
      this.allProperties.ForEach((Action<Property<T>>) (p => p.CheckValue((object) second)));
      return second;
    }

    public void TransactionalSave(object propertyValue)
    {
      if (this.hasExistingTransaction)
      {
        this.currentSession.Save(propertyValue);
      }
      else
      {
        using (ITransaction transaction = this.currentSession.BeginTransaction())
        {
          this.currentSession.Save(propertyValue);
          transaction.Commit();
        }
      }
    }

    public PersistenceSpecification<T> RegisterCheckedProperty(Property<T> property)
    {
      return this.RegisterCheckedProperty(property, (IEqualityComparer) null);
    }

    public PersistenceSpecification<T> RegisterCheckedProperty(
      Property<T> property,
      IEqualityComparer equalityComparer)
    {
      property.EntityEqualityComparer = equalityComparer ?? this.entityEqualityComparer;
      this.allProperties.Add(property);
      property.HasRegistered(this);
      return this;
    }
  }
}
