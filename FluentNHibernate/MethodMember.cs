// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MethodMember
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  internal class MethodMember : Member
  {
    private readonly MethodInfo member;
    private Member backingField;

    public override void SetValue(object target, object value)
    {
      throw new NotSupportedException("Cannot set the value of a method Member.");
    }

    public override object GetValue(object target) => this.member.Invoke(target, (object[]) null);

    public override bool TryGetBackingField(out Member field)
    {
      if (this.backingField != (Member) null)
      {
        field = this.backingField;
        return true;
      }
      string name = this.Name;
      if (name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
        name = name.Substring(3);
      FieldInfo member = (this.DeclaringType.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? this.DeclaringType.GetField("_" + name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) ?? this.DeclaringType.GetField("m_" + name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (member == null)
      {
        field = (Member) null;
        return false;
      }
      field = this.backingField = (Member) new FieldMember(member);
      return true;
    }

    public MethodMember(MethodInfo member) => this.member = member;

    public override string Name => this.member.Name;

    public override Type PropertyType => this.member.ReturnType;

    public override bool CanWrite => false;

    public override MemberInfo MemberInfo => (MemberInfo) this.member;

    public override Type DeclaringType => this.member.DeclaringType;

    public override bool HasIndexParameters => false;

    public override bool IsMethod => true;

    public override bool IsField => false;

    public override bool IsProperty => false;

    public override bool IsAutoProperty => false;

    public override bool IsPrivate => this.member.IsPrivate;

    public override bool IsProtected => this.member.IsFamily || this.member.IsFamilyAndAssembly;

    public override bool IsPublic => this.member.IsPublic;

    public override bool IsInternal => this.member.IsAssembly || this.member.IsFamilyAndAssembly;

    public bool IsCompilerGenerated
    {
      get
      {
        return ((IEnumerable<object>) this.member.GetCustomAttributes(typeof (CompilerGeneratedAttribute), true)).Any<object>();
      }
    }

    public override string ToString() => "{Method: " + this.member.Name + "}";
  }
}
