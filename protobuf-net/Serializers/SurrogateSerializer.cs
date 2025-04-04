// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.SurrogateSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class SurrogateSerializer : IProtoTypeSerializer, IProtoSerializer
  {
    private readonly Type forType;
    private readonly Type declaredType;
    private readonly MethodInfo toTail;
    private readonly MethodInfo fromTail;
    private IProtoTypeSerializer rootTail;

    bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType) => false;

    void IProtoTypeSerializer.EmitCallback(
      CompilerContext ctx,
      Local valueFrom,
      TypeModel.CallbackType callbackType)
    {
    }

    void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
    {
      throw new NotSupportedException();
    }

    bool IProtoTypeSerializer.CanCreateInstance() => false;

    object IProtoTypeSerializer.CreateInstance(ProtoReader source)
    {
      throw new NotSupportedException();
    }

    void IProtoTypeSerializer.Callback(
      object value,
      TypeModel.CallbackType callbackType,
      SerializationContext context)
    {
    }

    public bool ReturnsValue => false;

    public bool RequiresOldValue => true;

    public Type ExpectedType => this.forType;

    public SurrogateSerializer(
      TypeModel model,
      Type forType,
      Type declaredType,
      IProtoTypeSerializer rootTail)
    {
      this.forType = forType;
      this.declaredType = declaredType;
      this.rootTail = rootTail;
      this.toTail = this.GetConversion(model, true);
      this.fromTail = this.GetConversion(model, false);
    }

    private static bool HasCast(
      TypeModel model,
      Type type,
      Type from,
      Type to,
      out MethodInfo op)
    {
      MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      Type attributeType = (Type) null;
      for (int index = 0; index < methods.Length; ++index)
      {
        MethodInfo methodInfo = methods[index];
        if (!(methodInfo.ReturnType != to))
        {
          ParameterInfo[] parameters = methodInfo.GetParameters();
          if (parameters.Length == 1 && parameters[0].ParameterType == from)
          {
            if (attributeType == (Type) null)
            {
              attributeType = model.MapType(typeof (ProtoConverterAttribute), false);
              if (attributeType == (Type) null)
                break;
            }
            if (methodInfo.IsDefined(attributeType, true))
            {
              op = methodInfo;
              return true;
            }
          }
        }
      }
      for (int index = 0; index < methods.Length; ++index)
      {
        MethodInfo methodInfo = methods[index];
        if ((!(methodInfo.Name != "op_Implicit") || !(methodInfo.Name != "op_Explicit")) && !(methodInfo.ReturnType != to))
        {
          ParameterInfo[] parameters = methodInfo.GetParameters();
          if (parameters.Length == 1 && parameters[0].ParameterType == from)
          {
            op = methodInfo;
            return true;
          }
        }
      }
      op = (MethodInfo) null;
      return false;
    }

    public MethodInfo GetConversion(TypeModel model, bool toTail)
    {
      Type to = toTail ? this.declaredType : this.forType;
      Type from = toTail ? this.forType : this.declaredType;
      MethodInfo op;
      if (SurrogateSerializer.HasCast(model, this.declaredType, from, to, out op) || SurrogateSerializer.HasCast(model, this.forType, from, to, out op))
        return op;
      throw new InvalidOperationException("No suitable conversion operator found for surrogate: " + this.forType.FullName + " / " + this.declaredType.FullName);
    }

    public void Write(object value, ProtoWriter writer)
    {
      this.rootTail.Write(this.toTail.Invoke((object) null, new object[1]
      {
        value
      }), writer);
    }

    public object Read(object value, ProtoReader source)
    {
      object[] parameters = new object[1]{ value };
      value = this.toTail.Invoke((object) null, parameters);
      parameters[0] = this.rootTail.Read(value, source);
      return this.fromTail.Invoke((object) null, parameters);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      using (Local local = new Local(ctx, this.declaredType))
      {
        ctx.LoadValue(valueFrom);
        ctx.EmitCall(this.toTail);
        ctx.StoreValue(local);
        this.rootTail.EmitRead(ctx, local);
        ctx.LoadValue(local);
        ctx.EmitCall(this.fromTail);
        ctx.StoreValue(valueFrom);
      }
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadValue(valueFrom);
      ctx.EmitCall(this.toTail);
      this.rootTail.EmitWrite(ctx, (Local) null);
    }
  }
}
