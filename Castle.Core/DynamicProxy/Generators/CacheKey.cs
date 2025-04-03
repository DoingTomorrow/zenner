// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.CacheKey
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  [Serializable]
  public class CacheKey
  {
    private readonly MemberInfo target;
    private readonly Type[] interfaces;
    private readonly ProxyGenerationOptions options;
    private readonly Type type;

    public CacheKey(
      MemberInfo target,
      Type type,
      Type[] interfaces,
      ProxyGenerationOptions options)
    {
      this.target = target;
      this.type = type;
      this.interfaces = interfaces ?? Type.EmptyTypes;
      this.options = options;
    }

    public CacheKey(Type target, Type[] interfaces, ProxyGenerationOptions options)
      : this((MemberInfo) target, (Type) null, interfaces, options)
    {
    }

    public override int GetHashCode()
    {
      int hashCode = this.target.GetHashCode();
      foreach (Type type in this.interfaces)
        hashCode += 29 + type.GetHashCode();
      if (this.options != null)
        hashCode = 29 * hashCode + this.options.GetHashCode();
      if (this.type != null)
        hashCode = 29 * hashCode + this.type.GetHashCode();
      return hashCode;
    }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      if (!(obj is CacheKey cacheKey) || !object.Equals((object) this.type, (object) cacheKey.type) || !object.Equals((object) this.target, (object) cacheKey.target) || this.interfaces.Length != cacheKey.interfaces.Length)
        return false;
      for (int index = 0; index < this.interfaces.Length; ++index)
      {
        if (!object.Equals((object) this.interfaces[index], (object) cacheKey.interfaces[index]))
          return false;
      }
      return object.Equals((object) this.options, (object) cacheKey.options);
    }
  }
}
