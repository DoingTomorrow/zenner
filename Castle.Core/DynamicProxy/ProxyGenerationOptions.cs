// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.ProxyGenerationOptions
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy
{
  [Serializable]
  public class ProxyGenerationOptions : ISerializable
  {
    public static readonly ProxyGenerationOptions Default = new ProxyGenerationOptions();
    private List<object> mixins;
    internal readonly IList<Attribute> attributesToAddToGeneratedTypes = (IList<Attribute>) new List<Attribute>();
    private readonly IList<CustomAttributeBuilder> additionalAttributes = (IList<CustomAttributeBuilder>) new List<CustomAttributeBuilder>();
    [NonSerialized]
    private MixinData mixinData;

    public ProxyGenerationOptions(IProxyGenerationHook hook)
    {
      this.BaseTypeForInterfaceProxy = typeof (object);
      this.Hook = hook;
    }

    public ProxyGenerationOptions()
      : this((IProxyGenerationHook) new AllMethodsHook())
    {
    }

    private ProxyGenerationOptions(SerializationInfo info, StreamingContext context)
    {
      this.Hook = (IProxyGenerationHook) info.GetValue("hook", typeof (IProxyGenerationHook));
      this.Selector = (IInterceptorSelector) info.GetValue("selector", typeof (IInterceptorSelector));
      this.mixins = (List<object>) info.GetValue(nameof (mixins), typeof (List<object>));
      this.BaseTypeForInterfaceProxy = Type.GetType(info.GetString("baseTypeForInterfaceProxy.AssemblyQualifiedName"));
    }

    public void Initialize()
    {
      if (this.mixinData != null)
        return;
      try
      {
        this.mixinData = new MixinData((IEnumerable<object>) this.mixins);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidMixinConfigurationException("There is a problem with the mixins added to this ProxyGenerationOptions: " + ex.Message, (Exception) ex);
      }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("hook", (object) this.Hook);
      info.AddValue("selector", (object) this.Selector);
      info.AddValue("mixins", (object) this.mixins);
      info.AddValue("baseTypeForInterfaceProxy.AssemblyQualifiedName", (object) this.BaseTypeForInterfaceProxy.AssemblyQualifiedName);
    }

    public IProxyGenerationHook Hook { get; set; }

    public IInterceptorSelector Selector { get; set; }

    public Type BaseTypeForInterfaceProxy { get; set; }

    [Obsolete("This property is obsolete and will be removed in future versions. Use AdditionalAttributes property instead. You can use AttributeUtil class to simplify creating CustomAttributeBuilder instances for common cases.")]
    public IList<Attribute> AttributesToAddToGeneratedTypes => this.attributesToAddToGeneratedTypes;

    public IList<CustomAttributeBuilder> AdditionalAttributes => this.additionalAttributes;

    public MixinData MixinData
    {
      get
      {
        return this.mixinData != null ? this.mixinData : throw new InvalidOperationException("Call Initialize before accessing the MixinData property.");
      }
    }

    public void AddMixinInstance(object instance)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (this.mixins == null)
        this.mixins = new List<object>();
      this.mixins.Add(instance);
      this.mixinData = (MixinData) null;
    }

    public object[] MixinsAsArray() => this.mixins == null ? new object[0] : this.mixins.ToArray();

    public bool HasMixins => this.mixins != null && this.mixins.Count != 0;

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) this, obj))
        return true;
      ProxyGenerationOptions objA = obj as ProxyGenerationOptions;
      if (object.ReferenceEquals((object) objA, (object) null))
        return false;
      this.Initialize();
      objA.Initialize();
      return object.Equals((object) this.Hook, (object) objA.Hook) && object.Equals((object) (this.Selector == null), (object) (objA.Selector == null)) && object.Equals((object) this.MixinData, (object) objA.MixinData) && object.Equals((object) this.BaseTypeForInterfaceProxy, (object) objA.BaseTypeForInterfaceProxy);
    }

    public override int GetHashCode()
    {
      this.Initialize();
      return 29 * (29 * (29 * (this.Hook != null ? this.Hook.GetType().GetHashCode() : 0) + (this.Selector != null ? 1 : 0)) + this.MixinData.GetHashCode()) + (this.BaseTypeForInterfaceProxy != null ? this.BaseTypeForInterfaceProxy.GetHashCode() : 0);
    }
  }
}
