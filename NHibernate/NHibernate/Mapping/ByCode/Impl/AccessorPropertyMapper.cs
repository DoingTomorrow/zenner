// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.AccessorPropertyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class AccessorPropertyMapper : IAccessorPropertyMapper
  {
    private const BindingFlags FieldBindingFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
    private readonly bool canChangeAccessor = true;
    private readonly Type declaringType;
    private readonly IDictionary<string, IFieldNamingStrategy> fieldNamningStrategies = PropertyToField.DefaultStrategies;
    private readonly string propertyName;
    private readonly Action<string> setAccessor;

    public AccessorPropertyMapper(
      Type declaringType,
      string propertyName,
      Action<string> accesorValueSetter)
    {
      this.PropertyName = propertyName;
      if (declaringType == null)
        throw new ArgumentNullException(nameof (declaringType));
      MemberInfo memberInfo = (MemberInfo) null;
      if (propertyName != null)
        memberInfo = ((IEnumerable<MemberInfo>) declaringType.GetMember(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).FirstOrDefault<MemberInfo>();
      if (memberInfo == null)
      {
        accesorValueSetter("none");
        this.canChangeAccessor = false;
      }
      else if (memberInfo is FieldInfo)
      {
        accesorValueSetter("field");
        this.canChangeAccessor = false;
      }
      this.declaringType = declaringType;
      this.propertyName = propertyName;
      this.setAccessor = accesorValueSetter;
    }

    public string PropertyName { get; set; }

    public void Access(Accessor accessor)
    {
      if (!this.canChangeAccessor)
        return;
      switch (accessor)
      {
        case Accessor.Property:
          this.setAccessor("property");
          break;
        case Accessor.Field:
          string namingFieldStrategy1 = this.GetNamingFieldStrategy();
          if (namingFieldStrategy1 != null)
          {
            this.setAccessor("field." + namingFieldStrategy1);
            break;
          }
          this.setAccessor("field");
          break;
        case Accessor.NoSetter:
          string namingFieldStrategy2 = this.GetNamingFieldStrategy();
          if (namingFieldStrategy2 == null)
            throw new ArgumentOutOfRangeException(nameof (accessor), "The property name " + this.propertyName + " does not match with any supported field naming strategies.");
          this.setAccessor("nosetter." + namingFieldStrategy2);
          break;
        case Accessor.ReadOnly:
          this.setAccessor("readonly");
          break;
        case Accessor.None:
          this.setAccessor("none");
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (accessor));
      }
    }

    public void Access(Type accessorType)
    {
      if (!this.canChangeAccessor)
        return;
      if (accessorType == null)
        throw new ArgumentNullException(nameof (accessorType));
      if (!typeof (IPropertyAccessor).IsAssignableFrom(accessorType))
        throw new ArgumentOutOfRangeException(nameof (accessorType), "The accessor should implements IPropertyAccessor.");
      this.setAccessor(accessorType.AssemblyQualifiedName);
    }

    private string GetNamingFieldStrategy()
    {
      return this.fieldNamningStrategies.FirstOrDefault<KeyValuePair<string, IFieldNamingStrategy>>((Func<KeyValuePair<string, IFieldNamingStrategy>, bool>) (p => AccessorPropertyMapper.GetField(this.declaringType, p.Value.GetFieldName(this.propertyName)) != null)).Key;
    }

    private static MemberInfo GetField(Type type, string fieldName)
    {
      return type == typeof (object) || type == null ? (MemberInfo) null : (MemberInfo) type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) ?? AccessorPropertyMapper.GetField(type.BaseType, fieldName);
    }
  }
}
