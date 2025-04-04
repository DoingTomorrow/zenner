// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.FieldMember
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  internal class FieldMember : Member
  {
    private readonly FieldInfo member;

    public override void SetValue(object target, object value)
    {
      this.member.SetValue(target, value);
    }

    public override object GetValue(object target) => this.member.GetValue(target);

    public override bool TryGetBackingField(out Member backingField)
    {
      backingField = (Member) null;
      return false;
    }

    public FieldMember(FieldInfo member) => this.member = member;

    public override string Name => this.member.Name;

    public override Type PropertyType => this.member.FieldType;

    public override bool CanWrite => true;

    public override MemberInfo MemberInfo => (MemberInfo) this.member;

    public override Type DeclaringType => this.member.DeclaringType;

    public override bool HasIndexParameters => false;

    public override bool IsMethod => false;

    public override bool IsField => true;

    public override bool IsProperty => false;

    public override bool IsAutoProperty => false;

    public override bool IsPrivate => this.member.IsPrivate;

    public override bool IsProtected => this.member.IsFamily || this.member.IsFamilyAndAssembly;

    public override bool IsPublic => this.member.IsPublic;

    public override bool IsInternal => this.member.IsAssembly || this.member.IsFamilyAndAssembly;

    public override string ToString() => "{Field: " + this.member.Name + "}";
  }
}
