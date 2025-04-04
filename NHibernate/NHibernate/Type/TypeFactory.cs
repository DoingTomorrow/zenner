// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TypeFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;
using NHibernate.Classic;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace NHibernate.Type
{
  public sealed class TypeFactory
  {
    private static readonly string[] EmptyAliases = new string[0];
    private static readonly char[] PrecisionScaleSplit = new char[3]
    {
      '(',
      ')',
      ','
    };
    private static readonly char[] LengthSplit = new char[2]
    {
      '(',
      ')'
    };
    private static readonly TypeFactory Instance;
    private static readonly System.Type[] GenericCollectionSimpleSignature = new System.Type[3]
    {
      typeof (string),
      typeof (string),
      typeof (bool)
    };
    private static readonly IDictionary<string, IType> typeByTypeOfName = (IDictionary<string, IType>) new ThreadSafeDictionary<string, IType>((IDictionary<string, IType>) new Dictionary<string, IType>());
    private static readonly IDictionary<string, TypeFactory.GetNullableTypeWithLength> getTypeDelegatesWithLength = (IDictionary<string, TypeFactory.GetNullableTypeWithLength>) new ThreadSafeDictionary<string, TypeFactory.GetNullableTypeWithLength>((IDictionary<string, TypeFactory.GetNullableTypeWithLength>) new Dictionary<string, TypeFactory.GetNullableTypeWithLength>());
    private static readonly IDictionary<string, TypeFactory.GetNullableTypeWithPrecision> getTypeDelegatesWithPrecision = (IDictionary<string, TypeFactory.GetNullableTypeWithPrecision>) new ThreadSafeDictionary<string, TypeFactory.GetNullableTypeWithPrecision>((IDictionary<string, TypeFactory.GetNullableTypeWithPrecision>) new Dictionary<string, TypeFactory.GetNullableTypeWithPrecision>());

    private static void RegisterType(
      System.Type systemType,
      IType nhibernateType,
      IEnumerable<string> aliases)
    {
      System.Collections.Generic.List<string> aliases1 = new System.Collections.Generic.List<string>(aliases);
      aliases1.AddRange(TypeFactory.GetClrTypeAliases(systemType));
      TypeFactory.RegisterType(nhibernateType, (IEnumerable<string>) aliases1);
    }

    private static void RegisterType(
      System.Type systemType,
      IType nhibernateType,
      IEnumerable<string> aliases,
      TypeFactory.GetNullableTypeWithLength ctorLength)
    {
      System.Collections.Generic.List<string> aliases1 = new System.Collections.Generic.List<string>(aliases);
      aliases1.AddRange(TypeFactory.GetClrTypeAliases(systemType));
      TypeFactory.RegisterType(nhibernateType, (IEnumerable<string>) aliases1, ctorLength);
    }

    private static void RegisterType(
      System.Type systemType,
      IType nhibernateType,
      IEnumerable<string> aliases,
      TypeFactory.GetNullableTypeWithPrecision ctorPrecision)
    {
      System.Collections.Generic.List<string> aliases1 = new System.Collections.Generic.List<string>(aliases);
      aliases1.AddRange(TypeFactory.GetClrTypeAliases(systemType));
      TypeFactory.RegisterType(nhibernateType, (IEnumerable<string>) aliases1, ctorPrecision);
    }

    private static IEnumerable<string> GetClrTypeAliases(System.Type systemType)
    {
      System.Collections.Generic.List<string> clrTypeAliases = new System.Collections.Generic.List<string>()
      {
        systemType.FullName,
        systemType.AssemblyQualifiedName
      };
      if (systemType.IsValueType)
      {
        System.Type type = typeof (Nullable<>).MakeGenericType(systemType);
        clrTypeAliases.Add(type.FullName);
        clrTypeAliases.Add(type.AssemblyQualifiedName);
      }
      return (IEnumerable<string>) clrTypeAliases;
    }

    private static void RegisterType(IType nhibernateType, IEnumerable<string> aliases)
    {
      foreach (string key in new System.Collections.Generic.List<string>(aliases)
      {
        nhibernateType.Name
      })
        TypeFactory.typeByTypeOfName[key] = nhibernateType;
    }

    private static void RegisterType(
      IType nhibernateType,
      IEnumerable<string> aliases,
      TypeFactory.GetNullableTypeWithLength ctorLength)
    {
      foreach (string key in new System.Collections.Generic.List<string>(aliases)
      {
        nhibernateType.Name
      })
      {
        TypeFactory.typeByTypeOfName[key] = nhibernateType;
        TypeFactory.getTypeDelegatesWithLength.Add(key, ctorLength);
      }
    }

    private static void RegisterType(
      IType nhibernateType,
      IEnumerable<string> aliases,
      TypeFactory.GetNullableTypeWithPrecision ctorPrecision)
    {
      foreach (string key in new System.Collections.Generic.List<string>(aliases)
      {
        nhibernateType.Name
      })
      {
        TypeFactory.typeByTypeOfName[key] = nhibernateType;
        TypeFactory.getTypeDelegatesWithPrecision.Add(key, ctorPrecision);
      }
    }

    static TypeFactory()
    {
      TypeFactory.Instance = new TypeFactory();
      TypeFactory.RegisterDefaultNetTypes();
      TypeFactory.RegisterBuiltInTypes();
    }

    private static void RegisterDefaultNetTypes()
    {
      TypeFactory.RegisterType(typeof (byte[]), (IType) NHibernateUtil.Binary, (IEnumerable<string>) new string[1]
      {
        "binary"
      }, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.Binary, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new BinaryType(SqlTypeFactory.GetBinary(len))))));
      TypeFactory.RegisterType(typeof (bool), (IType) NHibernateUtil.Boolean, (IEnumerable<string>) new string[2]
      {
        "boolean",
        "bool"
      });
      TypeFactory.RegisterType(typeof (byte), (IType) NHibernateUtil.Byte, (IEnumerable<string>) new string[1]
      {
        "byte"
      });
      TypeFactory.RegisterType(typeof (char), (IType) NHibernateUtil.Character, (IEnumerable<string>) new string[2]
      {
        "character",
        "char"
      });
      TypeFactory.RegisterType(typeof (CultureInfo), (IType) NHibernateUtil.CultureInfo, (IEnumerable<string>) new string[1]
      {
        "locale"
      });
      TypeFactory.RegisterType(typeof (DateTime), (IType) NHibernateUtil.DateTime, (IEnumerable<string>) new string[1]
      {
        "datetime"
      });
      TypeFactory.RegisterType(typeof (DateTimeOffset), (IType) NHibernateUtil.DateTimeOffset, (IEnumerable<string>) new string[1]
      {
        "datetimeoffset"
      });
      TypeFactory.RegisterType(typeof (Decimal), (IType) NHibernateUtil.Decimal, (IEnumerable<string>) new string[2]
      {
        "big_decimal",
        "decimal"
      }, (TypeFactory.GetNullableTypeWithPrecision) ((p, s) => TypeFactory.GetType(NHibernateUtil.Decimal, p, s, (TypeFactory.NullableTypeCreatorDelegate) (st => (NullableType) new DecimalType(st)))));
      TypeFactory.RegisterType(typeof (double), (IType) NHibernateUtil.Double, (IEnumerable<string>) new string[1]
      {
        "double"
      }, (TypeFactory.GetNullableTypeWithPrecision) ((p, s) => TypeFactory.GetType(NHibernateUtil.Double, p, s, (TypeFactory.NullableTypeCreatorDelegate) (st => (NullableType) new DoubleType(st)))));
      TypeFactory.RegisterType(typeof (Guid), (IType) NHibernateUtil.Guid, (IEnumerable<string>) new string[1]
      {
        "guid"
      });
      TypeFactory.RegisterType(typeof (short), (IType) NHibernateUtil.Int16, (IEnumerable<string>) new string[1]
      {
        "short"
      });
      TypeFactory.RegisterType(typeof (int), (IType) NHibernateUtil.Int32, (IEnumerable<string>) new string[2]
      {
        "integer",
        "int"
      });
      TypeFactory.RegisterType(typeof (long), (IType) NHibernateUtil.Int64, (IEnumerable<string>) new string[1]
      {
        "long"
      });
      TypeFactory.RegisterType(typeof (sbyte), (IType) NHibernateUtil.SByte, (IEnumerable<string>) TypeFactory.EmptyAliases);
      TypeFactory.RegisterType(typeof (float), (IType) NHibernateUtil.Single, (IEnumerable<string>) new string[2]
      {
        "float",
        "single"
      }, (TypeFactory.GetNullableTypeWithPrecision) ((p, s) => TypeFactory.GetType(NHibernateUtil.Single, p, s, (TypeFactory.NullableTypeCreatorDelegate) (st => (NullableType) new SingleType(st)))));
      TypeFactory.RegisterType(typeof (string), (IType) NHibernateUtil.String, (IEnumerable<string>) new string[1]
      {
        "string"
      }, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.String, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new StringType(SqlTypeFactory.GetString(len))))));
      TypeFactory.RegisterType(typeof (TimeSpan), (IType) NHibernateUtil.TimeSpan, (IEnumerable<string>) new string[1]
      {
        "timespan"
      });
      TypeFactory.RegisterType(typeof (System.Type), (IType) NHibernateUtil.Class, (IEnumerable<string>) new string[1]
      {
        "class"
      }, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.Class, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new TypeType(SqlTypeFactory.GetString(len))))));
      TypeFactory.RegisterType(typeof (ushort), (IType) NHibernateUtil.UInt16, (IEnumerable<string>) new string[1]
      {
        "ushort"
      });
      TypeFactory.RegisterType(typeof (uint), (IType) NHibernateUtil.UInt32, (IEnumerable<string>) new string[1]
      {
        "uint"
      });
      TypeFactory.RegisterType(typeof (ulong), (IType) NHibernateUtil.UInt64, (IEnumerable<string>) new string[1]
      {
        "ulong"
      });
      TypeFactory.RegisterType(typeof (XmlDocument), (IType) NHibernateUtil.XmlDoc, (IEnumerable<string>) new string[3]
      {
        "xmldoc",
        "xmldocument",
        "xml"
      });
      TypeFactory.RegisterType(typeof (Uri), (IType) NHibernateUtil.Uri, (IEnumerable<string>) new string[2]
      {
        "uri",
        "url"
      });
      TypeFactory.RegisterType(typeof (XDocument), (IType) NHibernateUtil.XDoc, (IEnumerable<string>) new string[2]
      {
        "xdoc",
        "xdocument"
      });
      TypeFactory.RegisterType(typeof (object), NHibernateUtil.Object, (IEnumerable<string>) new string[1]
      {
        "object"
      });
    }

    private static void RegisterBuiltInTypes()
    {
      TypeFactory.RegisterType((IType) NHibernateUtil.AnsiString, (IEnumerable<string>) TypeFactory.EmptyAliases, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.AnsiString, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new AnsiStringType(SqlTypeFactory.GetAnsiString(len))))));
      TypeFactory.RegisterType((IType) NHibernateUtil.AnsiChar, (IEnumerable<string>) TypeFactory.EmptyAliases);
      TypeFactory.RegisterType((IType) NHibernateUtil.BinaryBlob, (IEnumerable<string>) TypeFactory.EmptyAliases, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.BinaryBlob, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new BinaryBlobType((BinarySqlType) SqlTypeFactory.GetBinaryBlob(len))))));
      TypeFactory.RegisterType((IType) NHibernateUtil.StringClob, (IEnumerable<string>) TypeFactory.EmptyAliases, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.StringClob, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new StringClobType((StringSqlType) SqlTypeFactory.GetStringClob(len))))));
      TypeFactory.RegisterType((IType) NHibernateUtil.Date, (IEnumerable<string>) new string[1]
      {
        "date"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.Timestamp, (IEnumerable<string>) new string[1]
      {
        "timestamp"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.DbTimestamp, (IEnumerable<string>) new string[1]
      {
        "dbtimestamp"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.Time, (IEnumerable<string>) new string[1]
      {
        "time"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.TrueFalse, (IEnumerable<string>) new string[1]
      {
        "true_false"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.YesNo, (IEnumerable<string>) new string[1]
      {
        "yes_no"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.Ticks, (IEnumerable<string>) new string[1]
      {
        "ticks"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.TimeAsTimeSpan, (IEnumerable<string>) TypeFactory.EmptyAliases);
      TypeFactory.RegisterType((IType) NHibernateUtil.LocalDateTime, (IEnumerable<string>) new string[1]
      {
        "localdatetime"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.UtcDateTime, (IEnumerable<string>) new string[1]
      {
        "utcdatetime"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.Currency, (IEnumerable<string>) new string[1]
      {
        "currency"
      }, (TypeFactory.GetNullableTypeWithPrecision) ((p, s) => TypeFactory.GetType(NHibernateUtil.Currency, p, s, (TypeFactory.NullableTypeCreatorDelegate) (st => (NullableType) new CurrencyType(st)))));
      TypeFactory.RegisterType((IType) NHibernateUtil.DateTime2, (IEnumerable<string>) new string[1]
      {
        "datetime2"
      });
      TypeFactory.RegisterType((IType) NHibernateUtil.Serializable, (IEnumerable<string>) new string[2]
      {
        "Serializable",
        "serializable"
      }, (TypeFactory.GetNullableTypeWithLength) (l => TypeFactory.GetType(NHibernateUtil.Serializable, l, (TypeFactory.GetNullableTypeWithLength) (len => (NullableType) new SerializableType(typeof (object), SqlTypeFactory.GetBinary(len))))));
    }

    public ICollectionTypeFactory CollectionTypeFactory
    {
      get => NHibernate.Cfg.Environment.BytecodeProvider.CollectionTypeFactory;
    }

    private TypeFactory()
    {
    }

    private static TypeFactory.TypeClassification GetTypeClassification(string typeName)
    {
      int startIndex = typeName.IndexOf("(");
      int num = 0;
      if (startIndex >= 0)
        num = typeName.IndexOf(",", startIndex);
      if (startIndex < 0)
        return TypeFactory.TypeClassification.Plain;
      return num >= 0 ? TypeFactory.TypeClassification.PrecisionScale : TypeFactory.TypeClassification.Length;
    }

    public static IType Basic(string name)
    {
      IType type;
      if (TypeFactory.typeByTypeOfName.TryGetValue(name, out type))
        return type;
      switch (TypeFactory.GetTypeClassification(name))
      {
        case TypeFactory.TypeClassification.Length:
          string[] strArray1 = name.Split(TypeFactory.LengthSplit);
          return strArray1.Length >= 3 ? TypeFactory.BuiltInType(strArray1[0].Trim(), int.Parse(strArray1[1].Trim())) : throw new ArgumentOutOfRangeException("TypeClassification.Length", (object) name, "It is not a valid Length name");
        case TypeFactory.TypeClassification.PrecisionScale:
          string[] strArray2 = name.Split(TypeFactory.PrecisionScaleSplit);
          return strArray2.Length >= 4 ? TypeFactory.BuiltInType(strArray2[0].Trim(), byte.Parse(strArray2[1].Trim()), byte.Parse(strArray2[2].Trim())) : throw new ArgumentOutOfRangeException("TypeClassification.PrecisionScale", (object) name, "It is not a valid Precision/Scale name");
        default:
          return (IType) null;
      }
    }

    internal static IType BuiltInType(string typeName, int length)
    {
      TypeFactory.GetNullableTypeWithLength nullableTypeWithLength;
      return TypeFactory.getTypeDelegatesWithLength.TryGetValue(typeName, out nullableTypeWithLength) ? (IType) nullableTypeWithLength(length) : (IType) null;
    }

    internal static IType BuiltInType(string typeName, byte precision, byte scale)
    {
      TypeFactory.GetNullableTypeWithPrecision typeWithPrecision;
      return TypeFactory.getTypeDelegatesWithPrecision.TryGetValue(typeName, out typeWithPrecision) ? (IType) typeWithPrecision(precision, scale) : (IType) null;
    }

    private static void AddToTypeOfName(string key, IType type)
    {
      TypeFactory.typeByTypeOfName.Add(key, type);
      TypeFactory.typeByTypeOfName.Add(type.Name, type);
    }

    private static void AddToTypeOfNameWithLength(string key, IType type)
    {
      TypeFactory.typeByTypeOfName.Add(key, type);
    }

    private static void AddToTypeOfNameWithPrecision(string key, IType type)
    {
      TypeFactory.typeByTypeOfName.Add(key, type);
    }

    private static string GetKeyForLengthBased(string name, int length)
    {
      return name + "(" + (object) length + ")";
    }

    private static string GetKeyForPrecisionScaleBased(string name, byte precision, byte scale)
    {
      return name + "(" + (object) precision + ", " + (object) scale + ")";
    }

    public static IType HeuristicType(string typeName)
    {
      return TypeFactory.HeuristicType(typeName, (IDictionary<string, string>) null);
    }

    public static IType HeuristicType(string typeName, IDictionary<string, string> parameters)
    {
      return TypeFactory.HeuristicType(typeName, parameters, new int?());
    }

    public static IType HeuristicType(
      string typeName,
      IDictionary<string, string> parameters,
      int? length)
    {
      IType type1 = TypeFactory.Basic(typeName);
      if (type1 == null)
      {
        TypeFactory.TypeClassification typeClassification = TypeFactory.GetTypeClassification(typeName);
        string[] strArray1;
        string[] strArray2;
        switch (typeClassification)
        {
          case TypeFactory.TypeClassification.Length:
            strArray1 = typeName.Split(TypeFactory.LengthSplit);
            goto label_6;
          case TypeFactory.TypeClassification.PrecisionScale:
            strArray2 = typeName.Split(TypeFactory.PrecisionScaleSplit);
            break;
          default:
            strArray2 = new string[1]{ typeName };
            break;
        }
        strArray1 = strArray2;
label_6:
        System.Type type2;
        try
        {
          type2 = ReflectHelper.ClassForName(strArray1[0]);
        }
        catch (Exception ex)
        {
          type2 = (System.Type) null;
        }
        if (type2 != null)
        {
          if (typeof (IType).IsAssignableFrom(type2))
          {
            try
            {
              type1 = (IType) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(type2);
            }
            catch (Exception ex)
            {
              throw new MappingException("Could not instantiate IType " + type2.Name + ": " + (object) ex, ex);
            }
            TypeFactory.InjectParameters((object) type1, parameters);
          }
          else if (typeof (ICompositeUserType).IsAssignableFrom(type2))
            type1 = (IType) new CompositeCustomType(type2, parameters);
          else if (typeof (IUserType).IsAssignableFrom(type2))
            type1 = (IType) new CustomType(type2, parameters);
          else if (typeof (ILifecycle).IsAssignableFrom(type2))
            type1 = NHibernateUtil.Entity(type2);
          else if (type2.IsEnum)
          {
            try
            {
              type1 = (IType) Activator.CreateInstance(typeof (EnumType<>).MakeGenericType(type2));
            }
            catch (Exception ex)
            {
              throw new MappingException("Can't instantiate enum " + type2.FullName + "; The enum can't be empty", ex);
            }
          }
          else if (TypeFactory.IsNullableEnum(type2))
          {
            try
            {
              type1 = (IType) Activator.CreateInstance(typeof (EnumType<>).MakeGenericType(type2.GetGenericArguments()[0]));
            }
            catch (Exception ex)
            {
              throw new MappingException("Can't instantiate enum " + type2.FullName + "; The enum can't be empty", ex);
            }
          }
          else if (type2.IsSerializable)
            type1 = typeClassification != TypeFactory.TypeClassification.Length ? (!length.HasValue ? (IType) TypeFactory.GetSerializableType(type2) : (IType) TypeFactory.GetSerializableType(type2, length.Value)) : (IType) TypeFactory.GetSerializableType(type2, int.Parse(strArray1[1]));
        }
      }
      return type1;
    }

    private static bool IsNullableEnum(System.Type typeClass)
    {
      return typeClass.IsGenericType && typeof (Nullable<>).Equals(typeClass.GetGenericTypeDefinition()) && typeClass.GetGenericArguments()[0].IsSubclassOf(typeof (Enum));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetAnsiStringType(int length)
    {
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(NHibernateUtil.AnsiString.Name, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) new AnsiStringType(SqlTypeFactory.GetAnsiString(length));
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetBinaryType(int length)
    {
      if (length == 0)
        return NHibernateUtil.Binary;
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(NHibernateUtil.Binary.Name, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) new BinaryType(SqlTypeFactory.GetBinary(length));
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private static NullableType GetType(
      NullableType defaultUnqualifiedType,
      int length,
      TypeFactory.GetNullableTypeWithLength ctorDelegate)
    {
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(defaultUnqualifiedType.Name, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) ctorDelegate(length);
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private static NullableType GetType(
      NullableType defaultUnqualifiedType,
      byte precision,
      byte scale,
      TypeFactory.NullableTypeCreatorDelegate ctor)
    {
      string precisionScaleBased = TypeFactory.GetKeyForPrecisionScaleBased(defaultUnqualifiedType.Name, precision, scale);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(precisionScaleBased, out type))
      {
        type = (IType) ctor(SqlTypeFactory.GetSqlType(defaultUnqualifiedType.SqlType.DbType, precision, scale));
        TypeFactory.AddToTypeOfNameWithPrecision(precisionScaleBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetSerializableType(System.Type serializableType)
    {
      string assemblyQualifiedName = serializableType.AssemblyQualifiedName;
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(assemblyQualifiedName, out type))
      {
        type = (IType) new SerializableType(serializableType);
        TypeFactory.AddToTypeOfName(assemblyQualifiedName, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetSerializableType(System.Type serializableType, int length)
    {
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(serializableType.AssemblyQualifiedName, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) new SerializableType(serializableType, SqlTypeFactory.GetBinary(length));
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetSerializableType(int length)
    {
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(NHibernateUtil.Serializable.Name, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) new SerializableType(typeof (object), SqlTypeFactory.GetBinary(length));
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetStringType(int length)
    {
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(NHibernateUtil.String.Name, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) new StringType(SqlTypeFactory.GetString(length));
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static NullableType GetTypeType(int length)
    {
      string keyForLengthBased = TypeFactory.GetKeyForLengthBased(typeof (TypeType).FullName, length);
      IType type;
      if (!TypeFactory.typeByTypeOfName.TryGetValue(keyForLengthBased, out type))
      {
        type = (IType) new TypeType(SqlTypeFactory.GetString(length));
        TypeFactory.AddToTypeOfNameWithLength(keyForLengthBased, type);
      }
      return (NullableType) type;
    }

    public static EntityType OneToOne(
      string persistentClass,
      ForeignKeyDirection foreignKeyType,
      string uniqueKeyPropertyName,
      bool lazy,
      bool unwrapProxy,
      bool isEmbeddedInXML,
      string entityName,
      string propertyName)
    {
      return (EntityType) new OneToOneType(persistentClass, foreignKeyType, uniqueKeyPropertyName, lazy, unwrapProxy, isEmbeddedInXML, entityName, propertyName);
    }

    public static EntityType ManyToOne(string persistentClass)
    {
      return (EntityType) new ManyToOneType(persistentClass);
    }

    public static EntityType ManyToOne(string persistentClass, bool lazy)
    {
      return (EntityType) new ManyToOneType(persistentClass, lazy);
    }

    public static EntityType ManyToOne(
      string persistentClass,
      string uniqueKeyPropertyName,
      bool lazy,
      bool unwrapProxy,
      bool isEmbeddedInXML,
      bool ignoreNotFound)
    {
      return (EntityType) new ManyToOneType(persistentClass, uniqueKeyPropertyName, lazy, unwrapProxy, isEmbeddedInXML, ignoreNotFound);
    }

    public static CollectionType Array(
      string role,
      string propertyRef,
      bool embedded,
      System.Type elementClass)
    {
      return TypeFactory.Instance.CollectionTypeFactory.Array(role, propertyRef, embedded, elementClass);
    }

    public static CollectionType List(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.List(role, propertyRef, embedded);
    }

    public static CollectionType Bag(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.Bag(role, propertyRef, embedded);
    }

    public static CollectionType IdBag(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.IdBag(role, propertyRef, embedded);
    }

    public static CollectionType Map(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.Map(role, propertyRef, embedded);
    }

    public static CollectionType Set(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.Set(role, propertyRef, embedded);
    }

    public static CollectionType SortedMap(
      string role,
      string propertyRef,
      bool embedded,
      IComparer comparer)
    {
      return TypeFactory.Instance.CollectionTypeFactory.SortedMap(role, propertyRef, embedded, comparer);
    }

    public static CollectionType OrderedMap(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.OrderedMap(role, propertyRef, embedded);
    }

    public static CollectionType SortedSet(
      string role,
      string propertyRef,
      bool embedded,
      IComparer comparer)
    {
      return TypeFactory.Instance.CollectionTypeFactory.SortedSet(role, propertyRef, embedded, comparer);
    }

    public static CollectionType OrderedSet(string role, string propertyRef, bool embedded)
    {
      return TypeFactory.Instance.CollectionTypeFactory.OrderedSet(role, propertyRef, embedded);
    }

    public static CollectionType GenericBag(string role, string propertyRef, System.Type elementClass)
    {
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("Bag", new System.Type[1]
      {
        elementClass
      }, TypeFactory.GenericCollectionSimpleSignature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[3]
      {
        (object) role,
        (object) propertyRef,
        (object) false
      });
    }

    public static CollectionType GenericIdBag(string role, string propertyRef, System.Type elementClass)
    {
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("IdBag", new System.Type[1]
      {
        elementClass
      }, TypeFactory.GenericCollectionSimpleSignature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[3]
      {
        (object) role,
        (object) propertyRef,
        (object) false
      });
    }

    public static CollectionType GenericList(string role, string propertyRef, System.Type elementClass)
    {
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("List", new System.Type[1]
      {
        elementClass
      }, TypeFactory.GenericCollectionSimpleSignature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[3]
      {
        (object) role,
        (object) propertyRef,
        (object) false
      });
    }

    public static CollectionType GenericMap(
      string role,
      string propertyRef,
      System.Type indexClass,
      System.Type elementClass)
    {
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("Map", new System.Type[2]
      {
        indexClass,
        elementClass
      }, TypeFactory.GenericCollectionSimpleSignature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[3]
      {
        (object) role,
        (object) propertyRef,
        (object) false
      });
    }

    public static CollectionType GenericSortedList(
      string role,
      string propertyRef,
      object comparer,
      System.Type indexClass,
      System.Type elementClass)
    {
      System.Type[] signature = new System.Type[4]
      {
        typeof (string),
        typeof (string),
        typeof (bool),
        typeof (IComparer<>).MakeGenericType(indexClass)
      };
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("SortedList", new System.Type[2]
      {
        indexClass,
        elementClass
      }, signature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[4]
      {
        (object) role,
        (object) propertyRef,
        (object) false,
        comparer
      });
    }

    public static CollectionType GenericSortedDictionary(
      string role,
      string propertyRef,
      object comparer,
      System.Type indexClass,
      System.Type elementClass)
    {
      System.Type[] signature = new System.Type[4]
      {
        typeof (string),
        typeof (string),
        typeof (bool),
        typeof (IComparer<>).MakeGenericType(indexClass)
      };
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("SortedDictionary", new System.Type[2]
      {
        indexClass,
        elementClass
      }, signature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[4]
      {
        (object) role,
        (object) propertyRef,
        (object) false,
        comparer
      });
    }

    public static CollectionType GenericSet(string role, string propertyRef, System.Type elementClass)
    {
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("Set", new System.Type[1]
      {
        elementClass
      }, TypeFactory.GenericCollectionSimpleSignature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[3]
      {
        (object) role,
        (object) propertyRef,
        (object) false
      });
    }

    public static CollectionType GenericSortedSet(
      string role,
      string propertyRef,
      object comparer,
      System.Type elementClass)
    {
      System.Type[] signature = new System.Type[4]
      {
        typeof (string),
        typeof (string),
        typeof (bool),
        typeof (IComparer<>).MakeGenericType(elementClass)
      };
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("SortedSet", new System.Type[1]
      {
        elementClass
      }, signature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[4]
      {
        (object) role,
        (object) propertyRef,
        (object) false,
        comparer
      });
    }

    public static CollectionType GenericOrderedSet(
      string role,
      string propertyRef,
      System.Type elementClass)
    {
      return (CollectionType) ReflectHelper.GetGenericMethodFrom<ICollectionTypeFactory>("OrderedSet", new System.Type[1]
      {
        elementClass
      }, TypeFactory.GenericCollectionSimpleSignature).Invoke((object) TypeFactory.Instance.CollectionTypeFactory, new object[3]
      {
        (object) role,
        (object) propertyRef,
        (object) false
      });
    }

    public static CollectionType CustomCollection(
      string typeName,
      IDictionary<string, string> typeParameters,
      string role,
      string propertyRef,
      bool embedded)
    {
      System.Type userTypeClass;
      try
      {
        userTypeClass = ReflectHelper.ClassForName(typeName);
      }
      catch (Exception ex)
      {
        throw new MappingException("user collection type class not found: " + typeName, ex);
      }
      CustomCollectionType customCollectionType = new CustomCollectionType(userTypeClass, role, propertyRef, embedded);
      if (typeParameters != null)
        TypeFactory.InjectParameters((object) customCollectionType.UserType, typeParameters);
      return (CollectionType) customCollectionType;
    }

    public static void InjectParameters(object type, IDictionary<string, string> parameters)
    {
      if (type is IParameterizedType)
        ((IParameterizedType) type).SetParameterValues(parameters);
      else if (parameters != null && parameters.Count != 0)
        throw new MappingException("type is not parameterized: " + type.GetType().Name);
    }

    private enum TypeClassification
    {
      Plain,
      Length,
      PrecisionScale,
    }

    private delegate NullableType GetNullableTypeWithLength(int length);

    private delegate NullableType GetNullableTypeWithPrecision(byte precision, byte scale);

    private delegate NullableType NullableTypeCreatorDelegate(SqlType sqlType);
  }
}
