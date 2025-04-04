// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.TypeReference
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class TypeReference
  {
    public static readonly TypeReference Empty = new TypeReference("nop");
    private readonly Type innerType;
    private readonly string innerName;

    public TypeReference(string name)
    {
      this.innerType = Type.GetType(name, false, true);
      this.innerName = name;
    }

    public TypeReference(Type type)
    {
      this.innerType = type;
      this.innerName = type.Name;
    }

    public string Name => this.innerName;

    public bool IsEnum
    {
      get
      {
        if (this.innerType == null)
          return false;
        return this.innerType.IsGenericType && this.innerType.GetGenericTypeDefinition() == typeof (GenericEnumMapper<>) || this.innerType.IsEnum;
      }
    }

    public bool IsGenericType => this.innerType != null && this.innerType.IsGenericType;

    public bool IsGenericTypeDefinition
    {
      get => this.innerType != null && this.innerType.IsGenericTypeDefinition;
    }

    public Type GetGenericTypeDefinition()
    {
      return this.innerType == null ? (Type) null : this.innerType.GetGenericTypeDefinition();
    }

    public Type GenericTypeDefinition => this.GetGenericTypeDefinition();

    public bool IsNullable => this.GenericTypeDefinition == typeof (Nullable<>);

    public Type[] GetGenericArguments()
    {
      return this.innerType == null ? new Type[0] : this.innerType.GetGenericArguments();
    }

    public IEnumerable<Type> GenericArguments => (IEnumerable<Type>) this.GetGenericArguments();

    public override string ToString()
    {
      return this.innerType != null ? this.innerType.AssemblyQualifiedName : this.innerName;
    }

    public bool Equals(TypeReference other)
    {
      if (object.ReferenceEquals((object) other, (object) null))
        return false;
      if (other.innerType == null && this.innerType == null)
        return other.innerName.Equals(this.innerName);
      return other.innerType != null && other.innerType.Equals(this.innerType);
    }

    public bool Equals(Type other)
    {
      return !object.ReferenceEquals((object) other, (object) null) && other.Equals(this.innerType);
    }

    public bool Equals(string other)
    {
      return !object.ReferenceEquals((object) other, (object) null) && other.Equals(this.innerName);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (obj.GetType() == typeof (TypeReference))
        return this.Equals((TypeReference) obj);
      if (obj is Type)
        return this.Equals((Type) obj);
      return obj.GetType() == typeof (string) && this.Equals((string) obj);
    }

    public override int GetHashCode()
    {
      return (this.innerType != null ? this.innerType.GetHashCode() : 0) * 397 ^ (this.innerName != null ? this.innerName.GetHashCode() : 0);
    }

    public Type GetUnderlyingSystemType() => this.innerType;

    public static bool operator ==(TypeReference original, Type type)
    {
      return type != null && !(original == (Type) null) && original.innerType != null && original.innerType == type;
    }

    public static bool operator !=(TypeReference original, Type type) => !(original == type);

    public static bool operator ==(Type original, TypeReference type) => type == original;

    public static bool operator !=(Type original, TypeReference type) => !(original == type);

    public static bool operator ==(TypeReference original, TypeReference other)
    {
      return original.Equals(other);
    }

    public static bool operator !=(TypeReference original, TypeReference other)
    {
      return !(original == other);
    }
  }
}
