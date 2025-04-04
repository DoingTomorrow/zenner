// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.NetObjectSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class NetObjectSerializer : IProtoSerializer
  {
    private readonly int key;
    private readonly Type type;
    private readonly BclHelpers.NetObjectOptions options;

    public NetObjectSerializer(
      TypeModel model,
      Type type,
      int key,
      BclHelpers.NetObjectOptions options)
    {
      bool flag = (options & BclHelpers.NetObjectOptions.DynamicType) != 0;
      this.key = flag ? -1 : key;
      this.type = flag ? model.MapType(typeof (object)) : type;
      this.options = options;
    }

    public Type ExpectedType => this.type;

    public bool ReturnsValue => true;

    public bool RequiresOldValue => true;

    public object Read(object value, ProtoReader source)
    {
      return BclHelpers.ReadNetObject(value, source, this.key, this.type == typeof (object) ? (Type) null : this.type, this.options);
    }

    public void Write(object value, ProtoWriter dest)
    {
      BclHelpers.WriteNetObject(value, dest, this.key, this.options);
    }

    public void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadValue(valueFrom);
      ctx.CastToObject(this.type);
      ctx.LoadReaderWriter();
      ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(this.key));
      if (this.type == ctx.MapType(typeof (object)))
        ctx.LoadNullRef();
      else
        ctx.LoadValue(this.type);
      ctx.LoadValue((int) this.options);
      ctx.EmitCall(ctx.MapType(typeof (BclHelpers)).GetMethod("ReadNetObject"));
      ctx.CastFromObject(this.type);
    }

    public void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadValue(valueFrom);
      ctx.CastToObject(this.type);
      ctx.LoadReaderWriter();
      ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(this.key));
      ctx.LoadValue((int) this.options);
      ctx.EmitCall(ctx.MapType(typeof (BclHelpers)).GetMethod("WriteNetObject"));
    }
  }
}
