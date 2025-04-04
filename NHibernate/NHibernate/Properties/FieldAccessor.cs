// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.FieldAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class FieldAccessor : IPropertyAccessor
  {
    private readonly IFieldNamingStrategy namingStrategy;

    public FieldAccessor()
    {
    }

    public FieldAccessor(IFieldNamingStrategy namingStrategy)
    {
      this.namingStrategy = namingStrategy;
    }

    public IFieldNamingStrategy NamingStrategy => this.namingStrategy;

    public IGetter GetGetter(Type theClass, string propertyName)
    {
      string fieldName = this.GetFieldName(propertyName);
      if (!object.Equals((object) fieldName, (object) propertyName) && !theClass.HasProperty(propertyName))
        throw new PropertyNotFoundException(propertyName, fieldName, theClass);
      return (IGetter) new FieldAccessor.FieldGetter(FieldAccessor.GetField(theClass, fieldName), theClass, fieldName);
    }

    public ISetter GetSetter(Type theClass, string propertyName)
    {
      string fieldName = this.GetFieldName(propertyName);
      return (ISetter) new FieldAccessor.FieldSetter(FieldAccessor.GetField(theClass, fieldName), theClass, fieldName);
    }

    public bool CanAccessThroughReflectionOptimizer => true;

    private static FieldInfo GetField(Type type, string fieldName, Type originalType)
    {
      if (type == null || type == typeof (object))
        throw new PropertyNotFoundException(originalType, fieldName);
      return type.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? FieldAccessor.GetField(type.BaseType, fieldName, originalType);
    }

    internal static FieldInfo GetField(Type type, string fieldName)
    {
      return FieldAccessor.GetField(type, fieldName, type);
    }

    private string GetFieldName(string propertyName)
    {
      return this.namingStrategy == null ? propertyName : this.namingStrategy.GetFieldName(propertyName);
    }

    [Serializable]
    public sealed class FieldGetter : IGetter, IOptimizableGetter
    {
      private readonly FieldInfo field;
      private readonly Type clazz;
      private readonly string name;

      public FieldGetter(FieldInfo field, Type clazz, string name)
      {
        this.field = field;
        this.clazz = clazz;
        this.name = name;
      }

      public object Get(object target)
      {
        try
        {
          return this.field.GetValue(target);
        }
        catch (Exception ex)
        {
          throw new PropertyAccessException(ex, "could not get a field value by reflection", false, this.clazz, this.name);
        }
      }

      public Type ReturnType => this.field.FieldType;

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;

      public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
      {
        return this.Get(owner);
      }

      public void Emit(ILGenerator il) => il.Emit(OpCodes.Ldfld, this.field);
    }

    [Serializable]
    public sealed class FieldSetter : ISetter, IOptimizableSetter
    {
      private readonly FieldInfo field;
      private readonly Type clazz;
      private readonly string name;

      public FieldSetter(FieldInfo field, Type clazz, string name)
      {
        this.field = field;
        this.clazz = clazz;
        this.name = name;
      }

      public void Set(object target, object value)
      {
        try
        {
          this.field.SetValue(target, value);
        }
        catch (ArgumentException ex)
        {
          if (!this.field.FieldType.IsAssignableFrom(value.GetType()))
          {
            string message = string.Format("The type {0} can not be assigned to a field of type {1}", (object) value.GetType().ToString(), (object) this.field.FieldType.ToString());
            throw new PropertyAccessException((Exception) ex, message, true, this.clazz, this.name);
          }
          throw new PropertyAccessException((Exception) ex, "ArgumentException while setting the field value by reflection", true, this.clazz, this.name);
        }
        catch (Exception ex)
        {
          throw new PropertyAccessException(ex, "could not set a field value by reflection", true, this.clazz, this.name);
        }
      }

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;

      public void Emit(ILGenerator il) => il.Emit(OpCodes.Stfld, this.field);
    }
  }
}
