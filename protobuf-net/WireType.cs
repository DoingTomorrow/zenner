// Decompiled with JetBrains decompiler
// Type: ProtoBuf.WireType
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

#nullable disable
namespace ProtoBuf
{
  public enum WireType
  {
    None = -1, // 0xFFFFFFFF
    Variant = 0,
    Fixed64 = 1,
    String = 2,
    StartGroup = 3,
    EndGroup = 4,
    Fixed32 = 5,
    SignedVariant = 8,
  }
}
