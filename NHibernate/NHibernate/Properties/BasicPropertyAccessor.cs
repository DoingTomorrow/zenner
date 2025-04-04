// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.BasicPropertyAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class BasicPropertyAccessor : IPropertyAccessor
  {
    public IGetter GetGetter(Type type, string propertyName)
    {
      return (IGetter) (BasicPropertyAccessor.GetGetterOrNull(type, propertyName) ?? throw new PropertyNotFoundException(type, propertyName, "getter"));
    }

    public ISetter GetSetter(Type type, string propertyName)
    {
      return (ISetter) (BasicPropertyAccessor.GetSetterOrNull(type, propertyName) ?? throw new PropertyNotFoundException(type, propertyName, "setter"));
    }

    public bool CanAccessThroughReflectionOptimizer => true;

    internal static BasicPropertyAccessor.BasicGetter GetGetterOrNull(
      Type type,
      string propertyName)
    {
      if (type == typeof (object) || type == null)
        return (BasicPropertyAccessor.BasicGetter) null;
      PropertyInfo property = type.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (property != null && property.CanRead)
        return new BasicPropertyAccessor.BasicGetter(type, property, propertyName);
      BasicPropertyAccessor.BasicGetter getterOrNull = BasicPropertyAccessor.GetGetterOrNull(type.BaseType, propertyName);
      if (getterOrNull == null)
      {
        Type[] interfaces = type.GetInterfaces();
        for (int index = 0; getterOrNull == null && index < interfaces.Length; ++index)
          getterOrNull = BasicPropertyAccessor.GetGetterOrNull(interfaces[index], propertyName);
      }
      return getterOrNull;
    }

    internal static BasicPropertyAccessor.BasicSetter GetSetterOrNull(
      Type type,
      string propertyName)
    {
      if (type == typeof (object) || type == null)
        return (BasicPropertyAccessor.BasicSetter) null;
      BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      if (type.IsValueType)
        bindingAttr |= BindingFlags.IgnoreCase;
      PropertyInfo property = type.GetProperty(propertyName, bindingAttr);
      if (property != null && property.CanWrite)
        return new BasicPropertyAccessor.BasicSetter(type, property, propertyName);
      BasicPropertyAccessor.BasicSetter setterOrNull = BasicPropertyAccessor.GetSetterOrNull(type.BaseType, propertyName);
      if (setterOrNull == null)
      {
        Type[] interfaces = type.GetInterfaces();
        for (int index = 0; setterOrNull == null && index < interfaces.Length; ++index)
          setterOrNull = BasicPropertyAccessor.GetSetterOrNull(interfaces[index], propertyName);
      }
      return setterOrNull;
    }

    [Serializable]
    public sealed class BasicGetter : IGetter, IOptimizableGetter
    {
      private readonly Type clazz;
      private readonly PropertyInfo property;
      private readonly string propertyName;

      public BasicGetter(Type clazz, PropertyInfo property, string propertyName)
      {
        this.clazz = clazz;
        this.property = property;
        this.propertyName = propertyName;
      }

      public PropertyInfo Property => this.property;

      public object Get(object target)
      {
        try
        {
          return this.property.GetValue(target, new object[0]);
        }
        catch (Exception ex)
        {
          throw new PropertyAccessException(ex, "Exception occurred", false, this.clazz, this.propertyName);
        }
      }

      public Type ReturnType => this.property.PropertyType;

      public string PropertyName => this.property.Name;

      public MethodInfo Method => this.property.GetGetMethod(true);

      public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
      {
        return this.Get(owner);
      }

      public void Emit(ILGenerator il)
      {
        il.EmitCall(OpCodes.Callvirt, this.Method ?? throw new PropertyNotFoundException(this.clazz, this.property.Name, "getter"), (Type[]) null);
      }
    }

    [Serializable]
    public sealed class BasicSetter : ISetter, IOptimizableSetter
    {
      private readonly Type clazz;
      private readonly PropertyInfo property;
      private readonly string propertyName;

      public BasicSetter(Type clazz, PropertyInfo property, string propertyName)
      {
        this.clazz = clazz;
        this.property = property;
        this.propertyName = propertyName;
      }

      public PropertyInfo Property => this.property;

      public void Set(object target, object value)
      {
        try
        {
          this.property.SetValue(target, value, new object[0]);
        }
        catch (ArgumentException ex)
        {
          if (!this.property.PropertyType.IsAssignableFrom(value.GetType()))
          {
            string message = string.Format("The type {0} can not be assigned to a property of type {1}", (object) value.GetType(), (object) this.property.PropertyType);
            throw new PropertyAccessException((Exception) ex, message, true, this.clazz, this.propertyName);
          }
          throw new PropertyAccessException((Exception) ex, "ArgumentException while setting the property value by reflection", true, this.clazz, this.propertyName);
        }
        catch (Exception ex)
        {
          throw new PropertyAccessException(ex, "could not set a property value by reflection", true, this.clazz, this.propertyName);
        }
      }

      public string PropertyName => this.property.Name;

      public MethodInfo Method => this.property.GetSetMethod(true);

      public void Emit(ILGenerator il)
      {
        il.EmitCall(OpCodes.Callvirt, this.Method ?? throw new PropertyNotFoundException(this.clazz, this.property.Name, "setter"), (Type[]) null);
      }
    }
  }
}
