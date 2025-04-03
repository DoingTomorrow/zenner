// Decompiled with JetBrains decompiler
// Type: AutoMapper.SourceMemberConfig
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class SourceMemberConfig
  {
    private bool _ignored;

    public SourceMemberConfig(MemberInfo sourceMember) => this.SourceMember = sourceMember;

    public MemberInfo SourceMember { get; private set; }

    public void Ignore() => this._ignored = true;

    public bool IsIgnored() => this._ignored;
  }
}
