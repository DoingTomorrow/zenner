// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ResolverContractKey
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  internal struct ResolverContractKey(Type resolverType, Type contractType) : 
    IEquatable<ResolverContractKey>
  {
    private readonly Type _resolverType = resolverType;
    private readonly Type _contractType = contractType;

    public override int GetHashCode()
    {
      return this._resolverType.GetHashCode() ^ this._contractType.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return obj is ResolverContractKey other && this.Equals(other);
    }

    public bool Equals(ResolverContractKey other)
    {
      return this._resolverType == other._resolverType && this._contractType == other._contractType;
    }
  }
}
