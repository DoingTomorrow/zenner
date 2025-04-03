// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MetaMethod
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  [DebuggerDisplay("{Method}")]
  public class MetaMethod : MetaTypeElement, IEquatable<MetaMethod>
  {
    private const MethodAttributes ExplicitImplementationAttributes = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask;
    private string name;

    public MetaMethod(
      MethodInfo method,
      MethodInfo methodOnTarget,
      bool standalone,
      bool proxyable,
      bool hasTarget)
      : base(method.DeclaringType)
    {
      this.Method = method;
      this.name = method.Name;
      this.MethodOnTarget = methodOnTarget;
      this.Standalone = standalone;
      this.Proxyable = proxyable;
      this.HasTarget = hasTarget;
      this.Attributes = this.ObtainAttributes();
    }

    private MethodAttributes ObtainAttributes()
    {
      MethodInfo method = this.Method;
      MethodAttributes attributes = MethodAttributes.Virtual;
      if (method.IsFinal || this.Method.DeclaringType.IsInterface)
        attributes |= MethodAttributes.VtableLayoutMask;
      if (method.IsPublic)
        attributes |= MethodAttributes.Public;
      if (method.IsHideBySig)
        attributes |= MethodAttributes.HideBySig;
      if (InternalsHelper.IsInternal(method) && InternalsHelper.IsInternalToDynamicProxy(method.DeclaringType.Assembly))
        attributes |= MethodAttributes.Assembly;
      if (method.IsFamilyAndAssembly)
        attributes |= MethodAttributes.FamANDAssem;
      else if (method.IsFamilyOrAssembly)
        attributes |= MethodAttributes.FamORAssem;
      else if (method.IsFamily)
        attributes |= MethodAttributes.Family;
      if (!this.Standalone)
        attributes |= MethodAttributes.SpecialName;
      return attributes;
    }

    public bool Proxyable { get; private set; }

    public MethodInfo MethodOnTarget { get; private set; }

    public bool Standalone { get; private set; }

    public MethodInfo Method { get; private set; }

    public bool HasTarget { get; private set; }

    public bool Equals(MetaMethod other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      if (!StringComparer.OrdinalIgnoreCase.Equals(this.name, other.name))
        return false;
      MethodSignatureComparer instance = MethodSignatureComparer.Instance;
      return instance.EqualSignatureTypes(this.Method.ReturnType, other.Method.ReturnType) && instance.EqualGenericParameters(this.Method, other.Method) && instance.EqualParameters(this.Method, other.Method);
    }

    internal override void SwitchToExplicitImplementation()
    {
      this.Attributes = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask;
      if (!this.Standalone)
        this.Attributes |= MethodAttributes.SpecialName;
      this.name = string.Format("{0}.{1}", (object) this.Method.DeclaringType.Name, (object) this.Method.Name);
    }

    public MethodAttributes Attributes { get; private set; }

    public string Name => this.name;
  }
}
