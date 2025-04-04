// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.CharSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class CharSerializer(TypeModel model) : UInt16Serializer(model)
  {
    private static readonly Type expectedType = typeof (char);

    public override Type ExpectedType => CharSerializer.expectedType;

    public override void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteUInt16((ushort) (char) value, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      return (object) (char) source.ReadUInt16();
    }
  }
}
