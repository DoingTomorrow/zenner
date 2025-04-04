// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoAfterSerializationAttribute
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.ComponentModel;

#nullable disable
namespace ProtoBuf
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  [ImmutableObject(true)]
  public sealed class ProtoAfterSerializationAttribute : Attribute
  {
  }
}
