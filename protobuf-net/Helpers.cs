// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Helpers
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace ProtoBuf
{
  internal sealed class Helpers
  {
    public static readonly Type[] EmptyTypes = Type.EmptyTypes;

    private Helpers()
    {
    }

    public static StringBuilder AppendLine(StringBuilder builder) => builder.AppendLine();

    public static bool IsNullOrEmpty(string value) => value == null || value.Length == 0;

    [Conditional("DEBUG")]
    public static void DebugWriteLine(string message, object obj)
    {
    }

    [Conditional("DEBUG")]
    public static void DebugWriteLine(string message)
    {
    }

    [Conditional("TRACE")]
    public static void TraceWriteLine(string message) => Trace.WriteLine(message);

    [Conditional("DEBUG")]
    public static void DebugAssert(bool condition, string message)
    {
    }

    [Conditional("DEBUG")]
    public static void DebugAssert(bool condition, string message, params object[] args)
    {
    }

    [Conditional("DEBUG")]
    public static void DebugAssert(bool condition)
    {
    }

    public static void Sort(int[] keys, object[] values)
    {
      bool flag;
      do
      {
        flag = false;
        for (int index = 1; index < keys.Length; ++index)
        {
          if (keys[index - 1] > keys[index])
          {
            int key = keys[index];
            keys[index] = keys[index - 1];
            keys[index - 1] = key;
            object obj = values[index];
            values[index] = values[index - 1];
            values[index - 1] = obj;
            flag = true;
          }
        }
      }
      while (flag);
    }

    public static void BlockCopy(byte[] from, int fromIndex, byte[] to, int toIndex, int count)
    {
      Buffer.BlockCopy((Array) from, fromIndex, (Array) to, toIndex, count);
    }

    public static bool IsInfinity(float value) => float.IsInfinity(value);

    internal static MethodInfo GetInstanceMethod(Type declaringType, string name)
    {
      return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    internal static MethodInfo GetStaticMethod(Type declaringType, string name)
    {
      return declaringType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    internal static MethodInfo GetStaticMethod(
      Type declaringType,
      string name,
      Type[] parameterTypes)
    {
      return declaringType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, parameterTypes, (ParameterModifier[]) null);
    }

    internal static MethodInfo GetInstanceMethod(Type declaringType, string name, Type[] types)
    {
      if (types == null)
        types = Helpers.EmptyTypes;
      return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, types, (ParameterModifier[]) null);
    }

    internal static bool IsSubclassOf(Type type, Type baseClass) => type.IsSubclassOf(baseClass);

    public static bool IsInfinity(double value) => double.IsInfinity(value);

    public static ProtoTypeCode GetTypeCode(Type type)
    {
      TypeCode typeCode = Type.GetTypeCode(type);
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Boolean:
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
        case TypeCode.DateTime:
        case TypeCode.String:
          return (ProtoTypeCode) typeCode;
        default:
          if (type == typeof (TimeSpan))
            return ProtoTypeCode.TimeSpan;
          if (type == typeof (Guid))
            return ProtoTypeCode.Guid;
          if (type == typeof (Uri))
            return ProtoTypeCode.Uri;
          if (type == typeof (byte[]))
            return ProtoTypeCode.ByteArray;
          return type == typeof (Type) ? ProtoTypeCode.Type : ProtoTypeCode.Unknown;
      }
    }

    internal static Type GetUnderlyingType(Type type) => Nullable.GetUnderlyingType(type);

    internal static bool IsValueType(Type type) => type.IsValueType;

    internal static bool IsSealed(Type type) => type.IsSealed;

    internal static bool IsClass(Type type) => type.IsClass;

    internal static bool IsEnum(Type type) => type.IsEnum;

    internal static MethodInfo GetGetMethod(
      PropertyInfo property,
      bool nonPublic,
      bool allowInternal)
    {
      if (property == (PropertyInfo) null)
        return (MethodInfo) null;
      MethodInfo getMethod = property.GetGetMethod(nonPublic);
      if (((!(getMethod == (MethodInfo) null) ? 0 : (!nonPublic ? 1 : 0)) & (allowInternal ? 1 : 0)) != 0)
      {
        getMethod = property.GetGetMethod(true);
        if (getMethod == (MethodInfo) null && !getMethod.IsAssembly && !getMethod.IsFamilyOrAssembly)
          getMethod = (MethodInfo) null;
      }
      return getMethod;
    }

    internal static MethodInfo GetSetMethod(
      PropertyInfo property,
      bool nonPublic,
      bool allowInternal)
    {
      if (property == (PropertyInfo) null)
        return (MethodInfo) null;
      MethodInfo setMethod = property.GetSetMethod(nonPublic);
      if (((!(setMethod == (MethodInfo) null) ? 0 : (!nonPublic ? 1 : 0)) & (allowInternal ? 1 : 0)) != 0)
      {
        setMethod = property.GetGetMethod(true);
        if (setMethod == (MethodInfo) null && !setMethod.IsAssembly && !setMethod.IsFamilyOrAssembly)
          setMethod = (MethodInfo) null;
      }
      return setMethod;
    }

    internal static ConstructorInfo GetConstructor(
      Type type,
      Type[] parameterTypes,
      bool nonPublic)
    {
      return type.GetConstructor(nonPublic ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public, (Binder) null, parameterTypes, (ParameterModifier[]) null);
    }

    internal static ConstructorInfo[] GetConstructors(Type type, bool nonPublic)
    {
      return type.GetConstructors(nonPublic ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public);
    }

    internal static PropertyInfo GetProperty(Type type, string name, bool nonPublic)
    {
      return type.GetProperty(name, nonPublic ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public);
    }

    internal static object ParseEnum(Type type, string value) => Enum.Parse(type, value, true);

    internal static MemberInfo[] GetInstanceFieldsAndProperties(Type type, bool publicOnly)
    {
      BindingFlags bindingAttr = publicOnly ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      PropertyInfo[] properties = type.GetProperties(bindingAttr);
      FieldInfo[] fields = type.GetFields(bindingAttr);
      MemberInfo[] fieldsAndProperties = new MemberInfo[fields.Length + properties.Length];
      properties.CopyTo((Array) fieldsAndProperties, 0);
      fields.CopyTo((Array) fieldsAndProperties, properties.Length);
      return fieldsAndProperties;
    }

    internal static Type GetMemberType(MemberInfo member)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          return ((FieldInfo) member).FieldType;
        case MemberTypes.Property:
          return ((PropertyInfo) member).PropertyType;
        default:
          return (Type) null;
      }
    }

    internal static bool IsAssignableFrom(Type target, Type type) => target.IsAssignableFrom(type);

    internal static Assembly GetAssembly(Type type) => type.Assembly;

    internal static byte[] GetBuffer(MemoryStream ms) => ms.GetBuffer();
  }
}
