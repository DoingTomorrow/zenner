// Decompiled with JetBrains decompiler
// Type: Ninject.Parameters.Parameter
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Planning.Targets;
using System;

#nullable disable
namespace Ninject.Parameters
{
  public class Parameter : IParameter, IEquatable<IParameter>
  {
    public string Name { get; private set; }

    public bool ShouldInherit { get; private set; }

    public Func<IContext, ITarget, object> ValueCallback { get; private set; }

    public Parameter(string name, object value, bool shouldInherit)
      : this(name, (Func<IContext, ITarget, object>) ((ctx, target) => value), shouldInherit)
    {
    }

    public Parameter(string name, Func<IContext, object> valueCallback, bool shouldInherit)
    {
      Ensure.ArgumentNotNullOrEmpty(name, nameof (name));
      Ensure.ArgumentNotNull((object) valueCallback, nameof (valueCallback));
      this.Name = name;
      this.ValueCallback = (Func<IContext, ITarget, object>) ((ctx, target) => valueCallback(ctx));
      this.ShouldInherit = shouldInherit;
    }

    public Parameter(
      string name,
      Func<IContext, ITarget, object> valueCallback,
      bool shouldInherit)
    {
      Ensure.ArgumentNotNullOrEmpty(name, nameof (name));
      Ensure.ArgumentNotNull((object) valueCallback, nameof (valueCallback));
      this.Name = name;
      this.ValueCallback = valueCallback;
      this.ShouldInherit = shouldInherit;
    }

    public object GetValue(IContext context, ITarget target)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      return this.ValueCallback(context, target);
    }

    public override bool Equals(object obj)
    {
      return !(obj is IParameter other) ? base.Equals(obj) : this.Equals(other);
    }

    public override int GetHashCode() => this.GetType().GetHashCode() ^ this.Name.GetHashCode();

    public bool Equals(IParameter other)
    {
      return other.GetType() == this.GetType() && other.Name.Equals(this.Name);
    }
  }
}
