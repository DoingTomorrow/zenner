// Decompiled with JetBrains decompiler
// Type: ProtoBuf.MemberSerializationOptions
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;

#nullable disable
namespace ProtoBuf
{
  [Flags]
  public enum MemberSerializationOptions
  {
    None = 0,
    Packed = 1,
    Required = 2,
    AsReference = 4,
    DynamicType = 8,
    OverwriteList = 16, // 0x00000010
    AsReferenceHasValue = 32, // 0x00000020
  }
}
