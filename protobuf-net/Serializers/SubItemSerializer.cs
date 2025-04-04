// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.SubItemSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class SubItemSerializer : IProtoTypeSerializer, IProtoSerializer
  {
    private readonly int key;
    private readonly Type type;
    private readonly ISerializerProxy proxy;
    private readonly bool recursionCheck;

    bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
    {
      return ((IProtoTypeSerializer) this.proxy.Serializer).HasCallbacks(callbackType);
    }

    bool IProtoTypeSerializer.CanCreateInstance()
    {
      return ((IProtoTypeSerializer) this.proxy.Serializer).CanCreateInstance();
    }

    void IProtoTypeSerializer.EmitCallback(
      CompilerContext ctx,
      Local valueFrom,
      TypeModel.CallbackType callbackType)
    {
      ((IProtoTypeSerializer) this.proxy.Serializer).EmitCallback(ctx, valueFrom, callbackType);
    }

    void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
    {
      ((IProtoTypeSerializer) this.proxy.Serializer).EmitCreateInstance(ctx);
    }

    void IProtoTypeSerializer.Callback(
      object value,
      TypeModel.CallbackType callbackType,
      SerializationContext context)
    {
      ((IProtoTypeSerializer) this.proxy.Serializer).Callback(value, callbackType, context);
    }

    object IProtoTypeSerializer.CreateInstance(ProtoReader source)
    {
      return ((IProtoTypeSerializer) this.proxy.Serializer).CreateInstance(source);
    }

    public SubItemSerializer(Type type, int key, ISerializerProxy proxy, bool recursionCheck)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (proxy == null)
        throw new ArgumentNullException(nameof (proxy));
      this.type = type;
      this.proxy = proxy;
      this.key = key;
      this.recursionCheck = recursionCheck;
    }

    Type IProtoSerializer.ExpectedType => this.type;

    bool IProtoSerializer.RequiresOldValue => true;

    bool IProtoSerializer.ReturnsValue => true;

    void IProtoSerializer.Write(object value, ProtoWriter dest)
    {
      if (this.recursionCheck)
        ProtoWriter.WriteObject(value, this.key, dest);
      else
        ProtoWriter.WriteRecursionSafeObject(value, this.key, dest);
    }

    object IProtoSerializer.Read(object value, ProtoReader source)
    {
      return ProtoReader.ReadObject(value, this.key, source);
    }

    private bool EmitDedicatedMethod(CompilerContext ctx, Local valueFrom, bool read)
    {
      MethodBuilder dedicatedMethod = ctx.GetDedicatedMethod(this.key, read);
      if ((MethodInfo) dedicatedMethod == (MethodInfo) null)
        return false;
      using (Local local = new Local(ctx, ctx.MapType(typeof (SubItemToken))))
      {
        Type declaringType1 = ctx.MapType(read ? typeof (ProtoReader) : typeof (ProtoWriter));
        ctx.LoadValue(valueFrom);
        if (!read)
        {
          if (Helpers.IsValueType(this.type) || !this.recursionCheck)
            ctx.LoadNullRef();
          else
            ctx.CopyValue();
        }
        ctx.LoadReaderWriter();
        CompilerContext compilerContext = ctx;
        Type declaringType2 = declaringType1;
        Type[] parameterTypes;
        if (!read)
          parameterTypes = new Type[2]
          {
            ctx.MapType(typeof (object)),
            declaringType1
          };
        else
          parameterTypes = new Type[1]{ declaringType1 };
        MethodInfo staticMethod = Helpers.GetStaticMethod(declaringType2, "StartSubItem", parameterTypes);
        compilerContext.EmitCall(staticMethod);
        ctx.StoreValue(local);
        ctx.LoadReaderWriter();
        ctx.EmitCall((MethodInfo) dedicatedMethod);
        if (read && this.type != dedicatedMethod.ReturnType)
          ctx.Cast(this.type);
        ctx.LoadValue(local);
        ctx.LoadReaderWriter();
        ctx.EmitCall(Helpers.GetStaticMethod(declaringType1, "EndSubItem", new Type[2]
        {
          ctx.MapType(typeof (SubItemToken)),
          declaringType1
        }));
      }
      return true;
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      if (this.EmitDedicatedMethod(ctx, valueFrom, false))
        return;
      ctx.LoadValue(valueFrom);
      if (Helpers.IsValueType(this.type))
        ctx.CastToObject(this.type);
      ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(this.key));
      ctx.LoadReaderWriter();
      ctx.EmitCall(Helpers.GetStaticMethod(ctx.MapType(typeof (ProtoWriter)), this.recursionCheck ? "WriteObject" : "WriteRecursionSafeObject", new Type[3]
      {
        ctx.MapType(typeof (object)),
        ctx.MapType(typeof (int)),
        ctx.MapType(typeof (ProtoWriter))
      }));
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.EmitDedicatedMethod(ctx, valueFrom, true))
        return;
      ctx.LoadValue(valueFrom);
      if (Helpers.IsValueType(this.type))
        ctx.CastToObject(this.type);
      ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(this.key));
      ctx.LoadReaderWriter();
      ctx.EmitCall(Helpers.GetStaticMethod(ctx.MapType(typeof (ProtoReader)), "ReadObject"));
      ctx.CastFromObject(this.type);
    }
  }
}
