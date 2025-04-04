// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MemberEqualityComparer
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate
{
  public class MemberEqualityComparer : IEqualityComparer<Member>
  {
    public bool Equals(Member x, Member y)
    {
      return x.MemberInfo.MetadataToken.Equals(y.MemberInfo.MetadataToken) && x.MemberInfo.Module.Equals((object) y.MemberInfo.Module);
    }

    public int GetHashCode(Member obj)
    {
      return obj.MemberInfo.MetadataToken.GetHashCode() & obj.MemberInfo.Module.GetHashCode();
    }
  }
}
