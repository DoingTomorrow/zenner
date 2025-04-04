// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoMemberAttribute
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class ProtoMemberAttribute : Attribute, IComparable, IComparable<ProtoMemberAttribute>
  {
    internal MemberInfo Member;
    internal bool TagIsPinned;
    private string name;
    private DataFormat dataFormat;
    private int tag;
    private MemberSerializationOptions options;

    public int CompareTo(object other) => this.CompareTo(other as ProtoMemberAttribute);

    public int CompareTo(ProtoMemberAttribute other)
    {
      if (other == null)
        return -1;
      if (this == other)
        return 0;
      int num = this.tag.CompareTo(other.tag);
      if (num == 0)
        num = string.CompareOrdinal(this.name, other.name);
      return num;
    }

    public ProtoMemberAttribute(int tag)
      : this(tag, false)
    {
    }

    internal ProtoMemberAttribute(int tag, bool forced)
    {
      this.tag = tag > 0 || forced ? tag : throw new ArgumentOutOfRangeException(nameof (tag));
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public DataFormat DataFormat
    {
      get => this.dataFormat;
      set => this.dataFormat = value;
    }

    public int Tag => this.tag;

    internal void Rebase(int tag) => this.tag = tag;

    public bool IsRequired
    {
      get
      {
        return (this.options & MemberSerializationOptions.Required) == MemberSerializationOptions.Required;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.Required;
        else
          this.options &= ~MemberSerializationOptions.Required;
      }
    }

    public bool IsPacked
    {
      get
      {
        return (this.options & MemberSerializationOptions.Packed) == MemberSerializationOptions.Packed;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.Packed;
        else
          this.options &= ~MemberSerializationOptions.Packed;
      }
    }

    public bool OverwriteList
    {
      get
      {
        return (this.options & MemberSerializationOptions.OverwriteList) == MemberSerializationOptions.OverwriteList;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.OverwriteList;
        else
          this.options &= ~MemberSerializationOptions.OverwriteList;
      }
    }

    public bool AsReference
    {
      get
      {
        return (this.options & MemberSerializationOptions.AsReference) == MemberSerializationOptions.AsReference;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.AsReference;
        else
          this.options &= ~MemberSerializationOptions.AsReference;
        this.options |= MemberSerializationOptions.AsReferenceHasValue;
      }
    }

    internal bool AsReferenceHasValue
    {
      get
      {
        return (this.options & MemberSerializationOptions.AsReferenceHasValue) == MemberSerializationOptions.AsReferenceHasValue;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.AsReferenceHasValue;
        else
          this.options &= ~MemberSerializationOptions.AsReferenceHasValue;
      }
    }

    public bool DynamicType
    {
      get
      {
        return (this.options & MemberSerializationOptions.DynamicType) == MemberSerializationOptions.DynamicType;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.DynamicType;
        else
          this.options &= ~MemberSerializationOptions.DynamicType;
      }
    }

    public MemberSerializationOptions Options
    {
      get => this.options;
      set => this.options = value;
    }
  }
}
