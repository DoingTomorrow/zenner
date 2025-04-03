// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.TypePair
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Impl
{
  public struct TypePair : IEquatable<TypePair>
  {
    private readonly Type _destinationType;
    private readonly int _hashcode;
    private readonly Type _sourceType;

    public TypePair(Type sourceType, Type destinationType)
      : this()
    {
      this._sourceType = sourceType;
      this._destinationType = destinationType;
      this._hashcode = this._sourceType.GetHashCode() * 397 ^ this._destinationType.GetHashCode();
    }

    public Type SourceType => this._sourceType;

    public Type DestinationType => this._destinationType;

    public bool Equals(TypePair other)
    {
      return object.Equals((object) other._sourceType, (object) this._sourceType) && object.Equals((object) other._destinationType, (object) this._destinationType);
    }

    public override bool Equals(object obj)
    {
      return !object.ReferenceEquals((object) null, obj) && (object) obj.GetType() == (object) typeof (TypePair) && this.Equals((TypePair) obj);
    }

    public override int GetHashCode() => this._hashcode;
  }
}
