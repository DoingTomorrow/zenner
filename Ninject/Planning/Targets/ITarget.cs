// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Targets.ITarget
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Planning.Bindings;
using System;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Targets
{
  public interface ITarget : ICustomAttributeProvider
  {
    Type Type { get; }

    string Name { get; }

    MemberInfo Member { get; }

    Func<IBindingMetadata, bool> Constraint { get; }

    bool IsOptional { get; }

    bool HasDefaultValue { get; }

    object DefaultValue { get; }

    object ResolveWithin(IContext parent);
  }
}
