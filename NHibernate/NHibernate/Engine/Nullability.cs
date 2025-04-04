// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Nullability
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Text;

#nullable disable
namespace NHibernate.Engine
{
  public sealed class Nullability
  {
    private ISessionImplementor session;

    public Nullability(ISessionImplementor session) => this.session = session;

    public void CheckNullability(object[] values, IEntityPersister persister, bool isUpdate)
    {
      bool[] propertyNullability = persister.PropertyNullability;
      bool[] flagArray = isUpdate ? persister.PropertyUpdateability : persister.PropertyInsertability;
      IType[] propertyTypes = persister.PropertyTypes;
      for (int index = 0; index < values.Length; ++index)
      {
        if (flagArray[index])
        {
          object obj = values[index];
          if (!propertyNullability[index] && obj == null)
            throw new PropertyValueException("not-null property references a null or transient value", persister.EntityName, persister.PropertyNames[index]);
          if (obj != null)
          {
            string child = this.CheckSubElementsNullability(propertyTypes[index], obj);
            if (child != null)
              throw new PropertyValueException("not-null property references a null or transient value", persister.EntityName, Nullability.BuildPropertyPath(persister.PropertyNames[index], child));
          }
        }
      }
    }

    private string CheckSubElementsNullability(IType propertyType, object value)
    {
      if (propertyType.IsComponentType)
        return this.CheckComponentNullability(value, (IAbstractComponentType) propertyType);
      if (propertyType.IsCollectionType)
      {
        CollectionType collectionType = (CollectionType) propertyType;
        IType elementType = collectionType.GetElementType(this.session.Factory);
        if (elementType.IsComponentType)
        {
          IAbstractComponentType compType = (IAbstractComponentType) elementType;
          foreach (object obj in CascadingAction.GetLoadedElementsIterator(this.session, collectionType, value))
          {
            if (obj != null)
              return this.CheckComponentNullability(obj, compType);
          }
        }
      }
      return (string) null;
    }

    private string CheckComponentNullability(object value, IAbstractComponentType compType)
    {
      bool[] propertyNullability = compType.PropertyNullability;
      if (propertyNullability != null)
      {
        object[] propertyValues = compType.GetPropertyValues(value, this.session.EntityMode);
        IType[] subtypes = compType.Subtypes;
        for (int index = 0; index < propertyValues.Length; ++index)
        {
          object obj = propertyValues[index];
          if (!propertyNullability[index] && obj == null)
            return compType.PropertyNames[index];
          if (obj != null)
          {
            string child = this.CheckSubElementsNullability(subtypes[index], obj);
            if (child != null)
              return Nullability.BuildPropertyPath(compType.PropertyNames[index], child);
          }
        }
      }
      return (string) null;
    }

    private static string BuildPropertyPath(string parent, string child)
    {
      return new StringBuilder(parent.Length + child.Length + 1).Append(parent).Append('.').Append(child).ToString();
    }
  }
}
