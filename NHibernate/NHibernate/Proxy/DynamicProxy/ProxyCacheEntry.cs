// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.ProxyCacheEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  [Serializable]
  public class ProxyCacheEntry
  {
    private readonly int hashCode;

    public ProxyCacheEntry(Type baseType, Type[] interfaces)
    {
      this.BaseType = baseType != null ? baseType : throw new ArgumentNullException(nameof (baseType));
      this.Interfaces = interfaces ?? new Type[0];
      if (this.Interfaces.Length == 0)
      {
        this.hashCode = baseType.GetHashCode();
      }
      else
      {
        HashSet<Type> typeSet = new HashSet<Type>((IEnumerable<Type>) this.Interfaces)
        {
          baseType
        };
        this.hashCode = 59;
        foreach (object obj in typeSet)
          this.hashCode ^= obj.GetHashCode();
      }
    }

    public Type BaseType { get; private set; }

    public Type[] Interfaces { get; private set; }

    public override bool Equals(object obj)
    {
      ProxyCacheEntry objB = obj as ProxyCacheEntry;
      return !object.ReferenceEquals((object) null, (object) objB) && this.hashCode == objB.GetHashCode();
    }

    public override int GetHashCode() => this.hashCode;
  }
}
