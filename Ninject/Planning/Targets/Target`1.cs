// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Targets.Target`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Targets
{
  public abstract class Target<T> : ITarget, ICustomAttributeProvider where T : ICustomAttributeProvider
  {
    private readonly Future<Func<IBindingMetadata, bool>> _constraint;
    private readonly Future<bool> _isOptional;

    public MemberInfo Member { get; private set; }

    public T Site { get; private set; }

    public abstract string Name { get; }

    public abstract Type Type { get; }

    public Func<IBindingMetadata, bool> Constraint
    {
      get => (Func<IBindingMetadata, bool>) this._constraint;
    }

    public bool IsOptional => (bool) this._isOptional;

    public virtual bool HasDefaultValue => false;

    public virtual object DefaultValue
    {
      get
      {
        throw new InvalidOperationException(ExceptionFormatter.TargetDoesNotHaveADefaultValue((ITarget) this));
      }
    }

    protected Target(MemberInfo member, T site)
    {
      Ensure.ArgumentNotNull((object) member, nameof (member));
      Ensure.ArgumentNotNull((object) site, nameof (site));
      this.Member = member;
      this.Site = site;
      this._constraint = new Future<Func<IBindingMetadata, bool>>(new Func<Func<IBindingMetadata, bool>>(this.ReadConstraintFromTarget));
      this._isOptional = new Future<bool>(new Func<bool>(this.ReadOptionalFromTarget));
    }

    public object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      Ensure.ArgumentNotNull((object) attributeType, nameof (attributeType));
      return this.Site.GetCustomAttributesExtended(attributeType, inherit);
    }

    public object[] GetCustomAttributes(bool inherit) => this.Site.GetCustomAttributes(inherit);

    public bool IsDefined(Type attributeType, bool inherit)
    {
      Ensure.ArgumentNotNull((object) attributeType, nameof (attributeType));
      return this.Site.IsDefined(attributeType, inherit);
    }

    public object ResolveWithin(IContext parent)
    {
      Ensure.ArgumentNotNull((object) parent, nameof (parent));
      if (this.Type.IsArray)
      {
        Type elementType = this.Type.GetElementType();
        return (object) this.GetValues(elementType, parent).CastSlow(elementType).ToArraySlow(elementType);
      }
      if (this.Type.IsGenericType)
      {
        Type genericTypeDefinition = this.Type.GetGenericTypeDefinition();
        Type genericArgument = this.Type.GetGenericArguments()[0];
        if (genericTypeDefinition == typeof (List<>) || genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (ICollection<>))
          return (object) this.GetValues(genericArgument, parent).CastSlow(genericArgument).ToListSlow(genericArgument);
        if (genericTypeDefinition == typeof (IEnumerable<>))
          return (object) this.GetValues(genericArgument, parent).CastSlow(genericArgument);
      }
      return this.GetValue(this.Type, parent);
    }

    protected virtual IEnumerable<object> GetValues(Type service, IContext parent)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) parent, nameof (parent));
      IRequest child = parent.Request.CreateChild(service, parent, (ITarget) this);
      child.IsOptional = true;
      return parent.Kernel.Resolve(child);
    }

    protected virtual object GetValue(Type service, IContext parent)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) parent, nameof (parent));
      IRequest child = parent.Request.CreateChild(service, parent, (ITarget) this);
      child.IsUnique = true;
      return parent.Kernel.Resolve(child).SingleOrDefault<object>();
    }

    protected virtual bool ReadOptionalFromTarget()
    {
      return this.Site.HasAttribute(typeof (OptionalAttribute));
    }

    protected virtual Func<IBindingMetadata, bool> ReadConstraintFromTarget()
    {
      ConstraintAttribute[] attributes = this.GetCustomAttributes(typeof (ConstraintAttribute), true) as ConstraintAttribute[];
      if (attributes == null || attributes.Length == 0)
        return (Func<IBindingMetadata, bool>) null;
      return attributes.Length == 1 ? new Func<IBindingMetadata, bool>(attributes[0].Matches) : (Func<IBindingMetadata, bool>) (metadata => ((IEnumerable<ConstraintAttribute>) attributes).All<ConstraintAttribute>((Func<ConstraintAttribute, bool>) (attribute => attribute.Matches(metadata))));
    }
  }
}
