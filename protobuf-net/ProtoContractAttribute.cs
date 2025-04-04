// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoContractAttribute
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;

#nullable disable
namespace ProtoBuf
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
  public sealed class ProtoContractAttribute : Attribute
  {
    private string name;
    private int implicitFirstTag;
    private ImplicitFields implicitFields;
    private int dataMemberOffset;
    private byte flags;
    private const byte OPTIONS_InferTagFromName = 1;
    private const byte OPTIONS_InferTagFromNameHasValue = 2;
    private const byte OPTIONS_UseProtoMembersOnly = 4;
    private const byte OPTIONS_SkipConstructor = 8;
    private const byte OPTIONS_IgnoreListHandling = 16;
    private const byte OPTIONS_AsReferenceDefault = 32;
    private const byte OPTIONS_EnumPassthru = 64;
    private const byte OPTIONS_EnumPassthruHasValue = 128;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public int ImplicitFirstTag
    {
      get => this.implicitFirstTag;
      set
      {
        this.implicitFirstTag = value >= 1 ? value : throw new ArgumentOutOfRangeException(nameof (ImplicitFirstTag));
      }
    }

    public bool UseProtoMembersOnly
    {
      get => this.HasFlag((byte) 4);
      set => this.SetFlag((byte) 4, value);
    }

    public bool IgnoreListHandling
    {
      get => this.HasFlag((byte) 16);
      set => this.SetFlag((byte) 16, value);
    }

    public ImplicitFields ImplicitFields
    {
      get => this.implicitFields;
      set => this.implicitFields = value;
    }

    public bool InferTagFromName
    {
      get => this.HasFlag((byte) 1);
      set
      {
        this.SetFlag((byte) 1, value);
        this.SetFlag((byte) 2, true);
      }
    }

    internal bool InferTagFromNameHasValue => this.HasFlag((byte) 2);

    public int DataMemberOffset
    {
      get => this.dataMemberOffset;
      set => this.dataMemberOffset = value;
    }

    public bool SkipConstructor
    {
      get => this.HasFlag((byte) 8);
      set => this.SetFlag((byte) 8, value);
    }

    public bool AsReferenceDefault
    {
      get => this.HasFlag((byte) 32);
      set => this.SetFlag((byte) 32, value);
    }

    private bool HasFlag(byte flag) => ((int) this.flags & (int) flag) == (int) flag;

    private void SetFlag(byte flag, bool value)
    {
      if (value)
        this.flags |= flag;
      else
        this.flags &= ~flag;
    }

    public bool EnumPassthru
    {
      get => this.HasFlag((byte) 64);
      set
      {
        this.SetFlag((byte) 64, value);
        this.SetFlag((byte) 128, true);
      }
    }

    internal bool EnumPassthruHasValue => this.HasFlag((byte) 128);
  }
}
