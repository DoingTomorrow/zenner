// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.SplitDefinition
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class SplitDefinition
  {
    public SplitDefinition(Type on, string groupId, MemberInfo member)
    {
      this.On = on;
      this.GroupId = groupId;
      this.Member = member;
    }

    public Type On { get; private set; }

    public string GroupId { get; private set; }

    public MemberInfo Member { get; private set; }
  }
}
