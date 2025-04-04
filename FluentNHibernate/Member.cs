// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Member
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  public abstract class Member : IEquatable<Member>
  {
    public abstract string Name { get; }

    public abstract Type PropertyType { get; }

    public abstract bool CanWrite { get; }

    public abstract MemberInfo MemberInfo { get; }

    public abstract Type DeclaringType { get; }

    public abstract bool HasIndexParameters { get; }

    public abstract bool IsMethod { get; }

    public abstract bool IsField { get; }

    public abstract bool IsProperty { get; }

    public abstract bool IsAutoProperty { get; }

    public abstract bool IsPrivate { get; }

    public abstract bool IsProtected { get; }

    public abstract bool IsPublic { get; }

    public abstract bool IsInternal { get; }

    public bool Equals(Member other)
    {
      return object.Equals((object) other.MemberInfo.MetadataToken, (object) this.MemberInfo.MetadataToken) && object.Equals((object) other.MemberInfo.Module, (object) this.MemberInfo.Module);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) (obj as Member) != null && this.Equals((Member) obj);
    }

    public override int GetHashCode() => this.MemberInfo.GetHashCode() ^ 3;

    public static bool operator ==(Member left, Member right)
    {
      return object.Equals((object) left, (object) right);
    }

    public static bool operator !=(Member left, Member right)
    {
      return !object.Equals((object) left, (object) right);
    }

    public abstract void SetValue(object target, object value);

    public abstract object GetValue(object target);

    public abstract bool TryGetBackingField(out Member backingField);
  }
}
