// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.MessageHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Text;

#nullable disable
namespace NHibernate.Impl
{
  public sealed class MessageHelper
  {
    private MessageHelper()
    {
    }

    public static string InfoString(System.Type clazz, object id)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (clazz == null)
        stringBuilder.Append("<null Class>");
      else
        stringBuilder.Append(clazz.Name);
      stringBuilder.Append('#');
      if (id == null)
        stringBuilder.Append("<null>");
      else
        stringBuilder.Append(id);
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(
      IEntityPersister persister,
      object id,
      ISessionFactoryImplementor factory)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
        stringBuilder.Append("<null Class>");
      else
        stringBuilder.Append(persister.EntityName);
      stringBuilder.Append('#');
      if (id == null)
        stringBuilder.Append("<null>");
      else
        stringBuilder.Append(id);
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(
      IEntityPersister persister,
      object id,
      IType identifierType,
      ISessionFactoryImplementor factory)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
        stringBuilder.Append("<null Class>");
      else
        stringBuilder.Append(persister.EntityName);
      stringBuilder.Append('#');
      if (id == null)
        stringBuilder.Append("<null>");
      else
        stringBuilder.Append(identifierType.ToLoggableString(id, factory));
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(
      IEntityPersister persister,
      object[] ids,
      ISessionFactoryImplementor factory)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
      {
        stringBuilder.Append("<null IEntityPersister>");
      }
      else
      {
        stringBuilder.Append(persister.EntityName).Append("#<");
        for (int index = 0; index < ids.Length; ++index)
        {
          stringBuilder.Append(persister.IdentifierType.ToLoggableString(ids[index], factory));
          if (index < ids.Length - 1)
            stringBuilder.Append(", ");
          stringBuilder.Append('>');
        }
      }
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(IEntityPersister persister, object id)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
        stringBuilder.Append("<null ClassPersister>");
      else
        stringBuilder.Append(persister.EntityName);
      stringBuilder.Append('#');
      if (id == null)
        stringBuilder.Append("<null>");
      else
        stringBuilder.Append(id);
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(IEntityPersister persister)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
        stringBuilder.Append("<null ClassPersister>");
      else
        stringBuilder.Append(persister.EntityName);
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(ICollectionPersister persister, object id)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
      {
        stringBuilder.Append("<unreferenced>");
      }
      else
      {
        stringBuilder.Append(persister.Role);
        stringBuilder.Append('#');
        if (id == null)
          stringBuilder.Append("<null>");
        else
          stringBuilder.Append(id);
      }
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(string entityName, string propertyName, object key)
    {
      return new StringBuilder().Append('[').Append(entityName).Append('.').Append(propertyName).Append('#').Append(key ?? (object) "<null>").Append(']').ToString();
    }

    public static string InfoString(
      ICollectionPersister persister,
      object id,
      ISessionFactoryImplementor factory)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (persister == null)
      {
        stringBuilder.Append("<unreferenced>");
      }
      else
      {
        stringBuilder.Append(persister.Role);
        stringBuilder.Append('#');
        if (id == null)
          stringBuilder.Append("<null>");
        else
          stringBuilder.Append(persister.OwnerEntityPersister.IdentifierType.ToLoggableString(id, factory));
      }
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public static string InfoString(string entityName, object id)
    {
      return new StringBuilder().Append('[').Append(entityName ?? "<null entity name>").Append('#').Append(id ?? (object) "<null>").Append(']').ToString();
    }
  }
}
