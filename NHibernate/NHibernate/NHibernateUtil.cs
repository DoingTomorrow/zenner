// Decompiled with JetBrains decompiler
// Type: NHibernate.NHibernateUtil
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.UserTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate
{
  public static class NHibernateUtil
  {
    private static readonly Dictionary<System.Type, IType> clrTypeToNHibernateType = new Dictionary<System.Type, IType>();
    public static readonly NullableType AnsiString = (NullableType) new AnsiStringType();
    public static readonly NullableType Binary = (NullableType) new BinaryType();
    public static readonly NullableType BinaryBlob = (NullableType) new BinaryBlobType();
    public static readonly NullableType Boolean = (NullableType) new BooleanType();
    public static readonly NullableType Byte = (NullableType) new ByteType();
    public static readonly NullableType Character = (NullableType) new CharType();
    public static readonly NullableType CultureInfo = (NullableType) new CultureInfoType();
    public static readonly NullableType DateTime = (NullableType) new DateTimeType();
    public static readonly NullableType DateTime2 = (NullableType) new DateTime2Type();
    public static readonly NullableType LocalDateTime = (NullableType) new LocalDateTimeType();
    public static readonly NullableType UtcDateTime = (NullableType) new UtcDateTimeType();
    public static readonly NullableType DateTimeOffset = (NullableType) new DateTimeOffsetType();
    public static readonly NullableType Date = (NullableType) new DateType();
    public static readonly NullableType Decimal = (NullableType) new DecimalType();
    public static readonly NullableType Double = (NullableType) new DoubleType();
    public static readonly NullableType Currency = (NullableType) new CurrencyType();
    public static readonly NullableType Guid = (NullableType) new GuidType();
    public static readonly NullableType Int16 = (NullableType) new Int16Type();
    public static readonly NullableType Int32 = (NullableType) new Int32Type();
    public static readonly NullableType Int64 = (NullableType) new Int64Type();
    public static readonly NullableType SByte = (NullableType) new SByteType();
    public static readonly NullableType UInt16 = (NullableType) new UInt16Type();
    public static readonly NullableType UInt32 = (NullableType) new UInt32Type();
    public static readonly NullableType UInt64 = (NullableType) new UInt64Type();
    public static readonly NullableType Single = (NullableType) new SingleType();
    public static readonly NullableType String = (NullableType) new StringType();
    public static readonly NullableType StringClob = (NullableType) new StringClobType();
    public static readonly NullableType Time = (NullableType) new TimeType();
    public static readonly NullableType Ticks = (NullableType) new TicksType();
    public static readonly NullableType TimeAsTimeSpan = (NullableType) new TimeAsTimeSpanType();
    public static readonly NullableType TimeSpan = (NullableType) new TimeSpanType();
    public static readonly NullableType Timestamp = (NullableType) new TimestampType();
    public static readonly NullableType DbTimestamp = (NullableType) new DbTimestampType();
    public static readonly NullableType TrueFalse = (NullableType) new TrueFalseType();
    public static readonly NullableType YesNo = (NullableType) new YesNoType();
    public static readonly NullableType Class = (NullableType) new TypeType();
    public static readonly IType ClassMetaType = (IType) new NHibernate.Type.ClassMetaType();
    public static readonly NullableType Serializable = (NullableType) new SerializableType();
    public static readonly IType Object = (IType) new AnyType();
    public static readonly NullableType AnsiChar = (NullableType) new AnsiCharType();
    public static readonly NullableType XmlDoc = (NullableType) new XmlDocType();
    public static readonly NullableType XDoc = (NullableType) new XDocType();
    public static readonly NullableType Uri = (NullableType) new UriType();

    static NHibernateUtil()
    {
      foreach (FieldInfo field in typeof (NHibernateUtil).GetFields())
      {
        if (typeof (IType).IsAssignableFrom(field.FieldType))
        {
          IType type = (IType) field.GetValue((object) null);
          NHibernateUtil.clrTypeToNHibernateType[type.ReturnedClass] = type;
        }
      }
      NHibernateUtil.clrTypeToNHibernateType[NHibernateUtil.Boolean.ReturnedClass] = (IType) NHibernateUtil.Boolean;
    }

    public static IType GuessType(object obj)
    {
      if (!(obj is System.Type type))
        type = obj.GetType();
      return NHibernateUtil.GuessType(type);
    }

    public static IType GuessType(System.Type type)
    {
      if (type.IsGenericType && typeof (Nullable<>).Equals(type.GetGenericTypeDefinition()))
        type = type.GetGenericArguments()[0];
      if (NHibernateUtil.clrTypeToNHibernateType.ContainsKey(type))
        return NHibernateUtil.clrTypeToNHibernateType[type];
      return type.IsEnum ? (IType) Activator.CreateInstance(typeof (EnumType<>).MakeGenericType(type)) : (typeof (IUserType).IsAssignableFrom(type) || typeof (ICompositeUserType).IsAssignableFrom(type) ? NHibernateUtil.Custom(type) : NHibernateUtil.Entity(type));
    }

    public static IType Enum(System.Type enumClass) => (IType) new PersistentEnumType(enumClass);

    public static IType GetSerializable(System.Type serializableClass)
    {
      return (IType) new SerializableType(serializableClass);
    }

    public static IType Any(IType metaType, IType identifierType)
    {
      return (IType) new AnyType(metaType, identifierType);
    }

    public static IType Entity(System.Type persistentClass)
    {
      return (IType) new ManyToOneType(persistentClass.FullName);
    }

    public static IType Entity(string entityName) => (IType) new ManyToOneType(entityName);

    public static IType Custom(System.Type userTypeClass)
    {
      return typeof (ICompositeUserType).IsAssignableFrom(userTypeClass) ? (IType) new CompositeCustomType(userTypeClass, (IDictionary<string, string>) null) : (IType) new CustomType(userTypeClass, (IDictionary<string, string>) null);
    }

    public static void Initialize(object proxy)
    {
      if (proxy == null)
        return;
      if (proxy.IsProxy())
      {
        ((INHibernateProxy) proxy).HibernateLazyInitializer.Initialize();
      }
      else
      {
        if (!(proxy is IPersistentCollection))
          return;
        ((IPersistentCollection) proxy).ForceInitialization();
      }
    }

    public static bool IsInitialized(object proxy)
    {
      if (proxy.IsProxy())
        return !((INHibernateProxy) proxy).HibernateLazyInitializer.IsUninitialized;
      return !(proxy is IPersistentCollection) || ((IPersistentCollection) proxy).WasInitialized;
    }

    public static System.Type GetClass(object proxy)
    {
      return proxy.IsProxy() ? ((INHibernateProxy) proxy).HibernateLazyInitializer.GetImplementation().GetType() : proxy.GetType();
    }

    public static void Close(IEnumerator enumerator)
    {
      if (!(enumerator is EnumerableImpl enumerableImpl))
        throw new ArgumentException("Not a NHibernate enumerator", nameof (enumerator));
      enumerableImpl.Dispose();
    }

    public static void Close(IEnumerable enumerable)
    {
      if (!(enumerable is EnumerableImpl enumerableImpl))
        throw new ArgumentException("Not a NHibernate enumerable", nameof (enumerable));
      enumerableImpl.Dispose();
    }

    public static bool IsPropertyInitialized(object proxy, string propertyName)
    {
      object entity;
      if (proxy.IsProxy())
      {
        ILazyInitializer hibernateLazyInitializer = ((INHibernateProxy) proxy).HibernateLazyInitializer;
        if (hibernateLazyInitializer.IsUninitialized)
          return false;
        entity = hibernateLazyInitializer.GetImplementation();
      }
      else
        entity = proxy;
      if (!FieldInterceptionHelper.IsInstrumented(entity))
        return true;
      IFieldInterceptor fieldInterceptor = FieldInterceptionHelper.ExtractFieldInterceptor(entity);
      return fieldInterceptor == null || fieldInterceptor.IsInitializedField(propertyName);
    }
  }
}
