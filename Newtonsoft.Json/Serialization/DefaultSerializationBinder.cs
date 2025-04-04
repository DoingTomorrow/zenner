// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultSerializationBinder
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class DefaultSerializationBinder : SerializationBinder
  {
    internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();
    private readonly ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type> _typeCache = new ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type>(new Func<DefaultSerializationBinder.TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));

    private static Type GetTypeFromTypeNameKey(DefaultSerializationBinder.TypeNameKey typeNameKey)
    {
      string assemblyName = typeNameKey.AssemblyName;
      string typeName = typeNameKey.TypeName;
      if (assemblyName == null)
        return Type.GetType(typeName);
      Assembly assembly1 = Assembly.LoadWithPartialName(assemblyName);
      if (assembly1 == (Assembly) null)
      {
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == assemblyName)
          {
            assembly1 = assembly2;
            break;
          }
        }
      }
      Type type = !(assembly1 == (Assembly) null) ? assembly1.GetType(typeName) : throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) assemblyName));
      return !(type == (Type) null) ? type : throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeName, (object) assembly1.FullName));
    }

    public override Type BindToType(string assemblyName, string typeName)
    {
      return this._typeCache.Get(new DefaultSerializationBinder.TypeNameKey(assemblyName, typeName));
    }

    public override void BindToName(
      Type serializedType,
      out string assemblyName,
      out string typeName)
    {
      assemblyName = serializedType.Assembly.FullName;
      typeName = serializedType.FullName;
    }

    internal struct TypeNameKey(string assemblyName, string typeName) : 
      IEquatable<DefaultSerializationBinder.TypeNameKey>
    {
      internal readonly string AssemblyName = assemblyName;
      internal readonly string TypeName = typeName;

      public override int GetHashCode()
      {
        return (this.AssemblyName != null ? this.AssemblyName.GetHashCode() : 0) ^ (this.TypeName != null ? this.TypeName.GetHashCode() : 0);
      }

      public override bool Equals(object obj)
      {
        return obj is DefaultSerializationBinder.TypeNameKey other && this.Equals(other);
      }

      public bool Equals(DefaultSerializationBinder.TypeNameKey other)
      {
        return this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
      }
    }
  }
}
