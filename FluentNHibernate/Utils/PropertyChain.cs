// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.PropertyChain
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Utils
{
  public class PropertyChain : Accessor
  {
    private readonly Member[] _chain;
    private readonly SingleMember innerMember;

    public PropertyChain(Member[] members)
    {
      this._chain = new Member[members.Length - 1];
      for (int index = 0; index < this._chain.Length; ++index)
        this._chain[index] = members[index];
      this.innerMember = new SingleMember(members[members.Length - 1]);
    }

    public void SetValue(object target, object propertyValue)
    {
      target = this.findInnerMostTarget(target);
      if (target == null)
        return;
      this.innerMember.SetValue(target, propertyValue);
    }

    public object GetValue(object target)
    {
      target = this.findInnerMostTarget(target);
      return target == null ? (object) null : this.innerMember.GetValue(target);
    }

    public string FieldName => this.innerMember.FieldName;

    public Type PropertyType => this.innerMember.PropertyType;

    public Member InnerMember => this.innerMember.InnerMember;

    public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression)
    {
      return (Accessor) new PropertyChain(new List<Member>((IEnumerable<Member>) this._chain)
      {
        this.innerMember.InnerMember,
        expression.ToMember<T, object>()
      }.ToArray());
    }

    public string Name
    {
      get
      {
        string str = string.Empty;
        foreach (Member member in this._chain)
          str = str + member.Name + ".";
        return str + this.innerMember.Name;
      }
    }

    private object findInnerMostTarget(object target)
    {
      foreach (Member member in this._chain)
      {
        target = member.GetValue(target);
        if (target == null)
          return (object) null;
      }
      return target;
    }
  }
}
