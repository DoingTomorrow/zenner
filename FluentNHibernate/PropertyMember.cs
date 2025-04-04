// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.PropertyMember
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  internal class PropertyMember : Member
  {
    private readonly PropertyInfo member;
    private readonly MethodMember getMethod;
    private readonly MethodMember setMethod;
    private Member backingField;

    public PropertyMember(PropertyInfo member)
    {
      this.member = member;
      this.getMethod = this.GetMember(member.GetGetMethod(true));
      this.setMethod = this.GetMember(member.GetSetMethod(true));
    }

    private MethodMember GetMember(MethodInfo method)
    {
      return method == null ? (MethodMember) null : (MethodMember) MemberExtensions.ToMember(method);
    }

    public override void SetValue(object target, object value)
    {
      this.member.SetValue(target, value, (object[]) null);
    }

    public override object GetValue(object target) => this.member.GetValue(target, (object[]) null);

    public override bool TryGetBackingField(out Member field)
    {
      if (this.backingField != (Member) null)
      {
        field = this.backingField;
        return true;
      }
      FieldInfo member = (this.DeclaringType.GetField(this.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? this.DeclaringType.GetField("_" + this.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) ?? this.DeclaringType.GetField("m_" + this.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (member == null)
      {
        field = (Member) null;
        return false;
      }
      field = this.backingField = (Member) new FieldMember(member);
      return true;
    }

    public override string Name => this.member.Name;

    public override Type PropertyType => this.member.PropertyType;

    public override bool CanWrite
    {
      get
      {
        return (!this.IsAutoProperty || !((Member) this.setMethod == (Member) null) && !this.setMethod.IsPrivate) && this.member.CanWrite;
      }
    }

    public override MemberInfo MemberInfo => (MemberInfo) this.member;

    public override Type DeclaringType => this.member.DeclaringType;

    public override bool HasIndexParameters => this.member.GetIndexParameters().Length > 0;

    public override bool IsMethod => false;

    public override bool IsField => false;

    public override bool IsProperty => true;

    public override bool IsAutoProperty
    {
      get
      {
        if ((Member) this.getMethod != (Member) null && this.getMethod.IsCompilerGenerated)
          return true;
        return (Member) this.setMethod != (Member) null && this.setMethod.IsCompilerGenerated;
      }
    }

    public override bool IsPrivate => this.getMethod.IsPrivate;

    public override bool IsProtected => this.getMethod.IsProtected;

    public override bool IsPublic => this.getMethod.IsPublic;

    public override bool IsInternal => this.getMethod.IsInternal;

    public MethodMember Get => this.getMethod;

    public MethodMember Set => this.setMethod;

    public override string ToString() => "{Property: " + this.member.Name + "}";
  }
}
