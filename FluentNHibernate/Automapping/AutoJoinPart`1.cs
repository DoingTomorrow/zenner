// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoJoinPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoJoinPart<T> : JoinPart<T>
  {
    private readonly IList<Member> mappedMembers;

    public AutoJoinPart(IList<Member> mappedMembers, string tableName)
      : base(tableName)
    {
      this.mappedMembers = mappedMembers;
    }

    internal override void OnMemberMapped(Member member) => this.mappedMembers.Add(member);
  }
}
