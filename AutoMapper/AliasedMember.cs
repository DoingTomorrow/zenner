// Decompiled with JetBrains decompiler
// Type: AutoMapper.AliasedMember
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

#nullable disable
namespace AutoMapper
{
  public class AliasedMember
  {
    public AliasedMember(string member, string alias)
    {
      this.Member = member;
      this.Alias = alias;
    }

    public string Member { get; private set; }

    public string Alias { get; private set; }

    public bool Equals(AliasedMember other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.Member, (object) this.Member) && object.Equals((object) other.Alias, (object) this.Alias);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (AliasedMember) && this.Equals((AliasedMember) obj);
    }

    public override int GetHashCode() => this.Member.GetHashCode() * 397 ^ this.Alias.GetHashCode();
  }
}
