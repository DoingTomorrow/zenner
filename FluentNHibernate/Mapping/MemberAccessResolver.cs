// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.MemberAccessResolver
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Mapping
{
  public static class MemberAccessResolver
  {
    public static Access Resolve(Member member)
    {
      if (member.IsAutoProperty)
      {
        PropertyMember propertyMember = (PropertyMember) member;
        return (Member) propertyMember.Set != (Member) null && !propertyMember.Set.IsPrivate && !propertyMember.Set.IsInternal ? Access.Property : Access.BackField;
      }
      if (member.IsProperty)
      {
        PropertyMember propertyMember = (PropertyMember) member;
        Member backingField;
        return (Member) propertyMember.Set != (Member) null && !propertyMember.Set.IsPrivate && !propertyMember.Set.IsInternal || !propertyMember.TryGetBackingField(out backingField) ? Access.Property : Access.ReadOnlyPropertyWithField(Naming.Determine(backingField.Name));
      }
      return member.IsMethod ? (!member.TryGetBackingField(out Member _) ? Access.Property : Access.Field) : (member.IsField ? Access.Field : Access.Property);
    }
  }
}
