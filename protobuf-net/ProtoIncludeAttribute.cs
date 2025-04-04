// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoIncludeAttribute
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using System;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace ProtoBuf
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
  public sealed class ProtoIncludeAttribute : Attribute
  {
    private readonly int tag;
    private readonly string knownTypeName;
    private DataFormat dataFormat;

    public ProtoIncludeAttribute(int tag, Type knownType)
      : this(tag, knownType == (Type) null ? "" : knownType.AssemblyQualifiedName)
    {
    }

    public ProtoIncludeAttribute(int tag, string knownTypeName)
    {
      if (tag <= 0)
        throw new ArgumentOutOfRangeException(nameof (tag), "Tags must be positive integers");
      if (Helpers.IsNullOrEmpty(knownTypeName))
        throw new ArgumentNullException(nameof (knownTypeName), "Known type cannot be blank");
      this.tag = tag;
      this.knownTypeName = knownTypeName;
    }

    public int Tag => this.tag;

    public string KnownTypeName => this.knownTypeName;

    public Type KnownType
    {
      get => TypeModel.ResolveKnownType(this.KnownTypeName, (TypeModel) null, (Assembly) null);
    }

    [DefaultValue(DataFormat.Default)]
    public DataFormat DataFormat
    {
      get => this.dataFormat;
      set => this.dataFormat = value;
    }
  }
}
