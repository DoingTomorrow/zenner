// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.BlobSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class BlobSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (byte[]);
    private readonly bool overwriteList;

    public Type ExpectedType => BlobSerializer.expectedType;

    public BlobSerializer(TypeModel model, bool overwriteList)
    {
      this.overwriteList = overwriteList;
    }

    public object Read(object value, ProtoReader source)
    {
      return (object) ProtoReader.AppendBytes(this.overwriteList ? (byte[]) null : (byte[]) value, source);
    }

    public void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteBytes((byte[]) value, dest);
    }

    bool IProtoSerializer.RequiresOldValue => !this.overwriteList;

    bool IProtoSerializer.ReturnsValue => true;

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteBytes", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.overwriteList)
        ctx.LoadNullRef();
      else
        ctx.LoadValue(valueFrom);
      ctx.LoadReaderWriter();
      ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("AppendBytes"));
    }
  }
}
