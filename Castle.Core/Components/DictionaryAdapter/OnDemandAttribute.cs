// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.OnDemandAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false)]
  public class OnDemandAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryPropertyGetter,
    IDictionaryBehavior
  {
    public OnDemandAttribute()
    {
    }

    public OnDemandAttribute(Type type)
    {
      this.Type = type.GetConstructor(Type.EmptyTypes) != null ? type : throw new ArgumentException("On-demand values must have a parameterless constructor");
    }

    public OnDemandAttribute(object value) => this.Value = value;

    public Type Type { get; private set; }

    public object Value { get; private set; }

    public object GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      if (storedValue == null && !ifExists)
      {
        IValueInitializer initializer = (IValueInitializer) null;
        if (this.Value != null)
        {
          storedValue = this.Value;
        }
        else
        {
          Type type = this.Type ?? this.GetInferredType(dictionaryAdapter, property, out initializer);
          if (this.IsAcceptedType(type))
          {
            if (type.IsInterface)
            {
              if (!property.IsDynamicProperty && storedValue == null)
                storedValue = dictionaryAdapter.Create(property.PropertyType);
            }
            else if (type.IsArray)
              storedValue = (object) Array.CreateInstance(type.GetElementType(), 0);
            else if (storedValue == null)
            {
              object[] parameters = (object[]) null;
              ConstructorInfo constructorInfo = (ConstructorInfo) null;
              if (property.IsDynamicProperty)
              {
                constructorInfo = ((IEnumerable<ConstructorInfo>) type.GetConstructors()).Select(ctor => new
                {
                  ctor = ctor,
                  parms = ctor.GetParameters()
                }).Where(_param1 => _param1.parms.Length == 1 && _param1.parms[0].ParameterType.IsAssignableFrom(dictionaryAdapter.Meta.Type)).Select(_param0 => _param0.ctor).FirstOrDefault<ConstructorInfo>();
                if (constructorInfo != null)
                  parameters = (object[]) new IDictionaryAdapter[1]
                  {
                    dictionaryAdapter
                  };
              }
              if (constructorInfo == null)
                constructorInfo = type.GetConstructor(Type.EmptyTypes);
              if (constructorInfo != null)
                storedValue = constructorInfo.Invoke(parameters);
            }
          }
        }
        if (storedValue != null)
        {
          using (dictionaryAdapter.SuppressNotificationsBlock())
          {
            if (storedValue is ISupportInitialize)
            {
              ((ISupportInitialize) storedValue).BeginInit();
              ((ISupportInitialize) storedValue).EndInit();
            }
            initializer?.Initialize(dictionaryAdapter, storedValue);
            property.SetPropertyValue(dictionaryAdapter, property.PropertyName, ref storedValue, dictionaryAdapter.This.Descriptor);
          }
        }
      }
      return storedValue;
    }

    private bool IsAcceptedType(Type type)
    {
      return type != null && type != typeof (string) && !type.IsPrimitive && !type.IsEnum;
    }

    private Type GetInferredType(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property,
      out IValueInitializer initializer)
    {
      initializer = (IValueInitializer) null;
      Type propertyType = property.PropertyType;
      if (!typeof (IEnumerable).IsAssignableFrom(propertyType))
        return propertyType;
      Type type = (Type) null;
      if (propertyType.IsGenericType)
      {
        Type genericTypeDefinition = propertyType.GetGenericTypeDefinition();
        Type genericArg = propertyType.GetGenericArguments()[0];
        bool flag = genericTypeDefinition == typeof (BindingList<>);
        if (flag || genericTypeDefinition == typeof (List<>))
        {
          if (dictionaryAdapter.CanEdit)
            type = flag ? typeof (EditableBindingList<>) : typeof (EditableList<>);
          if (flag && genericArg.IsInterface)
          {
            Func<object> func = (Func<object>) (() => dictionaryAdapter.Create(genericArg));
            initializer = (IValueInitializer) Activator.CreateInstance(typeof (BindingListInitializer<>).MakeGenericType(genericArg), new object[4]
            {
              null,
              (object) func,
              null,
              null
            });
          }
        }
        else if (genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (ICollection<>))
          type = dictionaryAdapter.CanEdit ? typeof (EditableList<>) : typeof (List<>);
        if (type != null)
          return type.MakeGenericType(genericArg);
      }
      else if (propertyType == typeof (IList) || propertyType == typeof (ICollection))
        return !dictionaryAdapter.CanEdit ? typeof (List<object>) : typeof (EditableList);
      return propertyType;
    }
  }
}
