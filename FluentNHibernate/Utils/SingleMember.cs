// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.SingleMember
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Utils
{
  public class SingleMember : Accessor
  {
    private readonly Member member;

    public SingleMember(Member member) => this.member = member;

    public string FieldName => this.member.Name;

    public Type PropertyType => this.member.PropertyType;

    public Member InnerMember => this.member;

    public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression)
    {
      return (Accessor) new PropertyChain(new Member[2]
      {
        this.member,
        expression.ToMember<T, object>()
      });
    }

    public string Name => this.member.Name;

    public void SetValue(object target, object propertyValue)
    {
      this.member.SetValue(target, propertyValue);
    }

    public object GetValue(object target) => this.member.GetValue(target);

    public static SingleMember Build<T>(Expression<Func<T, object>> expression)
    {
      return new SingleMember(expression.ToMember<T, object>());
    }

    public static SingleMember Build<T>(string propertyName)
    {
      return new SingleMember(MemberExtensions.ToMember(typeof (T).GetProperty(propertyName)));
    }
  }
}
