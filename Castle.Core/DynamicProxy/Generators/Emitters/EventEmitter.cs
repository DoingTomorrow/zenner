// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.EventEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public class EventEmitter : IMemberEmitter
  {
    private readonly AbstractTypeEmitter typeEmitter;
    private readonly Type type;
    private readonly EventBuilder eventBuilder;
    private MethodEmitter addMethod;
    private MethodEmitter removeMethod;

    public EventEmitter(
      AbstractTypeEmitter typeEmitter,
      string name,
      EventAttributes attributes,
      Type type)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      this.typeEmitter = typeEmitter;
      this.type = type;
      this.eventBuilder = typeEmitter.TypeBuilder.DefineEvent(name, attributes, type);
    }

    public MemberInfo Member => (MemberInfo) null;

    public Type ReturnType => this.type;

    public void Generate()
    {
      if (this.addMethod == null)
        throw new InvalidOperationException("Event add method was not created");
      if (this.removeMethod == null)
        throw new InvalidOperationException("Event remove method was not created");
      this.addMethod.Generate();
      this.eventBuilder.SetAddOnMethod(this.addMethod.MethodBuilder);
      this.removeMethod.Generate();
      this.eventBuilder.SetRemoveOnMethod(this.removeMethod.MethodBuilder);
    }

    public MethodEmitter CreateAddMethod(
      string addMethodName,
      MethodAttributes attributes,
      MethodInfo methodToOverride)
    {
      if (this.addMethod != null)
        throw new InvalidOperationException("An add method exists");
      this.addMethod = new MethodEmitter(this.typeEmitter, addMethodName, attributes, methodToOverride);
      return this.addMethod;
    }

    public void EnsureValidCodeBlock()
    {
      this.addMethod.EnsureValidCodeBlock();
      this.removeMethod.EnsureValidCodeBlock();
    }

    public MethodEmitter CreateRemoveMethod(
      string removeMethodName,
      MethodAttributes attributes,
      MethodInfo methodToOverride)
    {
      if (this.removeMethod != null)
        throw new InvalidOperationException("A remove method exists");
      this.removeMethod = new MethodEmitter(this.typeEmitter, removeMethodName, attributes, methodToOverride);
      return this.removeMethod;
    }
  }
}
