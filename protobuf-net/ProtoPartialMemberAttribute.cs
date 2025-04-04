// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoPartialMemberAttribute
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;

#nullable disable
namespace ProtoBuf
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public sealed class ProtoPartialMemberAttribute : ProtoMemberAttribute
  {
    private readonly string memberName;

    public ProtoPartialMemberAttribute(int tag, string memberName)
      : base(tag)
    {
      this.memberName = !Helpers.IsNullOrEmpty(memberName) ? memberName : throw new ArgumentNullException(nameof (memberName));
    }

    public string MemberName => this.memberName;
  }
}
