// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.IProtoTypeSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal interface IProtoTypeSerializer : IProtoSerializer
  {
    bool HasCallbacks(TypeModel.CallbackType callbackType);

    bool CanCreateInstance();

    object CreateInstance(ProtoReader source);

    void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context);

    void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType);

    void EmitCreateInstance(CompilerContext ctx);
  }
}
