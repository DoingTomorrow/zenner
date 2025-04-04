// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoEnumAttribute
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;

#nullable disable
namespace ProtoBuf
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public sealed class ProtoEnumAttribute : Attribute
  {
    private bool hasValue;
    private int enumValue;
    private string name;

    public int Value
    {
      get => this.enumValue;
      set
      {
        this.enumValue = value;
        this.hasValue = true;
      }
    }

    public bool HasValue() => this.hasValue;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }
  }
}
