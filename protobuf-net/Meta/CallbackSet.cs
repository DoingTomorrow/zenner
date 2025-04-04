// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.CallbackSet
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf.Meta
{
  public class CallbackSet
  {
    private readonly MetaType metaType;
    private MethodInfo beforeSerialize;
    private MethodInfo afterSerialize;
    private MethodInfo beforeDeserialize;
    private MethodInfo afterDeserialize;

    internal CallbackSet(MetaType metaType)
    {
      this.metaType = metaType != null ? metaType : throw new ArgumentNullException(nameof (metaType));
    }

    internal MethodInfo this[TypeModel.CallbackType callbackType]
    {
      get
      {
        switch (callbackType)
        {
          case TypeModel.CallbackType.BeforeSerialize:
            return this.beforeSerialize;
          case TypeModel.CallbackType.AfterSerialize:
            return this.afterSerialize;
          case TypeModel.CallbackType.BeforeDeserialize:
            return this.beforeDeserialize;
          case TypeModel.CallbackType.AfterDeserialize:
            return this.afterDeserialize;
          default:
            throw new ArgumentException("Callback type not supported: " + callbackType.ToString(), nameof (callbackType));
        }
      }
    }

    internal static bool CheckCallbackParameters(TypeModel model, MethodInfo method)
    {
      foreach (ParameterInfo parameter in method.GetParameters())
      {
        Type parameterType = parameter.ParameterType;
        if (!(parameterType == model.MapType(typeof (SerializationContext))) && !(parameterType == model.MapType(typeof (Type))) && !(parameterType == model.MapType(typeof (StreamingContext))))
          return false;
      }
      return true;
    }

    private MethodInfo SanityCheckCallback(TypeModel model, MethodInfo callback)
    {
      this.metaType.ThrowIfFrozen();
      if (callback == (MethodInfo) null)
        return callback;
      if (callback.IsStatic)
        throw new ArgumentException("Callbacks cannot be static", nameof (callback));
      if (callback.ReturnType != model.MapType(typeof (void)) || !CallbackSet.CheckCallbackParameters(model, callback))
        throw CallbackSet.CreateInvalidCallbackSignature(callback);
      return callback;
    }

    internal static Exception CreateInvalidCallbackSignature(MethodInfo method)
    {
      return (Exception) new NotSupportedException("Invalid callback signature in " + method.DeclaringType.FullName + "." + method.Name);
    }

    public MethodInfo BeforeSerialize
    {
      get => this.beforeSerialize;
      set => this.beforeSerialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    public MethodInfo BeforeDeserialize
    {
      get => this.beforeDeserialize;
      set => this.beforeDeserialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    public MethodInfo AfterSerialize
    {
      get => this.afterSerialize;
      set => this.afterSerialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    public MethodInfo AfterDeserialize
    {
      get => this.afterDeserialize;
      set => this.afterDeserialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    public bool NonTrivial
    {
      get
      {
        return this.beforeSerialize != (MethodInfo) null || this.beforeDeserialize != (MethodInfo) null || this.afterSerialize != (MethodInfo) null || this.afterDeserialize != (MethodInfo) null;
      }
    }
  }
}
