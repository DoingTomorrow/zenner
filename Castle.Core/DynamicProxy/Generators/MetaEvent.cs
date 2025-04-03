// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MetaEvent
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class MetaEvent : MetaTypeElement, IEquatable<MetaEvent>
  {
    private string name;
    private readonly Type type;
    private EventEmitter emitter;
    private readonly MetaMethod adder;
    private readonly MetaMethod remover;

    public bool Equals(MetaEvent other)
    {
      return !object.ReferenceEquals((object) null, (object) other) && (object.ReferenceEquals((object) this, (object) other) || this.type.Equals(other.type) && StringComparer.OrdinalIgnoreCase.Equals(this.name, other.name));
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (MetaEvent) && this.Equals((MetaEvent) obj);
    }

    public override int GetHashCode()
    {
      return ((this.adder.Method != null ? this.adder.Method.GetHashCode() : 0) * 397 ^ (this.remover.Method != null ? this.remover.Method.GetHashCode() : 0)) * 397 ^ this.Attributes.GetHashCode();
    }

    public MetaEvent(
      string name,
      Type declaringType,
      Type eventDelegateType,
      MetaMethod adder,
      MetaMethod remover,
      EventAttributes attributes)
      : base(declaringType)
    {
      if (adder == null)
        throw new ArgumentNullException(nameof (adder));
      if (remover == null)
        throw new ArgumentNullException(nameof (remover));
      this.name = name;
      this.type = eventDelegateType;
      this.adder = adder;
      this.remover = remover;
      this.Attributes = attributes;
    }

    public EventAttributes Attributes { get; private set; }

    public EventEmitter Emitter
    {
      get
      {
        return this.emitter != null ? this.emitter : throw new InvalidOperationException("Emitter is not initialized. You have to initialize it first using 'BuildEventEmitter' method");
      }
    }

    public MetaMethod Adder => this.adder;

    public MetaMethod Remover => this.remover;

    public void BuildEventEmitter(ClassEmitter classEmitter)
    {
      if (this.emitter != null)
        throw new InvalidOperationException();
      this.emitter = classEmitter.CreateEvent(this.name, this.Attributes, this.type);
    }

    internal override void SwitchToExplicitImplementation()
    {
      this.name = string.Format("{0}.{1}", (object) this.sourceType.Name, (object) this.name);
      this.adder.SwitchToExplicitImplementation();
      this.remover.SwitchToExplicitImplementation();
    }
  }
}
